using System.Web.Mvc;
using LiveLink.Services.EventImportService;
using LiveLink.Services.FacebookEventsService;
using Newtonsoft.Json;

namespace LiveLink.Areas.API.Controllers
{
	public class ImportFacebookEventsController : Controller
	{
		private readonly IEventImportService _eventImportService;
		private readonly IFacebookEventsService _facebookEventsService;

		public ImportFacebookEventsController(IFacebookEventsService facebookEventsService,
			IEventImportService eventImportService)
		{
			_facebookEventsService = facebookEventsService;
			_eventImportService = eventImportService;
		}

		// GET: API/ImportFacebookEvents
		public string Index(int? limit)
		{
			var events = _facebookEventsService.GetEventsForVenues(limit);
			_eventImportService.SaveEvents(events);

			return JsonConvert.SerializeObject(events);
		}
	}
}