namespace LiveLink.Services.IndexFormatters
{
	// TODO: This needs tests!
	public class IntegerIndexFormatter : IIndexFormatter<int>
	{
		private const int MaxValue = 99999999;

		public string Format(int value)
		{
			var invertedNumber = Invert(value);

			return SignSymbol(value) + invertedNumber.ToString("000000000");
		}

		private double Invert(int number)
		{
			return IsNegative(number) ? MaxValue + number : number;
		}

		private string SignSymbol(int number)
		{
			return IsNegative(number) ? "n" : "p";
		}

		private bool IsNegative(int number)
		{
			return number < 0;
		}
	}
}