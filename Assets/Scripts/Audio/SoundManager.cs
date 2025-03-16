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
        } 
        else
        {
            LoadVolume();
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
}
