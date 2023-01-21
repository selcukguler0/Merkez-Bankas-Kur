﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ConsoleApp1
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var today = DateTime.Now;
            var isWeekend = today.DayOfWeek == DayOfWeek.Saturday || today.DayOfWeek == DayOfWeek.Sunday;

            var newDate = today.DayOfWeek == DayOfWeek.Saturday ? today.AddDays(-1) : today.AddDays(-2);
            var requestURL = $"https://www.tcmb.gov.tr/kurlar/{newDate.Year}{newDate.ToString("MM")}/{newDate.ToString("dd")}{newDate.ToString("MM")}{newDate.Year}.xml";
            
            var client = new HttpClient();

            var response = isWeekend ? await client.GetAsync(requestURL) : await client.GetAsync("https://www.tcmb.gov.tr/kurlar/today.xml");

            var content = await response.Content.ReadAsStringAsync();

            var xdoc = XDocument.Parse(content);

            foreach ( var x in xdoc.Descendants("Currency"))
            {
                Console.WriteLine($"---- {x.Attribute("Kod").Value} ----");
                Console.WriteLine("Forex Selling: " + x.Element("ForexBuying").Value);
                Console.WriteLine("Forex Selling: " + x.Element("ForexSelling").Value);
            }

            //programın kapanmasını önlemek için
            Console.ReadLine();
        }
    }
}
