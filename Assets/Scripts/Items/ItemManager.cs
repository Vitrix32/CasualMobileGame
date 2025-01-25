using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class ItemManager : MonoBehaviour
{
    public PlayerStats PS;
    public TextAsset statsJSON;
    public ItemList IL;
    public TextAsset itemsJSON;
    public StatType attack, defense, health;
    public Dictionary<string, int> consumables = new Dictionary<string, int>();

    private string itemsFilePath;

    private void Start()
    {

        itemsFilePath = Path.Combine(Application.dataPath, "Scripts/Items", "Items.txt");

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
    }

    public bool UseConsumable(string itemName)
    {
        if (HasConsumable(itemName))
        {
            consumables[itemName]--;

            // Also update IL so that items.txt can be saved correctly.
            for (int i = 0; i < IL.items.Length; i++)
            {
                if (IL.items[i].name == itemName && IL.items[i].type == "consumable")
                {
                    IL.items[i].quantity = consumables[itemName];
                    break;
                }
            }

            // Write the updated item list to the file
            SaveItemList();

            return true;
        }
        Debug.Log("No consumable left or missing consumable: " + itemName);
        return false;
    }

    private void SaveItemList()
    {
        string newJson = JsonUtility.ToJson(IL, true);  // pretty-print for clarity
        File.WriteAllText(itemsFilePath, newJson);
        Debug.Log("Updated items saved to: " + itemsFilePath);
    }

    public void checkItemAquire(string s)
    {
        foreach (var item in IL.items)
        {
            if (item.questName == s)
            {
                Debug.Log("Aquired Item: " + item.name);
                if (item.type.StartsWith("attack:"))
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
