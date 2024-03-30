using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;

public class RebindableKeybindScript : MonoBehaviour
{
    // https://www.youtube.com/watch?v=dUCcZrPhwSo
    [SerializeField] private InputActionReference action;
    [SerializeField] private InputActionAsset playerControls;
    [SerializeField] private TMP_Text bindDisplayText;
    [SerializeField] private GameObject startRebindObject;

    private InputActionRebindingExtensions.RebindingOperation rebindingOperation;

    private void Start()
    {
        Load();
    }

    public void Save()
    {
        string rebinds = playerControls.SaveBindingOverridesAsJson();
        PlayerPrefs.SetString("rebinds", rebinds);
        PlayerPrefs.Save();

    }

    public void Load()
    {
        string rebinds = PlayerPrefs.GetString("rebinds", string.Empty);

        if (string.IsNullOrEmpty(rebinds)) { return; }
        playerControls.LoadBindingOverridesFromJson(rebinds);

        int bindingIndex = action.action.GetBindingIndexForControl(action.action.controls[0]);
        bindDisplayText.text = InputControlPath.ToHumanReadableString(action.action.bindings[bindingIndex].effectivePath, InputControlPath.HumanReadableStringOptions.OmitDevice);


    }
    public void StartRebinding()
    {
        startRebindObject.SetActive(false);
        rebindingOperation = action.action.PerformInteractiveRebinding()
            .WithControlsExcluding("Mouse")
            .OnMatchWaitForAnother(0.1f)
            .OnComplete(opperation => rebindComplete())
            .Start();
    }

    private void rebindComplete()
    {

        int bindingIndex = action.action.GetBindingIndexForControl(action.action.controls[0]);
        bindDisplayText.text = InputControlPath.ToHumanReadableString(action.action.bindings[bindingIndex].effectivePath,InputControlPath.HumanReadableStringOptions.OmitDevice);
        rebindingOperation.Dispose();
        startRebindObject.SetActive(true);
        Save();

    }
}
