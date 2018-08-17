using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AXISAutomation.Lookups.FixtureConfiguration;
using AXISAutomation.Tools.Logging;
using AXISAutomation.Solvers.Sections;
using AXISAutomation.Solvers.LightingEmittersLayout.BEAMCartridges;
using AXISAutomation.Solvers.LightingEmittersLayout.SCRTape;
using AXISAutomation.Solvers.LightingEmittersLayout.BEAMCartridges.Exceptions;
using AXISAutomation.Solvers.FixtureSetupCodeParser;

namespace BOM_MANAGER
{
    public class _BOMSection
    {

        _BOM _MyBOM;
        _Section _SolverSection;
        List<_Assembly> _Assemblies = new List<_Assembly>();
        public List<String> AllSelectedPACAFS = new List<String>();

        public _BOMSection(_BOM bom1, _Section solverSection)
        {
            _MyBOM = bom1;
            _SolverSection = solverSection;
            GetAllPACAFS();
            GetAssemblies();
        }

        public _BOM MyBOM => _MyBOM;
        public AXIS_AutomationEntitiesBOM DbConn => MyBOM.DbConn;

        public Boolean IsAtStart => SolverSection.IsAtStart;
        public Boolean IsAtMiddle => !(SolverSection.IsAtStart && SolverSection.IsAtEnd);
        public Boolean IsAtEnd => SolverSection.IsAtEnd;
        public Decimal Length => SolverSection.Length;
        public Int32 Index => SolverSection.Index;
        _Section SolverSection => _SolverSection;
        _CustomerRequest.SelectionLookups Selection => MyBOM.FixtureConfiguration.Selection;
        public Int32 MyFixtureCode => DbConn.Fixtures.Where(o=> o.Code == GetselectedproductID_Category).First().id;

        //public List<Int32> AllSelectedPACAFS => Selection.AllPACAFS;

        public String GetselectedproductID_Category => Selection.ProductID.SelectionBaseValue;
        public String GetSelectedLumen_Category => Selection.LumensDirect.SelectionBaseValue;
        public String GetSelectedCRI_Category => Selection.CRI.SelectionBaseValue;
        public String GetSelectedColorTemp_Category => Selection.ColorTemperature.SelectionBaseValue;
        public String GetSelectedLength_Category => Selection.Length.SelectionBaseValue;
        public String GetSelectedFinish_Category => Selection.Finish.SelectionBaseValue;
        public String GetSelectedVoltage_Category => Selection.Voltage.SelectionBaseValue;
        public String GetSelectedDriver_Category => Selection.Driver.SelectionBaseValue;
        public String GetSelectedCircuits_Category => Selection.Circuits.SelectionBaseValue;
        public String GetSelectedMounting_Category => Selection.Mounting.SelectionBaseValue;
        public String GetSelectedBattery_Category => Selection.Battery.SelectionBaseValue;
        public String GetSelectedOther_Category => Selection.Other.SelectionBaseValue;
        public String GetSelectedICControl_Category => Selection.IC.SelectionBaseValue;
        public String GetSelectedCustom_Category => Selection.Custom.SelectionBaseValue;
                
        

        public void GetAllPACAFS()
        {

            MyBOM.FixtureConfiguration.CustomerRequest.Template.Categories.Where(c => c.CategoryType != "ProductID").Select(o => o.SelectedParameters.Select(f => f.PACAFID.ToString()).ToList()).ToList().ForEach(o => AllSelectedPACAFS.AddRange(o));
        }

        private void GetAssemblies()
        {
            
            DbConn.AssemblyAtAssemblies.Where(o=> o.FixtureID == MyFixtureCode).ToList().ForEach(o => _Assemblies.Add(new _Assembly(o, this)));

        }

        public void SummarizeBOMSectionsToRTB(_RTFMessenger ApplicableParts, _RTFMessenger NonApplicableParts)
        {
            foreach (_Assembly MyAssemblyItems in _Assemblies)
            {
                MyAssemblyItems.SummarizeMyAssemblyInToRTB(ApplicableParts, NonApplicableParts);
                
            }
        }

    }
}
