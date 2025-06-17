using System.Net.Http;

namespace WinPrintLimiter.PrintControl
{
    internal class RemoteUserContext : UserContextBase
    {
        private const string ApiPath = "/PrintLimitApi";
        HttpClient ApiClient;

        string Endpoint;
        string Username;

        internal SharedInt GlobalJobsCounter = new SharedInt(0);
        internal SharedInt MaxGlobal;

        internal RemoteUserContext(string ApiEndpoint)
        {
            ApiClient = new HttpClient();
            Endpoint = ApiEndpoint + ApiPath;
            Username = Environment.UserName;

        }

        public void SetLimitslLimit(int current, int limit)
        {
            MaxGlobal = new SharedInt(limit);
        }


        public async Task<string> ServerHello()
        {
            Console.WriteLine(Username);
            //  HttpResponseMessage response = await ApiClient.SendAsync(new HttpRequestMessage(HttpMethod.Get,$"{Endpoint}/hello"));
            var data = new MultipartFormDataContent
            {
                { new StringContent(Username), "username" }
            };

            HttpResponseMessage response = await ApiClient.PostAsync($"{Endpoint}/hello", data);
            return await response.Content.ReadAsStringAsync();

        }

        public async Task<string> IncrementCurrentAmount(int amount)
        {
            var data = new MultipartFormDataContent
            {
                { new StringContent(Username), "username" },
                { new StringContent(amount.ToString()),"amount" }
            };

            HttpResponseMessage response = await ApiClient.PostAsync($"{Endpoint}/incrementcount", data);
            return await response.Content.ReadAsStringAsync();
        }


        internal override string GetCurrentUsername()
        {
            throw new NotImplementedException();
        }
    }
}
