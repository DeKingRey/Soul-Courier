using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public GameObject[] exits;
    private List<NavMeshAgent> enemies = new List<NavMeshAgent>();
    private bool enemiesRemain = true;
    private bool currentRoom;

    void Start()
    {
        OpenDoors();

        foreach (Transform child in this.transform)
        {
            if (child.CompareTag("Enemy"))
            {
                enemies.Add(child.gameObject.GetComponent<NavMeshAgent>());
                child.gameObject.SetActive(false);
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
                foreach (GameObject enemy in enemies)
                {
                    enemy.SetActive(true);
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
