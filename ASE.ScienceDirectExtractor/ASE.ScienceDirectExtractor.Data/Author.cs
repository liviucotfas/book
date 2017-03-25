using System.Collections.Generic;

namespace ASE.ScienceDirectExtractor.Data
{
	public class AuthorEx
	{
		public long Id { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }

        public List<Publication> Publications { get; set; }

		public SearchAuthorApi.Entry AuthorFromSearchApi { get; private set; }

		public AuthorEx(long id, string firstName, string lastName, SearchAuthorApi.Entry authorFromSearchApi)
	    {
	        Id = id;
	        FirstName = firstName;
	        LastName = lastName;

            Publications = new List<Publication>();
			AuthorFromSearchApi = authorFromSearchApi;
	    }
    }
}
