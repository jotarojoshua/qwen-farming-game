using UnityEngine;

namespace FarmManagement.Data
{
    [CreateAssetMenu(fileName = "RecipeData", menuName = "Farm Data/Recipe")]
    public class RecipeData : ScriptableObject
    {
        [Header("Basic Info")]
        public string id;
        public string displayNameKey;
        public Sprite icon;

        [Header("Requirements")]
        public BuildingData requiredBuilding;
        public ItemRequirement[] inputs;
        public ItemReward[] outputs;
        public float craftTimeSeconds = 0f; // 0 for instant
        public int unlockLevel = 1;
    }

    [System.Serializable]
    public class ItemRequirement
    {
        public CropData itemData;
        public int count;
    }

    [System.Serializable]
    public class ItemReward
    {
        public CropData itemData;
        public int count;
    }
}