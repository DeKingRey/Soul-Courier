using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SoulCourier.Abilities;
using UnityEngine.UI;

/// <summary>
/// The wooden Hei Tiki increases player luck
/// It is a common variant of the hei tiki pounamu which is a powerful one shot item
/// The hei tiki symbolises luck and good fortune
/// </summary>
public class HeiTikiWood : MonoBehaviour, IUseAbility
{
    private Player player;
    public float luckIncrease;

    void Start()
    {
        player = FindObjectOfType<Player>();
    }

    public void Use()
    {
        // Increases player luck
        player.rangeMultiplier += luckIncrease; // Small increase like 0.2f
        Destroy(gameObject);
    }
}
