
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using System.Security.Cryptography;
using System.Text;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ManageStudentController : ControllerBase
    {
        //create all students, school class / send a new "excel" file to the front with all the new data (Full Name, Email, Password)
        [HttpPost("upload")]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file uploaded");
            }


            //init the worksheet for file send to the front
            var packageToSend = new ExcelPackage();
            var worksheetToSend = packageToSend.Workbook.Worksheets.Add("Sheet1");
            worksheetToSend.Cells[1, 1].Value = "Full Name";
            worksheetToSend.Cells[1, 2].Value = "Email";
            worksheetToSend.Cells[1, 3].Value = "Password";
            worksheetToSend.Cells[1, 4].Value = "School Class";

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
                    var classID = 0;
                    var rowSheet = 2;
                    //Loop over the excel file
                    for (int col = 1; col <= colCount; col++)
                    {
                        for (int row = 1; row <= rowCount; row++)
                        {
                            if(row == 1)
                            {
                                // Create School Class
                                className = worksheet.Cells[row, col].Value?.ToString();
                            }
                            else
                            {
                                //Create Student 
                                var StudentFullName = worksheet.Cells[row, col].Value?.ToString();
                                var StudentEmail = StudentFullName + "@msdmail.com";
                                Random rnd = new Random();
                                int studentPassword = rnd.Next();
                                worksheetToSend.Cells[rowSheet, 1].Value = StudentFullName;
                                worksheetToSend.Cells[rowSheet, 2].Value = StudentEmail;
                                worksheetToSend.Cells[rowSheet, 3].Value = studentPassword;
                                worksheetToSend.Cells[rowSheet, 4].Value = className;
                                rowSheet++;
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

        [HttpGet("uploadGET")]
        public IActionResult Test()
        {
            var data = new { Message = "Hello, world!" };
            return Ok(data);
        }

    }
}
