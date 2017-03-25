using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASE.ScienceDirectExtractor.Data.ScopusSearchApi
{
    public class Rootobject
    {
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
        public string _ref { get; set; }
        public string href { get; set; }
        public string type { get; set; }
    }

    public class Entry
    {
        public string _fa { get; set; }
        public Link1[] link { get; set; }
        public string prismurl { get; set; }
        public string dcidentifier { get; set; }
        public string eid { get; set; }
        public string dctitle { get; set; }
        public string dccreator { get; set; }
        public string prismpublicationName { get; set; }
        public string prismeIssn { get; set; }
        public string prismvolume { get; set; }
        public string prismissueIdentifier { get; set; }
        public string prismcoverDate { get; set; }
        public string prismcoverDisplayDate { get; set; }
        public string prismdoi { get; set; }
        public string dcdescription { get; set; }
        public string citedbycount { get; set; }
        public Affiliation[] affiliation { get; set; }
        public string prismaggregationType { get; set; }
        public string subtype { get; set; }
        public string subtypeDescription { get; set; }
        public AuthorCount authorcount { get; set; }
        public Author[] author { get; set; }
        public string authkeywords { get; set; }
        public string intid { get; set; }
        public string articlenumber { get; set; }
        public string sourceid { get; set; }
        public string prismpageRange { get; set; }
        public string pii { get; set; }
        public string prismissn { get; set; }
    }

    public class AuthorCount
    {
        public string limit { get; set; }
        public string _ { get; set; }
    }

    public class Link1
    {
        public string _fa { get; set; }
        public string _ref { get; set; }
        public string href { get; set; }
    }

    public class Affiliation
    {
        public string _fa { get; set; }
        public string affiliationurl { get; set; }
        public string afid { get; set; }
        public string affilname { get; set; }
        public NameVariant[] namevariant { get; set; }
        public string affiliationcity { get; set; }
        public string affiliationcountry { get; set; }
    }

    public class NameVariant
    {
        public string _fa { get; set; }
        public string _ { get; set; }
    }

    public class Author
    {
        public string _fa { get; set; }
        public string seq { get; set; }
        public string authorurl { get; set; }
        public string authid { get; set; }
        public string authname { get; set; }
        public string surname { get; set; }
        public string givenname { get; set; }
        public string initials { get; set; }
        public Afid[] afid { get; set; }
    }

    public class Afid
    {
        public string _fa { get; set; }
        public string _ { get; set; }
    }

}
