using CsvHelper;
using SocialEvolutionSensor.Models;
using System;
using System.Collections.Generic;
using System.IO;

namespace SocialEvolutionSensor
{
    class Program
    {
        static List<Call> calls = new List<Call>();

        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            Console.WriteLine(System.Diagnostics.Process.GetCurrentProcess().ProcessName);

            using (var reader = new StreamReader("../../../SensorData/Calls.csv"))
            using (var csv = new CsvReader(reader))
            {
                csv.Configuration.HasHeaderRecord = true;
                while (csv.Read())
                {
                    var record = csv.GetRecord<Call>();
                    calls.Add(record);
                }
            }
        }
    }
}
