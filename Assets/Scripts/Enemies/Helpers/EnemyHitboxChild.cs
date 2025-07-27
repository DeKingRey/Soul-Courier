using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHitboxChild : MonoBehaviour
{
    private Enemy enemy;

    void Start()
    {
        enemy = GetComponentInParent<Enemy>();
    }

    void OnTriggerEnter(Collider obj)
    {
        if (enemy)
        {
            if (obj.tag == "Bullet" && enemy.defence > 0)
            {
                Bullet bullet = obj.GetComponent<Bullet>();
                enemy.TakeDamage(bullet.damage, bullet);
                Destroy(bullet.gameObject);
            }
        }
    }
}
