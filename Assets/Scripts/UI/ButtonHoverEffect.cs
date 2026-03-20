using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;


public class ButtonHoverEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [Header("Componentes")]
    [SerializeField] private Image buttonBackground;
    [SerializeField] private TextMeshProUGUI buttonText;

    [Header("Cores de Hover")]
    [SerializeField] private Color bgHoverColor = Color.white;
    [SerializeField] private Color textHoverColor = Color.black;

    [Header("Configurações de Áudio")]
    [SerializeField] private string clickSoundName = "ButtonClick";
    [SerializeField] private string hoverSoundName = "ButtonHover";
    [SerializeField] private bool playHoverSound = false;

    private Color originalBgColor;
    private Color originalTextColor;

    private void Awake()
    {
        if (buttonBackground == null) buttonBackground = GetComponent<Image>();
        if (buttonText == null) buttonText = GetComponentInChildren<TextMeshProUGUI>();

        if (buttonBackground != null) originalBgColor = buttonBackground.color;
        if (buttonText != null) originalTextColor = buttonText.color;
    }

    
    public void OnPointerClick(PointerEventData eventData)
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySFX(clickSoundName);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (buttonBackground != null) buttonBackground.color = bgHoverColor;
        if (buttonText != null) buttonText.color = textHoverColor;

        if (playHoverSound && AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySFX(hoverSoundName);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (buttonBackground != null) buttonBackground.color = originalBgColor;
        if (buttonText != null) buttonText.color = originalTextColor;
    }

    private void OnDisable()
    {
        if (buttonBackground != null) buttonBackground.color = originalBgColor;
        if (buttonText != null) buttonText.color = originalTextColor;
    }
}