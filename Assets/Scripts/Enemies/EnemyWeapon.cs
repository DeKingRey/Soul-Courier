using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeapon : MonoBehaviour
{
    private bool canDamage;
    private bool hasDamaged;

    //public float weaponLength;
    public float damage;

    void OnTriggerEnter(Collider obj)
    {
        if (obj.CompareTag("Player") && canDamage && !hasDamaged)
        {
            obj.GetComponent<Player>().TakeDamage(damage);
            hasDamaged = true;
        }
    }

    public void StartAttack()
    {
        canDamage = true;
        hasDamaged = false;
    }

    public void EndAttack()
    {
        canDamage = false;
    }
}
