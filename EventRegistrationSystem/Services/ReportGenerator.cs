using EventRegistrationSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EventRegistrationSystem.Services
{
    class ReportGenerator
    {
        public static void GenerateReport(
            List<EventRegistration> records)
        {
            Console.WriteLine(
                "\n===== EVENT REPORT =====\n");

            var grouped =
                records
                .FindAll(r => r.IsActive)
                .GroupBy(r => r.EventName);

            foreach (var group in grouped)
            {
                Console.WriteLine(
                    group.Key +
                    " : " +
                    group.Count() +
                    " Registrations");
            }
        }
    }
}