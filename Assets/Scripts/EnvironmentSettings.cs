using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentSettings : MonoBehaviour
{
    public float fogDen;
    public bool fogOn = true;
    void Start()
    {
        RenderSettings.fogDensity = fogDen;
        RenderSettings.fog = fogOn;
    }
}
