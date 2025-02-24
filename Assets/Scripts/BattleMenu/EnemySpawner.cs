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
        Vector3 spawnOffset = new Vector3(9f, 0.25f, 0f); // Adjust as needed

        Vector3 spawnPoint = player.position + spawnOffset;
        //Debug.Log(player.position);
        //Debug.Log(spawnPoint);
        Debug.Log("text of enemy.json" + enemyJson.text);
        // Dynamically load the enemy prefab using the provided name
        GameObject enemyPrefab = Resources.Load<GameObject>("Enemies/" + enemyPrefabName);

        Enemy enemy = enemyPrefab.GetComponent<Enemy>();

        if(enemy != null) {
            //ReadEnemyFromJson reader = enemy.AddComponent<ReadEnemyFromJson>();
            EnemyData data = ReturnRandomEnemy();

            enemy.attackDamage = Int32.Parse(data.Damage);
            enemy.maxHealth = Int32.Parse(data.Hp);

            Sprite newSprite = Resources.Load("EnemyArt/" + data.Name, typeof(Sprite)) as Sprite;
            //Debug.Log("EnemyArt/" + data.Name);
            //Debug.Log(newSprite);
            SpriteRenderer spriteRenderer = enemy.GetComponent<SpriteRenderer>();
            spriteRenderer.sprite = newSprite;

            //Debug.Log(data.Name);
        }

        if (enemyPrefab != null && spawnPoint != null)
        {
            // Instantiate the enemy at the spawn point's position and rotation
            Instantiate(enemyPrefab, spawnPoint, Quaternion.identity);
        }
        else
        {
            //Debug.LogError("Enemy prefab or spawn point is not properly set.");
        }
    }

    private EnemyData[] ReturnEnemyArray()
    {
        EnemyList enemyList = new EnemyList();
       // Debug.Log("text file read in: " + enemyJson.text);
        //string tempJson = File.ReadAllText("EnemyDataFile.json");
        enemyList = JsonUtility.FromJson<EnemyList>(enemyJson.text);
        List<EnemyData> list = new List<EnemyData>();
        for (int i = 0; i < enemyList.Enemies.Length; i++)
        {
            // ADDING FUNCTIONALITY - IF STATEMENT - GET ENEMIES BASED ON LOCATION
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
