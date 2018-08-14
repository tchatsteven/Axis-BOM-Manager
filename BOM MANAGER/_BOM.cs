using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AXISAutomation.Lookups.FixtureConfiguration;
using AXISAutomation.Tools.Logging;
using AXISAutomation.Solvers.Sections;
using AXISAutomation.Solvers.LightingEmittersLayout.BEAMCartridges;
using AXISAutomation.Solvers.LightingEmittersLayout.SCRTape;
using AXISAutomation.Solvers.LightingEmittersLayout.BEAMCartridges.Exceptions;
using AXISAutomation.Solvers.FixtureSetupCodeParser;

namespace BOM_MANAGER
{

    public class _BOM
    {
        // OTHER CLASS  ASSOCIATION
        public AXIS_AutomationEntitiesBOM DbConn;
        _FixtureConfiguration _fixtureConfiguration;
        List<_Assembly> Assemblies = new List<_Assembly>();

        // CLASS CONSTRUCTORS


        public _BOM(String _fixtureCode)
        {
            _code = _fixtureCode;
            
        }

        public _BOM(String _fixtureCode, AXIS_AutomationEntitiesBOM dbConn)
        {
            _code = _fixtureCode;
            DbConn = dbConn;
            GetAssemblies();
        }

        public _FixtureConfiguration FixtureConfiguration
        {
            get
            {
                return _fixtureConfiguration;
            }
        }

               
        // CLASS MEMBER VARIABLE

        public String _code;

        public String Code
        {
            get
            {
                return _code;
            }
        }

        //public String RequestedLength
        //{
        //    get { return SectionSolver.RequestedLength; }
        //}

        //public Decimal RequestedLengthAsDecimalInch
        //{
        //    get { return SectionSolver.ConvertToDecimalInch(RequestedLength); }
        //}

        //public Decimal ActualExtrusionsLength
        //{
        //    get { return SectionSolver.ActualExtrusionsLength; }
        //}

        public List<String> All_PACAFs;

        public String GetselectedproductID_Category { get; set; }
        public String GetSelectedLumen_Category { get; set; }
        public String GetSelectedCRI_Category { get; set; }
        public String GetSelectedColorTemp_Category { get; set; }
        public String GetSelectedLength_Category { get; set; }
        public String GetSelectedFinish_Category { get; set; }
        public String GetSelectedVoltage_Category { get; set; }
        public String GetSelectedDriver_Category { get; set; }
        public String GetSelectedCircuits_Category { get; set; }
        public String GetSelectedMounting_Category { get; set; }
        public String GetSelectedBattery_Category { get; set; }
        public String GetSelectedOther_Category { get; set; }
        public String GetSelectedICControl_Category { get; set; }
        public String GetSelectedCustom_Category { get; set; }
                      
        public String GetSelectedProductID_PACAF { get; set; }
        public String GetSelectedLumen_PACAF { get; set; }
        public String GetSelectedCRI_PACAF { get; set; }
        public String GetSelectedColorTemp_PACAF { get; set; }
        public String GetSelectedLength_PACAF { get; set; }
        public String GetSelectedFinish_PACAF { get; set; }
        public String GetSelectedVoltage_PACAF { get; set; }
        public String GetSelectedDriver_PACAF { get; set; }
        public String GetSelectedCircuits_PACAF { get; set; }
        public String GetSelectedMounting_PACAF { get; set; }
        public String GetSelectedBattery_PACAF { get; set; }
        public String GetSelectedOther_PACAF { get; set; }
        public String GetSelectedICControl_PACAF { get; set; }
        public String GetSelectedCustom_PACAF { get; set; }


        // CLASS MEMBER FUNCTION

        public void GetAssemblies()
        {
            //DbConn.AssemblyViews.AssemblyViews.Where(o => o.Code == MySelectedFixture).ToList();
        }


        public void TestconfigSection(_RTFMessenger Messenger)
        {
            try
            {
                _fixtureConfiguration = new _FixtureConfiguration(Code);
                _fixtureConfiguration.ConfigureClientRequest();
                _fixtureConfiguration.ConfigureSections();
                _fixtureConfiguration.ConfigureCoverElements();

            }
            catch 
            {
                Messenger.NewMessage().SetSpaceAfter(0).AddText("Request for: ").NewLine().AddBoldText("Fixture Setup Code Parsing Failed").Log();
                
            }
            
        }

