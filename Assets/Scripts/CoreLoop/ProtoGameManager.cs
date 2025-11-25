using System;
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
        // public Plant Plant;
        [FormerlySerializedAs("Plot")] public PlantPlot PlotPrefab;
        public int BaseCurrencyGain = 1;
        public float CurrencyGainDelay = .5f;
        public float BaseGrowthGain = 1;
        public float GrowthGainDelay = 1;
        public GameObject plantShopElement;
        public BasePlantData[] plants;
        public BasePlotData[] plots;
        
        [NonSerialized]
        public Plant HeldPlant;
        
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

            InitPlantShop();
        }

        private void InitPlantShop()
        {
            foreach (BasePlantData plant in plants)
            {
                GameObject newGO = Instantiate(plantShopElement, UIManager.Instance.drawer.transform);
                newGO.GetComponent<PlantDrawerElement>().Init(plant);
            }
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
            Inventory.RegisterCurrencyYield(newPlot);
            return newPlot;
        }

        public static void ReturnPlot(PlantPlot plantPlot)
        {
            Inventory.LockPlot(plantPlot);
        }
    }
}