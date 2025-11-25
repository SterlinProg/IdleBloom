using UnityEngine;

namespace Data
{
    [CreateAssetMenu(fileName = "BasePlotData", menuName = "Data/ Plot")]
    public class BasePlotData: ScriptableObject
    {
        public int basePrice = 5;
        public int priceScaling = 2;
        public int totalUses = 1;
        public GameObject slotPrefab;
    }
}