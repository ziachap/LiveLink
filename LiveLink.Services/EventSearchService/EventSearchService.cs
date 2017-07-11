using System;
using System.Collections.Generic;
using System.Linq;
using Examine.SearchCriteria;
using Gibe.DittoServices.ModelConverters;
using LiveLink.Services.ExamineService;
using LiveLink.Services.Models.ViewModels;
using Umbraco.Core.Models;

namespace LiveLink.Services.EventSearchService
{
	public class EventSearchService : IEventSearchService
	{
		private readonly IExamineSearchProviderWrapper _examineSearchProviderWrapper;
		private readonly IExamineService _examineService;
		private readonly IModelConverter _modelConverter;

		public EventSearchService(IExamineService examineService, IExamineSearchProviderWrapper examineSearchProviderWrapper,
			IModelConverter modelConverter)
		{
			_examineService = examineService;
			_examineSearchProviderWrapper = examineSearchProviderWrapper;
			_modelConverter = modelConverter;
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
				query = query.And().Range("contentStartDateTime", configuration.EarliestDate.Value, configuration.LatestDate.Value);
			else if (configuration.EarliestDate.HasValue)
				query = query.And().Range("contentStartDateTime", configuration.EarliestDate.Value, DateTime.MaxValue);
			else if (configuration.LatestDate.HasValue)
				query = query.And().Range("contentStartDateTime", DateTime.MinValue, configuration.LatestDate.Value);

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