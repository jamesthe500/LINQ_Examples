using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Queries
{
    public static class MyLinq
    {
        // this creates an infinite loop 
        // but not really
        // it keeps adding random doubles to itself
        // to demonstrate that you can work with an infinite dataset
        public static IEnumerable<double> Random()
        {
            var random = new Random();
            while (true)
            {
                yield return random.NextDouble();
            }
        }

        public static IEnumerable<T> Filter<T>(this IEnumerable<T> source,
                                               Func<T, bool> predicate)
        {            
            foreach (var item in source)
            {
                if (predicate(item))
                {
                    yield return item;
                }
            }
        }
    }
}
