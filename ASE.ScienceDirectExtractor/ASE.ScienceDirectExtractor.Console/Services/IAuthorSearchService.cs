using System.Threading.Tasks;
using ASE.ScienceDirectExtractor.SearchAuthorApi;

namespace ASE.ScienceDirectExtractor.Console.Services
{
	public interface IAuthorSearchService
	{
		Task<SearchAuthorApiResult> SearchAuthorAsync(string firstName, string lastName);
	}
}