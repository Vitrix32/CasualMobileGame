using UnityEngine;

public class statueHealTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "WorldPlayer")
        {
            HealthManager.Instance.SetHealth(HealthManager.MAX_HEALTH);
            Debug.Log("Player healed");
        }
    }
}