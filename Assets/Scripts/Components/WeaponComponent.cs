using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using System.Collections;

public class WeaponComponent : MonoBehaviour
{
    [System.Serializable]
    public class AmmoState
    {
        public int currentMag;
        public int currentTotalAmmo;
    }

    [SerializeField] private WeaponData[] loadout;
    private AmmoState[] ammoInventory;
    private int currentWeaponIndex = 0;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private SpriteRenderer weaponRenderer;
    [SerializeField] private bool isPlayer = true;
    private bool isCurrentReloading;
    private WeaponData currentWeaponData;
    private BulletEffect currentBulletEffect = BulletEffect.None;
    private float effectTimer = 0f;
    private float maxEffectDuration = 0f;

    private float nextFireTime;
    private Camera cam;
    private PlayerInputHandler inputHandler;
    private GameManager gameManager;

    public event System.Action<int, int> OnAmmoChanged;
    public float EffectProgress
    {
        get
        {
            if (maxEffectDuration <= 0) return 0;
            return effectTimer / maxEffectDuration;
        }
    }

    public BulletEffect CurrentEffect => currentBulletEffect;

    private void Awake()
    {
        cam = Camera.main;
       
        inputHandler = GetComponentInParent<PlayerInputHandler>();

        InitializeAmmo();
    }

    private void InitializeAmmo()
    {
        ammoInventory = new AmmoState[loadout.Length];
        for (int i = 0; i < loadout.Length; i++)
        {
            if (loadout[i] != null)
            {
                ammoInventory[i] = new AmmoState
                {
                    currentMag = loadout[i].magazineSize,
                    currentTotalAmmo = loadout[i].maxTotalAmmo
                };
            }
        }
    }

    private void Start()
    {
        EquipWeapon(0);
    }

    private void Update()
    {
        UpdateWeaponVisuals();

        if (effectTimer > 0)
        {
            effectTimer -= Time.deltaTime;
            if (effectTimer <= 0) 
            { 
                currentBulletEffect = BulletEffect.None; 
                maxEffectDuration = 0;
            }
        }

        if (!isPlayer) return;
        
        HandleAiming();
        HandleWeaponSwitch();
        
        if (inputHandler != null)
        {
         
            if (inputHandler.isReloading && !isCurrentReloading)
            {
                StartCoroutine(Reload());
                inputHandler.ResetReloadTrigger();
            }

          
            if (inputHandler.isfiring && Time.time >= nextFireTime && !isCurrentReloading)
            {

                if (ammoInventory[currentWeaponIndex].currentMag > 0)
                {
                    
                    Shoot();
                }
                else
                {
                    AudioManager.Instance.PlaySFX("EmptyClick");
                    nextFireTime = Time.time + 0.7f;
                    Debug.Log("Out of ammo in magazine!");
                }
            }
        }
    }

    private void Shoot()
    {
        if (Time.timeScale == 0) return;
        nextFireTime = Time.time + currentWeaponData.fireRate;

        if (isPlayer)
        {
            ammoInventory[currentWeaponIndex].currentMag--;
            UpdateAmmoUI();
        }

        for (int i = 0; i < currentWeaponData.bulletsPerShot; i++)
        {
            GameObject bullet = PoolManager.Instance.SpawnFromPool("Bullet", firePoint.position, firePoint.rotation);

          
            bullet.layer = LayerMask.NameToLayer(isPlayer ? "PlayerProjectiles" : "EnemyProjectiles");

           
            float currentZRotation = transform.rotation.eulerAngles.z;
            float randomSpread = Random.Range(-currentWeaponData.spreadAngle / 2, currentWeaponData.spreadAngle / 2);
            Quaternion bulletRotation = Quaternion.Euler(0, 0, currentZRotation + randomSpread);

            bullet.transform.rotation = bulletRotation;

            if (bullet.TryGetComponent<Projectile>(out var projectile))
            {
                projectile.Setup(currentWeaponData.bulletSpeed, currentWeaponData.bulletLifeTime, currentWeaponData.bulletDamage,currentBulletEffect);
                projectile.ShootProjectile();
            }
            string soundName = currentWeaponData.name == "Shotgun_Data" ? "ShotgunFire" : "RifleFire";
            AudioManager.Instance.PlaySFX(soundName);
        }
    }

    public void SetTemporaryEffect(BulletEffect effect, float duration)
    {
        currentBulletEffect = effect;
        effectTimer = duration;
        maxEffectDuration = duration;
    }

    private IEnumerator Reload()
    {
        AmmoState state = ammoInventory[currentWeaponIndex];
        int bulletsNeeded = currentWeaponData.magazineSize - state.currentMag;

       
        if (bulletsNeeded <= 0 || state.currentTotalAmmo <= 0) yield break;

        isCurrentReloading = true;
        Debug.Log("Reloading...");
        AudioManager.Instance.PlaySFX("Reload");

        yield return new WaitForSeconds(currentWeaponData.reloadTime);

        int bulletsToReload = Mathf.Min(bulletsNeeded, state.currentTotalAmmo);
        state.currentMag += bulletsToReload;
        state.currentTotalAmmo -= bulletsToReload;

        isCurrentReloading = false;
        UpdateAmmoUI();
        
        Debug.Log("Reload Complete");
    }


    public void AddAmmo(int weaponIndex, int amount)
    {
      
        if (ammoInventory == null || weaponIndex < 0 || weaponIndex >= ammoInventory.Length)
        {
            
            return;
        }

        if (ammoInventory[weaponIndex] == null) return;

      
        int oldAmmo = ammoInventory[weaponIndex].currentTotalAmmo;

       
        ammoInventory[weaponIndex].currentTotalAmmo = Mathf.Clamp(
            ammoInventory[weaponIndex].currentTotalAmmo + amount,
            0,
            loadout[weaponIndex].maxTotalAmmo
        );

       

     
        if (weaponIndex == currentWeaponIndex)
        {
            UpdateAmmoUI();
        }
    }

    private void UpdateAmmoUI()
    {
        if (!isPlayer) return;

        AmmoState state = ammoInventory[currentWeaponIndex];
        OnAmmoChanged?.Invoke(state.currentMag, state.currentTotalAmmo);
    }

    public void TryEnemyShoot()
    {
        if (Time.time >= nextFireTime)
        {
            Shoot();
        }
    }

    private void EquipWeapon(int index)
    {
        if (index < 0 || index >= loadout.Length || loadout[index] == null) return;

        
        StopAllCoroutines();
        isCurrentReloading = false;

        currentWeaponIndex = index;
        currentWeaponData = loadout[currentWeaponIndex];
        weaponRenderer.sprite = currentWeaponData.weaponSprite;

        UpdateAmmoUI();
    }

    private void HandleWeaponSwitch()
    {
        if (Keyboard.current.digit1Key.wasPressedThisFrame) EquipWeapon(0);
        if (Keyboard.current.digit2Key.wasPressedThisFrame) EquipWeapon(1);
    }

    private void HandleAiming()
    {
        Vector2 mouseScreenPosition = Mouse.current.position.ReadValue();
        Vector3 mouseWorldPosition = cam.ScreenToWorldPoint(new Vector3(mouseScreenPosition.x, mouseScreenPosition.y, cam.nearClipPlane));
        mouseWorldPosition.z = 0;

        Vector3 lookDirection = mouseWorldPosition - transform.position;
        float angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;

        Quaternion targetRotation = Quaternion.Euler(0, 0, angle);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    private void UpdateWeaponVisuals()
    {
        float zAngle = transform.eulerAngles.z;
        weaponRenderer.flipY = (zAngle > 90f && zAngle < 270f);
    }
}