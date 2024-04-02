
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ManageStudentController : ControllerBase
    {
        [HttpPost("upload")]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file uploaded");
            }

            List<List<string>> excelData = new List<List<string>>();

            var stream = new MemoryStream();
            
            file.CopyTo(stream);
            using(var package = new ExcelPackage(stream))
            {
                var worksheet = package.Workbook.Worksheets.First();
                var rowCount = worksheet.Dimension.Rows;
                var colCount = worksheet.Dimension.Columns;

                for(int col = 1; col <= colCount; col++)
                {
                    List<string> rowData = new List<string>();
                    for (int row = 1; row <= rowCount; row++)
                    {
                        var cellValue = worksheet.Cells[row, col].Value?.ToString();
                        rowData.Add(cellValue);
                    }
                    excelData.Add(rowData);
                }
            }
            stream.Position = 0;
            byte[] fileBytes = stream.ToArray();
            return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "test.xlsx");
            


        }

        [HttpGet("uploadGET")]
        public IActionResult Test()
        {
            var data = new { Message = "Hello, world!" };
            return Ok(data);
        }

    }
}
