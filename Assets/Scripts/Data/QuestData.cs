using UnityEngine;

namespace FarmManagement.Data
{
    [CreateAssetMenu(fileName = "QuestData", menuName = "Farm Data/Quest")]
    public class QuestData : ScriptableObject
    {
        [Header("Basic Info")]
        public string id;
        public string titleKey;
        public string descriptionKey;
        public bool isTutorial = false;

        [Header("Objectives")]
        public QuestObjective[] objectives;

        [Header("Rewards")]
        public Reward[] rewards;

        [Header("Prerequisites")]
        public string prerequisiteQuestId; // Empty if no prerequisite
    }

    [System.Serializable]
    public class QuestObjective
    {
        public ObjectiveType type;
        public string targetId; // CropId, BuildingId, etc.
        public int targetCount;
        [System.NonSerialized] public int currentCount; // Not saved, calculated during gameplay
    }

    public enum ObjectiveType
    {
        HarvestCrop,
        CollectItem,
        BuildBuilding,
        ReachLevel,
        SpendCoins,
        CompleteQuest
    }

    [System.Serializable]
    public class Reward
    {
        public RewardType type;
        public int amount;
        public CropData itemData; // For item rewards
        public string unlockId; // For building unlocks
    }

    public enum RewardType
    {
        Coins,
        XP,
        Item,
        UnlockBuilding
    }
}