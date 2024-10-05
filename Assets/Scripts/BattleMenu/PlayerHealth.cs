using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;

    public int heal = 0;

    public int damage = 0;

    public Text healthText;
    public Slider healthSlider;

    public Button[] attackButtons;
    public Button[] spellButtons;


    private bool playerTurn = true;

    public Enemy enemy; // Reference to the enemy, which we can assign in the Inspector

    public GameObject attackPanel;

    public GameObject spellPanel;
    public GameObject mainMenuPanel;

    // Variables for the shake effect
    public float shakeDuration = 0.5f;  // How long the shake lasts
    public float shakeMagnitude = 0.9f; // How much the object shakes (for UI units)
    private Vector2 originalPosition;   // To store the original position of the UI element

    private RectTransform rectTransform; // Reference to the RectTransform for UI element

    public GameObject UniversalAudio;

    void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthUI();
        SetAttackButtonsInteractable(playerTurn);

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        enemy = enemies[0].GetComponent<Enemy>();

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

    // Method for taking damage from the enemy
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHealthUI();

        StartCoroutine(Shake()); // Start the shake effect when taking damage


        if (currentHealth <= 0)
        {
            Die();
            mainMenuPanel.SetActive(false);      // Show attack panel

        }
    }

    private IEnumerator Shake()
    {
        float elapsed = 0.0f;

        while (elapsed < shakeDuration)
        {
            // Generate a random offset for the shake effect
            float offsetX = Random.Range(-1f, 1f) * shakeMagnitude * 10;
            float offsetY = Random.Range(-1f, 1f) * shakeMagnitude * 10;

            // Apply the offset to the RectTransform's anchored position
            rectTransform.anchoredPosition = new Vector2(originalPosition.x + offsetX, originalPosition.y + offsetY);

            elapsed += Time.deltaTime;

            yield return null;
        }

        // Reset the UI element's position after the shake effect is over
        rectTransform.anchoredPosition = originalPosition;
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
        UniversalAudio = GameObject.Find("UniversalAudio");
        UniversalAudio.GetComponent<UniversalAudioHandling>().Die();
        SceneManager.LoadScene("DeathScene");
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

    public void Heal(int health)
    {
        currentHealth += health;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        SetSpellButtonsInteractable(false);
        UpdateHealthUI();

        StartCoroutine(EnemyAttackTurn());

        spellPanel.SetActive(false);      // Show attack panel
        mainMenuPanel.SetActive(true);      // Show attack panel
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

    public void AttackWithDOT()
    {
        int dotDamage = 5; // The amount of damage to apply per turn
        int duration = 3;  // Number of turns DOT will last

        /*if (enemy != null)
        {
            enemy.ApplyDOT(dotDamage, duration);
        }*/
        if (playerTurn && enemy != null)
        {
            playerTurn = false;
            SetSpellButtonsInteractable(false);
            enemy.ApplyDOT(dotDamage, duration);

            StartCoroutine(EnemyAttackTurn());

            spellPanel.SetActive(false);      // Show attack panel
            mainMenuPanel.SetActive(true);      // Show attack panel

        }
    }


    IEnumerator EnemyAttackTurn()
    {
        yield return new WaitForSeconds(1);

        enemy.ApplyTurnEffects();  // Apply any DOT effects before the enemy attacks
        enemy.EnemyAttack(this);   // Enemy attacks player

        // Allow the player to attack again after the enemy turn ends
        playerTurn = true;
        SetAttackButtonsInteractable(true);
        SetSpellButtonsInteractable(true);
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

     /*public void ApplyDOT(int damagePerTurn, int duration)
    {
        dotDamage = damagePerTurn;
        dotTurnsRemaining = duration;
        Debug.Log("Enemy inflicted with DOT: " + damagePerTurn + " damage for " + duration + " turns.");
    }*/
}