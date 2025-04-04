using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CombatStats : MonoBehaviour
{
    //Stat icons
    [SerializeField]
    private GameObject shield;
    [SerializeField]
    private GameObject empower;
    [SerializeField]
    private GameObject weakness;
    [SerializeField]
    private GameObject poison;

    //Stat description objects
    [SerializeField]
    private GameObject statPanel;
    [SerializeField] 
    private GameObject statImage;
    [SerializeField]
    private GameObject statName;
    [SerializeField]
    private GameObject statDescription;

    //Additional stat data
    [SerializeField]
    private Sprite[] statImages;
    [SerializeField] 
    private string[] statNames;
    [SerializeField] 
    private string[] statDescriptions;
    [SerializeField]
    private int activeStatDisplay;

    [SerializeField] private string[] clips;

    public void SetStat(int stat)
    {
        switch (stat) 
        { 
            case 0:
                shield.SetActive(true);
                break;
            case 1: empower.SetActive(true);
                break;
            case 2: weakness.SetActive(true); 
                break;
            case 3: poison.SetActive(true);
                break;
        }
    }

    public void UnsetStat(int stat) 
    { 
        switch (stat) 
        {
            case 0:
                shield.SetActive(false);
                break;
            case 1:
                empower.SetActive(false);
                break;
            case 2:
                weakness.SetActive(false);
                break;
            case 3:
                poison.SetActive(false);
                break;
        } 
    }

    public void StatInvestigated(int val)
    {
        activeStatDisplay = val;
        statPanel.SetActive(true);
        UpdateStatDisplayFields(activeStatDisplay);
    }

    private void UpdateStatDisplayFields(int index)
    {
        statImage.GetComponent<Image>().sprite = statImages[index];
        statName.GetComponent<TMP_Text>().text = statNames[index];
        statDescription.GetComponent<TMP_Text>().text = statDescriptions[index];
    }
}
