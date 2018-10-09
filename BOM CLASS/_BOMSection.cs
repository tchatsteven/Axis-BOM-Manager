using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AXISAutomation.Solvers.FixtureConfiguration;
using AXISAutomation.Tools.Logging;
using AXISAutomation.Solvers.SectionSolving;
using AXISAutomation.Solvers.LightingEmittersLayout.BEAMCartridges;
//using AXISAutomation.Solvers.LightingEmittersLayout.SCRTape;
using AXISAutomation.Solvers.LightingEmittersLayout.BEAMCartridges.Exceptions;
using AXISAutomation.Solvers.FixtureSetupCodeParser;
using BOM_CLASS;


namespace AXISAutomation.Solvers.BOM
{
    public class _BOMSection
    {

        _BOM _MyBOM;
        _FixtureConfiguration._Section _SolverSection;
        public _Assembly _RootAssembly;
        public String AssyName_Final = "";
        public Int32? AssyQuantity = 1;

        public _BOMSection(_BOM bom, _FixtureConfiguration._Section solverSection)
        {
            _MyBOM = bom;
            _SolverSection = solverSection;
            InitRootAssembly();
        }
        public String Name => String.Format(@"Section {0}", Index);

        public _BOM MyBOM => _MyBOM;
        public AutomationEntitiesBOM DbConn => MyBOM.DbConn;

        public Boolean IsAtStart => SolverSection.IsAtStart;
        public Boolean IsAtMiddle => (!SolverSection.IsAtStart && !SolverSection.IsAtEnd);
        public Boolean IsAtEnd => SolverSection.IsAtEnd;
        public Decimal Length => SolverSection.Length;
        public Int32 Index => SolverSection.StartingIndex;
        _FixtureConfiguration._Section SolverSection => _SolverSection;
        _CustomerRequest._SelectionLookups Selection => MyBOM.FixtureConfiguration.Selection;
        public String MyFixtureCode => DbConn.Fixtures.Where(o => o.Code == GetselectedproductID_Category).First().Code;

        public List<Int32> AllSelectedPACAFS => MyBOM.FixtureConfiguration.CustomerRequest.Selection.MatchedPACAFSList;

        public String GetselectedproductID_Category => Selection.ProductID.SelectionBaseValue;
        public String GetSelectedLumen_Category => Selection.LumensDirect.SelectionBaseValue;
        public String GetSelectedCRI_Category => Selection.CRI.SelectionBaseValue;
        public String GetSelectedColorTemp_Category => Selection.ColorTemperature.SelectionBaseValue;
        public String GetSelectedLength_Category => Selection.Length.SelectionBaseValue;
        public String GetSelectedOpticsDirect_Category => Selection.OpticsDirect.SelectionBaseValue;
        public String GetSelectedOpticsIndirect_Category => Selection.OpticsIndirect.SelectionBaseValue;
        public String GetSelectedFinish_Category => Selection.Finish.SelectionBaseValue;
        public String GetSelectedFinish_CustomValue => Selection.Finish.SelectionCustomValue;
        public String GetSelectedVoltage_Category => Selection.Voltage.SelectionBaseValue;
        public String GetSelectedDriver_Category => Selection.Driver.SelectionBaseValue;
        public String GetSelectedMounting_Category => Selection.Mounting.SelectionBaseValue;
        public String GetSelectedBattery_Category => Selection.Battery.SelectionBaseValue;
        public String GetSelectedOther_Category => Selection.Other.SelectionBaseValue;
        public String GetSelectedICControl_Category => Selection.IC.SelectionBaseValue;
        public String GetSelectedICControl_CustomValue => Selection.IC.SelectionCustomValue;
        public String GetSelectedCustom_Category => Selection.Custom.SelectionBaseValue;

        //Circuitry
        public String GetSelectedCircuits_Category => Selection.Circuits.SelectionBaseValue;
        public Boolean GetSelectedEMCircuits_Category => Selection.Circuits.Emergency.IsSelected;
        public Int32? GetSelectedEMCircuits_Quantity => Selection.Circuits.Emergency.Quantity;
        public Boolean GetSelectedNLCircuits_Category => Selection.Circuits.NightLight.IsSelected;
        public Int32? GetSelectedNLCircuits_Quantity => Selection.Circuits.NightLight.Quantity;
        public Boolean GetSelectedGTDCircuits_Category => Selection.Circuits.GTD.IsSelected;
        public Int32? GetSelectedGTDCircuits_Quantity => Selection.Circuits.GTD.Quantity;

        public TreeNode RootAssyTreeNode => _RootAssembly.MyTreeNode;

        public List<_Assembly> _Assemblies => _RootAssembly.Assemblies;

        private AssemblyView RootAssemblyView => DbConn.AssemblyViews.Where(o => o.AssemblyName == MyFixtureCode).First();

        private void InitRootAssembly()
        {
            _RootAssembly = new _Assembly(RootAssemblyView, this);
        }

        public void SummarizeExistingComponentInToRTB(_RTFMessenger ApplicableParts)
        {
            _RootAssembly.SummarizeExistingComponentInToRTB(ApplicableParts);

        }

        public void SummarizeNonExistingCompToRTB(_RTFMessenger NonApplicableParts)
        {
           _RootAssembly.SummarizeNonExistingCompInToRTB(NonApplicableParts);
        }


    }
}
