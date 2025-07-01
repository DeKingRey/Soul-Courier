using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapStaticRoom : MonoBehaviour
{
    // This script is for the minimap rooms like, start, end, blackmarket, shop, etc that don't have the room spawner script to emable them on the minimap

    public Minimap minimap;
    private SpriteRenderer[] minimapSprites;
    private bool entered;

    void Start()
    {
        minimap = GameObject.FindGameObjectWithTag("Minimap").GetComponent<Minimap>();
        minimapSprites = GetComponentsInChildren<SpriteRenderer>(true);
    }

    void OnTriggerEnter(Collider obj)
    {
        // Enables sprites once in minimap
        if (obj.CompareTag("Player") && !entered)
        {
            entered = true;
            minimap.EnableRoom(minimapSprites);
        }
    }
}
