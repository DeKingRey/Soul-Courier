using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeapon : MonoBehaviour
{
    private bool canDamage;
    private bool hasDamaged;

    public float weaponLength;
    public float damage;

    void Update()
    {
        if (canDamage && !hasDamaged)
        {
            RaycastHit hit;

            if (Physics.Raycast(transform.position, -transform.up, out hit, weaponLength))
            {
                if (hit.transform.TryGetComponent(out Player player))
                {
                    player.TakeDamage(damage);
                    hasDamaged = true;
                }
            }
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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position - transform.up * weaponLength);
    }
}
