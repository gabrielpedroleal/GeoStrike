using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Slider healthSlider;
    [SerializeField] private Health playerHealth;
    [SerializeField] private Image healthIcon;
    [SerializeField] private Color damageFlashColor = Color.white;
    private Color originalIconColor;

    [SerializeField] private Image ammoIcon;
    [SerializeField] private TextMeshProUGUI ammoText;
    [SerializeField] private WeaponComponent playerWeapon;

    [SerializeField] private Animator ammoAnimator;

    private void Start()
    {
        if(healthIcon != null) originalIconColor = healthIcon.color;

        if(playerHealth != null)
        {
            healthSlider.maxValue = playerHealth.MaxHealth;
            healthSlider.value = playerHealth.MaxHealth;

            playerHealth.OnHealthChanged += UpdateHealthBar;
            playerHealth.OnDamageTaken += FlashHealthIcon;
        }

        if(playerWeapon != null)
        {
            playerWeapon.OnAmmoChanged += UpdateAmmoText;
        }
    }

    private void UpdateHealthBar(float currentHealth)
    {
        healthSlider.value = currentHealth;
    }

    private void UpdateAmmoText(int magazine, int totalAmmo)
    {
        ammoText.text = $"{magazine}/{totalAmmo}";
        if (ammoAnimator != null)
        {
            ammoAnimator.SetBool("isEmpty", magazine == 0);
        }
    }

    private void FlashHealthIcon()
    {
        if(healthIcon != null && gameObject.activeInHierarchy)
        {
            StartCoroutine(FlashHealthIconRoutine());
        }
    }

    private System.Collections.IEnumerator FlashHealthIconRoutine()
    {
        healthIcon.color = damageFlashColor;
        yield return new WaitForSeconds(0.15f);
        healthIcon.color = originalIconColor;
    }

    private void OnDestroy()
    {
        if (playerHealth != null) 
        {
            playerHealth.OnHealthChanged -= UpdateHealthBar;
            playerHealth.OnDamageTaken -= FlashHealthIcon;
        }
        if(playerWeapon != null) playerWeapon.OnAmmoChanged -= UpdateAmmoText;
    }
}
