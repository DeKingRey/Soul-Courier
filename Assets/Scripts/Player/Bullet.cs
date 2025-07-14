using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed;
    public float damage;
    public float knockbackForce;
    public float stunTime;
    public GameObject impactPrefab;
    public GameObject hitImpactPrefab;
    public Material hitMat;

    void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Player" && !other.isTrigger)
        {
            // Creates a forward ray that gets information as to what it hit
            if (Physics.Raycast(transform.position - transform.forward * 0.5f, transform.forward, out RaycastHit hit, 1f))
            {
                // Instantiates the impact particles at the hit point and the direction of the hit normal
                GameObject impact = Instantiate(impactPrefab, hit.point + transform.forward * 0.5f, Quaternion.LookRotation(hit.normal));

                /*Renderer impactRenderer = impact.GetComponent<Renderer>();
                Material hitMat = hit.collider.GetComponent<MeshRenderer>().material;
                impactRenderer.material = hitMat;*/
            }
            
            Destroy(gameObject);
        }
    }
}
