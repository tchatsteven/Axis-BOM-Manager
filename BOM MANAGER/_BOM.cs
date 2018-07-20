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

    class _BOM
    {
        // OTHER CLASS  ASSOCIATION

        _FixtureConfiguration _fixtureConfiguration;
        

        // CLASS CONSTRUCTORS


        public _BOM(String _fixtureCode)
        {
            _code = _fixtureCode;
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

        public String getselectedproductID_Category { get; set; }
        public String getSelectedLumen_Category { get; set; }
        public String getSelectedCRI_Category { get; set; }
        public String getSelectedColorTemp_Category { get; set; }
        public String getSelectedLength_Category { get; set; }
        public String getSelectedFinish_Category { get; set; }
        public String getSelectedVoltage_Category { get; set; }
        public String getSelectedDriver_Category { get; set; }
        public String getSelectedCircuits_Category { get; set; }
        public String getSelectedMounting_Category { get; set; }
        public String getSelectedBattery_Category { get; set; }
        public String getSelectedOther_Category { get; set; }
        public String getSelectedICControl_Category { get; set; }
        public String getSelectedCustom_Category { get; set; }

        public String getSelectedProductID_PACAF { get; set; }
        public String getSelectedLumen_PACAF { get; set; }
        public String getSelectedCRI_PACAF { get; set; }
        public String getSelectedColorTemp_PACAF { get; set; }
        public String getSelectedLength_PACAF { get; set; }
        public String getSelectedFinish_PACAF { get; set; }
        public String getSelectedVoltage_PACAF { get; set; }
        public String getSelectedDriver_PACAF { get; set; }
        public String getSelectedCircuits_PACAF { get; set; }
        public String getSelectedMounting_PACAF { get; set; }
        public String getSelectedBattery_PACAF { get; set; }
        public String getSelectedOther_PACAF { get; set; }
        public String getSelectedICControl_PACAF { get; set; }
        public String getSelectedCustom_PACAF { get; set; }


        // CLASS MEMBER FUNCTION


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

        public void SummarizeBOMinIntoRTB(_RTFMessenger Messenger, Int32 sectionsIncreament)
        {
            int initCount = sectionsIncreament;
            if (FixtureConfiguration.Sections.Items[initCount].IsAtStart)
            {
                int Temp = initCount + 1;
                Messenger.NewMessage().SetSpaceAfter(0).AddBoldText("Section " + Temp + " Length: ").AddBoldText(FixtureConfiguration.Sections.Items[0].Length.ToString()).Log();

            }
            else if(FixtureConfiguration.Sections.Items[initCount].IsAtEnd)
            {
                int Temp = initCount + 1;
                Messenger.NewMessage().SetSpaceAfter(0).AddBoldText("Section " + Temp + " Length: ").AddBoldText(FixtureConfiguration.Sections.Items[initCount].Length.ToString()).Log();
            }
            else
            {
                int Temp = initCount + 1;
                Messenger.NewMessage().SetSpaceAfter(0).AddBoldText("Section " + Temp + " Length: ").AddBoldText(FixtureConfiguration.Sections.Items[initCount].Length.ToString()).Log();
            }
            initCount++;

        }

        public void GetOrderingCodePACAFs()
        {
            //try
            //{

            getselectedproductID_Category = _fixtureConfiguration.Selection.ProductID.SelectionBaseValue;
            getSelectedLumen_Category = _fixtureConfiguration.Selection.LumensDirect.SelectionBaseValue;
            getSelectedCRI_Category = _fixtureConfiguration.Selection.CRI.SelectionBaseValue;
            getSelectedColorTemp_Category = _fixtureConfiguration.Selection.ColorTemperature.SelectionBaseValue;
            getSelectedLength_Category = _fixtureConfiguration.Selection.Length.SelectionBaseValue;
            getSelectedFinish_Category = _fixtureConfiguration.Selection.Finish.SelectionBaseValue;
            getSelectedVoltage_Category = _fixtureConfiguration.Selection.Voltage.SelectionBaseValue;
            getSelectedDriver_Category = _fixtureConfiguration.Selection.Driver.SelectionBaseValue;
            getSelectedCircuits_Category = _fixtureConfiguration.Selection.Circuits.SelectionBaseValue;
            getSelectedMounting_Category = _fixtureConfiguration.Selection.Mounting.SelectionBaseValue;
            getSelectedBattery_Category = _fixtureConfiguration.Selection.Battery.SelectionBaseValue;
            getSelectedOther_Category = _fixtureConfiguration.Selection.Other.SelectionBaseValue;
            getSelectedICControl_Category = _fixtureConfiguration.Selection.IC.SelectionBaseValue;
            getSelectedCustom_Category = _fixtureConfiguration.Selection.Custom.SelectionBaseValue;

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
