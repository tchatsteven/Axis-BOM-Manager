using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOM_MANAGER
{
    public class _Part
    {
        PartView DbData;
        _Assembly _MyAssembly;
        public _Part(PartView dbData, _Assembly myAssembly)
        {
            DbData = dbData;

        }

        public String Name => DbData.PartName;
        public String Type => DbData.PartType;
        public String Description => DbData.Description;
        public _Assembly MyAssembly => _MyAssembly;
    }
}
