using EventRegistrationSystem.Models;
using EventRegistrationSystem.Services;

using System;
using System.Collections.Generic;
using System.IO;

namespace EventRegistrationSystem
{
    class Program
    {
        static void Main(string[] args)
        {
            InitializeStorage();
            Menu();
        }

        static void InitializeStorage()
        {
            if (!Directory.Exists("Data"))
            {
                Directory.CreateDirectory("Data");
            }

            if (!File.Exists("Data/registrations.txt"))
            {
                File.Create("Data/registrations.txt").Close();
            }

            if (!File.Exists("Data/audit_log.txt"))
            {
                File.Create("Data/audit_log.txt").Close();
            }
        }

        static void Menu()
        {
            while (true)
            {
                Console.Clear();

                Console.WriteLine("===== EVENT REGISTRATION SYSTEM =====");
                Console.WriteLine("1. Add Record");
                Console.WriteLine("2. View Records");
                Console.WriteLine("3. Search Record");
                Console.WriteLine("4. Update Record");
                Console.WriteLine("5. Soft Delete");
                Console.WriteLine("6. Hard Delete");
                Console.WriteLine("7. Generate Report");
                Console.WriteLine("8. Exit"); ;

                Console.Write("\nChoose: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        AddRecord();
                        break;

                    case "2":
                        ViewRecords();
                        break;

                    case "3":
                        SearchRecord();
                        break;

                    case "4":
                        UpdateRecord();
                        break;

                    case "5":
                        SoftDelete();
                        break;

                    case "6":
                        HardDelete();
                        break;

                    case "7":
                        GenerateReport();
                        break;

                    case "8":
                        return;

                    default:
                        Console.WriteLine("Invalid Choice");
                        Console.ReadLine();
                        break;
                }
            }
        }

        static void AddRecord()
        {
            try
            {
                List<EventRegistration> records =
                    FileRepository.GetAllRecords();

                EventRegistration record =
                    new EventRegistration();

                // Generate Unique ID
                record.RecordId =
                    FileRepository.GenerateId(records);

                // User Input
                Console.Write("Full Name: ");
                record.FullName = Console.ReadLine();

                Console.Write("Event Name: ");
                record.EventName = Console.ReadLine();

                Console.Write("Email: ");
                record.Email = Console.ReadLine();

                Console.Write("Contact Number: ");
                record.ContactNumber = Console.ReadLine();

                // Validation
                if (!ValidationService.IsNotEmpty(record.FullName) ||
                    !ValidationService.IsValidEmail(record.Email) ||
                    !ValidationService.IsValidContact(record.ContactNumber))
                {
                    Console.WriteLine("\nInvalid Input!");

                    AuditLogger.Log(
                        "ERROR",
                        "Invalid Input During Add"
                    );

                    Console.ReadLine();
                    return;
                }

                // Dates
                record.RegistrationDate = DateTime.Now;

                record.CreatedAt = DateTime.Now;

                record.UpdatedAt = DateTime.Now;

                // Active Status
                record.IsActive = true;

                // Checksum
                string checksumData =
                    record.FullName +
                    record.EventName +
                    record.Email;

                record.Checksum =
                    FileRepository.GenerateChecksum(
                        checksumData
                    );

                // Add Record To List
                records.Add(record);

                // Save To File
                FileRepository.SaveAll(records);

                // Audit Log
                AuditLogger.Log(
                    "ADD",
                    "Added Record ID " + record.RecordId
                );

                Console.WriteLine(
                    "\nRecord Added Successfully!"
                );
            }
            catch (Exception ex)
            {
                AuditLogger.Log(
                    "ERROR",
                    ex.Message
                );

                Console.WriteLine(
                    "An Error Occurred."
                );
            }

            Console.ReadLine();
        }

