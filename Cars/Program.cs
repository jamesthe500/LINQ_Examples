using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Linq.Expressions;

namespace Cars
{
    class Program
    {
        static void Main(string[] args)
        {

            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<CardDb>());

            InsertData();
            QueryData();
        }

        private static void QueryData()
        {
            var db = new CardDb();

            // Big caveat with EntityFW is to be aware of when you are working with an
            // IQueryable vs IEnumerable
            // with IQueryable the EntityFW is converting your queries to efficient SQL code
            // with IEnumerable, it always has to load the data into memory.
            // here we can see that with the addition of .ToList() before .Take(10), 
            // we are querying the entire set, bringing it to memory, whereas with .ToList after, it's more efficient.
            // you can see in the intellisense options that the .Take operator is working against an IEnumerable now.

            db.Database.Log = Console.WriteLine;

            var query2 =
                db.Cars.Where(c => c.Manufacturer == "BMW")
                       .OrderByDescending(c => c.Combined)
                       .ThenBy(c => c.Name)
                       .ToList()
                       .Take(10);

            // this is a query that will compile, but will not run.
            // Linq doesn't know how to convert .Split to SQL syntax.
            // it can be made to work by putting the .ToList() before the select, thus allowing it to happen in memory, where C# is understood.

            var query3 =
               db.Cars.Where(c => c.Manufacturer == "BMW")
                      .OrderByDescending(c => c.Combined)
                      .ThenBy(c => c.Name)
                      .Take(10)
                      .ToList()
                      .Select(c => new { Name = c.Name.Split(' ') });
                      
                      

            foreach (var car in query3)
            {
                Console.WriteLine($"{car.Name[1]}" );
            }
        }

        private static void InsertData()
        {
            var cars = ProcessCars("fuel.csv");
            var db = new CardDb();

            if (!db.Cars.Any())
            {
                foreach (var car in cars)
                {
                    db.Cars.Add(car);
                }
                db.SaveChanges();
            }
        }

        private static void QueryXml()
        {
            var ns = (XNamespace)"http://pluralsight.com/cars/2016";
            var ex = (XNamespace)"http://pluralsight.com/cars/2016/ex";
            var document = XDocument.Load("fuel.xml");
 
            var query =
                from element in document.Element(ns + "Cars")?.Elements(ex + "Car") ?? Enumerable.Empty<XElement>()
                where element.Attribute("Manufacturer")?.Value == "BMW"
                select element.Attribute("Model").Value;

            foreach (var car in query)
            {
                Console.WriteLine(car);
            }
        }

        private static void CreateXml()
        {
            var records = ProcessCars("fuel.csv");

            var ns = (XNamespace)"http://pluralsight.com/cars/2016";
            var ex = (XNamespace)"http://pluralsight.com/cars/2016/ex";
            var document = new XDocument();
            var cars = new XElement(ns + "Cars",
                from record in records
                select new XElement(ex + "Car",
                                        new XAttribute("Model", record.Name),
                                        new XAttribute("Combined", record.Combined),
                                        new XAttribute("Manufacturer", record.Manufacturer))
                    );
            cars.Add(new XAttribute(XNamespace.Xmlns + "ex", ex));

            document.Add(cars);
            document.Save("fuel.xml");
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

    public class CarStatistics
    {
        public CarStatistics()
        {
            Max = Int32.MinValue;
            Min = Int32.MaxValue;
        }   

        internal CarStatistics Accumulate(Car car)
        {
            Count++;
            Total += car.Combined;

            if(car.Combined > Max)
            {
                Max = car.Combined;
            }

            Min = Math.Min(Min, car.Combined);
            

            return this;
        }

        public CarStatistics Compute()
        {
            Avg = Total / Count;
            return this;
        }
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
