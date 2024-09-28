using UnityEngine;
using UnityEngine.UI;

public class AttackManager : MonoBehaviour
{
    public GameObject attackPanel;
    public GameObject mainMenuPanel;

    public Enemy enemy;  // Reference to the Enemy
    //public Text battleLog; // A Text UI element to display battle events

    // Example attack methods
    public void QuickPunch()
    {
        int damage = 30; // Set damage amount
        enemy.TakeDamage(damage); // Apply damage to the enemy
        attackPanel.SetActive(false);      // Show attack panel
        mainMenuPanel.SetActive(true);      // Show attack panel

        //UpdateBattleLog("Player used Sword Slash! It dealt " + damage + " damage.");
    }

    public void FlamingSword()
    {
        int damage = 40; // Set damage amount
        enemy.TakeDamage(damage); // Apply damage to the enemy
        attackPanel.SetActive(false);      // Show attack panel
        mainMenuPanel.SetActive(true);      // Show attack panel
        //UpdateBattleLog("Player cast Fireball! It dealt " + damage + " damage.");
    }

    /*
    // Optional: Update the battle log in the UI
    void UpdateBattleLog(string message)
    {
        if (battleLog != null)
        {
            battleLog.text += message + "\n"; // Add the message to the battle log
        }
    }*/
}
