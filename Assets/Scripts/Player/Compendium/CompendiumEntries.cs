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
        public GameObject page;
        [TextArea]
        public string description;
        [HideInInspector] public bool unlocked;
    }
    public List<Entry> entries;
}
