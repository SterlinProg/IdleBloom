using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace CoreLoop
{
    public class ProtoGameManager:MonoBehaviour
    {
        public Plant Plant;
        public PlantPlot Plot;
        public int BaseCurrencyGain = 1;
        public float DelayCurrencyGain = .5f;
        
        public static ProtoGameManager Instance { get; private set; }
        protected virtual void Awake()
        {
            if (Instance != null && Instance != this)
                Destroy(gameObject);
            else
                Instance = this as ProtoGameManager;
        }
        
        private void Start()
        {
            PlayerInputManager.interacted += Interacted;
            PlayerInputManager.started += Started;
        }

        private void Started(object sender, EventArgs e)
        {
            Inventory.StartProcessingCurrency();
        }

        private void Interacted(object sender, EventArgs e)
        {
            if (Inventory.CurrentCurrency >= Plant.currencyCost)
            {
                Inventory.BuyItem(Plant);
                Plot.PlantIn(Plant);
            }
        }
        
        
    }
}