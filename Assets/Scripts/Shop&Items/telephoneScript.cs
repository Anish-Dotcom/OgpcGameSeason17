using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class telephoneScript : MonoBehaviour
{
    public PopupInfo lookingAtCheck;
    public int myInfoIndex;

    public GameObject telephone; // this is the telephone object. in scene this is "telephone (1)" this is used to remove that object when picking up the telephone to make it look like you actually picked up the telephone
    public GameObject telephoneOutline; // since the outline of the telephone is a seperate object, i used this game object to set active false as well
    public GameObject telephoneUI; // this is the canvas ui menu that pops up when you open the telephone
    public GameObject telephoneImage;

    public GameObject callShopButton; // i have this button as a gameobject so i can set it active false whenever you open up the shop
    public GameObject shopUI; // this is the canvas ui menu that pops up when you open the shop
    public AudioClip greeting;

    private float moveSpeedSaved; // the following variables are here to store what the sens was and then set it to 0 so that they cant look around when inside the menu and then goes back when the ui is closed
    private float sensXSaved;
    private float sensYSaved;

    public float distFromObject = 0;

    public bool telephoneIsOpen;

    public TMP_Text subtitles;
    public GameObject subtitlesHighlight;

    public Button callButton; // buttons in the telephone menu
    public Button awayButton;

    public GameObject commissionsUI; // this was the commissions menu but its prolly gon get removed cause brady had a better idea

    private MusicController musicController;

    public GameObject Telephone; // this is the actual telephone object that has this script attached to it. it was used to reference the item manager script also attached to this object.

    public GameObject interact; // this is what set active true when you hover over the telephone

    public MenuController menuController; // for controlling menus
    public bedScript bedScript;


    public void PutTelephoneBackButton()
    {
        //ItemsInShop.reset();
        for (int i = 0; i < Telephone.GetComponent<ItemManager>().costs.Length; i++)
        {
            Telephone.GetComponent<ItemManager>().costs[i] = 0;
        }
        callButton.interactable = true;
        awayButton.interactable = true;
        telephoneIsOpen = false;
        telephone.SetActive(true);
        telephoneOutline.SetActive(true);
        menuController.closeMenu(telephoneUI);
        telephoneImage.SetActive(false);
    }

    public void CallShopButton()
    {
        callButton.interactable = false;
        awayButton.interactable = false;
        if(bedScript.Day == 5 || bedScript.Day == 6)
        {
            StartCoroutine(shopClosed());
        }
        else
        {
            StartCoroutine(showText());
            musicController.PlayDialog(greeting);
        }
    }

    IEnumerator showText()
    {
        yield return new WaitForSeconds(0.3f);
        subtitlesHighlight.SetActive(true);
        subtitles.text = "Thank you for calling Playful Depot.";
        yield return new WaitForSeconds(2.7f);
        subtitles.text = "What would you like to buy?";
        yield return new WaitForSeconds(2.3f);
        subtitlesHighlight.SetActive(false);
        subtitles.text = "";
        callShopButton.SetActive(false);
        awayButton.interactable = true;
        menuController.openMenu(shopUI);
    }

    IEnumerator shopClosed()
    {
        yield return new WaitForSeconds(0.3f);
        subtitlesHighlight.SetActive(true);
        subtitles.text = "Sorry, Playful Depot is closed today.";
        yield return new WaitForSeconds(2.7f);
        subtitlesHighlight.SetActive(false);
        subtitles.text = "";
        awayButton.interactable = true;
    }

    void Start()
    {
        musicController = GameObject.FindGameObjectsWithTag("Music Controller")[0].GetComponent<MusicController>();
        
    }

    void Update()
    {
        if (lookingAtCheck.lookingAt[myInfoIndex])
        {
            if (Input.GetMouseButton(0) && !telephoneIsOpen && !menuController.menuOpen)
            {
                //commissionsUI.SetActive(false);
                //shopUI.SetActive(false);

                telephone.SetActive(false);
                telephoneOutline.SetActive(false);

                menuController.openMenu(telephoneUI);
                telephoneImage.SetActive(true);
                callShopButton.SetActive(true);
                telephoneIsOpen = true;
            }
        }

        if (shopUI.activeInHierarchy)// close shop menu if escape pressed. IDK were it was programmed to close before
        {
            if (Input.GetKey(KeyCode.Escape))
            {
                callButton.interactable = true;
                menuController.closeMenu(shopUI);
                menuController.closeMenu(telephoneUI);
                telephoneImage.SetActive(false);
                telephoneIsOpen = false;
                telephone.SetActive(true);
                telephoneOutline.SetActive(true);
                Cursor.visible = false;
            }
        }
    }
}
