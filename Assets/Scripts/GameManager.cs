using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance; // Singleton for easy access

    [Header("Game Settings")]
    public float gameDuration = 60f; // 1 minute
    private float timer;

    [Header("Tree Tracking")]
    private int totalTrees;
    private int revivedTrees;

    [Header("Enemy Tracking")]
    private int enemiesKilled;

    // Data to carry to result scene
    public float RevivedTreePercent => totalTrees == 0 ? 0 : (revivedTrees / (float)totalTrees) * 100f;
    public int EnemiesKilled => enemiesKilled;
    public float TimeLeft => Mathf.Max(0, timer);

    void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        timer = gameDuration;
        CountTotalTrees();
    }

    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            timer = 0f;
            GoToResultScene();
        }
        UpdateRevivedTrees();
    }

    void CountTotalTrees()
    {
        // Assumes all trees have the TreeBehavior script
        totalTrees = FindObjectsOfType<TreeBehavior>().Length;
    }

    void UpdateRevivedTrees()
    {
        int count = 0;
        foreach (var tree in FindObjectsOfType<TreeBehavior>())
        {
            if (tree.health >= tree.reviveHealth)
                count++;
        }
        revivedTrees = count;
        Debug.Log(revivedTrees);
    }

    public void AddEnemyKill()
    {
        enemiesKilled++;
    }

    void GoToResultScene()
    {
        // Store data in a static class or use PlayerPrefs for cross-scene data
        ResultData.revivedTreePercent = RevivedTreePercent;
        ResultData.enemiesKilled = EnemiesKilled;
        SceneManager.LoadScene("ResultScene"); // Change to your result scene name
    }
}

// Helper static class to carry data between scenes
public static class ResultData
{
    public static float revivedTreePercent;
    public static int enemiesKilled;
}
