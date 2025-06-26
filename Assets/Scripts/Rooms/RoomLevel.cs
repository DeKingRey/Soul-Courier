using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "New Level")]
public class RoomLevel : ScriptableObject
{
    public int enemyAmount;
    public List<int> enemyDifficultyRatio; // List of ratios of enemies, e.g. level 1, 2, 3 enemies 3:1:2

    public List<int> GetDifficulties()
    {
        List <int> difficulties = new List<int>();

        // Adds the weights in the ratio together
        int total = 0;
        foreach (int weight in enemyDifficultyRatio)
        {
            total += weight;
        }

        // Gets the share of each ratio part
        int share = Mathf.FloorToInt(enemyAmount / (float)total); // 12 / 6
        
        int tier = 0;
        foreach (int weight in enemyDifficultyRatio) // Loops through the weights
        {
            tier++; // Gets the current tier of the weight
            for (int i = 0; i < share * weight; i++) // Loops the amount of times of the shares
            {
                difficulties.Add(tier); // e.g. total = 6, total = 12, 2 * 3 = 6, 2 * 2 = 4, 2 * 1 = 2
            }
        }

        while (difficulties.Count < enemyAmount)
        {
            difficulties.Add(1);
        }

        return difficulties;
    }
}
