using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{

    [SerializeField]
    private GameObject WorldPlayer;
    public GameObject MapPanel;
    public GameObject OpenMapButton;
    public GameObject PauseButton;
    public GameObject QuestsButton;

    void Start()
    {
        WorldPlayer = GameObject.Find("WorldPlayer");
    }

    public void OpenMap()
    {
        WorldPlayer.GetComponent<UniversalAudioHandling>().ButtonPressed();
        MapPanel.SetActive(true);
        WorldPlayer.GetComponent<UniversalAudioHandling>().Pause();
        OpenMapButton.SetActive(false);
        PauseButton.SetActive(false);
        QuestsButton.SetActive(false);
        Time.timeScale = 0f;
    }

    public void CloseMap()
    {
        WorldPlayer.GetComponent<UniversalAudioHandling>().ButtonPressed();
        MapPanel.SetActive(false);
        WorldPlayer.GetComponent<UniversalAudioHandling>().Resume();
        OpenMapButton.SetActive(true);
        PauseButton.SetActive(true);
        QuestsButton.SetActive(true);
        Time.timeScale = 1f;
    }
}
