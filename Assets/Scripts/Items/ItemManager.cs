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

        if (File.Exists(itemsFilePath))
        {
            string fileJson = File.ReadAllText(itemsFilePath);
            IL = JsonUtility.FromJson<ItemList>(fileJson);
        }
        else
        {
            IL = JsonUtility.FromJson<ItemList>(itemsJSON.text);
        }

        PS = JsonUtility.FromJson<PlayerStats>(statsJSON.text);
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
            // Only apply non-quest items or items with "none" as questName
            else if (item.questName == "none")
            {
                ApplyItemBuff(item);
            }
        }
        
        // Load already completed quests' items
        LoadCompletedQuestItems();
    }

    // Helper method to apply an item's buff
    private void ApplyItemBuff(Item item)
    {
        if (item.type.StartsWith("attack:"))
        {
            string attackTypeKey = item.type.Split(':')[1];
            foreach (var attackType in attack.attackTypes)
            {
                if (attackType.type == attackTypeKey)
                {
                    attackType.itemEnhancement = item.value;
                    attackType.itemName = item.name;
                    Debug.Log($"Applied {item.name} to {attackTypeKey} attack (+{item.value})");
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
        
        // Save the changes to the files
        SavePlayerStats();
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

            for (int i = 0; i < IL.items.Length; i++)
            {
                if (IL.items[i].name == itemName && IL.items[i].type == "consumable")
                {
                    IL.items[i].quantity = consumables[itemName];
                    break;
                }
            }

            SaveItemList();
            return true;
        }
        Debug.Log("No consumable left or missing consumable: " + itemName);
        return false;
    }

    private void SaveItemList()
    {
        string newJson = JsonUtility.ToJson(IL, true);
        File.WriteAllText(itemsFilePath, newJson);
        Debug.Log("Updated items saved to: " + itemsFilePath);
    }

    public void checkItemAquire(string questName)
    {
        bool itemAcquired = false;
        foreach (var item in IL.items)
        {
            if (item.questName == questName)
            {
                Debug.Log("Acquired Item: " + item.name);
                ApplyItemBuff(item);
                itemAcquired = true;
                
                // If it's a consumable, you might want to increment the count
                if (item.type == "consumable")
                {
                    if (consumables.ContainsKey(item.name))
                    {
                        consumables[item.name] += item.quantity;
                    }
                    else
                    {
                        consumables[item.name] = item.quantity;
                    }
                    
                    // Update the item in IL to match
                    for (int i = 0; i < IL.items.Length; i++)
                    {
                        if (IL.items[i].name == item.name && IL.items[i].type == "consumable")
                        {
                            IL.items[i].quantity = consumables[item.name];
                            break;
                        }
                    }
                    
                    SaveItemList();
                }
            }
        }
        
        // Save player stats after applying buffs
        if (itemAcquired)
        {
            SavePlayerStats();
        }
    }

    // Add this new method to save player stats
    private void SavePlayerStats()
    {
        string statsPath = Path.Combine(Application.dataPath, "Scripts/Items", "PlayerStats.txt");
        string json = JsonUtility.ToJson(PS, true);
        File.WriteAllText(statsPath, json);
        
        // Also update the save file
        string saveStatsPath = Path.Combine(Application.dataPath, "Scripts/Items", "SavePlayerStats.txt");
        File.WriteAllText(saveStatsPath, json);
        
        Debug.Log("Player stats saved with new item buffs");
    }

    private void LoadCompletedQuestItems()
    {
        // Load quests.txt to check which quests are completed
        string questsPath = Path.Combine(Application.dataPath, "Scripts/Dialogue/Quests.txt");
        if (File.Exists(questsPath))
        {
            string questJson = File.ReadAllText(questsPath);
            QuestList questList = JsonUtility.FromJson<QuestList>(questJson);
            
            foreach (var quest in questList.quests)
            {
                // Check if quest is completed (value > 0)
                if (quest.value > 0)
                {
                    // Apply items for completed quests
                    foreach (var item in IL.items)
                    {
                        if (item.questName == quest.name)
                        {
                            ApplyItemBuff(item);
                        }
                    }
                }
            }
        }
    }
}
