using System;
using System.Collections;
using Data;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace CoreLoop
{
    public class Plant: MonoBehaviour,IPointerProcessor,IInventoryItem
    {
        [NonSerialized]
        public BasePlantData data;
        public TMP_Text Text;
        public Collider collider;
        public int CurrentStateIndex { get; private set; }
        public BasePlantData.PlantState CurrentState => data.states[CurrentStateIndex];

        public bool IsFinalState => CurrentStateIndex == data.states.Length-1;

        public void Init(BasePlantData data)
        {
            this.data = data;
            Text.text = this.data.plantName;
            PlayerInputManager.mouseMoved += ProcessDragEvent;
        }

        public void StartGrowing()
        {
            collider.enabled = false;
            StartCoroutine(DoGrowProcess());
        }

        private IEnumerator DoGrowProcess()
        {
            for (var i = 0; i < data.states.Length; i++)
            {
                var plantState = data.states[i];
                ExecuteState(plantState);
                CurrentStateIndex = i;
                yield return new WaitForSeconds(plantState.timeToGrow);
            }
        }

        private void ExecuteState(BasePlantData.PlantState plantState)
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
            return data.inventoryKey;
        }
    }
}