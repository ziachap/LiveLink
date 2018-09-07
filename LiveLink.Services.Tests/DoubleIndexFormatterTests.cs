using LiveLink.Services.IndexFormatters;
using NUnit.Framework;

namespace LiveLink.Services.Tests
{
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