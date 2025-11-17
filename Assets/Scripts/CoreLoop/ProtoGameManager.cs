using System;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace CoreLoop
{
    public class ProtoGameManager:MonoBehaviour
    {
        // public Plant Plant;
        [FormerlySerializedAs("Plot")] public PlantPlot PlotPrefab;
        public int BaseCurrencyGain = 1;
        public float CurrencyGainDelay = .5f;
        public float BaseGrowthGain = 1;
        public float GrowthGainDelay = 1;
        public Plant HeldPlant;
        public int plotAmount = 6;
        public int basePlotPrice = 5;
        public int plotPriceScaling = 2;
        public int maxPlotUses = 3;
        
        
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
            PlayerInputManager.pointerPressed += OnPointerPressed;
        }

        private void OnPointerPressed(object sender, bool e)
        {
            if(HeldPlant != null && !e)
                DenyPlant();
        }

        private void Started(object sender, EventArgs e)
        {
            Inventory.StartProcessingCurrency();
        }

        private void Interacted(object sender, EventArgs e)
        {
        }

        public static void SpawnPlant(GameObject prefab, Vector2 touchPosition)
        {
            GameObject spawnedGO = Instantiate(prefab, new Vector3(touchPosition.x,touchPosition.y),quaternion.identity);
            Instance.HeldPlant = spawnedGO.GetComponent<Plant>();
        }


        public static void DenyPlant()
        {
            Destroy(Instance.HeldPlant.gameObject);
            Instance.HeldPlant = null;
        }

        public static PlantPlot SpawnPlot(PlotDrawerElement plot)
        {
            PlantPlot newPlot = Instantiate(Instance.PlotPrefab, plot.elementRoot.transform.parent);
            newPlot.Init();
            plot.elementRoot.SetActive(false);
            Inventory.RegisterCurrencyYield(newPlot);
            return newPlot;
        }

        public static int CalculateNextPlotPrice()
        {
            int basePrice = Instance.basePlotPrice;
            return basePrice + Inventory.Instance.purchasedPlotAmount * Instance.plotPriceScaling;   
        }

        public static void ReturnPlot(PlantPlot plantPlot)
        {
            Inventory.LockPlot(plantPlot);
        }
    }
}