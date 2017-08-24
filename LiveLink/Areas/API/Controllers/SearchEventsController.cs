using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Gibe.DittoServices.ModelConverters;
using Gibe.UmbracoWrappers;
using LiveLink.Services.EventSearchService;
using LiveLink.Services.Extensions;
using LiveLink.Services.Models;
using LiveLink.Services.Models.ViewModels;
using Newtonsoft.Json;
using Our.Umbraco.Ditto;
using Umbraco.Core.Models;

namespace LiveLink.Areas.API.Controllers
{
    public class SearchEventsController : Controller
    {
		private readonly IEventSearchService _eventSearchService;
		private readonly IModelConverter _modelConverter;
	    private readonly IUmbracoWrapper _umbracoWrapper;

		public SearchEventsController(IEventSearchService eventSearchService, 
			IModelConverter modelConverter, IUmbracoWrapper umbracoWrapper)
		{
			_eventSearchService = eventSearchService;
			_modelConverter = modelConverter;
			_umbracoWrapper = umbracoWrapper;
		}
		
		public object MapEvents(GetEventsConfiguration configuration)
		{
			var results = _eventSearchService.GetVenueEvents(configuration);
			var groupedResults = results.GroupBy(x => x.Parent.Id).Select(ToVenueViewModel).ToList();

			// TODO: Generic API Response type
			
			return JsonConvert.SerializeObject(new ApiSuccessResponse(groupedResults));
		}

		public object FeedEvents(GetEventsConfiguration configuration)
		{
			var results = _eventSearchService.GetVenueEvents(configuration);
			var groupedResults = ToFeedViewModel(results, configuration);

			// TODO: Generic API Response type

			return JsonConvert.SerializeObject(new ApiSuccessResponse(groupedResults));
		}
		
		private VenueViewModel ToVenueViewModel(IEnumerable<IPublishedContent> events)
		{
			var venue = _modelConverter.ToModel<VenueViewModel>(events.First().Parent);
			venue.Events = _modelConverter.ToModel<EventViewModel>(events);
			return venue;
		}

	    private FeedViewModel ToFeedViewModel(IEnumerable<IPublishedContent> events, GetEventsConfiguration configuration)
		{
			const int featuredEventsCount = 3;
			const int subFeaturedEvents = 6;

			var typedEvents = events.ToModel<EventViewModel>().OrderBy(x => x.StartTime);

			var locationContent = _umbracoWrapper.TypedContent(configuration.LocationId.Value);
			var viewModel = _modelConverter.ToModel<FeedViewModel>(locationContent);
			viewModel.FeaturedEvents = typedEvents.Take(featuredEventsCount);
			viewModel.SubFeaturedEvents = typedEvents.Skip(featuredEventsCount).Take(subFeaturedEvents);
			viewModel.Events = typedEvents.Skip(featuredEventsCount + subFeaturedEvents);

			return viewModel;
		}
	}

	public class FeedViewModel
	{
		[UmbracoProperty("contentTitle")]
		public string LocationTitle { get; set; }

		[DittoIgnore]
		public IEnumerable<EventViewModel> FeaturedEvents { get; set; }

		[DittoIgnore]
		public IEnumerable<EventViewModel> SubFeaturedEvents { get; set; }

		[DittoIgnore]
		public IEnumerable<EventViewModel> Events { get; set; }
	}
}