using System;
using System.Web.Mvc;
using Gibe.DittoProcessors.Processors;

namespace LiveLink.Services.Processors
{
	public abstract class InjectableProcessorAttribute : TestableDittoProcessorAttribute
	{
		public Func<T> Inject<T>() => () => DependencyResolver.Current.GetService<T>();

		public bool ValueIsNull => string.IsNullOrEmpty(Value?.ToString());
	}
}
