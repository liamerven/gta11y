using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrandTheftAccessibility
{
    class Setting
    {
        public string displayName;
        public int value;
        public string id;

public Setting(string id, string displayName, int value)
        {
            this.id = id;
            this.displayName = displayName;
            this.value = value;
        }

    }
}
