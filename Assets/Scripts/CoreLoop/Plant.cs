using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace CoreLoop
{
    public class Plant: MonoBehaviour,IPointerProcessor
    {
        [Serializable]
        public struct PlantState
        {
            public float protoScale;
            public float timeToGrow;
            public UnityEvent onStateGrow;
            public int baseYield ;
        }

        public string name;
        public int currencyCost = 10;
        public PlantState[] states;
        public TMP_Text Text;
        public Collider collider;
        public PlantState CurrentState { get; private set; }

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
            foreach (PlantState plantState in states)
            {
                ExecuteState(plantState);
                CurrentState = plantState;
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

    }
}