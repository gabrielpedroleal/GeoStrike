using UnityEngine;
using TMPro;
using System.Collections.Generic;

[RequireComponent(typeof(TMP_Dropdown))]
public class LocalizedDropdown : MonoBehaviour
{
    [System.Serializable]
    public struct DropdownOption
    {
        public string englishText;
        public string portugueseText;
    }

    [Header("Traduções das Opções")]
    public List<DropdownOption> localizedOptions;

    private TMP_Dropdown dropdown;

    private void Awake()
    {
        dropdown = GetComponent<TMP_Dropdown>();
    }

    private void Start()
    {
        if (LocalizationManager.Instance != null)
        {
            LocalizationManager.Instance.OnLanguageChanged += UpdateDropdownOptions;
        }
        UpdateDropdownOptions();
    }

    private void UpdateDropdownOptions()
    {
        if (dropdown == null) return;

        int currentLang = PlayerPrefs.GetInt("Language", 0);
        int currentSelection = dropdown.value;

        dropdown.options.Clear();

        foreach (var option in localizedOptions)
        {
            string text = (currentLang == 0) ? option.englishText : option.portugueseText;
            dropdown.options.Add(new TMP_Dropdown.OptionData(text));
        }

        dropdown.value = currentSelection;
        dropdown.RefreshShownValue();
    }

    private void OnDestroy()
    {
        if (LocalizationManager.Instance != null)
        {
            LocalizationManager.Instance.OnLanguageChanged -= UpdateDropdownOptions;
        }
    }
}