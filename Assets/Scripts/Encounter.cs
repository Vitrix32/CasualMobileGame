using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Encounter : MonoBehaviour
{
    [SerializeField]
    private float delay;
    private GameObject FadePanel;
    private GameObject player;
    private int enemyId;
    private int numberOfIds;
    //private int enemyAmount;
    // Start is called before the first frame update
    void Start()
    {
        FadePanel = GameObject.Find("FadePanel");
        numberOfIds = 5;
        enemyId = Random.Range(1, numberOfIds + 1);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            int rand = Random.Range(0, 10);
            if(rand == 0)
            {
                FadePanel.GetComponent<SceneTransition>().End();
                collision.gameObject.GetComponent<PlayerStatus>().LeavingGameWorld(true, 3.1f);
                StartCoroutine("ChangeScene");
            }
        }
    }

    private IEnumerator ChangeScene()
    {
        yield return new WaitForSeconds(3.1f);
        SceneManager.LoadScene("JoelTestScene");
    }
}
