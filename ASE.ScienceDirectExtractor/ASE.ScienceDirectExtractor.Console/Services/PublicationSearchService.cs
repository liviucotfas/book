using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ASE.ScienceDirectExtractor.ElsevierSearchApi;
using Newtonsoft.Json;

namespace ASE.ScienceDirectDataExtractor.Console.Services
{
	public class PublicationSearchService
	{
		public async Task<ScienceDirectSearchResult> GetPublicationsAsync(int entryNumber)
		{
			var httpClient = new HttpClient();
			httpClient.DefaultRequestHeaders.Add("X-ELS-APIKey", "ed8c0431fc17c1553bc48fc094671d26");

			var uri = new Uri(
						string.Format(
							"http://api.elsevier.com/content/search/scidir?query=keywords%28social+media%29&start={0}&count={1}",
							entryNumber,
							25));
			var requestResult = await httpClient.GetAsync(uri);
			var resultString = await requestResult.Content.ReadAsStringAsync();
			return JsonConvert.DeserializeObject<ScienceDirectSearchResult>(resultString);
		}
	}
}
