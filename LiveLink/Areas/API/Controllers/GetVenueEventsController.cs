using System.Web.Mvc;
using LiveLink.Services.EventSearchService;
using LiveLink.Services.Models;
using Newtonsoft.Json;

namespace LiveLink.Areas.API.Controllers
{
    public class GetVenueEventsController : Controller
    {
		private readonly IEventSearchService _eventSearchService;

		public GetVenueEventsController(IEventSearchService eventSearchService)
		{
			_eventSearchService = eventSearchService;
		}

		// GET: API/GetVenueEvents
		public object Index(GetEventsConfiguration configuration)
		{
			var results = _eventSearchService.GetVenueEvents(configuration);

			// TODO: Generic API Response type

			return JsonConvert.SerializeObject(new ApiSuccessResponse(results));
		}
	}
}