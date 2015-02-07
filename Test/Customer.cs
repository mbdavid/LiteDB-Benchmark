using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteDB_Benchmark
{
    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime BirdDate { get; set; }
        public Guid MyGuid { get; set; }
        public List<string> Phones { get; set; }
        public string Description { get; set; }

        public static IEnumerable<Customer> GetData(int count)
        {
            for(var i = 1; i <= count; i++)
            {
                yield return new Customer
                {
                    Id = i,
                    Name = Helper.LoremIpsum(3, 5, 1, 1, 1), 
                    BirdDate = DateTime.Now.AddDays(count),
                    MyGuid = Guid.NewGuid(),
                    Phones = new List<string> { "+555 123456" },
                    Description = Helper.LoremIpsum(15, 15, 3, 3, 2)
                };
            }
        }
    }
}
