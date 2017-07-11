using System;

namespace LiveLink.Services.EventSearchService
{
	public class GetEventsConfiguration
	{
		public DateTime? EarliestDate { get; set; }
		public DateTime? LatestDate { get; set; }
	}
}