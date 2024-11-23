using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public TMPro.TextMeshProUGUI sampleQuestText;
    public GameObject questPrefab;
    public GameObject questPanel;
    private GameObject player;
    public bool questActive = false;
    public TextAsset quests;
    public QuestList questList;
    public List<Quest> currentQuests = new List<Quest>();
    public Dictionary<Quest, bool> starts = new Dictionary<Quest, bool>();
    public Dictionary<Quest, bool> ends = new Dictionary<Quest, bool>();
    private ItemManager IM;
    
    void Start()
    {
        player = GameObject.Find("WorldPlayer");
        IM = this.GetComponent<ItemManager>();
        questList = JsonUtility.FromJson<QuestList>(quests.text);
        foreach(Quest q in questList.quests)
        {
            starts[q] = false;
            ends[q] = false;
        }
    }

    public void OpenQuests()
    {
        questActive = !questActive;
        questPanel.SetActive(questActive);
        if(questActive)
        {
            player.GetComponent<PlayerStatus>().BeginDialogue();
        }
        else
        {
            player.GetComponent<PlayerStatus>().EndDialogue();
        }
        
    }

    public void TryQuest(string s)
    {
        foreach (Quest quest in questList.quests)
        {
            if (quest.parts[0].increment == s && !starts[quest])
            {
                quest.value = 1;
                AddQuest(quest);
                starts[quest] = true;
            }
            if (quest.parts[quest.parts.Length - 1].increment == s && !ends[quest])
            {
                RemoveQuest(quest);
                ends[quest] = true;
            }
        }
    }

    void AddQuest(Quest q)
    {
        if (!currentQuests.Contains(q))
        {
            currentQuests.Add(q);
            GameObject qp = Instantiate(questPrefab);
            qp.name = q.name;
            qp.transform.SetParent(questPanel.transform.GetChild(1).GetChild(0));
            qp.transform.localScale = Vector3.one;
            GameObject qpPanel = qp.transform.GetChild(0).gameObject;
            
            TMPro.TextMeshProUGUI name = Instantiate(sampleQuestText);
            TMPro.TextMeshProUGUI desc = Instantiate(sampleQuestText);
            name.text = q.name;
            desc.text = q.questInfo;
            name.transform.SetParent(qpPanel.transform);
            desc.transform.SetParent(qpPanel.transform);
            name.transform.localScale = Vector3.one;
            desc.transform.localScale = Vector3.one;
            
            for (int i = 0; i < q.parts.Length; i++)
            {
                TMPro.TextMeshProUGUI textMesh = Instantiate(sampleQuestText);
                textMesh.text = "  " + q.parts[i].description;
                textMesh.transform.SetParent(qpPanel.transform);
                textMesh.transform.localScale = Vector3.one;
            }
            
        }
    }

    void RemoveQuest(Quest q)
    {
        if (currentQuests.Contains(q))
        {
            for (int i = 0; i < questPanel.transform.childCount; i++)
            {
                if (questPanel.transform.GetChild(1).GetChild(0).GetChild(i).name == q.name)
                {
                    Destroy(questPanel.transform.GetChild(1).GetChild(0).GetChild(i).gameObject);
                    break;
                }
            }
            currentQuests.Remove(q);
            Debug.Log("quest ended: " + q.name);
            IM.checkItemAquire(q.name);
        }
    }


    /*
    // These should all 3 be seperate files
    [System.Serializable]
    public class QuestPart
    {
        public string description;
        public string increment;
    }

    [System.Serializable]
    public class Quest
    {
        public string name;
        public string questInfo;
        public int value;
        public QuestPart[] parts;
    }

    [System.Serializable]
    public class QuestList
    {
        public Quest[] quests;
    }
    */
}
