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
                // Anonymous objects to make it possible to filter by two fields
                join manufacturer in manufacturers 
                    on new { car.Manufacturer, car.Year } 
                        equals // Need to specify what Manufacturer equals since the field names don't match.
                        new { Manufacturer = manufacturer.Name, manufacturer.Year }
                orderby car.Combined descending, car.Name ascending
                select new
                {
                    manufacturer.Headquarters,
                    car.Combined,
                    car.Name
                };

            var query2 =
                cars.Join(manufacturers,
                            c => new { c.Manufacturer, c.Year },
                            m => new { Manufacturer = m.Name, m.Year },
                            (c, m) => new
                            {
                                m.Headquarters,
                                c.Name,
                                c.Combined
                            })
                    .OrderByDescending(c => c.Combined)
                    .ThenBy(c => c.Name);

            var query3 =
                cars.Join(carDetails,
                            c => c.Name,
                            d => d.Name,
                            (c, d) => new
                            {
                                c.Manufacturer,
                                c.Name,
                                d.VehicleClass,
                                d.Fuel,
                                c.Combined
                            })
                    .Join(manufacturers,
                            c => c.Manufacturer,
                            m => m.Name,
                            (c, m) => new
                            {
                                m.Headquarters,
                                c.Manufacturer,
                                c.Name,
                                c.VehicleClass,
                                c.Fuel,
                                c.Combined
                            })
                    .Where(c => c.VehicleClass == "Small Station Wagons")
                    .OrderByDescending(c => c.Fuel)
                    .ThenBy(c => c.Name);

            foreach (var car in query2.Take(10))
            {
                Console.WriteLine($"{car.Headquarters} {car.Name} {car.Combined}");
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
