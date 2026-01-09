using System.Collections;
using System.Collections.Generic;
using Data;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace CoreLoop
{
    public class WateringUIElement:MonoBehaviour,IPointerProcessor
    {
        [SerializeField]
        private TMP_Text text;
        [SerializeField]
        private GameObject MoveableObject;
        [SerializeField]
        private Transform anchor;
        
        public void Awake()
        {
            text.text = "Water";
            MoveableObject.transform.position = anchor.position;
        }

        public void ProcessTap(Vector2 touchPosition)
        {
            PlayerInputManager.mouseMoved += ProcessDragEvent;
        }

        public void ProcessRelease(Vector2 touchPosition)
        {
            PlayerInputManager.mouseMoved -= ProcessDragEvent;
            MoveableObject.transform.position = anchor.position;
            StartCoroutine(CancelWateringPail());
        }

        private IEnumerator CancelWateringPail()
        {
            yield return new WaitForEndOfFrame();
            ProtoGameManager.Instance.HeldWateringPail = null;
        }

        public void ProcessDrag(Vector2 touchPosition)
        {
            MoveableObject.transform.position = new Vector3(touchPosition.x,touchPosition.y);
            ProtoGameManager.Instance.HeldWateringPail = this;
        }
        
        
        private void ProcessDragEvent(object sender, Vector2 e)
        {
            ProcessDrag(e);
        }
    }
}