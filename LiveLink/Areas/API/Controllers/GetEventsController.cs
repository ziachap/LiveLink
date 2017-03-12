using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using LiveLink.Services.EventSearchService;
using LiveLink.Services.Models.ViewModels;

namespace LiveLink.Areas.API
{
    public class GetEventsController : Controller
    {
	    private readonly IEventSearchService _eventSearchService;

	    public GetEventsController(IEventSearchService eventSearchService)
	    {
		    _eventSearchService = eventSearchService;
	    }

	    // GET: API/GetEvents
        public object Index(GetEventsConfiguration configuration)
        {
	        var results = _eventSearchService.GetEvents(configuration);
			

			return string.Join(", ", results.Select(x => x.Title));
        }
    }
}