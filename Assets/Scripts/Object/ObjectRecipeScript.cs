using UnityEngine;

public class ObjectRecipeScript : MonoBehaviour
{
    public GameObject product;
    public Vector3 productDirection = new Vector3(0, 0, 0);
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        bool complete = true;
        foreach (Transform objectTrigger in transform)
        {
            partLocationTrigger script = objectTrigger.GetComponent<partLocationTrigger>();
            if (!script.completed)
            {
                complete = false;
                break;
            }

        }
        if (complete)
        {
            foreach (Transform objectTrigger in transform)
            {
                partLocationTrigger script = objectTrigger.GetComponent<partLocationTrigger>();
                Debug.Log(script);
                script.completedPart.tag = "Untagged";
                Destroy(script.completedPart);
                script.completedPart = null;

            }
            Debug.Log("IT WORKED");
            Instantiate(product, transform.position, Quaternion.Euler(productDirection));

        }
    }
}
