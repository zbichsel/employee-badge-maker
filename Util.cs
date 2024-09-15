// Import correct packages
using System;
using System.IO;
using System.Collections.Generic;

namespace CatWorx.BadgeMaker
{
    class Util
    {
        // Method declared as "static" so it can be called without creating an instance of the Util class
        // Add List parameter to the method
        public static void PrintEmployees(List<Employee> employees)
        {
            for (int i = 0; i < employees.Count; i++)
            {
                string template = "{0,-10}\t{1,-20}\t{2}";
                Console.WriteLine(String.Format(template, employees[i].GetId(), employees[i].GetFullName(), employees[i].GetPhotoUrl()));
            }
        }

// 1. Add another static method to the Util class, which will make the CSV file.
public static void MakeCSV(List<Employee> employees)
{
    // Check if the data directory exists
    if (!Directory.Exists("data"))
    {
        // If it doesn't, create it
        Directory.CreateDirectory("data");
    }
    using (StreamWriter file = new StreamWriter("data/employees.csv"))
    {
        file.WriteLine("ID,Name,PhotoUrl");

        // Loop over employees
        for (int i = 0; i < employees.Count; i++)
        {
            // Write each employee to the file
            string template = "{0},{1},{2}";
            file.WriteLine(String.Format(template, employees[i].GetId(), employees[i].GetFullName(), employees[i].GetPhotoUrl()));
        }
    }
}
    /* 2. Call this method from within Program, passing in the list of employees.
    3. In the method, check whether a data folder exists. If not, create it.
    4. Create new file located at data/employees.csv.
    5. Loop over given list of employees and write each one to the file in CSV format.
    6. Write the ID, full name, and photo URL for each employee -- separated by commas. */
    }
}