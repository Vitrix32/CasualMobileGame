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
    private bool hasLoadedQuests = false;

    private string questsPath;
    private string saveQuestsPath;

    private void Awake()
    {
        // Set the paths
        questsPath = Path.Combine(Application.persistentDataPath, "Quests.txt");
        saveQuestsPath = Path.Combine(Application.persistentDataPath, "SaveQuests.txt");

        // Check if the files exist, log information for debugging
        Debug.Log($"Quests.txt exists: {File.Exists(questsPath)}");
        Debug.Log($"SaveQuests.txt exists: {File.Exists(saveQuestsPath)}");

        /*// Force load from saved file if it exists
        if (File.Exists(saveQuestsPath))
        {
            try
            {
                string json = File.ReadAllText(saveQuestsPath);
                Debug.Log($"SaveQuests.txt content length: {json.Length}");
                // Load quests from this file
                // ...
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Error loading SaveQuests.txt: {e.Message}");
            }
        }*/
    }

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
        LoadQuestProgress();
        hasLoadedQuests = true;

        Debug.LogError(questList.ToString());
    }

    void OnEnable()
    {
        UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
        if (!hasLoadedQuests)
        {
            LoadQuestProgress();
            hasLoadedQuests = true;
        }
    }

    void OnDisable()
    {
        UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnSceneLoaded;
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
        if (r)
        {
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
        string path = Path.Combine(Application.persistentDataPath, "Quests.txt");
        QuestList currentProgress = new QuestList();
        currentProgress.quests = questList.quests;
        string json = JsonUtility.ToJson(currentProgress, true);
        File.WriteAllText(path, json);
    }

    private void LoadQuestProgress()
    {

        Debug.LogError(File.ReadAllText(questsPath));

        // Always use the live file, not the save file
        // string path = Path.Combine(Application.persistentDataPath, "Quests.txt");
        string path = questsPath;
        if (File.Exists(path))
        {
            Debug.Log("Loading quest progress from: " + path);
            string json = File.ReadAllText(path);

            Debug.LogError(json);

            if (string.IsNullOrEmpty(json))
            {
                Debug.LogError("Loaded Quest JSON is empty!");
                return;
            }

            QuestList savedProgress = JsonUtility.FromJson<QuestList>(json);

            if (savedProgress == null)
            {
                Debug.LogError("Failed to deserialize QuestList!");
                return;
            }

            if (savedProgress.quests == null || savedProgress.quests.Length == 0)
            {
                Debug.LogError("No quests in saved data!");
                return;
            }

            // Clear current quests
            currentQuests.Clear();

            // First, clear any existing quest UI elements
            GameObject scrollContent = questPanel.transform.GetChild(1).GetChild(0).gameObject;
            foreach (Transform child in scrollContent.transform)
            {
                Destroy(child.gameObject);
            }

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
                            if (currentQuest.value == currentQuest.parts.Length)
                            {
                                ends[currentQuest] = true;
                            }
                            else
                            {
                                AddQuest(currentQuest);

                                // Make sure the correct quest step is visible
                                GameObject qp = null;
                                foreach (Transform child in scrollContent.transform)
                                {
                                    if (child.name == currentQuest.name)
                                    {
                                        qp = child.gameObject;
                                        break;
                                    }
                                }

                                if (qp != null)
                                {
                                    GameObject qpPanel = qp.transform.GetChild(0).GetChild(0).gameObject;

                                    // Hide all steps except the current one
                                    for (int i = 0; i < currentQuest.parts.Length; i++)
                                    {
                                        // Account for name and description (first 2 children)
                                        if (i + 2 < qpPanel.transform.childCount)
                                        {
                                            qpPanel.transform.GetChild(i + 2).gameObject.SetActive(i == currentQuest.value);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        else
        {
            Debug.LogWarning("No saved quest progress found at: " + path);
        }
    }

    public void ReloadQuests()
    {
        LoadQuestProgress();
        hasLoadedQuests = true;
    }

    void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, UnityEngine.SceneManagement.LoadSceneMode mode)
    {
        if (scene.name == "GameplayScene")
        {
            ReloadQuests();
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
