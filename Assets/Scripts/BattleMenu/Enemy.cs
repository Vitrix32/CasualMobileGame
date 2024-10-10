using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEditor.UI;

public class Enemy : MonoBehaviour
{
    private GameObject MenuManager;
    public int maxHealth = 100; // Maximum health for the enemy
    public int currentHealth;

    public Slider healthBar; // Reference to the health bar UI

    public int attackDamage = 20; // Damage enemy deals to the player

    // DOT-related variables
    private int dotDamage = 0;
    private int dotTurnsRemaining = 0;

    // Variables for the shake effect
    public float shakeDuration = 0.5f;  // How long the shake lasts
    public float shakeMagnitude = 0.9f; // How much the object shakes (for UI units)
    private Vector2 originalPosition;   // To store the original position of the UI element

    private RectTransform rectTransform; // Reference to the RectTransform for UI element

    void Start()
    {
        MenuManager = GameObject.Find("MenuManager");
        currentHealth = maxHealth;
        UpdateHealthBar();

        // Get the RectTransform from the child object
        rectTransform = GetComponentInChildren<RectTransform>();

        // Check if rectTransform is found
        if (rectTransform == null)
        {
            Debug.LogError("No RectTransform found on child Image!");
        }
        else
        {
            originalPosition = rectTransform.anchoredPosition;
        }
    }

    public int GetDOTTurn() {
        return dotTurnsRemaining;
    }


    // Method for enemy to take damage
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // Ensure health doesn't go below 0
        UpdateHealthBar(); // Update the health bar

        Debug.Log("Enemy took " + damage + " damage! Health remaining: " + currentHealth);

        StartCoroutine(Shake()); // Start the shake effect when taking damage

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

    // Coroutine to handle the shake effect
    private IEnumerator Shake()
    {
        float elapsed = 0.0f;

        while (elapsed < shakeDuration)
        {
            // Generate a random offset for the shake effect
            float offsetX = Random.Range(-1f, 1f) * shakeMagnitude * 0.5f;
            float offsetY = Random.Range(-1f, 1f) * shakeMagnitude * 0.5f;

            // Apply the offset to the RectTransform's anchored position
            rectTransform.anchoredPosition = new Vector2(originalPosition.x + offsetX, originalPosition.y + offsetY);

            elapsed += Time.deltaTime;

            yield return null;
        }

        // Reset the UI element's position after the shake effect is over
        rectTransform.anchoredPosition = originalPosition;
    }

    // Method to handle enemy's attack
    public void EnemyAttack(Player player)
    {
        Debug.Log("Enemy attacks!");
        player.TakeDamage(attackDamage); // Enemy attacks the player
    }

    public void EnemySkip(Player player)
    {
        Debug.Log("Enemy skipped!");
    }

    // Method to handle enemy death
    void Die()
    {
        Debug.Log("Enemy died!");
        MenuManager.GetComponent<BattleMenu>().EnemyNeutralized();
        Destroy(gameObject); // Destroy enemy when health is 0
    }

    // Call this method when the DOT attack hits the enemy
    public void ApplyDOT(int damagePerTurn, int numberOfTurns)
    {
        dotDamage = damagePerTurn;
        dotTurnsRemaining = numberOfTurns;

    }

    public void ApplyTurnEffects()
    {
        if (dotTurnsRemaining > 0)
        {
            currentHealth -= dotDamage;
            currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // Ensure health doesn't go below 0
            dotTurnsRemaining--;
            UpdateHealthBar();
            StartCoroutine(Shake()); // Start the shake effect when taking damage
            Debug.Log("Enemy took " + dotDamage + " damage! Health remaining: " + currentHealth);

            if (currentHealth <= 0)
            {
                Die();
            }
        }
    }  
}