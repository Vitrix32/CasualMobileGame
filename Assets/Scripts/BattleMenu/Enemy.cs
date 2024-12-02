using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class Enemy : MonoBehaviour
{
    private GameObject MenuManager;
    public int maxHealth = 100;
    public int currentHealth;

    public TMP_Text healthText;
    public Slider healthBar;
    public int attackDamage = 20;
    private int dotDamage = 0;
    private int dotTurnsRemaining = 0;

    public float shakeDuration = 0.5f;
    public float shakeMagnitude = 0.9f;
    private Vector2 originalPosition;
    private RectTransform rectTransform;
    private AudioSource audioSource;
    public AudioClip attackSound;

    public bool isShieldActive = false;
    public float shieldReduction = 0.5f; // Reduces damage by 50%

    // New flag to indicate if the enemy is weakened
    private bool isWeakened = false;
    private float weakenedDamageMultiplier = 0.5f; // Reduces damage by 50%

    void Start()
    {
        MenuManager = GameObject.Find("MenuManager");
        currentHealth = maxHealth;
        UpdateHealthUI();
        audioSource = GetComponent<AudioSource>();

        rectTransform = GetComponentInChildren<RectTransform>();

        if (rectTransform == null)
        {
            Debug.LogError("No RectTransform found on child Image!");
        }
        else
        {
            originalPosition = rectTransform.anchoredPosition;
        }
    }

    public int GetDOTTurn()
    {
        return dotTurnsRemaining;
    }

    public void TakeDamage(int damage)
    {
        if (isShieldActive)
        {
            damage = Mathf.RoundToInt(damage * shieldReduction);
            isShieldActive = false; // Shield only lasts for one attack
            Debug.Log("Enemy's shield reduced damage to " + damage);
        }

        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHealthUI();
        Debug.Log("Enemy took " + damage + " damage! Health remaining: " + currentHealth);

        StartCoroutine(Shake());
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void UpdateHealthUI()
    {
        if (healthText != null)
        {
            healthText.text = "HP: " + currentHealth;
        }
        if (healthBar != null)
        {
            healthBar.value = (float)currentHealth / maxHealth;
        }
    }

    private IEnumerator Shake()
    {
        float elapsed = 0.0f;

        while (elapsed < shakeDuration)
        {
            float offsetX = Random.Range(-1f, 1f) * shakeMagnitude * 0.5f;
            float offsetY = Random.Range(-1f, 1f) * shakeMagnitude * 0.5f;

            rectTransform.anchoredPosition = new Vector2(originalPosition.x + offsetX, originalPosition.y + offsetY);

            elapsed += Time.deltaTime;

            yield return null;
        }

        rectTransform.anchoredPosition = originalPosition;
    }

    public void EnemyAttack(Player player)
    {
        float actionRoll = Random.Range(0f, 1f);
        int damageToDeal = attackDamage;

        if (isWeakened)
        {
            damageToDeal = Mathf.RoundToInt(damageToDeal * weakenedDamageMultiplier);
            isWeakened = false; // Reset weakened status after one attack
            Debug.Log("Enemy's attack is weakened! Damage reduced to " + damageToDeal);
            player.UpdateText("Enemy's attack was weakened!");
        }

        if (actionRoll <= 0.15f)
        {
            // Critical hit
            int criticalDamage = Mathf.RoundToInt(damageToDeal * 1.5f); // 50% more damage
            Debug.Log("Enemy performs a critical hit!");
            player.UpdateText("Enemy performs a critical hit!");
            audioSource.PlayOneShot(attackSound);
            player.TakeDamage(criticalDamage);
        }
        else if (actionRoll <= 0.35f)
        {
            // Raise shield
            isShieldActive = true;
            Debug.Log("Enemy raises a shield!");
            player.UpdateText("Enemy raises a shield!");
            // Optional: Add visual effect here
        }
        else
        {
            // Normal attack
            Debug.Log("Enemy attacks!");
            player.UpdateText("Enemy attacks!");
            audioSource.PlayOneShot(attackSound);
            player.TakeDamage(damageToDeal);
        }
    }

    public void EnemySkip(Player player)
    {
        Debug.Log("Enemy skipped their turn!");
        player.UpdateText("Enemy skipped their turn!");
    }

    void Die()
    {
        Debug.Log("Enemy died!");
        MenuManager.GetComponent<BattleMenu>().EnemyNeutralized();
        gameObject.SetActive(false);
    }

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
            currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
            dotTurnsRemaining--;
            UpdateHealthUI();
            StartCoroutine(Shake());
            Debug.Log("Enemy took " + dotDamage + " damage! Health remaining: " + currentHealth);

            if (currentHealth <= 0)
            {
                Die();
            }
        }
    }

    // Method to set the weakened status
    public void SetWeakened(bool status)
    {
        isWeakened = status;
        if (isWeakened)
        {
            Debug.Log("Enemy has been weakened. Their next attack will deal reduced damage.");
        }
    }
}
