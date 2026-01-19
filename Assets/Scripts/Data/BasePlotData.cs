using UnityEngine;

namespace Data
{
    [CreateAssetMenu(fileName = "BasePlotData", menuName = "Data/ Plot")]
    public class BasePlotData: ScriptableObject
    {
        public string inventoryKey = "ProtoPlot";
        public string displayName = "ProtoPlot";
        public int basePrice = 5;
        public int priceScaling = 2;
        public int totalUses = 1;
        public GameObject slotPrefab;
    }
}