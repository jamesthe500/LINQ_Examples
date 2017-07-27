using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Queries
{
    class Program
    {
        static void Main(string[] args)
        {
            var movies = new List<Movie>
            {
                 new Movie {Title="The Dark Knight", Rating=8.9f, Year = 2008 },
                 new Movie {Title="The King's Speech", Rating=8.0f, Year = 2010 },
                 new Movie {Title="Casablanca", Rating=8.5f, Year = 1942 },
                 new Movie {Title="Star Wars V", Rating = 8.7f, Year = 1980 }
            };

            // 2. Turning off deferred is necessary here.
            // there are many operations that do it, they all start with "To"
            // they convert it to a concrete data structure.
            var query = movies.Filter(m => m.Year >= 2000).ToList();

            // 1. the Count() operator does not use deferred execution, it forces the query to execute immediatly.
            // thus, whe running, it has to do twicce teh amount of work- 
            // the count has to loop through once to coun the number of items it finds,
            // then the actual results we want to print.
            // this is a situation where we want to turn off deferred execution ^
            Console.WriteLine(query.Count());
            var enumerator = query.GetEnumerator();
            while (enumerator.MoveNext())
            {
                Console.WriteLine(enumerator.Current.Title);
            }
        }
    }
}
