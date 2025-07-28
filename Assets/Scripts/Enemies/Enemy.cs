using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [Header("Prefabs")]
    public static GameObject soul;
    public static GameObject deathParticles;
    public static GameObject ghost;

    [Header("Stats")]
    public float health;
    public float value;
    public float defence = 1f;
    public float stunResistance = 1f;
    public bool isInvulnerable = false;

    private NavMeshAgent agent;
    private Transform target;
    private Animator animator;
    private Player player;

    private Renderer[] renderers;
    private List<Material> originalMats = new List<Material>();

    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        player = FindObjectOfType<Player>();
        animator = GetComponentInChildren<Animator>(true);

        renderers = GetComponentsInChildren<Renderer>(true);
        foreach (Renderer renderer in renderers)
        {
            originalMats.Add(renderer.material);
        }
    }

    void Update()
    {
        //if (agent.enabled && agent.isOnNavMesh) agent.destination = target.position;
    }

    public void TakeDamage(float damage, Bullet bullet)
    {
        health -= damage / defence;

        #region Damage Effects
        // Spawns hit particle effect
        if (Physics.Raycast(bullet.transform.position - transform.forward * 0.5f, transform.forward, out RaycastHit hit, 1f))
        {
            Instantiate(bullet.hitImpactPrefab,
            hit.point,
            Quaternion.LookRotation(hit.normal));
        }
        
        // Makes the enemy flash white
        foreach (Renderer renderer in renderers)    
        {
            renderer.material = bullet.hitMat;
        }
        StartCoroutine(FlashRoutine());

        // Gets the direction the enemy should be knocked back in
        Vector3 knockbackDirection = (transform.position - player.transform.position).normalized;
        knockbackDirection.y = 0f; // Removes any vertical knockback
        knockbackDirection.Normalize(); // Normalizes 
        StartCoroutine(HandleKnockback(knockbackDirection, bullet.knockbackForce));

        StartCoroutine(StunEnemy(bullet.stunTime * stunResistance));
        #endregion

        if (health <= 0)
        {
            for (int i = 0; i < value; i++)
            {
                Instantiate(soul, new Vector3(transform.position.x, 1, transform.position.z), transform.rotation);
            }

            #region Death Effects
            Instantiate(deathParticles, new Vector3(transform.position.x, 0, transform.position.z), Quaternion.Euler(-90, 0, 0));

            GameObject ghostParent = Instantiate(ghost, new Vector3(transform.position.x, 0, transform.position.z), Quaternion.identity);
            GameObject ghostModel = Instantiate(gameObject, transform.position, Quaternion.identity, ghostParent.transform);

            // Gets every component on the model and removes them if they aren't transform(navmesh, anim, enemy script, etc)
            Component[] components = ghostModel.GetComponents<Component>();
            foreach (Component component in components)
            {
                if (!(component is Transform))
                {
                    Destroy(component);
                }
            }
            #endregion

            Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider obj)
    {
        if (obj.tag == "Bullet" && !isInvulnerable)
        {
            Bullet bullet = obj.GetComponent<Bullet>();
            TakeDamage(bullet.damage, bullet);
            Destroy(bullet.gameObject);
        }
    }

    private IEnumerator FlashRoutine()
    {
        yield return new WaitForSeconds(0.1f);

        for (int i = 0; i < originalMats.Count; i++)
        {
            renderers[i].material = originalMats[i];
        }
    }

    private IEnumerator HandleKnockback(Vector3 direction, float force)
    {
        float elapsed = 0f;
        while (elapsed < 0.2f)
        {
            transform.position += direction * force * stunResistance * Time.deltaTime; // Applies the knockback
            elapsed += Time.deltaTime;

            yield return null;
        }
    }

    private IEnumerator StunEnemy(float stunTime)
    {
        float elapsed = 0f;
        float animatorSpeed = animator.speed;

        // Disables the enemy until the stun is worn off
        while (elapsed < stunTime)
        {
            agent.enabled = false;
            animator.speed = 0; // Pauses the animator
            elapsed += Time.deltaTime;

            yield return null;
        }

        if (!isInvulnerable && defence == 1) agent.enabled = true;
        animator.speed = animatorSpeed;
    }
}
