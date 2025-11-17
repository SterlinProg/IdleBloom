using UnityEngine;

namespace CoreLoop
{
    public interface ICurrencyYield
    {
        enum YieldOperator
        {
            Add,
            Substract,
            Multiply,
            Divide
        }

        YieldOperator ProcessCurrencyYield(int baseValue, out int result);
    }

    public interface IPointerProcessor
    {
        void ProcessTap(Vector2 touchPosition);
        void ProcessRelease(Vector2 touchPosition);
        void ProcessDrag(Vector2 touchPosition);
    }

    public interface IInventoryItem
    {
        string GetInventoryKey(); 
    }
}