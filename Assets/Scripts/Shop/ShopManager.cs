using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SoulCourier.Abilities;

public class ShopManager : MonoBehaviour
{
    // Dictionary
    public class ShopItem
    {
        public GameObject prefab;
        public int price;
    }
    private Dictionary<int, ShopItem> items = new Dictionary<int, ShopItem>();

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
            items.Add(i, new ShopItem { prefab = chosenAbility.abilityPrefab, price = chosenAbility.price});
        }
        GameObject chosenItem = pickupItems[Random.Range(0, pickupItems.Length)];
        items.Add(3, new ShopItem { prefab = chosenItem, price = chosenItem.GetComponentInChildren<Pickup>().price });

        int j = 0;
        foreach (var pair in items)
        {
            GameObject prefab = pair.Value.prefab;
            
            GameObject item = Instantiate(prefab, spawnPositions[j].position, Quaternion.identity, spawnPositions[j]);
            item.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);

            // Disables colliders and item magnets
            item.GetComponent<BoxCollider>().isTrigger = false; 
            foreach (Transform child in item.transform)
            {
                BoxCollider collider = child.GetComponent<BoxCollider>();

                if (collider != null && collider.isTrigger)
                {
                    collider.isTrigger = false;
                }
            }
            Pickup pickup = item.GetComponentInChildren<Pickup>();
            if (pickup != null)
            {
                pickup.speed = 0;
            }
            Rigidbody rb = item.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = true;
            }

            // Adds animation to items that don't already have the default animation
            GameObject animatorChild = item.transform.GetChild(0).gameObject;
            Animator animator = animatorChild.GetComponent<Animator>();
            if (animator == null)
            {
                animator = animatorChild.AddComponent<Animator>();
                animator.runtimeAnimatorController = animatorController;
            }

            // Adds layer and outline 
            item.layer = LayerMask.NameToLayer("Shop Item");
            Outline outline = item.AddComponent<Outline>();
            outline.OutlineWidth = 0;

            j++;
        }
    }
}
