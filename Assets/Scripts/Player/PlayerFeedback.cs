using System.Collections;
using UnityEngine;
[RequireComponent (typeof(Health))]
public class PlayerFeedBack : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Color flashColor = Color.red;
    [SerializeField] private float flashDuration = 0.1f;

    private Color originalColor;
    private Health playerHealth;
    private Coroutine flashCoroutine;

    private void Awake()
    {
        playerHealth = GetComponent<Health>();
        if(spriteRenderer == null) spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        if(spriteRenderer != null) originalColor = spriteRenderer.color;
    }

    private void OnEnable()
    {
        playerHealth.OnDamageTaken += TriggerDamageFeedback;
    }

    private void OnDisable()
    {
        playerHealth.OnDamageTaken -= TriggerDamageFeedback;
    }

    private void TriggerDamageFeedback()
    {
        if(flashCoroutine != null) StopCoroutine(flashCoroutine);
        flashCoroutine = StartCoroutine(FlashRoutine());
    }

    private IEnumerator FlashRoutine()
    {
        spriteRenderer.color = flashColor;
        yield return new WaitForSeconds(flashDuration);
        spriteRenderer.color = originalColor;
    }
}
