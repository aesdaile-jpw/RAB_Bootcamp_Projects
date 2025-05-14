using Autodesk.Revit.DB.Architecture;
using Autodesk.Revit.DB.Structure;
using Microsoft.VisualBasic.Logging;
using RAB_Bootcamp_Projects.Common;
using ExcelDataReader;

namespace RAB_Bootcamp_Projects
{
    [Transaction(TransactionMode.Manual)]
    public class cmdChallenge03 : IExternalCommand
    {
        public Result Execute( ExternalCommandData commandData, ref string message, ElementSet elements )
        {
            // Revit application and document variables
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;

            // Your Module 03 Challenge code goes here
            // Delete the TaskDialog below and add your code
            // TaskDialog.Show("Module 03 Challenge", "Coming Soon!");

            //  code below is to run excel import, might need fixing from excel import v2
            // NOTE: Copy the ExcelReader.dlls to the Revit addins folder, or it won't work

            // string filePath = @"C:\Users\Adrian.Esdaile\OneDrive\Documents\Code\ArchSmarter\Bootcamp Lessons\RAB_Module 03_Furniture List.xlsx";

            string filePath = @"C:\Users\adria\OneDrive\Documents\Code\ArchSmarter\Bootcamp Lessons\RAB_Module 03_Furniture List.xlsx";

            List<RoomData> roomDataList = ExcelReader.ReadRoomDataFromExcel( filePath );


            using ( Transaction t = new Transaction( doc ) )
            {
                t.Start( "Place objects in rooms" );
                // be cautious with transactions in methods

                RoomPopulator.PopulateRooms( roomDataList, doc );

                t.Commit();
            }

            return Result.Succeeded;
        }
        internal static PushButtonData GetButtonData()
        {
            // use this method to define the properties for this command in the Revit ribbon
            string buttonInternalName = "btnChallenge03";
            string buttonTitle = "Module\r03";

            Common.ButtonDataClass myButtonData = new Common.ButtonDataClass(
                buttonInternalName,
                buttonTitle,
                MethodBase.GetCurrentMethod().DeclaringType?.FullName,
                Properties.Resources.Module03,
                Properties.Resources.Module03,
                "Module 03 Challenge");

            return myButtonData.Data;
        }
    }

}
