
using static System.Decimal;

namespace LiveLink.Services.Processors
{
	public class TextToDecimalAttribute : InjectableProcessorAttribute
	{
		public override object ProcessValue()
		{
			if (ValueIsNull) return null;

			return Parse(Value.ToString());
		}
	}
}
