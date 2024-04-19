
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileSystemGlobbing.Internal;
using OfficeOpenXml;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ManageStudentController : ControllerBase
    {
        private readonly DataContext _context;

        public ManageStudentController(DataContext context)
        {
            _context = context;
        }



        //create all students, school class / send a new "excel" file to the front with all the new data (Full Name, Email, Password)
        [HttpPost("upload")]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file uploaded");
            }
            string patternRegexL = @"[a-zA-Z]+";
            string patternRegexLM = @"[a-zA-Z0-9]+";

            //init the worksheet for file send to the front
            var packageToSend = new ExcelPackage();
            var worksheetToSend = packageToSend.Workbook.Worksheets.Add("Sheet1");
            worksheetToSend.Cells[1, 1].Value = "Full Name ---------------";
            worksheetToSend.Cells[1, 2].Value = "Email ------------------------------";
            worksheetToSend.Cells[1, 3].Value = "Password -------------------";
            worksheetToSend.Cells[1, 4].Value = "School Class --------";

            using (var stream = new MemoryStream())
            {
                file.CopyTo(stream);
                using (var package = new ExcelPackage(stream))
                {
                    //init the worksheet for the received file
                    var worksheet = package.Workbook.Worksheets.First();
                    var rowCount = worksheet.Dimension.Rows;
                    var colCount = worksheet.Dimension.Columns;

                    var className = "";
                    var rowSheet = 2;
                    //Loop over the excel file
                    for (int col = 1; col <= colCount; col++)
                    {
                        worksheetToSend.Columns[col].AutoFit();
                        for (int row = 1; row <= rowCount; row++)
                        {
                            if (row == 1)
                            {
                                // Create School Class
                                className = worksheet.Cells[row, col].Value?.ToString();
                                
                                Match m = Regex.Match(className != null ? className : "" , patternRegexLM, RegexOptions.IgnoreCase);
                                try
                                {
                                    if(className != null & m.Success)
                                    {
                                        var classN = new SchoolClass();
                                        classN.ClassName = className;
                                        _context.SchoolClasses.Add(classN);
                                        _context.SaveChanges();

                                    }
                                }catch (Exception ex)
                                {
                                    return BadRequest(ex + "className ADD");
                                }
                            }
                            else
                            {
                                //Create Student 
                                Match m = Regex.Match(worksheet.Cells[row, col].Value?.ToString() != null ? worksheet.Cells[row, col].Value?.ToString() : "" , patternRegexL, RegexOptions.IgnoreCase);
                                if (worksheet.Cells[row, col].Value?.ToString() != "" & m.Success)
                                {
                                    var StudentFullName = worksheet.Cells[row, col].Value?.ToString();
                                    var StudentEmail = StudentFullName != null ? String.Concat(StudentFullName.Where(c => !Char.IsWhiteSpace(c))) + "@msdmail.com" : null;
                                    Random rnd = new Random();
                                    int rndInt = rnd.Next();
                                    string studentPassword = BCrypt.Net.BCrypt.HashPassword(rndInt.ToString());
                                    if(StudentFullName != null & StudentEmail != null)
                                    {
                                        worksheetToSend.Cells[rowSheet, 1].Value = StudentFullName;
                                        worksheetToSend.Cells[rowSheet, 2].Value = StudentEmail;
                                        worksheetToSend.Cells[rowSheet, 3].Value = rndInt;
                                        worksheetToSend.Cells[rowSheet, 4].Value = className;
                                        rowSheet++;
                                        var userStudent = new User
                                        {
                                            Name = StudentFullName,
                                            Email = StudentEmail,
                                            Password = studentPassword.ToString()
                                        };
                                        _context.Users.Add(userStudent);
                                        _context.SaveChanges();
                                        var scId = _context.SchoolClasses.Where(sc => sc.ClassName == className).FirstOrDefault();
                                        var newStudent = new Student
                                        {
                                            UserId = userStudent.Id,
                                            SchoolClassId = scId.Id
                                        };
                                        try
                                        {
                                            _context.Students.Add(newStudent);
                                            _context.SaveChanges();
                                        }
                                        catch (Exception e)
                                        {
                                            return BadRequest(e + "student ADD");
                                        }
                                    }
                                    

                                }
                                
                            }

                        }
                    }
                }
                var stream2 = new MemoryStream(packageToSend.GetAsByteArray());
                stream2.Position = 0;
                byte[] fileBytes = stream2.ToArray();
                return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "test.xlsx");
            }

        }

       
    }
}
