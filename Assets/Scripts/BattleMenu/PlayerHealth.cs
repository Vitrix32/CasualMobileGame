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
    [SerializeField]
    private GameObject combatVFX;
    [SerializeField]
    private Sprite[] VFXSprites;
    private AudioSource audioSource;

    public int maxHealth = 50;
    public int currentHealth;
    public int damage = 0;
    public TMP_Text healthText;
    public Slider healthSlider;

    public GameObject attackPanel;
    public GameObject spellPanel;
    public GameObject itemPanel;
    public GameObject mainMenuPanel;

    public float shakeDuration = 0.5f;
    public float shakeMagnitude = 0.9f;
    private Vector2 originalPosition;
    private RectTransform rectTransform;

    public ParticleSystem healParticleEffect;

    public GameObject UniversalAudio;
    public TextMeshProUGUI battleText;
    public ItemManager itemManager;
    public TMP_Text appleCountText;
    public TMP_Text milkCountText;
    public TMP_Text cookieCountText;
    public TMP_Text bananaCountText;

    public Enemy enemy;

    [System.Serializable]
    public class AttackButtonInfo
    {
        public string attackName;
        public Button button;
        [HideInInspector]
        public int cooldown = 0;
        public int maxCooldown = 2; // Default cooldown
    }

    public AttackButtonInfo[] attackButtonInfos;

    private bool playerTurn = true;

    private bool isEmpowered = false;
    private float damageMultiplier = 1.0f;

    private bool isShieldActive = false;
    private float shieldBlockChance = 0.8f;

    private bool skipEnemyTurn = false;

    // Reference to BattleMenu
    private BattleMenu battleMenu;

    public int swordDamage;
    public int axeDamage;
    public int clawDamage;
    public int bowDamage;

    void Start()
    {
        WorldPlayer = GameObject.Find("WorldPlayer");

        currentHealth = PlayerPrefs.GetInt("Health");

        UpdateHealthUI();
        SetAttackButtonsInteractable();

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        if (enemies.Length > 0)
        {
            enemy = enemies[0].GetComponent<Enemy>();
            //Debug.log("Enemy found: " + enemy.gameObject.name);
        }
        else
        {
            //Debug.logWarning("No enemies found in the scene.");
        }

        rectTransform = GetComponentInChildren<RectTransform>();

        if (rectTransform == null)
        {
            //Debug.logError("No RectTransform found on child Image!");
        }
        else
        {
            originalPosition = rectTransform.anchoredPosition;
        }

        if (healParticleEffect == null)
        {
            //Debug.logError("No particle system assigned for healing effects!");
        }

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            //Debug.logError("No AudioSource component found on the Player.");
        }

        UpdateText("What would you like to do?");

        ItemManager im = itemManager;

        swordDamage = im.attack.attackTypes[0].basic;
        axeDamage   = im.attack.attackTypes[1].basic;
        clawDamage  = im.attack.attackTypes[2].basic;
        bowDamage   = im.attack.attackTypes[3].basic;


        //Debug.log(im.attack.basic);
        //Debug.log("Player damage calculated: " + damage);

        foreach (var attackInfo in attackButtonInfos)
        {
            attackInfo.cooldown = 0;
            //Debug.log($"Initialized cooldown for attack '{attackInfo.attackName}' to 0.");
        }

        AssignButtonListeners();

        // Initialize BattleMenu reference
        battleMenu = GameObject.Find("BattleMenu").GetComponent<BattleMenu>();
       
        UpdateItemCountUI(); // Initial update
    }

    void AssignButtonListeners()
    {
        foreach (var attackInfo in attackButtonInfos)
        {
            if (attackInfo.button != null)
            {
                attackInfo.button.onClick.RemoveAllListeners();

                string currentAttack = attackInfo.attackName;

                // Debugging: Log assignment of listeners
                //Debug.log($"Assigning listener to attack '{currentAttack}'.");

                attackInfo.button.onClick.AddListener(() => PlayerAttack(currentAttack));
            }
            else
            {
                //Debug.logWarning($"Attack '{attackInfo.attackName}' has no button assigned.");
            }
        }
    }

    public void UpdateText(string newText)
    {
        if (battleText != null)
        {
            battleText.text = newText;
            //Debug.log($"Battle Text Updated: {newText}");
        }
        else
        {
            //Debug.logWarning("battleText is not assigned.");
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
                    //Debug.log("Player's shield successfully blocked the attack.");
                    isShieldActive = false;
                    return;
                }
                else
                {
                    UpdateText("Shield failed to block the attack.");
                    //Debug.log("Player's shield failed to block the attack.");
                    isShieldActive = false;
                }
            }

            currentHealth -= damageAmount;
            currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
            UpdateHealthUI();
            //Debug.log($"Player took {damageAmount} damage. Current Health: {currentHealth}");
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
            //Debug.logWarning("healthText is not assigned.");
        }

        if (healthSlider != null)
        {
            healthSlider.value = (float)currentHealth / maxHealth;
        }
        else
        {
            //Debug.logWarning("healthSlider is not assigned.");
        }

        PlayerPrefs.SetInt("Health", currentHealth);
    }

    void Die()
    {
        //Debug.log("You died!");
        Destroy(gameObject);
        if (WorldPlayer != null)
        {
            WorldPlayer.GetComponent<UniversalAudioHandling>().Die();
        }
        else
        {
            //Debug.logWarning("WorldPlayer is not assigned.");
        }
        SceneManager.LoadScene("DeathScene");
    }

    public void PlayerAttack(string attack)
    {
        //Debug.log($"PlayerAttack called with attack: {attack}");
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
                    PlayEffect(2);
                    PerformAttackWithDOT();
                    break;

                case "SkipEnemyTurn":
                    PerformSkipEnemyTurn();
                    break;

                case "DoubleDamage":
                    PlayEffect(1);
                    PerformDoubleDamage();
                    break;

                case "WeakenEnemy":
                    PlayEffect(3);
                    PerformWeakenEnemy();
                    break;

                case "HealSelf": // New attack
                    PerformHealSelf();
                    break;
                
                case "Apple":
                case "Milk":
                case "Cookie":
                case "Banana":
                    // Check if the item is available; if not, don't consume a turn or exit
                    if (!itemManager.HasConsumable(attack))
                    {
                        UpdateText("No " + attack + " left!");
                        return; 
                    }
                    else
                    {
                        PerformConsumeItem(attack);
                        UpdateItemCountUI(); // Update after consumption
                    }
                    break;

                default:
                    PlayEffect(4);
                    PerformRegularAttack(attack);
                    break;
            }

            playerTurn = false;

            // Disable menus since it's now the enemy's turn
            if (battleMenu != null)
            {
                battleMenu.SetMenusInteractable(false);
                //Debug.log("Battle menus have been disabled.");
            }

            foreach (var attackInfo in attackButtonInfos)
            {
                if (attackInfo.attackName == attack)
                {
                    attackInfo.cooldown = attackInfo.maxCooldown;
                    attackInfo.button.interactable = false;
                    //Debug.log($"Attack '{attackInfo.attackName}' is now on cooldown ({attackInfo.cooldown} turns).");
                    break;
                }
            }

            StartCoroutine(BattleSequence());

            attackPanel.SetActive(false);
            spellPanel.SetActive(false);
            itemPanel.SetActive(false);
            mainMenuPanel.SetActive(true);
        }
        else
        {
            //Debug.logWarning("PlayerAttack called, but it's not the player's turn or no enemy is present.");
        }
    }

    void PerformRegularAttack(string attackName)
    {
        int actualDamage = Mathf.RoundToInt(swordDamage/* * damageMultiplier*/);
        //Debug.log($"Calculated damage for attack '{attackName}': {actualDamage}");

        if (GameObject.Find("DebugMenu").GetComponent<DebugMenu>().inGodMode())
        {
            actualDamage = 10000;
            //Debug.log("God Mode is active. Damage set to 10000.");
        }

        enemy.TakeDamage(actualDamage);
        UpdateText("You used " + attackName + "!");
    }

    void PerformConsumeItem(string itemName)
    {
        int healAmount = 10;
        HealPlayer(healAmount);
        UpdateText("You used " + itemName + "!");
        itemManager.UseConsumable(itemName);
        //Debug.log($"Player used {itemName} and healed for {healAmount} health.");
    } 

    // Call this whenever items change
    private void UpdateItemCountUI()
    {
        if (appleCountText != null && itemManager.consumables.ContainsKey("Apple"))
        {
            appleCountText.text = "Apple (x" + itemManager.consumables["Apple"] + ")";
        }

        if (milkCountText != null && itemManager.consumables.ContainsKey("Milk"))
        {
            milkCountText.text = "Milk (x" + itemManager.consumables["Milk"] + ")";
        }

        if (cookieCountText != null && itemManager.consumables.ContainsKey("Cookie"))
        {
            cookieCountText.text = "Cookie (x" + itemManager.consumables["Cookie"] + ")";
        }

        if (bananaCountText != null && itemManager.consumables.ContainsKey("Banana"))
        {
            bananaCountText.text = "Banana (x" + itemManager.consumables["Banana"] + ")";
        }
    }      

    void PerformAttackWithDOT()
    {
        int duration = 3;
        enemy.ApplyDOT(clawDamage, duration);
        UpdateText("You used Attack With DOT!");
        //Debug.log($"Player applied DOT: {dotDamage} damage per turn for {duration} turns.");
    }

    void PerformEmpower()
    {
        damageMultiplier = 1.5f;
        isEmpowered = true;
        //Debug.log("Player damage increased by 50% for the next attack.");

        int healAmount = 10;
        HealPlayer(healAmount);
        //Debug.log($"Player healed for {healAmount} health.");

        UpdateText("You empowered yourself!");
    }

    void PerformShield()
    {
        isShieldActive = true;
        //Debug.log("Player has activated Shield. 80% chance to block the next enemy attack.");

        UpdateText("You raised a shield!");
    }

    void PerformSkipEnemyTurn()
    {
        skipEnemyTurn = true;
        UpdateText("You used Skip Enemy Turn!");
        //Debug.log("Player will skip the enemy's next turn.");
    }

    void PerformDoubleDamage()
    {
        int actualDamage = Mathf.RoundToInt(axeDamage/*damage * 2 * damageMultiplier*/);
        //Debug.log($"DoubleDamage attack used. Calculated damage: {actualDamage}");

        if (GameObject.Find("DebugMenu").GetComponent<DebugMenu>().inGodMode())
        {
            actualDamage = 10000;
            //Debug.log("God Mode is active. DoubleDamage set to 10000.");
        }

        enemy.TakeDamage(actualDamage);
        UpdateText("You used Double Damage!");
    }

    void PerformWeakenEnemy()
    {
        int actualDamage = Mathf.RoundToInt(bowDamage/*damage * 0.5f * damageMultiplier*/);
        //Debug.log($"WeakenEnemy attack used. Calculated damage: {actualDamage}");

        if (GameObject.Find("DebugMenu").GetComponent<DebugMenu>().inGodMode())
        {
            actualDamage = 10000;
            //Debug.log("God Mode is active. WeakenEnemy damage set to " + actualDamage + ".");
        }

        enemy.TakeDamage(actualDamage);
        enemy.SetWeakened(true);
        UpdateText("You used Weaken Enemy!");
        //Debug.log("Enemy's next attack will be weakened.");
    }

    // New method to perform Heal Self attack
    void PerformHealSelf()
    {
        int healAmount = 10;
        HealPlayer(healAmount);
        UpdateText("You used Heal Self!");
        //Debug.log($"Player healed for {healAmount} health.");
    }

    void PlayHealingEffect()
    {
        if (healParticleEffect != null)
        {
            healParticleEffect.Emit(100);
            //Debug.log("Healing effect played.");
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

        // Disable menus since it's now the enemy's turn
        if (battleMenu != null)
        {
            battleMenu.SetMenusInteractable(false);
            //Debug.log("Battle menus have been disabled.");
        }

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
            yield return new WaitForSeconds(1);

            playerTurn = true;

            // Re-enable menus since it's now the player's turn
            if (battleMenu != null)
            {
                battleMenu.SetMenusInteractable(true);
                //Debug.log("Battle menus have been enabled.");
            }

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

        if (skipEnemyTurn)
        {
            skipEnemyTurn = false;
            yield return StartCoroutine(EnemySkipTurn()); // Enemy skips their turn
        }
        else
        {
            yield return StartCoroutine(EnemyAttackTurn()); // Enemy takes their turn
        }
    }

    IEnumerator EnemyDOTTurn()
    {
        yield return new WaitForSeconds(2);

        UpdateText("The enemy was hurt!");
        enemy.ApplyTurnEffects();
        //Debug.log("Enemy took DOT damage.");
    }

    IEnumerator EnemySkipTurn()
    {
        yield return new WaitForSeconds(2);
        enemy.EnemySkip(this);
        playerTurn = true;

        // Re-enable menus since it's now the player's turn
        if (battleMenu != null)
        {
            battleMenu.SetMenusInteractable(true);
            //Debug.log("Battle menus have been enabled.");
        }

        UpdateAttackCooldowns();
        SetAttackButtonsInteractable();
        //Debug.log("Enemy skipped their turn.");
    }

    /*void SetAttackButtonsInteractable()
    {
        foreach (var attackInfo in attackButtonInfos)
        {
            bool isInteractable = attackInfo.cooldown == 0 && playerTurn;
            attackInfo.button.interactable = isInteractable;
            //Debug.log($"Attack '{attackInfo.attackName}' interactable: {isInteractable}");
        }
    }*/

    void SetAttackButtonsInteractable()
{
    foreach (var attackInfo in attackButtonInfos)
    {
        bool isInteractable = (attackInfo.cooldown == 0) && playerTurn;
        if ((attackInfo.attackName == "Apple" || attackInfo.attackName == "Milk" 
            || attackInfo.attackName == "Cookie" || attackInfo.attackName == "Banana")
            && !itemManager.HasConsumable(attackInfo.attackName))
        {
            isInteractable = false;
        }
        attackInfo.button.interactable = isInteractable;
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
                    //Debug.log($"Attack '{attackInfo.attackName}' is now off cooldown and available.");
                }
                else
                {
                    //Debug.log($"Attack '{attackInfo.attackName}' cooldown decreased to {attackInfo.cooldown}.");
                }
            }
        }
    }

    private void PlayEffect(int index)
    {
        combatVFX.GetComponent<SpriteRenderer>().sprite = VFXSprites[index];
        Invoke("ResetEffect", 0.8f);
    }

    private void ResetEffect()
    {
        combatVFX.GetComponent<SpriteRenderer>().sprite = VFXSprites[0];
    }
}
