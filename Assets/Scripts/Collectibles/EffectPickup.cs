using UnityEngine;

public class EffectPickup : Collectible
{
    [SerializeField] private BulletEffect effectType;
    [SerializeField] private float effectDuration = 10f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            WeaponComponent weapon = collision.GetComponentInChildren<WeaponComponent>();
            if (weapon != null)
            {
                weapon.SetTemporaryEffect(effectType, effectDuration);

               
                if (AudioManager.Instance != null) AudioManager.Instance.PlaySFX("AmmoPickup");

                Destroy(gameObject);
            }
        }
    }

    protected override void ApplyEffect(GameObject player)
    {
        WeaponComponent weapon = player.GetComponentInChildren<WeaponComponent>();
        if (weapon != null)
        {
            weapon.SetTemporaryEffect(effectType, effectDuration);
        }
    }
}
