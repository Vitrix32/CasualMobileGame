using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int health = 100;
    public int attackDamage = 20;

    public void TakeDamage(int damage)
    {
        health -= damage;
        Debug.Log("Enemy took " + damage + " damage! Health remaining: " + health);

        if (health <= 0)
        {
            Die();
        }
    }

    // Method to attack the player
    public void EnemyAttack(Player player)
    {
        Debug.Log("Enemy attacks!");
        player.TakeDamage(attackDamage); // Enemy attacks the player
    }

    // Handle enemy death
    void Die()
    {
        Debug.Log("Enemy died!");
        Destroy(gameObject); // Destroy enemy when health is 0
    }
}
