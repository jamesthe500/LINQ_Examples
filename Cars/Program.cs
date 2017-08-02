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

            // where operator to filter the sort. Have to filter before sorting.
            /*
            var query =
                from car in cars
                where car.Manufacturer == "BMW" && car.Year == 2018
                orderby car.Combined descending, car.Name ascending
                select car;
            */

            // Same in extension method syntax
            var query = cars.Where(c => c.Manufacturer == "BMW")
                            .OrderByDescending(c => c.Combined)
                            .ThenBy(c => c.Name);

            // .First() has two overloads
            // 1- it takes no parameters and just gives you first back
            // 2- it takes a func and a bool, so it can replace the where 
            /*
            var topCar = cars.Where(c => c.Manufacturer == "BMW" && c.Year == 2018)
                            .OrderByDescending(c => c.Combined)
                            .ThenBy(c => c.Name)
                            // doesn't give you anything to iterate over. cannot use deferred execution
                            .First();
            */

            // Here is the way to do it with .First() replacing where
            // must be careful about where you place that .First().
            // if before sorting, you'll get the wrong results
            /*
            var topCar = cars
                            .OrderByDescending(c => c.Combined)
                            .ThenBy(c => c.Name)
                            .First(c => c.Manufacturer == "BMW" && c.Year == 2018);
            */

            // there is also .Last()
            // also .LastOrDefault() / .FirstOrDefault()
            // These are handy if there is a chance that the dataset will be empty or your query could come up empty
            // in these cases, it will return a defalt value, and error checking will have to happen down the line.
            var topCar = cars
                            .OrderByDescending(c => c.Combined)
                            .ThenBy(c => c.Name)
                            .LastOrDefault(c => c.Manufacturer == "Goof Balls" && c.Year == 2018);

            Console.WriteLine(topCar.Name);
            
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
