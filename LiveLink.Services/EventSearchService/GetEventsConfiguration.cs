using System;

namespace LiveLink.Services.EventSearchService
{
	public class GetEventsConfiguration
	{
		public DateTime? EarliestDate { get; set; }
		public DateTime? LatestDate { get; set; }

		public double? BoundMinX { get; set; }
		public double? BoundMaxX { get; set; }
		public double? BoundMinY { get; set; }
		public double? BoundMaxY { get; set; }

		public int? CountryId { get; set; }
		public int? CityId { get; set; }
		public int? VenueId { get; set; }

		public int? ItemsPerPage { get; set; }
		public int? Page { get; set; }

		public int? LocationId => VenueId ?? CityId ?? CountryId;

		public bool HasBounds => BoundMinX.HasValue 
			&& BoundMaxX.HasValue && BoundMinY.HasValue && BoundMaxY.HasValue;
	}
}