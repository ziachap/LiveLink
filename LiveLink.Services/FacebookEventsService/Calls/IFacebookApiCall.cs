
namespace LiveLink.Services.FacebookEventsService.Calls
{
	public interface IFacebookApiCall<in TConfiguration, out TResponse>
	{
		TResponse Call(TConfiguration config);
	}
}