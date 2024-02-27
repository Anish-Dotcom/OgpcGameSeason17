using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FPSCounter : MonoBehaviour
{
    public TMP_Text fpsCounter;

    private int lastFrameIndex;
    private float[] frameDeltaTimeArray;

    // Start is called before the first frame update
    private void Awake()
    {
        frameDeltaTimeArray = new float[50];
    }

    // Update is called once per frame
    void Update()
    {
        frameDeltaTimeArray[lastFrameIndex] = Time.deltaTime;
        lastFrameIndex = (lastFrameIndex + 1) % frameDeltaTimeArray.Length;

        Application.targetFrameRate = Screen.currentResolution.refreshRate;
        fpsCounter.text = "FPS: " + Mathf.RoundToInt(CalculateFPS()).ToString();
    }

    private float CalculateFPS()
    {
        float total = 0f;
        foreach(float deltaTime in frameDeltaTimeArray)
        {
            total += deltaTime;
        }
        return frameDeltaTimeArray.Length / total;
    }
}
