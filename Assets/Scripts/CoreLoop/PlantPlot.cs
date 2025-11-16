using System;
using UnityEngine;

namespace CoreLoop
{
    public class PlantPlot:MonoBehaviour,ICurrencyYield, IPointerProcessor
    {
        private Plant currentPlant;
        [SerializeField]
        private Transform anchorPoint;
        [SerializeField]
        private Collider collider;

        private Plant potentialPlant => ProtoGameManager.Instance.HeldPlant;

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
            if (currentPlant == null && potentialPlant != null && Inventory.TryBuyPlant(potentialPlant))
            {
                PlantIn(potentialPlant);
                ProtoGameManager.Instance.HeldPlant = null;
            }

            if (potentialPlant == null && currentPlant != null)
            {
                CollectPlant();
            }
        }

        public void ProcessDrag(Vector2 touchPosition)
        {
        }
    }
}