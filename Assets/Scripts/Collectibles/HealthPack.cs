using UnityEngine;

public class HealthPack : Collectible
{
    [SerializeField] private int healthAmmount = 20;

    protected override void ApplyEffect(GameObject player)
    {
        if(player.TryGetComponent<Health>(out var health))
        {
            health.Heal(healthAmmount);
        }
    }
}
