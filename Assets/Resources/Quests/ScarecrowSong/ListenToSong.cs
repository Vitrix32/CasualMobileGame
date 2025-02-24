using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListenToSong : QuestStep
{
    private GameObject musicHandler;
    [SerializeField]
    private AudioSource musicSource;
    protected override void SetQuestStepState(string state)
    {
        //things;
    }
    // Start is called before the first frame update
    void Start()
    {
        musicHandler = GameObject.Find("MusicHandler");
    }

    private void OnMouseDown()
    {
        StartCoroutine(PlayClip());
    }

    private IEnumerator PlayClip()
    {
        musicHandler.GetComponent<SceneMusicTest>().Pause();
        //Play scarecrow song
        musicSource.Play();
        yield return new WaitForSeconds(musicSource.clip.length);
        musicSource.Stop();
        musicHandler.GetComponent<SceneMusicTest>().Resume();
    }
}
