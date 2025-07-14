using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    [Header("Quota")]
    public float quota;
    public float maxQuota;
    public Slider quotaSlider;
    public RectTransform targetQuota;
    private float currentVelocity = 0;

    private Player player;

    [Header("Items UI")]
    public TMP_Text stampText;
    public TMP_Text keyText;
    public TMP_Text bombText;

    private bool begin;

    [Header("Enemy")]
    public GameObject soul;
    public GameObject deathParticles;
    public GameObject ghost;

    void Start()
    {
        player = FindObjectOfType<Player>();
        StartCoroutine(BeginEnemyCount());

        Enemy.soul = soul;
        Enemy.deathParticles = deathParticles;
        Enemy.ghost = ghost;
    }

    void Update()
    {
        if (!begin) return;

        UpdateItems();
    }

    IEnumerator BeginEnemyCount()
    {
        while (GameObject.FindGameObjectsWithTag("Enemy").Length == 0)
        {
            yield return null; // Waits until enemies are spawned
        }

        yield return null; // Waits one extra frame

        begin = true;
        EnemyCount();
    }

    public void EnemyCount()
    {
        // Just uses var as a temporary variable(the variable is an array)
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        // Sets the quota to the amount of enemies
        quota = enemies.Length;

        // Loops through each enemy and adds their soul value to the max quota
        foreach (var enemy in enemies)
        {
            maxQuota += enemy.GetComponent<Enemy>().value;
        }

        float targetValue = quota / maxQuota; // gets the target value, e.g. if the target quota is 50 and the max value is 100, then the target value is 0.5 // quota / quotaSlider.maxValue
        float sliderHeight = quotaSlider.GetComponent<RectTransform>().rect.height; // The slider goes bottom to top, so this gets the height of it
        targetQuota.anchoredPosition = new Vector2(targetQuota.anchoredPosition.x, targetValue * sliderHeight); // Keeps the x position the same, and calculates the y position by multiplying the target value by the slider height

        // targetValue = 0.5, sliderHeight = 200, 0.5 * 200 = 100
    }

    void UpdateItems()
    {
        float currentValue = Mathf.SmoothDamp(quotaSlider.value, player.souls / maxQuota, ref currentVelocity, 100 * Time.deltaTime);
        quotaSlider.value = currentValue;
        stampText.text = player.stamps.ToString();
        keyText.text = player.keys.ToString();
    }
}
