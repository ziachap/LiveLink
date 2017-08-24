using System.Collections.Generic;
using Gibe.DittoProcessors.Processors;
using Our.Umbraco.Ditto;

namespace LiveLink.Services.Models
{
	public class LocationOption
	{
		[UmbracoProperty("id")]
		public int Id { get; set; }

		[UmbracoProperty("contentTitle")]
		public string Title { get; set; }

		public bool Selected { get; set; }

		public bool Disabled { get; set; }
	}
}