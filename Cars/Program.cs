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
                select new
                {
                    car.Manufacturer,
                    car.Combined,
                    car.Name
                };


            // this is a way of drilling into a sequence within a sequence
            /*
            var result = cars.Select(c => c.Name);
            foreach (var name in result)
            {
                foreach (var character in name)
                {
                    Console.WriteLine(character);
                }
            }
            */

            // here is a better way
            // with SelectMany, we get to the inner sequences.
            var result = cars.SelectMany(c => c.Name)
            // this sort puts the characters in order, whihc may be interesting.
                             .OrderBy(c => c);
            foreach (var character in result)
            {
                Console.WriteLine(character);
            }

            // this was for fuel efficiency
            //foreach (var car in query.Take(10))
            //{
            //    Console.WriteLine($"{car.Manufacturer} {car.Name} : {car.Combined}");
            //}
        }

        private static List<Car> ProcessFile(string path)
        {
            var query = 
            File.ReadAllLines(path)
                .Skip(1)
                .Where(l => l.Length > 444)
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
        }
    }
}
