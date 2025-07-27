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

    private bool isHiding;
    public float hideDuration;
    public float underWaterY;
    public float waterTransitionDuration;
    private List<Transform> spawnPoints;

    private GameObject player;
    private Player playerScript;
    private NavMeshAgent agent;
    private Animator animator;
    private float timePassed = 0;
    private float moveCooldown;
    private EnemyWeapon[] weapons;
    private Enemy enemyHealth;
    private float currentHealth;
    private float defaultDefence;

    void Start()
    {
        player = GameObject.FindWithTag("Player");
        playerScript = player.GetComponent<Player>();
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>(true);
        weapons = GetComponentsInChildren<EnemyWeapon>(true);

        enemyHealth = GetComponent<Enemy>();
        currentHealth = enemyHealth.health;
        defaultDefence = enemyHealth.defence;

        moveCooldown = moveCooldownTime;
    }

    void Update()
    {
        // Will prevent attacking AI and prevent losing health
        if (isHiding) return;

        // Will hide if hit
        if (enemyHealth.health < currentHealth)
        {
            currentHealth = enemyHealth.health;
            Hide();
            return;
        }
        currentHealth = enemyHealth.health;

        HandleAttack();
        HandleMovement();
    }

    private void HandleAttack()
    {
        timePassed += Time.deltaTime;
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
    }

    private void HandleMovement()
    {
        if (isHiding || !agent.enabled || !agent.isOnNavMesh) return;

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

        // Sets animator speed relative to the moving speed so the running speed looks accurate
        if (animator.GetBool("moving"))
        {
            animator.speed = agent.velocity.magnitude / agent.speed;
        }
        else
        {
            animator.speed = 1f;
        }
    }

    private void Hide()
    {
        if (isHiding) return;

        if (agent.enabled && agent.isOnNavMesh) agent.ResetPath();
        agent.enabled = false;

        // Animator trigger
        enemyHealth.defence = 0;
        StartCoroutine(WaterRiseSinkRoutine(underWaterY, false));
    }

    private IEnumerator WaterRiseSinkRoutine(float yPos, bool isRising)
    {
        // Will cause the Pona-Turi to either rise or sink
        Vector3 startPos = transform.position;
        Vector3 newPos = new Vector3(startPos.x, yPos, startPos.z);

        float elapsed = 0f;
        // Changes y position
        while (elapsed < waterTransitionDuration)
        {
            elapsed += Time.deltaTime;
            transform.position = Vector3.Lerp(startPos, newPos, elapsed / waterTransitionDuration);
            yield return null;
        }
        transform.position = newPos;

        // Functionality for whether it has sunk or risen
        if (isRising)
        {
            agent.enabled = true;
            // Animator trigger
            enemyHealth.defence = defaultDefence;
            isHiding = false;
        }
        else
        {
            StartCoroutine(AwaitReemerge());
        }
    }

    private IEnumerator AwaitReemerge()
    {
        // Will wait a duration then enable the Pona-Turi again
        yield return new WaitForSeconds(hideDuration);

        // Gets a random position for it to rise from
        transform.position = spawnPoints[Random.Range(0, spawnPoints.Count)].position;
        StartCoroutine(WaterRiseSinkRoutine(0, true));
    }

    public void SetSpawnPoints(List<Transform> points)
    {
        spawnPoints = points;
        for (int i = 0; i < spawnPoints.Count - 1; i++)
        {
            Vector3 pos = spawnPoints[i].position;
            pos.y = underWaterY;
            spawnPoints[i].position = pos;
        }
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

        Hide();
    }
}
