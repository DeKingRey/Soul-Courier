using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FernRandomness : MonoBehaviour
{
    private float delayTime;
    private float size;
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        delayTime = Random.Range(0, 2f);
        size = Random.Range(0.5f, 1f);
        transform.localScale = Vector3.one * size; // Random size
        StartCoroutine(StartDelay(delayTime));
    }

    IEnumerator StartDelay(float delay)
    {
        animator.speed = 0;
        yield return new WaitForSeconds(delay);
        animator.speed = 1;
    }
}
