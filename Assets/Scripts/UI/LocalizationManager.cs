using UnityEngine;
using System;

public class LocalizationManager : MonoBehaviour
{
    public static LocalizationManager Instance { get; private set; }

   
    public event Action OnLanguageChanged;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ChangeLanguage(int languageIndex)
    {
        PlayerPrefs.SetInt("Language", languageIndex);
        OnLanguageChanged?.Invoke(); 
    }

    public int GetCurrentLanguage()
    {
        return PlayerPrefs.GetInt("Language", 0);
    }
}