using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public PlayerStats PS;
    public TextAsset statsJSON;
    public ItemList IL;
    public TextAsset itemsJSON;
    public StatType attack, defense, health;

    private void Start()
    {
        PS = JsonUtility.FromJson<PlayerStats>(statsJSON.text);
        foreach (var stat in PS.stats)
        {
            if (stat.type == "attack")
            {
                attack = stat;
                foreach (var attackType in attack.attackTypes)
                {
                    Debug.Log($"Attack Type: {attackType.type}, Basic: {attackType.basic}");
                }
            }
            else if (stat.type == "defense")
            {
                defense = stat;
            }
            else if (stat.type == "health")
            {
                health = stat;
            }
        }
        IL = JsonUtility.FromJson<ItemList>(itemsJSON.text);
    }

    public void checkItemAquire(string s)
    {
        foreach (var item in IL.items)
        {
            if (item.questName == s)
            {
                Debug.Log("Aquired Item: " + item.name);
                if (item.type.StartsWith("attack:")) // e.g., attack:melee
                {
                    string attackTypeKey = item.type.Split(':')[1];
                    foreach (var attackType in attack.attackTypes)
                    {
                        if (attackType.type == attackTypeKey)
                        {
                            attackType.itemEnhancement = item.value;
                            attackType.itemName = item.name;
                            Debug.Log($"Enhanced {attackTypeKey} attack with {item.name}");
                        }
                    }
                }
                else if (item.type == "health")
                {
                    health.itemEnhancement = item.value;
                    health.itemName = item.name;
                }
                else if (item.type == "defense")
                {
                    defense.itemEnhancement = item.value;
                    defense.itemName = item.name;
                }
            }
        }
    }
}
