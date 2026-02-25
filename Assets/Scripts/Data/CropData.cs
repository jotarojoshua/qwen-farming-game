using UnityEngine;

namespace FarmManagement.Data
{
    [CreateAssetMenu(fileName = "CropData", menuName = "Farm Data/Crop")]
    public class CropData : ScriptableObject
    {
        [Header("Basic Info")]
        public string id;
        public string displayNameKey;
        public Sprite icon;
        public int unlockLevel = 1;

        [Header("Economics")]
        public int seedCostCoins;
        public int xpRewardOnHarvest;
        public int harvestYieldCount;

        [Header("Growth")]
        public float growthTimeSeconds;
        public bool isPerennial = false;
        public int perennialHarvestIntervalSeconds = 300; // 5 minutes default

        [Header("Visuals")]
        public GameObject[] growthStagePrefabs; // Empty array for non-growing items like trees
    }
}