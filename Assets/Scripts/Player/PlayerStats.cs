using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats Instance { get; private set; }
    public List<string> abilityIDs = new List<string>();
    public AbilityDatabase abilityDatabase;
    private Player player;
    public float health;
    public float stamps;
    public float bombs;
    public float keys;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        if (player == null)
        {
            player = FindObjectOfType<Player>();
        }
    }

    public void UpdateStats()
    {
        stamps = player.stamps;
        keys = player.keys;
        bombs = player.bombs;
        health = player.health;
    }

    public void LoadStats(Player playerScript)
    {
        player = playerScript;

        player.stamps = stamps;
        player.keys = keys;
        player.bombs = bombs;
        player.health = health;
        player.TakeDamage(0);

        foreach (string id in abilityIDs)
        {
            Debug.Log(id);
            AbilityDatabase.Ability abilityData =
                abilityDatabase.abilities.Find(a => a.abilityPrefab.GetComponent<UseAbility>().id == id);

            GameObject abilityObj = Instantiate(abilityData.abilityPrefab);
            abilityObj.GetComponent<UseAbility>().PickUpAbility(true);
        }
    }

    public void ResetStats()
    {
        stamps = 0;
        keys = 0;
        bombs = 0;
        health = 3;

        abilityIDs = new List<string>();
    }
}
