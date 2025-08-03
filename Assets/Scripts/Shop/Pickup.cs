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
    public int price;

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

        // Moves the item towards the player, if close enough
        if (distance <= 4f)
        {
            transform.position = Vector3.MoveTowards(transform.position, playerPos, speed * Time.deltaTime);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && item != "")
        {
            player.Pickup(item);
            Destroy(obj);
        }
    }
}
