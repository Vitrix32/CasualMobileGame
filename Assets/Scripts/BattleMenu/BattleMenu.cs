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
    public GameObject itemPanel;
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

        if (itemPanel != null)
        {
            Button[] itemButtons = itemPanel.GetComponentsInChildren<Button>();
            allButtons.AddRange(itemButtons);
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

    public void ShowItems()
    {
        mainMenuPanel.SetActive(false);
        itemPanel.SetActive(true);
    }

    public void BackToMenu()
    {
        attackPanel.SetActive(false);
        spellPanel.SetActive(false);
        itemPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }

    public void Flee()
    {
        if (PlayerPrefs.GetInt("LocID") == 3)
        {
            FadePanel.GetComponent<SceneTransition>().End();
            // Fade out all enemies
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (GameObject enemy in enemies)
            {
                // Add Fade component if it doesn't exist
                Fade fadeComponent = enemy.GetComponent<Fade>();
                if (fadeComponent == null)
                {
                    fadeComponent = enemy.AddComponent<Fade>();
                }
                // Start fade out over 1 second
                fadeComponent.startFade(0f, 1.0f);
                // Deactivate after fade
                Destroy(enemy, 1f);
            }

            PlayerPrefs.SetInt("TempYVal", 4);
            PlayerPrefs.SetInt("LocID", 2);

            Invoke("ExitScene", 2.0f);
        } 
        else
        {
            // Fade out all enemies
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (GameObject enemy in enemies)
            {
                Fade fadeComponent = enemy.GetComponent<Fade>();
                if (fadeComponent == null)
                {
                    fadeComponent = enemy.AddComponent<Fade>();
                }
                fadeComponent.startFade(0f, 1f);
                Destroy(enemy, 1f);
            }
            AllEnemiesDead();
        }
    }

    public void EnemyNeutralized()
    {
        enemiesAmount -= 1;
        if (enemiesAmount == 0)
        {
            AllEnemiesDead();
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
