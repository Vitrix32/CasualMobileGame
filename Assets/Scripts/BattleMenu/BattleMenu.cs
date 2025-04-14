using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;

public class BattleMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject FadePanel;
    public GameObject attackPanel;
    public GameObject spellPanel;
    public GameObject helpPanel;
    public GameObject mainMenuPanel;

    private int enemiesAmount;

    // Added list to store all buttons in the battle menus
    private List<Button> allButtons = new List<Button>();

    private void Start()
    {
        enemiesAmount = 1;
        FadePanel = GameObject.Find("FadePanel");

        // Initialize allButtons list by finding all Button components in the panels
        InitializeAllButtons();
    }

    private void InitializeAllButtons()
    {
        if (attackPanel != null)
        {
            Button[] attackButtons = attackPanel.GetComponentsInChildren<Button>();
            allButtons.AddRange(attackButtons);
        }

        if (spellPanel != null)
        {
            Button[] spellButtons = spellPanel.GetComponentsInChildren<Button>();
            allButtons.AddRange(spellButtons);
        }

        if (helpPanel != null)
        {
            Button[] helpButtons = helpPanel.GetComponentsInChildren<Button>();
        }

        if (mainMenuPanel != null)
        {
            Button[] mainMenuButtons = mainMenuPanel.GetComponentsInChildren<Button>();
            allButtons.AddRange(mainMenuButtons);
        }
    }

    // Method to enable or disable all menu buttons
    public void SetMenusInteractable(bool isInteractable)
    {
        foreach (Button btn in allButtons)
        {
            btn.interactable = isInteractable;
        }
    }

    public void ShowAttacks()
    {
        mainMenuPanel.SetActive(false);
        attackPanel.SetActive(true);
    }

    public void ShowSpells()
    {
        mainMenuPanel.SetActive(false);
        spellPanel.SetActive(true);
    }

    public void ShowHelp()
    {
        mainMenuPanel.SetActive(false);
        helpPanel.SetActive(true);
    }

    public void BackToMenu()
    {
        attackPanel.SetActive(false);
        spellPanel.SetActive(false);
        helpPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }

    public void Flee()
    {
        GameObject enemy = GameObject.FindGameObjectWithTag("Enemy");
        if (PlayerPrefs.GetInt("LocID") == 3)
        {
            FadePanel.GetComponent<SceneTransition>().End();

            // Fade out enemy
            
            enemy.GetComponent<Fade>().ChangeColor(Vector3.zero, 1.3f);

            // Deactivate after fade
            Destroy(enemy, 1.0f);

            PlayerPrefs.SetInt("TempYVal", 4);
            PlayerPrefs.SetInt("LocID", 2);

            Invoke("ExitScene", 2.0f);
        } 
        else
        {
            // Fade out enemy
            enemy.GetComponent<Fade>().ChangeColor(Vector3.zero, 1.3f);
            // Deactivate after fade
            Destroy(enemy, 1f);
            AllEnemiesDead();
        }
    }

    public void EnemyNeutralized()
    {
        enemiesAmount -= 1;
        if (enemiesAmount == 0)
        {
            AllEnemiesDead();
            if(PlayerPrefs.GetInt("LocID")==6)
            {
                SceneManager.LoadScene("EndScreen");
            }
        }
    }

    public void AllEnemiesDead()
    {
        FadePanel.GetComponent<SceneTransition>().End();
        if (PlayerPrefs.GetInt("LocID") == 3)
        {
            //FindObjectOfType<DungeonBossEncounter>().IsDungeonBossEncounter = false;
            PlayerPrefs.SetInt("LocID", 2);
            PlayerPrefs.SetInt("DunBoss", 1);
        }
        if (PlayerPrefs.GetInt("LocID") == 4)
        {
            //FindObjectOfType<DungeonBossEncounter>().IsDungeonBossEncounter = false;
            PlayerPrefs.SetInt("LocID", 0);
            PlayerPrefs.SetInt("DunBoss", 1);
            // DO SOMETHING HERE FOR GAME OVER
        }
        Invoke("ExitScene", 2.0f);
    }

    private void ExitScene()
    {
        GameObject.Find("WorldPlayer").GetComponent<PlayerStatus>().EnteringGameWorld(true, 0.0f);
        SceneManager.LoadScene("GameplayScene");
    }
}
