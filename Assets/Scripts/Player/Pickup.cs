using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    private Player player;
    public string item;

    private GameObject obj;
    public bool isChild;

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

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            player.Pickup(item);
            Destroy(obj);
        }
    }
}
