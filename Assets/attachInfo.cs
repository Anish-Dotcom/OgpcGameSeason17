using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class attachInfo : MonoBehaviour
{
    public Vector3 attachPoint;//the location where the object connects to toy
    public Vector3 directionPointAims;

    private void Start()
    {
        Debug.DrawRay(attachPoint, directionPointAims, Color.white, 5f);
    }
}
