using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationChild : MonoBehaviour
{
    // This script will be to use functions from the parent within the model of an enemies animation events

    public Manaia manaia;
    public Porotai porotai;

    public void ThrowRock()
    {
        manaia.ThrowRock();
    }

    public void StartAttack()
    {
        if (manaia) manaia.StartAttack();
        if (porotai) porotai.StartAttack();
    }

    public void EndAttack()
    {
        if (porotai) porotai.EndAttack();
    }
}
