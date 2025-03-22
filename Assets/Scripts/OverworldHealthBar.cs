using UnityEngine;
using UnityEngine.UI;

public class OverworldHealthBar : MonoBehaviour
{
    public Slider hpSlider;
    private float lastKnownHP;
    private const float MAX_HEALTH = 50f;

    void Start()
    {
        // Set default health if it doesn't exist
        if (!PlayerPrefs.HasKey("Health"))
        {
            PlayerPrefs.SetInt("Health", (int)MAX_HEALTH);
        }

        float currentHP = PlayerPrefs.GetInt("Health") / MAX_HEALTH;
        lastKnownHP = currentHP;
        hpSlider.value = currentHP;
    }

    void Update()
    {
        float currentHP = PlayerPrefs.GetInt("Health") / MAX_HEALTH;
        if (Mathf.Abs(currentHP - lastKnownHP) > 0.01f)
        {
            hpSlider.value = currentHP;
            lastKnownHP = currentHP;
        }
    }
}