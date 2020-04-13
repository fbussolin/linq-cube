using System;
using System.Diagnostics;
using System.Globalization;
using System.Linq;

namespace dasz.LinqCube.Example
{
    class Program
    {
        static void Main()
        {
            Console.WriteLine("Testing Linq-Cube v{0}", typeof(Program).Assembly.GetName().Version);

            TestCube();

            Console.WriteLine("Finished, press any key to exit");
            Console.ReadKey();
        }

        public static void TestCube()
        {
            Console.WriteLine("Building DateTime dimensions");
            // Dimension year - months
            var birthday = new Dimension<DateTime, Person>("Birthday", k => k.Birthday)
                .BuildYear(1978, Repository.CURRENT_YEAR)
                .BuildMonths()
                .Build<DateTime, Person>();

            // A period dimension, only look at jan-sept. very use full for comparing time periods during a year
            var time_empstart = new Dimension<DateTime, Person>("Time employment start", k => k.EmploymentStart.Value, k => k.EmploymentStart.HasValue)
                .BuildYearSlice(Repository.MIN_DATE.Year, Repository.CURRENT_YEAR, sliceFromMonth: 1, sliceThruMonth: 9) // only look at jan-sept
                .BuildMonths()
                .Build<DateTime, Person>();

            // Year only time dimension
            var time_employment = new Dimension<DateTime, Person>("Time employment", k => k.EmploymentStart.Value, k => k.EmploymentEnd ?? DateTime.MaxValue, k => k.EmploymentStart.HasValue)
                .BuildYear(Repository.MIN_DATE.Year, Repository.CURRENT_YEAR)
                .Build<DateTime, Person>();


            Console.WriteLine("Building string dimensions");
            // "Enum" dimension with strings
            var gender = new Dimension<string, Person>("Gender", k => k.Gender)
                .BuildEnum("M", "F")
                .Build<string, Person>();

            // "Enum" dimension with strings
            var offices = new Dimension<string, Person>("Office", k => k.Office)
                .BuildEnum(Repository.OFFICES)
                .Build<string, Person>();


            Console.WriteLine("Building decimal dimensions");
            // A partition dimension. Step size 500, from 1000 up to 2500, lower hierarchy has a step size of 100
            var salary = new Dimension<decimal, Person>("Salary", k => k.Salary)
                .BuildPartition(500, 1000, 2500, "up to {1}", "{0} up to {1}", "{0} and more")
                .BuildPartition(100)
                .Build<decimal, Person>();


            Console.WriteLine("Building bool dimensions");
            // Dimension boolean
            var is_active = new Dimension<bool, Person>("Active", k => k.Active)
                .BuildBool()
                .Build<bool, Person>();


            Console.WriteLine("Building measures");
            var countAll = new CountMeasure<Person>("Count", k => true);

            var countEmployedFullMonth = new FilteredMeasure<Person, bool>("Count full month", k => k.EmploymentStart.HasValue && k.EmploymentStart.Value.Day == 1, countAll);

            var countStartingEmployment = new CountMeasure<Person>("Count Starting Employment (whole year)", (k, entry) =>
                entry.Count<DateTime>(time_employment, (e) =>
                    k.EmploymentStart.HasValue && e.Min.Year == k.EmploymentStart.Value.Year));

            var sumSalary = new DecimalSumMeasure<Person>("Sum of Salaries", k => k.Salary);

            Console.WriteLine("Building queries");
            var genderAgeQuery = new Query<Person>("gender over birthday")
                .WithChainedDimension(gender)
                .WithChainedDimension(birthday)
                .WithMeasure(countAll);

            var salaryQuery = new Query<Person>("salary over gender and date of employment")
                .WithChainedDimension(time_empstart)
                .WithChainedDimension(salary)
                .WithChainedDimension(gender)
                .WithMeasure(countAll)
                .WithMeasure(countEmployedFullMonth)
                .WithMeasure(sumSalary);

            var countByOfficeQuery = new Query<Person>("count currently employed by office")
                .WithChainedDimension(offices)
                .WithChainedDimension(time_employment)
                .WithChainedDimension(is_active)
                .WithMeasure(countAll)
                .WithMeasure(countStartingEmployment);

            // this query's dimensions can only be accessed in the order specified in "WithDimensions"
            // internally this enables the query to optimise measuring significantly
            var specialisedQuery = new Query<Person>("test-drive for a single-path query)")
                .WithChainedDimension(offices)
                .WithChainedDimension(time_empstart)
                .WithChainedDimension(time_employment)
                .WithChainedDimension(gender)
                .WithChainedDimension(salary)
                .WithMeasure(countAll)
                .WithMeasure(sumSalary);

            Stopwatch watch = new Stopwatch();
            watch.Start();

            CubeResult<Person> result;
            using (var ctx = new Repository())
            {
                result = Cube.Execute(ctx.Persons.OrderBy(x => x.EmploymentStart),
                            genderAgeQuery,
                            salaryQuery,
                            countByOfficeQuery);
            }

            watch.Stop();
            Console.WriteLine("Cube ran for {0}", watch.Elapsed);
            Console.WriteLine();

            ////////////////////////////////////////////////////////////////////////////////////
            ////////////////////////////////////////////////////////////////////////////////////

            Console.WriteLine(genderAgeQuery.Name);
            Console.WriteLine("==================");
            Console.WriteLine();

            foreach (var g in gender)
            {
                foreach (var b in birthday)
                {
                    Console.WriteLine("{0}-{1}: {2}",
                        g.Name,
                        b.Name,
                        result[genderAgeQuery][g][b][countAll].IntValue);
                }
            }
            ////////////////////////////////////////////////////////////////////////////////////
            ////////////////////////////////////////////////////////////////////////////////////

            Console.WriteLine(salaryQuery.Name);
            Console.WriteLine("==================");
            Console.WriteLine();
            foreach (var year in time_empstart)
            {
                Console.WriteLine(year.Name);
                Console.WriteLine("==================");
                foreach (var gPart in salary)
                {
                    foreach (var gPart2 in gPart)
                    {
                        Console.WriteLine("{0}: {1,13}, M: {2,3} W: {3,3}, monthStart: {4,3}",
                            salary.Name,
                            gPart2.Name,
                            result[salaryQuery][year][gPart2][gender]["M"][countAll].IntValue,
                            result[salaryQuery][year][gPart2][gender]["F"][countAll].IntValue,
                            result[salaryQuery][year][gPart2][countEmployedFullMonth].IntValue);
                    }
                }
                Console.WriteLine();
            }

            ////////////////////////////////////////////////////////////////////////////////////
            ////////////////////////////////////////////////////////////////////////////////////

            Console.WriteLine(countByOfficeQuery.Name);
            Console.WriteLine("==================");
            Console.WriteLine();
            Console.WriteLine("{0,10}|{1}",
                string.Empty,
                string.Join("|", time_employment.Children.Select(c => string.Format(CultureInfo.InvariantCulture, " {0,6} ", c.Name)).ToArray())
            );
            Console.WriteLine("----------+--------+--------+--------+--------+--------+--------+--------+--------+--------+--------+--------");
            foreach (var officeEntry in offices)
            {
                var officeCounts = result[countByOfficeQuery][officeEntry];
                Console.WriteLine("{0,10}| {1,6} |        |        |        |        |        |        |        |        |        |        ",
                    officeEntry.Name,
                    officeCounts[countAll].IntValue
                );
                Console.WriteLine("          |{0}",
                    string.Join("|", time_employment.Children.Select(c => string.Format(CultureInfo.InvariantCulture, " {0,6} ", officeCounts[c][countAll].IntValue)).ToArray())
                );
                Console.WriteLine("starting  |{0}",
                    string.Join("|", time_employment.Children.Select(c => string.Format(CultureInfo.InvariantCulture, " {0,6} ", officeCounts[c][countStartingEmployment].IntValue)).ToArray())
                );
                Console.WriteLine("active    |{0}",
                    string.Join("|", time_employment.Children.Select(c => string.Format(CultureInfo.InvariantCulture, " {0,6} ", officeCounts[c][is_active][true.ToString(CultureInfo.InvariantCulture)][countAll].IntValue)).ToArray())
                );
                Console.WriteLine("          |        |        |        |        |        |        |        |        |        |        |        ");
            }
        }
    }
}
