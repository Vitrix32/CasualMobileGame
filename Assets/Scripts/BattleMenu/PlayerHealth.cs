using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using TMPro;
using System;
using Random = UnityEngine.Random;
using JetBrains.Annotations;
//using UnityEditor.Animations;

public class PlayerHealth : MonoBehaviour
{
    #region Inspector Fields

    [Header("References")]
    [SerializeField] private GameObject WorldPlayer;
    [SerializeField] private GameObject combatVFX;
    [SerializeField] private GameObject spellVFX;
    [SerializeField] private GameObject combatStats;
    [SerializeField] private GameObject enemyCombatStats;
    [SerializeField] private Sprite[] VFXSprites;
    [SerializeField] private ParticleSystem healParticleEffect;
    [SerializeField] private GameObject UniversalAudio;
    [SerializeField] private ItemManager itemManager;
    [SerializeField] private Enemy enemy;
    [SerializeField] private TMP_Text[] cooldowns;

    [Header("UI Elements")]
    [SerializeField] private TMP_Text healthText;
    [SerializeField] private Slider healthSlider;
    [SerializeField] private GameObject enemyHealthSlider;
    [SerializeField] private TMP_Text appleCountText;
    [SerializeField] private TMP_Text milkCountText;
    [SerializeField] private TMP_Text cookieCountText;
    [SerializeField] private TMP_Text bananaCountText;
    [SerializeField] private TextMeshProUGUI battleText;
    [SerializeField] private GameObject attackPanel;
    [SerializeField] private GameObject spellPanel;
    [SerializeField] private GameObject helpPanel;
    [SerializeField] private GameObject mainMenuPanel;

    [Header("Menu & Attack Data")]
    public AttackButtonInfo[] attackButtonInfos;

    [Header("Player Stats")]
    public int maxHealth = 50;
    public int currentHealth;
    public int damage = 0;

    [Header("Damage Values")]
    public int swordDamage;
    public int axeDamage;
    public int clawDamage;
    public int bowDamage;

    [Header("Screen Shake")]
    public float shakeDuration = 0.5f;
    public float shakeMagnitude = 0.9f;

    [SerializeField]
    private RuntimeAnimatorController[] animatorControllers;
    #endregion

    #region Private Fields

    private AudioSource audioSource;
    private Vector2 originalPosition;
    private RectTransform rectTransform;
    private bool playerTurn = true;
    private bool isEmpowered = false;
    private float damageMultiplier = 1.0f;
    private bool isShieldActive = false;
    private float shieldBlockChance = 0.75f;
    private bool skipEnemyTurn = false;
    private BattleMenu battleMenu;
    private GameObject debugMenu;

    #endregion

    #region Structs / Classes

    [System.Serializable]
    public class AttackButtonInfo
    {
        public string attackName;
        public Button button;
        [HideInInspector] public int cooldown = 0;
        public int maxCooldown = 2;
    }

    #endregion

    #region Unity Callbacks

    private void Start()
    {
        WorldPlayer = GameObject.Find("WorldPlayer");
        debugMenu = GameObject.Find("DebugMenu");
        enemyHealthSlider = GameObject.Find("EnemyHealthSlider");
        enemyCombatStats = GameObject.Find("EnemyCombatStats");

        // Add cooldowns to UI for each action
        for (int i = 0; i < cooldowns.Length; i++)
        {
            cooldowns[i].text = "Cooldown\n" + attackButtonInfos[i].maxCooldown.ToString();
        }

        // PlayerPrefs.SetInt("Health", maxHealth);
        currentHealth = (int)HealthManager.Instance.GetHealth();
        UpdateHealthUI();
        SetAttackButtonsInteractable();

        // Assign the first found Enemy in the scene
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        if (enemies.Length > 0)
        {
            enemy = enemies[0].GetComponent<Enemy>();
        }

        rectTransform = GetComponentInChildren<RectTransform>();
        if (rectTransform != null)
        {
            originalPosition = rectTransform.anchoredPosition;
        }

        audioSource = GetComponent<AudioSource>();

        UpdateText("What would you like to do?");

        // Setup Attack Damage from ItemManager
        ItemManager im = itemManager;
        swordDamage = im.attack.attackTypes[0].basic + im.attack.attackTypes[0].itemEnhancement;
        axeDamage   = im.attack.attackTypes[1].basic + im.attack.attackTypes[1].itemEnhancement;
        clawDamage  = im.attack.attackTypes[2].basic + im.attack.attackTypes[2].itemEnhancement;
        bowDamage   = im.attack.attackTypes[3].basic + im.attack.attackTypes[3].itemEnhancement;
        foreach (var attackInfo in attackButtonInfos)
        {
            attackInfo.cooldown = 0;
        }

        AssignButtonListeners();

        // Find the BattleMenu in the scene
        battleMenu = GameObject.Find("BattleMenu").GetComponent<BattleMenu>();

        UpdateItemCountUI();
    }

