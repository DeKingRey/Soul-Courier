using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SoulCourier.Abilities;
using UnityEngine.UI;

/// <summary>
/// Ti Kouka Fruit is a passive ability that increases players range
/// The Ti Kouka was used to navigate
/// </summary>
public class TiKoukaFruit : MonoBehaviour, IUseAbility
{
    private Player player;
    public float rangeIncrease;

    void Start()
    {
        player = FindObjectOfType<Player>();
    }

    public void Use()
    {
        // Increases player range
        player = FindObjectOfType<Player>();
        player.rangeMultiplier += rangeIncrease; // Small increase like 0.2f
        Destroy(gameObject);
    }
}
