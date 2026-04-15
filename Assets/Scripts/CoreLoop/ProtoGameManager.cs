using System;
using System.Collections.Generic;
using Data;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace CoreLoop
{
    public class ProtoGameManager:MonoBehaviour
    {

        [Serializable]
        public struct BaseInventory
        {
            public BasePlantData startingPlant;
            public int startingPlantAmount;
            public BasePlotData startingPlot;
            public int startingPlotAmount;
            public int startingPlotSpots;
        }
        // public Plant Plant;
        [FormerlySerializedAs("Plot")] public PlantPlot PlotPrefab;
        public int BaseCurrencyGain = 1;
        public float CurrencyGainDelay = .5f;
        public float BaseGrowthGain = 1;
        public float GrowthGainDelay = 1;
        public bool UseCurrency;
        public float PlantTickRate = .5f;
        public GameObject plantShopElement;
        [SerializeReference]
        public BasePlantData[] plants;
        [SerializeReference]
        public BasePlotData[] plots;

        public BaseInventory baseInventory;
        
        [NonSerialized]
        public Plant HeldPlant;
        [NonSerialized]
        public WateringUIElement HeldWateringPail;

        public EventHandler<PlantPlot> PlantCollected;
        public EventHandler<PlantPlot> PlantWatered;
        public EventHandler<Plant> PlantGrown;
        
        
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
            PlayerInputManager.pointerPressed += OnPointerPressed;
            InitPlantShop();
            CommissionSystem.Initialize();

        }

        public static void InitPlantShop()
        {
            foreach (BasePlantData plant in Instance.plants)
            {
                GameObject newGO = Instantiate(Instance.plantShopElement, UIManager.Instance.drawer.transform);
                newGO.GetComponent<PlantDrawerElement>().Init(plant);
            }
        }

        private void OnPointerPressed(object sender, bool e)
        {
            if(HeldPlant != null && !e)
                DenyPlant();
            if (HeldWateringPail != null && !e)
                HeldWateringPail = null;
        }

        private void Interacted(object sender, EventArgs e)
        {
        }

        public static void SpawnPlant(BasePlantData data, Vector2 touchPosition)
        {
            GameObject spawnedGO = Instantiate(data.prefab, new Vector3(touchPosition.x,touchPosition.y),quaternion.identity);
            Plant plant = spawnedGO.GetComponent<Plant>();
            plant.Init(data);
            Instance.HeldPlant = plant;
        }


        public static void DenyPlant()
        {
            Destroy(Instance.HeldPlant.gameObject);
            Instance.HeldPlant = null;
        }

        public static PlantPlot SpawnPlot(PlotDrawerElement plot, BasePlotData plotData)
        {
            PlantPlot newPlot = Instantiate(plotData.slotPrefab, plot.elementRoot.transform.parent).GetComponent<PlantPlot>();
            newPlot.Init(plotData);
            plot.elementRoot.SetActive(false);
            return newPlot;
        }

        public static void UnlockNewPlot()
        {
            UIManager.InitializePlotElement(Instance.plots[1]);
        }

        public static void ReturnPlot(PlantPlot plantPlot)
        {
            Inventory.LockPlot(plantPlot);
        }

        public static void OnPlantCollected(PlantPlot plot)
        {
            Instance.PlantCollected?.Invoke(Instance,plot);
        } 
        public static void OnPlantWatered(PlantPlot plot)
        {
            Instance.PlantWatered?.Invoke(Instance,plot);
        }

        public static void OnPlantGrowth(Plant plant)
        {
            Instance.PlantGrown?.Invoke(Instance,plant);
        }

    }
}