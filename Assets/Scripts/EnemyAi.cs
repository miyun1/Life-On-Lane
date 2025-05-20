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
    private TreeBehavior targetTree;
    private float lastAttackTime = -999f;
    private bool isDead = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
        SetIdle();
    }

    void FixedUpdate()
    {
        if (isDead)
        {
            rb.velocity = Vector3.zero;
            return;
        }

        // If the current target tree is dead, reset to idle and clear target
        if (targetTree != null && targetTree.health <= 0)
        {
            targetTree = null;
            SetIdle();
        }

        // Only find a new target if we don't have one
        if (targetTree == null)
        {
            targetTree = FindNearestFullyRevivedTree();
        }

        // If we have a target, keep attacking until it's dead
        if (targetTree != null && targetTree.health > 0)
        {
            float dist = Vector3.Distance(transform.position, targetTree.transform.position);

            if (dist > attackRange)
            {
                // Running towards tree
                Vector3 dir = (targetTree.transform.position - transform.position).normalized;
                rb.velocity = new Vector3(dir.x, dir.y, dir.z) * moveSpeed;
                SetRunning();

                // Face the target tree
                if (dir != Vector3.zero)
                {
                    Quaternion lookRot = Quaternion.LookRotation(new Vector3(dir.x, dir.y, dir.z));
                    transform.rotation = Quaternion.Slerp(transform.rotation, lookRot, 0.2f);
                }
            }
            else
            {
                // In melee stance
                rb.velocity = new Vector3(0, rb.velocity.y, 0);
                SetMeleeStance();

                if (Time.time - lastAttackTime > attackCooldown)
                {
                    animator.SetTrigger("Attack");
                    lastAttackTime = Time.time;

                    // Apply attack effect
                    if (targetTree != null && targetTree.health > 0)
                    {
                        targetTree.ReceiveDamage(1);
                    }
                }
            }
        }
        else
        {
            // No valid tree, idle
            rb.velocity = new Vector3(0, rb.velocity.y, 0);
            SetIdle();
        }
    }

    TreeBehavior FindNearestFullyRevivedTree()
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
        //rb.velocity = Vector3.zero;
        animator.SetTrigger("Death");
        Destroy(gameObject, 2f); // Destroy after death animation
    }

    // Animation helpers
    void SetIdle()
    {
        animator.SetBool("isIdle", true);
        animator.SetBool("isRunning", false);
        animator.SetBool("isAttacking", false);
    }

    void SetRunning()
    {
        animator.SetBool("isIdle", false);
        animator.SetBool("isRunning", true);
        animator.SetBool("isAttacking", false);
    }

    void SetMeleeStance()
    {
        animator.SetBool("isIdle", false);
        animator.SetBool("isRunning", false);
        animator.SetBool("isAttacking", true);
    }
}
