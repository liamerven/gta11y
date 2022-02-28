using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrandTheftAccessibility
{
    class Location
    {
    public string name;
        public GTA.Math.Vector3 coords;
        public Location(string name, GTA.Math.Vector3 coords)
        {
            this.name = name;
            this.coords = coords;

        }
    
    }
}
