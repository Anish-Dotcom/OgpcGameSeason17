using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuBlueprintScript : MonoBehaviour
{
    public AssemblyController ac;
    public GameObject assembly;
    // Start is called before the first frame update

    public void SetAssembly()
    {
        Debug.Log("Button pressed");
        ac.SetAssembly(assembly);
    }
}
