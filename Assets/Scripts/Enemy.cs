using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    private NavMeshAgent agent;

    private Transform target;

    public float damage;
    public float health;

    public float value;

    private Player player;

    public GameObject soul;

    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (agent.enabled && agent.isOnNavMesh) agent.destination = target.position; 
    }

    public void TakeDamage(float damage)
    {
        health -= damage;

        if (health <= 0)
        {
            for (int i = 0; i < value; i++)
            {
                Instantiate(soul, new Vector3(transform.position.x, 1, transform.position.z), transform.rotation);
            }
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider obj)
    {
        if (obj.tag == "Bullet")
        {
            Bullet bullet = obj.GetComponent<Bullet>();
            TakeDamage(bullet.damage);
            Destroy(bullet.gameObject);
        }
    }
}
