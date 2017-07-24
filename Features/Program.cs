using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Features
{
    class Program
    {
        static void Main(string[] args)
        {
            // ALL ABOUT VAR m2c6
            // var kw was introduced to C# along with LINQ
            // makes woking with queries easier a lto of the time.
            // allows compiler to infer type.
            // only usable in local variables,
            // not okay for parameters for a method or field or property on a class
            // must initialize an expression to a var variable otherwise compiler can't infer.
            // also can't assign it to null
            // this isn't weak typing. The compiler still won't changes types on an established var.

            // QUERY SYNTAX m2c7
            // The query syntax resembles SQL embeded in C#
            // there is also method syntax, which looks like chained methods .select()...
            // Always starts with "from" which is like a foreach
            // must end in "select" or "group" 
            // 

            Func<int, int> square = x => x * x;
            Console.WriteLine(square(3));

            Func<int, int, int> add = (int x, int y) => x + y;
            Console.WriteLine(add(2,3));
            Console.WriteLine(square(add(2,3)));

            Func<int, int, bool> evenSum = (x, y) =>
            {
                int temp = x + y;
                int truthiness = temp % 2;
                if (truthiness == 0)
                {
                    return true;
                } else
                {
                    return false;
                }
            };

            Console.WriteLine(evenSum(1,3));

           
            Action<int> write = x => Console.WriteLine(x);
            write(square(add(3, 4)));


            Console.WriteLine( "****");
            
            IEnumerable<Employee> developers = new Employee[]
            {
                new Employee {Id = 1, Name = "Scott" },
                new Employee {Id = 2, Name = "Sarah" },
                new Employee {Id = 3, Name = "Dharia" }
            };

          
            IEnumerable<Employee> sales = new List<Employee>()
            {
                new Employee {Id = 3, Name = "Alexi" }
            };

            // method syntax
            // the Select statement is optional. In this situation it is a "No-Op" or no operation
            var query = developers.Where(e => e.Name.Length == 5)
                                  .OrderByDescending(e => e.Name)
                                  .Select(e => e);

            // query syntax same result
            var query2 = from developer in developers
                         where developer.Name.Length == 5
                         orderby developer.Name descending
                         select developer;

            foreach (var employee in query2 )
            {
                Console.WriteLine(employee.Name);
            }
        }

       
        private static bool NameStartsWithS(Employee employee)
        {
            return employee.Name.StartsWith("S");
        }
    }
}
