using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.QueryParsers;
using Lucene.Net.Search;
using Lucene.Net.Store;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace task_1.Models.ProfileModel
{
    public class ProfileSearch
    {
        private const string Path = @"G:\itransition\kursSite\LuceneIndexes";

        private const string idName = "Id";
        private const string userName = "UserName";

        private const int Size = 10;

        private readonly Lucene.Net.Util.Version _version = Lucene.Net.Util.Version.LUCENE_29;

        private readonly Analyzer _analyzer = new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_29);

        public void Index()
        {
            var indexDirectory = new DirectoryInfo(Path);

            if (indexDirectory.Exists)
                indexDirectory.Delete(true);

            FSDirectory entityDirectory = null;
            IndexWriter writer = null;

            try
            {
                entityDirectory = FSDirectory.Open(indexDirectory);
                writer = new IndexWriter(
                  entityDirectory,
                  _analyzer,
                  true,
                  IndexWriter.MaxFieldLength.UNLIMITED);

                using (var context = new ProfileModelContext())
                {
                    foreach (var profile in context.Profile)
                    {
                        var document = new Document();

                        document.Add(
                          new Lucene.Net.Documents.Field(
                            idName,
                            profile.Id.ToString(),
                            Lucene.Net.Documents.Field.Store.YES,
                            Lucene.Net.Documents.Field.Index.NOT_ANALYZED,
                            Lucene.Net.Documents.Field.TermVector.NO));

                        document.Add(
                          new Lucene.Net.Documents.Field(
                            userName,
                            profile.UserName,
                            Lucene.Net.Documents.Field.Store.YES,
                            Lucene.Net.Documents.Field.Index.ANALYZED,
                            Lucene.Net.Documents.Field.TermVector.WITH_POSITIONS_OFFSETS));

                        writer.AddDocument(document);
                    }
                }

                writer.Optimize(true);
            }
            finally
            {
                if (writer != null)
                    writer.Dispose();

                if (entityDirectory != null)
                    entityDirectory.Dispose();
            }
        }

        public IEnumerable<Profile> Find(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
                return Enumerable.Empty<Profile>();

            var indexDirectory = new DirectoryInfo(Path);

            FSDirectory entityDirectory = null;
            IndexSearcher searcher = null;

            try
            {
                entityDirectory = FSDirectory.Open(indexDirectory);
                searcher = new IndexSearcher(entityDirectory, true);

                var documentProfiles = FindDocumentProfiles(query, searcher);

                return documentProfiles.Select(db => db.Value).ToList();
            }
            finally
            {
                if (searcher != null)
                    searcher.Dispose();

                if (entityDirectory != null)
                    entityDirectory.Dispose();
            }
        }

        private IEnumerable<KeyValuePair<int, Profile>> FindDocumentProfiles(string query, Searcher searcher)
        {
            var parser = new MultiFieldQueryParser(
              _version,
              new[] { userName },
              _analyzer);

            var scoreDocs = searcher.Search(parser.Parse(query), Size).ScoreDocs;

            var documentProfilesIds = scoreDocs.Select(x => new KeyValuePair<int, int>
            (
              x.Doc,
              int.Parse(searcher.Doc(x.Doc).Get(idName))
            )).ToList();

            var profilesIds = documentProfilesIds.Select(db => db.Value);
            using (var context = new ProfileModelContext())
            {
                return context.Profile
                  .Where(b => profilesIds.Contains(b.Id))
                  .ToList()
                  .Select(b =>
                    new KeyValuePair<int, Profile>(
                      documentProfilesIds.Single(db => db.Value == b.Id).Key, b));
            }
        }
    }
}