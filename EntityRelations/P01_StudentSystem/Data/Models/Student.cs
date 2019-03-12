using System;

namespace P01_StudentSystem.Data.Models
{
    public class Student
    {
        public Student(string name, string phoneNumber = null, DateTime? birthDate = null)
        {
            Name = name;
            PhoneNumber = phoneNumber;
            RegisteredOn = DateTime.Now;
            Birthday = birthDate;
        }

        public int StudentId { get; set; }

        public string Name { get; }

        public string PhoneNumber { get; set; }

        public DateTime RegisteredOn { get; }

        public DateTime? Birthday { get; set; }
    }
}