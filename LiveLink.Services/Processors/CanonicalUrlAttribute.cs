using Our.Umbraco.Ditto;
using Umbraco.Web;

namespace LiveLink.Services.Processors
{
	public class CanonicalUrlAttribute : DittoProcessorAttribute
	{
		public override object ProcessValue()
		{
			return Context.Content.UrlAbsolute();
		}
	}
}