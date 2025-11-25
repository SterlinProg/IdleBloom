using System;
using Data;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace CoreLoop
{
    public class PlotDrawerElement:MonoBehaviour, IPointerProcessor
    {
        [SerializeField]
        private TMP_Text price;

        public GameObject elementRoot;
        private BasePlotData data;
        private void Awake()
        {
            Inventory.AddPlot(this);
        }

        public void Initialize(BasePlotData data)
        {
            this.data = data;
            price.text = $"{CalculateNextPlotPrice()}";
        }

        private void Update()
        {
        }

        public void ProcessTap(Vector2 touchPosition)
        {
        }

        public void ProcessRelease(Vector2 touchPosition)
        {
            if(Inventory.TryBuyPlot(this,data))
                UnlockPlot();
        }

        public void ProcessDrag(Vector2 touchPosition)
        {
            
        }

        public void UnlockPlot()
        {
            price.text = $"{CalculateNextPlotPrice()}";
        }

        public int CalculateNextPlotPrice()
        {
            int basePrice = data.basePrice;
            return basePrice + Inventory.GetBoughtPlotAmount(this) * data.priceScaling;   
        }
        
    }
}