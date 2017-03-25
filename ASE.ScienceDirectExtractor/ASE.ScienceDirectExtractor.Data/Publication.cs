using System.Linq;
using ASE.ScienceDirectExtractor.ElsevierSearchApi;
using Newtonsoft.Json;

namespace ASE.ScienceDirectExtractor.Data
{
	public class Publication
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

		[JsonIgnore]
		public int PrismCoverDateFirstYear => int.Parse(prismcoverDate.First()._.Substring(0, 4));


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
}
