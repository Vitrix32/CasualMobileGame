using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;

public class BattleMenu : MonoBehaviour
{
    private GameObject FadePanel;
    public GameObject attackPanel;
    public GameObject spellPanel;
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

    public void BackToMenu()
    {
        attackPanel.SetActive(false);
        spellPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }

    public void Flee()
    {
        AllEnemiesDead();
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
        Invoke("ExitScene", 3.1f);
    }

    private void ExitScene()
    {
        GameObject.Find("WorldPlayer").GetComponent<PlayerStatus>().EnteringGameWorld(true, 0.0f);
        SceneManager.LoadScene("GameplayScene");
    }
}
