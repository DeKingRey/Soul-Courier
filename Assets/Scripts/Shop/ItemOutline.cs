using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemOutline : MonoBehaviour
{
    private Outline currentOutline;
    public float raycastDistance;
    public LayerMask itemLayer;
    private Dictionary<Outline, Coroutine> activeCoroutines = new();

    void Update()
    {
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        RaycastHit hit;

        // Checks if the player is hovering over an item
        Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * raycastDistance, Color.red);
        if (Physics.Raycast(ray, out hit, raycastDistance, itemLayer))
        {
            Debug.Log("hit");
            Outline outline = hit.collider.GetComponent<Outline>();

            // Removes the current outline if they hover over something new
            if (currentOutline != outline)
            {
                if (currentOutline != null)
                    OutlineTransition(currentOutline, 0, 0.2f); // Clears outline

                currentOutline = outline;
                OutlineTransition(currentOutline, 5f, 0.2f); // Adds outline
            }
            return;
        }

        if (currentOutline != null)
        {
            OutlineTransition(currentOutline, 0, 0.2f); // Clears outline
            currentOutline = null;
        }
    }

    private void OutlineTransition(Outline outline, float targetWidth, float duration)
    {
        // Stops current coroutines
        if (activeCoroutines.TryGetValue(outline, out Coroutine existing))
        {
            StopCoroutine(existing);
            activeCoroutines.Remove(outline);
        }

        // Starts the coroutine if theres an outline
        Coroutine coroutine = StartCoroutine(OutlineCoroutine(outline, targetWidth, duration));
        activeCoroutines[outline] = coroutine;
    }

    private IEnumerator OutlineCoroutine(Outline outline, float targetWidth, float duration)
    {
        float startWidth = outline.OutlineWidth;
        float elapsed = 0f;

        // Will transition the outline width to fade in/out
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            outline.OutlineWidth = Mathf.Lerp(startWidth, targetWidth, elapsed / duration);
            yield return null;
        }

        outline.OutlineWidth = targetWidth;
        activeCoroutines.Remove(outline);
    }
}
