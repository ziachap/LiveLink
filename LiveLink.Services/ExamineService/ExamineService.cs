using System.Collections.Generic;
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

		public IEnumerable<IPublishedContent> Search(BaseSearchProvider searcher,
			ISearchCriteria criteria)
		{
			return _umbracoWrapper.TypedSearch(criteria, searcher);
		}
	}
}