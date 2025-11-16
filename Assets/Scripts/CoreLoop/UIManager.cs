using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace CoreLoop
{
    public class UIManager:MonoBehaviour
    {
        public TMP_Text CurrencyText;
        public GridLayoutGroup drawer;
        public GridLayoutGroup plotDrawer;
        public GameObject plotSlotPrefab;
        public GameObject GameplaySlotPrefab;
        private List<GameObject> plotSlots;
        

        private void Start()
        {
            Inventory.CurrencyUpdated += UpdateCurrency;
            plotSlots = new();
            InitPlotGrid();
        }

        private void InitPlotGrid()
        {
            for (int i = 0; i < ProtoGameManager.Instance.plotAmount; i++)
            {
                var go = Instantiate(GameplaySlotPrefab, plotDrawer.transform);
                Instantiate(plotSlotPrefab, go.transform);
                plotSlots.Add(go);
            }
        }

        private void UpdateCurrency(object sender, int e)
        {
            CurrencyText.text = $"Currency {e}";
        }
    }
}