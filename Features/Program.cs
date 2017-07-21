using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Features
{
    class Program
    {
        static void Main(string[] args)
        {
            //Employee[] developers = new Employee[]
            IEnumerable<Employee> developers = new Employee[]
            {
                new Employee {Id = 1, Name = "Scott" },
                new Employee {Id = 2, Name = "Sarah" }
            };

            //List<Employee> sales = new List<Employee>()
            IEnumerable<Employee> sales = new List<Employee>()
            {
                new Employee {Id = 3, Name = "Alexi" }
            };

            // A lot of LINQ operators off of IEnumberable<T> interface inheritance. 
            // it has flexibility to work with tyypes. can work with many differnt data sources

            // This is essentially a foreach doen the hard way. 
            // done to demonstrate teh importance of IEnumerator to LINQ
            // any kind of data sturcture or operation can be hidden behind IEnumerable

            IEnumerator<Employee> enumerator = developers.GetEnumerator();
            while (enumerator.MoveNext())
            {
                Console.WriteLine(enumerator.Current.Name);
            }
        }
    }
}
