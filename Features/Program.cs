using System;
using System.Collections.Generic;
// commented it out so we could see how Linq works. There is already a Count() method
// would work to not have our custom method as an extension method
//using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Features
{
    class Program
    {
        static void Main(string[] args)
        {
            
            IEnumerable<Employee> developers = new Employee[]
            {
                new Employee {Id = 1, Name = "Scott" },
                new Employee {Id = 2, Name = "Sarah" }
            };

          
            IEnumerable<Employee> sales = new List<Employee>()
            {
                new Employee {Id = 3, Name = "Alexi" }
            };

            // can count arrays, lists, anything
            Console.WriteLine(developers.Count());
            IEnumerator<Employee> enumerator = developers.GetEnumerator();
            while (enumerator.MoveNext())
            {
                Console.WriteLine(enumerator.Current.Name);
            }
        }
    }
}
