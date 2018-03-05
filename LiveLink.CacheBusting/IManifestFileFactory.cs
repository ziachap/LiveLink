using System.Collections.Generic;

namespace LiveLink.CacheBusting
{
	public interface IManifestFileFactory
	{
		IEnumerable<ManifestFile> GetManifestFiles();
	}
}