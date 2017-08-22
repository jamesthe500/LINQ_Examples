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
            var records = ProcessCars("fuel.csv");
            var document = new XDocument();
            var cars = new XElement("Cars",
                // instead of foreach, add a LINQ Query as the second parameter of the Cars element
                // this way vs. foreach is a source of debate. 
                // Foreach is more readable for maintenance.
                // this way is more succinct. 
                from record in records
                select new XElement("Car",
                                        new XAttribute("Model", record.Name),
                                        new XAttribute("Combined", record.Combined),
                                        new XAttribute("Manufacturer", record.Manufacturer))
                    );

            //// foreach can be avoided too see above.
            //foreach (var record in records)
            //{
            //    //// changed to an Attribute oriented XML file.
            //    //var name = new XAttribute("Name", record.Name);
            //    //var combined = new XAttribute("Combined", record.Combined);
            //    //// brought this down from above, so it comes . 
            //    //// added the attributes as parameters and removed the next two lines.
            //    ////car.Add(name);
            //    ////car.Add(combined);
            //    //// this is known as "Functional construction"
            //    //var car = new XElement("Car", name, combined);

            //    // A shorter way is to just add the attributes directly to the xElement
            //    var car = new XElement("Car", 
            //                            new XAttribute("Model", record.Name),
            //                            new XAttribute("Combined", record.Combined),
            //                            new XAttribute("Manufacturer", record.Manufacturer)
            //        );

            //    cars.Add(car);
            //}
            
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
