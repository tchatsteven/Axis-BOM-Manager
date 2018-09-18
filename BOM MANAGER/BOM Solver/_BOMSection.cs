using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AXISAutomation.Solvers.FixtureConfiguration;
using AXISAutomation.Tools.Logging;
using System.Windows.Forms;
using AXISAutomation.Solvers.SectionSolving;
using AXISAutomation.Solvers.LightingEmittersLayout.BEAMCartridges;
using AXISAutomation.Solvers.LightingEmittersLayout.SCRTape;
using AXISAutomation.Solvers.LightingEmittersLayout.BEAMCartridges.Exceptions;
using AXISAutomation.Solvers.FixtureSetupCodeParser;

namespace BOM_MANAGER
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
        public AXIS_AutomationEntitiesBOM DbConn => MyBOM.DbConn;

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
        public String GetSelectedFinish_Category => Selection.Finish.SelectionBaseValue;
        public String GetSelectedVoltage_Category => Selection.Voltage.SelectionBaseValue;
        public String GetSelectedDriver_Category => Selection.Driver.SelectionBaseValue;
        public String GetSelectedCircuits_Category => Selection.Circuits.SelectionBaseValue;
        public String GetSelectedMounting_Category => Selection.Mounting.SelectionBaseValue;
        public String GetSelectedBattery_Category => Selection.Battery.SelectionBaseValue;
        public String GetSelectedOther_Category => Selection.Other.SelectionBaseValue;
        public String GetSelectedICControl_Category => Selection.IC.SelectionBaseValue;
        public String GetSelectedCustom_Category => Selection.Custom.SelectionBaseValue;
        public String GetSelectedFinish_CustomValue => Selection.Finish.SelectionCustomValue;

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
