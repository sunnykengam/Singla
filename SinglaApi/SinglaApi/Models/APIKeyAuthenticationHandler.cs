using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace SinglaApi.Models
{
    public class APIKeyAuthenticationHandler : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage httpRequestMessage, CancellationToken cancellationToken)

        {
            string apiKeyToCheck = ConfigurationManager.AppSettings["ServiceApiKey"];
            string noKeyErrorMessage = "Key not found";
            string errorMessage = "API key validation failed";
            bool isvalid = false;
            bool isKeyExists = false;
            var queryStrings = httpRequestMessage.GetQueryNameValuePairs();
            if (queryStrings.Count() > 0)
            {
                var match = queryStrings.FirstOrDefault(i => string.Compare(i.Key, "Key", true) == 0);
                if (!string.IsNullOrEmpty(match.Key))
                {
                    isKeyExists = true;
                }
                if (!string.IsNullOrEmpty(match.Value))
                {
                    var base64EncodedBytes = System.Convert.FromBase64String(match.Value);
                    string encodekey = System.Text.Encoding.UTF8.GetString(base64EncodedBytes);

                    if (encodekey.Equals(apiKeyToCheck))
                    {
                        isvalid = true;
                    }
                }
            }
            if (!isKeyExists)
            {
                return httpRequestMessage.CreateResponse(System.Net.HttpStatusCode.Forbidden, noKeyErrorMessage);
            }
            if (!isvalid)
            {
                return httpRequestMessage.CreateResponse(System.Net.HttpStatusCode.Forbidden, noKeyErrorMessage);
            }
            var response = await base.SendAsync(httpRequestMessage, cancellationToken);
            return response;
        }
    }
}