﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dasz.LinqCube;

namespace dasz.LinqCube.Example
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Building dimensions");
            var time = new Dimension<DateTime, Person>("Time", k => k.Birthday)
                    .BuildYear(new DateTime(1978, 1, 1), new DateTime(2012, 1, 1))
                    .BuildMonths()
                    .Build<DateTime, Person>();

            var time_empstart = new Dimension<DateTime, Person>("Time employment start", k => k.EmploymentStart)
                    .BuildYear(new DateTime(2001, 1, 1), new DateTime(2011, 1, 1))
                    .Build<DateTime, Person>();

            var gender = new Dimension<string, Person>("Gender", k => k.Gender)
                    .BuildEnum("M", "F")
                    .Build<string, Person>();

            var salary = new Dimension<decimal, Person>("Salary", k => k.Salary)
                    .BuildPartition(500, 1000, 2500)
                    .BuildPartition(100)
                    .Build<decimal, Person>();

            var countAll = new CountMeasure<Person>("Count", k => k);

            var sumSalary = new DecimalSumMeasure<Person>("Sum of Salaries", k => k.Salary);

            var genderAgeQuery = new Query<Person>("gender over birthday")
                                    .WithDimension(time)
                                    .WithDimension(gender)
                                    .WithMeasure(countAll);

            var salaryQuery = new Query<Person>("salary over gender and date of employment")
                                    .WithDimension(time_empstart)
                                    .WithDimension(gender)
                                    .WithDimension(salary)
                                    .WithMeasure(countAll)
                                    .WithMeasure(sumSalary);

            var employmentCountQuery = new Query<Person>("count by date of employment")
                                    .WithDimension(time_empstart)
                                    .WithMeasure(countAll);

            CubeResult result;
            using (var ctx = new Repository())
            {
                result = Cube.Execute(ctx.Persons,
                                genderAgeQuery,
                                salaryQuery,
                                employmentCountQuery
                );
            }

            foreach (var year in time_empstart.Children)
            {
                Console.WriteLine(year.Label);
                Console.WriteLine("==================");
                foreach (var gPart in salary.Children)
                {
                    foreach (var gPart2 in gPart.Children)
                    {
                        Console.WriteLine("{0}: {1,12}, M: {2,3} W: {3,3}",
                            salary.Name,
                            gPart2.Label,
                            result[salaryQuery][year][gPart2][gender]["M"][countAll],
                            result[salaryQuery][year][gPart2][gender]["F"][countAll]);
                    }
                }
                Console.WriteLine();
            }


            Console.WriteLine("Finished, hit the anykey to exit");
            Console.ReadKey();
        }
    }
}
