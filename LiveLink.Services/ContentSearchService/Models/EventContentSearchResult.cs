using System;
using Gibe.DittoProcessors.Media.Models;
using LiveLink.Services.Processors;
using Our.Umbraco.Ditto;
using Umbraco.Core;

namespace LiveLink.Services.ContentSearchService.Models
{
	internal class EventContentSearchResult : IContentSearchResult
	{
		[UmbracoProperty("contentStartDateTime")]
		public DateTime StartDate { get; set; }

		[UmbracoProperty("contentSummary")] public string Summary { get; set; }

		[UmbracoProperty("contentTitle")] public string Title { get; set; }

		// TODO - Haven't worked out what the subtitle is going to be yet
		public string Subtitle => StartDate.ToShortDateString();

		public string Description => Summary.Truncate(64);

		[UmbracoProperty("url")] public string Link { get; set; }

		[UmbracoProperty("contentThumbnail")]
		[ImagePickerOrDefaultImage]
		public MediaImageModel Image { get; set; }

		public bool IsEvent => true;
		public bool IsVenue => false;
	}
}