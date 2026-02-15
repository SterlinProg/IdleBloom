using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Core;
using Data;
using Unity.VisualScripting;
using UnityEngine;

namespace CoreLoop
{
    public class Inventory: MonoBehaviour
    { 
        public class InventoryData
        {
            public int amount;
            public bool draggable;
        }

        public struct InventoryUpdatedArgs
        {
            public string InventoryKey;
            public int amountUpdated;
        }

        public bool clearData = false; 
            
        public static Inventory Instance { get; private set; }
        public static EventHandler<int> CurrencyUpdated;
        public static event EventHandler<InventoryUpdatedArgs> InventoryUpdated;
        public int currencyAmount = 0;
        private List<ICurrencyYield> currencyYields = new List<ICurrencyYield>();
        private Dictionary<PlotDrawerElement, PlantPlot> unlockablePlots = new Dictionary<PlotDrawerElement, PlantPlot>();
        private Dictionary<PlotDrawerElement, int> boughtPlots = new Dictionary<PlotDrawerElement, int>();
        public int purchasedPlotAmount = 0;
        public Dictionary<string, InventoryData> stocks = new();

        public static int CurrentCurrency => Instance.currencyAmount;


        protected virtual void Awake()
        {
            if (Instance != null && Instance != this)
                Destroy(gameObject);
            else
                Instance = this as Inventory;
            
            LoadData();
            StartProcessingCurrency();
        }
        public static void StartProcessingCurrency()
        {
            Instance.StartCoroutine(Instance.ProcessCurrencyYields());
        }

        private IEnumerator ProcessCurrencyYields()
        {
            while (true)
            {
                // CurrencyUpdated?.Invoke(this,currencyAmount);
                yield return new WaitForSeconds(ProtoGameManager.Instance.CurrencyGainDelay);
                SaveData();
            }
        }

        public static bool TryBuyPlant(Plant plant)
        {
            // if (CurrentCurrency >= plant.data.currencyCost)
            // {
            //     Instance.currencyAmount -= plant.data.currencyCost;
            //     return true;
            // }

            if (Instance.stocks.TryGetValue(plant.GetInventoryKey(), out InventoryData item))
            {
                if (item.amount > 0)
                {
                    item.amount--;
                    return true;
                }

                return false;
            }

            return false;

        }

        public static bool TryBuyPlot(PlotDrawerElement plot, BasePlotData data)
        {
            if (Instance.stocks.TryGetValue(data.inventoryKey, out InventoryData item))
            {
                if (item.amount > 0)
                {
                    SpawnNewPlot(plot, data);
                    item.amount--;
                    return true;
                }
                return false;

            }

            return false;
        }

        private static void SpawnNewPlot(PlotDrawerElement plot, BasePlotData data)
        {
            PlantPlot newPlot = ProtoGameManager.SpawnPlot(plot,data);
            Instance.unlockablePlots[plot] = newPlot;
        }

        public static void AddPlot(PlotDrawerElement plotShop)
        {
            Instance.unlockablePlots.Add(plotShop,null);
            Instance.boughtPlots.Add(plotShop,0);
        }
        

        public static void AddToStock(IInventoryItem item, bool draggable)
        {
            string key = item.GetInventoryKey();
            if (!Instance.stocks.ContainsKey(key))
            {
                Instance.stocks.Add(key, new InventoryData()
                {
                    draggable = draggable,
                    amount = 1
                });
            }
            else
            {
                Instance.stocks[key].amount++;
            }
            InventoryUpdated?.Invoke(Instance,new InventoryUpdatedArgs(){InventoryKey = key,amountUpdated = 1});
        }
        
        public static void AddToStock(string item, bool draggable)
        {
            if (!Instance.stocks.ContainsKey(item))
            {
                Instance.stocks.Add(item, new InventoryData()
                {
                    draggable = draggable,
                    amount = 1
                });
            }
            else
            {
                Instance.stocks[item].amount++;
            }
            InventoryUpdated?.Invoke(Instance,new InventoryUpdatedArgs(){InventoryKey = item,amountUpdated = 1});
        }

        public static void AddCurrency(int amount)
        {
            Instance.currencyAmount += amount;
        }

        public static void LockPlot(PlantPlot plantPlot)
        {
            GameObject parentObject = plantPlot.transform.parent.gameObject;
            PlotDrawerElement plotElement = parentObject.GetComponentInChildren<PlotDrawerElement>(includeInactive: true);
            plotElement.elementRoot.SetActive(true);
            Destroy(plantPlot.gameObject);
            Instance.unlockablePlots[plotElement] = null;
        }

        public void SaveData()
        {
            SaveSystem.SavePlayer();
        }

        public void LoadData()
        {
            if (clearData)
            {
                SaveSystem.ClearData();
            }
            PlayerData data = SaveSystem.LoadPlayer();
            if (data == null)
            {
                BootNewInventory();
            }
        }

        private void BootNewInventory()
        {
            for (int i = 0; i < ProtoGameManager.Instance.baseInventory.startingPlantAmount; i++)
                AddToStock(ProtoGameManager.Instance.baseInventory.startingPlant.inventoryKey, true);
            
            for (int i = 0; i < ProtoGameManager.Instance.baseInventory.startingPlotAmount; i++)
                AddToStock(ProtoGameManager.Instance.baseInventory.startingPlot.inventoryKey, true);
        }

        public static int GetItemAmount(string inventoryKey)
        {
            if (Instance.stocks.TryGetValue(inventoryKey, out InventoryData item))
            {
                return item.amount;
            }

            return 0;
        }
        
    }
}