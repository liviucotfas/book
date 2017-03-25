using System;
using System.Net.Http;
using System.Threading.Tasks;
using ASE.ScienceDirectExtractor.ElsevierSearchApi;
using Newtonsoft.Json;

namespace ASE.ScienceDirectExtractor.Console.Services
{
	public class PublicationSearchService
	{
		private readonly string _elsevierApiKey;

		public PublicationSearchService(string elsevierApiKey)
		{
			_elsevierApiKey = elsevierApiKey;
		}

		public async Task<ScienceDirectSearchResult> GetPublicationsAsync(string keyWord, int entryNumber)
		{
			var httpClient = new HttpClient();
			httpClient.DefaultRequestHeaders.Add("X-ELS-APIKey", _elsevierApiKey);

			var uri = new Uri(
				$"http://api.elsevier.com/content/search/scidir?query=keywords%28{Uri.EscapeDataString(keyWord)}%29&start={entryNumber}&count={25}");
			var requestResult = await httpClient.GetAsync(uri);
			var resultString = await requestResult.Content.ReadAsStringAsync();
			return JsonConvert.DeserializeObject<ScienceDirectSearchResult>(resultString);
		}
	}
}
