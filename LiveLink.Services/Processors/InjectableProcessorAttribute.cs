using System;
using System.Web.Mvc;
using Gibe.DittoProcessors.Processors;

namespace LiveLink.Services.Processors
{
	public abstract class InjectableProcessorAttribute : TestableDittoProcessorAttribute
	{
		public bool ValueIsNull => string.IsNullOrEmpty(Value?.ToString());

		public Func<T> Inject<T>()
		{
			return () => DependencyResolver.Current.GetService<T>();
		}
	}
}