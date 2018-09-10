using System;

namespace LiveLink.Services.Processors
{
	public class DebugAttribute : InjectableProcessorAttribute
	{
		public override object ProcessValue()
		{
			throw new NotImplementedException();
		}
	}
}