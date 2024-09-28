using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Player : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;

    public Text healthText;
    public Slider healthSlider;

    public Button[] attackButtons;
    public Button[] spellButtons;


    private bool playerTurn = true;

    public Enemy enemy; // Reference to the enemy, which we can assign in the Inspector

    public GameObject attackPanel;

    public GameObject spellPanel;
    public GameObject mainMenuPanel;

    void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthUI();
        SetAttackButtonsInteractable(playerTurn);
    }

    // Method for taking damage from the enemy
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHealthUI();

        if (currentHealth <= 0)
        {
            Die();
            mainMenuPanel.SetActive(false);      // Show attack panel

        }
    }

    void UpdateHealthUI()
    {
        if (healthText != null)
        {
            healthText.text = "Health: " + currentHealth;
        }

        if (healthSlider != null)
        {
            healthSlider.value = (float)currentHealth / maxHealth;
        }
    }

    void Die()
    {
        Debug.Log("You died!");
        Destroy(gameObject); // Destroy enemy when health is 0

    }

    // General PlayerAttack method without parameters, for UI buttons
    public void PlayerAttack(int damage)
    {
        if (playerTurn && enemy != null)
        {
            enemy.TakeDamage(damage); // Apply damage to the enemy
            playerTurn = false;
            SetAttackButtonsInteractable(false);

            StartCoroutine(EnemyAttackTurn());

            attackPanel.SetActive(false);      // Show attack panel
            mainMenuPanel.SetActive(true);      // Show attack panel

        }
    }

    public void PlayerSkipSpell()
    {
        if (playerTurn && enemy != null)
        {
            playerTurn = false;
            SetSpellButtonsInteractable(false);

            StartCoroutine(EnemySkipTurn());

            spellPanel.SetActive(false);      // Show attack panel
            mainMenuPanel.SetActive(true);      // Show attack panel

        }
    }


    IEnumerator EnemyAttackTurn()
    {
        yield return new WaitForSeconds(1);
        enemy.EnemyAttack(this);
        playerTurn = true;
        SetAttackButtonsInteractable(true);
    }

    IEnumerator EnemySkipTurn()
    {
        yield return new WaitForSeconds(1);
        enemy.EnemySkip(this);
        playerTurn = true;
        SetSpellButtonsInteractable(true);
    }

    void SetAttackButtonsInteractable(bool interactable)
    {
        foreach (Button button in attackButtons)
        {
            button.interactable = interactable;
        }
    }

    void SetSpellButtonsInteractable(bool interactable)
    {
        foreach (Button button in spellButtons)
        {
            button.interactable = interactable;
        }
    }
}
