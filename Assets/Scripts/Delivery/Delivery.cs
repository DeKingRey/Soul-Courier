using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Delivery : MonoBehaviour
{
    private Player player;
    private GameManager gm;

    private float deliveredSouls = 0;
    private float currentSouls;

    public Slider buttonSlider;
    public bool isHolding;
    public float waitSpeed;

    public CanvasGroup deliveryUI;
    public float fadeSpeed;

    private GameObject exitDoor;

    public Slider deliverySlider;
    public RectTransform targetDelivery;

    public GameObject stampPrefab;
    public float itemChance;
    private Transform spawnPos;
    public float upwardForce;

    void Start()
    {
        player = FindObjectOfType<Player>();
        gm = FindObjectOfType<GameManager>();

        SetQuota();
    }

    void Update()
    {
        // Sets the target alpha for the canvas group(delivery UI elements)
        float targetAlpha;

        // Checks if the player is within the area, and if so sets target alpha to 1, if not it is 0
        if (player.canDeliver)
        {
            targetAlpha = 1f;
            deliveryUI.gameObject.SetActive(true);

            Cursor.lockState = CursorLockMode.None; // Enables cursor within the screen
            Cursor.visible = true;
            exitDoor = GameObject.FindWithTag("ExitDoors");
            spawnPos = GameObject.FindWithTag("Reward").transform;
        }
        else
        {
            targetAlpha = 0f;
            deliveryUI.gameObject.SetActive(false);

            if (Time.timeScale != 0)
            {
                Cursor.lockState = CursorLockMode.Locked; // Disables cursor
                Cursor.visible = false;
            }   
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
        currentSouls = 0;
        currentSouls += player.souls;
        player.souls = 0;

        deliverySlider.value = deliveredSouls / gm.maxQuota;

        if (deliveredSouls >= gm.quota)
        {
            foreach (Transform door in exitDoor.transform)
            {
                door.gameObject.SetActive(true);
            }

            for (int i = 0; i < currentSouls; i++)
            {
                float chance = Random.Range(0, 100);
                
                if (chance <= itemChance / i + 1)
                {
                    Rigidbody stamp = Instantiate(stampPrefab, new Vector3(spawnPos.position.x, spawnPos.position.y + 3, spawnPos.position.z), Quaternion.identity).GetComponent<Rigidbody>();
                    stamp.AddForce(Random.Range(-3f, 3f), upwardForce, Random.Range(-3f, 3f), ForceMode.Impulse);
                }
            }
        }
    }

    void SetQuota()
    {
        float targetValue = gm.quota / deliverySlider.maxValue;
        float sliderWidth = deliverySlider.fillRect.rect.width;
        targetDelivery.anchoredPosition = new Vector2(targetValue * sliderWidth, targetDelivery.anchoredPosition.y);
    }
}
