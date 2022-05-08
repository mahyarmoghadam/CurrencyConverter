namespace CurrencyConverterExample
{
    public class GraphNode
    {
        public readonly string FromCurrency;
        public readonly string ToCurrency;
        public readonly decimal ExchangeRate;

        public GraphNode(string fromCurrency, string toCurrency, decimal exchangeRate)
        {
            FromCurrency = fromCurrency;
            ToCurrency = toCurrency;
            ExchangeRate = exchangeRate;
        }
    }
}