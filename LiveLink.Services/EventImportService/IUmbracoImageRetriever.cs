namespace LiveLink.Services.EventImportService
{
	public interface IUmbracoImageRetriever
	{
		int? RetrieveAndSaveImage(string url, string filename);
	}
}