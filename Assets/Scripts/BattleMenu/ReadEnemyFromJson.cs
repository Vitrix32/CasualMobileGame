using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class ReadEnemyFromJson : MonoBehaviour
{

    public TextAsset enemyJson; //and attach the Enemy Data File.json stuff

    // To use for a specific location: integer input + check in for loop for
    // if location ID matches ID passed in
    private EnemyData[] ReturnEnemyArray()
    {
        EnemyList enemyList = new EnemyList();
        Debug.Log("text file read in: "+enemyJson.text);
        //string tempJson = File.ReadAllText("EnemyDataFile.json");
        enemyList = JsonUtility.FromJson<EnemyList>(enemyJson.text);
        List<EnemyData> list = new List<EnemyData>();
        for (int i = 0; i < enemyList.Enemies.Length; i++)
        {
            list.Add(enemyList.Enemies[i]);        
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
        int rand = Random.Range(min, max);
        return enemyList[rand];
    }

}
