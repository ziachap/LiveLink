using System;
using Gibe.DittoServices.ModelConverters;
using Umbraco.Core.Models;

namespace LiveLink.Services.Processors
{
	public class ModelAttribute : InjectableProcessorAttribute
	{
		private readonly Type _type;

		public ModelAttribute(Type type)
		{
			_type = type;
		}

		public Func<IModelConverter> ModelConverter => Inject<IModelConverter>();
		public override object ProcessValue()
		{
			if (!(Value is IPublishedContent)) return null;

			return ModelConverter().ToModel(_type, (IPublishedContent) Value);
		}
	}
}
