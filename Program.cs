using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Project
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // int a = 2;
            // string text = string.Format("I'm a c# .NET Developer with {0} year experience",a);
            // Console.WriteLine(text);
            // char a = 'a';
            // Console.WriteLine((int)a);
            CreateHostBuilder(args).Build().Run();
            // int a = 10, b = 20;
            // Console.WriteLine(a > b ? "a is greater":"b is greater");
            // for (int i = 97; i < 123; i++)
            // {
            //     Console.WriteLine((char)i);
            // }
            // string sentence = "and that piece of art is useful\" - Dora Korpar, 2015-10-19";
            // foreach (var letter in sentence)
            // {
            //     Console.Write(letter);
            // }
        }
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
