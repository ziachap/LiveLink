using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

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

        public bool Success => true;

        public object Data { get; }
    }

    public class ApiFailureResponse : IApiResponse
    {
        public ApiFailureResponse(string message)
        {
            Message = message;
        }

        public bool Success => false;

        public string Message { get; }
    }
}