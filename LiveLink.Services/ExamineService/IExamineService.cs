using System.Collections.Generic;
using Examine.Providers;
using Examine.SearchCriteria;
using Umbraco.Core.Models;

namespace LiveLink.Services.ExamineService
{
	public interface IExamineService
	{
		IEnumerable<IPublishedContent> Search(BaseSearchProvider searcher, ISearchCriteria criteria, int? page,
			int? itemsPerPage);
	}
}