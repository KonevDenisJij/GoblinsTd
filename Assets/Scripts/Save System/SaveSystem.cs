using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
public static class SaveSystem
{
    public static void SaveWorldMap(WorldMapData data)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(GetMapPath(),FileMode.Create);

        formatter.Serialize(stream, data);
        stream.Close();
    }
    public static WorldMapData LoadWorldMap()
    {
        if (File.Exists(GetMapPath()))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(GetMapPath(), FileMode.Open);

            WorldMapData data = formatter.Deserialize(stream) as WorldMapData;
            stream.Close();

            return data;
        }
        Debug.Log("‘¿…À¿ Õ≈“!!!");
        return null;
    }

    public static void SaveVillage(VillageData data)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(GetVillagePath(), FileMode.Create);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static VillageData LoadVillage()
    {
        if (File.Exists(GetVillagePath()))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(GetVillagePath(), FileMode.Open);

            VillageData data = formatter.Deserialize(stream) as VillageData;
            stream.Close();

            return data;
        }
        Debug.Log("‘¿…À¿ Õ≈“!!!");
        return null;
    }

    public static void StartNewGame()
    {
        File.Delete(GetVillagePath());
        File.Delete(GetMapPath());
    }

    public static bool CheckSaveFile()
    {
        return File.Exists(GetMapPath());
    }

    private static string GetMapPath()
    {
        return Application.persistentDataPath + "/currentMap.gtd";
    }

    private static string GetVillagePath()
    {
        return Application.persistentDataPath + "/currentVillage.gtd";
    }
}
