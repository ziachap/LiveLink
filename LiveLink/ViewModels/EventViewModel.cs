using System;
using Our.Umbraco.Ditto;

namespace LiveLink.ViewModels
{
    public class EventViewModel
    {
        [UmbracoProperty("contentTitle")]
        public string Title { get; set; }

        [UmbracoProperty("contentSummary")]
        public string Summary { get; set; }

        [UmbracoProperty("contentDescription")]
        public string Description { get; set; }

        [UmbracoProperty("contentTicketUri")]
        public string TicketUri { get; set; }

        [UmbracoProperty("contentStartDateTime")]
        public DateTime StartTime { get; set; }

        [UmbracoProperty("contentEndDateTime")]
        public DateTime EndTime { get; set; }
    }
}