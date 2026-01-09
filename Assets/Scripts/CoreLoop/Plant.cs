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

        private float currentGrowthPoints;

        public void Init(BasePlantData data)
        {
            this.data = data;
            Text.text = this.data.plantName;
            PlayerInputManager.mouseMoved += ProcessDragEvent;
            currentGrowthPoints = 0;
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
                currentGrowthPoints = 0f;
                var plantState = data.states[i];
                ExecuteState(plantState);
                CurrentStateIndex = i;
                while (plantState.timeToGrow <= currentGrowthPoints)
                {
                    yield return null;
                    TickGrowth();
                }
                yield return new WaitForSeconds(plantState.timeToGrow);
            }
        }

        private void ExecuteState(BasePlantData.PlantState plantState)
        {
            SpriteRenderer renderer = GetComponentInChildren<SpriteRenderer>();
            renderer.sprite = plantState.currentGrowthSprite;
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

        public void TickGrowth()
        {
            float addition = data.growthRate * Time.fixedDeltaTime;
            IncreaseGrowth(addition);
        }

        public void IncreaseGrowth(float add)
        {
            currentGrowthPoints += add;
        }

        public void OnWatered()
        {
            Debug.Log($"{name}: :D");
        }

        public bool TricklesDownAction()
        {
            return true;
        }
    }
}