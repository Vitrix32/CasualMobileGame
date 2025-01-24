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
    public Dictionary<string, int> consumables = new Dictionary<string, int>();


    private void Start()
{
    PS = JsonUtility.FromJson<PlayerStats>(statsJSON.text);
    IL = JsonUtility.FromJson<ItemList>(itemsJSON.text);
    consumables.Clear();

    foreach (var stat in PS.stats)
    {
        if (stat.type == "attack")
        {
            attack = stat;
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

    foreach (var item in IL.items)
    {
        if (item.type == "consumable")
        {
            consumables[item.name] = item.quantity;
        }
        else if (item.type.StartsWith("attack:"))
        {
            string attackTypeKey = item.type.Split(':')[1];
            foreach (var attackType in attack.attackTypes)
            {
                if (attackType.type == attackTypeKey)
                {
                    attackType.itemEnhancement = item.value;
                    attackType.itemName = item.name;
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

    public bool HasConsumable(string itemName)
    {
        return consumables.ContainsKey(itemName) && consumables[itemName] > 0;
         Debug.Log("HERE");
        Debug.Log(consumables[itemName]);
    }

    public bool UseConsumable(string itemName)
    {
        if (HasConsumable(itemName))
        {
            consumables[itemName]--;
            return true;
        }
        Debug.Log("USED");
        Debug.Log(consumables[itemName]);
        return false;
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
