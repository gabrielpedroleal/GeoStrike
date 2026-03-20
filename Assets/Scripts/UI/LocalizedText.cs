using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class LocalizedText : MonoBehaviour
{
    [Header("Traduções")]
    [TextArea(2, 5)] public string englishText;
    [TextArea(2, 5)] public string portugueseText;

    private TextMeshProUGUI textComponent;

    private void Start()
    {
        textComponent = GetComponent<TextMeshProUGUI>();

     
        if (LocalizationManager.Instance != null)
        {
            LocalizationManager.Instance.OnLanguageChanged += UpdateText;
        }

        UpdateText(); 
    }

    private void UpdateText()
    {
        if (textComponent == null) return;

        int currentLang = PlayerPrefs.GetInt("Language", 0);
        textComponent.text = (currentLang == 0) ? englishText : portugueseText;
    }

    private void OnDestroy()
    {
 
        if (LocalizationManager.Instance != null)
        {
            LocalizationManager.Instance.OnLanguageChanged -= UpdateText;
        }
    }
}