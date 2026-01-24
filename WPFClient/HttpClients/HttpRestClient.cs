using RestSharp;
using Shared.Constants;
using Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WPFClient.HttpClients
{
    public class HttpRestClient
    {

        private readonly RestClient _client;
        public HttpRestClient() {
            _client = new RestClient("http://localhost:5204");
        }
        public CommonDTO<T> Execute<T>(ApiRequest apiRequest) where T : class
        {
            RestRequest request = new RestRequest(apiRequest.Route, apiRequest.Method);
            request.AddHeader("Content-Type", apiRequest.ContentType);
            if (apiRequest.Parameters != null)
            {
                request.AddJsonBody(apiRequest.Parameters);
            }
            var response = _client.Execute<CommonDTO<T>>(request);
            if (response.IsSuccessful && response.Data != null)
            {
                return response.Data;
            }
            return new CommonDTO<T> { Code = ErrorCode.UnknownError,
                Message = response.ErrorMessage};
        }
    }
}
