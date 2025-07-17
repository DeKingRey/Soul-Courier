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
        public int tier; // A number like 1-5 which will determine the value and rarity of the ability
        [SerializeField]
        public AbilityType type; // One-shot, Active, or Passive
    }
    public List<Ability> abilities;    
}
