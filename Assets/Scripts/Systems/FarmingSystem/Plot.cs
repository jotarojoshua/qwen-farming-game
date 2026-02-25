using UnityEngine;

namespace FarmManagement.Systems.Farming
{
    public enum PlotState
    {
        Empty,
        Tilled,
        Planted,
        Growing,
        ReadyForHarvest
    }

    public class Plot : MonoBehaviour
    {
        [Header("References")]
        public Vector3Int coordinate;
        public Renderer groundRenderer;

        [Header("State")]
        public PlotState currentState = PlotState.Empty;
        private CropInstance currentCrop;

        [Header("Visual Materials")]
        public Material emptyMaterial;
        public Material tilledMaterial;
        public Material growingMaterial;
        public Material readyMaterial;

        public void Initialize(Vector3Int coord)
        {
            coordinate = coord;
            UpdateVisual();
        }

        public bool CanPerformAction(PlotState requiredState)
        {
            return currentState == requiredState;
        }

        public bool TransitionToState(PlotState newState)
        {
            // Define valid state transitions
            switch (currentState)
            {
                case PlotState.Empty:
                    if (newState == PlotState.Tilled) break;
                    return false;
                case PlotState.Tilled:
                    if (newState == PlotState.Planted) break;
                    return false;
                case PlotState.Planted:
                    if (newState == PlotState.Growing) break;
                    return false;
                case PlotState.Growing:
                    if (newState == PlotState.ReadyForHarvest) break;
                    return false;
                case PlotState.ReadyForHarvest:
                    if (newState == PlotState.Empty) break; // After harvest
                    return false;
                default:
                    return false;
            }

            currentState = newState;
            UpdateVisual();
            return true;
        }

        private void UpdateVisual()
        {
            if (groundRenderer == null) return;

            switch (currentState)
            {
                case PlotState.Empty:
                    groundRenderer.material = emptyMaterial;
                    break;
                case PlotState.Tilled:
                    groundRenderer.material = tilledMaterial;
                    break;
                case PlotState.Growing:
                    groundRenderer.material = growingMaterial;
                    break;
                case PlotState.ReadyForHarvest:
                    groundRenderer.material = readyMaterial;
                    break;
            }
        }

        public bool PlantCrop(CropData cropData)
        {
            if (currentState != PlotState.Tilled) return false;

            // Check if player can afford the seeds
            if (!Core.GameManager.Instance.CanAfford(cropData.seedCostCoins))
                return false;

            // Spend coins
            Core.GameManager.Instance.SpendCoins(cropData.seedCostCoins);

            // Transition to planted state
            if (TransitionToState(PlotState.Planted))
            {
                // Create crop instance
                Vector3 spawnPos = transform.position + Vector3.up * 0.1f; // Slightly above ground
                GameObject cropObj = Instantiate(cropData.growthStagePrefabs[0], spawnPos, Quaternion.identity, transform);
                
                currentCrop = cropObj.GetComponent<CropInstance>();
                if (currentCrop == null)
                {
                    currentCrop = cropObj.AddComponent<CropInstance>();
                }
                
                currentCrop.Initialize(this, cropData);
                
                // Publish event
                Core.GameEvents.OnCropPlanted?.Invoke(cropData, coordinate);
                
                return true;
            }

            return false;
        }

        public bool HarvestCrop()
        {
            if (currentState != PlotState.ReadyForHarvest || currentCrop == null) return false;

            CropData cropData = currentCrop.cropData;
            
            // Reward player
            Core.GameManager.Instance.AddCoins(cropData.harvestYieldCount * 10); // Simplified value
            Core.GameManager.Instance.AddXP(cropData.xpRewardOnHarvest);

            // Handle perennial crops
            if (cropData.isPerennial)
            {
                // Return to growing state for perennials
                TransitionToState(PlotState.Growing);
                currentCrop.StartGrowthCycle(); // Restart growth cycle
            }
            else
            {
                // Remove crop object and return to empty state for annuals
                Destroy(currentCrop.gameObject);
                currentCrop = null;
                TransitionToState(PlotState.Empty);
            }

            // Publish event
            Core.GameEvents.OnCropHarvested?.Invoke(cropData, coordinate);

            return true;
        }

        public void StartGrowth()
        {
            if (currentState == PlotState.Planted && currentCrop != null)
            {
                TransitionToState(PlotState.Growing);
                currentCrop.StartGrowth();
            }
        }

        public void MarkAsReady()
        {
            TransitionToState(PlotState.ReadyForHarvest);
        }
    }
}