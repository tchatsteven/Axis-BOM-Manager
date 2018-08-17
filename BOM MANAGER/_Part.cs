using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AXISAutomation.Tools.Logging;


namespace BOM_MANAGER
{
    public class _Part
    {
        PartAtAssembly DbData;
        _Assembly _MyAssembly;
        public Boolean _Exists;
        public Int32? Quantity = 1;
        List<_Filter> Filters = new List<_Filter>();

        public _Part(PartAtAssembly dbData, _Assembly myAssembly)
        {
            DbData = dbData;
            _MyAssembly = myAssembly;

            Get_Filters();
            Process_Filter();
        }

        public String Name => DbData.Part.PartName;
        public String Type => DbData.Part.PartType.PartType1;
        public String Description => DbData.Part.Description;
        public _Assembly MyAssembly => _MyAssembly;
        public AXIS_AutomationEntitiesBOM DbConn => MyBOM.DbConn;
        public String MyFixtureCode => MyBOM.FixtureConfiguration.ProductCode;
        public _BOM  MyBOM => MyAssembly.MyBOM;


        private void Get_Filters()
        {
            DbConn.PartRulesFilters.Where(o => o.PartName == Name && o.ProductCode == MyFixtureCode).OrderBy(p=> p.OrderOfExecution).ToList().ForEach(o=> Filters.Add(new _Filter(o,this)));
        }

        private void Process_Filter()
        {
            foreach (_Filter item in Filters)
            {
                item.Run();
            }
        }

        public Boolean Exists
        {
            set => _Exists = value;
            get => _Exists && Quantity > 0;

        }

        public void SummarizeMyPartsInToRTB(_RTFMessenger ApplicableParts, _RTFMessenger NonApplicableParts)
        {
            if (_Exists)
            {
                ApplicableParts.NewMessage().AddText("\t" + Name + "\t" + Type + "\t" + Quantity.ToString()).Log();
            }

            else
            {
                NonApplicableParts.NewMessage().AddText("\t" + Name + ":\t" + Type + "\tNOT APPLICABLE/NO RULE").Log();
            }
        }


    }
}
