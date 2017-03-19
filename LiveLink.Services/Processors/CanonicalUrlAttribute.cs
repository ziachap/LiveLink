using Our.Umbraco.Ditto;

namespace LiveLink.Services.Processors
{
	public class CanonicalUrlAttribute : DittoProcessorAttribute
	{
		public override object ProcessValue()
		{
			// TODO: Make this absolute
			return Context.Content.Url;
		}
	}
}