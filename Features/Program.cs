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
            // The Func type was created for working with delegates 
            // which are types that allow you to create variables to point to methods. 
            // the first parameter is the type the method takes in, second is what it returns
            // of course it can point to a named method too
            // this is an anonymous inline method using Lambda expression
            Func<int, int> square = x => x * x;
            Console.WriteLine(square(3));

            // with more than two types defined, it's the last one that's the return, the others are inputs
            // can explicitly define them to the right of the =, optional
            Func<int, int, int> add = (int x, int y) => x + y;
            Console.WriteLine(add(2,3));
            Console.WriteLine(square(add(2,3)));

            // can also use curly braces for more complex stuff
            // needs to have a return statement
            // though most Lambdas are short one-liners
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

            // Action methods are another thing, though LINQ mostly uses Funcs
            // they only return void, so all types laid out are for input.
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

            
            foreach (var employee in developers.Where(e => e.Name.Length == 5).OrderBy(e => e.Name))
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
