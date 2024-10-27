using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UniversalAudioHandling : MonoBehaviour
{
    [SerializeField]
    private AudioSource primaryMusicSource;
    [SerializeField]
    private AudioSource primaryUISource;
    [SerializeField]
    private AudioClip[] backgroundMusics;
    [SerializeField]
    private AudioClip[] combatMusics;

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
        primaryMusicSource.clip = combatMusic;
        primaryMusicSource.Play();
    }

    public void ExitingCombat() 
    {
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
}
