using LiteDB;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteDB_Benchmark
{
    public class LiteDBTest : Test
    {
        public virtual string ConnectionString { get { return "test.db"; } }

        private LiteDB.LiteEngine _db;
        private LiteDB.Collection<Customer> _col;

        public override void Init()
        {
            File.Delete(new LiteDB.ConnectionString(this.ConnectionString).Filename);
            _db = new LiteDB.LiteEngine(this.ConnectionString);
            _col = _db.GetCollection<Customer>("customer");
        }

        public override void Insert(int count)
        {
            _col.Insert(Customer.GetData(count));
        }

        public override void CreateIndex()
        {
            _col.EnsureIndex("Index");
        }

        public override void FetchRandom(int count, int max)
        {
            var rnd = new Random();

            for (var i = 0; i < count; i++)
            {
                var index = rnd.Next(1, max);

                var doc = _col.FindOne(Query.EQ("Index", index));

                if (doc == null) throw new NullReferenceException();
            }
        }

        public override void Update(int count)
        {
            _db.BeginTrans();

            for (var i = 1; i <= count; i++)
            {
                var doc = _col.FindOne(Query.EQ("Index", i));
                doc.Description = Helper.LoremIpsum(15, 15, 3, 3, 4);
                _col.Update(doc);
            }

            _db.Commit();
        }

        public override void Delete(int count)
        {
            _col.Delete(Query.LTE("Index", count));
        }

        public override void Upload(string filename)
        {
            _db.FileStorage.Upload("myfile", filename);
        }

        public override void Download(string filename)
        {
            File.Delete("_download_litedb.bin");
            _db.FileStorage.FindById("myfile").SaveAs("_download_litedb.bin"); 
        }

        public override void End()
        {
            _db.Dispose();
        }
    }
}
