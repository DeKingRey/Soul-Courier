using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SoulCourier.Abilities;

public class ShopManager : MonoBehaviour
{
    private Dictionary<int, GameObject> items = new Dictionary<int, GameObject>();

    public GameObject[] pickupItems;
    public AbilityDatabase abilityDatabase;
    public Transform[] spawnPositions;
    public RuntimeAnimatorController animatorController;

    void Start()
    {
        // Selects two random abilities and then one basic item(key, bomb, etc)
        for (int i = 0; i < 2; i++)
        {
            // Uses the luck multiplier to choose a random ability tier
            float luckMultiplier = FindObjectOfType<Player>().luckMultiplier;
            AbilityTier randomTier = AbilityTierHelper.GetRandomTier(luckMultiplier);

            // Selects a random ability with the chosen tier and spawns it
            var matchingAbilities = abilityDatabase.abilities.FindAll(a => a.tier == randomTier);
            int index = Random.Range(0, matchingAbilities.Count);
            AbilityDatabase.Ability chosenAbility = matchingAbilities[index];

            // Adds item prefab and price to the dict
            items.Add(i, chosenAbility.abilityPrefab);
        }
        GameObject chosenItem = pickupItems[Random.Range(0, pickupItems.Length)];
        items.Add(3, chosenItem);

        int j = 0;
        foreach (var pair in items)
        {
            GameObject prefab = pair.Value;
            
            GameObject item = Instantiate(prefab, spawnPositions[j].position, Quaternion.identity, spawnPositions[j]);
            item.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);
            ShopItem shopItem = item.GetComponent<ShopItem>();
            shopItem.enabled = true;
            shopItem.animatorController = animatorController;

            j++;
        }
    }
}
