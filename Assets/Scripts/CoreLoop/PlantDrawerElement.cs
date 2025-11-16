using System;
using TMPro;
using UnityEngine;

namespace CoreLoop
{
    public class PlantDrawerElement:MonoBehaviour, IPointerProcessor
    {
        [SerializeField]
        private GameObject prefabToSpawn;
        [SerializeField]
        private TMP_Text text;

        private void Start()
        {
            text.text = $"{prefabToSpawn.name} for 10";
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