using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public int maxHealth = 100; // Maximum health for the enemy
    public int currentHealth;

    public Slider healthBar; // Reference to the health bar UI

    public int attackDamage = 20; // Damage enemy deals to the player

    void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthBar(); // Initialize the health bar to show full health
    }

    // Method for enemy to take damage
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // Ensure health doesn't go below 0
        UpdateHealthBar(); // Update the health bar

        Debug.Log("Enemy took " + damage + " damage! Health remaining: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // Update the health bar based on the current health
    void UpdateHealthBar()
    {
        if (healthBar != null)
        {
            healthBar.value = (float)currentHealth / maxHealth;
        }
    }

    // Method to handle enemy's attack
    public void EnemyAttack(Player player)
    {
        Debug.Log("Enemy attacks!");
        player.TakeDamage(attackDamage); // Enemy attacks the player
    }

    // Handle enemy death
    void Die()
    {
        Debug.Log("Enemy died!");
        Destroy(gameObject); // Destroy enemy when health is 0
    }
}
