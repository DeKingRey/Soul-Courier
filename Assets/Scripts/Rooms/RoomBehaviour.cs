using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomBehaviour : MonoBehaviour
{
    public GameObject[] exits;

    [Header("Enemies")]
    public RoomLevel level;
    public EnemyInfo enemyDatabase;

    private List<UnityEngine.AI.NavMeshAgent> enemies = new List<UnityEngine.AI.NavMeshAgent>();
    private bool enemiesRemain = true;
    private bool currentRoom;

    public Vector2Int roomSize = new Vector2Int(4, 4);
    
    private Minimap minimap;
    private SpriteRenderer[] minimapSprites;
    private bool entered;

    private List<Transform> spawnpoints = new List<Transform>();
    private int spawnpointIndex = 0;

    [Header("Rewards")]
    public float rewardChance;
    public GameObject[] rewards;
    public Transform rewardSpawn;

    void Start()
    {
        exits = GetComponent<RoomDoors>().activeDoors.ToArray();

        foreach (Transform child in GetComponentsInChildren<Transform>(true))
        {
            if (child.CompareTag("Spawnpoint"))
            {
                spawnpoints.Add(child);
            }
        }

        OpenDoors();
        SpawnEnemies();       
    }

    void Update()
    {
        if (minimap == null)
        {
            minimap = GameObject.FindGameObjectWithTag("Minimap").GetComponent<Minimap>();
            minimapSprites = GetComponentsInChildren<SpriteRenderer>(true); // The true parameter gets all sprite renderers(despite active)
        }

        if (!currentRoom || !enemiesRemain) return;

        enemies.RemoveAll(enemy => enemy == null); // Removes all enemies that are null from the list(defeated enemies)
        if (enemies.Count == 0)
        {
            enemiesRemain = false;

            GrantRewards();
            OpenDoors();
        }
    }

    void SpawnEnemies()
    {
        // Spawn the amount of enemies, and the difficulty level ratios of enemies'
        List<int> enemyTiers = level.GetDifficulties();
        
        foreach (int tier in enemyTiers)
        {
            List<GameObject> availableEnemies = new List<GameObject>(); // Makes a new list of enemies for each tier
            foreach (EnemyInfo.Enemy enemy in enemyDatabase.enemies)    
            {
                if (enemy.difficulty == tier)
                {
                    availableEnemies.Add(enemy.enemyPrefab); // Loops through all enemies and adds the prefabs of the enemies that match the current tier of difficulty
                }
            }

            if (availableEnemies.Count > 0)
            {
                GameObject enemyPrefab = availableEnemies[Random.Range(0, availableEnemies.Count)]; // Gets a random available enemy
                
                Transform spawnpoint = spawnpoints[spawnpointIndex];
                spawnpointIndex++;
                GameObject spawnedEnemy = Instantiate(enemyPrefab, spawnpoint.position, Quaternion.identity);

                // Gives the Pona-Turi the rooms spawnpoints
                PonaTuri ponaTuri = spawnedEnemy.GetComponent<PonaTuri>();
                if (ponaTuri != null)
                {
                    ponaTuri.SetSpawnPoints(spawnpoints);
                }

                var agent = spawnedEnemy.GetComponent<UnityEngine.AI.NavMeshAgent>(); // Gets the agent of the spawned enemy
                agent.enabled = false;
                agent.gameObject.transform.GetChild(0).gameObject.SetActive(false);
                enemies.Add(agent);
            }
        }
    }

    void OpenDoors()
    {
        foreach (GameObject exit in exits)
        {
            exit.SetActive(false);
        }
    }

    void CloseDoors()
    {
        foreach (GameObject exit in exits)
        {
            exit.SetActive(true);
        }
    }

    void GrantRewards()
    {
        float randomChance = Random.Range(0, 1f);
        Player player = FindObjectOfType<Player>();

        if (randomChance <= rewardChance * player.luckMultiplier)
        {
            Instantiate(rewards[Random.Range(0, rewards.Length - 1)], rewardSpawn.position, Quaternion.identity);
        }
    }

    void OnTriggerEnter(Collider obj)
    {
        if (obj.tag == "Player")
        {
            currentRoom = true;

            // Enables enemies in the room and closes doors to the room
            if (enemiesRemain)
            {
                foreach (UnityEngine.AI.NavMeshAgent enemy in enemies)
                {
                    if (enemy == null || enemy.gameObject == null) continue; // Skips the loop iteration if the enemy has been killed
                    enemy.enabled = true;
                    enemy.gameObject.transform.GetChild(0).gameObject.SetActive(true);
                }
                CloseDoors();
            }

            if (!entered)
            {
                entered = true;
                minimap.EnableRoom(minimapSprites);
            }
        }
    }

    void OnTriggerExit(Collider obj)
    {
        if (obj.tag == "Player")
        {
            currentRoom = false;
        }
    }
}
