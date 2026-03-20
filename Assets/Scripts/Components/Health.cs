using UnityEngine;
using System;

public class Health : MonoBehaviour
{
    [SerializeField] private float maxHealth;
    public float MaxHealth { get { return maxHealth; } }
    [SerializeField] private float currentHealth;
    public bool isDead { get; private set;  }
    public bool isInvincible;

    public event Action<float> OnHealthChanged;
    public event Action OnDamageTaken;
    public event Action OnDeath;

    private void OnEnable()
    {
        currentHealth = maxHealth;
        isDead = false;
        if(TryGetComponent<Collider2D>(out var collider)) collider.enabled = true;
    }

    public void SetInvincible(bool state)
    {
        isInvincible = state;
    }

    public void TakeDamage(float amount)
    {
        if (isInvincible || isDead) return;

        currentHealth -= amount;
       
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        OnHealthChanged?.Invoke(currentHealth);
        OnDamageTaken?.Invoke();

        if (CompareTag("Player"))
        {
            AudioManager.Instance.PlaySFX("PlayerHurt");
        }

        if (currentHealth <= 0 && !isDead)
        {
            Die();
        }
    }

    private void Die()
    {
        isDead = true;
        OnDeath?.Invoke();

        if (CompareTag("Player"))
        {
            AudioManager.Instance.PlaySFX("PlayerDead");
        }

        if (TryGetComponent<Collider2D>(out var collider)) collider.enabled = false;
        if (TryGetComponent<Rigidbody2D>(out var rigidbody)) rigidbody.linearVelocity = Vector3.zero;

        if (TryGetComponent<UnityEngine.InputSystem.PlayerInput>(out var playerInput))
        {
            playerInput.enabled = false;
        }
        if (TryGetComponent<LootDropper>(out var dropper))
        {
            dropper.DropItem();
        }

        gameObject.SetActive(false);
        Debug.Log($"{gameObject.name} died!");
    }

    public void Heal(float amount)
    {
        if (isDead) return;
        currentHealth += amount;
        AudioManager.Instance.PlaySFX("Heal");
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        OnHealthChanged?.Invoke(currentHealth);
    }
}
