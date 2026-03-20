using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem; 

public class GameManager : MonoBehaviour
{
    [Header("Configurações de Tempo")]
    [SerializeField] private float timeToSurvive = 60f;
    [SerializeField] private TextMeshProUGUI timerText;

    [Header("Interfaces")]
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject victoryPanel;
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private Health playerHealth;

    private bool isGameOver = false;
    private bool isPaused = false;
    
    private float currentTime;

    private void Start()
    {
        currentTime = timeToSurvive;

        if (gameOverPanel != null) gameOverPanel.SetActive(false);
        if (victoryPanel != null) victoryPanel.SetActive(false);
        if (pausePanel != null) pausePanel.SetActive(false);

        if (playerHealth != null)
            playerHealth.OnDeath += TriggerGameOver;

        Time.timeScale = 1f;
        AudioManager.Instance.PlayMusic("LevelTheme");
        Cursor.lockState = CursorLockMode.Confined;
    }

    private void Update()
    {
       
        if (isGameOver || isPaused) return;

        UpdateTimer();
    }

    private void UpdateTimer()
    {
        currentTime -= Time.deltaTime;
        if (currentTime <= 0)
        {
            currentTime = 0;
            WinGame();
        }

        int minutes = Mathf.FloorToInt(currentTime / 60);
        int seconds = Mathf.FloorToInt(currentTime % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

   
    public void OnPause(InputValue value)
    {
   
        if (value.isPressed && !isGameOver)
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        isPaused = !isPaused;
        pausePanel.SetActive(isPaused);

        Time.timeScale = isPaused ? 0f : 1f;
    }

    private void TriggerGameOver()
    {
        if (isGameOver) return;
        isGameOver = true;
        gameOverPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    private void WinGame()
    {
        if (isGameOver) return;
        isGameOver = true;
        victoryPanel.SetActive(true);
        Time.timeScale = 0f;
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayMusic("VictoryTheme");
        }
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}