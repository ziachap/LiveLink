namespace LiveLink.Services.IndexFormatters
{
    public class DoubleIndexFormatter : IIndexFormatter<double>
    {
        private const double MaxValue = 99999999;

        public string Format(double value)
        {
            var invertedNumber = Invert(value);

            return SignSymbol(value) + invertedNumber.ToString("0000000000000.0000000000000").Replace(".", "d");
        }

        private double Invert(double number) => IsNegative(number) ? MaxValue + number : number;

        private string SignSymbol(double number) => IsNegative(number) ? "n" : "p";

        private bool IsNegative(double number) => number < 0;
    }
}