using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Queries
{
    // this is a model (?)
    public class Movie
    {
        public string Title { get; set; }
        public float Rating { get; set; }

        int _year;
        public int Year
        {
            get
            {
                // Pretty bad practice to use the base Excetion class
                // using it to throw an error every time it runs.
                throw new Exception("Whoa, buddy!");
                Console.WriteLine($"Returning {_year} for {Title}.");
                return _year;
            }
            set
            {
                _year = value;
            }
        }
    }
}
