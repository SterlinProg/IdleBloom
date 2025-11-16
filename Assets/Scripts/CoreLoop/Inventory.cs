using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CoreLoop
{
    public class Inventory: MonoBehaviour
    {
        public static Inventory Instance { get; private set; }
        public static EventHandler<int> CurrencyUpdated;
        protected virtual void Awake()
        {
            if (Instance != null && Instance != this)
                Destroy(gameObject);
            else
                Instance = this as Inventory;
        }
    
        private int currencyAmount = 0;
        private List<ICurrencyYield> currencyYields = new List<ICurrencyYield>();
        private Dictionary<PlotDrawerElement, bool> unlockablePlots = new Dictionary<PlotDrawerElement, bool>();
        public int purchasedPlotAmount = 0;
        public static int CurrentCurrency => Instance.currencyAmount;

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
            if (CurrentCurrency >= plant.currencyCost)
            {
                Instance.currencyAmount -= plant.currencyCost;
                return true;
            }

            return false;

        }

        public static bool TryBuyPlot(PlotDrawerElement plot)
        {
            int totalPrice = ProtoGameManager.CalculateNextPlotPrice();
            if (CurrentCurrency >= totalPrice)
            {
                Instance.currencyAmount -= totalPrice;
                Instance.unlockablePlots[plot] = true;
                ProtoGameManager.SpawnPlot(plot);
                Instance.purchasedPlotAmount++;
                return true;
            }

            return false;
        }

        public static void AddPlot(PlotDrawerElement plotShop)
        {
            Instance.unlockablePlots.Add(plotShop,false);
        }

        public static void RegisterCurrencyYield(ICurrencyYield currencyYield)
        {
            Instance.currencyYields.Add(ProtoGameManager.Instance.PlotPrefab);
        }
        
        public static void UnregisterCurrencyYield(ICurrencyYield currencyYield)
        {
            Instance.currencyYields.Remove(ProtoGameManager.Instance.PlotPrefab);
        }
    }
}