using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class MoneyScrip : MonoBehaviour
{
    public float currentMoney;
    public float unChangedMoney;
    public Text moneyText;
    public Text increaseText;
    public CanvasGroup increaseTextCanvasGroup;
    public Text decreaseText;
    public CanvasGroup decreaseTextCanvasGroup;
    public float amount2;

    public bedScript bedScript;
    public GameObject gameOverUI;
    public TMP_Text weeksSurvived;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        moneyText.text = "$" + currentMoney.ToString("F2");
        if(currentMoney < 0) // game over
        {
            gameOverUI.SetActive(true);
            weeksSurvived.text = "Weeks survived: " + bedScript.week.ToString();
        }
    }

    public void playAgain()
    {
        gameOverUI.SetActive(false);
        SceneManager.LoadScene("MainMenu");
    }


    public void IncreaseMoney(float amount)
    {
        unChangedMoney = currentMoney;
        amount2 = amount;
        increaseText.text = "+" + amount.ToString("F2");

        InvokeRepeating("TextRollUp", 0, 0.0001f);

        increaseTextCanvasGroup.alpha = 1;
        InvokeRepeating("increaseDisappear", 1.5f, 0.001f);

    }

    public void increaseDisappear()
    {
        increaseTextCanvasGroup.alpha -= 0.1f;
        if (increaseTextCanvasGroup.alpha == 0)
        {
            increaseTextCanvasGroup.alpha = 0;
            CancelInvoke("increaseDisappear");
        }
    }

    public void TextRollUp()
    {
        currentMoney += amount2 / 200;
        if (currentMoney > unChangedMoney + amount2)
        {
            currentMoney = unChangedMoney + amount2;
            CancelInvoke("TextRollUp");
        }
    }
    public void DecreaseMoney(float amount)
    {
        unChangedMoney = currentMoney;
        amount2 = amount;
        decreaseText.text = "-" + amount.ToString("F2");

        InvokeRepeating("TextRollDown", 0, 0.0001f);

        decreaseTextCanvasGroup.alpha = 1;
        InvokeRepeating("decreaseDisappear", 1.5f, 0.001f);
    }

    public void decreaseDisappear()
    {
        decreaseTextCanvasGroup.alpha -= 0.1f;
        if (decreaseTextCanvasGroup.alpha == 0)
        {
            decreaseTextCanvasGroup.alpha = 0;
            CancelInvoke("decreaseDisappear");
        }
    }

    public void TextRollDown()
    {
        currentMoney -= amount2 / 200;
        if (currentMoney < unChangedMoney - amount2)
        {
            currentMoney = unChangedMoney - amount2;
            CancelInvoke("TextRollDown");
        }
    }

}
