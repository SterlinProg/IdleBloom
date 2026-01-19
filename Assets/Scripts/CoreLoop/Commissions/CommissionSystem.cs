using Data;
using UnityEngine;

namespace CoreLoop
{
    public class CommissionSystem: MonoBehaviour
    {
        public CommissionUIElement[] commissionUI;
        
        public static CommissionSystem Instance { get; private set; }

        protected virtual void Awake()
        {
            if (Instance != null && Instance != this)
                Destroy(gameObject);
            else
                Instance = this as CommissionSystem;
        }
        
        void RegisterNotifications()
        {
            ProtoGameManager.Instance.PlantCollected += OnProtoFullyGrown;
        }

        private int amountProtoToHarvest = 2;
        [SerializeReference]
        public BasePlantData reward;
        private void OnProtoFullyGrown(object sender, PlantPlot e)
        {
            amountProtoToHarvest--;
            
            if (amountProtoToHarvest == 0)
                Inventory.AddToStock(reward.inventoryKey,true);
        }

        void On10GrowthCycles()
        {
            
        }

        public static void Initialize()
        {
            Instance.RegisterNotifications();
            Instance.commissionUI[0].Initialize("FirstQuest", "Fully grow two plants");
        }
    }
}