    #endregion

    #region Public Methods

    public void UpdateText(string newText)
    {
        if (battleText != null)
        {
            battleText.text = newText;
        }
    }

    public void TakeDamage(int damageAmount)
    {
        if (!debugMenu.GetComponent<DebugMenu>().inGodMode())
        {
            if (isShieldActive)
            {
                float roll = Random.Range(0f, 1f);
                if (roll <= shieldBlockChance)
                {
                    UpdateText("Your shield blocked the attack!");
                    return;
                }
                else
                {
                    UpdateText("Your shield failed to block the attack.");
                    isShieldActive = false;
                    combatStats.GetComponent<CombatStats>().UnsetStat(0);
                }
            }

            currentHealth -= damageAmount;
            currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
            UpdateHealthUI();
        }

        StartCoroutine(Shake());
        if (currentHealth <= 0)
        {
            // Immediately stop all battle interactions
            playerTurn = false;
            if (battleMenu != null)
            {
                battleMenu.SetMenusInteractable(false);
            }
            StopAllCoroutines(); // Stop any ongoing battle sequences
            StartCoroutine(DelayedDeath());
            return;
        }
    }

    public void PlayerAttack(string attack)
    {
        if (playerTurn && enemy != null)
        {
            switch (attack)
            {
                case "Empower":
                    PlayEffect(5);
                    PerformEmpower();
                    break;

                case "Shield":
                    PlayEffect(7);
                    PerformShield();
                    break;

                case "AttackWithDOT":
                    PlayEffect(2);
                    PerformAttackWithDOT();
                    break;

                case "SkipEnemyTurn":
                    PlayEffect(4);
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

                case "HealSelf":
                    PerformHealSelf();
                    PlayEffect(6);
                    break;

                case "Apple":
                case "Milk":
                case "Cookie":
                case "Banana":
                    if (!itemManager.HasConsumable(attack))
                    {
                        UpdateText("No " + attack + " left!");
                        return;
                    }
                    else
                    {
                        PerformConsumeItem(attack);
                        UpdateItemCountUI();
                    }
                    break;

                case "Flee":
                    PerformFlee();
                    return; // Add this to prevent the battle sequence from starting

                default:
                    PlayEffect(0);
                    PerformRegularAttack(attack);
                    break;
            }

            // End of player's turn
            playerTurn = false;
            if (battleMenu != null)
            {
                battleMenu.SetMenusInteractable(false);
            }

            // Apply cooldown to the used attack
            for (int i = 0; i< attackButtonInfos.Length; i++)
            {
                if (attackButtonInfos[i].attackName == attack)
                {
                    attackButtonInfos[i].cooldown = attackButtonInfos[i].maxCooldown;
                    attackButtonInfos[i].button.interactable = false;
                    cooldowns[i].text = "Cooldown\n" + attackButtonInfos[i].cooldown;
                    break;
                }
            }

            StartCoroutine(BattleSequence());

            attackPanel.SetActive(false);
            spellPanel.SetActive(false);
            helpPanel.SetActive(false);
            mainMenuPanel.SetActive(true);
        }
    }

    public void Heal(int health)
    {
        currentHealth += health;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHealthUI();
        UpdateText("You healed!");
        //PlayHealingEffect();

        playerTurn = false;
        if (battleMenu != null)
        {
            battleMenu.SetMenusInteractable(false);
        }

        StartCoroutine(BattleSequence());

        spellPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }

