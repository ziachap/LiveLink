using LiveLink.Services.ContentSearchService.Models;

namespace LiveLink.Services.ContentSearchService
{
	public interface IContentSearchService
	{
		ContentSearchResults Search(ContentSearchConfiguration config);
	}
}