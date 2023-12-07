using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using Input = UnityEngine.Input;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public Player[] players;
    [SerializeField] string mainMenu;
    [Header("Events")]
    public UnityEvent onWin;
    public UnityEvent onLose;
    public UnityEvent onPause;
    public UnityEvent onResume;
    public UnityEvent onPhaseChange;

    private bool isGamePaused = false;
    private bool hasGameEnded = false;


    private void OnEnable()
    {
        players = FindObjectsOfType<Player>();
        foreach (Player player in players)
        {
            player.OnPlayerDeath += Lose;
        }

    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        Time.timeScale = 1;
    }

    private void OnDisable()
    {
        foreach (Player player in players)
        {
            player.OnPlayerDeath -= Lose;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }

       if(hasGameEnded) Time.timeScale = 0;
    }

    public void Win()
    {
        if (!hasGameEnded)
        {
            onWin.Invoke();
            EndGame();
        }
    }

    public void Lose()
    {
        if (!hasGameEnded)
        {
            onLose.Invoke();
            EndGame();
        }
    }

    public void TogglePause()
    {
        if (!hasGameEnded)
        {
            isGamePaused = !isGamePaused;

            if (isGamePaused)
            {
                PauseGame();
            }
            else
            {
                ResumeGame();
            }
        }
    }

    private void PauseGame()
    {
        Time.timeScale = 0;
        onPause.Invoke();
    }

    private void ResumeGame()
    {
        Time.timeScale = 1;
        onResume.Invoke();
    }

    private void EndGame()
    {
        hasGameEnded = true;
        Time.timeScale = 0;
    }

    public void Restart()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void MainScreen()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(mainMenu);
    }

    public void ChangePhase()
    {
        onPhaseChange.Invoke();
    }

    public void QuitGame() => Application.Quit();

}