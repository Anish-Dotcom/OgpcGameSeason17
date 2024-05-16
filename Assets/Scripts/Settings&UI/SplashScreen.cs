using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SplashScreen : MonoBehaviour
{
    bool finished = false;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(runIt());
    }

    // Update is called once per frame
    void Update()
    {
        if (finished)
        {
            SceneManager.LoadScene("MainMenu");
        }
    }

    IEnumerator runIt()
    {
        yield return new WaitForSeconds(3f);
        finished = true;
    }
}
