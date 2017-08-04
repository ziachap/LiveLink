using System;
using Gibe.DittoProcessors.Media.Models;
using LiveLink.Services.Processors;
using Our.Umbraco.Ditto;
using Umbraco.Core;

namespace LiveLink.Services.ContentSearchService.Models
{
	internal class EventContentSearchResult : IContentSearchResult
	{
		[UmbracoProperty("contentTitle")]
		public string Title { get; set; }

		[UmbracoProperty("contentStartDateTime")]
		public DateTime StartDate { get; set; }

		// TODO
		public string Subtitle => StartDate.ToShortDateString();

		[UmbracoProperty("contentSummary")]
		public string Summary { get; set; }

		public string Description => Summary.Truncate(64);

		[UmbracoProperty("url")]
		public string Link { get; set; }

		[UmbracoProperty("contentThumbnail")]
		[ImagePickerOrDefaultImage]
		public MediaImageModel Image { get; set; }

		public bool IsEvent => true;
		public bool IsVenue => false;
	}
}