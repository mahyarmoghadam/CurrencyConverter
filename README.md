# Currency Converter



``` csharp
ICurrencyConverter currencyConverter = new CurrencyConverter();

currencyConverter.UpdateConfiguration(new List<Tuple<string, string, double>>()
{
    new Tuple<string, string, double>("USD", "CAD", 1.34),
    new Tuple<string, string, double>("CAD", "GBP", 0.58),
    new Tuple<string, string, double>("USD", "EUR", 0.86),
});

Console.WriteLine(currencyConverter.Convert("USD", "CAD", 100));
Console.WriteLine(currencyConverter.Convert("CAD", "GBP", 100));
Console.WriteLine(currencyConverter.Convert("USD", "EUR", 100));
Console.WriteLine(currencyConverter.Convert("CAD", "EUR", 100));

```

