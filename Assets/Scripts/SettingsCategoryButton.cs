using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsCategoryButton : MonoBehaviour
{
    public SelectedOutline selectedOutlineScript;
    public SettingsMenuController settingsMenuController;
    public int position = 0;
    void Start()
    {
        selectedOutlineScript = GameObject.Find("Selected Outline").GetComponent<SelectedOutline>();
    }
    public void setSelectedValue()
    {
        settingsMenuController.selected = position;
        settingsMenuController.updatePage();
    }

}
