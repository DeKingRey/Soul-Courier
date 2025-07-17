using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Porotai : MonoBehaviour
{
    [Header("Combat")]
    public float attackCooldown;
    public float attackRange;
    public float moveCooldownTime;
    public float rotationSpeed;

    [Header("Abilties")]
    public GameObject headbutAttack;
    public float visibleDistance;
    public float fadeSpeed;
    private float currentAlpha = 0f;

    private bool canMove = true;

    private GameObject player;
    private Player playerScript;
    private NavMeshAgent agent;
    private Animator animator;
    private float timePassed;
    private float moveCooldown;
    private EnemyWeapon[] weapons;
    private Renderer enemyRenderer;

    void Start()
    {
        player = GameObject.FindWithTag("Player");
        playerScript = player.GetComponent<Player>();
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>(true);
        weapons = GetComponentsInChildren<EnemyWeapon>(true);
        enemyRenderer = GetComponentInChildren<Renderer>(true);

        moveCooldown = moveCooldownTime;
    }

    void Update()
    {
        if (!agent.enabled && !agent.isOnNavMesh) return;

        // Checks if the attack cooldown has ended
        if (timePassed >= attackCooldown)
        {
            // Checks if the player is within the attack range
            if (Vector3.Distance(player.transform.position, transform.position) <= attackRange)
            {
                StartCoroutine(FacePlayer());
                timePassed = 0;
            }
        }
        timePassed += Time.deltaTime;

        if (moveCooldown <= 0 && canMove || agent.remainingDistance <= agent.stoppingDistance && canMove)
        {
            animator.SetBool("moving", true);
            moveCooldown = moveCooldownTime;
            agent.SetDestination(player.transform.position);
            //Move(); // The porotai walks in straight lines
        }

        if (animator.GetBool("moving"))
        {
            // Gets the speed ratio, based on how fast the enemy is currently moving / its max speed
            animator.speed = agent.velocity.magnitude / agent.speed; // Sets the animation speed relative to the moving speed
        }
        else
        {
            animator.speed = 1f;
        }

        #region Invisibility
        // Gets a number based on how far away the enemy is, the close the enemy the more visible and makes it smooth
        float targetAlpha = Mathf.SmoothStep(0f, 1f, Mathf.InverseLerp(visibleDistance, 0f, agent.remainingDistance));
        currentAlpha = Mathf.Lerp(currentAlpha, targetAlpha, fadeSpeed * Time.deltaTime); 

        Color color = enemyRenderer.material.color;
        color.a = currentAlpha;
        enemyRenderer.material.color = color;
        #endregion

        moveCooldown -= Time.deltaTime;
    }

    void Move()
    {
        // Chooses a random direction from the four basic directions
        Vector3[] directions = new Vector3[]
        {
            Vector3.forward,
            Vector3.back,
            Vector3.left,
            Vector3.right
        };
        // Chooses a random direction and its length
        Vector3 randomDirection = directions[Random.Range(0, directions.Length)]; //* moveDistance; 
        randomDirection += transform.position; // Gets the end position by adding the random dir to the current position

        // Checks if the position is a valid point on the navmesh
        if (NavMesh.SamplePosition(randomDirection, out NavMeshHit hit, 5, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position); // Sets the agents destination
        }
    }

    private IEnumerator FacePlayer()
    {
        // Gets direction of the player
        Vector3 playerDir = (player.transform.position - transform.position).normalized;
        playerDir.y = 0;
        // Gets target rotation
        Quaternion startRotation = transform.rotation;
        Quaternion targetRotation = Quaternion.LookRotation(playerDir);

        agent.isStopped = true;

        // Loop will continue until fully rotated then the enemy will attack
        float elapsed = 0;
        while (elapsed < 1f)
        {
            canMove = false; // Prevents movement
            elapsed += Time.deltaTime * rotationSpeed;
            transform.rotation = Quaternion.Slerp(startRotation, targetRotation, elapsed); // Elapsed is the progress
            yield return null;
        }

        animator.SetTrigger("attack"); // Plays attack animation
        animator.SetBool("moving", false);
    }

    public void StartAttack()
    {
        foreach (EnemyWeapon weapon in weapons)
        {
            headbutAttack.SetActive(true);
            weapon.StartAttack();
        }
    }

    public void EndAttack()
    {
        foreach (EnemyWeapon weapon in weapons)
        {
            headbutAttack.SetActive(false);
            weapon.EndAttack();
        }
        canMove = true;
        agent.isStopped = false;
    }
}
