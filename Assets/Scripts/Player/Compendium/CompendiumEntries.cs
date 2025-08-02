using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CompendiumCategory
{
    Enemy,
    Ability,
    Weapon
}

[CreateAssetMenu(menuName = "New Compendium Entries")]
public class CompendiumEntries : ScriptableObject
{
    [System.Serializable]
    public class Entry
    {
        public string id;
        public CompendiumCategory category;
        public bool unlocked;
    }
    public List<Entry> entries;
}
