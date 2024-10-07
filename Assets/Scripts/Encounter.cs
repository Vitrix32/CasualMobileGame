using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Encounter : MonoBehaviour
{
    private GameObject UniversalAudio;
    private GameObject FadePanel;
    private GameObject player;
    private int enemyId;
    private int numberOfIds;
    //private int enemyAmount;
    public bool encountered;
    // Start is called before the first frame update
    void Start()
    {
        UniversalAudio = GameObject.Find("UniversalAudio");
        //FadePanel = GameObject.Find("FadePanel");
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
            DontDestroyOnLoad(this.gameObject);
            UniversalAudio.GetComponent<UniversalAudioHandling>().EnteringCombat();
            FadePanel = GameObject.Find("FadePanel"); //Will remove this later once this object is no longer preloaded
            FadePanel.GetComponent<SceneTransition>().End();
            player.GetComponent<PlayerStatus>().DisableControl();
            StartCoroutine("ChangeScene");
        }
    }

    public void playerDead()
    {
        player.transform.position = Vector2.zero;
        encountered = false;
        this.GetComponent<SpriteRenderer>().enabled = true;
    }

    IEnumerator ChangeScene()
    {
        yield return new WaitForSeconds(3.1f);
        this.GetComponent<SpriteRenderer>().enabled = false;
        player.GetComponent<PlayerStatus>().EnteringCombat();
        SceneManager.LoadScene("JoelTestScene");
    }
}
