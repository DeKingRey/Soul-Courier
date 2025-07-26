using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackAnimChild : MonoBehaviour
{
    // This script will be to use functions from the parent within the model of an enemies animation events

    public Manaia manaia;
    public Porotai porotai;
    public PonaTuri ponaTuri;

    public void ThrowRock()
    {
        manaia.ThrowRock();
    }

    public void StartAttack()
    {
        if (manaia) manaia.StartAttack();
        if (porotai) porotai.StartAttack();
        if (ponaTuri) ponaTuri.StartAttack();
    }

    public void EndAttack()
    {
        if (porotai) porotai.EndAttack();
        if (ponaTuri) ponaTuri.EndAttack();
    }
}
