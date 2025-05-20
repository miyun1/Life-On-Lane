using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance; // Singleton for easy access

    [Header("Game Settings")]
    public float gameDuration = 60f;
    private float timer;

    [Header("Tree Tracking")]
    public static int totalTreeRevived = 0;
    private int totalTrees;
    private int revivedTrees;

    [Header("Enemy Tracking")]
    private int enemiesKilled;

    private bool gameEnded = false;

    // Data to carry to result scene
    public float RevivedTreePercent => totalTrees == 0 ? 0 : ((float)GameManager.totalTreeRevived / totalTrees) * 100f;
    public int EnemiesKilled => enemiesKilled;
    public float TimeLeft => Mathf.Max(0, timer);
    public int RevivedTreeCount => GameManager.totalTreeRevived;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        gameEnded = false;
        timer = gameDuration;
        CountTotalTrees();
    }

    void Update()
    {
        if (gameEnded) return;

        if (timer > 0f)
        {
            timer -= Time.deltaTime;
        }

        if (timer <= 0f)
        {
            timer = 0f;
            gameEnded = true;
            StoreResultsAndGoToResultScene();
            return;
        }
        UpdateRevivedTrees();
    }

    void CountTotalTrees()
    {
        totalTrees = FindObjectsOfType<TreeBehavior>().Length;
    }

    void UpdateRevivedTrees()
    {
        int count = 0;
        foreach (var tree in FindObjectsOfType<TreeBehavior>())
        {
            if (tree.isRevived)
                count++;
        }
        revivedTrees = count;
        Debug.Log(totalTreeRevived);
    }

    public void AddEnemyKill()
    {
        enemiesKilled++;
    }

    public void ResetGame()
    {
        gameEnded = false;
        timer = gameDuration;
        enemiesKilled = 0;
        totalTreeRevived = 0;
        revivedTrees = 0;
        CountTotalTrees();
    }

    void StoreResultsAndGoToResultScene()
    {
        ResultData.revivedTreePercent = RevivedTreePercent;
        ResultData.revivedTreeCount = RevivedTreeCount;
        ResultData.enemiesKilled = EnemiesKilled;
        SceneManager.LoadScene("ResultScene");
    }
}

// Helper static class to carry data between scenes
public static class ResultData
{
    public static float revivedTreePercent;
    public static int revivedTreeCount;
    public static int enemiesKilled;
}