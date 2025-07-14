using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationChild : MonoBehaviour
{
    // This script will be to use functions from the parent within the model of an enemies animation events

    public RangedEnemy manaia;

    public void ThrowRock()
    {
        manaia.ThrowRock();
    }

    public void StartAttack()
    {
        manaia.StartAttack();
    }
}
