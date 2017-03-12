using System.Web.Mvc;
using LiveLink.Services.EventImportService;
using LiveLink.Services.FacebookEventsService;
using Umbraco.Web.Models;
using Umbraco.Web.Mvc;

namespace LiveLink.Controllers
{
	public class EventController : RenderMvcController
	{
	    private readonly IFacebookEventsService _facebookEventsService;
	    private readonly IEventImportService _eventImportService;

	    public EventController(IFacebookEventsService facebookEventsService, IEventImportService eventImportService)
	    {
	        _facebookEventsService = facebookEventsService;
	        _eventImportService = eventImportService;
	    }

	    public override ActionResult Index(RenderModel model)
	    {
	        //var events = _facebookEventsService.GetEventsForVenues();
            //_eventImportService.SaveEvents(events);

            //Do some stuff here, then return the base method
            return base.Index(model);
        }

    }
}
