using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject WorldPlayer;
    public GameObject LoadGameButton;
    public GameObject NewGameButton;
    public GameObject OptionsButton;
    public GameObject QuitButton;

    void Start()
    {
        WorldPlayer = GameObject.Find("WorldPlayer");
        Invoke("TurnButtonsOn", 3);
    }

    private void TurnButtonsOn()
    {
        LoadGameButton.SetActive(true);
        NewGameButton.SetActive(true);
        OptionsButton.SetActive(true);
        QuitButton.SetActive(true);
    }

    public void LoadGame()
    {
        SetLiveJSONToSave();

        WorldPlayer.GetComponent<UniversalAudioHandling>().ButtonPressed();
        WorldPlayer.GetComponent<PlayerStatus>().EnteringGameWorld(false, 0.4f);
        WorldPlayer.GetComponent<PlayerStatus>().SetWorldPosition();
        WorldPlayer.transform.position = new Vector3(PlayerPrefs.GetFloat("XPos"), PlayerPrefs.GetFloat("YPos"), 0);
        SceneManager.LoadScene(PlayerPrefs.GetString("SceneName"));
    }

    public void QuitGame()
    {
        WorldPlayer.GetComponent<UniversalAudioHandling>().ButtonPressed();
        Invoke("Quit", 0.4f);
    }

    private void SetLiveJSONToSave()
    {
        string quest = File.ReadAllText(Application.dataPath + "/Scripts/Dialogue/SaveQuests.txt");
        File.WriteAllText(Application.dataPath + "/Scripts/Dialogue/Quests.txt", quest);

        string dialogue = File.ReadAllText(Application.dataPath + "/Scripts/Dialogue/SaveDialogue.txt");
        File.WriteAllText(Application.dataPath + "/Scripts/Dialogue/Dialogue.txt", dialogue);

        string stats = File.ReadAllText(Application.dataPath + "/Scripts/Items/SavePlayerStats.txt");
        File.WriteAllText(Application.dataPath + "/Scripts/Items/PlayerStats.txt", stats);
    }

    private void Quit()
    {
        Debug.Log(Application.persistentDataPath);
        Application.Quit();
    }
}
