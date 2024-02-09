using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
public class bedScript : MonoBehaviour
{

    public float distFromObject = 0;
    public int z = 0;
    public GameObject transparent;
    public GameObject GlobalVolume;
    public GameObject Blur;
    public GameObject playerCam;
    public VolumeProfile profile;
    public Vignette vig;
    public bool sleepy = false;
    public void Start()
    {
        if (profile.TryGet<Vignette>(out vig))
        {
            vig.intensity.value = 0.3f;
        }
        InvokeRepeating("checkSleep", 0, 1);
    }
    private void OnMouseUpAsButton()
    {
        if (sleepy)
        {
            print("sleep");
            StartCoroutine(transparentUp());
        }
        //Add animation or whatever here
    }
    IEnumerator transparentUp ()
    {
        if (profile.TryGet<Vignette> (out vig))
        {
            vig.intensity.value += 0.01f;
        }
        transparent.GetComponent<CanvasGroup>().alpha += 0.015f;
        yield return new WaitForSeconds(0.01f);
        z++;
        if(z <= 100)
        {
            StartCoroutine(transparentUp());
        } else
        {
            StartCoroutine(transparentDown());
        }
    }
    IEnumerator transparentDown()
    {
        StopCoroutine(transparentUp());
        if (profile.TryGet<Vignette>(out vig))
        {
            vig.intensity.value -= 0.01f;
        }
        transparent.GetComponent<CanvasGroup>().alpha -= 0.015f;
        yield return new WaitForSeconds(0.01f);
        z--;

        if (z <= 100)
        {
            StartCoroutine(transparentDown());
        }
    }
    void checkSleep()
    {
        
    }
}
