using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class Enemy : MonoBehaviour
{
    private GameObject MenuManager;
    public int maxHealth = 100;
    public int currentHealth;
    public string name;

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
    public float shieldReduction = 0.5f;

    private bool isWeakened = false;
    private float weakenedDamageMultiplier = 0.5f;

    void Start()
    {
        MenuManager = GameObject.Find("BattleMenu");
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
            isShieldActive = false;
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
            isWeakened = false;
            player.UpdateText("The " + name + "'s attack is weakened! Damage reduced to " + damageToDeal);
        }

        if (actionRoll <= 0.15f)
        {
            // Critical hit
            int criticalDamage = Mathf.RoundToInt(damageToDeal * 1.5f);
            player.UpdateText("The" + name + " performed a critical hit!");
            audioSource.PlayOneShot(attackSound);
            player.TakeDamage(criticalDamage);
        }
        else if (actionRoll <= 0.35f)
        {
            // Raise shield
            isShieldActive = true;
            player.UpdateText("The " + name + " raised a shield!");
        }
        else
        {
            // Normal attack
            player.UpdateText("The " + name + " attacked!");
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
            if (gameObject.activeSelf)
            {
                StartCoroutine(Shake());
            }
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
