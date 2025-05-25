using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Delivery : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private Player player;

    private float deliveredSouls = 0;

    public Slider buttonSlider;
    private bool isHolding;
    public float waitSpeed;

    public CanvasGroup deliveryUI;
    public float fadeSpeed;

    void Start()
    {
        player = FindObjectOfType<Player>();
    }

    void Update()
    {
        // Sets the target alpha for the canvas group(delivery UI elements)
        float targetAlpha;

        // Checks if the player is within the area, and if so sets target alpha to 1, if not it is 0
        if (player.canDeliver)
        {
            targetAlpha = 1f;
        }
        else
        {
            targetAlpha = 0f;
        }

        // Smoothly increments UI alpha to the target alpha at a speed
        deliveryUI.alpha = Mathf.MoveTowards(deliveryUI.alpha, targetAlpha, fadeSpeed * Time.deltaTime);

        // If holding the button, the sliders value will increment
        if (isHolding)
        {
            buttonSlider.value += (waitSpeed * Time.deltaTime);

            // If the slider reaches 1(full) it delivers souls and resets variables
            if (buttonSlider.value == 1f)
            {
                buttonSlider.value = 0f;
                isHolding = false;
                DeliverSouls();
            }
        }
    }

    void DeliverSouls()
    {
        // Adds player souls to delivered souls, then removes all players collected souls
        deliveredSouls += player.souls;
        player.souls = 0;
    }

    // If the button is being pressed(pointer down) and the player is in the room, holding is set to true
    public void OnPointerDown(PointerEventData eventData)
    {
        if (player.canDeliver)
        {
            isHolding = true;
            player.canShoot = false;
        }
    }

    // If the button is let go of, the slider will reset
    public void OnPointerUp(PointerEventData eventData)
    {
        if (player.canDeliver)
        {
            buttonSlider.value = 0f;
            isHolding = false;
            player.canShoot = true;
        }
    }
}
