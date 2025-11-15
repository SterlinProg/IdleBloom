using UnityEngine;

namespace CoreLoop
{
    public class PlantPlot:MonoBehaviour,ICurrencyYield
    {
        private Plant plant;
        [SerializeField]
        private Transform anchorPoint;
        
        public void PlantIn(Plant plant)
        {
            this.plant = plant;
            this.plant.StartGrowing();
            this.plant.transform.position = anchorPoint.position;
        }

        public ICurrencyYield.YieldOperator ProcessCurrencyYield(int baseValue, out int result)
        {
            result = plant == null ? 0: plant.CurrentState.baseYield;
            return ICurrencyYield.YieldOperator.Add;
        }
    }
}