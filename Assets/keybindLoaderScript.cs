using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class keybindLoaderScript : MonoBehaviour
{

    [SerializeField] private InputActionAsset playerControls;

    private void Start()
    {
        //string rebinds = PlayerPrefs.GetString("rebinds", string.Empty);

        //if (string.IsNullOrEmpty(rebinds)) { return; }
        //Debug.Log(rebinds);
        //playerControls.LoadBindingOverridesFromJson(rebinds);
    }
}
