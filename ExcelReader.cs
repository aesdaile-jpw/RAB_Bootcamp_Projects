using System;
using System.Collections.Generic;
using System.IO;
using OfficeOpenXml; // Install EPPlus via NuGet

namespace RAB_Bootcamp_Projects
{
    public class ExcelReader
    {
        public static List<RoomData> ReadRoomDataFromExcel(string filePath)
        {
            var roomDataList = new List<RoomData>();

            // Ensure EPPlus license context is set
            ExcelPackage.License.SetNonCommercialPersonal( "Adrian Esdaile" );
            // ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
          

            // Open the Excel file
            using (var package = new ExcelPackage(new FileInfo(filePath)))
            {
                var worksheet = package.Workbook.Worksheets[0]; // Get the first worksheet
                int rowCount = worksheet.Dimension.Rows; // Get the number of rows

                // Start reading from the second row (assuming the first row contains headers)
                for (int row = 2; row <= rowCount; row++)
                {
                    string name = worksheet.Cells[row, 1].Text; // Column 1: Name
                    string familyName = worksheet.Cells[row, 2].Text; // Column 2: FamilyName
                    string familyType = worksheet.Cells[row, 3].Text; // Column 3: FamilyType
                    int familyQuantity = int.Parse(worksheet.Cells[row, 4].Text); // Column 4: FamilyQuantity

                    // Create a RoomData instance and add it to the list
                    var roomData = new RoomData(name, familyName, familyType, familyQuantity);
                    roomDataList.Add(roomData);
                }
            }

            return roomDataList;
        }
    }
}
