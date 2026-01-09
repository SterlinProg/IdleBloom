using CoreLoop;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public int CurrentCurrency;

    public PlayerData()
    {
        CurrentCurrency = Inventory.CurrentCurrency;
    }
}
