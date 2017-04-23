using Examine.Providers;

namespace LiveLink.Services.ExamineService
{
	public interface IExamineSearchProviderWrapper
	{
		BaseSearchProvider EventSearcher();
	}
}
