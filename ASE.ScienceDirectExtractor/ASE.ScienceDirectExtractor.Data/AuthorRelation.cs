using System;
using System.Collections.Generic;

namespace ASE.ScienceDirectExtractor.Data
{
	public class AuthorRelation
	{
        public long Id { get; set; }
        public long Author1ID { get; set; }
        public long Author2ID { get; set; }
        public List<long> Years { get; set; }
		public bool SameAffiliationCity { get; set; }

		public AuthorRelation(long id, long author1Id, long author2Id, bool sameAffiliationCity)
	    {
            Id = id;
	        Author1ID = author1Id;
	        Author2ID = author2Id;
			SameAffiliationCity = sameAffiliationCity;

            Years = new List<long>();
        }
    }
}
