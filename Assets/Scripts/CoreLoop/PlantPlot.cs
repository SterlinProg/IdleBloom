using System;
using Data;
using UnityEngine;

namespace CoreLoop
{
    public class PlantPlot:MonoBehaviour,ICurrencyYield, IPointerProcessor
    {
        private Plant currentPlant;
        private BasePlotData data;
        [SerializeField]
        private Transform anchorPoint;
        [SerializeField]
        private Collider collider;

        private Plant potentialPlant => ProtoGameManager.Instance.HeldPlant;
        private int remainingUses;

        public void PlantIn(Plant plant)
        {
            currentPlant = plant;
            currentPlant.StartGrowing();
            currentPlant.transform.position = anchorPoint.position;
        }

        public ICurrencyYield.YieldOperator ProcessCurrencyYield(int baseValue, out int result)
        {
            result = currentPlant == null ? 0: currentPlant.CurrentState.baseYield;
            return ICurrencyYield.YieldOperator.Add;
        }

        public void ProcessTap(Vector2 touchPosition)
        {
        }

        public void ProcessRelease(Vector2 touchPosition)
        {
            if (currentPlant == null && potentialPlant != null && Inventory.TryBuyPlant(potentialPlant)
                && ProtoGameManager.Instance.HeldWateringPail == null)
            {
                PlantIn(potentialPlant);
                ProtoGameManager.Instance.HeldPlant = null;
                return;
            }

            if (potentialPlant == null && currentPlant != null && ProtoGameManager.Instance.HeldWateringPail == null)
            {
                CollectPlant();
                return;
            }

            if (currentPlant != null && ProtoGameManager.Instance.HeldWateringPail != null)
            {
                WaterPlant();
                return;
            }
        }

        private void CollectPlant()
        {
            if (currentPlant.IsFinalState)
            {
                Inventory.AddToStock(currentPlant, true);
            }
            Inventory.AddCurrency(currentPlant.CurrentState.collectYield);
            Destroy(currentPlant.gameObject);
            currentPlant = null;
            remainingUses--;
            if (remainingUses <= 0)
                ProtoGameManager.ReturnPlot(this);
        }

        public void ProcessDrag(Vector2 touchPosition)
        {
        }

        public void Init(BasePlotData data)
        {
            this.data = data;
            remainingUses = data.totalUses;
        }

        public void WaterPlant()
        {
            currentPlant.OnWatered();
        }
    }
}