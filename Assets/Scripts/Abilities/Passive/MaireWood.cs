using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SoulCourier.Abilities;
using UnityEngine.UI;

/// <summary>
/// The maire wood increases damage
/// Maire wood was used to make weapons
/// </summary>
public class MaireWood : MonoBehaviour, IUseAbility
{
    private Player player;
    public float damageIncrease;

    void Start()
    {
        player = FindObjectOfType<Player>();
    }

    public void Use()
    {
        // Increases player damage
        player = FindObjectOfType<Player>();
        player.damageMultiplier += damageIncrease; // Small increase like 0.2f
        Destroy(gameObject);
    }
}
