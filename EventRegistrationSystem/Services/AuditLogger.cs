using System;
using System.IO;

namespace EventRegistrationSystem.Services
{
    class AuditLogger
    {
        private static string logPath = "Data/audit_log.txt";

        public static void Log(string action, string details)
        {
            string log =
                DateTime.Now +
                " | " +
                action +
                " | " +
                details;

            File.AppendAllText(
                logPath,
                log + Environment.NewLine
            );
        }
    }
}