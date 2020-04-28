using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public void Play()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void Controls()
    {
        if (!PlayerPrefs.HasKey("control"))
        {
            PlayerPrefs.SetInt("control", 1);
        }
        else
        {
            int control;
            if (PlayerPrefs.GetInt("control") == 0) control = 1;
            else control = 0;
            PlayerPrefs.SetInt("control", control);
        }
    }

    public void Reset()
    {
        MaxScore.Save("0");
    }
}
