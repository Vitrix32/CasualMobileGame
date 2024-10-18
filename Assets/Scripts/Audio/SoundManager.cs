using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    [SerializeField] Slider musicVolumeSlider;

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
    }

    public void ChangeMusicVolume()
    {
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
}
