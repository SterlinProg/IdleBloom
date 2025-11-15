using System;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace CoreLoop
{
    public class UIManager:MonoBehaviour
    {
        [FormerlySerializedAs("Currency")] public TMP_Text CurrencyText;

        private void Start()
        {
            Inventory.CurrencyUpdated += UpdateCurrency;
        }

        private void UpdateCurrency(object sender, int e)
        {
            CurrencyText.text = $"Currency {e}";
        }
    }
}