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
                group car by car.Manufacturer into carGroup
                select new
                {
                    Name = carGroup.Key,
                    Max = carGroup.Max(c => c.Combined),
                    Min = carGroup.Min(c => c.Combined),
                    Avg = carGroup.Average(c => c.Combined)

                } into result
                orderby result.Max descending
                select result;

            // this extension methods way of doing it only loops through the dataset once. 
            // whereas above it loops through 3 times, once per statisitc.
            // obvs better. 
            var query2 =
               cars.GroupBy(c => c.Manufacturer)
                    .Select(g =>
                    {
                        // here we pass in the accumilator class defined below.
                        // an accumilator is needed as the first parameter to the Aggreagate() method
                        // "results will be car statistics 
                        // it's what happens when we Aggregate a grouping of cars"
                        var results = g.Aggregate(new CarStatistics(),
                                                    // second parameter takes teh accumulator and a car 
                                                    // and does somethign to allow them to interact 
                                                    // so stats can be tracked.
                                                    // then returns the accumulator
                                                    // "we pass in teh accumulator 
                                                    // invoke the accumulate() once for each car"
                                                    (acc, c) => acc.Accumulate(c),
                                                    // thirdly, we generate the Compute() method below.
                                                    // this computes the statistics we need.
                                                    // "At the end we compute the final statistics
                                                    acc => acc.Compute());
                        return new
                        {
                            Name = g.Key,
                            Avg = results.Avg,
                            Max = results.Max,
                            Min = results.Min
                        };
                    })
                    .OrderByDescending(r => r.Max);

            foreach (var result in query2)
            {
                Console.WriteLine("");
                Console.WriteLine($"{result.Name} :");
                Console.WriteLine($"\t Max={result.Max}");
                Console.WriteLine($"\t Min={result.Min}");
                Console.WriteLine($"\t Avg={Math.Round(result.Avg, 1)}");

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

    // this custom class would normally be in a separate file, of course.
    // Accumilator class
    public class CarStatistics
    {
        // this is a constructor to set the initial values of these two variables
        public CarStatistics()
        {
            Max = Int32.MinValue;
            Min = Int32.MaxValue;
        }   

        internal CarStatistics Accumulate(Car car)
        {
            Count++;
            Total += car.Combined;

            // my solution.
            if(car.Combined > Max)
            {
                Max = car.Combined;
            }

            // this is the better way to do this that what I did with an if.
            Min = Math.Min(Min, car.Combined);
            

            return this;
        }

        // this had to be changed to return CarStatistics and be a public method.
        public CarStatistics Compute()
        {
            Avg = Total / Count;
            return this;
        }

        // the total field is needed to compute the Avg, of course.
        public int Max { get; set; }
        public int Min { get; set; }
        public int Total { get; set; }
        public int Count { get; set; }
        public double Avg { get; set; }
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
