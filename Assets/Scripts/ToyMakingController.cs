using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToyController : MonoBehaviour
{
    public GameObject toyParentObj;
    public GameObject[] toyParts;
    public string[] toyTags;

    public Vector3[] toyPartFinalPos;
    public bool[] toyPartInFinal;
    public GameObject finalToy;
    

    void Start()
    {
        //when you get items delivered add them to the toy parts and toy tags arrays, the tag is what type of object it is, E: "spring," or "screw"
    }
    void Update()
    {
        
    }

    //---
    public void summonToy()
    {

    }
    //---

    private void OnTriggerEnter(Collider col)//To move 'other' which must be an object in toy parts (use same way as toy parts is refed) - this is for parts of the toys which requires it, but each toy will still have player originality
    {
        if (col.gameObject.CompareTag("Object")) 
        { 
            
        }
    }
}
