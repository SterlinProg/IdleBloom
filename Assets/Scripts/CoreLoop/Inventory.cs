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
        public static int CurrentCurrency => Instance.currencyAmount;

        public static void StartProcessingCurrency()
        {
            Instance.currencyYields.Add(ProtoGameManager.Instance.Plot);
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
                yield return new WaitForSeconds(ProtoGameManager.Instance.DelayCurrencyGain);
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

        public static void BuyItem(Plant plant)
        {
            Instance.currencyAmount -= plant.currencyCost;
        }
    }
}