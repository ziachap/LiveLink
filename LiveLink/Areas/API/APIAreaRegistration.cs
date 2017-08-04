using System.Web.Mvc;

namespace LiveLink.Areas.API
{
    public class APIAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "API";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute("ImportFacebookEvents", "API/import-facebook-events",
                new { controller = "ImportFacebookEvents", action = "Index", id = UrlParameter.Optional });

			context.MapRoute("AjaxEvent", "API/event",
				new { controller = "AjaxEvent", action = "Index", id = UrlParameter.Optional });

			context.MapRoute("GetEvents", "API/events",
				new { controller = "GetEvents", action = "Index", id = UrlParameter.Optional });

			context.MapRoute("GetVenueEvents", "API/venue-events",
				new { controller = "GetVenueEvents", action = "Index", id = UrlParameter.Optional });

			context.MapRoute("User", "API/user/{action}",
				new { controller = "User" });

			context.MapRoute("ContentSearch", "API/content-search",
				new { controller = "ContentSearch", action = "Index", text = UrlParameter.Optional });
		}
    }
}