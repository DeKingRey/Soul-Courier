using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SoulCourier.Abilities;
using UnityEngine.UI;

/// <summary>
/// Manawa Pounamu is a one shot ability that adds a temporary heart to the player
/// Note the Manawa Pounamu is a case in which it is a one shot ability but also passive, it'll be put under the one shot category however as it is a pounamu
/// This script is also used for the normal manawa
/// </summary>
public class Manawa : MonoBehaviour, IUseAbility
{
    private Player player;
    public Sprite manawaHeartUI;
    public float healAmount;
    public bool isTemp;

    void Start()
    {
        player = FindObjectOfType<Player>();
    }

    public void Use()
    {
        // Nothing in here
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Implements health benefits
            if (isTemp)
            {
                player.tempHearts++;
                player.UpdateHealth(healAmount, manawaHeartUI, isTemp);
                player.health += healAmount;
                Destroy(gameObject);
            }
            else
            {
                // Adds health if it isn't a temp heart
                if (player.health >= player.maxHealth)
                {
                    player.TakeDamage(-healAmount);
                    Destroy(gameObject);
                }    
            }
        }
    }
}
