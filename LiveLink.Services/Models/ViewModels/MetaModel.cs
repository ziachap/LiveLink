using LiveLink.Services.Processors;
using Our.Umbraco.Ditto;

namespace LiveLink.Services.Models.ViewModels
{
	public class MetaModel
	{
		[UmbracoProperty("metaTitle")] public string Title { get; set; }

		[UmbracoProperty("metaDescription")] public string Description { get; set; }

		[CanonicalUrl] public string CanonicalUrl { get; set; }
	}
}