using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DeliveryButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private Player player;
    private Delivery delivery;

    void Start()
    {
        player = FindObjectOfType<Player>();
        delivery = FindObjectOfType<Delivery>();
    }

    // If the button is being pressed(pointer down) and the player is in the room, holding is set to true
    public void OnPointerDown(PointerEventData eventData)
    {
        if (player.canDeliver)
        {
            delivery.isHolding = true;
            player.canShoot = false;
        }
    }

    // If the button is let go of, the slider will reset
    public void OnPointerUp(PointerEventData eventData)
    {
        if (player.canDeliver)
        {
            delivery.buttonSlider.value = 0f;
            delivery.isHolding = false;
            player.canShoot = true;
        }
    }
}
