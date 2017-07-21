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

            // original version that calls the named method below.
            //foreach (var employee in developers.Where(NameStartsWithS))

            // version 2. 
            /*foreach (var employee in developers.Where(
                delegate (Employee employee)
                {
                return employee.Name.StartsWith("S");
                }))*/

            // version 3 is the same as two, but using lamda expression
            // here's how it's the same:
            // complider doesn't need to be told it will be a delegate
            // knows the type will have to be Employee, so not needed
            // return, (), {}, ; also aren't needed
            // left with the parameter name and expression 
            // just add "Goes to" operator- =>
            // shorten the parameter name for sleekness
            foreach (var employee in developers.Where(
            e => e.Name.StartsWith("S")
            ))
            {
                Console.WriteLine(employee.Name);
            }
        }

        // this named method is a good way to filter for your needs. 
        // it was created using intelisense, putting the name as a parameter in Where() above.
        // In a big application, it would get cumbersome to do this over and over, so anonymous methods are better.
        // so, taking the method definition below, paste it in above with some synax fixing up. add kw delegate
        private static bool NameStartsWithS(Employee employee)
        {
            return employee.Name.StartsWith("S");
        }
    }
}
