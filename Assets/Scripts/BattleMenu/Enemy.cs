using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private GameObject combatStats;
    [SerializeField] private GameObject player;

    private GameObject playerCombatStats;
    private GameObject MenuManager;
    public int maxHealth = 100;
    public int currentHealth;
    public string name;

    public string[] attacks;
    public string[] attackNames;

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
    public string[] attackSounds;

    public bool isShieldActive = false;
    public float shieldReduction = 0.5f;

    private bool isWeakened = false;
    private float weakenedDamageMultiplier = 0.5f;

    void Start()
    {
        player = GameObject.Find("Player");
        MenuManager = GameObject.Find("BattleMenu");
        combatStats = GameObject.Find("EnemyCombatStats");
        playerCombatStats = GameObject.Find("CombatStats");
        currentHealth = maxHealth;
        UpdateHealthUI();
        audioSource = GetComponent<AudioSource>();
        this.GetComponent<Fade>().ChangeColor(new Vector3(255, 255, 255), 1.0f);
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
            combatStats.GetComponent<CombatStats>().UnsetStat(0);
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

    public void CritAttack(int damageToDeal, PlayerHealth player, string attackName)
    {
        int criticalDamage = Mathf.RoundToInt(damageToDeal * 1.5f);
        player.UpdateText("The " + name + " performed a critical hit with " + attackName + "!");
        audioSource.PlayOneShot(Resources.Load<AudioClip>(attackSounds[0]));
        player.TakeDamage(criticalDamage);  
    }

    public void regAttack(int damageToDeal, PlayerHealth player, string attackName)
    {
        player.UpdateText("The " + name + " attacked with " + attackName + "!");
        audioSource.PlayOneShot(Resources.Load<AudioClip>(attackSounds[2]));
        player.TakeDamage(damageToDeal);
    }

    public void shield(PlayerHealth player, string shieldName)
    {
        isShieldActive = true;
        combatStats.GetComponent<CombatStats>().SetStat(0);
        player.UpdateText("The " + name + " raised a " + shieldName);
        audioSource.PlayOneShot(Resources.Load<AudioClip>(attackSounds[1]));
    }

    public void heal(PlayerHealth player, int healAmount, string attackName)
    {
        currentHealth += healAmount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHealthUI();
        player.UpdateText("The " + name + " used " + attackName + " to heal!");
        audioSource.PlayOneShot(Resources.Load<AudioClip>(attackSounds[1]));
    }

    public void EnemyAttack(PlayerHealth player)
    {
        float actionRoll = Random.Range(0f, 1f);
        int damageToDeal = attackDamage;

        if (actionRoll <= 0.15f)
        {
            switch(attacks[0])
            {
                case "CritAttack":
                    CritAttack(damageToDeal, player, attackNames[0]);
                    if (isWeakened)
                    {
                        damageToDeal = Mathf.RoundToInt(damageToDeal * weakenedDamageMultiplier);
                        isWeakened = false;
                        combatStats.GetComponent<CombatStats>().UnsetStat(2);
                        player.UpdateText("The " + name + "'s attack is weakened!");
                    }
                    break;
                case "Shield":
                    shield(player, attackNames[0]);
                    break;
                case "Heal":
                    heal(player, 15, attackNames[0]);
                    break;
                default:
                    if (isWeakened)
                    {
                        damageToDeal = Mathf.RoundToInt(damageToDeal * weakenedDamageMultiplier);
                        isWeakened = false;
                        combatStats.GetComponent<CombatStats>().UnsetStat(2);
                        player.UpdateText("The " + name + "'s attack is weakened!");
                    }
                    regAttack(damageToDeal, player, attackNames[0]);
                    break;
            }
        }
        else if (actionRoll <= 0.35f)
        {
            switch(attacks[1])
            {
                case "CritAttack":
                    CritAttack(damageToDeal, player, attackNames[1]);
                    if (isWeakened)
                    {
                        damageToDeal = Mathf.RoundToInt(damageToDeal * weakenedDamageMultiplier);
                        isWeakened = false;
                        combatStats.GetComponent<CombatStats>().UnsetStat(2);
                        player.UpdateText("The " + name + "'s attack is weakened!");
                    }
                    break;
                case "Shield":
                    shield(player, attackNames[1]);
                    break;
                case "Heal":
                    heal(player, 10, attackNames[1]);
                    break;
                default:
                    if (isWeakened)
                    {
                        damageToDeal = Mathf.RoundToInt(damageToDeal * weakenedDamageMultiplier);
                        isWeakened = false;
                        combatStats.GetComponent<CombatStats>().UnsetStat(2);
                        player.UpdateText("The " + name + "'s attack is weakened!");
                    }
                    regAttack(damageToDeal, player, attackNames[1]);
                    break;
            }
        }
        else
        {
            switch(attacks[2])
            {
                case "CritAttack":
                    if (isWeakened)
                    {
                        damageToDeal = Mathf.RoundToInt(damageToDeal * weakenedDamageMultiplier);
                        isWeakened = false;
                        combatStats.GetComponent<CombatStats>().UnsetStat(2);
                        player.UpdateText("The " + name + "'s attack is weakened!");
                    }
                    CritAttack(damageToDeal, player, attackNames[2]);
                    break;
                case "Shield":
                    shield(player, attackNames[2]);
                    break;
                case "Heal":
                    heal(player, 5, attackNames[2]);
                    break;
                default:
                    if (isWeakened)
                    {
                        damageToDeal = Mathf.RoundToInt(damageToDeal * weakenedDamageMultiplier);
                        isWeakened = false;
                        combatStats.GetComponent<CombatStats>().UnsetStat(2);
                        player.UpdateText("The " + name + "'s attack is weakened!");
                    }
                    regAttack(damageToDeal, player, attackNames[2]);
                    break;
            }
        }
    }

    public void EnemySkip(PlayerHealth player)
    {
        Debug.Log("Enemy skipped their turn!");
        player.UpdateText("Enemy skipped their turn!");
    }

    void Die()
    {
        Debug.Log("Enemy died!");
        player.GetComponent<PlayerHealth>().EnemyDead();
        combatStats.SetActive(false);
        playerCombatStats.SetActive(false);
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

            if (dotTurnsRemaining > 0)
            {
                combatStats.GetComponent<CombatStats>().SetStat(3);
            }
            else
            {
                combatStats.GetComponent<CombatStats>().UnsetStat(3);
            }

            if (currentHealth <= 0)
            {
                Die();
            }
        }
    }

    public void SetWeakened(bool status)
    {
        isWeakened = status;
        if (isWeakened)
        {
            combatStats.GetComponent<CombatStats>().SetStat(2);
            Debug.Log("Enemy has been weakened. Their next attack will deal reduced damage.");
        }
    }
}