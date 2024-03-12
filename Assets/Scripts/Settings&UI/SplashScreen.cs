using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SplashScreen : MonoBehaviour
{
    public GameObject[] frames;
    bool finished = false;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(runIt());
    }

    // Update is called once per frame
    void Update()
    {
        if (finished && Input.anyKeyDown)
        {
            SceneManager.LoadScene("MainMenu");
        }
    }

    IEnumerator runIt()
    {
        yield return new WaitForSeconds(0.3f);
        for (int i = 1; i < frames.Length; i++)
        {
            yield return new WaitForSeconds(0.085f);
            frames[i].SetActive(true);
        }
        finished = true;
    }
}
