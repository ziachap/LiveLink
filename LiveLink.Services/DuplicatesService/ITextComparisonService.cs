namespace LiveLink.Services.DuplicatesService
{
	public interface ITextComparisonService
	{
		double PercentageSimilarity(string a, string b);
	}
}