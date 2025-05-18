using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeBehavior : MonoBehaviour
{
    [Header("Tree Meshes")]
    public GameObject deadTreeMesh;
    public GameObject revivedTreeMesh;

    [Header("Health Settings")]
    public int health = 0;
    public int reviveHealth = 3;

    [Header("Shake Settings")]
    public float shakeDuration = 0.2f;
    public float shakeMagnitude = 0.2f;

    private bool isRevived = false;
    private Vector3 originalPosition;

    void Start()
    {
        originalPosition = transform.localPosition;
        UpdateTreeVisual();
    }

    // Call this when the tree is hit by a heal orb
    public void ReceiveHeal(int amount)
    {
        if (isRevived) return;

        health += amount;
        if (health >= reviveHealth)
        {
            health = reviveHealth;
            isRevived = true;
            UpdateTreeVisual();
        }
        StartCoroutine(Shake());
    }

    public void ReceiveDamage(int amount)
    {
        if (!isRevived) return;

        health -= amount;
        if (health <= 0)
        {
            health = 0;
            isRevived = false;
            UpdateTreeVisual();
        }
        StartCoroutine(Shake());
    }

    void UpdateTreeVisual()
    {
        deadTreeMesh.SetActive(!isRevived);
        revivedTreeMesh.SetActive(isRevived);
    }

    IEnumerator Shake()
    {
        float elapsed = 0f;
        while (elapsed < shakeDuration)
        {
            Vector3 randomPoint = originalPosition + Random.insideUnitSphere * shakeMagnitude;
            transform.localPosition = new Vector3(randomPoint.x, originalPosition.y, randomPoint.z);
            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.localPosition = originalPosition;
    }
}
