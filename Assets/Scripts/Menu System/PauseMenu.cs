using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject settingsMenuPause;
    [SerializeField] GameObject audioMenu;
    [SerializeField] GameObject displayMenu;
    [SerializeField] GameObject controlsMenu;
    [SerializeField] bool isPaused;
    [SerializeField] string mainMenu;
    [SerializeField] string playAgain;
    // Start is called before the first frame update
    void Start()
    {
        pauseMenu.SetActive(false);
        settingsMenuPause.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }


    public void PauseGame()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }
    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
        displayMenu.SetActive(false);
        audioMenu.SetActive(false);
        controlsMenu.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }
    public void PauseOptions()
    {
        settingsMenuPause.SetActive(true);
        pauseMenu.SetActive(false);
    }

    public void AudioSetting()
    {
        pauseMenu.SetActive(false);
        displayMenu.SetActive(false);
        controlsMenu.SetActive(false);
        audioMenu.SetActive(true);
    }

    public void DisplaySetting()
    {
        pauseMenu.SetActive(false);
        audioMenu.SetActive(false);
        controlsMenu.SetActive(false);
        displayMenu.SetActive(true);
    }

    public void ControlsSetting()
    {
        pauseMenu.SetActive(false);
        audioMenu.SetActive(false);
        displayMenu.SetActive(false);
        controlsMenu.SetActive(true);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(playAgain);
    }

    public void ReturnToPauseStart()
    {
        pauseMenu.SetActive(true);
        settingsMenuPause.SetActive(false);
    }

    public void ExitGame()
    {
        SceneManager.LoadScene(mainMenu);
        Time.timeScale = 1f;
    }
}
