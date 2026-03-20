using UnityEngine;

public class DamageDealer : MonoBehaviour
{
   [SerializeField] private int damageAmount;
   [SerializeField] private bool isContinuousDamage;
   [SerializeField] private float damageTickRate = 0.5f;

    private float nextDamageTime;

    private void OnTriggerEnter2D(Collider2D other)
    {
        DealDamage(other);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if(isContinuousDamage && Time.time >= nextDamageTime)
        {
            DealDamage(other);
        }
    }

    private void DealDamage(Collider2D other)
    {
        if(other.TryGetComponent<Health>(out var health))
            {
            health.TakeDamage(damageAmount);
            nextDamageTime = Time.time + damageTickRate;

            if(TryGetComponent<Projectile>(out var projectile))
            {
                projectile.ReturnToPool();
            }
        }
    }

    public void SetDamage(int amount)
    {
        this.damageAmount = amount;
    }
}
