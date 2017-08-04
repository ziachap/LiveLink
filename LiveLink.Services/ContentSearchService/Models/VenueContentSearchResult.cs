using System;
using Gibe.DittoProcessors.Media.Models;
using LiveLink.Services.Processors;
using Our.Umbraco.Ditto;

namespace LiveLink.Services.ContentSearchService.Models
{
	internal class VenueContentSearchResult : IContentSearchResult
	{
		[UmbracoProperty("contentTitle")]
		public string Title { get; set; }
		
		public string Subtitle { get; set; }

		public string Description { get; set; }

		[UmbracoProperty("url")]
		public string Link { get; set; }

		[UmbracoProperty("contentLogo")]
		[ImagePickerOrDefaultImage]
		public MediaImageModel Image { get; set; }

		public bool IsEvent => false;
		public bool IsVenue => true;
	}
}