using Newtonsoft.Json;

namespace ASE.ScienceDirectExtractor.SearchAuthorApi
{
	public class SearchAuthorApiResult
    {
        [JsonProperty("search-results")]
        public SearchResults SearchResults { get; set; }
    }
    public class SearchResults
    {
        public string opensearchtotalResults { get; set; }
        public string opensearchstartIndex { get; set; }
        public string opensearchitemsPerPage { get; set; }
        public OpensearchQuery opensearchQuery { get; set; }
        public Link[] link { get; set; }
        public Entry[] entry { get; set; }
    }

    public class OpensearchQuery
    {
        public string role { get; set; }
        public string searchTerms { get; set; }
        public string startPage { get; set; }
    }

    public class Link
    {
        public string _fa { get; set; }
        public string href { get; set; }
        public string _ref { get; set; }
        public string type { get; set; }
    }

    public class Entry
    {
        public string _fa { get; set; }
        public Link1[] link { get; set; }

        [JsonProperty("prism:url")]
        public string prismurl { get; set; }

        public string dcidentifier { get; set; }
        public string eid { get; set; }

        [JsonProperty("preferred-name")]
        public PreferredName preferredname { get; set; }

        public NameVariant[] namevariant { get; set; }

        [JsonProperty("document-count")]
        public string documentcount { get; set; }

        [JsonProperty("subject-area")]
        public SubjectArea[] subjectarea { get; set; }

        [JsonProperty("affiliation-current")]
        public AffiliationCurrent affiliationcurrent { get; set; }
        public string orcid { get; set; }
    }

    public class PreferredName
    {
        public string surname { get; set; }

        [JsonProperty("given-name")]
        public string givenname { get; set; }

        public string initials { get; set; }
    }

    public class AffiliationCurrent
    {
        public string affiliationurl { get; set; }

        [JsonProperty("affiliation-id")]
        public string affiliationid { get; set; }

        [JsonProperty("affiliation-name")]
        public string affiliationname { get; set; }

        [JsonProperty("affiliation-city")]
        public string affiliationcity { get; set; }

        [JsonProperty("affiliation-country")]
        public string affiliationcountry { get; set; }
    }

    public class Link1
    {
        public string _fa { get; set; }
        public string href { get; set; }
        public string _ref { get; set; }
    }

    public class NameVariant
    {
        public string _fa { get; set; }
        public string surname { get; set; }
        public string initials { get; set; }
        public string givenname { get; set; }
    }

    public class SubjectArea
    {
        [JsonProperty("@abbrev")]
        public string abbrev { get; set; }


        public string forcearray { get; set; }

        [JsonProperty("@frequency")]
        public string frequency { get; set; }

        [JsonProperty("$")]
        public string _ { get; set; }
    }
}
