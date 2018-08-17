using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AXISAutomation.Tools.Logging;

namespace BOM_MANAGER
{
    public class _Assembly
    {
        AssemblyAtAssembly DbData;
        _BOMSection _MyBOMSection;
        List<_Part> _Parts = new List<_Part>();

        public _Assembly(AssemblyAtAssembly dbData, _BOMSection bomSection)
        {
            DbData = dbData;
            _MyBOMSection = bomSection;
            GetParts();
        }

        public String Name => DbData.ToParent.Name;
        public String Type => DbData.ToParent.AssemblyType.AssemblyType1;
        public _BOMSection MyBOMSection => _MyBOMSection;
        public AXIS_AutomationEntitiesBOM DbConn => MyBOMSection.DbConn;
        public Int32 Id => DbData.id;
        public _BOM MyBOM => MyBOMSection.MyBOM;
        public Int32 MyFixtureCode => DbConn.Fixtures.Where(o => o.Code == MyBOMSection.GetselectedproductID_Category).First().id;//MyBOMSection.MyFixtureCode;


        private void GetParts()
        {
            DbConn.PartAtAssemblies.Where(p => p.FixtureID == MyFixtureCode && p.AssRefID == Id).OrderBy(p=> p.Part.PartName).ToList().ForEach(o => _Parts.Add(new _Part(o, this)));
        }

        public void SummarizeMyAssemblyInToRTB(_RTFMessenger ApplicableParts, _RTFMessenger NonApplicableParts)
        {
            ApplicableParts.NewMessage().AddBoldText("Section " + (_MyBOMSection.Index+1) + " Length: ").AddBoldText(_MyBOMSection.Length.ToString()).Log();
            NonApplicableParts.NewMessage().AddBoldText("Section " + (_MyBOMSection.Index+1) + " Length: ").AddBoldText(_MyBOMSection.Length.ToString()).Log();

            foreach (_Part PartItems in _Parts)
            {
                PartItems.SummarizeMyPartsInToRTB( ApplicableParts,  NonApplicableParts);
            }
        }




        }
}
