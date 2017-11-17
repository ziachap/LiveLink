using System.Globalization;
using System.Web.Mvc;
using Gibe.DittoServices.ModelConverters;
using Gibe.UmbracoWrappers;
using LiveLink.Services.Models.ViewModels;
using Umbraco.Web.Models;
using Umbraco.Web.Mvc;

namespace LiveLink.Controllers
{
	public class VenueController : RenderMvcController
	{
		public void Index(RenderModel model)
		{
			var venue = model.Content;
			var city = venue.Parent;
			var country = city.Parent;

			var queryString = $"countryId={country.Id}&cityId={city.Id}&venueId={venue.Id}";

			Response.RedirectPermanent("/feed?" + queryString);
		}
	}
}