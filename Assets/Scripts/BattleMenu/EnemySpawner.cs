using System;
using Unity.VisualScripting;
using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Linq;


public class EnemySpawner : MonoBehaviour
{
    //Add a "public GameObject enemyPrefab" ???
    public TextAsset enemyJson;

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
        enemyPrefabName = "GenericEnemyTemplate";

        // Call the Spawn method repeatedly, you can also use your logic for conditional spawns
        //InvokeRepeating("SpawnEnemy", 2f, spawnInterval);
        SpawnEnemy();
    }

    // This method will spawn the enemy at a random spawn point
    void SpawnEnemy()
    {
        // Get the main camera
        Camera mainCamera = Camera.main;
        
        // Calculate spawn position based on screen width
        float screenWidth = mainCamera.orthographicSize * mainCamera.aspect;
        float horizontalOffset = screenWidth * 0.90f; // Adjust percentage as needed
        Vector3 spawnOffset = new Vector3(horizontalOffset, 0.25f, 0f);

        Vector3 spawnPosition = player.position + spawnOffset;
        
        // Dynamically load the enemy prefab using the provided name
        GameObject enemyPrefab = Resources.Load<GameObject>("Enemies/" + enemyPrefabName);

        Enemy enemy = enemyPrefab.GetComponent<Enemy>();

        if(enemy != null) {
            //ReadEnemyFromJson reader = enemy.AddComponent<ReadEnemyFromJson>();
            EnemyData data = ReturnRandomEnemy();

            enemy.attackDamage = Int32.Parse(data.Damage);
            enemy.maxHealth = Int32.Parse(data.Hp);
            enemy.name = data.Name;
            enemy.attacks = data.Attacks;
            enemy.attackNames = data.AttackNames;
            enemy.attackSounds = data.AudioClips;

            Sprite newSprite = Resources.Load("EnemyArt/" + data.Name, typeof(Sprite)) as Sprite;
            SpriteRenderer spriteRenderer = enemy.GetComponent<SpriteRenderer>();
            spriteRenderer.sprite = newSprite;
        }

        if (enemyPrefab != null)
        {
            // Instantiate the enemy at the calculated position
            Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
        }
    }

    private EnemyData[] ReturnEnemyArray()
    {
        EnemyList enemyList = new EnemyList();
        enemyList = JsonUtility.FromJson<EnemyList>(enemyJson.text);
        List<EnemyData> list = new List<EnemyData>();
        for (int i = 0; i < enemyList.Enemies.Length; i++)
        {
            // GET ENEMIES BASED ON LOCATION
            int locationID = PlayerPrefs.GetInt("LocID");
            if (locationID == Int32.Parse(enemyList.Enemies[i].LocID))
            {
                list.Add(enemyList.Enemies[i]);
            }
        }
        return list.ToArray();

    }

    // Can easily change this function to pull random enemy for a specific
    // location: add an integer input, pass to function above
    public EnemyData ReturnRandomEnemy()
    {
        EnemyData[] enemyList = ReturnEnemyArray();
        int min = 0;
        int max = enemyList.Length;
        int rand = UnityEngine.Random.Range(min, max);
        return enemyList[rand];
    }
}
