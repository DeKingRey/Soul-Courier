using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SelectItem : MonoBehaviour
{
    private Outline currentOutline;
    private ShopItem currentItem;
    public float raycastDistance;
    public LayerMask itemLayer;
    private Dictionary<Outline, Coroutine> activeCoroutines = new();

    public Image itemInfoDisplay;

    void Update()
    {
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        RaycastHit hit;

        // Checks if the player is hovering over an item
        Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * raycastDistance, Color.red);
        if (Physics.Raycast(ray, out hit, raycastDistance, itemLayer))
        {
            Outline outline = hit.collider.GetComponent<Outline>();
            ShopItem shopItem = hit.collider.GetComponent<ShopItem>();

            // Removes the current outline if they hover over something new
            if (currentOutline != outline)
            {
                if (currentOutline != null)
                {
                    OutlineTransition(currentOutline, 0, 0, 0.2f, currentItem); // Clears outline
                }

                currentOutline = outline;
                currentItem = shopItem;
                OutlineTransition(currentOutline, 5f, 185f, 0.2f, currentItem); // Adds outline
            }
            return;
        }

        if (currentOutline != null)
        {
            OutlineTransition(currentOutline, 0, 0, 0.2f, currentItem); // Clears outline
            currentOutline = null;
            currentItem = null;
        }
    }

    private void OutlineTransition(Outline outline, float targetWidth, float targetAlpha, float duration, ShopItem shopItem)
    {
        // Stops current coroutines
        if (activeCoroutines.TryGetValue(outline, out Coroutine existing))
        {
            StopCoroutine(existing);
            activeCoroutines.Remove(outline);
        }

        Debug.Log(shopItem);

        // Adds the info for the item
        if (shopItem != null)
        {
            // Sets the text to display info
            TextMeshProUGUI itemInfoText = itemInfoDisplay.GetComponentInChildren<TextMeshProUGUI>();
            itemInfoText.text = $"{shopItem.name}: ${shopItem.price.ToString()}";

            RectTransform backgroundRect = itemInfoDisplay.gameObject.GetComponent<RectTransform>();

            // Sets the position just above the item
            Vector3 itemPos = shopItem.gameObject.transform.position;
            Vector3 targetPos = new Vector3(itemPos.x, itemPos.y + 1f, itemPos.z);
            backgroundRect.position = targetPos;

            Vector2 infoDisplaySize = itemInfoText.GetPreferredValues();
            Vector2 padding = new Vector2(0f, 0f);
            backgroundRect.sizeDelta = infoDisplaySize + padding;
        }

        // Starts the coroutine if theres an outline
        Coroutine coroutine = StartCoroutine(OutlineCoroutine(outline, targetWidth, targetAlpha, duration));
        activeCoroutines[outline] = coroutine;
    }

    private IEnumerator OutlineCoroutine(Outline outline, float targetWidth, float targetAlpha, float duration)
    {
        float startWidth = outline.OutlineWidth;
        Color startColor = itemInfoDisplay.color;
        float startAlpha = startColor.a;
        float elapsed = 0f;

        // Will transition the outline width to fade in/out
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;

            outline.OutlineWidth = Mathf.Lerp(startWidth, targetWidth, elapsed / duration);

            Color newColor = itemInfoDisplay.color;
            newColor.a = Mathf.Lerp(startAlpha, targetAlpha, elapsed / duration);
            itemInfoDisplay.color = newColor;

            yield return null;
        }

        outline.OutlineWidth = targetWidth;
        
        Color finalColor = itemInfoDisplay.color;
        finalColor.a = targetAlpha;
        itemInfoDisplay.color = finalColor;

        activeCoroutines.Remove(outline);
    }
}
