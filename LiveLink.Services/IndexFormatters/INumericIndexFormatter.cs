using NUnit.Framework;

namespace LiveLink.Services.IndexFormatters
{
	public interface IIndexFormatter<T>
	{
		string Format(T value);
	}

	public class IntegerIndexFormatter : IIndexFormatter<int>
	{
		// TODO: tests

		private const int MaxValue = 99999999;

		public string Format(int value)
		{
			var invertedNumber = Invert(value);

			return SignSymbol(value) + invertedNumber.ToString("000000000");
		}

		private double Invert(int number) => IsNegative(number) ? MaxValue + number : number;

		private string SignSymbol(int number) => IsNegative(number) ? "n" : "p";

		private bool IsNegative(int number) => number < 0;
	}

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

	[TestFixture]
	internal class DoubleIndexFormatterTests
	{
		private IIndexFormatter<double> Formatter() => new DoubleIndexFormatter();

		[Test]
		public void Negatives_Are_Ordered_Correctly()
		{
			var min = Formatter().Format(-54.2456);
			var max = Formatter().Format(-54.2452);

			Assert.That(min, Is.LessThan(max));
			Assert.That(min.Length, Is.EqualTo(max.Length));
		}

		[Test]
		public void Positives_Are_Ordered_Correctly()
		{
			var min = Formatter().Format(54.2452);
			var max = Formatter().Format(54.2457);

			Assert.That(min, Is.LessThan(max));
			Assert.That(min.Length, Is.EqualTo(max.Length));
		}

		[Test]
		public void Positives_With_Negatives_Are_Ordered_Correctly()
		{
			var min = Formatter().Format(-54.246);
			var max = Formatter().Format(54.2456);

			Assert.That(min, Is.LessThan(max));
			Assert.That(min.Length, Is.EqualTo(max.Length));
		}
	}
}
