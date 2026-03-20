using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OptionsMenu : MonoBehaviour
{
    [Header("Componentes de UI")]
    [SerializeField] private TMP_Dropdown windowModeDropdown;
    [SerializeField] private TMP_Dropdown languageDropdown;
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;

    private void Start()
    {
  
        LoadSettings();
    }

    private void LoadSettings()
    {

        masterSlider.value = PlayerPrefs.GetFloat("MasterVolume", 1f);
        musicSlider.value = PlayerPrefs.GetFloat("MusicVol", 1f);
        sfxSlider.value = PlayerPrefs.GetFloat("SFXVol", 1f);

        languageDropdown.value = PlayerPrefs.GetInt("Language", 0);

       
        int savedWindowMode = PlayerPrefs.GetInt("WindowMode", GetCurrentWindowModeIndex());
        windowModeDropdown.value = savedWindowMode;
        SetWindowMode(savedWindowMode);
    }

    public void SetMasterVolume(float volume)
    {
        PlayerPrefs.SetFloat("MasterVol", volume);
        if (AudioManager.Instance != null) AudioManager.Instance.SetVolume("MasterVol", volume);
    }

    public void SetMusicVolume(float volume)
    {
        PlayerPrefs.SetFloat("MusicVol", volume);
        if (AudioManager.Instance != null) AudioManager.Instance.SetVolume("MusicVol", volume);
    }

    public void SetSFXVolume(float volume)
    {
        PlayerPrefs.SetFloat("SFXVol", volume);
        if (AudioManager.Instance != null) AudioManager.Instance.SetVolume("SFXVol", volume);
    }

    public void SetWindowMode(int index)
    {
        switch (index)
        {
            case 0:
                Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
                break;
            case 1:
                Screen.fullScreenMode = FullScreenMode.FullScreenWindow; 
                break;
            case 2:
                Screen.fullScreenMode = FullScreenMode.Windowed;
                break;
            default:
           
                break;
        }
        PlayerPrefs.SetInt("WindowMode", index);
    }

    public void SetLanguage(int index)
    {
        if (LocalizationManager.Instance != null)
        {
            LocalizationManager.Instance.ChangeLanguage(index);
        }
        else
        {
            PlayerPrefs.SetInt("Language", index);
        }
    }

    private int GetCurrentWindowModeIndex()
    {
        if (Screen.fullScreenMode == FullScreenMode.ExclusiveFullScreen) return 0;
        if (Screen.fullScreenMode == FullScreenMode.FullScreenWindow) return 1;
        return 2;
    }
}