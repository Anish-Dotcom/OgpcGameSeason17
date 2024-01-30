using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class telephoneScript : MonoBehaviour
{
    public GameObject playerCam;
    public GameObject telephone;
    public GameObject telephoneOutline;
    public GameObject telephoneUI;

    public GameObject callShopButton;
    public GameObject shopUI;
    public AudioClip greeting;

    private float moveSpeedSaved;
    private float sensXSaved;
    private float sensYSaved;

    public float distFromObject = 0;

    public bool telephoneIsOpen;

    public TMP_Text subtitles;
    public GameObject subtitlesHighlight;

    public Button callButton;
    public Button awayButton;

    public GameObject commissionsUI;

    public string[] daysOfWeek;
    private string currentDayOfWeek;
    public TMP_Text dayOfWeekText;
    public TMP_Text time;
    public string[] hour;
    public string[] minutes;
    private string currentHour;
    private string currentMinutes;
    private string meridiem;

    private int currentDayOfWeekIndex;
    private int currentHourIndex;
    private int currentMinutesIndex;

    public bool GameIsPaused;

    public float secondsPer10Minutes;
    private MusicController musicController;

    public GameObject Telephone;

    public TMP_Text fpsCounter;

    public GameObject interact;

    private void OnMouseOver()
    {
        distFromObject = Vector3.Distance(playerCam.transform.position, telephone.transform.position);

        if (distFromObject <= 2.1 && !telephoneIsOpen)
        {
            interact.SetActive(true);
        }
        else
        {
            interact.SetActive(false);
        }
    }

    private void OnMouseUpAsButton()
    {
        distFromObject = Vector3.Distance(playerCam.transform.position, telephone.transform.position);

        if(distFromObject <= 2.1 && !telephoneIsOpen)
        {
            commissionsUI.SetActive(false);
            shopUI.SetActive(false);
            callShopButton.SetActive(true);
            telephoneIsOpen = true;
            telephone.SetActive(false);
            telephoneOutline.SetActive(false);
            telephoneUI.SetActive(true);
            moveSpeedSaved = PlayerMove.moveSpeed;
            PlayerMove.moveSpeed = 0f;
            sensXSaved = PlayerCam.sensX;
            sensYSaved = PlayerCam.sensY;
            PlayerCam.sensX = 0f;
            PlayerCam.sensY = 0f;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }

    public void PutTelephoneBackButton()
    {
        ItemsInShop.reset();
        for (int i = 0; i < Telephone.GetComponent<ItemManager>().costs.Length; i++)
        {
            Telephone.GetComponent<ItemManager>().costs[i] = 0;
        }
        callButton.interactable = true;
        awayButton.interactable = true;
        telephoneIsOpen = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        PlayerMove.moveSpeed = moveSpeedSaved;
        PlayerCam.sensX = sensXSaved;
        PlayerCam.sensY = sensYSaved;
        telephone.SetActive(true);
        telephoneOutline.SetActive(true);
        telephoneUI.SetActive(false);
    }

    public void CallShopButton()
    {
        callButton.interactable = false;
        awayButton.interactable = false;
        StartCoroutine(showText());
        //musicController.PlayDialog(greeting);
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
        shopUI.SetActive(true);
        callShopButton.SetActive(false);
        awayButton.interactable = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        //musicController = GameObject.FindGameObjectsWithTag("Music Controller")[0].GetComponent<MusicController>();
        currentHourIndex = 0;
        currentMinutesIndex = 5;
        currentDayOfWeekIndex = 6;
        meridiem = "AM";
        timeGoes();
        StartCoroutine(fpsReadable());
    }

    IEnumerator fpsReadable()
    {
        yield return new WaitForSeconds(0.5f);
        fpsCounter.text = "FPS: " + (int)(1f / Time.deltaTime);
        StartCoroutine(fpsReadable());
    }

    // Update is called once per frame
    void Update()
    {
        currentDayOfWeek = daysOfWeek[currentDayOfWeekIndex]; // time system
        currentHour = hour[currentHourIndex];
        currentMinutes = minutes[currentMinutesIndex];

        //dayOfWeekText.text = currentDayOfWeek;
        time.text = currentHour + ":" + currentMinutes + " " + meridiem;

        if(currentHourIndex == 5)
        {
            meridiem = "PM";
        }
        if(currentHourIndex == 17)
        {
            meridiem = "AM";
        }

        if (Input.GetKeyDown(KeyCode.C) && callButton.interactable && !shopUI.activeInHierarchy) // show commissions
        {
            if (commissionsUI.activeInHierarchy)
            {
                commissionsUI.SetActive(false);
            }
            else
            {
                commissionsUI.SetActive(true);
            }
        }
    }

    public void timeGoes()
    {
        if (currentHourIndex == 0 && currentMinutesIndex == 5) // if its a new day
        {
            if (currentDayOfWeekIndex == 6)
            {
                currentDayOfWeekIndex = 0;
            }
            else
            {
                currentDayOfWeekIndex = currentDayOfWeekIndex + 1;
            }
            currentHourIndex = 1;
            currentMinutesIndex = 0;
        }

        StartCoroutine(minutesPass());
    }

    IEnumerator minutesPass()
    {
        if (!GameIsPaused)
        {
            yield return new WaitForSeconds(secondsPer10Minutes);
            currentMinutesIndex = currentMinutesIndex + 1;
            if(currentMinutesIndex == 6)
            {
                currentHourIndex = currentHourIndex + 1;
                currentMinutesIndex = 0;
            }
            if(currentHourIndex == 19)
            {
                // falls on floor from sleepiness
            }
            else
            {
                StartCoroutine(minutesPass());
            }
        }
    }
}
