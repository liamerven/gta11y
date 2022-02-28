using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GTA;
using GTA.Native;

namespace GrandTheftAccessibility
{
    class VehicleSpawn : IComparable
    {
    public string name;
        public VehicleHash id;

        public int CompareTo(object obj)
        {
            VehicleSpawn c2 = (VehicleSpawn)obj;
            return name.CompareTo(c2.name);
        }


        public VehicleSpawn(string name, VehicleHash id)
        {
            this.name = name;
            this.id = id;
            if (name == "")
                this.name = Function.Call<string>(Hash.GET_DISPLAY_NAME_FROM_VEHICLE_MODEL, id);
        }
    
    }
}
