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

        private string titleText;
        private string descriptionText;
        public void Initialize(string title, string description)
        {
            titleText = title;
            descriptionText = description;

            OnQuestReopened();
        }

        public void OnQuestCompleted()
        {
            title.text = $"<s>{title.text}<s>";
            description.text = $"<s>{description.text}<s>";
        }

        public void OnQuestReopened()
        {
             title.text = titleText;
             description.text = descriptionText;
        }
    }
}