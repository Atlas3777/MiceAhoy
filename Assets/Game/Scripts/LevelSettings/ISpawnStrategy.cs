using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game.Scripts.LevelSettings
{
    public interface ISpawnStrategy
    {
        // lastSpawnTimes: словарь guestId -> last spawn time (Tgime.time), now == current Time.time
        GuestProfile TrySelectGuest(float currentBudget, List<GuestProfile> availableGuests, float progress,
            float intensity, Dictionary<int, float> lastSpawnTimes, float now);
    }

    [System.Serializable]
    public class CostAwareWeightedSpawnStrategy : ISpawnStrategy
    {
        [Range(0f, 1f)] public float SaveChance = 0.45f; // шанс "копить" если бюджет меньше MaxCost*MaxCostBias

        public float MaxCostBias = 1.2f; // если бюджет < maxCost * MaxCostBias — допускается копить

        public float RecentSpawnCooldown = 8f; // сек — если гость заспавнен недавно, снижаем его вес
        public float RecentSpawnPenalty = 0.3f; // множитель веса для недавно заспавненного гостя

        public float ExpensiveBiasFactor = 1.2f; // как сильно дорогие гости получают бонус при высокой интенсивности

        public GuestProfile TrySelectGuest(float currentBudget, List<GuestProfile> availableGuests, float progress,
            float intensity, Dictionary<int, float> lastSpawnTimes, float now)
        {
            // afford considering reserve: не тратить весь бюджет ниже резерва
            float maxGuestCostAll = availableGuests.Max(g => (float)g.Cost);
            float
                budgetReserve =
                    maxGuestCostAll *
                    (1 - MaxCostBias); // пример резервирования, но проще - использовать MinBudgetReserveFraction уже в LevelConfig
            // фильтр по реальному affordability
            var affordable = availableGuests.Where(g => g.Cost <= currentBudget).ToList();
            if (affordable.Count == 0) return null;

            // шанс копить (если бюджет меньше чем самый дорогой * MaxCostBias)
            float maxAffordableCost = affordable.Max(g => (float)g.Cost);
            if (currentBudget < maxAffordableCost * MaxCostBias)
            {
                if (Random.value < SaveChance * (1 - intensity))
                    return null;
            }

            // Вычисляем веса с учётом цены и последнего спавна
            float maxCost = affordable.Max(g => (float)g.Cost);
            var weighted = new List<(GuestProfile guest, float weight)>();
            foreach (var g in affordable)
            {
                float w = g.Weight;

                // bias для дорогих гостей при росте intensity/progress
                float priceFactor = (g.Cost / Mathf.Max(1f, maxCost)); // 0..1
                w *= 1f + (priceFactor - 0.5f) * ExpensiveBiasFactor * intensity;

                // penalty если недавно заспавнен
                if (lastSpawnTimes != null && lastSpawnTimes.TryGetValue(g.GetInstanceID(), out var last) &&
                    now - last < RecentSpawnCooldown)
                {
                    w *= RecentSpawnPenalty;
                }

                if (w < 0.001f) w = 0.001f;
                weighted.Add((g, w));
            }

            float total = weighted.Sum(t => t.weight);
            if (total <= 0.001f) return null;

            float roll = Random.Range(0f, total);
            float acc = 0f;
            foreach (var t in weighted)
            {
                acc += t.weight;
                if (roll <= acc) return t.guest;
            }

            return weighted[weighted.Count - 1].guest;
        }
    }
}