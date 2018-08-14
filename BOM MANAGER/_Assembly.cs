using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOM_MANAGER
{
    public class _Assembly
    {
        AssemblyView DbDAta;
        _BOM _MyBOM;
        List<_Part> Parts = new List<_Part>();
        public _Assembly(AssemblyView dbData, _BOM bom)
        {
            DbDAta = dbData;
            //DbConn = dbConn;
            _MyBOM = bom;
            GetParts();
        }

        public String Name => DbDAta.Name;
        public String Type => DbDAta.AssemblyType;
        public _BOM MyBOM => _MyBOM;
        public AXIS_AutomationEntitiesBOM DbConn => MyBOM.DbConn;
        public String MySelectedFixture => DbDAta.Code;

        private void GetParts()
        {
             DbConn.PartViews.OrderBy(o => o.PartName).Where(p => p.Code == MySelectedFixture).ToList().ForEach(o=> Parts.Add(new _Part(o,this)));
        }
       
    }
}
