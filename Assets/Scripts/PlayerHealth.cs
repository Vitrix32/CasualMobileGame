using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;

    public Text healthText; // UI Text to display health
    public Slider healthSlider; // Optional health slider UI

    private bool playerTurn = true; // Track whose turn it is

    void Start()
    {
        currentHealth = maxHealth; // Set initial health
        UpdateHealthUI(); // Update UI at the start
    }

    // Method to take damage
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // Ensure health doesn't go below 0
        UpdateHealthUI();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // Update health display on the UI
    void UpdateHealthUI()
    {
        if (healthText != null)
        {
            healthText.text = "Health: " + currentHealth;
        }

        if (healthSlider != null)
        {
            healthSlider.value = (float)currentHealth / maxHealth; // Update health bar
        }
    }

    // Handle player death
    void Die()
    {
        Debug.Log("Player died!");
        // Implement death logic, such as restarting the game or showing a Game Over screen
    }

    // Player's attack turn, only allowed if it's their turn
    public void PlayerAttack(Enemy enemy, int damage)
    {
        if (playerTurn)
        {
            enemy.TakeDamage(damage);
            playerTurn = false; // End player's turn
            StartCoroutine(EnemyAttackTurn(enemy)); // Start enemy turn after player attacks
        }
    }

    // Coroutine to manage the enemy's attack after player's turn
    IEnumerator EnemyAttackTurn(Enemy enemy)
    {
        yield return new WaitForSeconds(1); // Delay for enemy attack, simulating the turn system
        enemy.EnemyAttack(this);
        playerTurn = true; // Player can attack after enemy
    }
}
