using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeFollowPlayer : MonoBehaviour
{
    public GlobalDissolveCon globalDissolveCon;

    public GameObject Eye;
    public GameObject Player;
    public GameObject referencePos;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (globalDissolveCon.trueArea == 3)
        {
            //angle between current vectors
            float angle = Vector3.Angle(Eye.transform.position - referencePos.transform.position, Eye.transform.position-Player.transform.position);
            // Calculate the axis of rotation
            Vector3 axis1 = Vector3.Cross(Eye.transform.position - referencePos.transform.position, Eye.transform.position - Player.transform.position).normalized;
            Eye.transform.Rotate(axis1, angle);
        }
    }
}
