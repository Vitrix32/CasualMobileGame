using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootstepAudioHandling : MonoBehaviour
{
    [SerializeField]
    private AudioSource audioSource;
    [SerializeField] 
    private AudioClip[] currentFootsteps;
    [SerializeField] 
    private AudioClip[] brushFootsteps;
    [SerializeField]
    private AudioClip[] grassFootsteps;
    [SerializeField]
    private AudioClip[] sandFootsteps;
    [SerializeField] 
    private AudioClip[] stoneFootsteps;
    [SerializeField] 
    private AudioClip[] waterFootsteps;
    [SerializeField]
    private AudioClip[] woodFootsteps;
    private int prevRandom;
    private bool inProgress;
    private bool newTerrain;
    // Start is called before the first frame update
    void Start()
    {
        prevRandom = 0;
        inProgress = false;
        newTerrain = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!inProgress && this.GetComponent<PlayerMovement>().isMoving())
        {
            inProgress = true;
            StartCoroutine(PlayFootsteps());
        }
        else if (!this.GetComponent<PlayerMovement>().isMoving()) 
        {
            inProgress = false;
            StopAllCoroutines();
        }
    }

    private IEnumerator PlayFootsteps()
    {
        while (true) 
        {
            evaluateTerrain();
            int random = prevRandom;
            while (random == prevRandom)
            {
                random = Random.Range(0, currentFootsteps.Length);
            }
            audioSource.clip = currentFootsteps[random];
            audioSource.Play();
            yield return new WaitForSeconds(0.5f);
        }
    }

    private void evaluateTerrain()
    {
        if (newTerrain)
        {
            newTerrain = false;
            currentFootsteps = grassFootsteps;
        }
    }
}
