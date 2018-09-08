using Examine;
using Examine.Providers;

namespace LiveLink.Services.ExamineService
{
	public class ExamineSearchProviderWrapper : IExamineSearchProviderWrapper
	{
		public BaseSearchProvider EventSearcher()
		{
			return ExamineManager.Instance.SearchProviderCollection["EventSearcher"];
		}

		public BaseSearchProvider ContentSearcher()
		{
			return ExamineManager.Instance.SearchProviderCollection["ContentSearcher"];
		}
	}
}