using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace CoreLoop
{
    public class Plant: MonoBehaviour,IPointerProcessor,IInventoryItem
    {
        [Serializable]
        public struct PlantState
        {
            public float protoScale;
            public float timeToGrow;
            public UnityEvent onStateGrow;
            public int baseYield;
            public int collectYield ;
        }

        public string name;
        public string inventoryKey;
        public int currencyCost = 10;
        public PlantState[] states;
        public TMP_Text Text;
        public Collider collider;
        public int CurrentStateIndex { get; private set; }
        public PlantState CurrentState => states[CurrentStateIndex];

        public bool IsFinalState => CurrentStateIndex == states.Length-1;

        private void Start()
        {
            Text.text = name;
            PlayerInputManager.mouseMoved += ProcessDragEvent;
        }

        public void StartGrowing()
        {
            collider.enabled = false;
            StartCoroutine(DoGrowProcess());
        }

        private IEnumerator DoGrowProcess()
        {
            for (var i = 0; i < states.Length; i++)
            {
                var plantState = states[i];
                ExecuteState(plantState);
                CurrentStateIndex = i;
                yield return new WaitForSeconds(plantState.timeToGrow);
            }
        }

        private void ExecuteState(PlantState plantState)
        {
            var transform1 = transform;
            transform1.localScale = new Vector3(plantState.protoScale/2,plantState.protoScale, transform1.localScale.z);
            plantState.onStateGrow?.Invoke();
        }

        #region GameplayManipulation


        public void ProcessTap(Vector2 touchPosition)
        {
            PlayerInputManager.mouseMoved += ProcessDragEvent;
        }

        private void ProcessDragEvent(object sender, Vector2 e)
        {
            ProcessDrag(e);
        }

        public void ProcessRelease(Vector2 touchPosition)
        {
            PlayerInputManager.mouseMoved -= ProcessDragEvent;
        }

        public void ProcessDrag(Vector2 touchPosition)
        {
            gameObject.transform.position = touchPosition;
        }
        #endregion

        public string GetInventoryKey()
        {
            return inventoryKey;
        }
    }
}