using System;
using System.Threading;

namespace ConsoleApp1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            DateTime dateTime = DateTime.Now;

            while(true)
            {
                Thread.Sleep(1000);
                Console.WriteLine("In break:"+DateTime.Now.Subtract(dateTime).ToString(@"mm\ss"));
            }    
        }
    }
}
