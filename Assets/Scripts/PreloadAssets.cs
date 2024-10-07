using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PreloadAssets : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameObject player = GameObject.Find("WorldPlayer");
        DontDestroyOnLoad(player);
        player.GetComponent<SpriteRenderer>().enabled = true;
        DontDestroyOnLoad(GameObject.Find("RandomEncounter")); //Remove later
        //SceneManager.LoadScene("IsaacTestScene");
        GameObject.Find("UniversalAudio").GetComponent<UniversalAudioHandling>().ExitingCombat();
        SceneManager.LoadScene("GameplayScene");
    }
}
