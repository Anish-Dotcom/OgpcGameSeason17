using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SettingsMenuController : MonoBehaviour
{
    public int selected = 1;
    public GameObject settingsPageContainer;
    public List<GameObject> settingsPages;
    // Start is called before the first frame update
    void Start()
    {
        foreach (Transform child in settingsPageContainer.transform)
        {
            settingsPages.Add(child.gameObject);
        }
        updatePage();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void updatePage()
    {
        int index = 0;
        foreach (GameObject page in settingsPages)
        {
            page.SetActive(index == selected);
            index++;
        }
    }
}
