
using Newtonsoft.Json;

namespace ASE.ScienceDirectExtractor.ElsevierSearchApi
{
    public class ScienceDirectSearchResult
    {
        [JsonProperty("search-results")]
        public SearchResults searchresults { get; set; }
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
        public string dcidentifier { get; set; }
        public string eid { get; set; }
        public string prismurl { get; set; }

        [JsonProperty("dc:title")]
        public string dctitle { get; set; }
        public string dccreator { get; set; }

        [JsonProperty("prism:publicationName")]
        public string prismpublicationName { get; set; }
        public string prismisbn { get; set; }
        public string prismissueIdentifier { get; set; }

        [JsonProperty("prism:coverDate")]
        public PrismCoverdate[] prismcoverDate { get; set; }

        [JsonProperty("prism:coverDisplayDate")]
        public string prismcoverDisplayDate { get; set; }
        public string prismstartingPage { get; set; }
        public string prismendingPage { get; set; }

        [JsonProperty("prism:doi")]
        public string prismdoi { get; set; }
        public string openaccess { get; set; }
        public bool openaccessArticle { get; set; }
        public bool openArchiveArticle { get; set; }
        public string openaccessUserLicense { get; set; }
        public string pii { get; set; }
        public Authors authors { get; set; }
        public string prismteaser { get; set; }
        public string prismedition { get; set; }
        public string prismissn { get; set; }
        public string prismvolume { get; set; }
        public string prismissueName { get; set; }
        public string pubType { get; set; }
    }

    public class Authors
    {
        public Author[] author { get; set; }
    }

    public class Author
    {
        public string _fa { get; set; }
        [JsonProperty("given-name")]
        public string givenname { get; set; }
        public string surname { get; set; }
    }

    public class Link1
    {
        public string _fa { get; set; }
        public string href { get; set; }
        public string _ref { get; set; }
    }

    public class PrismCoverdate
    {
        [JsonProperty("@_fa")]
        public string _fa { get; set; }
        [JsonProperty("$")]
        public string _ { get; set; }
    }


}
