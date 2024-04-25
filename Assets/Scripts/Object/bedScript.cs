using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.Audio;
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
    public CaledarScript caledarScript;
    public FatigueController fatigueController;
    public bool lookingAt;
    public GameObject sleepQuestionMark;
    public int Day = 0;
    public AudioMixer Mixer;
    public AudioMixerSnapshot Default;
    public AudioMixerSnapshot Muffled;
    public int sleepingLength;
    public GameObject crosshair;

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
        if (sleepy&&lookingAt) 
        {
            StartCoroutine(transparentUp());
        }
    }

    public void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(playerCam.transform.position, playerCam.transform.forward, out hit, 3.5f))//working
        {
            //print("Ray");
            if (hit.collider.gameObject.CompareTag("Bed"))
            {
                lookingAt = true;
                sleepQuestionMark.SetActive(true);
            }
            else
            {
                lookingAt = false;
                sleepQuestionMark.SetActive(false);
            }
        }
        else
        {
            lookingAt = false;
            sleepQuestionMark.SetActive(false);
        }
    }
    IEnumerator transparentUp ()
    {
        Muffled.TransitionTo(1f);
        crosshair.SetActive(false);
        //print("transparentUp");

        if (profile.TryGet<Vignette>(out vig))
        {
            vig.intensity.value += 0.01f;
        }
        transparent.GetComponent<CanvasGroup>().alpha += 0.015f;
        yield return new WaitForSeconds(0.01f);
        z++;
        if (z <= sleepingLength)
        {
            StartCoroutine(transparentUp());
        }
        else
        {
            StartCoroutine(transparentDown());
        }
    }
    IEnumerator transparentDown()
    {

        fatigueController.resetTransparency();
        StopCoroutine(transparentUp());
        if (profile.TryGet<Vignette>(out vig))
        {
            vig.intensity.value -= 0.01f;
        }
        transparent.GetComponent<CanvasGroup>().alpha -= 0.015f;
        yield return new WaitForSeconds(0.01f);
        z--;

        if (z <= sleepingLength && z > 0)
        {
            StartCoroutine(transparentDown());
        }
        if (z == 0)
        {
            Default.TransitionTo(1f);
            crosshair.SetActive(true);
            caledarScript.text = caledarScript.text + " /";

            Day++;
            caledarScript.day = Day;
            if (Day == 7)
            {
                Day = 0;
                caledarScript.bills();
            }
        }

    }
    void checkSleep()
    {
        
    }
}
