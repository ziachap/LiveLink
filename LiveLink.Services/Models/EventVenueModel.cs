using LiveLink.Services.Processors;
using Our.Umbraco.Ditto;

namespace LiveLink.Services.Models
{
	public class EventVenueModel
	{
		[UmbracoProperty("contentTitle")]
		public string Title { get; set; }

		[UmbracoProperty("contentLatitude")]
		[TextToDecimal]
		public decimal Latitude { get; set; }

		[UmbracoProperty("contentLongitude")]
		[TextToDecimal]
		public decimal Longitude { get; set; }
	}
}
