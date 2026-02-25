using UnityEngine;

namespace FarmManagement.Data
{
    [CreateAssetMenu(fileName = "AnimalData", menuName = "Farm Data/Animal")]
    public class AnimalData : ScriptableObject
    {
        [Header("Basic Info")]
        public string id;
        public string displayNameKey;
        public Sprite icon;
        public int unlockLevel = 1;

        [Header("Production")]
        public OutputType outputType;
        public float productionTimeSeconds;
        public int outputAmount;
        public string requiredFeedId; // If empty, animal produces automatically

        [Header("Visual")]
        public GameObject animalPrefab;
    }

    public enum OutputType
    {
        Milk,
        Egg,
        Wool,
        Meat,
        Fur,
        None
    }
}