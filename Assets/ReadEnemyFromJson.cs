using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ReadEnemyFromJson : MonoBehaviour
{
    // To use for a specific location: integer input + check in for loop for
    // if location ID matches ID passed in
    private EnemyData[] ReturnEnemyArray()
    {
        if (File.Exists(Application.dataPath + "/EnemyDataFile.json"))
        {
            EnemyList enemyList = new EnemyList();
            string tempJson = File.ReadAllText(Application.dataPath + "/EnemyDataFile.json");
            enemyList = JsonUtility.FromJson<EnemyList>(tempJson);
            List<EnemyData> list = new List<EnemyData>();
            for (int i = 0; i < enemyList.Enemies.Length; i++)
            {
                list.Add(enemyList.Enemies[i]);
            }
            return list.ToArray();
        } 
        else
        {
            return null;
        }
    }

    // Can easily change this function to pull random enemy for a specific
    // location: add an integer input, pass to function above
    public EnemyData ReturnRandomEnemy()
    {
        EnemyData[] enemyList = ReturnEnemyArray();
        int min = 0;
        int max = enemyList.Length;
        int rand = Random.Range(min, max);
        return enemyList[rand];
    }

}
