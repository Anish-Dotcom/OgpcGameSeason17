using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class sellProperties
{
    public string toyType;
    public List<string> typesOfParts = new List<string>();
    public List<int> numOfType = new List<int>();
}

public class SellCalculation : MonoBehaviour
{
    public ComissionController comissionController;
    public ItemsInShop itemsInShop;
    public MoneyScrip moneyScript;

    public GameObject player;

    public float colorWeight;
    public float priceWeight;
    public float typeWeight;

    public List<Color> colorsUsed;
    public List<float> colorWeights;

    public List<string> partTypes;
    public List<int> numOfParts;
    public string[] toySubstrings;
    public int[] toyIndexes;
    public string[] toyPartSubstrings;
    public int toyType;

    public List<sellProperties> sellItemProperties = new List<sellProperties>();
    public void CalculatePrice(GameObject objToSell, int completingComission)
    {
        float price = 0f;
        colorsUsed.Clear();
        toyType = -1;
        for (int i = 0; i < objToSell.transform.childCount; i++)
        {
            GameObject currentObj = objToSell.transform.GetChild(i).gameObject;//the current object we are checking, component
            //calculating how much each color was used
            Color currentColor = currentObj.GetComponent<MaterialInfo>().currentMat.GetColor("_BaseColor");
            int index = -1;//is the color already in the list?
            for (int j = 0; j < colorsUsed.Count; j++)
            {
                if (currentColor == colorsUsed[j])
                {
                    index = j;
                }
            }
            if (index == -1)//add a new color
            {
                colorsUsed.Add(currentColor);
                colorWeights.Add(0);
                GetAllMaterialInstancesArea(currentObj.transform, currentObj.GetComponent<MaterialInfo>().currentMat, colorWeights.Count - 1);
            }
            else
            {
                GetAllMaterialInstancesArea(currentObj.transform, currentObj.GetComponent<MaterialInfo>().currentMat, index);
            }

            //check how much each item added costs
            for (int j = 0; j < currentObj.transform.childCount; j++)
            {
                GameObject currentPart = currentObj.transform.GetChild(j).gameObject;

                for (int k = 0; k < itemsInShop.properties.Count; k++)
                {
                    if (itemsInShop.properties[k].itemObject.name == currentPart.name)
                    {
                        price += itemsInShop.properties[k].itemPrice;
                        break;
                    }
                }
            }

            //Get type of toy
            bool toySubApplied = false;
            if (i == 0)
            {
                for (int j = 0; j < toySubstrings.Length; j++)
                {
                    if (objToSell.name.Contains(toySubstrings[j]))
                    {
                        toyType = toyIndexes[j];
                        toySubApplied = true;
                    }
                }
            }
            if (!toySubApplied)
            {
                for (int j = 0; j < toyPartSubstrings.Length; j++)//components
                {
                    if (objToSell.name.Contains(toySubstrings[j]))
                    {
                        for (int k = 0; k < partTypes.Count; k++)
                        {
                            if (toySubstrings[j] == partTypes[k])
                            {
                                numOfParts[k] += 1;
                                return;
                            }
                        }
                        partTypes.Add(toySubstrings[j]);
                        numOfParts.Add(1);
                    }
                }
            }
        }
        //calculate
        float totalPrice = 0f;
        int intDex = -1;
        for (int i = 0; i < colorsUsed.Count; i++)
        {
            float highestWeight = 0f;
            if (colorWeights[i] > highestWeight)
            {
                highestWeight = colorWeights[i];
                intDex = i;
            }
        }//finds the most used color based on calculated weights

        int current = 2 + (4 * (completingComission - 1));
        Vector3 goalColor = comissionController.colorRGBValues[current];
        Vector3 reformattedGoalColor = new Vector3(Mathf.Pow(goalColor.x, 2), Mathf.Pow(goalColor.y, 2), Mathf.Pow(goalColor.z, 2));
        Vector3 reformattedMostUsedColor = Vector3.zero;

        if (intDex != -1)
        {
            reformattedMostUsedColor = new Vector3(Mathf.Pow(colorsUsed[intDex].r, 2), Mathf.Pow(colorsUsed[intDex].g, 2), Mathf.Pow(colorsUsed[intDex].b, 2));
        }
        float colorDistance = Mathf.Sqrt(Vector3.Distance(reformattedGoalColor, reformattedMostUsedColor));
        totalPrice += colorWeight / colorDistance;

        totalPrice += price * priceWeight;

        //toy requirements
        current = 1 + (4 * (completingComission - 1));
        float toytype = 0.3f;
        if (toyType == -1)
        {
            int goaltype = comissionController.inputNums[current];
            List<bool> partsSatisfied = new List<bool>(sellItemProperties[goaltype].typesOfParts.Count);
            for (int i = 0; i < partTypes.Count; i++)//iterate through each type of part in the toy
            {
                for (int j = 0; j < sellItemProperties[goaltype].typesOfParts.Count; j++)
                {
                    if (partTypes[i].Contains(sellItemProperties[goaltype].typesOfParts[j]))//contains one of the goal parts
                    {
                        if (numOfParts[i] == sellItemProperties[goaltype].numOfType[j])
                        {
                            partsSatisfied[j] = true;
                        }
                    }
                }
            }
            bool allSatisfied = true;
            for (int i = 0; i < partsSatisfied.Count; i++)
            {
                if (!partsSatisfied[i])
                {
                    allSatisfied = false;
                    break;
                }
            }
            if (allSatisfied)
            {
                toytype = 1.1f;
            }
        }
        else
        {
            if (comissionController.toyTypes[comissionController.inputNums[current]].Contains(toySubstrings[toyType]))
            {
                toytype = 1;
            }
            else
            {
                toytype = 0.2f;
            }
        }
        totalPrice *= typeWeight * toytype;

        totalPrice = Mathf.RoundToInt(totalPrice);
        moneyScript.IncreaseMoney(totalPrice);
        FinishSell(objToSell);
    }
    public void GetAllMaterialInstancesArea(Transform parent, Material sharedMat, int index)
    {
        for (int i = 0; i < parent.childCount; i++)
        {
            Transform child = parent.GetChild(i);
            Renderer childRenderer = child.GetComponent<Renderer>();
            if (childRenderer != null && childRenderer.sharedMaterial == sharedMat)
            {
                Debug.Log(index + " size " + colorWeights.Count);
                colorWeights[index] += GetSurfaceArea(child.gameObject);
            }
            GetAllMaterialInstancesArea(child, sharedMat, index);
        }
    }

    public float GetSurfaceArea(GameObject objToCalculate)
    {
        Vector3[] vertices = objToCalculate.GetComponent<MeshFilter>().mesh.vertices;
        int[] triangles = objToCalculate.GetComponent<MeshFilter>().mesh.triangles;

        float result = 0f;
        for (int p = 0; p < triangles.Length; p += 3)
        {
            result += (Vector3.Cross(vertices[triangles[p + 1]] - vertices[triangles[p]],
                        vertices[triangles[p + 2]] - vertices[triangles[p]])).magnitude;
        }
        return (result *= 0.5f);
    }
    public void FinishSell(GameObject sellingobj)
    {
        player.GetComponent<ObjectPickUp>().Drop(sellingobj);
        Destroy(sellingobj);
    }
}