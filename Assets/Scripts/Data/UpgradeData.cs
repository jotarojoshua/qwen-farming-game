using UnityEngine;

namespace FarmManagement.Data
{
    [CreateAssetMenu(fileName = "UpgradeData", menuName = "Farm Data/Upgrade")]
    public class UpgradeData : ScriptableObject
    {
        [Header("Basic Info")]
        public string id;
        public string displayNameKey;
        public Sprite icon;
        public int unlockLevel = 1;

        [Header("Upgrade Target")]
        public string targetBuildingId; // Which building this upgrade applies to

        [Header("Effects")]
        public int newCapacity; // For storage upgrades
        public float productionSpeedModifier; // For factory upgrades
        public int costCoins;
        public int costXP;
    }
}