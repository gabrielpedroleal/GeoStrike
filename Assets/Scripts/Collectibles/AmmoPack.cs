using UnityEngine;

public class AmmoPack : Collectible
{
    public enum AmmoType {Rifle = 0, Shotgun = 1}
    [SerializeField] private AmmoType type;
    [SerializeField] private int ammoAmount = 10;

    protected override void ApplyEffect(GameObject player)
    {
  
        WeaponComponent weaponComp = player.GetComponentInChildren<WeaponComponent>();

        if (weaponComp == null)
        {
            weaponComp = player.GetComponentInParent<WeaponComponent>();
        }

        if (weaponComp != null)
        {
           
            weaponComp.AddAmmo((int)type, ammoAmount);
            AudioManager.Instance.PlaySFX("AmmoPickup");
        }
       
    }
}
