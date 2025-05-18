using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbScript : MonoBehaviour
{
    public enum OrbType { Damage, Heal }
    public OrbType orbType = OrbType.Damage;
    public int effectAmount = 1;
    public float lifeTime = 3f;

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    void OnCollisionEnter(Collision collision)
    {
        // Check for Tree
        TreeBehavior tree = collision.gameObject.GetComponent<TreeBehavior>();
        if (tree != null)
        {
            if (orbType == OrbType.Heal)
            {
                tree.ReceiveHeal(effectAmount);
            }
            if (orbType == OrbType.Damage)
            {
                tree.ReceiveDamage(effectAmount);
            }
            // Optionally, you could allow damage orbs to affect trees if needed
            Destroy(gameObject);
            return;
        }

        //// Check for Enemy
        //EnemyBehavior enemy = collision.gameObject.GetComponent<EnemyBehavior>();
        //if (enemy != null)
        //{
        //    if (orbType == OrbType.Damage)
        //    {
        //        enemy.ReceiveDamage(effectAmount);
        //    }
        //    // Optionally, you could allow heal orbs to affect enemies if needed
        //    Destroy(gameObject);
        //    return;
        //}
    }
}
