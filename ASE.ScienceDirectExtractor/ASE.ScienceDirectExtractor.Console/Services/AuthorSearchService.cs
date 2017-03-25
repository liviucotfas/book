using System;
using System.Net.Http;
using System.Threading.Tasks;
using ASE.ScienceDirectExtractor.SearchAuthorApi;
using Newtonsoft.Json;

namespace ASE.ScienceDirectDataExtractor.Console.Services
{
    public class AuthorSearchService
    {
        //private HttpClient _httpClient ;

        public AuthorSearchService()
        {
          
        }

        public async Task<SearchAuthorApiResult> SearchAuthorAsync(string firstName, string lastName)
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("X-ELS-APIKey", "ed8c0431fc17c1553bc48fc094671d26");
            httpClient.DefaultRequestHeaders.Add("Origin", "http://www.ase.ro");
            httpClient.DefaultRequestHeaders.Add("Accept", "application/json");

            var encodedQuery = Uri.EscapeDataString($"authlast({lastName}) and authfirst({firstName})");

            var uri = new Uri($"http://api.elsevier.com/content/search/author?query={encodedQuery}");
            var requestResult = await httpClient.GetAsync(uri);
            var resultString = await requestResult.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<SearchAuthorApiResult>(resultString);
        }
    }
}
