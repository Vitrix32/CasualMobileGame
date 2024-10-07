using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemManager : MonoBehaviour
{
    public TextAsset items;
    [SerializeField]
    public Inventory inventory;

    [SerializeField]
    NewDict spritelist;
    Dictionary<string, Sprite> spriteList = new Dictionary<string, Sprite>();
    public bool toggleInventory = false, toggleArmor = false, toggleWeapons = false, togglePotions = false;
    public GameObject inventoryPanel, weaponPanel, potionPanel, armorPanel, openInventoryButton;

    private void Start()
    {
        spriteList = spritelist.ToDictionary();
        inventory = JsonUtility.FromJson<Inventory>(items.text);
    }

    public void ToggleInventory()
    {
        toggleInventory = !toggleInventory;
        inventoryPanel.SetActive(toggleInventory);
        openInventoryButton.SetActive(!toggleInventory);
    }

    public void ToggleArmor()
    {
        toggleInventory = !toggleInventory;
        inventoryPanel.SetActive(toggleInventory);
        toggleArmor = !toggleArmor;
        armorPanel.SetActive(toggleArmor);
        if (toggleArmor)
        {
            List<Item> itemArr = new();
            foreach(Item item in inventory.items)
            {
                if (item.type == "Armor")
                {
                    itemArr.Add(item);
                }
            }
            PanelDisplay(armorPanel, itemArr);
        }
    }

    public void ToggleWeapons()
    {
        toggleInventory = !toggleInventory;
        inventoryPanel.SetActive(toggleInventory);
        toggleWeapons = !toggleWeapons;
        weaponPanel.SetActive(toggleWeapons);
        if (toggleWeapons)
        {
            List<Item> itemArr = new();
            foreach (Item item in inventory.items)
            {
                if (item.type == "Weapon")
                {
                    itemArr.Add(item);
                }
            }
            PanelDisplay(weaponPanel, itemArr);
        }
    }
    public void TogglePotions()
    {
        toggleInventory = !toggleInventory;
        inventoryPanel.SetActive(toggleInventory);
        togglePotions = !togglePotions;
        potionPanel.SetActive(togglePotions);
        if (togglePotions)
        {
            List<Item> itemArr = new();
            foreach (Item item in inventory.items)
            {
                if (item.type == "Potion")
                {
                    itemArr.Add(item);
                }
            }
            PanelDisplay(potionPanel, itemArr);
        }
    }

    void PanelDisplay(GameObject panel, List<Item> itemList)
    {
        
        for (int i = panel.transform.childCount - 1; i >= 1; i--)
        {
            Destroy(panel.transform.GetChild(i).gameObject);
        }
        Debug.Log(panel.transform.childCount);
        foreach (Item i in itemList)
        {
            if (spriteList.ContainsKey(i.name))
            {
                //Debug.Log("Adding: " + i.name);
                Sprite sprite = spriteList[i.name];
                GameObject imageObject = new GameObject("Image");
                Image imageComponent = imageObject.AddComponent<Image>();
                imageComponent.sprite = sprite;
                RectTransform rectTransform = imageObject.GetComponent<RectTransform>();
                rectTransform.sizeDelta = new Vector2(50,50);
                rectTransform.SetParent(panel.transform);
                rectTransform.anchoredPosition = new Vector2(-25 + (12.5f * ((panel.transform.childCount -2)  % 5)), 25 - (50 * Mathf.FloorToInt((panel.transform.childCount - 2) / 4)));
            } else
            {
                //Debug.Log("The sprite was not found:" + i.name);
            }
        }
    }

    [Serializable]
    public class Item
    {
        [SerializeField]
        public string name;
        [SerializeField]
        public string type;
        [SerializeField]
        public string effect;
        [SerializeField]
        public string amount;
        [SerializeField]
        public string count;
        [SerializeField]
        public bool equipped;
    }

    [Serializable]
    public class Inventory
    {
        [SerializeField]
        public Item[] items;
    }
}

[Serializable]
public class NewDict 
{
    [SerializeField]
    NewDictItem[] NDI;

    public Dictionary<string, Sprite> ToDictionary()
    {
        Dictionary<string, Sprite> newDict = new Dictionary<string, Sprite>();
        foreach(var item in NDI)
        {
            newDict.Add(item.name, item.sprite);
        }
        return newDict;
    }
}

[Serializable]
public class NewDictItem
{
    [SerializeField]
    public string name;
    [SerializeField]
    public Sprite sprite;
}
