using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameManager gameManager;
    public GameObject winScreen;
    public GameObject loseScreen;

    public GameObject pauseScreenCanvas;
    public GameObject resumeButton;

    public void ShowWinScreen()
    {
        winScreen.SetActive(true);
    }

    public void ShowLoseScreen()
    {
        loseScreen.SetActive(true);
    }

    public void ShowPauseScreen()
    {
        pauseScreenCanvas.SetActive(true);
        resumeButton.SetActive(true);
    }

    public void HidePauseScreen()
    {
        pauseScreenCanvas.SetActive(false);
        resumeButton.SetActive(false);
    }
}
