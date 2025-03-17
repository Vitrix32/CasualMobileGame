using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    [SerializeField] Slider VolumeSlider;

    // Start is called before the first frame update
    void Start()
    {
        if (!PlayerPrefs.HasKey("Volume"))
        {
            PlayerPrefs.SetFloat("Volume", 1);
            LoadVolume();
            PlayerPrefs.SetFloat("MusicVolume", 1);
            LoadMusicVolume();
            PlayerPrefs.SetFloat("SFXVolume", 1);
            LoadSFXVolume();
            PlayerPrefs.SetFloat("UIVolume", 1);
            LoadUIVolume();
        } 
        else
        {
            LoadVolume();
            LoadMusicVolume();
            LoadSFXVolume();
            LoadUIVolume();
        }
    }

    public void ChangeVolume()
    {
        //Not Audio Listener
        AudioListener.volume = VolumeSlider.value;
        SaveVolume();
    }

    private void SaveVolume()
    {
        PlayerPrefs.SetFloat("Volume", VolumeSlider.value);
    }

    private void LoadVolume()
    {
        VolumeSlider.value = PlayerPrefs.GetFloat("Volume");
    }

    //music
    public void ChangeMusicVolume()
    {
        //Not Audio Listener
        GameObject player = GameObject.Find("WorldPlayer");
        player.GetComponent<UniversalAudioHandling>().ChangeSourceVolume("Music", VolumeSlider.value);
        SaveMusicVolume();
    }

    private void SaveMusicVolume()
    {
        PlayerPrefs.SetFloat("MusicVolume", VolumeSlider.value);
    }

    private void LoadMusicVolume()
    {
        VolumeSlider.value = PlayerPrefs.GetFloat("MusicVolume");
    }

    //effects
    public void ChangeSFXVolume()
    {
        //Not Audio Listener
        GameObject player = GameObject.Find("WorldPlayer");
        player.GetComponent<UniversalAudioHandling>().ChangeSourceVolume("SFX", VolumeSlider.value);
        SaveSFXVolume();
    }

    private void SaveSFXVolume()
    {
        PlayerPrefs.SetFloat("SFXVolume", VolumeSlider.value);
    }

    private void LoadSFXVolume()
    {
        VolumeSlider.value = PlayerPrefs.GetFloat("SFXVolume");
    }

    //UI
    public void ChangeUIVolume()
    {
        //Not Audio Listener
        GameObject player = GameObject.Find("WorldPlayer");
        player.GetComponent<UniversalAudioHandling>().ChangeSourceVolume("UI", VolumeSlider.value);
        SaveUIVolume();
    }

    private void SaveUIVolume()
    {
        PlayerPrefs.SetFloat("UIVolume", VolumeSlider.value);
    }

    private void LoadUIVolume()
    {
        VolumeSlider.value = PlayerPrefs.GetFloat("UIVolume");
    }
}
