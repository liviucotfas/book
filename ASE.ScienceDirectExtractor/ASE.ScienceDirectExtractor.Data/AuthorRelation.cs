using System.Collections.Generic;

namespace ASE.ScienceDirectExtractor.Data
{
	public class AuthorRelation
	{
        public long Id { get; set; }
        public long Author1Id { get; set; }
        public long Author2Id { get; set; }
        public List<long> Years { get; set; }
		public bool SameAffiliationCity { get; set; }

		public AuthorRelation(long id, long author1Id, long author2Id, bool sameAffiliationCity)
	    {
            Id = id;
	        Author1Id = author1Id;
	        Author2Id = author2Id;
			SameAffiliationCity = sameAffiliationCity;

            Years = new List<long>();
        }
    }
}
