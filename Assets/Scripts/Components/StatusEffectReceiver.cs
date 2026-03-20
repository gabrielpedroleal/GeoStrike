using UnityEngine;
using System.Collections;

public class StatusEffectReceiver : MonoBehaviour
{
    [SerializeField] private ParticleSystem fireParticles;
    [SerializeField] private ParticleSystem electricParticles;
    private SpriteRenderer spriteRenderer;

    private Health health;
    private EnemyAI enemyAI;

    private Coroutine fireCoroutine;
    private Coroutine electricCoroutine;
    private Color originalColor;

    private void Awake()
    {
        health = GetComponent<Health>();
        enemyAI = GetComponent<EnemyAI>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
        if (fireParticles != null) fireParticles.Stop();
        if (electricParticles != null) electricParticles.Stop();
    }

    public void ApplyEffect(BulletEffect effect, float duration, float damagePerTick)
    {
        if (effect == BulletEffect.Electric)
        {
            if (electricCoroutine != null) StopCoroutine(electricCoroutine);
            electricCoroutine = StartCoroutine(ParalyzeRoutine(duration));
        }
        else if (effect == BulletEffect.Fire)
        {
            if (fireCoroutine != null) StopCoroutine(fireCoroutine);
            fireCoroutine = StartCoroutine(BurnRoutine(duration, damagePerTick));
        }
    }

    private IEnumerator ParalyzeRoutine(float duration)
    {
      
        if (enemyAI != null) enemyAI.SetParalyzed(true);

        if (electricParticles != null) electricParticles.Play();
        if (spriteRenderer != null)
        {
            spriteRenderer.color = Color.yellow;
            yield return new WaitForSeconds(0.1f);
        }

        yield return new WaitForSeconds(duration - 0.1f);

        if (enemyAI != null) enemyAI.SetParalyzed(false);
        if (electricParticles != null) electricParticles.Stop();
        spriteRenderer.color = originalColor;


    }

    private IEnumerator BurnRoutine(float duration, float damagePerTick)
    {
        if(fireParticles != null) fireParticles.Play();

        float elapsed = 0f;
      

        while (elapsed < duration)
        {
            yield return new WaitForSeconds(1f);

            if (health != null && !health.isDead)
            {
                health.TakeDamage(damagePerTick);
            }
            elapsed += 1f;
        }
        
        if(fireParticles != null) fireParticles.Stop();
    }
}