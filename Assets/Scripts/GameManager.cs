using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public float quota;
    public float maxQuota;
    public Slider quotaSlider;
    public RectTransform targetQuota;

    private Player player;

    public TMP_Text stampText;
    public TMP_Text keyText;
    public TMP_Text bombText;

    private bool begin;

    void Start()
    {
        player = FindObjectOfType<Player>();
        StartCoroutine(BeginEnemyCount());
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

        float targetValue = quota / quotaSlider.maxValue;
        float sliderWidth = quotaSlider.fillRect.rect.width;
        targetQuota.anchoredPosition = new Vector2(targetValue * sliderWidth, targetQuota.anchoredPosition.y);
    }

    void UpdateItems()
    {
        quotaSlider.value = player.souls / maxQuota;
        stampText.text = player.stamps.ToString();
        keyText.text = player.keys.ToString();
    }
}
