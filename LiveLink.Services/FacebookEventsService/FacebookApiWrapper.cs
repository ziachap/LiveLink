using System;
using System.Linq;
using System.Linq.Expressions;
using Gibe.UmbracoWrappers;
using Skybrud.Social.Facebook;
using Skybrud.Social.Umbraco.Facebook.PropertyEditors.OAuth;
using Umbraco.Core.Logging;
using Umbraco.Core.Models;

namespace LiveLink.Services.FacebookEventsService
{
	public class FacebookApiWrapper : IFacebookApiWrapper
	{
		private readonly IUmbracoWrapper _umbracoWrapper;

		public FacebookApiWrapper(IUmbracoWrapper umbracoWrapper)
		{
			_umbracoWrapper = umbracoWrapper;
		}

		public T Execute<T>(Expression<Func<FacebookService, T>> request)
		{
			try
			{
				return request.Compile().Invoke(Service(AuthData()));
			}
			catch (Exception ex)
			{
				LogHelper.Error<FacebookApiWrapper>("Facebook API request failed", ex);
				return default(T);
			}
		}

		public FacebookService Service(FacebookOAuthData authenticationData)
		{
			return FacebookService.CreateFromAccessToken(authenticationData.AccessToken);
		}

		private FacebookOAuthData AuthData()
		{
			return _umbracoWrapper.GetPropertyValue<FacebookOAuthData>(Settings(), "settingsFacebookOAuth");
		}

		private IPublishedContent Settings()
		{
			return _umbracoWrapper.TypedContentAtRoot().First(x => x.DocumentTypeAlias.Equals("settings"));
		}
	}
}