using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupInfo : MonoBehaviour
{
    public string[] heldObjTag;//tags for held objs in order for popup to appear
    public int[] popupIndex;//int index value of the popup

    public bool[] lookingAt;
    //public int[] childIndex;// -1 bc 0 is current obj
}