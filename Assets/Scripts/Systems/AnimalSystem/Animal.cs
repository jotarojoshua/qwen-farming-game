using UnityEngine;

namespace FarmManagement.Systems.AnimalSystem
{
    public enum ProductionMode { Automatic, Manual }

    public class Animal : MonoBehaviour
    {
        [Header("Animal Data")]
        public Data.AnimalData data;
        public ProductionMode mode;

        [Header("State")]
        private float productionTimer;
        private bool isFed;
        private bool hasProduct;

        [Header("Production")]
        private float productionInterval;
        
        void Start()
        {
            productionInterval = data.productionTimeSeconds;
            productionTimer = 0f;
        }

        void Update()
        {
            if (mode == ProductionMode.Automatic)
            {
                productionTimer += Time.deltaTime * Core.GameManager.Instance.TimeScale;
                if (productionTimer >= productionInterval && !hasProduct)
                {
                    Produce();
                    productionTimer = 0f;
                }
            }
        }

        public void Feed(Data.CropData food)
        {
            if (string.IsNullOrEmpty(data.requiredFeedId) || food.id == data.requiredFeedId)
            {
                isFed = true;
                productionTimer = 0f; // Start production
                
                // For manual mode, start production immediately
                if (mode == ProductionMode.Manual)
                {
                    Invoke(nameof(Produce), 0.5f); // Small delay for animation
                }
            }
        }

        private void Produce()
        {
            hasProduct = true;
            
            // Publish event for UI updates
            Core.GameEvents.OnAnimalProduced?.Invoke(data);
        }

        public bool CollectProduct()
        {
            if (hasProduct)
            {
                // Give player the product
                Core.GameManager.Instance.AddCoins(data.outputAmount * 5); // Simplified value
                
                // Reset production state
                hasProduct = false;
                
                if (mode == ProductionMode.Automatic)
                {
                    productionTimer = 0f; // Start next production cycle
                }
                
                return true;
            }
            
            return false;
        }

        public bool HasProductReady()
        {
            return hasProduct;
        }

        public float GetProductionProgress()
        {
            return Mathf.Clamp01(productionTimer / productionInterval);
        }
    }
}