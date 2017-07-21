using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Introduction
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = @"C:\windows";
            ShowLargeFilesWithoutLinq(path);
            Console.WriteLine("####");
            ShowLargeFilesWithLinq(path);
            Console.WriteLine("####");
            ShowLargeFilesWithLinqOther(path); 
        }

        private static void ShowLargeFilesWithLinqOther(string path)
        {
            // another way to use LINQ is a series of method calls like this:
            var query = new DirectoryInfo(path).GetFiles()
                        .OrderByDescending(f => f.Length)
                        .Take(5);

            foreach (var file in query)
            {
                Console.WriteLine($"{file.Name,-20} : {file.Length,10:N0}");
            }
        }

        private static void ShowLargeFilesWithLinq(string path)
        {
            // LINQ code is more readable, in this technique it looks like an SQL query, doesn't require the helper method...
            var query = from file in new DirectoryInfo(path).GetFiles()
                        orderby file.Length descending
                        select file;
            // Take() is pretty handy, take only 5 results. Nice!
            foreach (var file in query.Take(5))
            {
                Console.WriteLine($"{file.Name,-20} : {file.Length,10:N0}");
            }
        }

        private static void ShowLargeFilesWithoutLinq(string path)
        {
            
            DirectoryInfo directory = new DirectoryInfo(path);
            FileInfo[] files = directory.GetFiles();
            Array.Sort(files, new FileInfoComparer());

            for (int i = 0; i<5; i++)
            {
                FileInfo file = files[i];
                Console.WriteLine($"{file.Name,-20} : {file.Length,10:N0}");
            }
        }
    }

    public class FileInfoComparer : IComparer<FileInfo>
    {
        public int Compare(FileInfo x, FileInfo y)
        {
            return y.Length.CompareTo(x.Length);

        }
    }
}
