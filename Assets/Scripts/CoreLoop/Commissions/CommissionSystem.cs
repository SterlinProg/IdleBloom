using System;
using System.Collections;
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
            ProtoGameManager.Instance.PlantGrown += On10GrowthCycles;
        }

        private int amountProtoToWater = 1;
        private int currentwater = 0;
        private void OnPlantWatered(object sender, PlantPlot e)
        {
            currentwater++;
            if (currentwater == amountProtoToWater)
            {
                Inventory.AddToStock(reward.inventoryKey,true);
                CompleteQuest(1);
            }
                
        }

        [SerializeReference]
        private BasePlantData collectPlant;
        private const string collectedKey = "a_";
        [SerializeReference]
        public BasePlantData reward2;
        private void OnAddedItemToInventory(object sender, Inventory.InventoryUpdatedArgs inventoryUpdatedArgs)
        {
            if (collectedKey+collectPlant.inventoryKey == inventoryUpdatedArgs.InventoryKey)
            {
                Inventory.AddToStock(reward2.inventoryKey,true);
                Inventory.AddToStock(reward2.inventoryKey,true);
                Inventory.AddToStock(reward2.inventoryKey,true);
                Inventory.AddToStock(ProtoGameManager.Instance.baseInventory.startingPlot.inventoryKey, true);
                CompleteQuest(2);
            }
        }

        private int amountProtoToHarvest = 2;
        private int currentProtoHarvest = 0;
        [SerializeReference]
        public BasePlantData reward;
        private void OnProtoFullyGrown(object sender, PlantPlot e)
        {
            currentProtoHarvest++;

            if (currentProtoHarvest == amountProtoToHarvest)
            {
                Inventory.AddToStock(reward.inventoryKey,true);
                CompleteQuest(0);
            }
        }


        [SerializeField]
        private int growthCycleTodo = 10;
        private int growthCycleCount = 0;
        private int clearCount = 0;
        void On10GrowthCycles(object o, Plant plant)
        {
            if(plant.CurrentStateIndex > 0)
                growthCycleCount++;
            if (growthCycleCount >= growthCycleTodo)
            {
                if (clearCount <= 0)
                {
                    clearCount++;
                    ProtoGameManager.UnlockNewPlot();
                }
                CompleteQuest(3);
                Inventory.AddToStock(ProtoGameManager.Instance.baseInventory.startingPlot.inventoryKey, true);
                Inventory.AddToStock(ProtoGameManager.Instance.baseInventory.startingPlot.inventoryKey, true);
                Inventory.AddToStock(ProtoGameManager.Instance.baseInventory.startingPlot.inventoryKey, true);

                ReopenQuests(0,1,2);
            }
        }

        public static void Initialize()
        {
            Instance.RegisterNotifications();
            Instance.commissionUI[0].Initialize("FirstQuest", $"Collect {Instance.amountProtoToHarvest} fully grown plants");
            Instance.commissionUI[1].Initialize("SecondQuest", $"Water {Instance.amountProtoToWater} Plant in need");
            Instance.commissionUI[2].Initialize("ThirdQuest", "Collect a fully grown Second");
            Instance.commissionUI[3].Initialize("FourQuest", $"Wait a total of {Instance.growthCycleTodo} Growth ");
        }

        public static void CompleteQuest(int commission)
        {
            Instance.commissionUI[commission].OnQuestCompleted();
            Instance.ToggleQuest(commission, false);
        }

        //this is bad coding I know but it's really cool I swear
        public void ReopenQuests(params int[] commissionNums)
        {
            foreach (int i in commissionNums)
            {
                Instance.commissionUI[i].OnQuestReopened();
                ToggleQuest(i, true);
            }
        }

        IEnumerator StartResetCooldown()
        {
            yield return new WaitForSeconds(15f);
            ReopenQuests(3);
        }

        public void ToggleQuest(int quest, bool active)
        {
            switch (quest)
            {
                case 0:
                    if (active)
                    {
                        currentProtoHarvest = 0;
                        amountProtoToHarvest++;
                        ProtoGameManager.Instance.PlantCollected += OnProtoFullyGrown;
                        Instance.commissionUI[0].Initialize("FirstQuest", $"Collect {Instance.amountProtoToHarvest} fully grown plants");
                    }
                    else
                    {
                        ProtoGameManager.Instance.PlantCollected -= OnProtoFullyGrown;
                    }
                    break;
                case 1:
                    if (active)
                    {
                        currentwater = 0;
                        amountProtoToWater++;
                        ProtoGameManager.Instance.PlantCollected += OnProtoFullyGrown;
                        Instance.commissionUI[1].Initialize("SecondQuest", $"Water {Instance.amountProtoToWater} Plant in need");

                    }
                    else
                        ProtoGameManager.Instance.PlantCollected -= OnProtoFullyGrown;
                    break;
                case 2:
                    if (active)
                        Inventory.InventoryUpdated += OnAddedItemToInventory;
                    else
                        Inventory.InventoryUpdated -= OnAddedItemToInventory;
                    break;
                case 3:
                    if (active)
                    {
                        ProtoGameManager.Instance.PlantGrown += On10GrowthCycles;
                        growthCycleCount = 0;
                        growthCycleTodo += 10;
                        Instance.commissionUI[3].Initialize("FourQuest", $"Wait a total of {Instance.growthCycleTodo} Growth ");
                    }
                    else
                    {
                        ProtoGameManager.Instance.PlantGrown -= On10GrowthCycles;
                        StartCoroutine(StartResetCooldown());
                    }
                    break;
                default:
                    break;
            }
        }
    }
}