    public void EnemyDead()
    {
        healthSlider.gameObject.SetActive(false);
        combatStats.SetActive(false);
        enemyCombatStats.SetActive(false);
    }

    #endregion

    #region Private Methods

    private void AssignButtonListeners()
    {
        foreach (var attackInfo in attackButtonInfos)
        {
            if (attackInfo.button != null)
            {
                attackInfo.button.onClick.RemoveAllListeners();
                string currentAttack = attackInfo.attackName;
                attackInfo.button.onClick.AddListener(() => PlayerAttack(currentAttack));
            }
        }
    }

    private IEnumerator Shake()
    {
        float elapsed = 0.0f;

        while (elapsed < shakeDuration)
        {
            float offsetX = Random.Range(-1f, 1f) * shakeMagnitude * 10;
            float offsetY = Random.Range(-1f, 1f) * shakeMagnitude * 10;

            rectTransform.anchoredPosition = new Vector2(
                originalPosition.x + offsetX,
                originalPosition.y + offsetY
            );

            elapsed += Time.deltaTime;
            yield return null;
        }

        rectTransform.anchoredPosition = originalPosition;
    }

    private void UpdateHealthUI()
    {
        if (healthText != null)
        {
            healthText.text = "HP: " + currentHealth;
        }

        if (healthSlider != null)
        {
            healthSlider.value = (float)currentHealth / maxHealth;
        }

        HealthManager.Instance.SetHealth(currentHealth);
    }

    private IEnumerator DelayedDeath()
    {
        // Wait to show the attack effects
        yield return new WaitForSeconds(2.0f);
        
        // Show death message
        UpdateText("You died!");
        
        // Wait briefly to show death message
        yield return new WaitForSeconds(2.0f);
        
        // Now hide the UI and transition
        mainMenuPanel.SetActive(false);
        attackPanel.SetActive(false);
        spellPanel.SetActive(false);
        helpPanel.SetActive(false);
        
        // Clean up and transition
        if (WorldPlayer != null)
        {
            WorldPlayer.GetComponent<UniversalAudioHandling>().Die();
        }
        SceneManager.LoadScene("DeathScene");
    }

    private void PerformRegularAttack(string attackName)
    {
        //Randomizer
        int var = GetVar(2);
        Debug.Log("Random Number Added: " + var);
        int actualDamage = Mathf.RoundToInt(swordDamage * damageMultiplier + var);

        if (GameObject.Find("DebugMenu").GetComponent<DebugMenu>().inGodMode())
        {
            actualDamage = 10000;
        }

        enemy.TakeDamage(actualDamage);
        damageMultiplier = 1.0f;
        isEmpowered = false;
        combatStats.GetComponent<CombatStats>().UnsetStat(1);
        UpdateText("You used Slash to deal damage!");
    }

    private void PerformConsumeItem(string itemName)
    {
        //Randomizer
        int var = GetVar(1);
        Debug.Log("Random Number Added: " + var);
        int healAmount = 10 + var;
        HealPlayer(healAmount);
        UpdateText("You used " + itemName + "!");
        itemManager.UseConsumable(itemName);
    }

    private void PerformAttackWithDOT()
    {
        //Randomizer
        int var = GetVar(1);
        Debug.Log("Random Number Added: " + var);
        int actualDamage = Mathf.RoundToInt(clawDamage * damageMultiplier + var);

        if (GameObject.Find("DebugMenu").GetComponent<DebugMenu>().inGodMode())
        {
            actualDamage = 10000;
        }

        enemy.TakeDamage(actualDamage);
        int duration = 3;
        damageMultiplier = 1.0f;
        isEmpowered = false;
        enemy.ApplyDOT(clawDamage, duration);
        combatStats.GetComponent<CombatStats>().UnsetStat(1);
        UpdateText("You used Shadow Scratch to deal DOT!");
    }

