namespace TollCalculatorExercise.Domain.Models
{
    public class TollFee
    {
        public TollFee(decimal amount)
        {
            Amount = amount;
        }
        public decimal Amount { get; private set; }
    }
}
