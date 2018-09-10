namespace LiveLink.Services.Models
{
	public interface IApiResponse
	{
		bool Success { get; }
	}

	public class ApiSuccessResponse : IApiResponse
	{
		public ApiSuccessResponse(object data)
		{
			Data = data;
		}

		public object Data { get; }

		public bool Success => true;
	}

	public class ApiFailureResponse : IApiResponse
	{
		public ApiFailureResponse(string message)
		{
			Message = message;
		}

		public string Message { get; }

		public bool Success => false;
	}
}