    private void PerformEmpower()
    {
        damageMultiplier = 1.5f;
        isEmpowered = true;

        //Randomizer
        int var = Random.Range(0, 3) - 1;
        Debug.Log("Random Number Added: " + var);
        int healAmount = 10 + var;
        HealPlayer(healAmount);

        combatStats.GetComponent<CombatStats>().SetStat(1);
        UpdateText("You empowered yourself!");
    }

    private void PerformShield()
    {
        isShieldActive = true;
        combatStats.GetComponent<CombatStats>().SetStat(0);
        UpdateText("You raised a shield!");
    }

    private void PerformSkipEnemyTurn()
    {
        int actualDamage = Mathf.RoundToInt(5 * damageMultiplier);

        if (GameObject.Find("DebugMenu").GetComponent<DebugMenu>().inGodMode())
        {
            actualDamage = 10000;
        }
        damageMultiplier = 1.0f;
        isEmpowered = false;
        enemy.TakeDamage(actualDamage);
        skipEnemyTurn = true;
        combatStats.GetComponent<CombatStats>().UnsetStat(1);
        UpdateText("You used Time Skip to skip the enemies turn!");
    }

    private void PerformDoubleDamage()
    {
        //Randomizer
        int var = GetVar(4);
        Debug.Log("Random Number Added: " + var);
        int actualDamage = Mathf.RoundToInt(axeDamage * damageMultiplier + var);

        if (GameObject.Find("DebugMenu").GetComponent<DebugMenu>().inGodMode())
        {
            actualDamage = 10000;
        }
        damageMultiplier = 1.0f;
        isEmpowered = false;
        enemy.TakeDamage(actualDamage);
        combatStats.GetComponent<CombatStats>().UnsetStat(1);
        UpdateText("You used Heavy Axe for double damage!");
    }

    private void PerformWeakenEnemy()
    {
        //Randomizer
        int var = Random.Range(0, 3) - 1;
        int actualDamage = Mathf.RoundToInt(bowDamage * damageMultiplier + var);

        if (GameObject.Find("DebugMenu").GetComponent<DebugMenu>().inGodMode())
        {
            actualDamage = 10000;
        }
        damageMultiplier = 1.0f;
        isEmpowered = false;
        enemy.TakeDamage(actualDamage);
        enemy.SetWeakened(true);
        combatStats.GetComponent<CombatStats>().UnsetStat(1);
        UpdateText("You used Sharp Shot to weaken the enemy!");
    }

    private void PerformHealSelf()
    {
        //Randomizer
        int var = GetVar(3);
        Debug.Log("Random Number Added: " + var);
        int healAmount = 30 + var;
        HealPlayer(healAmount);
        UpdateText("You used Healing Winds to heal yourself!");
    }

    private void HealPlayer(int health)
    {
        currentHealth += health;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHealthUI();
        //PlayHealingEffect();
    }

    private void PlayHealingEffect()
    {
        if (healParticleEffect != null)
        {
            healParticleEffect.Emit(100);
        }
    }

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

    private void SetAttackButtonsInteractable()
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

    private void UpdateAttackCooldowns()
    {
        for (int i = 0; i < attackButtonInfos.Length; i++)
        {
            if (attackButtonInfos[i].cooldown > 0)
            {
                attackButtonInfos[i].cooldown--;
                cooldowns[i].text = "Cooldown\n" + attackButtonInfos[i].cooldown;
                if (attackButtonInfos[i].cooldown == 0)
                {
                    attackButtonInfos[i].button.interactable = playerTurn;
                    cooldowns[i].text = "Cooldown\n" + attackButtonInfos[i].maxCooldown.ToString();
                }
            }
        }
    }

