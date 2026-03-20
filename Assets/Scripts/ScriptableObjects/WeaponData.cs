using UnityEngine;

[CreateAssetMenu(fileName ="New Weapon", menuName = "Custom/Weapon")]
public class WeaponData : ScriptableObject
{
    public Sprite weaponSprite;

    public float fireRate;
    public float spreadAngle;
    public int bulletsPerShot;

    public float reloadTime;
    public int magazineSize;
    public int maxTotalAmmo;

    public int bulletDamage;
    public float bulletSpeed;
    public float bulletLifeTime;
  
}
