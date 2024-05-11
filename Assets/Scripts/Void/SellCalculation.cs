using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SellCalculation : MonoBehaviour
{
    public ComissionController comissionController;
    public ItemsInShop itemsInShop;

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
    public void CalculatePrice(GameObject objToSell, int completingComission)
    {
        colorsUsed.Clear();
        for (int i = 0; i < objToSell.transform.childCount; i++)
        {
            GameObject currentObj = objToSell.transform.GetChild(i).gameObject;//the current object we are checking, component
            //calculating how much each color was used
            Color currentColor = currentObj.GetComponent<Material>().color;
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
                colorWeights.Add(GetSurfaceArea(currentObj));
            }
            else
            {
                colorWeights[i] += GetSurfaceArea(currentObj);
            }

            //check how much each item added costs
            float price = 0f;
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
                            continue;
                        }
                        partTypes.Add(toySubstrings[j]);
                        numOfParts.Add(1);
                    }
                }
            }
            
            //add each of the components of price together
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
}
