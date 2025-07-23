using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SoulCourier.Abilities;
using UnityEngine.UI;

/// <summary>
/// The Hei Tuna will increase speed
/// A hei tuna is a like a necklace of an eel
/// It will be a token, not an actual eel
/// </summary>
public class HeiTuna : MonoBehaviour, IUseAbility
{
    private Player player;
    public float speedIncrease;

    void Start()
    {
        player = FindObjectOfType<Player>();
    }

    public void Use()
    {
        // Increases player speed
        player.speedMultiplier += speedIncrease; // Small increase like 0.2f
        Destroy(gameObject);
    }
}
