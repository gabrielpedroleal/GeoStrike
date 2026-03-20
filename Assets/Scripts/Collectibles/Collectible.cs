using UnityEngine;

[RequireComponent (typeof(Rigidbody2D))]
public abstract class Collectible : MonoBehaviour
{
    [SerializeField] private float explosionForce = 5f;
    [SerializeField] protected string poolTag;

    private Rigidbody2D rb;
    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    protected virtual void OnEnable()
    {
        ApplyExplosionForce();
    }

    private void ApplyExplosionForce()
    {
        Vector2 randomDirection = Random.insideUnitCircle.normalized;
        rb.linearVelocity = Vector2.zero;
        rb.AddForce(randomDirection * explosionForce, ForceMode2D.Impulse);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            ApplyEffect(collision.gameObject);
            Collect();
        }
    }

    protected abstract void ApplyEffect(GameObject player);
  
    protected virtual void Collect()
    {
        PoolManager.Instance.ReturnToPool(poolTag, gameObject);
    }
}
