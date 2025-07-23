using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SoulCourier.Abilities;
using UnityEngine.UI;

/// <summary>
/// Kaiaia Feather will increase fire rate
/// This is because it is a feather of a falcon which is a fast hunter
/// </summary>
public class KaiaiaFeather : MonoBehaviour, IUseAbility
{
    private Player player;
    public float firerateIncrease;

    void Start()
    {
        player = FindObjectOfType<Player>();
    }

    public void Use()
    {
        // Increases player fire rate
        player.firerateMultiplier += firerateIncrease; // Small increase like 0.2f
        Destroy(gameObject);
    }
}
