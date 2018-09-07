using System.Collections.Generic;
using Our.Umbraco.Ditto;

namespace LiveLink.Services.Models
{
    public class NavigationNode
    {
        [UmbracoProperty("name")]
        public string Title { get; set; }

        [UmbracoProperty("url")]
        public string Url { get; set; }

        [DittoIgnore]
        public bool Active { get; set; }

        [DittoIgnore]
        public IEnumerable<NavigationNode> Children { get; set; }
    }
}