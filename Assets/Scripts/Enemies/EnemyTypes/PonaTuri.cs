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
    public float riseRadius;

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
        weapons = GetComponentsInChildren<EnemyWeapon>(true);

        moveCooldown = moveCooldownTime;
    }

    void Update()
    {
        if (isHiding) return;

        #region Attacking AI
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

        // Sets animator speed relative to the moving speed so the running speed looks accurate
        if (animator.GetBool("moving"))
        {
            animator.speed = agent.velocity.magnitude / agent.speed;
        }
        else
        {
            animator.speed = 1f;
        }
        #endregion
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
        float elapsed = 0f;

        while (elapsed < hideDuration)
        {
            elapsed += Time.deltaTime;
            yield return null;
        }

        // Gets a random position for it to rise from
        transform.position = GetRisePosition();
        StartCoroutine(WaterRiseSinkRoutine(0, true));
    }

    private Vector3 GetRisePosition()
    {
        // Trys to get a random position within the navmesh, if one isn't found the current pos is returned
        for (int i = 0; i < 10; i++)
        {
            Vector3 randomPos = transform.position + Random.insideUnitSphere * riseRadius;
            randomPos.y = transform.position.y;

            if (NavMesh.SamplePosition(randomPos, out NavMeshHit hit, 5f, NavMesh.AllAreas))
            {
                return hit.position;
            }
        }
        return transform.position;
    }

    public void StartAttack()
    {
        foreach (EnemyWeapon weapon in weapons)
        {
            Debug.Log(weapon.name);
            weapon.StartAttack();
        }
    }

    public void EndAttack()
    {
        foreach (EnemyWeapon weapon in weapons)
        {
            weapon.EndAttack();
        }

        agent.enabled = false;
        // Animator trigger
        isHiding = true;
        StartCoroutine(WaterRiseSinkRoutine(underWaterY, false));
    }
}
