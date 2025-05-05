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
}

