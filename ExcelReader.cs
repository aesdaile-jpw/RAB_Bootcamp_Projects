using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.IO;
using ExcelDataReader;

namespace RAB_Bootcamp_Projects
{
    public class ExcelReader
    {
        public static List<RoomData> ReadRoomDataFromExcel( string filePath )
        {
            var roomDataList = new List<RoomData>();

            // Open the Excel file
            using ( var stream = File.Open( filePath, FileMode.Open, FileAccess.Read ) )
            {
                // Configure ExcelDataReader to handle Excel files
                using ( var reader = ExcelReaderFactory.CreateReader( stream ) )
                {
                    // Convert the data to a DataSet
                    var result = reader.AsDataSet();

                    // Assume the first table contains the data
                    var table = result.Tables[ 0 ];

                    // Start reading from the second row (assuming the first row contains headers)
                    for ( int i = 1; i < table.Rows.Count; i++ )
                    {
                        var row = table.Rows[ i ];

                        string name = row[ 0 ].ToString(); // Column 1: Name
                        string familyName = row[ 1 ].ToString(); // Column 2: FamilyName
                        string familyType = row[ 2 ].ToString(); // Column 3: FamilyType
                        int familyQuantity = int.Parse( row[ 3 ].ToString() ); // Column 4: FamilyQuantity

                        // Create a RoomData instance and add it to the list
                        var roomData = new RoomData( name, familyName, familyType, familyQuantity );
                        roomDataList.Add( roomData );
                    }
                }
            }

            return roomDataList;
        }
    }
}
