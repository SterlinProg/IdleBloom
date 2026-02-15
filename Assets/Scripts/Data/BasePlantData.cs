using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Data
{
    [CreateAssetMenu(fileName = "PlantData", menuName = "Data/ Plant")]
    public class BasePlantData:ScriptableObject
    {
        [Serializable]
        public struct PlantState
        {
            public Sprite currentGrowthSprite;
            public float timeToGrow;
            public int baseYield;
            public int collectYield;
            public WaterBehaviour waterBehaviour;
        }
        
        [Serializable]
        public struct WaterBehaviour
        {
            [Range(0,1)]
            public float growthReductionRate;
            [Range(0,100)]
            public float needChancePerTick;
            
        }
        
        public string plantName;
        public string inventoryKey;
        public int currencyCost = 10;
        public PlantState[] states;
        public GameObject prefab;
        public int growthRate;
    }
}