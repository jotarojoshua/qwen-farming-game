using UnityEngine;
using System.Collections;

namespace FarmManagement.Systems.Farming
{
    public class CropInstance : MonoBehaviour
    {
        [Header("Crop Data")]
        public Data.CropData cropData;
        
        [Header("State")]
        public Plot parentPlot;
        public int currentGrowthStage = 0;
        private Coroutine growthCoroutine;

        public void Initialize(Plot plot, Data.CropData data)
        {
            parentPlot = plot;
            cropData = data;
            currentGrowthStage = 0;
        }

        public void StartGrowth()
        {
            if (growthCoroutine != null)
            {
                StopCoroutine(growthCoroutine);
            }

            growthCoroutine = StartCoroutine(GrowthRoutine());
        }

        public void StartGrowthCycle() // For perennials
        {
            StartGrowth();
        }

        private IEnumerator GrowthRoutine()
        {
            float totalGrowthTime = cropData.growthTimeSeconds / Core.GameManager.Instance.TimeScale;
            float stageDuration = totalGrowthTime / cropData.growthStagePrefabs.Length;

            // Wait for each growth stage
            for (int i = 1; i < cropData.growthStagePrefabs.Length; i++)
            {
                yield return new WaitForSeconds(stageDuration);
                
                // Change to next growth stage visual
                if (cropData.growthStagePrefabs[i] != null)
                {
                    GameObject newStage = Instantiate(cropData.growthStagePrefabs[i], transform.position, transform.rotation, parentPlot.transform);
                    Destroy(transform.GetChild(0).gameObject); // Destroy previous stage
                    
                    // Update reference if needed
                    transform.SetParent(parentPlot.transform);
                }
                
                currentGrowthStage = i;
            }

            // Final stage reached - mark plot as ready for harvest
            parentPlot.MarkAsReady();
        }

        private void OnDestroy()
        {
            if (growthCoroutine != null)
            {
                StopCoroutine(growthCoroutine);
            }
        }
    }
}