using System.Threading.Tasks;
using ASE.ScienceDirectExtractor.ElsevierSearchApi;

namespace ASE.ScienceDirectExtractor.Console.Services
{
	public interface IPublicationSearchService
	{
		Task<ScienceDirectSearchResult> GetPublicationsAsync(string keyWord, int entryNumber);
	}
}