using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SoulCourier.Abilities;
using UnityEngine.UI;

/// <summary>
/// Silver fern is a passive ability that increases players stamp(money) collection
/// </summary>
public class SilverFern : MonoBehaviour, IUseAbility
{
    private Player player;
    public float stampIncrease;

    void Start()
    {
        player = FindObjectOfType<Player>();
    }

    public void Use()
    {
        // Increases player stamp collection
        player.stampMultiplier += stampIncrease; // Small increase like 0.2f
        Destroy(gameObject);
    }
}
