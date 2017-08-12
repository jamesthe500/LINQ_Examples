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
                // using groupjoin for more efficiency. can be done in two steps, this is better.
                // start with manufacturer as that is the key of the groupa
                from manufacturer in manufacturers
                join car in cars on manufacturer.Name equals car.Manufacturer
                    // with this, you lose the car var, but the manufacturer is still available
                    into carGroup
                orderby(manufacturer.Name)
                select new
                {
                    Manufacturer = manufacturer,
                    Cars = carGroup
                };

            // in extension methods.
            // it needs the outer and inner joins and the output for parameters
            var query2 =
                manufacturers.GroupJoin(cars, m => m.Name, c => c.Manufacturer, 
                    (m, g) =>
                        new
                        {
                            Manufacturer = m,
                            Cars = g
                        })
                .OrderBy(m => m.Manufacturer.Name);

            foreach (var group in query2)
            {
                // the shape of the object has changed, so things need to be presented diferently.
                Console.WriteLine("");
                Console.WriteLine($"{group.Manufacturer.Name}:{group.Manufacturer.Headquarters}");
                foreach (var model in group.Cars.OrderByDescending(c => c.Combined).Take(2))
                {
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
