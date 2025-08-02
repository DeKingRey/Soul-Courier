using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minimap : MonoBehaviour
{
    private float yRotation;


    [Header("Map Switching")]
    private Camera mapCamera;
    public GameObject minimapUI;
    public GameObject fullMapUI;
    public RenderTexture minimapRt;
    public RenderTexture fullMapRt;
    public float miniZoom;
    public float fullZoom;


    void Start()
    {
        // Use euler angles, as quaternions are not easy to use in code(4D!), eulers are to access the raw x, y, and z values of rotation and work with quaternions

        mapCamera = GetComponent<Camera>();
    }


    // MAKE THE FULL MAP CENTERED AND CONSTANT
    void Update()
    {
        if (Time.timeScale == 0) return;

        bool showingFullMap = fullMapUI.activeSelf; // Bool which depends on whether the game object is active or not
        if (!showingFullMap)
        {
            // Gets the parents Y rotation as this is the direction the player will be facing
            yRotation = transform.parent.rotation.eulerAngles.y;
        }
        else
        {
            yRotation = 0;
        }
        
        // Locks the X and Z rotation, and makes the Y rotation relative to the player/main camera
        transform.localRotation = Quaternion.Euler(90f, yRotation, 0); // Eulers are a vector 3

        if (Input.GetKeyDown(KeyCode.M)) // Map switches when pressing 'M'
        {
            if (showingFullMap) // If the full map is displaying then CLOSE it
            {
                // Sets maps on and off (may want to turn off other UI elements too)
                fullMapUI.SetActive(false);
                minimapUI.SetActive(true);

                // Sets the cameras new render texture and size
                mapCamera.targetTexture = minimapRt;
                mapCamera.orthographicSize = miniZoom;
            }
            else // Opens full map
            {
                fullMapUI.SetActive(true);
                minimapUI.SetActive(false);

                mapCamera.targetTexture = fullMapRt;
                mapCamera.orthographicSize = fullZoom;
            }
        }
    }

    public void EnableRoom(SpriteRenderer[] sprites)
    {
        foreach (SpriteRenderer sprite in sprites)
        {
            sprite.enabled = true;
        }
    }
}
