using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;

/*
 * Name: UniversalAudioHandling
 * Author: Isaac Drury
 * Date: January 2025
 * Description:
 * This script handles all music, UI sound effects, and other universal sounds.
 */

public class UniversalAudioHandling : MonoBehaviour
{
    [SerializeField]
    private AudioSource primaryMusicSource;
    [SerializeField]
    private AudioSource primaryUISource;
    [SerializeField] 
    private AudioSource primaryAmbianceSource;
    [SerializeField]
    private AudioSource primaryFXSource;

    [SerializeField]
    private AudioClip[] backgroundMusics;
    [SerializeField]
    private AudioClip[] combatMusics;

    private GameObject musicHandler;

    [SerializeField]
    private AudioClip baseSceneMusic;
    private AudioClip backgroundMusic;
    private AudioClip combatMusic;

    // Start is called before the first frame update
    void Start()
    {
        backgroundMusic = backgroundMusics[0];
        combatMusic = combatMusics[0];
    }

    public void EnteringCombat()
    {
        primaryMusicSource.Stop();
        primaryMusicSource.clip = combatMusic;
        primaryMusicSource.Play();
    }

    public void ExitingCombat() 
    {
        primaryMusicSource.Stop();
        primaryMusicSource.clip = backgroundMusic;
        primaryMusicSource.Play();
    }

    public void Die()
    {
        primaryMusicSource.Stop();
    }

    public void Pause()
    {
        primaryMusicSource.Pause();
    }

    public void Resume()
    {
        primaryMusicSource.Play();
    }

    public void ButtonPressed()
    {
        primaryUISource.Play();
    }

    public void NewScene(string s)
    {
        musicSelection(s);
        primaryMusicSource.clip = backgroundMusic;
        primaryMusicSource.Play();

        //Find a music handler if it exists
        musicHandler=GameObject.Find("MusicHandler");


    }


    private void musicSelection(string sceneName)
    {
        if (sceneName == "GameplayScene")
        {
            backgroundMusic = baseSceneMusic;
            combatMusic = combatMusics[0];
        }
        else if (sceneName == "MainMenu")
        {
            backgroundMusic = backgroundMusics[0];
            combatMusic = combatMusics[0];
        }
        
    }

    public void enteredZone(string name)
    {
        Debug.Log("entered trigger thing in UAH");
        if(name == "groveZone")
        {
            switchMusic(4);
        }
        if (name == "dungeonZone")
        {
            switchMusic(2);
        }
        if (name == "mountainZone")
        {
            switchMusic(1);
        }
        if (name == "desertZone")
        {
            switchMusic(3);
        }
        if (name == "churchZone")
        {
            switchMusic(7);
        }
        if (name == "indoorZone")
        {
            switchMusic(5);
        }
        if (name == "trailZone")
        {
            switchMusic(6);
        }
    }

    public void exitedZone()
    {
        backgroundMusic = baseSceneMusic;
        primaryMusicSource.Stop();
        primaryMusicSource.clip = baseSceneMusic;
        primaryMusicSource.Play();
    }

    private void switchMusic(int index)
    {
        Debug.Log("switching Music");
        backgroundMusic=backgroundMusics[index];
        primaryMusicSource.Stop();
        primaryMusicSource.clip = backgroundMusic;
        primaryMusicSource.Play();
    }

    public void ChangeSourceVolume(string source, float level)
    {
        if(source =="Music")
            primaryMusicSource.volume = level;
        if(source=="Ambiance")
            primaryAmbianceSource.volume = level;
        if(source=="SFX")
            primaryFXSource.volume = level;
        if(source == "UI")
            primaryUISource.volume = level;
    }

}
