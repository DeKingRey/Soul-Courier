using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    private Player player;
    public string item;

    private GameObject obj;
    public bool isChild;

    public float speed;

    void Start()
    {
        player = FindObjectOfType<Player>();

        if (isChild)
        {
            obj = this.transform.parent.gameObject;
        }
        else
        {
            obj = this.gameObject;
        }
    }

    void Update()
    {
        Vector3 playerPos = player.transform.position + new Vector3(0, 1.5f, 0);
        float distance = Vector3.Distance(playerPos, transform.position); // Gets the shortest distance to the player from the item

        if (distance <= 4f)
        {
            transform.position = Vector3.MoveTowards(transform.position, playerPos, speed * Time.deltaTime); // Moves the item towarss the player
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            player.Pickup(item);
            Destroy(obj);
        }
    }
}
