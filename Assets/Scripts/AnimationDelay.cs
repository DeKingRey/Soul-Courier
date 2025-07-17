using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationDelay : MonoBehaviour
{
    private float delayTime;
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        delayTime = Random.Range(0, 2f);
        StartCoroutine(StartDelay(delayTime));
    }

    IEnumerator StartDelay(float delay)
    {
        animator.speed = 0;
        yield return new WaitForSeconds(delay);
        animator.speed = 1;
    }
}
