using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public GameObject[] exits;
    private List<UnityEngine.AI.NavMeshAgent> enemies = new List<UnityEngine.AI.NavMeshAgent>();
    private bool enemiesRemain = true;
    private bool currentRoom;

    void Start()
    {
        OpenDoors();

        foreach (Transform child in this.transform)
        {
            if (child.CompareTag("Enemy"))
            {
                var agent = child.GetComponent<UnityEngine.AI.NavMeshAgent>();
                enemies.Add(agent);
                agent.enabled = false;
            }
        }
    }

    void Update()
    {
        if (!currentRoom || !enemiesRemain) return;

        enemies.RemoveAll(enemy => enemy == null); // Removes all enemies that are null from the list(defeated enemies)
        if (enemies.Count == 0)
        {
            enemiesRemain = false;
            OpenDoors();
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

    void OnTriggerEnter(Collider obj)
    {
        if (obj.tag == "Player")
        {
            currentRoom = true;

            if (enemiesRemain)
            {
                foreach (UnityEngine.AI.NavMeshAgent enemy in enemies)
                {
                    enemy.enabled = true;
                }

                CloseDoors();
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
