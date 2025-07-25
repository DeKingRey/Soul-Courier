using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PonaTuri : MonoBehaviour
{
    [Header("Combat")]
    public float attackCooldown;
    public float attackRange;
    public float moveCooldownTime;

    private GameObject player;
    private Player playerScript;
    private NavMeshAgent agent;
    private Animator animator;
    private float timePassed = 0;
    private float moveCooldown;
    private EnemyWeapon[] weapons;

    void Start()
    {
        player = GameObject.FindWithTag("Player");
        playerScript = player.GetComponent<Player>();
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>(true);
        weapons = GetComponentsInChildren<EnemyWeapon>();

        moveCooldown = moveCooldownTime;
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
            moveCooldown = moveCooldownTime;
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
