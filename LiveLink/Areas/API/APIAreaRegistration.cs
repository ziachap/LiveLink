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
            // TODO: Don't think there's any benefit for manual routing in the API area, auto routing instead?

            context.MapRoute("ImportFacebookEvents", "API/import-facebook-events",
                new { controller = "ImportFacebookEvents", action = "Index", id = UrlParameter.Optional });

			context.MapRoute("ImportFacebookVenueEvents", "API/import-facebook-venue-events",
				new { controller = "ImportFacebookVenueEvents", action = "Index" });

			context.MapRoute("CleanupEvents", "API/cleanup-events",
				new { controller = "CleanupEvents", action = "Index" });

			context.MapRoute("CleanupEventsDebug", "API/cleanup-events-debug",
				new { controller = "CleanupEvents", action = "Debug" });

			context.MapRoute("AjaxEvent", "API/event",
				new { controller = "AjaxEvent", action = "Index", id = UrlParameter.Optional });

			context.MapRoute("GetEvents", "API/events",
				new { controller = "GetEvents", action = "Index", id = UrlParameter.Optional });

			context.MapRoute("SearchEventsMap", "API/search-events/map",
				new { controller = "SearchEvents", action = "MapEvents"});
			
			context.MapRoute("SearchEventsFeed", "API/search-events/feed",
				new { controller = "SearchEvents", action = "FeedEvents" });

			context.MapRoute("User", "API/user/{action}",
				new { controller = "User" });

			context.MapRoute("ContentSearch", "API/content-search",
				new { controller = "ContentSearch", action = "Index", text = UrlParameter.Optional });
		}
    }
}