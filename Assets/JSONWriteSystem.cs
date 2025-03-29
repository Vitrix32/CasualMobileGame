using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class JSONWriteSystem : MonoBehaviour
{
    public TMPro.TMP_InputField idInputField;
    public TMPro.TMP_InputField NameInputField;
    public TMPro.TMP_InputField HpInputField;
    public TMPro.TMP_InputField DamageInputField;

    private string enemyDataPath;

    private void Awake()
    {
        // Set up path
        enemyDataPath = Path.Combine(Application.persistentDataPath, "EnemyDataFile.json");
        
        // Check if the file exists in persistent data path, if not, copy default
        if (!File.Exists(enemyDataPath))
        {
            // Check if default exists in dataPath
            string defaultPath = Path.Combine(Application.dataPath, "EnemyDataFile.json");
            if (File.Exists(defaultPath))
            {
                // Copy from dataPath to persistentDataPath
                string defaultJson = File.ReadAllText(defaultPath);
                File.WriteAllText(enemyDataPath, defaultJson);
                Debug.Log("Copied default enemy data to: " + enemyDataPath);
            }
        }
    }

    public void SaveToJson()
    {
        EnemyData data = new EnemyData();
        data.Id     = idInputField.text;
        data.Name   = NameInputField.text;
        data.Hp     = HpInputField.text;
        data.Damage = DamageInputField.text;

        EnemyList enemyList = new EnemyList();
        if (File.Exists(enemyDataPath))
        {
            string tempJson = File.ReadAllText(enemyDataPath);
            enemyList = JsonUtility.FromJson<EnemyList>(tempJson);
            List<EnemyData> list = new List<EnemyData>();
            for (int i = 0; i < enemyList.Enemies.Length; i++)
            {
                if (enemyList.Enemies[i].Id == data.Id)
                {
                    Debug.Log("Cannot input another enemy with the same ID");
                    return;
                }
                list.Add(enemyList.Enemies[i]);
            }
            list.Add(data);
            enemyList.Enemies = list.ToArray();
            string json = JsonUtility.ToJson(enemyList, true);
            File.WriteAllText(enemyDataPath, json);
        }
        else
        {
            EnemyData[] enemies = new EnemyData[1];
            enemies[0] = data;
            enemyList.Enemies = enemies;
            string json = JsonUtility.ToJson(enemyList, true);
            File.WriteAllText(enemyDataPath, json);
        }
        
        Debug.Log("Saved enemy data to: " + enemyDataPath);
    }
}
