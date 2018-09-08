using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Gibe.DittoServices.ModelConverters;
using LiveLink.Services.EventSearchService;
using LiveLink.Services.Extensions;
using LiveLink.Services.Models;
using LiveLink.Services.Models.ViewModels;
using Newtonsoft.Json;
using Umbraco.Core.Models;

namespace LiveLink.Areas.API.Controllers
{
	public class SearchEventsController : Controller
	{
		private readonly IEventSearchService _eventSearchService;
		private readonly IModelConverter _modelConverter;

		public SearchEventsController(IEventSearchService eventSearchService,
			IModelConverter modelConverter)
		{
			_eventSearchService = eventSearchService;
			_modelConverter = modelConverter;
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
			var typedResults = results.ToModel<EventViewModel>();

			// TODO: Generic API Response type
			return JsonConvert.SerializeObject(new ApiSuccessResponse(typedResults));
		}

		private VenueViewModel ToVenueViewModel(IEnumerable<IPublishedContent> events)
		{
			var venue = _modelConverter.ToModel<VenueViewModel>(events.First().Parent);
			venue.Events = _modelConverter.ToModel<EventViewModel>(events);
			return venue;
		}
	}
}