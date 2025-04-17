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
    
    // Add a second filename for saved health
    private readonly string SAVE_FILENAME = "CurrentHealth.json";
    private readonly string BACKUP_SAVE_FILENAME = "SaveCurrentHealth.json";

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
        // Use persistentDataPath instead of dataPath
        string path = Path.Combine(Application.persistentDataPath, SAVE_FILENAME);
        
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);
            _currentHealth = data.currentHealth;
            Debug.Log($"Loaded health: {_currentHealth} from {path}");
        }
        else
        {
            // If file doesn't exist in persistentDataPath, check Resources for default
            TextAsset defaultHealthJson = Resources.Load<TextAsset>("CurrentHealth");
            if (defaultHealthJson != null)
            {
                SaveData data = JsonUtility.FromJson<SaveData>(defaultHealthJson.text);
                _currentHealth = data.currentHealth;
                Debug.Log($"Loaded default health: {_currentHealth} from Resources");
                
                // Save this to persistentDataPath for future use
                SaveHealth();
            }
            else
            {
                _currentHealth = MAX_HEALTH;
                Debug.Log($"Set default max health: {_currentHealth}");
                SaveHealth();
            }
        }
    }

    private void SaveHealth()
    {
        // Use persistentDataPath instead of dataPath
        string path = Path.Combine(Application.persistentDataPath, SAVE_FILENAME);
        SaveData data = new SaveData { currentHealth = _currentHealth };
        string json = JsonUtility.ToJson(data);
        
        // Ensure the directory exists
        string directory = Path.GetDirectoryName(path);
        if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }
        
        File.WriteAllText(path, json);
        Debug.Log($"Saved health: {_currentHealth} to {path}");
    }

    // Add a method to backup current health
    public void BackupCurrentHealth()
    {
        string livePath = Path.Combine(Application.persistentDataPath, SAVE_FILENAME);
        string savePath = Path.Combine(Application.persistentDataPath, BACKUP_SAVE_FILENAME);
        
        if (File.Exists(livePath))
        {
            File.Copy(livePath, savePath, true);
            Debug.Log($"Backed up health file from {livePath} to {savePath}");
        }
    }
    
    // Add a method to restore from backup
    public void RestoreFromBackup()
    {
        string savePath = Path.Combine(Application.persistentDataPath, BACKUP_SAVE_FILENAME);
        
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            SaveData data = JsonUtility.FromJson<SaveData>(json);
            SetHealth(data.currentHealth);
            Debug.Log($"Restored health from backup: {_currentHealth}");
        }
        else
        {
            Debug.LogWarning("No health backup file found. Health remains unchanged.");
        }
    }
}