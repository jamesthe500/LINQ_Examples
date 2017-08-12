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
            var cars = ProcessCars("fuel.csv");
            var manufacturers = ProcessManufacturers("manufacturers.csv");
            var carDetails = ProcessDetails("car_details2.csv");

            var query =
                from car in cars
                    // putting orderby before group seemed to work, but maybe it's not reliable.
                    //orderby car.Manufacturer
                    // here's how Scott did it.
                group car by car.Manufacturer into manufacturer
                orderby manufacturer.Key
                select manufacturer;

            // Same thing in extension query syntax.
            var query2 =
                cars.GroupBy(c => c.Manufacturer)
                    .OrderBy(g => g.Key);

            //foreach(var group in query)
            //{
            //    // Wihtout a select, it behaves like an iEnumerable 
            //    // and you can use the Key (Manufacturer)
            //    // with a group, it places the results into buckets, the Key is the value you grouped by
            //    // You can also use its LINQ count query.
            //    Console.WriteLine($"{group.Key} has {group.Count()} models.");
            //}

            foreach (var group in query2)
            {
                // here it prints the key of each bucket
                Console.WriteLine("");
                Console.WriteLine(group.Key);
                // then you can iterate through each bucket
                // here we are just looking at the top two.
                foreach (var model in group.OrderByDescending(c => c.Combined).Take(2))
                {
                    // \t = tab.
                    Console.WriteLine($"\t{model.Name} : {model.Combined}");
                }
            }
            
        }

        private static List<CarDetails> ProcessDetails(string path)
        {
            var query =
                File.ReadAllLines(path)
                .Skip(1)
                    .Select(l =>
                    {
                        var columns = l.Split(',');
                        return new CarDetails
                        {
                            Name = columns[0].ToUpper(),
                            Drive = columns[1],
                            Fuel = columns[2],
                            VehicleClass = columns[3],
                            Co2 = int.Parse(columns[4])
                        };
                    });
            return query.ToList();
        }

        private static List<Manufacturer> ProcessManufacturers(string path)
        {
            
            var query =
                File.ReadAllLines(path)
                    .Where(l => l.Length > 1)
                    .Select(l =>
                    {
                        var columns = l.Split(',');
                        return new Manufacturer
                        {
                            Name = columns[0],
                            Headquarters = columns[1],
                            Year = int.Parse(columns[2])
                        };

                    });
            
            return query.ToList();

        }

        private static List<Car> ProcessCars(string path)
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
                    Name = columns[2].ToUpper(),
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
