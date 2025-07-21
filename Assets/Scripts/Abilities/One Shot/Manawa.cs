using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SoulCourier.Abilities;
using UnityEngine.UI;

/// <summary>
/// Manawa Pounamu is a one shot ability that adds a temporary heart to the player
/// Note the Manawa Pounamu is a case in which it is a one shot ability but also passive, it'll be put under the one shot category however as it is a pounamu
/// </summary>
public class Manawa : MonoBehaviour, IUseAbility
{
    private Player player;
    public Sprite manawaHeartUI;

    void Start()
    {
        player = FindObjectOfType<Player>();
    }

    public void Use()
    {
        // Implements temporary health benefits
        player.health++;
        player.tempHearts++;
        player.UpdateHealth(1f, manawaHeartUI, true);
        Destroy(gameObject);
    }
}
