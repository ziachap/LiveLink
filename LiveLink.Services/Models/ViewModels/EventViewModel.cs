using System;
using System.Collections.Generic;
using Gibe.DittoProcessors.Media.Models;
using Gibe.DittoProcessors.Processors;
using LiveLink.Services.Processors;
using Our.Umbraco.Ditto;
using Umbraco.Core;

namespace LiveLink.Services.Models.ViewModels
{
    public class EventViewModel
    {
		[UmbracoProperty("id")]
		public int Id { get; set; }

        [UmbracoProperty("contentTitle")]
        public string Title { get; set; }

	    public string SummaryShort => Summary.Truncate(128);

		[UmbracoProperty("contentSummary")]
        public string Summary { get; set; }

        [UmbracoProperty("contentDescription")]
        public string Description { get; set; }

		[UmbracoProperty("contentThumbnail")]
		[ImagePickerOrDefaultImage]
		public MediaImageModel Thumbnail { get; set; }

	    public string ThumbnailUrl => Thumbnail?.Url;

		[UmbracoProperty("contentTicketUri")]
        public string TicketUri { get; set; }

        [UmbracoProperty("contentStartDateTime")]
        public DateTime StartTime { get; set; }

	    public string FormattedStartTime 
			=> string.Format("{0:dddd dd MMMM yyyy} at {0:h:mmtt}", StartTime);

		public string FormattedStartDate
		   => string.Format("{0:dddd dd MMMM yyyy}", StartTime);

		[UmbracoProperty("contentEndDateTime")]
        public DateTime EndTime { get; set; }

		public string FormattedEndTime
		   => string.Format("{0:dddd dd MMMM yyyy} at {0:h:mmtt}", EndTime);

		public string FormattedDateTime => StartTime.AddHours(12) < EndTime 
			? FormattedStartTime + " - " + FormattedEndTime
			: FormattedStartTime + $" - {EndTime:h:mmtt}";

		[UmbracoProperty("url")]
		public string Url { get; set; }

		[Parent]
		[Model(typeof(EventVenueModel))]
		public EventVenueModel Venue { get; set; }

		[UserIsWatching]
		public bool Watching { get; set; }

		[OtherVenueEvents(4)]
		public IEnumerable<OtherEventModel> OtherEvents { get; set; }
    }
}