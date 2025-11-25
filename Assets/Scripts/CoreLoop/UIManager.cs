using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace CoreLoop
{
    public class UIManager:MonoBehaviour
    {
        public static UIManager Instance;
        public TMP_Text CurrencyText;
        public GridLayoutGroup drawer;
        public GridLayoutGroup plotDrawer;
        public GameObject plotSlotPrefab;
        public GameObject GameplaySlotPrefab;
        private List<GameObject> plotSlots;

        protected virtual void Awake()
        {
            if (Instance != null && Instance != this)
                Destroy(gameObject);
            else
                Instance = this as UIManager;
        }

        private void Start()
        {
            Inventory.CurrencyUpdated += UpdateCurrency;
            plotSlots = new();
            InitPlotGrid();
        }

        private void InitPlotGrid()
        {
            for (int i = 0; i < ProtoGameManager.Instance.plots.Length; i++)
            {
                var plot = ProtoGameManager.Instance.plots[i];
                var go = Instantiate(GameplaySlotPrefab, plotDrawer.transform);
                GameObject lockedSlot = Instantiate(plotSlotPrefab, go.transform);
                lockedSlot.GetComponentInChildren<PlotDrawerElement>().Initialize(plot);
                plotSlots.Add(go);
            }
        }

        private void UpdateCurrency(object sender, int e)
        {
            CurrencyText.text = $"Currency {e}";
        }
    }
}