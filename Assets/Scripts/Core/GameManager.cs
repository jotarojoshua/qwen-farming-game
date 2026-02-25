using UnityEngine;

namespace FarmManagement.Core
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        [Header("Game State")]
        public int Coins { get; private set; } = 100;
        public int XP { get; private set; } = 0;
        public int Level { get; private set; } = 1;
        public float TimeScale { get; set; } = 1.0f;

        [Header("Balancing")]
        public float XPPerLevelMultiplier = 1.5f;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void AddCoins(int amount)
        {
            Coins += amount;
            GameEvents.OnCoinsChanged?.Invoke(Coins);
        }

        public void SpendCoins(int amount)
        {
            if (Coins >= amount)
            {
                Coins -= amount;
                GameEvents.OnCoinsChanged?.Invoke(Coins);
            }
        }

        public void AddXP(int amount)
        {
            XP += amount;
            CheckLevelUp();
        }

        private void CheckLevelUp()
        {
            int newLevel = CalculateLevelFromXP(XP);
            if (newLevel > Level)
            {
                Level = newLevel;
                GameEvents.OnLevelUp?.Invoke(Level);
            }
        }

        private int CalculateLevelFromXP(int xp)
        {
            // Simple formula: XP required for each level increases exponentially
            float level = 1 + Mathf.Log(xp / 100f, XPPerLevelMultiplier);
            return Mathf.Max(1, Mathf.FloorToInt(level));
        }

        public bool CanAfford(int cost)
        {
            return Coins >= cost;
        }
    }
}