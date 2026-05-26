using System;

namespace EventRegistrationSystem.Models
{
    class EventRegistration
    {
        public int RecordId { get; set; }

        public string FullName { get; set; }

        public string EventName { get; set; }

        public string Email { get; set; }

        public string ContactNumber { get; set; }

        public DateTime RegistrationDate { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public bool IsActive { get; set; }

        public string Checksum { get; set; }
    }
}