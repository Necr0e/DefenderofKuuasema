﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu:MonoBehaviour
{
    public void NewGameClick()
    {
        SceneManager.LoadScene(1);
    }

    public void QuitGameClick()
    {
        Application.Quit();
    }
}
