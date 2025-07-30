using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "New Compendium Entries")]
public class CompendiumEntries : ScriptableObject
{
    [System.Serializable]
    public class Entry
    {
        public string id;
        public CompendiumCategory category;
        public string displayName;
        public Sprite icon;
        [TextArea]
        public string description;
        public bool unlocked;
    }
    public List<Entry> entries;
}
