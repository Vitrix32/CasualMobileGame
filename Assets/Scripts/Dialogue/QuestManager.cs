using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public TextAsset quests;
    public QuestList questList;
    public Dictionary<Quest, bool> starts = new Dictionary<Quest, bool>();
    public Dictionary<Quest, bool> ends = new Dictionary<Quest, bool>();
    
    void Start()
    {
        questList = JsonUtility.FromJson<QuestList>(quests.text);
        foreach(Quest q in questList.quests)
        {
            starts[q] = false;
            ends[q] = false;
        }
    }

    public void TryQuest(string s)
    {
        foreach (Quest quest in questList.quests)
        {
            if (quest.start == s && !starts[quest])
            {
                AddQuest(quest);
                starts[quest] = true;
            }
            if (quest.end == s && !ends[quest])
            {
                RemoveQuest(quest);
                ends[quest] = true;
            }
        }
    }

    void AddQuest(Quest q)
    {
        Debug.Log("quest started: " + q.name);
    }

    void RemoveQuest(Quest q)
    {
        Debug.Log("quest ended: " + q.name);
    }
    
    [System.Serializable]
    public class Quest
    {
        public string name;
        public string start;
        public string end;
        public string questInfo;
    }

    [System.Serializable]
    public class QuestList
    {
        public Quest[] quests;
    }
}
