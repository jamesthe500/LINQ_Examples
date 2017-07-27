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

        // in order to see when the year property is being accessed in queries
        // we're changing it from being an automatic property
        // to a property backed by a private field

        int _year;
        public int Year
        {
            get
            {
                // every time this get is accessed, 
                // the cw is triggered
                // our custom Filter() pings every title before priting the two results
                // switch it to the built in Where() and you get the year pinged right before priting a result then another title pinged.
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
