using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerDamageEffects : MonoBehaviour
{
    [Header("Damage Vignette")]
    public float vignetteTime;
    public Image vignette;

    [Header("Blink Flash")]
    public GameObject playerObj;
    private Renderer[] renderers;

    public int blinkCount;
    public float blinkInterval;

    void Start()
    {
        renderers = playerObj.GetComponentsInChildren<Renderer>();
    }

    public void TakeDamageEffects()
    {
        StartCoroutine(VignetteFade());
        StartCoroutine(BlinkFlash());
    }

    private IEnumerator BlinkFlash()
    {
        for (int i = 0; i < blinkCount; i++)
        {
            foreach (Renderer renderer in renderers)
            {
                renderer.enabled = false;
            }

            yield return new WaitForSeconds(blinkInterval);

            foreach (Renderer renderer in renderers)
            {
                renderer.enabled = true;
            }

            yield return new WaitForSeconds(blinkInterval);
        }
    }

    private IEnumerator VignetteFade()
    {
        float opacity = 1f;
        Color colour = vignette.color;
        colour.a = opacity;
        vignette.color = colour; // Sets to full opacity

        yield return new WaitForSeconds(vignetteTime); // Starts a timer

        while (opacity > 0)
        {
            opacity -= Time.deltaTime; // Decreases opacity

            if (opacity <= 0) opacity = 0;

            colour.a = opacity;
            vignette.color = colour; // Sets the current opacity to the vignette

            yield return null;
        }

        colour.a = 0;
        vignette.color = colour;
        yield break;
    }
}