        static void ViewRecords()
        {
            List<EventRegistration> records =
                FileRepository.GetAllRecords();

            Console.WriteLine(
                "\n===== ACTIVE RECORDS =====\n"
            );

            foreach (EventRegistration record in records)
            {
                if (record.IsActive)
                {
                    Console.WriteLine(
                        "Record ID: " + record.RecordId);

                    Console.WriteLine(
                        "Full Name: " + record.FullName);

                    Console.WriteLine(
                        "Event Name: " + record.EventName);

                    Console.WriteLine(
                        "Email: " + record.Email);

                    Console.WriteLine(
                        "Contact Number: " +
                        record.ContactNumber);

                    Console.WriteLine(
                        "Created At: " +
                        record.CreatedAt);

                    Console.WriteLine();
                }
            }
                AuditLogger.Log(
                "READ",
                "Viewed Records"
            );

            Console.ReadLine();
        }
        static void SearchRecord()
            {
                List<EventRegistration> records =
                    FileRepository.GetAllRecords();

                Console.Write(
                    "Enter Event Name: ");

                string search =
                    Console.ReadLine();

                var results =
                    records.FindAll(r =>
                        r.EventName.ToLower()
                        .Contains(search.ToLower())
                        && r.IsActive);

                Console.WriteLine(
                    "\n===== SEARCH RESULTS =====\n");

                foreach (EventRegistration record in results)
                {
                    Console.WriteLine(
                        "ID: " + record.RecordId);

                    Console.WriteLine(
                        "Name: " + record.FullName);

                    Console.WriteLine(
                        "Event: " + record.EventName);

                    Console.WriteLine();
                }

                AuditLogger.Log(
                    "READ",
                    "Search Record"
                );

                Console.ReadLine();
            }
            static void UpdateRecord()
            {
                List<EventRegistration> records =
                    FileRepository.GetAllRecords();

                Console.Write(
                    "Enter Record ID: ");

                int id =
                    Convert.ToInt32(
                        Console.ReadLine());

                EventRegistration record =
                    records.Find(r =>
                        r.RecordId == id);

                if (record == null)
                {
                    Console.WriteLine(
                        "Record Not Found");

                    Console.ReadLine();
                    return;
                }

                Console.Write(
                    "New Full Name: ");

                record.FullName =
                    Console.ReadLine();

                Console.Write(
                    "New Event Name: ");

                record.EventName =
                    Console.ReadLine();

                record.UpdatedAt =
                    DateTime.Now;

                string checksumData =
                    record.FullName +
                    record.EventName +
                    record.Email;

                record.Checksum =
                    FileRepository.GenerateChecksum(
                        checksumData);

                FileRepository.SaveAll(records);

                AuditLogger.Log(
                    "UPDATE",
                    "Updated Record ID " + id);

                Console.WriteLine(
                    "Record Updated Successfully");

                Console.ReadLine();
            }

            static void SoftDelete()
            {
                List<EventRegistration> records =
                    FileRepository.GetAllRecords();

                Console.Write(
                    "Enter Record ID: ");

                int id =
                    Convert.ToInt32(
                        Console.ReadLine());

                EventRegistration record =
                    records.Find(r =>
                        r.RecordId == id);

                if (record == null)
                {
                    Console.WriteLine(
                        "Record Not Found");

                    Console.ReadLine();
                    return;
                }

                record.IsActive = false;

                FileRepository.SaveAll(records);

                AuditLogger.Log(
                    "DELETE",
                    "Soft Deleted Record ID " + id);

                Console.WriteLine(
                    "Soft Delete Successful");

                Console.ReadLine();
            }
        static void HardDelete()
        {
            List<EventRegistration> records =
                FileRepository.GetAllRecords();

            Console.Write(
                "Enter Record ID: ");

            int id =
                Convert.ToInt32(
                    Console.ReadLine());

            records.RemoveAll(r =>
                r.RecordId == id);

            FileRepository.SaveAll(records);

            AuditLogger.Log(
                "DELETE",
                "Hard Deleted Record ID " + id);

            Console.WriteLine(
                "Hard Delete Successful");

            Console.ReadLine();
        }
        static void GenerateReport()
        {
            List<EventRegistration> records =
                FileRepository.GetAllRecords();

            ReportGenerator.GenerateReport(records);

            AuditLogger.Log(
                "REPORT",
                "Generated Report");

            Console.ReadLine();
        }
    }
}