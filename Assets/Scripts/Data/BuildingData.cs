using UnityEngine;

namespace FarmManagement.Data
{
    [CreateAssetMenu(fileName = "BuildingData", menuName = "Farm Data/Building")]
    public class BuildingData : ScriptableObject
    {
        [Header("Basic Info")]
        public string id;
        public string displayNameKey;
        public Sprite icon;
        public int unlockLevel = 1;

        [Header("Economics")]
        public int buildCostCoins;
        public int xpRewardOnBuild;

        [Header("Functionality")]
        public BuildingType type;
        public int capacity; // For storage buildings
        public bool isFactory; // For production buildings

        [Header("Visual")]
        public GameObject buildingPrefab;
    }

    public enum BuildingType
    {
        Storage,
        Factory,
        Decoration,
        Utility
    }
}