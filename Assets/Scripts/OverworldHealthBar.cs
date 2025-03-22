using UnityEngine;
using UnityEngine.UI;

public class OverworldHealthBar : MonoBehaviour
{
    public Slider hpSlider;
    private float lastKnownHP;

    void Start()
    {
        float currentHP = HealthManager.Instance.GetHealth() / HealthManager.MAX_HEALTH;
        lastKnownHP = currentHP;
        hpSlider.value = currentHP;
    }

    void Update()
    {
        float currentHP = HealthManager.Instance.GetHealth() / HealthManager.MAX_HEALTH;
        if (Mathf.Abs(currentHP - lastKnownHP) > 0.01f)
        {
            hpSlider.value = currentHP;
            lastKnownHP = currentHP;
        }
    }
}