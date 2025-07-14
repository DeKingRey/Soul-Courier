using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RangedEnemy : MonoBehaviour
{
    [Header("Combat")]
    public float attackCooldown;
    public float attackRange;
    public GameObject projectileWeapon;
    public float projectileForce;
    public Transform throwPoint;

    private GameObject player;
    private Player playerScript;
    private NavMeshAgent agent;
    private Animator animator;
    private float timePassed;
    private float moveCooldown;
    private GameObject projectile;

    void Start()
    {
        player = GameObject.FindWithTag("Player");
        playerScript = player.GetComponent<Player>();
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>(true);
    }

    void Update()
    {
        // Checks if the attack cooldown has ended
        if (timePassed >= attackCooldown)
        {
            // Checks if the player is within the attack range
            if (Vector3.Distance(player.transform.position, transform.position) <= attackRange)
            {
                animator.SetTrigger("attack"); // Plays attack animation
                timePassed = 0;
            }
        }
        timePassed += Time.deltaTime;

        if (moveCooldown <= 0)
        {
            moveCooldown = 0.5f;
            if (agent.enabled && agent.isOnNavMesh) agent.SetDestination(player.transform.position);

            if (Vector3.Distance(player.transform.position, transform.position) >= agent.stoppingDistance)
            {
                animator.SetBool("moving", true);
            }
            else
            {
                animator.SetBool("moving", false);
            }
        }
        moveCooldown -= Time.deltaTime;
    }

    public void ThrowRock()
    {
        // Spawns the rock as a child of the hand position so it moves with the animation
        projectile = Instantiate(projectileWeapon, throwPoint);
        projectile.transform.localPosition = new Vector3(0, 0.015f, 0); // Spawns at local position to avoid positional error
        projectile.GetComponent<Rigidbody>().isKinematic = true; // Disables physics(for now)
    }

    public void StartAttack()
    {
        EnemyWeapon[] weapons = GetComponentsInChildren<EnemyWeapon>();
        foreach (EnemyWeapon weapon in weapons)
        {
            projectile.transform.SetParent(null); // Removes the rock from its parent

            Rigidbody rb = projectile.GetComponent<Rigidbody>();
            rb.isKinematic = false; // Enables physics
            Vector3 playerDir = (player.transform.position - projectile.transform.position).normalized; // Gets the direction of the player
            Vector3 launchDir = (playerDir + Vector3.up * 0.2f).normalized; // Adds upward direction to the launch dir
            rb.drag = 0f; // Removes air drag force
            rb.AddForce(launchDir * projectileForce, ForceMode.Impulse); // Adds an impulse force to the rock in the direction of the player

            weapon.StartAttack();
        }
    }

    public void EndAttack()
    {
        EnemyWeapon[] weapons = GetComponentsInChildren<EnemyWeapon>();
        foreach (EnemyWeapon weapon in weapons)
        {
            weapon.EndAttack();
        }
    }
}
