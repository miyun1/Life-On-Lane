using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

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

    void Start()
    {
        // Assign button listeners if buttons are set
        if (restartButton != null) restartButton.onClick.AddListener(OnRestart);
        if (quitButton != null) quitButton.onClick.AddListener(OnQuit);
        if (homeButton != null) homeButton.onClick.AddListener(OnHome);
    }

    void Update()
    {
        if (timerText != null && GameManager.Instance != null)
        {
            float timer = GameManager.Instance.TimeLeft;
            int minutes = Mathf.FloorToInt(timer / 60f);
            int seconds = Mathf.FloorToInt(timer % 60f);
            timerText.text = $"Time: {minutes:00}:{seconds:00}";
        }

        if (treeCountText != null && GameManager.Instance != null)
        {
            treeCountText.text = $"Tree Revived: {GameManager.Instance.RevivedTreePercent:F0}%";
        }
        if (enemyCountText != null && GameManager.Instance != null)
        {
            enemyCountText.text = $"Enemy Downed: {GameManager.Instance.EnemiesKilled}";
        }
    }

    public void OnPlay()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("GameScene");
        GameManager.Instance.ResetGame();
    }

    public void OnRestart()
    {
        GameManager.Instance.ResetGame();
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void OnHome()
    {
        
        Time.timeScale = 1f;
        SceneManager.LoadScene("MenuScene");
        GameManager.Instance.ResetGame();
    }

    public void OnQuit()
    {
        Time.timeScale = 1f;
        Application.Quit();
    }
}