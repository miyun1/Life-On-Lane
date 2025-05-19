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
        EnemyAi enemy = collision.gameObject.GetComponent<EnemyAi>();

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
            Destroy(gameObject);
            return;
        }

        if (enemy != null)
        {
            if (orbType == OrbType.Heal)
            {
                enemy.ReceiveHeal(effectAmount);
            }
            if (orbType == OrbType.Damage)
            {
                enemy.ReceiveDamage(effectAmount);
            }
            Destroy(gameObject);
            return;
        }
    }
}
