using UnityEngine;
using System;

namespace FarmManagement.Core
{
    public static class GameEvents
    {
        // Farming Events
        public static Action<CropData, Vector3Int> OnCropPlanted;
        public static Action<CropData, Vector3Int> OnCropHarvested;

        // Economy Events
        public static Action<int> OnCoinsChanged;
        public static Action<int> OnXPChanged;
        public static Action<int> OnLevelUp;

        // Quest Events
        public static Action<QuestObjective> OnObjectiveProgress;
        public static Action<QuestData> OnQuestCompleted;

        // UI Events
        public static Action<string> OnShowToast;
        public static Action<GameState> OnGameStateChange;

        // Animal Events
        public static Action<AnimalData> OnAnimalProduced;

        // Building Events
        public static Action<BuildingData> OnBuildingConstructed;
    }
}