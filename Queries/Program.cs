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

            // one good reason for Deferred is - perhaps you only need to Take(1), as below.
            // with deferred, it only has to run into the DB once. 
            // without it, it still insppects all 4 films first, then prints the Take(1)
            // Many LINQ operators use deffered execution, you can search MSDN to see which ones.
            var query = movies.Filter(m => m.Year >= 2000)
                .Take(1);

            // rewriting the foreach, which is really an enumerator behind the scenes anyhow.
            // this helps in seeing what the program is actually doing.
            // Step through F10, Step in F11

            var enumerator = query.GetEnumerator();
            while (enumerator.MoveNext())
            {
                Console.WriteLine(enumerator.Current.Title);
            }
        }
    }
}
