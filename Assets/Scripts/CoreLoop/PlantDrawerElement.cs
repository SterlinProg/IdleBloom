using System;
using Data;
using TMPro;
using UnityEngine;

namespace CoreLoop
{
    public class PlantDrawerElement:MonoBehaviour, IPointerProcessor
    {
        private BasePlantData prefabToSpawn;
        [SerializeField]
        private TMP_Text text;
        
        public void Init(BasePlantData plant)
        {
            prefabToSpawn = plant;
            text.text = $"{prefabToSpawn.plantName} for {prefabToSpawn.currencyCost}";
        }

        public void ProcessTap(Vector2 touchPosition)
        {
            PlayerInputManager.mouseMoved += ProcessDragEvent;
        }

        public void ProcessRelease(Vector2 touchPosition)
        {
            PlayerInputManager.mouseMoved -= ProcessDragEvent;
        }

        public void ProcessDrag(Vector2 touchPosition)
        {
            PlayerInputManager.mouseMoved -= ProcessDragEvent;
            ProtoGameManager.SpawnPlant(prefabToSpawn,touchPosition);
        }
        
        private void ProcessDragEvent(object sender, Vector2 e)
        {
            ProcessDrag(e);
        }
    }
}