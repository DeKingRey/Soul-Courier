using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RoomBehaviour : MonoBehaviour
{
    [System.Serializable]
    public class WallRow
    {
        public List<GameObject> row;
    }
    public List<WallRow> walls; // 0 - Up, 1 - Down, 2 - Right, 3 - Left


    [System.Serializable]
    public class DoorRow
    {
        public List<GameObject> row;
    }
    public List<DoorRow> doors;

    public List<GameObject> activeDoors;

    public void UpdateRoom(bool[][] status)
    {
        for (int i = 0; i < status.Length; i++)
        {
            for (int j = 0; j < status[i].Length; j++)
            {
                doors[i].row[j].SetActive(status[i][j]); // Will set each doors active status to their open bool status
                walls[i].row[j].SetActive(!status[i][j]); // Walls are opposite of door status

                if (status[i][j])
                {
                    GameObject door = doors[i].row[j].transform.GetChild(0).GetChild(0).gameObject; 
                    activeDoors.Add(door);   // Gets the door game object itself and adds to active doors if it is being used
                }
            }
        }

        NavMeshSurface surface = GetComponentInChildren<NavMeshSurface>();
        if (surface != null)
        {
            surface.BuildNavMesh();
        }  
    }
}
