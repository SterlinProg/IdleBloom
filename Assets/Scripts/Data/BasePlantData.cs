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
            public UnityEvent onStateGrow;
            public int baseYield;
            public int collectYield;
        }

        public struct WaterBehaviour
        {
            public float needGrowthRate;
            public UnityEvent onNeedWatering;
            public UnityEvent onNeedFulfilled; 
        }
        
        public string plantName;
        public string inventoryKey;
        public int currencyCost = 10;
        public PlantState[] states;
        public GameObject prefab;
        public int growthRate;
    }
}