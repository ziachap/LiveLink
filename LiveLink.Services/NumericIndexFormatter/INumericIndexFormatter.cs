using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace LiveLink.Services.NumericIndexFormatter
{
	public interface INumericIndexFormatter
	{
		string Format(double number);
	}

	public class NumericIndexFormatter : INumericIndexFormatter
	{
		private const double MaxValue = 99999999;

		public string Format(double number)
		{
			var invertedNumber = Invert(number);

			return SignSymbol(number) + invertedNumber.ToString("0000000000000.0000000000000").Replace(".", "d");
		}

		private double Invert(double number) => IsNegative(number) ? MaxValue + number : number;

		private string SignSymbol(double number) => IsNegative(number) ? "n" : "p";

		private bool IsNegative(double number) => number < 0;
	}

	[TestFixture]
	internal class NumericIndexFormatterTests
	{
		private INumericIndexFormatter Formatter() => new NumericIndexFormatter();

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
