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

    //The indexes in the backgroundMusics array where each area's
    //background music is stored
    [SerializeField]
    private int startvilleMusicIndex;
    [SerializeField]
    private int newCastleMusicIndex;
    [SerializeField]
    private int otherAreaMusicIndex;
    [SerializeField]
    private int scorchedMountMusicIndex;
    [SerializeField]
    private int roadToNewCastleMusicIndex;
    [SerializeField]
    private int roadToOtherAreaMusicIndex;
    [SerializeField]
    private int roadToScorchedMountMusicIndex;
    //The indexes in the combatMusics array where each area's
    //combat music is stored
    [SerializeField]
    private int startvilleCombatMusicIndex;
    [SerializeField]
    private int newCastleCombatMusicIndex;
    [SerializeField]
    private int otherAreaCombatMusicIndex;
    [SerializeField]
    private int scorchedMountCombatMusicIndex;
    [SerializeField]
    private int roadToNewCastleCombatMusicIndex;
    [SerializeField]
    private int roadToOtherAreaCombatMusicIndex;
    [SerializeField]
    private int roadToScorchedMountCombatMusicIndex;

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

    public void NewScene()
    {

    }
}
