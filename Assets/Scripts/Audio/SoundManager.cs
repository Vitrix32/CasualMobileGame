using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    [SerializeField] GameObject WorldPlayer;
    [SerializeField] Slider VolumeSlider;
    [SerializeField] Slider MusicVolumeSlider;
    [SerializeField] Slider SFXVolumeSlider;
    [SerializeField] Slider UIVolumeSlider;

    // Start is called before the first frame update
    void Start()
    {
        WorldPlayer = GameObject.Find("WorldPlayer");
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
        Debug.Log("VOLUME SAVE");
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
        WorldPlayer.GetComponent<UniversalAudioHandling>().ChangeSourceVolume("Music", MusicVolumeSlider.value);
        SaveMusicVolume();
    }

    private void SaveMusicVolume()
    {
        PlayerPrefs.SetFloat("MusicVolume", MusicVolumeSlider.value);
        Debug.Log("MUSIC VOLUME SAVE");
    }

    private void LoadMusicVolume()
    {
        MusicVolumeSlider.value = PlayerPrefs.GetFloat("MusicVolume");
    }

    //effects
    public void ChangeSFXVolume()
    {
        //Not Audio Listener
        GameObject player = GameObject.Find("WorldPlayer");
        WorldPlayer.GetComponent<UniversalAudioHandling>().ChangeSourceVolume("SFX", SFXVolumeSlider.value);
        SaveSFXVolume();
    }

    private void SaveSFXVolume()
    {
        PlayerPrefs.SetFloat("SFXVolume", SFXVolumeSlider.value);
        Debug.Log("SFX VOLUME SAVE");
    }

    private void LoadSFXVolume()
    {
        SFXVolumeSlider.value = PlayerPrefs.GetFloat("SFXVolume");
    }

    //UI
    public void ChangeUIVolume()
    {
        //Not Audio Listener
        GameObject player = GameObject.Find("WorldPlayer");
        WorldPlayer.GetComponent<UniversalAudioHandling>().ChangeSourceVolume("UI", UIVolumeSlider.value);
        SaveUIVolume();
    }

    private void SaveUIVolume()
    {
        PlayerPrefs.SetFloat("UIVolume", UIVolumeSlider.value);
        Debug.Log("UI VOLUME SAVE");
    }

    private void LoadUIVolume()
    {
        UIVolumeSlider.value = PlayerPrefs.GetFloat("UIVolume");
    }
}
