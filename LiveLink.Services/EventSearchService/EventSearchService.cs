using System;
using System.Collections.Generic;
using System.Linq;
using Examine.SearchCriteria;
using LiveLink.Services.DateTimeProvider;
using LiveLink.Services.ExamineService;
using LiveLink.Services.IndexFormatters;
using Umbraco.Core.Models;

namespace LiveLink.Services.EventSearchService
{
	public class EventSearchService : IEventSearchService
	{
		private readonly IDateTimeProvider _dateTimeProvider;
		private readonly IIndexFormatter<double> _doubleFormatter;
		private readonly IExamineSearchProviderWrapper _examineSearchProviderWrapper;
		private readonly IExamineService _examineService;
		private readonly IIndexFormatter<int> _intFormatter;

		public EventSearchService(IExamineService examineService,
			IExamineSearchProviderWrapper examineSearchProviderWrapper,
			IIndexFormatter<double> doubleFormatter,
			IIndexFormatter<int> intFormatter,
			IDateTimeProvider dateTimeProvider)
		{
			_examineService = examineService;
			_examineSearchProviderWrapper = examineSearchProviderWrapper;
			_doubleFormatter = doubleFormatter;
			_intFormatter = intFormatter;
			_dateTimeProvider = dateTimeProvider;
		}

		public IEnumerable<IPublishedContent> GetVenueEvents(GetEventsConfiguration configuration)
		{
			var searcher = _examineSearchProviderWrapper.EventSearcher();

			var query = searcher.CreateSearchCriteria(BooleanOperation.And).Field("__NodeTypeAlias", "event");

			// TODO: Make IFilters and loop through them to build query
			if (configuration.EarliestDate.HasValue && configuration.LatestDate.HasValue)
			{
				query = query.And().Range("contentStartDateTime",
					configuration.EarliestDate.Value, configuration.LatestDate.Value.Date.AddDays(1));
			}
			else if (configuration.EarliestDate.HasValue)
			{
				query = query.And().Range("contentStartDateTime",
					configuration.EarliestDate.Value, DateTime.MaxValue);
			}
			else if (configuration.LatestDate.HasValue)
			{
				query = query.And().Range("contentStartDateTime",
					_dateTimeProvider.Now(), configuration.LatestDate.Value.Date.AddDays(1));
			}
			else
			{
				query = query.And().Range("contentStartDateTime",
					_dateTimeProvider.Now(), DateTime.MaxValue);
			}

			if (configuration.HasBounds)
			{
				query = query.And().Range("contentLongitude",
					_doubleFormatter.Format(configuration.BoundMinX.Value),
					_doubleFormatter.Format(configuration.BoundMaxX.Value));
				query = query.And().Range("contentLatitude",
					_doubleFormatter.Format(configuration.BoundMinY.Value),
					_doubleFormatter.Format(configuration.BoundMaxY.Value));
			}

			if (configuration.LocationId.HasValue)
			{
				query = query.And().GroupedOr(new[] {"country", "city", "venue"},
					_intFormatter.Format(configuration.LocationId.Value));
			}

			query = query.And().OrderBy("contentStartDateTime");

			var results = _examineService.Search(searcher, query.Compile(), configuration.Page,
				configuration.ItemsPerPage);

			return results.ToList();
		}
	}
}