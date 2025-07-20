using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SoulCourier.Abilities;

public class SpawnAbility : MonoBehaviour
{
    public AbilityDatabase abilityDatabase;
    public Transform spawnPosition;

    void Start()
    {
        // Uses the luck multiplier to choose a random ability tier
        float luckMultiplier = FindObjectOfType<Player>().luck;
        AbilityTier randomTier = AbilityTierHelper.GetRandomTier(luckMultiplier);
        TestAbilityTiers(10);

        // Selects a random ability with the chosen tier and spawns it
        var matchingAbilities = abilityDatabase.abilities.FindAll(a => a.tier == randomTier);
        int index = Random.Range(0, matchingAbilities.Count);
        GameObject chosenAbility = matchingAbilities[index].abilityPrefab;
        Instantiate(chosenAbility, spawnPosition.position, Quaternion.identity);
    }

    void TestAbilityTiers(int count)
    {
        for (int i = 0; i < count; i++)
        {
            Debug.Log($"Random Tier Selected: {AbilityTierHelper.GetRandomTier(1)}");
        }
    }
}
