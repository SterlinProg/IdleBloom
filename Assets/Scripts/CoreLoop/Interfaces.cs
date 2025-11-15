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
}