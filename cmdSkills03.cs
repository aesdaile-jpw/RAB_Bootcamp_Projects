using Autodesk.Revit.DB.Architecture;
using Autodesk.Revit.DB.Structure;
using RAB_Bootcamp_Projects.Common;


namespace RAB_Bootcamp_Projects
{
    [Transaction(TransactionMode.Manual)]
    public class cmdSkills03 : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            // Revit application and document variables
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;

            // Your Module 03 Skills code goes here
            // Delete the TaskDialog below and add your code
            // TaskDialog.Show("Module 03 Skills", "Got Here!");

            // Class is like a blueprint for an object
            // Dynamic classes need NEW to instanciate
            // Static classes do not need NEW to instanciate
            // Static classes are like a toolbox, containing methods and properties
            // Static classes are not instanciated, but can be used directly

            // Create a new instance of the Building class
            Building building1 = new Building("Big Office Building", "10 Main street", 10, 150000);
            //building.Name = "Big office building";
            //building.Address = "10 Main street";
            //building.NumberOfFloors = 10;
            //building.Area = 150000;
            Building building2 = new Building( "Small Office Building", "20 Main street", 5, 50000 );
            // can change properties after instanciation
            building1.NumberOfFloors = 11;

            List<Building> buildings = new List<Building>();
            buildings.Add( building1 );
            buildings.Add( building2 );
            buildings.Add(new Building( "Medium Office Building", "30 Main street", 7, 70000 ) );
            buildings.Add(new Building("Giant store", "15 somewhere", 5, 200000 ) );

            // create neighbourhood object
            Neighbourhood downtown = new Neighbourhood("Downtown", "Middletown", "CT", buildings );

            TaskDialog.Show("Test", $"There are {downtown.GetBuildingCount()} buildings in {downtown.Name} neighbourhood." );

            // working with rooms
            FilteredElementCollector roomCollector = new FilteredElementCollector( doc );
            roomCollector.OfCategory( BuiltInCategory.OST_Rooms );
            // roomCollector.OfCategory(BuiltInCategory.OST_MEPSpaces );

            Room curRoom = roomCollector.First() as Room;
            // get room name
            string roomName = curRoom.Name;
            // note Revit combines room Name and room Number as a single string
            // check room Name only
            if(roomName.Contains( "Room" ) )
            {
                TaskDialog.Show( "Room Name", $"The room name is {roomName}." );
            }

            // get room point
            Location roomLocation = curRoom.Location;
            LocationPoint roomLocPt = roomLocation as LocationPoint;
            XYZ roomPoint = roomLocPt.Point;

            // families
            // in code, Family Symbol is equivalent to Family Type in Revit itself

            using(Transaction t = new Transaction( doc ) )
            {
                t.Start( "Inset Families into rooms" );


                // insert families (ItemFactoryBase)
                FamilySymbol curFamSymbol = Utils.GetFamilySymbolByName( doc, "Desk", "Large" );
                curFamSymbol.Activate(); // loads family type into project

                foreach (Room curRoom2 in roomCollector )
                {
                   LocationPoint loc = curRoom2.Location as LocationPoint;

                    FamilyInstance curFamInstance = doc.Create.NewFamilyInstance(loc.Point, curFamSymbol, StructuralType.NonStructural );

                    //string area = GetParameterValueAsString( curRoom2, "Area" );

                    string department = Utils.GetParameterValueAsString(curRoom2, "Department" );
                    double area = Utils.GetParameterValueAsDouble( curRoom2, BuiltInParameter.ROOM_AREA );
                    double area2 = Utils.GetParameterValueAsDouble(curRoom, "Area" );

                    Utils.SetParameterValue(curRoom2, "Department", "Architecture" );


                }
                t.Commit();
            }

            // getting ans setting parameters




            return Result.Succeeded;
        }


    }



  
}
