using System;

namespace dasz.LinqCube.Tests
{
    public class Person
    {
        public int ID { get; set; }

        public string Gender { get; set; }

        public Gender GenderEnum { get; set; }

        public Gender0 Gender0Enum { get; set; }

        public Gender1 Gender1Enum { get; set; }

        public Gender2 Gender2Enum { get; set; }

        public DateTime Birthday { get; set; }

        public DateTime? EmploymentStart { get; set; }

        public DateTime? EmploymentEnd { get; set; }

        public decimal Salary { get; set; }

        public string Office { get; set; }

        public bool Active { get; set; }
    }
}