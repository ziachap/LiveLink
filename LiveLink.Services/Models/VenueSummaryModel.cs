using Gibe.DittoProcessors.Media.Models;
using LiveLink.Services.Processors;
using Our.Umbraco.Ditto;

namespace LiveLink.Services.Models
{
	public class VenueSummaryModel
	{
		[UmbracoProperty("contentTitle")]
		public string Title { get; set; }

		[UmbracoProperty("contentLatitude")]
		[TextToDecimal]
		public decimal Latitude { get; set; }

		[UmbracoProperty("contentLongitude")]
		[TextToDecimal]
		public decimal Longitude { get; set; }

		[UmbracoProperty("contentLogo")]
		[ImagePickerOrDefaultImage]
		public MediaImageModel Logo { get; set; }

		[DittoIgnore]
		public string LogoUrl => Logo?.Url;
	}
}
