using UnityEngine;

public class statueHealTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "WorldPlayer")
        {
            PlayerPrefs.SetInt("Health", 50);
            Debug.Log("Player healed");
        }
    }
}