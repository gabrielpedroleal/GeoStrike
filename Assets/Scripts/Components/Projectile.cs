using NUnit.Framework.Internal;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float bulletSpeed;
    [SerializeField] private float bulletLifeTime;
    [SerializeField] private string poolTag = "Bullet";


    [SerializeField] private float fireDuration = 2f;
    [SerializeField] private float fireDamagePerSecond = 2f;
    [SerializeField] private float electricDuration = 3f;

    private Rigidbody2D rb;
    private Collider2D coll;
    private DamageDealer damageDealer;
    private BulletEffect effectOnHit;
    private SpriteRenderer spriteRenderer;
   

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
        damageDealer = GetComponent<DamageDealer>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    
    public void Setup(float speed, float lifeTime, int damage, BulletEffect effect)
    {
        this.bulletSpeed = speed;
        this.bulletLifeTime = lifeTime;
        this.effectOnHit = effect;
        if(spriteRenderer != null)
        {
            if (effect == BulletEffect.Fire) spriteRenderer.color = Color.red;
            else if (effect == BulletEffect.Electric) spriteRenderer.color = Color.yellow;
            else spriteRenderer.color = Color.white;
        }
        if (damageDealer != null)
        {
            damageDealer.SetDamage(damage);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (effectOnHit == BulletEffect.None) return;
        if (collision.TryGetComponent<StatusEffectReceiver>(out var receiver))
        {
            if (effectOnHit == BulletEffect.Fire)
                receiver.ApplyEffect(effectOnHit, fireDuration, fireDamagePerSecond);
            else if (effectOnHit == BulletEffect.Electric)
                receiver.ApplyEffect(effectOnHit, electricDuration, 0);
        }
    }

    public void ShootProjectile()
    {
        coll.enabled = true;
        rb.linearVelocity = transform.right * bulletSpeed;
        Invoke(nameof(ReturnToPool), bulletLifeTime);
    }

    public void ReturnToPool()
    {
        coll.enabled = false;
        rb.linearVelocity = Vector2.zero;
        PoolManager.Instance.ReturnToPool(poolTag, this.gameObject);
    }

    private void OnDisable()
    {
        CancelInvoke();
    }
}
