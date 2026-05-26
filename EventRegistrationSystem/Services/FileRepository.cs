using EventRegistrationSystem.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace EventRegistrationSystem.Services
{
    class FileRepository
    {
        private static string filePath =
            "Data/registrations.txt";

        // READ ALL RECORDS
        public static List<EventRegistration> GetAllRecords()
        {
            List<EventRegistration> records =
                new List<EventRegistration>();

            // If file does not exist
            if (!File.Exists(filePath))
            {
                return records;
            }

            string[] lines =
                File.ReadAllLines(filePath);

            foreach (string line in lines)
            {
                try
                {
                    string[] data = line.Split('|');

                    EventRegistration record =
                        new EventRegistration();

                    record.RecordId =
                        Convert.ToInt32(data[0]);

                    record.FullName = data[1];

                    record.EventName = data[2];

                    record.Email = data[3];

                    record.ContactNumber = data[4];

                    record.RegistrationDate =
                        Convert.ToDateTime(data[5]);

                    record.CreatedAt =
                        Convert.ToDateTime(data[6]);

                    record.UpdatedAt =
                        Convert.ToDateTime(data[7]);

                    record.IsActive =
                        Convert.ToBoolean(data[8]);

                    record.Checksum = data[9];

                    records.Add(record);
                }
                catch
                {
                    AuditLogger.Log(
                        "ERROR",
                        "Malformed Record Found"
                    );
                }
            }

            return records;
        }

        // SAVE ALL RECORDS
        public static void SaveAll(
            List<EventRegistration> records)
        {
            List<string> lines =
                new List<string>();

            foreach (EventRegistration record in records)
            {
                string line =
                    record.RecordId + "|" +
                    record.FullName + "|" +
                    record.EventName + "|" +
                    record.Email + "|" +
                    record.ContactNumber + "|" +
                    record.RegistrationDate + "|" +
                    record.CreatedAt + "|" +
                    record.UpdatedAt + "|" +
                    record.IsActive + "|" +
                    record.Checksum;

                lines.Add(line);
            }

            File.WriteAllLines(filePath, lines);
        }

        // GENERATE UNIQUE ID
        public static int GenerateId(
            List<EventRegistration> records)
        {
            if (records.Count == 0)
            {
                return 1;
            }

            return records.Max(r => r.RecordId) + 1;
        }

        // GENERATE CHECKSUM
        public static string GenerateChecksum(
            string data)
        {
            SHA256 sha = SHA256.Create();

            byte[] bytes =
                Encoding.UTF8.GetBytes(data);

            byte[] hash =
                sha.ComputeHash(bytes);

            StringBuilder builder =
                new StringBuilder();

            foreach (byte b in hash)
            {
                builder.Append(
                    b.ToString("x2"));
            }

            return builder.ToString();
        }
    }
}