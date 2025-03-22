using UnityEngine;
using System.IO;

public class HealthManager : MonoBehaviour
{
    private static HealthManager _instance;
    public static HealthManager Instance
    {
        get {
            if (_instance == null)
            {
                _instance = FindObjectOfType<HealthManager>();
                if (_instance == null)
                {
                    GameObject go = new GameObject("HealthManager");
                    _instance = go.AddComponent<HealthManager>();
                    DontDestroyOnLoad(go);
                }
            }
            return _instance;
        }
    }

    public const float MAX_HEALTH = 50f;
    private float _currentHealth;
    private readonly string SAVE_PATH = "/Scripts/Managers/CurrentHealth.json";

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }
        _instance = this;
        DontDestroyOnLoad(gameObject);
        
        LoadHealth();
    }

    public float GetHealth() => _currentHealth;
    
    public void SetHealth(float value)
    {
        _currentHealth = Mathf.Clamp(value, 0, MAX_HEALTH);
        SaveHealth();
    }

    [System.Serializable]
    private class SaveData
    {
        public float currentHealth;
    }

    private void LoadHealth()
    {
        string path = Application.dataPath + SAVE_PATH;
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);
            _currentHealth = data.currentHealth;
        }
        else
        {
            _currentHealth = MAX_HEALTH;
        }
    }

    private void SaveHealth()
    {
        string path = Application.dataPath + SAVE_PATH;
        SaveData data = new SaveData { currentHealth = _currentHealth };
        string json = JsonUtility.ToJson(data);
        File.WriteAllText(path, json);
    }
}