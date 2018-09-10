namespace LiveLink.Services.IndexFormatters
{
	public interface IIndexFormatter<in T>
	{
		string Format(T value);
	}
}