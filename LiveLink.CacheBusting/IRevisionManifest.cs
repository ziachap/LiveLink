namespace LiveLink.CacheBusting
{
	public interface IRevisionManifest
	{
		bool ContainsPath(string path);
		string GetHashedPath(string original);
	}
}