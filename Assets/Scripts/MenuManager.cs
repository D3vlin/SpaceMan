using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public static MenuManager sharedInstance;
    public Canvas menuCanvas;
    public Canvas inGameCanvas;
    public Canvas gameOverCanvas;

    void Awake()
    {
        if (sharedInstance == null)
        {
            sharedInstance = this;
        }
    }

    public void ShowMainMenu()
    {
        menuCanvas.enabled = true;
        inGameCanvas.enabled = false;
        gameOverCanvas.enabled = false;
    }

    public void HideMainMenu()
    {
        menuCanvas.enabled = false;
        inGameCanvas.enabled = true;
        gameOverCanvas.enabled = false;
    }

    public void ShowGameOver()
    {
        menuCanvas.enabled = false;
        inGameCanvas.enabled = false;
        gameOverCanvas.enabled = true;
    }

    public void ExitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}