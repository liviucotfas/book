using System;
using System.Net.Http;
using System.Threading.Tasks;
using ASE.ScienceDirectExtractor.SearchAuthorApi;
using Newtonsoft.Json;

namespace ASE.ScienceDirectExtractor.Console.Services
{
    public class AuthorSearchService : IAuthorSearchService
    {
        private readonly string _elsevierApiKey;

        public AuthorSearchService(string elsevierApiKey)
        {
	        _elsevierApiKey = elsevierApiKey;
        }

        public async Task<SearchAuthorApiResult> SearchAuthorAsync(string firstName, string lastName)
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("X-ELS-APIKey", _elsevierApiKey);
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
