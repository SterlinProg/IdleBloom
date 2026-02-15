using TMPro;
using UnityEngine;

namespace CoreLoop
{
    public class CommissionUIElement: MonoBehaviour
    {
        [SerializeField]
        private TMP_Text title;
        
        [SerializeField]
        private TMP_Text description;

        public void Initialize(string title, string description)
        {
            this.title.text = title;
            this.description.text = description;
            
        }

        public void OnQuestCompleted()
        {
            title.text = $"<s>{title.text}<s>";
            description.text = $"<s>{description.text}<s>";
        }
    }
}