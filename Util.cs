// Import correct packages
using System;
using System.IO;
using System.Collections.Generic;
using System.Net.Http;
using SkiaSharp;
using System.Threading.Tasks;

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

        /* 
            1. Add another static method to the Util class, which will make the CSV file.
            2. Call this method from within Program, passing in the list of employees.
            3. In the method, check whether a data folder exists. If not, create it.
            4. Create new file located at data/employees.csv.
            5. Loop over given list of employees and write each one to the file in CSV format.
            6. Write the ID, full name, and photo URL for each employee -- separated by commas.
        */
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
        /*
        1. Import the badge template image file that will work as the background image.
        2. Customize each employee's badge with their unique information.
        3. Add this new image to the data folder.
        */
        async public static Task MakeBadges(List<Employee> employees)
        {
            // Layout variables
            int BADGE_WIDTH = 669;
            int BADGE_HEIGHT = 1044;

            int PHOTO_LEFT_X = 184;
            int PHOTO_TOP_Y = 215;
            int PHOTO_RIGHT_X = 486;
            int PHOTO_BOTTOM_Y = 517;

            int COMPANY_NAME_Y = 150;

            int EMPLOYEE_NAME_Y = 600;

            int EMPLOYEE_ID_Y = 730;

            var company = new SKPaint {
                TextSize = 42.0f,
                IsAntialias = true,
                Color = SKColors.White,
                IsStroke = false,
                TextAlign = SKTextAlign.Center,
                Typeface = SKTypeface.FromFamilyName("Arial")
            };

            var empname = new SKPaint {
                TextSize = 42.0f,
                IsAntialias = true,
                Color = SKColors.Black,
                IsStroke = false,
                TextAlign = SKTextAlign.Center,
                Typeface = SKTypeface.FromFamilyName("Arial")
            };
            
            var empid = new SKPaint {
                TextSize = 42.0f,
                IsAntialias = true,
                Color = SKColors.Black,
                IsStroke = false,
                TextAlign = SKTextAlign.Center,
                Typeface = SKTypeface.FromFamilyName("Courier New")
            };
            // instance of HttpClient is disposed after code in the block has run
            using (HttpClient client = new HttpClient())
            {
                /*
                1. Place the employee picture onto the badge template.
                2. Write the company name.
                3. Write the employee's name.
                4. Write the employee's ID #.
                5. Create a new file with .png extension.
                */
                for (int i = 0; i < employees.Count; i++)
                {
                    SKImage photo = SKImage.FromEncodedData(await client.GetStreamAsync(employees[i].GetPhotoUrl()));
                    SKImage background = SKImage.FromEncodedData(File.OpenRead("badge.png"));

                    // SKData data = background.Encode();
                    // data.SaveTo(File.OpenWrite("data/employeeBadge.png"));
                    SKBitmap badge = new SKBitmap(BADGE_WIDTH, BADGE_HEIGHT);
                    SKCanvas canvas = new SKCanvas(badge);

                    canvas.DrawImage(background, new SKRect(0, 0, BADGE_WIDTH, BADGE_HEIGHT));
                    canvas.DrawImage(photo, new SKRect(PHOTO_LEFT_X, PHOTO_TOP_Y, PHOTO_RIGHT_X, PHOTO_BOTTOM_Y));

                    canvas.DrawText(employees[i].GetCompanyName(), BADGE_WIDTH / 2f, COMPANY_NAME_Y, company);

                    canvas.DrawText(employees[i].GetFullName(), BADGE_WIDTH / 2f, EMPLOYEE_NAME_Y, empname);

                    canvas.DrawText(employees[i].GetId().ToString(), BADGE_WIDTH / 2f, EMPLOYEE_ID_Y, empid);

                    SKImage finalImage = SKImage.FromBitmap(badge);
                    SKData data = finalImage.Encode();
                    string template = "data/{0}_badge.png";
                    data.SaveTo(File.OpenWrite(string.Format(template, employees[i].GetId())));
                }
            }
        }
    }
}