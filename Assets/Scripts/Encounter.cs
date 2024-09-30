using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Encounter : MonoBehaviour
{
    private GameObject player;
    private int enemyAmount;
    public bool encountered;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        encountered = false;
        //Add code to randomize later
    }

    // Update is called once per frame
    void Update()
    {
        if (encountered && Vector3.Distance(this.transform.position, player.transform.position) > 20.0f)
        {
            Destroy(this.gameObject);
        }
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
