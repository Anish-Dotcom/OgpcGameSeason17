using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bedScript : MonoBehaviour
{

    public float distFromObject = 0;

    public GameObject transparent;
    public GameObject playerCam;
    private void OnMouseUpAsButton()
    {
        transparent.GetComponent<CanvasGroup>().alpha = 0.3f;
        //playerCam.transform.Translate(new Vector3(-0.384f, 0.787f, 1.406f));
    }

}
