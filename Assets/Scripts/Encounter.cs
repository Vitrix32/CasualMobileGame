using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Encounter : MonoBehaviour
{
    private GameObject player;
    private int enemyId;
    private int numberOfIds;
    //private int enemyAmount;
    public bool encountered;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("WorldPlayer");
        numberOfIds = 5;
        enemyId = Random.Range(1, numberOfIds + 1);
        encountered = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!encountered && collision == player.GetComponent<Collider2D>())
        {
            encountered = true;
            this.GetComponent<SpriteRenderer>().enabled = false;
            DontDestroyOnLoad(this.gameObject);
            player.GetComponent<PlayerStatus>().enteringCombat();
            SceneManager.LoadScene("JoelTestScene");
        }
    }
}
