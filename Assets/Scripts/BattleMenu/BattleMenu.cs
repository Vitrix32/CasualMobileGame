using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BattleMenu : MonoBehaviour
{
    private GameObject UniversalAudio;
    private GameObject FadePanel;
    // References to the panels and main menu
    public GameObject attackPanel;
    public GameObject spellPanel;
    public GameObject mainMenuPanel;

    private void Start()
    {
        UniversalAudio = GameObject.Find("UniversalAudio");
        FadePanel = GameObject.Find("FadePanel");
    }

    // Method to show the Attack options
    public void ShowAttacks()
    {
        mainMenuPanel.SetActive(false);   // Hide main menu
        attackPanel.SetActive(true);      // Show attack panel
    }

    // Method to show the Spell options
    public void ShowSpells()
    {
        mainMenuPanel.SetActive(false);   // Hide main menu
        spellPanel.SetActive(true);       // Show spell panel
    }

    // Method to return back to the main menu
    public void BackToMenu()
    {
        attackPanel.SetActive(false);     // Hide attack panel
        spellPanel.SetActive(false);      // Hide spell panel
        mainMenuPanel.SetActive(true);    // Show main menu
    }

    // Method to handle Fleeing from battle
    public void Flee()
    {
        Debug.Log("Flee from battle!");   // Just a log for now, you can replace this with actual flee logic
        FadePanel.GetComponent<SceneTransition>().End();
        //UniversalAudio.GetComponent<UniversalAudioHandling>().ExitingCombat();
        Invoke("ExitScene", 3.1f);
    }

    private void ExitScene()
    {
        GameObject.Find("WorldPlayer").GetComponent<PlayerStatus>().EnableControl();
        GameObject.Find("WorldPlayer").GetComponent<PlayerStatus>().ExitingCombat();
        UniversalAudio.GetComponent<UniversalAudioHandling>().ExitingCombat();
        SceneManager.LoadScene("IsaacTestScene");
    }
}
