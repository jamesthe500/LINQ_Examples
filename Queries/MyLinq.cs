using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Queries
{
    // another custom class to reproduce what LINQ does.
    public static class MyLinq
    {
        // Filter<T>() reproduces Where() though in a different way.
        // The parameters are source- what you want to have filtered
        // and predicate- the parameters you want to sort by.
        // here is how we are calling it: movies.Filter(m => m.Year >= 2000);
        // Aha, "this IEnumerable" leads it to the movies object
        // and then the Lambda is a Func<T, bool> ?
        public static IEnumerable<T> Filter<T>(this IEnumerable<T> source,
                                               Func<T, bool> predicate)
        {
            var result = new List<T>();
            
            foreach (var item in source)
            {
                if (predicate(item))
                {
                    result.Add(item);
                }
            }

            return result;
        }
    }
}
