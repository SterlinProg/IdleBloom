using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Data;
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
        public static Inventory Instance { get; private set; }
        public static EventHandler<int> CurrencyUpdated;
        private int currencyAmount = 0;
        private List<ICurrencyYield> currencyYields = new List<ICurrencyYield>();
        private Dictionary<PlotDrawerElement, PlantPlot> unlockablePlots = new Dictionary<PlotDrawerElement, PlantPlot>();
        private Dictionary<PlotDrawerElement, int> boughtPlots = new Dictionary<PlotDrawerElement, int>();
        public int purchasedPlotAmount = 0;
        private Dictionary<string, InventoryData> stocks = new();

        public static int CurrentCurrency => Instance.currencyAmount;


        protected virtual void Awake()
        {
            if (Instance != null && Instance != this)
                Destroy(gameObject);
            else
                Instance = this as Inventory;
        }
        public static void StartProcessingCurrency()
        {
            Instance.StartCoroutine(Instance.ProcessCurrencyYields());
        }

        private IEnumerator ProcessCurrencyYields()
        {
            while (true)
            {
                currencyAmount += ProtoGameManager.Instance.BaseCurrencyGain;
                foreach (ICurrencyYield currencyYield in currencyYields)
                {
                    var yieldOperator = currencyYield.ProcessCurrencyYield(0, out int resultValue);
                    ApplyYield(ref currencyAmount,yieldOperator,resultValue);
                }
                CurrencyUpdated?.Invoke(this,currencyAmount);
                yield return new WaitForSeconds(ProtoGameManager.Instance.CurrencyGainDelay);
            }
        }

        private void ApplyYield(ref int currentAmount, ICurrencyYield.YieldOperator yieldOperator, int resultValue)
        {
            switch (yieldOperator)
            {
                case ICurrencyYield.YieldOperator.Add:
                    currentAmount += resultValue;
                    break;
                case ICurrencyYield.YieldOperator.Substract:
                    break;
                case ICurrencyYield.YieldOperator.Divide:
                    break;
                case ICurrencyYield.YieldOperator.Multiply:
                    break;
            }
        }

        public static bool TryBuyPlant(Plant plant)
        {
            if (CurrentCurrency >= plant.data.currencyCost)
            {
                Instance.currencyAmount -= plant.data.currencyCost;
                return true;
            }

            return false;

        }

        public static bool TryBuyPlot(PlotDrawerElement plot, BasePlotData data)
        {
            int totalPrice = plot.CalculateNextPlotPrice();
            if (CurrentCurrency >= totalPrice)
            {
                Instance.currencyAmount -= totalPrice;
                PlantPlot newPlot = ProtoGameManager.SpawnPlot(plot,data);
                Instance.unlockablePlots[plot] = newPlot;
                Instance.boughtPlots[plot]++;
                return true;
            }

            return false;
        }

        public static void AddPlot(PlotDrawerElement plotShop)
        {
            Instance.unlockablePlots.Add(plotShop,null);
            Instance.boughtPlots.Add(plotShop,0);
        }

        public static void RegisterCurrencyYield(ICurrencyYield currencyYield)
        {
            Instance.currencyYields.Add(ProtoGameManager.Instance.PlotPrefab);
        }
        
        public static void UnregisterCurrencyYield(ICurrencyYield currencyYield)
        {
            Instance.currencyYields.Remove(ProtoGameManager.Instance.PlotPrefab);
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

        public static int GetBoughtPlotAmount(PlotDrawerElement plot)
        {
            return Instance.boughtPlots[plot];
        }
    }
}