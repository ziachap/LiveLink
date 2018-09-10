using System.Web.Mvc;
using Gibe.UmbracoWrappers;
using LiveLink.Services.EventImportService;
using LiveLink.Services.FacebookEventsService;
using Newtonsoft.Json;

namespace LiveLink.Areas.API.Controllers
{
	public class ImportFacebookVenueEventsController : Controller
	{
		private readonly IEventImportService _eventImportService;
		private readonly IFacebookEventsService _facebookEventsService;
		private readonly IUmbracoWrapper _umbracoWrapper;

		public ImportFacebookVenueEventsController(IFacebookEventsService facebookEventsService,
			IEventImportService eventImportService, IUmbracoWrapper umbracoWrapper)
		{
			_facebookEventsService = facebookEventsService;
			_eventImportService = eventImportService;
			_umbracoWrapper = umbracoWrapper;
		}

		// GET: API/ImportFacebookVenueEvents
		public string Index(int id, int? limit)
		{
			var venueContent = _umbracoWrapper.TypedContent(id);
			var events = _facebookEventsService.GetEventsForVenue(venueContent, limit);
			_eventImportService.SaveEvents(events);

			return JsonConvert.SerializeObject(events);
		}
	}
}