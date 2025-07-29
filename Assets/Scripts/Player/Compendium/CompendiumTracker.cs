using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompendiumTracker : MonoBehaviour
{
    public static CompendiumTracker Instance;

    public CompendiumEntries entriesData;
    private Dictionary<string, CompendiumEntries.Entry> entriesDict;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Keeps instance across scenes
            InitializeDictionary();
            LoadCompendium();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void InitializeDictionary()
    {
        entriesDict = new Dictionary<string, CompendiumEntries.Entry>();
        foreach (var entry in entriesData.entries)
        {
            entriesDict[entry.id] = entry;
        }
    }

    public void UnlockEntry(string id)
    {
        // Tries to get id
        if (entriesDict.TryGetValue(id, out CompendiumEntries.Entry entry))
        {
            // Unlocks it and saves it as unlocked to player prefs
            if (!entry.unlocked)
            {
                entry.unlocked = true;
                PlayerPrefs.SetInt($"Compendium_{id}", 1);
                PlayerPrefs.Save();
            }
        }
        else
        {
            Debug.LogWarning($"No compendium entry for {id}");
        }
    }

    public bool IsEntryUnlocked(string id)
    {
        // Checks if the entry is unlocked
        return PlayerPrefs.GetInt($"Compendium_{id}", 0) == 1;
    }

    private void LoadCompendium()
    {
        // Loads all unlocked entries
        foreach (var entry in entriesData.entries)
        {
            entry.unlocked = IsEntryUnlocked(entry.id);
        }
    }
}
