namespace LiveLink.Services.ContentSearchService.Models
{
	public class ContentSearchConfiguration
	{
		public ContentSearchConfiguration(string text, int limit)
		{
			Text = text;
			Limit = limit;
		}

		public string Text { get; }

		public int Limit { get; }
	}
}
