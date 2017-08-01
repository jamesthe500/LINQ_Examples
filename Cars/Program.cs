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
            foreach (var car in cars)
            {
                Console.WriteLine(car.Name);
            }
        }

        private static List<Car> ProcessFile(string path)
        {
            return
            File.ReadAllLines(path)
                // .Skip(1) passes over the first row, which is headers
                .Skip(1)
                // This filters out the last row which has no data
                // in guide, > 1 worked, but with my dataset, the commas are being counted. Probably cuz I went through Excel. 
                .Where(l => l.Length > 444)
                // here we map from a string to a Car object. 
                // This is complex, so we're building a helper method in the Car class
                // other options would be to do it inline with {}
                // or put the method in this class.
                .Select(Car.ParseFromCsv)
                .ToList();
        }
    }
}
