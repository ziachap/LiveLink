using System.Collections.Generic;
using LiveLink.Services.Models.ViewModels;

namespace LiveLink.Services.EventSearchService
{
	public interface IEventSearchService
	{
		IEnumerable<EventViewModel> GetEvents(GetEventsConfiguration configuration);
		IEnumerable<VenueViewModel> GetVenueEvents(GetEventsConfiguration configuration);
	}
}