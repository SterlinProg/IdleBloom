using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace CoreLoop
{
    public class Plant: MonoBehaviour
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
        public PlantState CurrentState { get; private set; }

        private void Start()
        {
            Text.text = name;
        }

        public void StartGrowing()
        {
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
    }
}