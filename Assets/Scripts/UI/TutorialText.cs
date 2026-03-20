using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SpriteRenderer))]
public class TutorialWorldText : MonoBehaviour
{
    [Header("Configurações")]
    [SerializeField] private float delayBeforeFade = 4f; //
    [SerializeField] private float fadeDuration = 2f; 

    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
       
        StartCoroutine(FadeAndDestroy());
    }

    private IEnumerator FadeAndDestroy()
    {
      
        yield return new WaitForSeconds(delayBeforeFade);

        float elapsedTime = 0f;
        Color startColor = spriteRenderer.color;

       
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float newAlpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
            spriteRenderer.color = new Color(startColor.r, startColor.g, startColor.b, newAlpha);
            yield return null;
        }

      
        Destroy(gameObject);
    }
}