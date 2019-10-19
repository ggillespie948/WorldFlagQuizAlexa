using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace WorldFlagQuiz.AzureFunction.Utility
{
    public static class AlexaHttpUtility
    {
        public static async Task<string> MakeRequest(string WebApiAddress, string parameter)
        {
            var client = new HttpClient();
            var response = await client.GetAsync(WebApiAddress + parameter);

            return await response.Content.ReadAsStringAsync();
        }
    }
}
