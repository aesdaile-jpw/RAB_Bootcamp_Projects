using Autodesk.Revit.DB.Architecture;
using Autodesk.Revit.DB.Structure;
using RAB_Bootcamp_Projects.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RAB_Bootcamp_Projects
{
    public class Neighbourhood
    {
        public string Name { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public List<Building> BuildingList { get; set; }

        public Neighbourhood( string _name, string _city, string _state, List<Building> _buildingList )
        {
            Name = _name;
            City = _city;
            State = _state;
            BuildingList = _buildingList;
        }

        public int GetBuildingCount()
        {
            return BuildingList.Count;
        }
    }

    // create class
    public class Building
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public int NumberOfFloors { get; set; }
        public double Area { get; set; }

        // add constructor to class
        public Building( string _name, string _address, int _numberOfFloors, double _area )
        {
            Name = _name;
            Address = _address;
            NumberOfFloors = _numberOfFloors;
            Area = _area;
        }
    }

    /// <summary>
    /// This class is used to store data for each room in the Excel file.
    /// </summary>
    public class RoomData
    {
        public string Name { get; set; }
        public string FamilyName { get; set; }
        public string FamilyType{ get; set; }
        public int FamilyQuantity { get; set; }
        public RoomData( string _name, string _FamilyName, string _FamilyType, int _FamilyQuantity )
        {
            Name = _name;
            FamilyName = _FamilyName;
            FamilyType = _FamilyType;
            FamilyQuantity = _FamilyQuantity;
        }
    }

    /// <summary>
    /// This class is used to populate rooms with furniture.
    /// </summary>
    public class RoomPopulator
    {
        /// <summary>
        /// Populate rooms with furniture from a list of room data.
        /// </summary>
        /// <param name="roomDataList"></param>
        /// <param name="doc"></param>
        public static void PopulateRooms( List<RoomData> roomDataList, Document doc )
        {

            for ( int i = 0; i < roomDataList.Count; i++ )
            {
                string roomName = roomDataList[ i ].Name;
                string familySymbol = roomDataList[ i ].FamilyName;
                string familyType = roomDataList[ i ].FamilyType;
                int familyQuantity = roomDataList[ i ].FamilyQuantity;
                // TaskDialog.Show( "Test", $"Room Name: {roomName}, Family Symbol: {familySymbol}, Family Type: {familyType}, Family Quantity: {familyQuantity}" );
                FilteredElementCollector roomCollector = new FilteredElementCollector( doc );
                roomCollector.OfCategory( BuiltInCategory.OST_Rooms );

                foreach ( Room room in roomCollector )
                {
                    double furnitureCount = Utils.GetParameterValueAsDouble( room, "Furniture Count" );
                    string roomString = Utils.GetParameterValueAsString( room, BuiltInParameter.ROOM_NAME );

                    if ( roomString.Contains(roomName)  )
                    {
                        FamilySymbol curFamily = Utils.GetFamilySymbolByName( doc, familySymbol, familyType );
                        curFamily.Activate(); // loads family type into project
                        Location roomLocation = room.Location;
                        LocationPoint roomLocPt = roomLocation as LocationPoint;
                        // XYZ roomPoint = roomLocPt.Point;

                        for ( int j = 0; j < familyQuantity; j++ )
                        {
                            // create family instance
                            FamilyInstance curFamInstance = doc.Create.NewFamilyInstance( roomLocPt.Point, curFamily, StructuralType.NonStructural );
                            furnitureCount++;
                            Utils.SetParameterValue( room, "Furniture Count", furnitureCount );
                        }


                    }
                }
            }
        }
    }
}

