using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class attachInfo : MonoBehaviour
{
    public GameObject attachPoint;//the location where the object connects to toy
    public Vector3 directionPointAims;

    private void Start()
    {
        Debug.DrawRay(attachPoint.transform.position, directionPointAims, Color.white, 5f);
    }
}
