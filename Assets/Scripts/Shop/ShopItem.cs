using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopItem : MonoBehaviour
{
    public string name;
    public int price;

    void Start()
    {
        // Disables colliders and item magnets
        item.GetComponent<BoxCollider>().isTrigger = false; 
        foreach (Transform child in item.transform)
        {
            BoxCollider collider = child.GetComponent<BoxCollider>();

            if (collider != null && collider.isTrigger)
            {
                collider.isTrigger = false;
            }
        }
        Pickup pickup = item.GetComponentInChildren<Pickup>();
        if (pickup != null)
        {
            pickup.speed = 0;
        }
        Rigidbody rb = item.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
        }

        // Adds animation to items that don't already have the default animation
        GameObject animatorChild = item.transform.GetChild(0).gameObject;
        Animator animator = animatorChild.GetComponent<Animator>();
        if (animator == null)
        {
            animator = animatorChild.AddComponent<Animator>();
            animator.runtimeAnimatorController = animatorController;
        }

        // Adds layer and outline 
        item.layer = LayerMask.NameToLayer("Shop Item");
        Outline outline = item.AddComponent<Outline>();
        outline.OutlineWidth = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
