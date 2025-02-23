using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void Play()
    {
        SceneManager.LoadScene("Scene1");
    }
    public void Quit()
    {
        Application.Quit();
        Debug.Log("Play Has Quit The Game");

    }
}
