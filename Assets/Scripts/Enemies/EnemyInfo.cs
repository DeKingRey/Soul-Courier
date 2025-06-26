using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "New Enemy Database")]
public class EnemyInfo : ScriptableObject
{
    [System.Serializable]
    public class Enemy {
        public GameObject enemyPrefab;
        public int difficulty; // A number like 1-3, and each room may spawn a certain amount of enemies of difficulties, e.g. 3 level 1's 1 level 3 etc
    }
    public List<Enemy> enemies;    
}
