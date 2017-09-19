using System;
using System.Collections.Generic;
using System.Linq;
using Examine.Providers;
using Examine.SearchCriteria;
using Gibe.UmbracoWrappers;
using Umbraco.Core.Models;

namespace LiveLink.Services.ExamineService
{
	public class ExamineService : IExamineService
	{
		private readonly IUmbracoWrapper _umbracoWrapper;

		public ExamineService(IUmbracoWrapper umbracoWrapper)
		{
			_umbracoWrapper = umbracoWrapper;
		}

		// TODO: Return some sort of pagination type
		public IEnumerable<IPublishedContent> Search(BaseSearchProvider searcher,
			ISearchCriteria criteria, int? page, int? itemsPerPage)
		{
			if (IsPaginated(page, itemsPerPage))
			{
				return searcher.Search(criteria, page.Value * itemsPerPage.Value)
					.Select(x => _umbracoWrapper.TypedContent(x.Id))
					.Skip((page.Value - 1) * itemsPerPage.Value);
			}
			
			return _umbracoWrapper.TypedSearch(criteria, searcher);
		}

		private bool IsPaginated(int? page, int? itemsPerPage)
		{
			if (page.HasValue)
			{
				if (itemsPerPage.HasValue)
				{
					return true;
				}
				else
				{
					throw new Exception("Page specified but number of items per page not specified");
				}
			}
			return false;
		}
	}
}