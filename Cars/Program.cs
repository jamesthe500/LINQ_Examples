using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cars
{
    class Program
    {
        static void Main(string[] args)
        {
            CreateXml();
            QueryXml();
        }

        
        private static void QueryXml()
        {
            // in order to get the Xml into memory we're using XDocument.
            // if we had a huge Xml doc it would be better to use the older
            // XmlReader API. It streams, rather than just loads the whole thing.
            // there are many ways to load. .Parse() .ReadFrom()
            // .Load() has many overloads including a way to put in a URI and stream in the doc.
            var document = XDocument.Load("fuel.xml");

            // browsing the Xml documetn
            // like JQuery. Selet the 1st Cars element, then select all elements that are "Car"
            // It will return an iEnumberable<XElement> so we can use LInQ against this collection of elements.
            var query =
                //from element in document.Element("Cars").Elements("Car")
                // another option for the above, simpler 
                // any descendents that are "Car" from any element. 
                // if there were others than just "Cars" it would search them too.
                from element in document.Descendants("Car")
                    // the ? is a "null conditional" operator. It allows the code to continue if it finds an element
                    // that doesn't have that attribute. It'll return 'null' if it doesn't find that one.
                    // prevents exception.
                where element.Attribute("Manufacturer")?.Value == "BMW"
                select element.Attribute("Model").Value;

            foreach (var car in query)
            {
                Console.WriteLine(car);
            }
        }

        

        // created using Edit > Refactor > Extract Method.
        private static void CreateXml()
        {
            var records = ProcessCars("fuel.csv");
            var document = new XDocument();
            var cars = new XElement("Cars",
                from record in records
                select new XElement("Car",
                                        new XAttribute("Model", record.Name),
                                        new XAttribute("Combined", record.Combined),
                                        new XAttribute("Manufacturer", record.Manufacturer))
                    );

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
