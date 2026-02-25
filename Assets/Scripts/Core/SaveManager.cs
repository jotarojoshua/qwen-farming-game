using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace FarmManagement.Core
{
    [System.Serializable]
    public class GameSaveData
    {
        public PlayerData player;
        public FarmData farm;
        public QuestData quest;
        public SettingsData settings;
    }

    [System.Serializable]
    public class PlayerData
    {
        public int coins;
        public int xp;
        public int level;
    }

    [System.Serializable]
    public class FarmData
    {
        public PlotData[] plots;
        public AnimalData[] animals;
        public string[] buildings;
        public StorageData storage;
    }

    [System.Serializable]
    public class PlotData
    {
        public Vector3Int coord;
        public string state;
        public string cropId;
        public long plantedTime;
    }

    [System.Serializable]
    public class AnimalData
    {
        public string enclosureId;
        public string animalId;
        public float productionProgress;
    }

    [System.Serializable]
    public class StorageData
    {
        public int barnUsed;
        public int barnCapacity;
        public int warehouseUsed;
        public int warehouseCapacity;
    }

    [System.Serializable]
    public class QuestData
    {
        public string[] completed;
        public string[] inProgress;
    }

    [System.Serializable]
    public class SettingsData
    {
        public string language;
        public float soundVolume;
    }

    public class SaveManager : MonoBehaviour
    {
        public static SaveManager Instance { get; private set; }

        private string savePath;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
                savePath = Path.Combine(Application.persistentDataPath, "savegame.dat");
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void SaveGame(GameSaveData data)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            using (FileStream stream = new FileStream(savePath, FileMode.Create))
            {
                formatter.Serialize(stream, data);
            }
        }

        public GameSaveData LoadGame()
        {
            if (!File.Exists(savePath)) return null;

            BinaryFormatter formatter = new BinaryFormatter();
            using (FileStream stream = new FileStream(savePath, FileMode.Open))
            {
                return (GameSaveData)formatter.Deserialize(stream);
            }
        }

        public bool HasSaveData()
        {
            return File.Exists(savePath);
        }

        public void DeleteSaveData()
        {
            if (File.Exists(savePath))
            {
                File.Delete(savePath);
            }
        }
    }
}