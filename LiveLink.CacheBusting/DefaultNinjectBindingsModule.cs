using Gibe.FileSystem;
using Ninject.Modules;

namespace LiveLink.CacheBusting
{
	public class DefaultNinjectBindingsModule : NinjectModule
	{
		public override void Load()
		{
			Bind<IRevisionManifest>().To<RevisionManifest>().InSingletonScope();
			Bind<IManifestFileFactory>().To<ConfigManifestFileFactory>();
			Bind<IFileService>().To<FileService>();
			Bind<IPaths>().To<Paths>();
		}
	}
}