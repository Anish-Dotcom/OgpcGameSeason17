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
    public void Start()
    {
        if (profile.TryGet<Vignette>(out vig))
        {
            vig.intensity.value = 0.3f;
        }
    }
    private void OnMouseUpAsButton()
    {
        StartCoroutine(transparentUp());

        playerCam.transform.position = new Vector3(-0.384f, 0.787f, 1.406f);
    }
    IEnumerator transparentUp ()
    {
        if (profile.TryGet<Vignette> (out vig))
        {
            vig.intensity.value += 0.01f;
        }
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
        yield return new WaitForSeconds(0.01f);
        z--;

        if (z <= 100)
        {
            StartCoroutine(transparentDown());
        }
    }
}
