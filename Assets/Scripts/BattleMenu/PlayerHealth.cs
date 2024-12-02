using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using TMPro;
using System;
using Random = UnityEngine.Random;

public class Player : MonoBehaviour
{
    [SerializeField]
    private GameObject WorldPlayer;
    private AudioSource audioSource;
    public int maxHealth = 100;
    public int currentHealth;
    public int heal = 0;
    public int damage = 0;
    public TMP_Text healthText;
    public Slider healthSlider;
    public GameObject attackPanel;
    public GameObject spellPanel;
    public GameObject mainMenuPanel;
    public float shakeDuration = 0.5f;
    public float shakeMagnitude = 0.9f;
    private Vector2 originalPosition;
    private RectTransform rectTransform;
    public ParticleSystem healParticleEffect;
    public GameObject UniversalAudio;
    public TextMeshProUGUI battleText;
    public ItemManager itemManager;

    public Enemy enemy;

    // New class to hold attack information
    [System.Serializable]
    public class AttackButtonInfo
    {
        public string attackName;
        public Button button;
        [HideInInspector]
        public int cooldown = 0; // Cooldown counter
    }

    // Updated array to hold attack information
    public AttackButtonInfo[] attackButtonInfos;

    private bool playerTurn = true;

    void Start()
    {
        WorldPlayer = GameObject.Find("WorldPlayer");

        // Set currentHealth before updating UI
        currentHealth = 100;
        Debug.Log("Player currentHealth set to: " + currentHealth);

        UpdateHealthUI();
        SetAttackButtonsInteractable();

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        if (enemies.Length > 0)
        {
            enemy = enemies[0].GetComponent<Enemy>();
            Debug.Log("Enemy found: " + enemy.gameObject.name);
        }
        else
        {
            Debug.LogWarning("No enemies found in the scene.");
        }

        rectTransform = GetComponentInChildren<RectTransform>();

        if (rectTransform == null)
        {
            Debug.LogError("No RectTransform found on child Image!");
        }
        else
        {
            originalPosition = rectTransform.anchoredPosition;
        }

        if (healParticleEffect == null)
        {
            Debug.LogError("No particle system assigned for healing effects!");
        }

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogError("No AudioSource component found on the Player.");
        }

        UpdateText("What would you like to do?");

        // PlayerPrefs.SetFloat("swordMultiplier", 2f);

        ItemManager im = itemManager;

        damage = im.attack.basic +
                 im.attack.itemEnhancement +
                 im.attack.boostEnhancement;

        Debug.Log(im.attack.basic);
        
        Debug.Log("Player damage calculated: " + damage);

        // Initialize cooldowns
        foreach (var attackInfo in attackButtonInfos)
        {
            attackInfo.cooldown = 0;
            Debug.Log($"Initialized cooldown for attack '{attackInfo.attackName}' to 0.");
        }

