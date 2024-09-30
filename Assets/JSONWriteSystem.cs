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

    public void SaveToJson()
    {
        EnemyData data = new EnemyData();
        data.Id     = idInputField.text;
        data.Name   = NameInputField.text;
        data.Hp     = HpInputField.text;
        data.Damage = DamageInputField.text;

        EnemyList enemyList = new EnemyList();
        string tempJson = "";
        if (File.Exists(Application.dataPath + "/EnemyDataFile.json"))
        {
            tempJson = File.ReadAllText(Application.dataPath + "/EnemyDataFile.json");
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
            File.WriteAllText(Application.dataPath + "/EnemyDataFile.json", json);
        }
        else
        {
            EnemyData[] enemies = new EnemyData[1];
            enemies[0] = data;
            enemyList.Enemies = enemies;
            string json = JsonUtility.ToJson(enemyList, true);
            File.WriteAllText(Application.dataPath + "/EnemyDataFile.json", json);
        }
    }
}
