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

    [System.Serializable]
    public class AttackButtonInfo
    {
        public string attackName;
        public Button button;
        [HideInInspector]
        public int cooldown = 0;
    }

    public AttackButtonInfo[] attackButtonInfos;

    private bool playerTurn = true;

    private bool isEmpowered = false;
    private float damageMultiplier = 1.0f;

    private bool isShieldActive = false;
    private float shieldBlockChance = 0.8f;
    void Start()
    {
        WorldPlayer = GameObject.Find("WorldPlayer");

        currentHealth = maxHealth;
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

        ItemManager im = itemManager;

        damage = im.attack.basic +
                 im.attack.itemEnhancement +
                 im.attack.boostEnhancement;

        Debug.Log(im.attack.basic);
        Debug.Log("Player damage calculated: " + damage);

        foreach (var attackInfo in attackButtonInfos)
        {
            attackInfo.cooldown = 0;
            Debug.Log($"Initialized cooldown for attack '{attackInfo.attackName}' to 0.");
        }

        AssignButtonListeners();
    }

    void AssignButtonListeners()
    {
        foreach (var attackInfo in attackButtonInfos)
        {
            if (attackInfo.button != null)
            {
                attackInfo.button.onClick.RemoveAllListeners();

                string currentAttack = attackInfo.attackName;

                attackInfo.button.onClick.AddListener(() => PlayerAttack(currentAttack));
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

    public void TakeDamage(int damageAmount)
    {
        if (!GameObject.Find("DebugMenu").GetComponent<DebugMenu>().inGodMode())
        {
            if (isShieldActive)
            {
                float roll = Random.Range(0f, 1f);
                if (roll <= shieldBlockChance)
                {
                    UpdateText("Shield blocked the attack!");
                    Debug.Log("Player's shield successfully blocked the attack.");
                    isShieldActive = false;
                    return;
                }
                else
                {
                    UpdateText("Shield failed to block the attack.");
                    Debug.Log("Player's shield failed to block the attack.");
                    isShieldActive = false;
                }
            }

            currentHealth -= damageAmount;
            currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
            UpdateHealthUI();
            Debug.Log($"Player took {damageAmount} damage. Current Health: {currentHealth}");
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
        }
        else
        {
            Debug.LogWarning("healthText is not assigned.");
        }

        if (healthSlider != null)
        {
            healthSlider.value = (float)currentHealth / maxHealth;
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

    public void PlayerAttack(string attack)
    {
        Debug.Log($"PlayerAttack called with attack: {attack}");

        if (playerTurn && enemy != null)
        {
            switch (attack)
            {
                case "Empower":
                    PerformEmpower();
                    break;

                case "Shield":
                    PerformShield();
                    break;

                case "AttackWithDOT":
                    PerformAttackWithDOT();
                    break;

                default:
                    PerformRegularAttack(attack);
                    break;
            }

            playerTurn = false;

            foreach (var attackInfo in attackButtonInfos)
            {
                if (attackInfo.attackName == attack)
                {
                    attackInfo.cooldown = 2;
                    attackInfo.button.interactable = false;
                    Debug.Log($"Attack '{attackInfo.attackName}' is now on cooldown (cooldown set to {attackInfo.cooldown}).");
                    break;
                }
            }

            StartCoroutine(BattleSequence());

            attackPanel.SetActive(false);
            spellPanel.SetActive(false);
            mainMenuPanel.SetActive(true);
        }
        else
        {
            Debug.LogWarning("PlayerAttack called, but it's not the player's turn or no enemy is present.");
        }
    }

    void PerformRegularAttack(string attackName)
    {
        int actualDamage = Mathf.RoundToInt(damage * damageMultiplier);
        Debug.Log($"Calculated damage for attack '{attackName}': {actualDamage}");

        if (GameObject.Find("DebugMenu").GetComponent<DebugMenu>().inGodMode())
        {
            actualDamage = 10000;
            Debug.Log("God Mode is active. Damage set to 10000.");
        }

        enemy.TakeDamage(actualDamage);
        UpdateText("You used " + attackName + "!");
    }

    void PerformAttackWithDOT()
    {
        int dotDamage = 5;
        int duration = 3;
        enemy.ApplyDOT(dotDamage, duration);
        UpdateText("You used Attack With DOT!");
        Debug.Log($"Player applied DOT: {dotDamage} damage per turn for {duration} turns.");
    }

    void PerformEmpower()
    {
        damageMultiplier = 1.5f;
        isEmpowered = true;
        Debug.Log("Player damage increased by 50% for the next attack.");

        int healAmount = 10;
        HealPlayer(healAmount);
        Debug.Log($"Player healed for {healAmount} health.");

        UpdateText("You empowered yourself!");
    }

    void PerformShield()
    {
        isShieldActive = true;
        Debug.Log("Player has activated Shield. 80% chance to block the next enemy attack.");

        UpdateText("You raised a shield!");
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

        StartCoroutine(BattleSequence());

        spellPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }

    private void HealPlayer(int health)
    {
        currentHealth += health;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHealthUI();
        PlayHealingEffect();
    }

    IEnumerator EnemyAttackTurn()
    {
        if (enemy.currentHealth > 0)
        {
            yield return new WaitForSeconds(2);

            enemy.EnemyAttack(this);
            // Removed redundant UpdateText call
            yield return new WaitForSeconds(1);

            playerTurn = true;
            UpdateAttackCooldowns();
            SetAttackButtonsInteractable();
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
        UpdateAttackCooldowns();
        SetAttackButtonsInteractable();
        Debug.Log("Enemy skipped their turn.");
    }

    void SetAttackButtonsInteractable()
    {
        foreach (var attackInfo in attackButtonInfos)
        {
            bool isInteractable = attackInfo.cooldown == 0 && playerTurn;
            attackInfo.button.interactable = isInteractable;
            Debug.Log($"Attack '{attackInfo.attackName}' interactable: {isInteractable}");
        }
    }

    void UpdateAttackCooldowns()
    {
        foreach (var attackInfo in attackButtonInfos)
        {
            if (attackInfo.cooldown > 0)
            {
                attackInfo.cooldown--;
                if (attackInfo.cooldown == 0)
                {
                    attackInfo.button.interactable = playerTurn;
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
