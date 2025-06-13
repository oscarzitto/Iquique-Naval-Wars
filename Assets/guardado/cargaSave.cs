using System.IO;
using UnityEngine;

public static class SaveSystem
{
    private static string path => Application.persistentDataPath + "/savegame.json";

    public static void Save(GameData data)
    {
        string json = JsonUtility.ToJson(data, prettyPrint: true);
        File.WriteAllText(path, json);
        Debug.Log("Partida guardada en: " + path);
    }

    public static GameData Load()
    {
        if (!File.Exists(path))
        {
            Debug.LogWarning("No existe archivo de guardado, se creará uno nuevo.");
            return new GameData();
        }

        string json = File.ReadAllText(path);
        return JsonUtility.FromJson<GameData>(json);
    }
}
