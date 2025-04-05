using System.IO;
using UnityEngine;

public class DataInitializer : MonoBehaviour
{
    private void Awake()
    {
        Debug.Log("DataInitializer Awake - Starting data initialization");
        InitializeAllData();
    }

    private void InitializeAllData()
    {
        // Initialize enemy data
        CopyDefaultIfNotExists("EnemyDataFile.json");
        
        // Initialize dialogue data
        CopyDefaultIfNotExists("Dialogue.txt");
        CopyDefaultIfNotExists("SaveDialogue.txt");
        
        // Initialize quest data
        CopyDefaultIfNotExists("Quests.txt");
        CopyDefaultIfNotExists("SaveQuests.txt");
        
        // Initialize player stats
        CopyDefaultIfNotExists("PlayerStats.txt");
        CopyDefaultIfNotExists("SavePlayerStats.txt");
        
        // Initialize items
        CopyDefaultIfNotExists("Items.txt");

        // Initialize health
        CopyDefaultIfNotExists("CurrentHealth.json");
        
        // Log persistentDataPath contents for debugging
        // LogDirectoryContents(Application.persistentDataPath);
        // LogResourcesContents();
    }

    public void ForceRefresh()
    {
        // Initialize enemy data
        CopyDefaultIfNotExists("EnemyDataFile.json", true);
        
        // Initialize dialogue data
        CopyDefaultIfNotExists("Dialogue.txt", true);
        CopyDefaultIfNotExists("SaveDialogue.txt", true);
        
        // Initialize quest data
        CopyDefaultIfNotExists("Quests.txt", true);
        CopyDefaultIfNotExists("SaveQuests.txt", true);
        
        // Initialize player stats
        CopyDefaultIfNotExists("PlayerStats.txt", true);
        CopyDefaultIfNotExists("SavePlayerStats.txt", true);
        
        // Initialize items
        CopyDefaultIfNotExists("Items.txt", true);

        // Initialize health
        CopyDefaultIfNotExists("CurrentHealth.json", true);
        
        // Log persistentDataPath contents for debugging
        // LogDirectoryContents(Application.persistentDataPath);
        // LogResourcesContents();
    }
    
    private void CopyDefaultIfNotExists(string fileName, bool forceCopy = false)
    {
        string destPath = Path.Combine(Application.persistentDataPath, fileName);
        string fileNameWithoutExt = Path.GetFileNameWithoutExtension(fileName);
        
        Debug.Log($"Checking for {fileName} at {destPath}");
        
        if (!File.Exists(destPath) || forceCopy)
        {
            Debug.Log($"File doesn't exist or force copy requested: {destPath}");
            
            // Only try loading directly from Resources without extension (Unity's standard)
            TextAsset defaultAsset = Resources.Load<TextAsset>(fileNameWithoutExt);
            Debug.Log($"Loading '{fileNameWithoutExt}' from Resources - Result: {(defaultAsset != null ? "Found" : "Not found")}");
            
            if (defaultAsset != null)
            {
                // Ensure directory exists
                string directory = Path.GetDirectoryName(destPath);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }
                
                File.WriteAllText(destPath, defaultAsset.text);
                Debug.Log($"Successfully initialized {fileName} at: {destPath}");
            }
            else
            {
                Debug.LogWarning($"Could not find {fileNameWithoutExt} in Resources folder.");
                // Create empty default files with proper structure
                CreateEmptyDefaultFile(fileName, destPath);
            }
        }
        else
        {
            Debug.Log($"File already exists: {destPath}");
        }
    }
    
    private void CreateEmptyDefaultFile(string fileName, string destPath)
    {
        Debug.Log($"Creating empty default file for: {fileName}");
        
        // Create appropriate empty files based on expected structure
        string content = "{}"; // Default empty JSON
        
        if (fileName.EndsWith("Quests.txt"))
        {
            content = "{\"quests\":[]}";
            Debug.Log($"Creating empty quests file with content: {content}");
        }
        else if (fileName.EndsWith("Dialogue.txt"))
        {
            content = "{\"npc_characters\":[]}";
            Debug.Log($"Creating empty dialogue file with content: {content}");
        }
        else if (fileName.EndsWith("PlayerStats.txt"))
        {
            content = "{\"currentLevel\":1,\"currentXP\":0,\"maxXP\":100}";
            Debug.Log($"Creating empty player stats file with content: {content}");
        }
        else if (fileName.EndsWith("Items.txt"))
        {
            content = "{\"items\":[]}";
            Debug.Log($"Creating empty items file with content: {content}");
        }
        else if (fileName.EndsWith("CurrentHealth.json"))
        {
            content = "{\"currentHealth\":50.0}";
            Debug.Log($"Creating empty health file with content: {content}");
        }
        
        File.WriteAllText(destPath, content);
        Debug.Log($"Created empty default file at: {destPath}");
    }
    
    private void LogDirectoryContents(string directory)
    {
        Debug.Log($"Contents of {directory}:");
        
        if (!Directory.Exists(directory))
        {
            Debug.LogWarning($"Directory does not exist: {directory}");
            return;
        }
        
        string[] files = Directory.GetFiles(directory);
        foreach (string file in files)
        {
            Debug.Log($"  - {Path.GetFileName(file)}");
        }
    }
    
    private void LogResourcesContents()
    {
        Debug.Log("Checking all TextAssets in Resources:");
        Object[] allTextAssets = Resources.LoadAll<TextAsset>("");
        foreach (TextAsset asset in allTextAssets)
        {
            Debug.Log($"  - Found TextAsset: {asset.name}");
        }
    }
}