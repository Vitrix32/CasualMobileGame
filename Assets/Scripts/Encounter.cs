using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Encounter : MonoBehaviour
{
    private int enemyAmount;
    // Start is called before the first frame update
    void Start()
    {
        //Add code to randomize later
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        this.GetComponent<Collider2D>().enabled = false;
        GameObject.Find("Player").GetComponent<PlayerMovement>().enabled = false;
        SceneManagement.DontDestroyOnLoad(GameObject.Find("Player"));
        SceneManager.LoadScene(0);
    }
}
