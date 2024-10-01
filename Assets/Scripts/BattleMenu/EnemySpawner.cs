using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    // The name of the enemy prefab to load dynamically
    public string enemyPrefabName;

    // Spawn position or region
    public Transform spawnPoint;

    // The interval between spawns (optional)
    public float spawnInterval = 3f;

    // Start is called before the first frame update
    void Start()
    {
        string[] enemyTypes = { "Goblin", "Lion", "Wolf" };
        int randomIndex = Random.Range(0, enemyTypes.Length);
        enemyPrefabName = enemyTypes[randomIndex];

        // Call the Spawn method repeatedly, you can also use your logic for conditional spawns
        //InvokeRepeating("SpawnEnemy", 2f, spawnInterval);
        SpawnEnemy();
    }

    // This method will spawn the enemy at a random spawn point
    void SpawnEnemy()
    {
        spawnPoint.position = new Vector3(5, 2.5f, 0);
        // Dynamically load the enemy prefab using the provided name
        GameObject enemyPrefab = Resources.Load<GameObject>("Enemies/" + enemyPrefabName);

        if (enemyPrefab != null && spawnPoint != null)
        {
            // Instantiate the enemy at the spawn point's position and rotation
            Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
        }
        else
        {
            Debug.LogError("Enemy prefab or spawn point is not properly set.");
        }
    }
}
