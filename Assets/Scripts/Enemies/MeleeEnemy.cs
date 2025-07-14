using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MeleeEnemy : MonoBehaviour
{
    [Header("Combat")]
    public float attackCooldown;
    public float attackRange;

    private GameObject player;
    private Player playerScript;
    private NavMeshAgent agent;
    private Animator animator;
    private float timePassed;
    private float moveCooldown;
    private EnemyWeapon[] weapons;

    void Start()
    {
        player = GameObject.FindWithTag("Player");
        playerScript = player.GetComponent<Player>();
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        weapons = GetComponentsInChildren<EnemyWeapon>();
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
        }
        moveCooldown -= Time.deltaTime;
    }

    public void StartAttack()
    {
        foreach (EnemyWeapon weapon in weapons)
        {
            weapon.StartAttack();
        }
    }

    public void EndAttack()
    {
        foreach (EnemyWeapon weapon in weapons)
        {
            weapon.EndAttack();
        }
    }
}
