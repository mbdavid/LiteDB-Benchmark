using MongoDB.Driver;
using MongoDB.Driver.Builders;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteDB_Benchmark
{
    public class MongoDBTest : Test
    {
        public virtual string ConnectionString { get { return "mongodb://localhost"; } }
        private MongoDatabase _db;
        private MongoCollection<Customer> _col;

        public override void Init()
        {
            var client = new MongoClient(this.ConnectionString);
            var server = client.GetServer();
            server.DropDatabase("test");

            _db = server.GetDatabase("test");
            _col = _db.GetCollection<Customer>("customer");
        }

        public override void Insert(int count)
        {
            _col.InsertBatch(Customer.GetData(count));
        }

        public override void CreateIndex()
        {
            _col.CreateIndex("Index");
        }

        public override void FetchRandom(int count, int max)
        {
            var rnd = new Random();

            for (var i = 0; i < count; i++)
            {
                var r = rnd.Next(1, max);

                var doc = _col.FindOne(Query.EQ("Index", r));

                if (doc == null) throw new NullReferenceException();
            }
        }

        public override void Update(int count)
        {
            for (var i = 1; i <= count; i++)
            {
                var doc = _col.FindOne(Query.EQ("Index", i));
                doc.Description = Helper.LoremIpsum(15, 15, 3, 3, 4);
                _col.Save(doc);
            }
        }

        public override void Delete(int count)
        {
            _col.Remove(Query.LTE("Index", count));
        }

        public override void Upload(string filename)
        {
            _db.GridFS.Upload(filename);
        }

        public override void Download(string filename)
        {
            File.Delete("_download_mongodb.bin");
            _db.GridFS.Download("_download_mongodb.bin", filename);
        }

        public override void End()
        {
        }
    }
}