        // Assign button listeners automatically
        AssignButtonListeners();
    }

    void AssignButtonListeners()
    {
        foreach (var attackInfo in attackButtonInfos)
        {
            if (attackInfo.button != null)
            {
                // Remove existing listeners to prevent duplicate calls
                attackInfo.button.onClick.RemoveAllListeners();

                // Capture the current attackName in a local variable to avoid closure issues
                string currentAttack = attackInfo.attackName;

                // Add listener to call PlayerAttack with the attackName
                attackInfo.button.onClick.AddListener(() => PlayerAttack(currentAttack));

                // Debug.Log($"Assigned PlayerAttack(\"{currentAttack}\") to button '{attackInfo.button.name}'.");
            }
            else
            {
                Debug.LogWarning($"Attack '{attackInfo.attackName}' has no button assigned.");
            }
        }
    }

    public void UpdateText(string newText)
    {
        if (battleText != null)
        {
            battleText.text = newText;
            Debug.Log($"Battle Text Updated: {newText}");
        }
        else
        {
            Debug.LogWarning("battleText is not assigned.");
        }
    }

    public void TakeDamage(int damage)
    {
        if (!GameObject.Find("DebugMenu").GetComponent<DebugMenu>().inGodMode())
        {
            currentHealth -= damage;
            currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
            UpdateHealthUI();
            Debug.Log($"Player took {damage} damage. Current Health: {currentHealth}");
        }
        StartCoroutine(Shake());
        if (currentHealth <= 0)
        {
            Die();
            mainMenuPanel.SetActive(false);
        }
    }

    private IEnumerator Shake()
    {
        float elapsed = 0.0f;

        while (elapsed < shakeDuration)
        {
            float offsetX = Random.Range(-1f, 1f) * shakeMagnitude * 10;
            float offsetY = Random.Range(-1f, 1f) * shakeMagnitude * 10;

            rectTransform.anchoredPosition = new Vector2(originalPosition.x + offsetX, originalPosition.y + offsetY);

            elapsed += Time.deltaTime;

            yield return null;
        }

        rectTransform.anchoredPosition = originalPosition;
    }

    void UpdateHealthUI()
    {
        if (healthText != null)
        {
            healthText.text = "HP: " + currentHealth;
            // Debug.Log($"Health UI Updated: HP = {currentHealth}");
        }
        else
        {
            Debug.LogWarning("healthText is not assigned.");
        }

        if (healthSlider != null)
        {
            healthSlider.value = (float)currentHealth / maxHealth;
            // Debug.Log($"Health Slider Updated: Value = {healthSlider.value}");
        }
        else
        {
            Debug.LogWarning("healthSlider is not assigned.");
        }
    }

    void Die()
    {
        Debug.Log("You died!");
        Destroy(gameObject);
        if (WorldPlayer != null)
        {
            WorldPlayer.GetComponent<UniversalAudioHandling>().Die();
        }
        else
        {
            Debug.LogWarning("WorldPlayer is not assigned.");
        }
        SceneManager.LoadScene("DeathScene");
    }

    // Modified PlayerAttack to handle cooldowns
    public void PlayerAttack(string attack)
    {
        Debug.Log($"PlayerAttack called with attack: {attack}");

        if (playerTurn && enemy != null)
        {
            //float baseDamage = PlayerPrefs.GetFloat(attack + "Damage", 10.0f);
            //float multiplier = PlayerPrefs.GetFloat(attack + "Multiplier", 1.0f);
            //int damage = (int)(baseDamage * multiplier);

            Debug.Log($"Calculated damage for attack '{attack}': {damage}");

            if (GameObject.Find("DebugMenu").GetComponent<DebugMenu>().inGodMode())
            {
                damage = 10000;
                Debug.Log("God Mode is active. Damage set to 10000.");
            }

            enemy.TakeDamage(damage);
            UpdateText("You used " + attack + "!");
            playerTurn = false;

            // **Set the used attack on cooldown**
            foreach (var attackInfo in attackButtonInfos)
            {
                if (attackInfo.attackName == attack)
                {
                    attackInfo.cooldown = 2; // **Set cooldown to 2 turns instead of 1**
                    attackInfo.button.interactable = false; // Disable the button
                    Debug.Log($"Attack '{attack}' is now on cooldown (cooldown set to {attackInfo.cooldown}).");
                    break;
                }
            }

            StartCoroutine(BattleSequence());

            attackPanel.SetActive(false);
            mainMenuPanel.SetActive(true);
        }
        else
        {
            Debug.LogWarning("PlayerAttack called, but it's not the player's turn or no enemy is present.");
        }
    }

    void PlayHealingEffect()
    {
        if (healParticleEffect != null)
        {
            healParticleEffect.Emit(100);
            Debug.Log("Healing effect played.");
        }
    }

    public void Heal(int health)
    {
        currentHealth += health;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHealthUI();
        UpdateText("You healed!");
        PlayHealingEffect();

        playerTurn = false;
        SetSpellButtonsInteractable(false);

        StartCoroutine(BattleSequence());

        spellPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }

    public void PlayerSkipSpell()
    {
        if (playerTurn && enemy != null)
        {
            playerTurn = false;
            SetSpellButtonsInteractable(false);

            UpdateText("You skipped the enemy's turn!");
            Debug.Log("Player skipped their turn.");

            StartCoroutine(Skip());

            spellPanel.SetActive(false);
            mainMenuPanel.SetActive(true);
        }
    }

    public void AttackWithDOT()
    {
        int dotDamage = 5;
        int duration = 3;
        if (playerTurn && enemy != null)
        {
            playerTurn = false;
            UpdateText("You dealt damage over time!");
            SetSpellButtonsInteractable(false);
            enemy.ApplyDOT(dotDamage, duration);
            Debug.Log($"Player applied DOT: {dotDamage} damage per turn for {duration} turns.");

            StartCoroutine(BattleSequence());

            spellPanel.SetActive(false);
            mainMenuPanel.SetActive(true);
        }
    }

    IEnumerator EnemyAttackTurn()
    {
        if (enemy.currentHealth > 0)
        {
            yield return new WaitForSeconds(2);

            enemy.EnemyAttack(this);
            UpdateText("The enemy attacked!");
            Debug.Log("Enemy performed an attack.");

            yield return new WaitForSeconds(1);

            playerTurn = true;
            UpdateAttackCooldowns(); // Update cooldowns at the start of player's turn
            SetAttackButtonsInteractable();
            SetSpellButtonsInteractable(true);
        }
        else
        {
            yield return new WaitForSeconds(1);
        }
    }

    IEnumerator BattleSequence()
    {
        if (enemy.GetDOTTurn() > 0)
        {
            yield return StartCoroutine(EnemyDOTTurn());
        }

        yield return StartCoroutine(EnemyAttackTurn());
    }

    IEnumerator Skip()
    {
        if (enemy.GetDOTTurn() > 0)
        {
            yield return StartCoroutine(EnemyDOTTurn());
        }

        yield return StartCoroutine(EnemySkipTurn());
    }

    IEnumerator EnemyDOTTurn()
    {
        yield return new WaitForSeconds(2);

        UpdateText("The enemy was hurt!");
        enemy.ApplyTurnEffects();
        Debug.Log("Enemy took DOT damage.");
    }

    IEnumerator EnemySkipTurn()
    {
        yield return new WaitForSeconds(2);
        enemy.EnemySkip(this);
        playerTurn = true;
        UpdateAttackCooldowns(); // Update cooldowns at the start of player's turn
        SetSpellButtonsInteractable(true);
        Debug.Log("Enemy skipped their turn.");
    }

    // Updated method to set attack buttons interactability based on cooldowns
    void SetAttackButtonsInteractable()
    {
        foreach (var attackInfo in attackButtonInfos)
        {
            bool isInteractable = attackInfo.cooldown == 0;
            attackInfo.button.interactable = isInteractable;
            // Debug.Log($"Attack '{attackInfo.attackName}' interactable: {isInteractable}");
        }
    }

    void SetSpellButtonsInteractable(bool interactable)
    {
        // Assuming you have a similar structure for spells
        // Update this method if necessary
    }

    // New method to update attack cooldowns
    void UpdateAttackCooldowns()
    {
        foreach (var attackInfo in attackButtonInfos)
        {
            if (attackInfo.cooldown > 0)
            {
                attackInfo.cooldown--;
                if (attackInfo.cooldown == 0)
                {
                    attackInfo.button.interactable = true; // Re-enable the button
                    Debug.Log($"Attack '{attackInfo.attackName}' is now off cooldown and available.");
                }
                else
                {
                    Debug.Log($"Attack '{attackInfo.attackName}' cooldown decreased to {attackInfo.cooldown}.");
                }
            }
        }
    }
}
