using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ASE.ScienceDirectExtractor.Console.Properties;
using ASE.ScienceDirectExtractor.Console.Services;
using ASE.ScienceDirectExtractor.Data;
using ASE.ScienceDirectExtractor.SearchAuthorApi;
using Newtonsoft.Json;
using Entry = ASE.ScienceDirectExtractor.ElsevierSearchApi.Entry;

namespace ASE.ScienceDirectExtractor.Console
{
	internal class Program
	{
		private static void Main()
		{
			//Settings
			const string keyWords = "social+media";
			const string authorsFile = "authors.csv";
			const string publicationsFile = "publications.csv";
			const string authorRealtionsFile = "authorRelations.csv";

			//AutoMapper Configuration
			AutoMapper.Mapper.CreateMap<Entry, Publication>();

			//Start the task
			Task.Run(async () =>
			{
				await FetchToCSVAsync(keyWords, authorsFile, publicationsFile, authorRealtionsFile);
			}).Wait();
		}

		private static async Task FetchToCSVAsync(string keyWords, string authorsFile, string publicationsFile, string authorRelationsFile)
		{
			var authorSearchService = new AuthorSearchService(Settings.Default.ElsevierAPIKey);
			var publicationsSearchService = new PublicationSearchService(Settings.Default.ElsevierAPIKey);

			try
			{
				long authorRelationId = 0;
				long authorId = 0;

				var publicationDictionary = new Dictionary<string, Publication>();
				var authorDictionary = new Dictionary<string, AuthorEx>();
				var authorRelationDictionary = new List<AuthorRelation>();

				//1478
				for (var entryNumber = 0; entryNumber < 1478; entryNumber = entryNumber + 25)
				{
					System.Console.WriteLine("start={0}&count={1}", entryNumber, 25);

					var publicationApiList = (await publicationsSearchService.GetPublicationsAsync(keyWords, entryNumber)).SearchResults.entry;

					foreach (var publicationApi in publicationApiList)
					{
						var publication = AutoMapper.Mapper.Map<Publication>(publicationApi);
						
						publicationDictionary.Add(publicationApi.prismdoi, publication);

						if (publicationApi.authors != null)
						{
							var authorApiList = publicationApi.authors.author;

							var authorTempList = new List<AuthorEx>();

							for (var i = 0; i < authorApiList.Length; i++)
							{
								var authorApi = authorApiList[i];

								//Get or create authorEX
								AuthorEx authorEx = null;
								if (!authorDictionary.ContainsKey(authorApi.surname + " " + authorApi.givenname))
								{
									SearchAuthorApiResult searchAuthorResult = null;
									try
									{
										searchAuthorResult = await authorSearchService.SearchAuthorAsync(authorApi.givenname, authorApi.surname);
									}
									catch (JsonSerializationException)
									{
										
									}

									if (searchAuthorResult?.SearchResults != null && searchAuthorResult.SearchResults.entry != null)
									{
										var firstAuthorMatch = searchAuthorResult.SearchResults.entry.FirstOrDefault();

										authorEx = new AuthorEx(authorId, authorApi.givenname, authorApi.surname, firstAuthorMatch);
										authorId++;

										authorDictionary.Add(authorApi.surname + " " + authorApi.givenname, authorEx);
									}
								}
								else
								{
									authorEx = authorDictionary[authorApi.surname + " " + authorApi.givenname];
								}

								//Only add author if SearchAuthorApi 
								if (authorEx != null)
								{
									authorEx.Publications.Add(publication);
									authorTempList.Add(authorEx);
								}
							}

							for (var i = 0; i < authorTempList.Count - 1; i++)
								for (var j = i + 1; j < authorTempList.Count; j++)
								{
									var authorI = authorTempList[i];
									var authorJ = authorTempList[j];

									//Check if the edge exists
									var authorRelation = authorRelationDictionary
										.FirstOrDefault(
											x => (x.Author1Id == authorTempList[i].Id && x.Author2Id == authorTempList[j].Id) ||
												 (x.Author1Id == authorTempList[j].Id && x.Author2Id == authorTempList[i].Id));

									if (authorRelation == null)
									{
										var sameAffiliationCity = false;

										var authorICurrentAffiliation = authorI.AuthorFromSearchApi.affiliationcurrent;
										var authorJCurrentAffiliation = authorJ.AuthorFromSearchApi.affiliationcurrent;

										if (authorICurrentAffiliation != null && authorJCurrentAffiliation != null)
										{
											var authorICurrentAffiliationCountry = authorICurrentAffiliation.affiliationcountry;
											var authorJCurrentAffiliationCountry = authorJCurrentAffiliation.affiliationcountry;

											if (!string.IsNullOrWhiteSpace(authorICurrentAffiliationCountry) && !string.IsNullOrWhiteSpace(authorJCurrentAffiliationCountry))
											{
												var authorICurrentAffiliationCity = authorICurrentAffiliation.affiliationcity;
												var authorJCurrentAffiliationCity = authorJCurrentAffiliation.affiliationcity;

												if (!string.IsNullOrWhiteSpace(authorICurrentAffiliationCity) &&
												    !string.IsNullOrWhiteSpace(authorJCurrentAffiliationCity))
												{
													if (authorICurrentAffiliationCity == authorJCurrentAffiliationCity)
														sameAffiliationCity = true;
												}
											}
										
										}

										authorRelation = new AuthorRelation(
											authorRelationId, 
											authorTempList[i].Id,
											authorTempList[j].Id, 
											sameAffiliationCity);

										authorRelationId++;
										authorRelationDictionary.Add(authorRelation);
									}

									//Add publication
									authorRelation.Years.Add(publication.PrismCoverDateFirstYear);
								}
						}
					}
				}

				using (var w = new StreamWriter(publicationsFile))
				{
					int papersWithoutAuthors = 0;

					foreach (var paperEntry in publicationDictionary)
					{
						var paper = paperEntry.Value;

						var line = $"\"{paper.dctitle}\",\"{paper.prismpublicationName}\",{paper.prismcoverDate.First()._},{paper.prismdoi}";

						var authors = paper.authors;
						if (authors != null)
						{
							foreach (var author in authors.author)
							{
								line += ", " + author.givenname + " " + author.surname;
							}
						}
						else
						{
							papersWithoutAuthors++;
						}

						w.WriteLine(line);
						w.Flush();
					}
				
					System.Console.WriteLine("Papers without authors: " + papersWithoutAuthors);
				}

				using (var w = new StreamWriter(authorsFile))
				{
					//Write the CSV Header
					var line = "Id,Label,AnPrimaPub,weight,Time Interval,AnUltimaPub,PublicationCountElsevier,PublicationCountScopus,affiliationid,affiliationname,affiliationcity,affiliationcountry";
					foreach (var subjectCategoryAbbrev in AppSettings.SubjectCategoriesAbbrev)
					{
						line += "," + subjectCategoryAbbrev;
					}

					w.WriteLine(line);
					w.Flush();

					//Write the values
					foreach (var author in authorDictionary.Values)
					{
						//<[1993-01-01,1996-05-13,1];[1996-05-13,1998 - 10 - 06,2];[1998-10-06,1999-07-05,3];[1999-07-05,2002-11-19,4];[2002-11-19,2013-01-01,5];> <[1993-01-01,2013-01-01]>

						var publicationYears = author.Publications.GroupBy(x => x.PrismCoverDateFirstYear).Select(x => x.Key).ToList();
						publicationYears.Sort();

						var firstPublicationYear = publicationYears[0];
						var lastPublicationYear = publicationYears[publicationYears.Count - 1];

						var publicationYearsDisplay = new StringBuilder("<");
						for (var yearIndex = 0; yearIndex < publicationYears.Count; yearIndex++)
						{
							publicationYearsDisplay.Append(
								$"[{publicationYears[yearIndex]},{publicationYears[yearIndex] + 1},{yearIndex + 1}];");
						}
						publicationYearsDisplay.Append(">");
						//publicationYearsDisplay.Append($" <[{firstPublicationYear},{lastPublicationYear + 1}]>");


						line = $"{author.Id}," +
							   $"{author.FirstName} {author.LastName}," +
							   $"{firstPublicationYear}," +
							   $"\"{$"{publicationYearsDisplay}"}\"," +
							   $"\"{$"<[{firstPublicationYear},{lastPublicationYear + 1}]>"}\"," +
							   $"{lastPublicationYear}," +
							   $"{author.Publications.Count}";

						try
						{
							var authorFromSearchApi = author.AuthorFromSearchApi;
							if (authorFromSearchApi != null)
							{
								line += "," + authorFromSearchApi.documentcount;

								//affiliationid
								if (authorFromSearchApi.affiliationcurrent?.affiliationid != null)
									line += ",\"" + authorFromSearchApi.affiliationcurrent.affiliationid + "\"";
								else
									line += ",";

								//affiliationname
								if (authorFromSearchApi.affiliationcurrent?.affiliationname != null)
									line += ",\"" + authorFromSearchApi.affiliationcurrent.affiliationname + "\"";
								else
									line += ",";

								//affiliationcity
								if (authorFromSearchApi.affiliationcurrent?.affiliationcity != null)
									line += ",\"" + authorFromSearchApi.affiliationcurrent.affiliationcity + "\"";
								else
									line += ",";

								//affiliationcountry
								if (authorFromSearchApi.affiliationcurrent?.affiliationcountry != null)
									line += ",\"" + authorFromSearchApi.affiliationcurrent.affiliationcountry + "\"";
								else
									line += ",";

								foreach (var subjectCategoryAbbrev in AppSettings.SubjectCategoriesAbbrev)
								{
									if(authorFromSearchApi.subjectarea.Any(x=>x.abbrev == subjectCategoryAbbrev))
										line += ",1";
									else
										line += ",0";
								}
							}
						}
						catch (Exception ex)
						{
							System.Console.WriteLine(ex);
						}

						w.WriteLine(line);
						w.Flush();
					}
				}

				using (var w = new StreamWriter(authorRelationsFile))
                {
                    var line = "Source,Target,Type,Id,Label,Weight,AnPrimaLegatura,AnUltimaLegatura,Time Interval,SameAffiliationCity";
                    w.WriteLine(line);
                    w.Flush();

                    foreach (var authorRelation in authorRelationDictionary)
                    {
                        System.Console.WriteLine($"{authorRelation.Author1Id}\t{authorRelation.Author2Id}\t{authorRelation.Id}");

                        var firstPublicationYear = authorRelation.Years.Min();
                        var lastPublicationYear = authorRelation.Years.Max();
						
						line = $"{authorRelation.Author1Id}," +
                               $"{authorRelation.Author2Id}," +
                               "Directed," +
                               $"{authorRelation.Id}," +
                               "," +
                               $"{authorRelation.Years.Count}," +
                               $"{firstPublicationYear}," +
                               $"{lastPublicationYear}," +
                               $"\"{$"<[{firstPublicationYear},{lastPublicationYear}]>"}\"," +
						       (authorRelation.SameAffiliationCity?1:0);

                        w.WriteLine(line);
                        w.Flush();
                    }
                }
			}
			catch (Exception ex)
			{
				throw;
			}
		}
	}
}
