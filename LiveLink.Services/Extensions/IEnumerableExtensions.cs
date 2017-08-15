using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Gibe.DittoServices.ModelConverters;
using Umbraco.Core.Models;

namespace LiveLink.Services.Extensions
{
	public static class IEnumerableExtensions
	{
		public static IEnumerable<T> ToModel<T>(this IEnumerable<IPublishedContent> content) where T : class
		{
			return DependencyResolver.Current.GetService<IModelConverter>().ToModel<T>(content);
		}
	}
}
