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
                //select car;
                // creates an anonymous object through projection that only looks at the relevant columns
                // This is a short syntax that replaces
                // Manufacturer = car.Manufacturer
                // new {} is a way to create an anonymous object, a shorthand.
                select new
                {
                    car.Manufacturer,
                    car.Combined,
                    car.Name
                };

            // here is the same thin as above in extension method syntax.
            var result = cars.Select(c => new { c.Manufacturer, c.Name, c.Combined });
            
            foreach (var car in query.Take(10))
            {
                Console.WriteLine($"{car.Manufacturer} {car.Name} : {car.Combined}");
            }
        }

        private static List<Car> ProcessFile(string path)
        {
            var query = 
            File.ReadAllLines(path)
                .Skip(1)
                .Where(l => l.Length > 444)
                // changing this up to a custom method that is mroe clear  
                .ToCar();

            return query.ToList();
        }
    }

    public static class CarExtensions
    {
        public static IEnumerable<Car> ToCar(this IEnumerable<string> source)
        {
            foreach (var line in source)
            {
                var columns = line.Split(',');

                // added a yield so that it will be deferred execution.
                yield return new Car
                {
                    Year = int.Parse(columns[0]),
                    Manufacturer = columns[1],
                    Name = columns[2],
                    Displacement = double.Parse(columns[3]),
                    Cylinders = int.Parse(columns[4]),
                    City = int.Parse(columns[5]),
                    Highway = int.Parse(columns[6]),
                    Combined = int.Parse(columns[7])

                };
            }

            // This was brought over from teh Car.cs class. 
            // needs to change around a bit since it is not dealing with a line at a time any more.
            // see above.
            /*
            var columns = line.Split(',');

            return new Car
            {
                Year = int.Parse(columns[0]),
                Manufacturer = columns[1],
                Name = columns[2],
                Displacement = double.Parse(columns[3]),
                Cylinders = int.Parse(columns[4]),
                City = int.Parse(columns[5]),
                Highway = int.Parse(columns[6]),
                Combined = int.Parse(columns[7])

            };
            */
        }
    }
}
