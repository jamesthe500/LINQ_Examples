using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Features
{
    public static class MyLinq
    {
        // adding the "this" to the parameter, it becomes an extension method
        // making it availabel to any IEnumberable<T> within the same (or using) namespace
        // with or without "this" it can be invoked with MyLinq.Count()
        // 
        public static int Count<T>(this IEnumerable<T> sequence)
        {
            int count = 0;
            foreach (var item in sequence)
            {
                count += 1;
            }
            return count;
        }
    }
}
