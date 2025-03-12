using System;
using System.Collections.Generic;
using System.Drawing.Text;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace WinPrintLimiter.PrintControl
{
    internal class RemoteUserContext : UserContextBase
    {
        private const string ApiPath ="/PrintLimitApi";
        HttpClient ApiClient;

        string Endpoint;
        string Username;

        internal RemoteUserContext(string ApiEndpoint)
        {
            ApiClient = new HttpClient();
            Endpoint = ApiEndpoint+ApiPath;
            Username = Environment.UserName;
        }


        public async Task<string> ServerHello()
        {
            
          //  HttpResponseMessage response = await ApiClient.SendAsync(new HttpRequestMessage(HttpMethod.Get,$"{Endpoint}/hello"));
            var data = new MultipartFormDataContent
            {
                { new StringContent("username"), Username }
            };

            HttpResponseMessage response = await ApiClient.PostAsync($"{Endpoint}/hello",data);
            return await response.Content.ReadAsStringAsync();
            
        }



        internal override string GetCurrentUsername()
        {
            throw new NotImplementedException();
        }
    }
}
