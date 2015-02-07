using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteDB_Benchmark
{
    public abstract class Test
    {
        public abstract void Init();
        public abstract void Insert(int count);
        public abstract void CreateIndex();
        public abstract void FetchRandom(int count, int max);
        public abstract void Update(int count);
        public abstract void Delete(int count);
        public abstract void Upload(string filename);
        public abstract void Download(string filename);
        public abstract void End();

        public List<Dictionary<string, long>> _results = new List<Dictionary<string, long>>();

        public void Run(int round, int insert, int fetch, int update, int delete, int fileSizeMB)
        {
            var fakeFile = Helper.CreateDumbFile("fake_file.bin", fileSizeMB * 1024 * 1024);

            var result = new Dictionary<string, long>();
            var sw = new Stopwatch();
            var title = "> " + this.GetType().Name + " (" + round + ")";
            Console.WriteLine(title);
            Console.WriteLine("".PadRight(title.Length, '-'));

            // Initialize test
            this.Init();

            // Insert
            sw.Restart();
            Console.Write("Insert".PadRight(20, '.'));
            this.Insert(insert);
            result["Insert"] = sw.ElapsedMilliseconds;
            Console.WriteLine(sw.ElapsedMilliseconds);

            // Create Index
            sw.Restart();
            Console.Write("CreateIndex".PadRight(20, '.'));
            this.CreateIndex();
            result["CreateIndex"] = sw.ElapsedMilliseconds;
            Console.WriteLine(sw.ElapsedMilliseconds);

            // Fetch
            sw.Restart();
            Console.Write("FetchRandom".PadRight(20, '.'));
            this.FetchRandom(fetch, insert);
            result["FetchRandom"] = sw.ElapsedMilliseconds;
            Console.WriteLine(sw.ElapsedMilliseconds);

            // Update
            sw.Restart();
            Console.Write("Update".PadRight(20, '.'));
            this.Update(update);
            result["Update"] = sw.ElapsedMilliseconds;
            Console.WriteLine(sw.ElapsedMilliseconds);

            // Update
            sw.Restart();
            Console.Write("Delete".PadRight(20, '.'));
            this.Delete(delete);
            result["Delete"] = sw.ElapsedMilliseconds;
            Console.WriteLine(sw.ElapsedMilliseconds);

            // Upload
            sw.Restart();
            Console.Write("Upload".PadRight(20, '.'));
            this.Upload(fakeFile);
            result["Upload"] = sw.ElapsedMilliseconds;
            Console.WriteLine(sw.ElapsedMilliseconds);

            // Download
            sw.Restart();
            Console.Write("Download".PadRight(20, '.'));
            this.Download(fakeFile);
            result["Download"] = sw.ElapsedMilliseconds;
            Console.WriteLine(sw.ElapsedMilliseconds);

            Console.WriteLine();

            // End test
            this.End();

            _results.Add(result);
        }

        public void ShowResults()
        {
            Console.WriteLine("> " + this.GetType().Name);
            Console.WriteLine("".PadRight(this.GetType().Name.Length + 2, '-'));

            Console.WriteLine("Insert............ " + _results.Avg2("Insert"));
            Console.WriteLine("CreateIndex....... " + _results.Avg2("CreateIndex"));
            Console.WriteLine("FetchRandom....... " + _results.Avg2("FetchRandom"));
            Console.WriteLine("Update............ " + _results.Avg2("Update"));
            Console.WriteLine("Delete............ " + _results.Avg2("Delete"));
            Console.WriteLine("Upload............ " + _results.Avg2("Upload"));
            Console.WriteLine("Download.......... " + _results.Avg2("Download"));
            Console.WriteLine();

        }

    }
}
