using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cars
{
    // this class is for storing the data while working with it.
    // we'll be transforming the data in teh .csv into a collection of cars. 
    // LINQ is not the obvious choice for .csv, but it's very useful for data files.
    // we'll use the filter operator to take out the header and footer lines
    // Project operator will actually transform each line of the file into a car object
    // AKA Select, Map, Projection, or Transform operator
    // the method name in LINQ is Select()
    // you'll have an incoming stream of data, foreach, transform into a different shape 
    // string > car
    class Car
    {
        public int Year { get; set; }
        public string Manufacturer { get; set; }
        public string Name { get; set; }
        public double Displacement { get; set; }
        public int Cylinders { get; set; }
        public int City { get; set; }
        public int Highway { get; set; }
        public int Combined { get; set; }

        internal static Car ParseFromCsv(string line)
        {
            // should be done with lots of error checking, but that's for antoher day

            // this .Split() takes the line and separates it into separate strings where the , appears
            // produces an array
            var columns = line.Split(',');

            return new Car
            {
                // this is all predicated on knowing which columns contain whcih data and the datatype in that column
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
