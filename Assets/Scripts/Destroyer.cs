using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroyer : MonoBehaviour
{
    public float destroyTime = 4f;
    void Start()
    {
        Destroy(gameObject, destroyTime);
    }
}
