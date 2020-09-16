using Prism.Mvvm;

namespace UnoPrism200.Infrastructure.Models
{
    public class StockPrice : BindableBase
    {
        private decimal price;
        private float change;

        public int Id { get; set; }

        public string Symbol { get; set; }

        public decimal Price
        {
            get => price;
            set => SetProperty(ref price, value);
        }

        public float Change
        {
            get => change;
            set => SetProperty(ref change, value);
        }
    }
}