        public void SummarizeBOMinIntoRTB(_RTFMessenger Messenger, Int32 sectionsIncreament, ref Decimal currentSectionLength,ref  String sectionDefinition)
        {
            int initCount = sectionsIncreament;
            
            if (FixtureConfiguration.Sections.Items[initCount].IsAtStart)
            {
                int Temp = initCount + 1;
                currentSectionLength = FixtureConfiguration.Sections.Items[0].Length;
                sectionDefinition = "START";
                Messenger.NewMessage().SetSpaceAfter(0).AddBoldText("Section " + Temp + " Length: ").AddBoldText(FixtureConfiguration.Sections.Items[0].Length.ToString()).Log();
            }
            else if(FixtureConfiguration.Sections.Items[initCount].IsAtEnd)
            {
                int Temp = initCount + 1;
                currentSectionLength = FixtureConfiguration.Sections.Items[initCount].Length;
                sectionDefinition = "END";
                Messenger.NewMessage().SetSpaceAfter(0).AddBoldText("Section " + Temp + " Length: ").AddBoldText(FixtureConfiguration.Sections.Items[initCount].Length.ToString()).Log();
            }
            else
            {
                int Temp = initCount + 1;
                currentSectionLength = FixtureConfiguration.Sections.Items[initCount].Length;
                sectionDefinition = "MIDDLE";
                Messenger.NewMessage().SetSpaceAfter(0).AddBoldText("Section " + Temp + " Length: ").AddBoldText(FixtureConfiguration.Sections.Items[initCount].Length.ToString()).Log();
            }
            initCount++;

        }

        public void GetOrderingCodePACAFs()
        {
            //try
            //{

            GetselectedproductID_Category = _fixtureConfiguration.Selection.ProductID.SelectionBaseValue;
            GetSelectedLumen_Category = _fixtureConfiguration.Selection.LumensDirect.SelectionBaseValue;
            GetSelectedCRI_Category = _fixtureConfiguration.Selection.CRI.SelectionBaseValue;
            GetSelectedColorTemp_Category = _fixtureConfiguration.Selection.ColorTemperature.SelectionBaseValue;
            GetSelectedLength_Category = _fixtureConfiguration.Selection.Length.SelectionBaseValue;
            GetSelectedFinish_Category = _fixtureConfiguration.Selection.Finish.SelectionBaseValue;
            GetSelectedVoltage_Category = _fixtureConfiguration.Selection.Voltage.SelectionBaseValue;
            GetSelectedDriver_Category = _fixtureConfiguration.Selection.Driver.SelectionBaseValue;
            GetSelectedCircuits_Category = _fixtureConfiguration.Selection.Circuits.SelectionBaseValue;
            GetSelectedMounting_Category = _fixtureConfiguration.Selection.Mounting.SelectionBaseValue;
            GetSelectedBattery_Category = _fixtureConfiguration.Selection.Battery.SelectionBaseValue;
            GetSelectedOther_Category = _fixtureConfiguration.Selection.Other.SelectionBaseValue;
            GetSelectedICControl_Category = _fixtureConfiguration.Selection.IC.SelectionBaseValue;
            GetSelectedCustom_Category = _fixtureConfiguration.Selection.Custom.SelectionBaseValue;

            //getSelectedLumen_PACAF =  (NewFixture.Selection.LumensDirect.SelectionPACAF);
            //getSelectedCRI_PACAF =  (NewFixture.Selection.CRI.SelectionPACAF);
            //getSelectedColorTemp_PACAF =  (NewFixture.Selection.ColorTemperature.SelectionPACAF);
            //getSelectedLength_PACAF =  (NewFixture.Selection.Length.SelectionPACAF);
            //getSelectedFinish_PACAF =  (NewFixture.Selection.Finish.SelectionPACAF);
            //getSelectedVoltage_PACAF =  (NewFixture.Selection.Voltage.SelectionPACAF);
            //getSelectedDriver_PACAF =  (NewFixture.Selection.Driver.SelectionPACAF);
            //getSelectedCircuits_PACAF =  (NewFixture.Selection.Circuits.SelectionPACAF);
            //getSelectedMounting_PACAF =  (NewFixture.Selection.Mounting.SelectionPACAF);
            //getSelectedBattery_PACAF =  (NewFixture.Selection.Battery.SelectionPACAF) ;
            //getSelectedOther_PACAF =  (NewFixture.Selection.Other.SelectionPACAF);
            //getSelectedICControl_PACAF =  (NewFixture.Selection.IC.SelectionPACAF);
            //getSelectedCustom_PACAF =  (NewFixture.Selection.Custom.SelectionPACAF);

            All_PACAFs = new List<String>();
            List<List<String>> Something = _fixtureConfiguration.CustomerRequest.Template.Categories.Where(c => c.CategoryType != "ProductID").Select(o => o.SelectedParameters.Select(f => f.PACAFID.ToString()).ToList()).ToList();
            Something.ForEach(o => All_PACAFs.AddRange(o));

            //}

            //catch
            //{

            //}



        }







    }

}
