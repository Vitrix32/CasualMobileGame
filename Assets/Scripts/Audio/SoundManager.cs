using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    [SerializeField] Slider musicVolumeSlider;
    [SerializeField] Slider SFXVolumeSlider;

    // Start is called before the first frame update
    void Start()
    {
        if (!PlayerPrefs.HasKey("MusicVolume"))
        {
            PlayerPrefs.SetFloat("MusicVolume", 1);
            LoadMusic();
        } 
        else
        {
            LoadMusic();
        }

        if (!PlayerPrefs.HasKey("SFXVolume"))
        {
            PlayerPrefs.SetFloat("SFXVolume", 1);
            LoadSFX();
        }
        else
        {
            Debug.Log("Here");
            LoadSFX();
        }
    }

    public void ChangeMusicVolume()
    {
        //Not Audio Listener
        AudioListener.volume = musicVolumeSlider.value;
        SaveMusic();
    }

    private void SaveMusic()
    {
        PlayerPrefs.SetFloat("MusicVolume", musicVolumeSlider.value);
    }

    private void LoadMusic()
    {
        musicVolumeSlider.value = PlayerPrefs.GetFloat("MusicVolume");
    }

    public void ChangeSFXVolume()
    {
        //Not Audio Listener
        AudioListener.volume = SFXVolumeSlider.value;
        SaveSFX();
    }

    private void SaveSFX()
    {
        PlayerPrefs.SetFloat("SFXVolume", SFXVolumeSlider.value);
    }

    private void LoadSFX() {
        Debug.Log(SFXVolumeSlider.value);
        SFXVolumeSlider.value = PlayerPrefs.GetFloat("SFXVolume");
    }
}
