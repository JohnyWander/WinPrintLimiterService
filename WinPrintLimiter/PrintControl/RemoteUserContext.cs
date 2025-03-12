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
        HttpClient ApiClient;

        string Endpoint;

        internal RemoteUserContext(string ApiEndpoint)
        {
            ApiClient = new HttpClient();
            Endpoint = ApiEndpoint;
            ///g
        }


        public async Task TryRegister()
        {
            HttpResponseMessage response = await ApiClient.SendAsync(new HttpRequestMessage(HttpMethod.Get,$"{Endpoint}/ok"));
            Console.WriteLine(await response.Content.ReadAsStringAsync());
            
        }



        internal override string GetCurrentUsername()
        {
            throw new NotImplementedException();
        }
    }
}
