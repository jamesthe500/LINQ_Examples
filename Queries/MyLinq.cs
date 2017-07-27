using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Queries
{
    public static class MyLinq
    {
       
        public static IEnumerable<T> Filter<T>(this IEnumerable<T> source,
                                               Func<T, bool> predicate)
        {
            // Switching this up to use yield return
            // which will give it DEFERRED EXECUTION it only runs when it's needed
            // a behavior more in line with Where()
            // yield helps build an IEnumberable or IEnumerator 
            // It yields control back to the caller 
            // and only executes when the result is actually going to be used.

            //var result = new List<T>();
            
            foreach (var item in source)
            {
                if (predicate(item))
                {
                    //result.Add(item);
                    yield return item;
                }
            }

            //return result;
        }
    }
}
