using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Encounter : MonoBehaviour
{
    private float delay;
    private GameObject FadePanel;
    private GameObject player;
    private int enemyId;
    private int numberOfIds;

    // Start is called before the first frame update
    void Start()
    {
        delay = 3;
        FadePanel = GameObject.Find("FadePanel");
        numberOfIds = 4;
        enemyId = Random.Range(1, numberOfIds);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            int rand = Random.Range(0, 100);
            if(rand <= 25 && !collision.gameObject.GetComponent<PlayerStatus>().IsCombatImmune())
            {
                FadePanel.GetComponent<SceneTransition>().End();
                collision.gameObject.GetComponent<PlayerStatus>().LeavingGameWorld(true, delay);
                StartCoroutine("ChangeScene");
            }
        }
    }

    private IEnumerator ChangeScene()
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene("JoelTestScene");
    }
}
