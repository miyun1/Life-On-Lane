using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAi : MonoBehaviour
{
    public float moveSpeed = 4f;
    public float attackRange = 1.5f;
    public float attackCooldown = 1.5f;
    public int health = 1;

    private Rigidbody rb;
    private Animator animator;
    private Transform player;
    private TreeBehavior targetTree;
    private float lastAttackTime = -999f;
    private bool isDead = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    void FixedUpdate()
    {
        if (isDead)
        {
            rb.velocity = Vector3.zero;
            return;
        }

        // If we have a target tree, check if it's still revived
        if (targetTree == null || targetTree.health < targetTree.reviveHealth)
        {
            targetTree = FindNearestRevivedTree();
        }

        Transform target = null;
        if (targetTree != null)
        {
            target = targetTree.transform;
        }
        else if (player != null)
        {
            target = player;
        }

        if (target != null)
        {
            float dist = Vector3.Distance(transform.position, target.position);

            if (dist > attackRange)
            {
                // Move towards target
                Vector3 dir = (target.position - transform.position).normalized;
                rb.velocity = new Vector3(dir.x, rb.velocity.y, dir.z) * moveSpeed;
                animator.SetBool("isRunning", true);
                animator.SetBool("isIdle", false);

                // Face the target
                if (dir != Vector3.zero)
                {
                    Quaternion lookRot = Quaternion.LookRotation(new Vector3(dir.x, 0, dir.z));
                    transform.rotation = Quaternion.Slerp(transform.rotation, lookRot, 0.2f);
                }
            }
            else
            {
                rb.velocity = new Vector3(0, rb.velocity.y, 0);
                animator.SetBool("isRunning", false);
                animator.SetBool("isIdle", true);

                if (Time.time - lastAttackTime > attackCooldown)
                {
                    animator.SetTrigger("Attack");
                    lastAttackTime = Time.time;

                    // Apply attack effect
                    if (targetTree != null && targetTree.health >= targetTree.reviveHealth)
                    {
                        targetTree.ReceiveDamage(1);
                    }
                    // If attacking player, implement player damage logic here
                }
            }
        }
        else
        {
            rb.velocity = new Vector3(0, rb.velocity.y, 0);
            animator.SetBool("isRunning", false);
            animator.SetBool("isIdle", true);
        }
    }

    TreeBehavior FindNearestRevivedTree()
    {
        TreeBehavior[] trees = FindObjectsOfType<TreeBehavior>();
        TreeBehavior nearest = null;
        float minDist = float.MaxValue;
        foreach (var tree in trees)
        {
            if (tree != null && tree.health >= tree.reviveHealth)
            {
                float dist = Vector3.Distance(transform.position, tree.transform.position);
                if (dist < minDist)
                {
                    minDist = dist;
                    nearest = tree;
                }
            }
        }
        return nearest;
    }

    public void ReceiveDamage(int amount)
    {
        if (isDead) return;
        health -= amount;
        if (health <= 0)
        {
            Die();
        }
    }

    public void ReceiveHeal(int amount)
    {
        if (isDead) return;
        health += amount;
    }

    void Die()
    {
        isDead = true;
        rb.velocity = Vector3.zero;
        animator.SetTrigger("Death");
        Destroy(gameObject, 2f); // Destroy after death animation
    }
}