    private void PlayEffect(int index)
    {
        if (index < 5)
        {
            combatVFX.GetComponent<Animator>().runtimeAnimatorController = animatorControllers[index];
            if (index == 0)
            {
                combatVFX.GetComponent<Animator>().Play("Sword_Animation");
            }
            else if (index == 1)
            {
                combatVFX.GetComponent<Animator>().Play("Axe_Animation");
            }
            else if (index == 2)
            {
                combatVFX.GetComponent<Animator>().Play("Scratch_Animation");
            }
            else if (index == 3)
            {
                combatVFX.GetComponent<Animator>().Play("Slingshot_Animation");
            }
            else if (index == 4)
            {
                combatVFX.GetComponent<Animator>().Play("Time_Skip_Animation");
            }
        }
        else
        {
            spellVFX.GetComponent<Animator>().runtimeAnimatorController = animatorControllers[index];
            if (index == 5)
            {
                spellVFX.GetComponent<Animator>().Play("Empower_Animation");
            }
            else if (index == 6)
            {
                spellVFX.GetComponent<Animator>().Play("Healing_Animation");
            }
            else if (index == 7)
            {
                spellVFX.GetComponent<Animator>().Play("Shield_Animation");
            }
        }
        Invoke("ResetEffect", 1.0f);
    }

    private void ResetEffect()
    {
        combatVFX.GetComponent<Animator>().runtimeAnimatorController = null;
        spellVFX.GetComponent<Animator>().runtimeAnimatorController = null;
    }

    private void PerformFlee()
    {
        float healthPercentage = (float)currentHealth / maxHealth;
        float baseChance = 0.8f; // 80% max success rate
        float fleeChance = baseChance * healthPercentage;
        
        float roll = Random.Range(0f, 1f);
        Debug.Log($"Flee chance: {fleeChance}, roll: {roll}");
        
        if (roll <= fleeChance)
        {
            UpdateText("You successfully fled from battle!");
            StartCoroutine(DelayedFleeSuccess());
        }
        else
        {
            int stumbleDamage = Mathf.RoundToInt(maxHealth * 0.1f);
            UpdateText($"You stumbled and took damage!");
            TakeDamage(stumbleDamage);
            
            playerTurn = false;
            if (battleMenu != null)
            {
                battleMenu.SetMenusInteractable(false);
            }
            StartCoroutine(BattleSequence());
        }
    }

    private IEnumerator DelayedFleeSuccess()
    {
        yield return new WaitForSeconds(2.0f);
        healthSlider.gameObject.SetActive(false);
        enemyHealthSlider.gameObject.SetActive(false);
        combatStats.SetActive(false);
        enemyCombatStats.SetActive(false);
        if (battleMenu != null)
        {
            battleMenu.Flee();
        }
    }

    #endregion

    #region Coroutines

    private IEnumerator BattleSequence()
    {
        if (enemy.GetDOTTurn() > 0)
        {
            yield return StartCoroutine(EnemyDOTTurn());
        }

        if (skipEnemyTurn)
        {
            skipEnemyTurn = false;
            yield return StartCoroutine(EnemySkipTurn());
        }
        else
        {
            yield return StartCoroutine(EnemyAttackTurn());
        }
    }

    private IEnumerator EnemyDOTTurn()
    {
        yield return new WaitForSeconds(2);
        UpdateText("The enemy was hurt!");
        enemy.ApplyTurnEffects();
    }

    private IEnumerator EnemySkipTurn()
    {
        yield return new WaitForSeconds(2);
        enemy.EnemySkip(this);
        playerTurn = true;

        if (battleMenu != null)
        {
            battleMenu.SetMenusInteractable(true);
        }

        UpdateAttackCooldowns();
        SetAttackButtonsInteractable();
    }

    private IEnumerator EnemyAttackTurn()
    {
        if (enemy.currentHealth > 0)
        {
            yield return new WaitForSeconds(2);
            enemy.EnemyAttack(this);
            yield return new WaitForSeconds(1);

            playerTurn = true;
            if (battleMenu != null)
            {
                battleMenu.SetMenusInteractable(true);
            }

            UpdateAttackCooldowns();
            SetAttackButtonsInteractable();
        }
        else
        {
            yield return new WaitForSeconds(1);
        }
    }

    // INPUT - order of magnitude of variance - 2 gives any number from -2 through 2
    private int GetVar(int inp)
    {
        int right = inp * 2 + 1; // give 3 - right = 7
        int rand = Random.Range(0, right); // anywhere from 0 - 6
        rand = rand - inp; // subtract 3 - -3 -2 -1 0 1 2 3
        return rand;
    }

    #endregion
}