using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

/*
 * Name: Fade
 * Author: Isaac Drury
 * Date: 9/22/24
 * Description:
 * This script was created for the purpose of allowing sprites and images to 
 * change their alpha value (transparency) over time (fade in/ fade out).
 * It contains a function, startFade, that can be called by other scripts to
 * pass on the parameters for and start the coroutines responsible for the 
 * change in transparency. There are two coroutines: one to support functionality
 * for sprites and another for images.
 */
public class Fade : MonoBehaviour
{
    [SerializeField]
    private bool isSprite;
    private bool first = true;
    public bool fadeFromCombat = false;

    public void startFade(float endVal, float duration)
    {
        SetObjectsToLiveJSON();
        if (isSprite)
        {
            StartCoroutine(fadeSprite(endVal, duration));
        }
        else
        {
            StartCoroutine(fadeImage(endVal, duration));
        }
    }

    //Changes alpha value to make sprites fade in/out
    IEnumerator fadeSprite(float endVal, float duration)
    {
        float counter = 0;
        Color spriteColor = this.GetComponent<SpriteRenderer>().material.color;
        while (counter < duration)
        {
            counter += Time.deltaTime;
            float alpha = Mathf.Lerp(spriteColor.a, endVal, counter / duration);
            this.GetComponent<SpriteRenderer>().color = new Color(spriteColor.r, spriteColor.g, spriteColor.b, alpha);
            yield return null;
        }
    }

    //Changes alpha value to make images fade in/out
    IEnumerator fadeImage(float endVal, float duration)
    {
        float counter = 0;
        Color imageColor = this.GetComponent<Image>().color;
        while (counter < duration)
        {
            counter += Time.deltaTime;
            float alpha = Mathf.Lerp(imageColor.a, endVal, counter / duration);
            this.GetComponent<Image>().color = new Color(imageColor.r, imageColor.g, imageColor.b, alpha);
            yield return null;
        }
    }


    public void SetObjectsToLiveJSON()
    {
        if (first) {
            first = false;
            return;
        }
        if (fadeFromCombat)
        {
            fadeFromCombat = false;
            return;
        }
        // QUESTS
        if (File.Exists(Application.dataPath + "/Scripts/Dialogue/SaveQuests.txt"))
        {


            QuestList tempList = FindObjectOfType<QuestManager>().questList;

            string tempJson = File.ReadAllText(Application.dataPath + "/Scripts/Dialogue/SaveQuests.txt");
            QuestList questList = JsonUtility.FromJson<QuestList>(tempJson);
            List<Quest> list = new List<Quest>();

            for (int i = 0; i < questList.quests.Length; i++)
            {
                for (int j = 0; j < tempList.quests.Length; j++)
                {
                    if (tempList.quests[j].name == questList.quests[i].name)
                    {
                        questList.quests[i].value = tempList.quests[j].value;
                        break;
                    }
                }
                list.Add(questList.quests[i]);
            }
            questList.quests = list.ToArray();
            string json = JsonUtility.ToJson(questList, true);
            // Reset the save and the live json files
            File.WriteAllText(Application.dataPath + "/Scripts/Dialogue/Quests.txt", json);
        }

        // DIALOGUE
        if (File.Exists(Application.dataPath + "/Scripts/Dialogue/SaveDialogue.txt"))
        {
            string tempJson = File.ReadAllText(Application.dataPath + "/Scripts/Dialogue/Dialogue.txt");
            NPCCollection npcList = JsonUtility.FromJson<NPCCollection>(tempJson);
            List<NPC> list = new List<NPC>();
            for (int i = 0; i < npcList.npc_characters.Length; i++)
            {
                NPC tempNPC = FindObjectOfType<DialogueManager>().FindNPCByName(npcList.npc_characters[i].name);
                if (tempNPC != null)
                {
                    Debug.Log("ERROR: FIND NPC BY NAME RETURNED NULL - PAUSE MENU SCRIPT");
                }
                npcList.npc_characters[i].value = tempNPC.value;
                list.Add(npcList.npc_characters[i]);
            }
            npcList.npc_characters = list.ToArray();
            string json = JsonUtility.ToJson(npcList, true);
            File.WriteAllText(Application.dataPath + "/Scripts/Dialogue/Dialogue.txt", json);
        }
    }
}
