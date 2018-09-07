using System;
using System.Collections.Generic;
using System.Linq;
using Gibe.DittoProcessors.Media.Models;
using Gibe.DittoProcessors.Processors;
using LiveLink.Services.Processors;
using Our.Umbraco.Ditto;
using Umbraco.Core;

namespace LiveLink.Services.Models.ViewModels
{
    public class VenueViewModel
    {
        [UmbracoProperty("id")] public int Id { get; set; }

        [UmbracoProperty("contentTitle")] public string Title { get; set; }

        [UmbracoProperty("contentLatitude")]
        [TextToDecimal]
        public decimal Latitude { get; set; }

        [UmbracoProperty("contentLongitude")]
        [TextToDecimal]
        public decimal Longitude { get; set; }

        [UmbracoProperty("url")] public string Url { get; set; }

        [UmbracoProperty("contentLogo")]
        [ImagePickerOrDefaultImage]
        public MediaImageModel Logo { get; set; }

        //[Children("event")]
        public IEnumerable<EventViewModel> Events { get; set; }

        public bool HasMultipleEvents => Events.Count() > 1;
    }
}