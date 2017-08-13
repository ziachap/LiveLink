using System.Collections.Generic;
using LiveLink.Services.Processors;

namespace LiveLink.Services.Models.ViewModels
{
	public class FeedViewModel
	{
		[VenueEvents]
		public IEnumerable<VenueViewModel> Venues { get; set; }

		[Countries]
		public IEnumerable<CountryModel> Countries { get; set; }
	}
}