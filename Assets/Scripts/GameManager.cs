using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    public enum GameState
    {
        Play,
        Pause,
        Win,
        Lose
    }

    public GameState gameState;

    public GameObject pauseUI;
    public GameObject winUI;
    public GameObject loseUI;
    public string sceneName;


    private void Start()
    {
        SetState(GameState.Play);
    }

    private void Update()
    {
        switch (gameState)
        {
            case GameState.Play:
                Time.timeScale = 1;
                pauseUI.SetActive(false);
                break;

            case GameState.Pause:
                Time.timeScale = 0;
                pauseUI.SetActive(true);
                break;

            case GameState.Win:
                Time.timeScale = 0;
                winUI.SetActive(true);
                break;

            case GameState.Lose:
                Time.timeScale = 0;
                loseUI.SetActive(true);
                break;
        }
    }

    private void SetState(GameState newState)
    {
        gameState = newState;       
    }

    public void PauseGame()
    {
        if (gameState == GameState.Play)
        {
            SetState(GameState.Pause);
        }
    }

    public void ResumeGame()
    {
        if (gameState == GameState.Pause)
        {
            SetState(GameState.Play);
        }
    }
    //change this to build index later
    public void Restart()
    {
        SceneManager.LoadScene(sceneName);
    }
}