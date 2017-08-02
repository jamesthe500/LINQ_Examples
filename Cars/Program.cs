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

            // .ThenBy() is the proper way to do a secondary sort. 
            // you can tack on a .OrderBy, but that undoes teh previous sort
            /*
            var query = cars.OrderByDescending(c => c.Combined)
                            .ThenBy(c => c.Name);
            */

            // same as above but in query syntax
            // "ascending" is an operator, but that's default, so not necessary
            var query =
                from car in cars
                orderby car.Combined ascending, car.Name ascending
                select car;
            
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
