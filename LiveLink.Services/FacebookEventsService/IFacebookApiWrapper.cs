using System;
using System.Linq.Expressions;
using SkybrudFacebookService = Skybrud.Social.Facebook.FacebookService;

namespace LiveLink.Services.FacebookEventsService
{
	public interface IFacebookApiWrapper
	{
		T Execute<T>(Expression<Func<SkybrudFacebookService, T>> request);
	}
}