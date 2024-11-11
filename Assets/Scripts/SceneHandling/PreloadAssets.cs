using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PreloadAssets : MonoBehaviour
{
    [SerializeField]
    private GameObject player;
    [SerializeField]
    private GameObject debugMenu;
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(player);
        DontDestroyOnLoad(debugMenu);
        player.GetComponent<SpriteRenderer>().enabled = true;
        SceneManager.LoadScene("MainMenu");
    }
}
