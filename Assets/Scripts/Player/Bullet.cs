using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed;
    public float damage;
    public float range;
    public float knockbackForce;
    public float stunTime;
    public GameObject impactPrefab;
    public GameObject hitImpactPrefab;
    public Material hitMat;

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;

        // Destroys the bullet when it reaches the range
        if (Vector3.Distance(startPos, transform.position) >= range)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player") && !other.CompareTag("Enemy") && !other.CompareTag("EnemyCollider") && !other.isTrigger)
        {
            // Creates a forward ray that gets information as to what it hit
            if (Physics.Raycast(transform.position - transform.forward * 0.5f, transform.forward, out RaycastHit hit, 1f))
            {
                // Instantiates the impact particles at the hit point and the direction of the hit normal
                GameObject impact = Instantiate(impactPrefab, hit.point + transform.forward * 0.5f, Quaternion.LookRotation(hit.normal));
            }
            
            Destroy(gameObject);
        }
    }
}
