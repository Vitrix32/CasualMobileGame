using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

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
        foreach (Quest q in questList.quests)
        {
            starts[q] = false;
            ends[q] = false;
        }
    }

    void OnEnable()
    {
        LoadQuestProgress();
    }

    public void OpenQuests()
    {
        questActive = !questActive;
        questPanel.SetActive(questActive);
        if (questActive)
        {
            player.GetComponent<PlayerStatus>().BeginDialogue();
        }
        else
        {
            player.GetComponent<PlayerStatus>().EndDialogue();
        }

    }

    public bool TryQuest(string s)
    {
        bool r = false;
        //Check to start the quests
        foreach (Quest quest in questList.quests)
        {
            //First part
            if (quest.parts[0].increment == s && !starts[quest])
            {
                r = true;
                quest.value = 1;
                AddQuest(quest);
                starts[quest] = true;
                SaveQuestProgress(); // Save when quest starts
            }
            //Last part
            if (quest.parts[quest.parts.Length - 1].increment == s && !ends[quest])
            {
                r = true;
                RemoveQuest(quest);
                ends[quest] = true;
                SaveQuestProgress(); // Save when quest ends
            }
        }
        //Increment the quest and set the next part of it to active
        GameObject scroll = questPanel.transform.GetChild(1).gameObject;
        for (int i = 0; i < scroll.transform.childCount; i++)
        {
            Transform currChild = scroll.transform.GetChild(i);
            foreach (Quest q in currentQuests)
            {
                Debug.Log(q.parts[q.value].increment + " " + s);
                if (q.parts[q.value].increment == s)
                {
                    
                    r = true;
                    q.value++;
                    GameObject qpPanel = currChild.GetChild(0).GetChild(0).gameObject;
                    if (qpPanel.transform.childCount >= q.value + 1)
                    {
                        qpPanel.transform.GetChild(q.value + 1).gameObject.SetActive(true);
                        qpPanel.transform.GetChild(q.value).gameObject.SetActive(false);
                    }
                }
            }
        }
        if (r) {
            SaveQuestProgress(); // Save when quest progresses
        }
        return r;
    }

    void AddQuest(Quest q)
    {
        if (!currentQuests.Contains(q))
        {
            currentQuests.Add(q);
            q.value = 1;
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
                if (i > 0)
                {
                    textMesh.gameObject.SetActive(false);
                }
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

    private void SaveQuestProgress()
    {
        string path = Application.dataPath + "/Scripts/Dialogue/Quests.txt";
        QuestList currentProgress = new QuestList();
        currentProgress.quests = questList.quests;
        string json = JsonUtility.ToJson(currentProgress, true);
        File.WriteAllText(path, json);
    }

    private void LoadQuestProgress()
    {
        string path = Application.dataPath + "/Scripts/Dialogue/Quests.txt";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            QuestList savedProgress = JsonUtility.FromJson<QuestList>(json);
            
            // Restore quest progress
            foreach (Quest savedQuest in savedProgress.quests)
            {
                foreach (Quest currentQuest in questList.quests)
                {
                    if (savedQuest.name == currentQuest.name)
                    {
                        currentQuest.value = savedQuest.value;
                        if (currentQuest.value > 0)
                        {
                            starts[currentQuest] = true;
                            AddQuest(currentQuest);
                        }
                    }
                }
            }
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
