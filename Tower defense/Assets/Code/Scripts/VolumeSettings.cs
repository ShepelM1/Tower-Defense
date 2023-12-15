using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSettings : MonoBehaviour
{
    [SerializeField] Sprite soundOnImage;
    [SerializeField] Sprite soundOffImage;
    [SerializeField] AudioSource musicAudioSource;
    [SerializeField] AudioSource soundEffectsAudioSource;
    [SerializeField] Button musicButton;
    [SerializeField] Button soundEffectsButton;
    [SerializeField] private AudioMixer myMixer;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider SFXSlider;

    private bool isMusicOn = true;
    private bool isSoundEffectsOn = true;

    private void Start()
    {
        musicButton.onClick.AddListener(ToggleMusic);
        soundEffectsButton.onClick.AddListener(ToggleSoundEffects);
        if (PlayerPrefs.HasKey("musicVolume"))
        {
            LoadVolune();
        }
        else
        {
            SetMusicVolume();
            SetSFXVolume();
        }
    }

    public void SetMusicVolume()
    {
        float volume = musicSlider.value;
        myMixer.SetFloat("music", Mathf.Log10(volume) * 30);
        PlayerPrefs.SetFloat("musicVolume", volume);
    }

    public void SetSFXVolume()
    {
        float volume = SFXSlider.value;
        myMixer.SetFloat("SFX", Mathf.Log10(volume) * 30);
        PlayerPrefs.SetFloat("SFXVolume", volume);
    }

    private void LoadVolune()
    {
        musicSlider.value = PlayerPrefs.GetFloat("musicVolume");
        SFXSlider.value = PlayerPrefs.GetFloat("SFXVolume");

        SetMusicVolume();
        SetSFXVolume();
    }

    private void ToggleMusic()
    {
        isMusicOn = !isMusicOn;
        musicButton.image.sprite = isMusicOn ? soundOnImage : soundOffImage;
        musicAudioSource.mute = !isMusicOn;
    }

    private void ToggleSoundEffects()
    {
        isSoundEffectsOn = !isSoundEffectsOn;
        soundEffectsButton.image.sprite = isSoundEffectsOn ? soundOnImage : soundOffImage;
        soundEffectsAudioSource.mute = !isSoundEffectsOn;
    }
}


