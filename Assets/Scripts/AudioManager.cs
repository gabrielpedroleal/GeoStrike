using UnityEngine;
using UnityEngine.Audio;
using System;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Mixer")]
    [SerializeField] private AudioMixer mainMixer;

    [Header("Canais de Áudio")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;

    [Header("Biblioteca de Áudios")]
    public Sound[] musicTracks;
    public Sound[] sfxClips;

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

    
    public void PlayMusic(string name)
    {
        Sound s = Array.Find(musicTracks, x => x.name == name);
        if (s == null) return;

        if (musicSource.isPlaying)
        {
            musicSource.Stop();
        }

        musicSource.clip = s.clip;
        musicSource.Play();

        musicSource.loop = (name != "VictoryTheme");
    }

    

    public void PlaySFX(string name)
    {
        Sound s = Array.Find(sfxClips, x => x.name == name);

        if (s == null) return;
        sfxSource.volume = s.volume;
        Debug.Log($"Tocando {s.name} com volume definido em: {s.volume}");
        sfxSource.PlayOneShot(s.clip, s.volume);
    }


    public void SetVolume(string parameterName, float sliderValue)
    {
      
        float dB = Mathf.Log10(Mathf.Max(sliderValue, 0.0001f)) * 20f;
        mainMixer.SetFloat(parameterName, dB);
    }
}

[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;
    [Range(0f, 1f)] public float volume = 1f;
}