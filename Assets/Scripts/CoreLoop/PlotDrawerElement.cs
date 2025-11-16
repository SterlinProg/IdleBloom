using System;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace CoreLoop
{
    public class PlotDrawerElement:MonoBehaviour, IPointerProcessor
    {
        [SerializeField]
        private TMP_Text price;

        [FormerlySerializedAs("elementROot")] public GameObject elementRoot;
        private void Awake()
        {
            Inventory.AddPlot(this);
        }

        private void Update()
        {
            price.text = $"{ProtoGameManager.CalculateNextPlotPrice()}";
        }

        public void ProcessTap(Vector2 touchPosition)
        {
        }

        public void ProcessRelease(Vector2 touchPosition)
        {
            if(Inventory.TryBuyPlot(this))
                UnlockPlot();
        }

        public void ProcessDrag(Vector2 touchPosition)
        {
            
        }

        public void UnlockPlot()
        {
            
        }

    }
}