
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileSystemGlobbing.Internal;
using OfficeOpenXml;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using WebApplication1.Data;
using WebApplication1.Models;
using WebApplication1.Repositories.Repo_Interfaces;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ManageStudentController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IStudentRepository _studentRepository;
        private readonly ISchoolClassRepository _schoolClassRepository;

        public ManageStudentController(IUserRepository userRepository, IStudentRepository studentRepository, ISchoolClassRepository schoolClassRepository)
        {
            _userRepository = userRepository;
            _studentRepository = studentRepository;
            _schoolClassRepository = schoolClassRepository;
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
            string patternRegexLN = @"^[a-zA-Z0-9\s]+$";

            //init the worksheet for file sent to the front -  first row values 
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
                    var sch = new List<SchoolClass>();
                    //Loop over the excel file
                    for (int col = 1; col <= colCount; col++)
                    {
                        worksheetToSend.Columns[col].AutoFit(); // fit cells 
                        for (int row = 1; row <= rowCount; row++)
                        {
                            if (row == 1)
                            {
                                // Create School Class - Process

                                className = worksheet.Cells[row, col].Value?.ToString();

                                //check if cell value match REGEX (only letters and numbers)

                                var cmCheck = String.Concat(className.Where(c => !Char.IsWhiteSpace(c)));
                                Match m = Regex.Match(cmCheck, patternRegexLN);
                                try
                                {
                                    
                                    if(className != null & m.Success)
                                    {
                                        // creating school Class
                                        var classN = new SchoolClass();
                                        classN.ClassName = className;
                                        _schoolClassRepository.Add(classN);


                                        //get all school classes
                                         sch = _schoolClassRepository.GetAll().ToList();
                                    }
                                    else {
                                        //Delete data already inserted before the error
                                        _schoolClassRepository.RemoveAll();
                                        _userRepository.RemoveAll();
                                        return Unauthorized("School Class names not allowed"); 
                                    }
                                }catch (Exception ex)
                                {
                                    return NotFound("Error Intern");
                                }
                            }
                            else
                            {

                                var cellValue = worksheet.Cells[row, col].Value?.ToString();

                                //Create Student (Name, generated Email, Generated Password)
                                Match m = Regex.Match( cellValue != null ? cellValue : "" , patternRegexL, RegexOptions.IgnoreCase);
                                
                                //check if cell value match REGEX (only letters)
                                if (cellValue != "" && m.Success && sch.Count != 0)
                                {

                                    var StudentFullName = cellValue;
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
                                        var newUser = new User
                                        {
                                            Name = StudentFullName,
                                            Email = StudentEmail,
                                            Password = studentPassword.ToString()
                                        };
                                        _userRepository.Add(newUser);
                                        var schId = sch.Where(sc => sc.ClassName == className).FirstOrDefault();
                                        var newStudent = new Student
                                        {
                                            UserId = newUser.Id,
                                            SchoolClassId = schId.Id
                                        };
                                        try
                                        {
                                            _studentRepository.Add(newStudent);
                                        }
                                        catch (Exception e)
                                        {
                                            return NotFound("Intern Error");
                                        }
                                    }
                                    else
                                    {
                                        //Delete data already inserted before the error
                                        _schoolClassRepository.RemoveAll();
                                        _userRepository.RemoveAll();
                                        return Unauthorized("One field value is not allowed ");
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
