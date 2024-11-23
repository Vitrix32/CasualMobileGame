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
        for (int i = 0; i < PS.stats.Length; i++)
        {
            if (PS.stats[i].type == "attack")
            {
                attack = PS.stats[i];
            } else if (PS.stats[i].type == "defense")
            {
                defense = PS.stats[i];
            } else
            {
                health = PS.stats[i];
            }
        }
        IL = JsonUtility.FromJson<ItemList>(itemsJSON.text);
    }

    public void checkItemAquire(string s)
    {
        for (int i = 0; i < IL.items.Length; i++)
        {
            if (IL.items[i].questName == s)
            {
                Debug.Log("Aquired Item: " + IL.items[i].name);
                if (IL.items[i].type == "attack")
                {
                    attack.itemEnhancement = IL.items[i].value;
                    attack.itemName = IL.items[i].name;
                } else if (IL.items[i].type == "health")
                {
                    health.itemEnhancement = IL.items[i].value;
                    health.itemName = IL.items[i].name;
                } else
                {
                    defense.itemEnhancement = IL.items[i].value;
                    defense.itemName = IL.items[i].name;
                }
            }
        }
    }



    /*
    // MAKE THIS 4 SEPERATE CLASS FILES
    [System.Serializable]
    public class ItemList
    {
        public Item[] items;
    }
    [System.Serializable]
    public class Item
    {
        public string name;
        public string type;
        public int value;
        public string questName;
    }
    [System.Serializable]
    public class PlayerStats
    {
        public StatType[] stats;
    }
    [System.Serializable]
    public class StatType
    {
        public string type;
        public int basic;
        public int itemEnhancement;
        public string itemName;
        public int boostEnhancement;
    }
    */
}
