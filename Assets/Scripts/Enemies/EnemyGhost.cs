using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGhost : MonoBehaviour
{
    public float speed;
    public float rotationSpeed; // Degrees per second
    public float swayAmplitude; // How far it sways
    public float swayFrequency; // How fast it sways
    public float duration;
    private Renderer[] renderers;
    private Material[] materials;

    void Start()
    {
        renderers = GetComponentsInChildren<Renderer>();
        materials = new Material[renderers.Length];

        for (int i = 0; i < renderers.Length; i++)
        {
            Material mat = renderers[i].material;
            materials[i] = mat;

            mat.SetFloat("_Mode", 2); // Sets the rendering mode to fade
            mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha); // Makes unity use the pixel's alpha value when rendering
            mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha); // Mix the current pixel with what's behind it
            mat.SetInt("_ZWrite", 0); // Prevents sorting issues
            mat.DisableKeyword("_ALPHATEST_ON"); // Disabling this allows for  smooth fading
            mat.EnableKeyword("_ALPHABLEND_ON"); // Allows smooth transparency
            mat.DisableKeyword("_ALPHAPREMULTIPLY_ON"); // Disables to ensure proper alpha fading
            mat.renderQueue = 3000; // Ensures background is seen
        }

        StartCoroutine(Dissipate());
    }

    private IEnumerator Dissipate()
    {
        float alpha = 1f;
        float noiseOffset = Random.Range(0, 100f); // Creates randomness in noise

        while (alpha > 0f)
        {
            for (int i = 0; i < materials.Length; i++)
            {
                Color color = materials[i].color;
                color.a = alpha;
                materials[i].color = color;
            }

            transform.position += transform.up * speed * Time.deltaTime; // Floats upward
            transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime); // Rotates slowly

            // Gets the noise using Perlins Noise which is smoother than random directions
            float noiseX = Mathf.PerlinNoise(Time.time * swayFrequency + noiseOffset, 0f) - 0.5f;
            float noiseZ = Mathf.PerlinNoise(0f, Time.time * swayFrequency + noiseOffset) - 0.5f;

            // Applies the noise to make the ghost sway
            Vector3 swayOffset = new Vector3(noiseX, 0, noiseZ) * swayAmplitude;
            transform.position += swayOffset * Time.deltaTime; 

            alpha -= Time.deltaTime / duration;

            yield return null;
        }

        Destroy(gameObject);
    }
}
