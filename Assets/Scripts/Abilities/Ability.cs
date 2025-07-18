using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SoulCourier.Abilities;
using UnityEngine.UI;

public class Ability : MonoBehaviour
{
    public AbilityType type;
    public Image icon;
    public IUseAbility abilityLogic;

    void Start()
    {
        abilityLogic = GetComponent<IUseAbility>();
    }

    void Update()
    {
        if (type == AbilityType.Passive) UpdatePassive();
        if (type == AbilityType.Active) UpdateActive();
        if (type == AbilityType.OneShot) UpdateOneShot();
    }

    void UpdatePassive()
    {
        abilityLogic.Use(); // Will constantly be used
    }

    void UpdateActive()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            abilityLogic.Use(); // Will start a countdown in the use too
        }
    }

    void UpdateOneShot()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            abilityLogic.Use(); // Will destroy the ability in the use function
        }
    }
}
