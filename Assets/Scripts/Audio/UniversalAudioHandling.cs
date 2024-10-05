using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UniversalAudioHandling : MonoBehaviour
{
    [SerializeField] 
    private AudioSource gravePeacefulTown;
    [SerializeField]
    private AudioSource commonBattle;

    private AudioSource backgroundMusic;
    private AudioSource combatMusic;
    // Start is called before the first frame update
    void Start()
    {
        backgroundMusic = gravePeacefulTown;
        combatMusic = commonBattle;
        backgroundMusic.Play();
        /**
        if (SceneManager.GetActiveScene().name == "IsaacTestScene")
        {
            backgroundMusic.Play();
        }
        /**/
    }

    public void EnteringCombat()
    {
        backgroundMusic.Stop();
        combatMusic.Play();
    }

    public void ExitingCombat() 
    { 
        combatMusic.Stop();
        backgroundMusic.Play();
    }

    public void Die()
    {
        combatMusic.Stop();
        backgroundMusic.Stop();
    }
}
