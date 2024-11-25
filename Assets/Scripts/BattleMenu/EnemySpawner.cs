using System;
using Unity.VisualScripting;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    // The name of the enemy prefab to load dynamically
    public string enemyPrefabName;

    // Spawn position or region
    public Transform spawnPoint;

    public Transform player; // Assign this in the Inspector

    // The interval between spawns (optional)
    public float spawnInterval = 3f;

    // Start is called before the first frame update
    void Start()
    {
        /*
        string[] enemyTypes = { "Goblin", "Lion", "Wolf" };
        int randomIndex = Random.Range(0, enemyTypes.Length);
        enemyPrefabName = enemyTypes[randomIndex];
        */
        enemyPrefabName = "GenericEnemyTemplate";

        // Call the Spawn method repeatedly, you can also use your logic for conditional spawns
        //InvokeRepeating("SpawnEnemy", 2f, spawnInterval);
        SpawnEnemy();
    }

    // This method will spawn the enemy at a random spawn point
    void SpawnEnemy()
    {
        Vector3 spawnOffset = new Vector3(10f, 0f, 0f); // Adjust as needed

        Vector3 spawnPoint = player.position + spawnOffset;
        Debug.Log(player.position);
        Debug.Log(spawnPoint);

        // Dynamically load the enemy prefab using the provided name
        GameObject enemyPrefab = Resources.Load<GameObject>("Enemies/" + enemyPrefabName);

        Enemy enemy = enemyPrefab.GetComponent<Enemy>();

        if(enemy != null) {
            ReadEnemyFromJson reader = enemy.AddComponent<ReadEnemyFromJson>();
            EnemyData data = reader.ReturnRandomEnemy();

            enemy.attackDamage = Int32.Parse(data.Damage);
            enemy.maxHealth = Int32.Parse(data.Hp);

            Sprite newSprite = Resources.Load("EnemyArt/" + data.Name, typeof(Sprite)) as Sprite;
            Debug.Log("EnemyArt/" + data.Name);
            Debug.Log(newSprite);
            SpriteRenderer spriteRenderer = enemy.GetComponent<SpriteRenderer>();
            spriteRenderer.sprite = newSprite;

            Debug.Log(data.Name);
        }

        if (enemyPrefab != null && spawnPoint != null)
        {
            // Instantiate the enemy at the spawn point's position and rotation
            Instantiate(enemyPrefab, spawnPoint, Quaternion.identity);
        }
        else
        {
            Debug.LogError("Enemy prefab or spawn point is not properly set.");
        }
    }
}
