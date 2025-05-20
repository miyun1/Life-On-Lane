using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private TextMeshProUGUI treeCountText;
    [SerializeField] private TextMeshProUGUI enemyCountText;

    [Header("Buttons")]
    [SerializeField] private Button playButton;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button quitButton;
    [SerializeField] private Button homeButton;

    private GameManager gameManager;

    void Start()
    {
        // Assign button listeners if buttons are set
        if (restartButton != null) restartButton.onClick.AddListener(OnRestart);
        if (quitButton != null) quitButton.onClick.AddListener(OnQuit);
        if (homeButton != null) homeButton.onClick.AddListener(OnHome);

        // Find the GameManager in the scene
        gameManager = FindObjectOfType<GameManager>();
    }

    void Update()
    {
        if (gameManager == null)
            gameManager = FindObjectOfType<GameManager>();

        if (gameManager == null)
            return;

        // Timer
        if (timerText != null)
        {
            float timer = gameManager.TimeLeft;
            int minutes = Mathf.FloorToInt(timer / 60f);
            int seconds = Mathf.FloorToInt(timer % 60f);
            timerText.text = $"Time: {minutes:00}:{seconds:00}";
        }

        // Tree revived percentage
        if (treeCountText != null)
        {
            treeCountText.text = $"Tree Revived: {ResultData.revivedTreeCount} ({ResultData.revivedTreePercent:F0}%)";
        }

        // Enemy downed count
        if (enemyCountText != null)
        {
            enemyCountText.text = $"Enemy Downed: {ResultData.enemiesKilled}";
        }
    }

    public void OnPlay()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("GameScene");
        // No need to call ResetGame() here, as a new GameManager will be created in the new scene
    }

    public void OnRestart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        // No need to call ResetGame() here, as a new GameManager will be created in the new scene
    }

    public void OnHome()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MenuScene");
        // No need to call ResetGame() here, as a new GameManager will be created in the new scene
    }

    public void OnQuit()
    {
        Time.timeScale = 1f;
        Application.Quit();
    }
}