using System;
using Data;
using UnityEngine;

namespace CoreLoop
{
    public class CommissionSystem: MonoBehaviour
    {
        public CommissionUIElement[] commissionUI;
        
        public static CommissionSystem Instance { get; private set; }

        protected virtual void Awake()
        {
            if (Instance != null && Instance != this)
                Destroy(gameObject);
            else
                Instance = this as CommissionSystem;
        }
        
        void RegisterNotifications()
        {
            ProtoGameManager.Instance.PlantCollected += OnProtoFullyGrown;
            ProtoGameManager.Instance.PlantWatered += OnPlantWatered;
            Inventory.InventoryUpdated += OnAddedItemToInventory;
        }

        private int amountProtoToWater = 1;
        private void OnPlantWatered(object sender, PlantPlot e)
        {
            amountProtoToWater--;
            if (amountProtoToWater == 0)
            {
                Inventory.AddToStock(reward.inventoryKey,true);
                CompleteQuest(1);
            }
                
        }

        [SerializeReference]
        private BasePlantData collectPlant;
        private const string collectedKey = "a_";

        private void OnAddedItemToInventory(object sender, Inventory.InventoryUpdatedArgs inventoryUpdatedArgs)
        {
            if (collectedKey+collectPlant.inventoryKey == inventoryUpdatedArgs.InventoryKey)
            {
                CompleteQuest(2);

            }
        }

        private int amountProtoToHarvest = 2;
        [SerializeReference]
        public BasePlantData reward;
        private void OnProtoFullyGrown(object sender, PlantPlot e)
        {
            amountProtoToHarvest--;

            if (amountProtoToHarvest == 0)
            {
                Inventory.AddToStock(reward.inventoryKey,true);
                CompleteQuest(0);
            }
        }

        void On10GrowthCycles()
        {
            
        }

        public static void Initialize()
        {
            Instance.RegisterNotifications();
            Instance.commissionUI[0].Initialize("FirstQuest", "Fully grow two plants");
            Instance.commissionUI[1].Initialize("SecondQuest", "Water a Plant in need");
            Instance.commissionUI[2].Initialize("ThirdQuest", "Collect a fully grown Second");
            Instance.commissionUI[3].Initialize("FourQuest", "Wait a total of ten Growth ");
        }

        public static void CompleteQuest(int commission)
        {
            Instance.commissionUI[commission].OnQuestCompleted();
        }
    }
}