using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SoulCourier.Abilities;
using UnityEngine.UI;

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
        return;
    }

    void OnTriggerEnter(Collider obj)
    {
        if (obj.CompareTag("Player"))
        {
            player.health++;
            player.tempHearts++;
            player.UpdateHealth(1f, manawaHeartUI, true);
            Destroy(gameObject);
        }
    }
}
