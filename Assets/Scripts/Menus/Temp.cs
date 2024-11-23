using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Temp : MonoBehaviour
{
    [SerializeField]
    private GameObject WorldPlayer;
    void Start()
    {
        WorldPlayer = GameObject.Find("WorldPlayer");
    }
    public void Die()
    {
        WorldPlayer.GetComponent<PlayerStatus>().LeavingGameWorld(false, 0.4f);
        SceneManager.LoadScene("DeathScene");
    }
}
