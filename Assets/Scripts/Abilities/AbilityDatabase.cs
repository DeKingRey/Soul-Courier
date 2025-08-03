using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SoulCourier.Abilities;

[CreateAssetMenu(menuName = "New Ability Database")]
public class AbilityDatabase : ScriptableObject
{
    [System.Serializable]
    public class Ability
    {
        public GameObject abilityPrefab;

        [SerializeField]
        public AbilityTier tier; // A tier from the ability types, D to S

        [SerializeField]
        public AbilityType type; // One-shot, Active, or Passive
        
        [SerializeField]
        public int price; // Shop price
    }
    public List<Ability> abilities;    
}
