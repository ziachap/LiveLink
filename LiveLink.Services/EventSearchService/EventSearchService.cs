using System;
using System.Collections.Generic;
using System.Linq;
using Examine.SearchCriteria;
using Gibe.DittoServices.ModelConverters;
using LiveLink.Services.ExamineService;
using LiveLink.Services.IndexFormatters;
using LiveLink.Services.Models.ViewModels;
using Umbraco.Core;
using Umbraco.Core.Models;

namespace LiveLink.Services.EventSearchService
{
	public class EventSearchService : IEventSearchService
	{
		private readonly IExamineSearchProviderWrapper _examineSearchProviderWrapper;
		private readonly IExamineService _examineService;
		private readonly IModelConverter _modelConverter;
		private readonly IIndexFormatter<double> _doubleFormatter;
		private readonly IIndexFormatter<int> _intFormatter;

		public EventSearchService(IExamineService examineService,
			IExamineSearchProviderWrapper examineSearchProviderWrapper,
			IModelConverter modelConverter,
			IIndexFormatter<double> doubleFormatter,
			IIndexFormatter<int> intFormatter)
		{
			_examineService = examineService;
			_examineSearchProviderWrapper = examineSearchProviderWrapper;
			_modelConverter = modelConverter;
			_doubleFormatter = doubleFormatter;
			_intFormatter = intFormatter;
		}

		public IEnumerable<EventViewModel> GetEvents(GetEventsConfiguration configuration)
		{
			var searcher = _examineSearchProviderWrapper.EventSearcher();

			var query = searcher.CreateSearchCriteria(BooleanOperation.And).Field("__NodeTypeAlias", "event");

			// TODO: Make IFilters and loop through them to build query

			if (configuration.EarliestDate.HasValue && configuration.LatestDate.HasValue)
				query = query.And().Range("contentStartDateTime", configuration.EarliestDate.Value, configuration.LatestDate.Value);
			else if (configuration.EarliestDate.HasValue)
				query = query.And().Range("contentStartDateTime", configuration.EarliestDate.Value, DateTime.MaxValue);
			else if (configuration.LatestDate.HasValue)
				query = query.And().Range("contentStartDateTime", DateTime.MinValue, configuration.LatestDate.Value);

			var results = _examineService.Search(searcher, query.Compile());

			return _modelConverter.ToModel<EventViewModel>(results);
		}

		public IEnumerable<VenueViewModel> GetVenueEvents(GetEventsConfiguration configuration)
		{
			var searcher = _examineSearchProviderWrapper.EventSearcher();

			var query = searcher.CreateSearchCriteria(BooleanOperation.And).Field("__NodeTypeAlias", "event");

			// TODO: Make IFilters and loop through them to build query

			if (configuration.EarliestDate.HasValue && configuration.LatestDate.HasValue)
				query = query.And().Range("contentStartDateTime", configuration.EarliestDate.Value.ToIsoString(), configuration.LatestDate.Value.ToIsoString());
			else if (configuration.EarliestDate.HasValue)
				query = query.And().Range("contentStartDateTime", configuration.EarliestDate.Value.ToIsoString(), DateTime.MaxValue.ToIsoString());
			else if (configuration.LatestDate.HasValue)
				query = query.And().Range("contentStartDateTime", DateTime.MinValue.ToIsoString(), configuration.LatestDate.Value.ToIsoString());

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
				query = query.And().GroupedOr(new[] { "country", "city" },
					   _intFormatter.Format(configuration.LocationId.Value));
			}

			var results = _examineService.Search(searcher, query.Compile());

			return results.GroupBy(x => x.Parent.Id).Select(ToVenueViewModel);
		}

		private VenueViewModel ToVenueViewModel(IEnumerable<IPublishedContent> events)
		{
			var venue = _modelConverter.ToModel<VenueViewModel>(events.First().Parent);
			venue.Events = _modelConverter.ToModel<EventViewModel>(events);
			return venue;
		}
	}
}