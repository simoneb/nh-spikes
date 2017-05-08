using System;
using Microsoft.Owin.Hosting;

namespace nh_spikes
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var url = "http://+:1234";

            using (WebApp.Start<Startup>(url))
            {
                Console.WriteLine("Running on {0}", url);
                Console.WriteLine("Press enter to exit");
                Console.ReadLine();
            }
        }
    }
}