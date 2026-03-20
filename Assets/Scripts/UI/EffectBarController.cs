using UnityEngine;
using UnityEngine.UI;

public class EffectBarController : MonoBehaviour
{
    [Header("Componentes de UI")]
    [SerializeField] private GameObject barRoot;
    [SerializeField] private Image fillImage; 

    [Header("Cores do Efeito")]
    [SerializeField] private Color fireColor = new Color(1f, 0.5f, 0f);
    [SerializeField] private Color electricColor = Color.yellow; 

    private WeaponComponent playerWeapon;

    private void Start()
    {
       
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerWeapon = player.GetComponentInChildren<WeaponComponent>();
        }

  
        if (barRoot != null) barRoot.SetActive(false);
    }

    private void Update()
    {
        if (playerWeapon == null || barRoot == null || fillImage == null) return;

        if (playerWeapon.CurrentEffect != BulletEffect.None)
        {
           
            if (!barRoot.activeSelf) barRoot.SetActive(true);

            fillImage.fillAmount = playerWeapon.EffectProgress;

            if (playerWeapon.CurrentEffect == BulletEffect.Fire)
            {
                fillImage.color = fireColor;
            }
            else if (playerWeapon.CurrentEffect == BulletEffect.Electric)
            {
                fillImage.color = electricColor;
            }
        }
        else
        {
            if (barRoot.activeSelf) barRoot.SetActive(false);
        }
    }
}