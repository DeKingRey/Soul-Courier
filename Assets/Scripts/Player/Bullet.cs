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
    private Player player;

    void Start()
    {
        player = FindObjectOfType<Player>();

        damage *= player.damageMultiplier;
        transform.localScale *= player.damageMultiplier / 2f;

        // Will get the start pos and add the offset if the bullet is very large
        float forwardOffset = transform.localScale.z / 2f;
        startPos = transform.position + transform.forward * forwardOffset;
    }

    void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
        float maxDistance = (range * player.rangeMultiplier) + (transform.localScale.z / 2f);

        // Destroys the bullet when it reaches the range
        if (Vector3.Distance(startPos, transform.position) >= maxDistance)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player") && !other.CompareTag("Enemy") && !other.CompareTag("EnemyCollider") && !other.isTrigger &&
        !other.CompareTag("Ground"))
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
