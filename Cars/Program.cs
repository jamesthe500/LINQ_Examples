using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cars
{
    class Program
    {
        static void Main(string[] args)
        {
            var cars = ProcessFile("fuel.csv");

           
            var query =
                from car in cars
                where car.Manufacturer == "BMW" && car.Year == 2018
                orderby car.Combined descending, car.Name ascending
                select car;

            // there are various quantifying operators
            // they are all immediately executed.
            // they try to be as lazy as possible, 
            // so .Any() stops exectuing as soon as it finds a predicate match

            // .Any() can also be blank. Is there anything in this dataset?
            //var result = cars.Any(c => c.Manufacturer == "Ford");

            // Are all manufacturers ford?
            //var result = cars.All(c => c.Manufacturer == "Ford");

            // Contains... Not really getting this one. :-/
            var result = cars.Contains();

            Console.WriteLine(result );
            
            foreach (var car in query.Take(10))
            {
                Console.WriteLine($"{car.Manufacturer} {car.Name} : {car.Combined}");
            }
        }

        private static List<Car> ProcessFile(string path)
        {
            return
            File.ReadAllLines(path)
                .Skip(1)
                .Where(l => l.Length > 444)
                .Select(Car.ParseFromCsv)
                .ToList();
        }
    }
}
