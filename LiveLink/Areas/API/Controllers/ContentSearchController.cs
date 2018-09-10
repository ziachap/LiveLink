using System.Web.Mvc;
using LiveLink.Services.ContentSearchService;
using LiveLink.Services.ContentSearchService.Models;
using LiveLink.Services.Models;
using Newtonsoft.Json;

namespace LiveLink.Areas.API.Controllers
{
	public class ContentSearchController : Controller
	{
		private readonly IContentSearchService _contentSearchService;

		public ContentSearchController(IContentSearchService contentSearchService)
		{
			_contentSearchService = contentSearchService;
		}

		// GET: API/ContentSearch
		public object Index(string text)
		{
			var configuration = new ContentSearchConfiguration(text, 10);

			var results = _contentSearchService.Search(configuration);

			return JsonConvert.SerializeObject(new ApiSuccessResponse(results));
		}
	}
}