using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace dasz.LinqCube.Example
{
    public class Person
    {
        public int ID { get; set; }
        public string Gender { get; set; }
        public DateTime Birthday { get; set; }
        public DateTime? EmploymentStart { get; set; }
        public DateTime? EmploymentEnd { get; set; }
        public decimal Salary { get; set; }
        public string Office { get; set; }
        public bool Active { get; set; }
    }

    public class Repository : IDisposable
    {
        public static readonly int DATA_COUNT = 50000;
        public static readonly DateTime MIN_DATE = new DateTime(DateTime.Today.Year - 10, 1, 1);
        public static readonly DateTime MAX_DATE = new DateTime(DateTime.Today.Year + 1, 1, 1);
        public static readonly int CURRENT_YEAR = MAX_DATE.Year - 1;
        public static readonly string[] OFFICES =
        {
            "New York",
            "Vienna",
            "Moscow",
            "Bejing",
            "Sydney",
            "Rio",
        };


        private IList<Person> persons;
        public IQueryable<Person> Persons => persons.AsQueryable();

        public Repository()
        {
            // CreateRandomTestData(true);
            CreateFixedTestData();
        }

        private void CreateRandomTestData(bool toOutput = false)
        {
            Console.WriteLine("Initializing repository");
            persons = new List<Person>(DATA_COUNT);

            Random rnd = new Random();
            for (int i = 0; i < DATA_COUNT; i++)
            {
                DateTime? empStart = MIN_DATE.AddDays(rnd.Next(3650));
                DateTime? empEnd = empStart.Value.AddDays(rnd.Next(3650 * 2));
                if (empEnd > DateTime.Today)
                {
                    empEnd = null;
                }

                if (rnd.NextDouble() > 0.9) empStart = empEnd = null;

                persons.Add(new Person()
                {
                    ID = i + 1,
                    Gender = rnd.Next(2) == 0 ? "F" : "M",
                    Salary = (decimal)(rnd.NextDouble() * 2500.0 + 500.0),
                    Birthday = MAX_DATE.AddYears(-18).AddDays(-rnd.Next(14600)),
                    EmploymentStart = empStart,
                    EmploymentEnd = empEnd,
                    Office = OFFICES[rnd.Next(OFFICES.Length)],
                    Active = rnd.NextDouble() > 0.3,
                });
            }

            if (toOutput)
            {
                using (var output = new StreamWriter("..\\..\\data\\persons.csv"))
                {
                    foreach (var person in persons)
                    {
                        var builder = new StringBuilder();
                        builder.Append(person.ID);
                        builder.Append(";");
                        builder.Append(person.Gender);
                        builder.Append(";");
                        builder.Append(person.Salary);
                        builder.Append(";");
                        builder.Append(person.Birthday);
                        builder.Append(";");
                        builder.Append(person.EmploymentStart);
                        builder.Append(";");
                        builder.Append(person.EmploymentEnd);
                        builder.Append(";");
                        builder.Append(person.Office);
                        builder.Append(";");
                        builder.Append(person.Active);
                        output.WriteLine(builder.ToString());
                    }
                }
            }

            Console.WriteLine("Initializing repository finished");
        }

        private void CreateFixedTestData()
        {
            Console.WriteLine("Initializing repository");
            var lines = File.ReadAllLines("..\\..\\data\\persons.csv");
            persons = new List<Person>(lines.Length);

            foreach (var line in lines)
            {
                var fields = line.Split(';');

                persons.Add(new Person()
                {
                    ID = Convert.ToInt32(fields[0], CultureInfo.CurrentCulture),
                    Gender = fields[1],
                    Salary = Convert.ToDecimal(fields[2], CultureInfo.CurrentCulture),
                    Birthday = DateTime.ParseExact(fields[3], "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture),
                    EmploymentStart = string.IsNullOrWhiteSpace(fields[4]) ? (DateTime?)null : DateTime.ParseExact(fields[4], "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture),
                    EmploymentEnd = string.IsNullOrWhiteSpace(fields[5]) ? (DateTime?)null : DateTime.ParseExact(fields[5], "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture),
                    Office = fields[6],
                    Active = bool.Parse(fields[7])
                });
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // Close your database connection here
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}