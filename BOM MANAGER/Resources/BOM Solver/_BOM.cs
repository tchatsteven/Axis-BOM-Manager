using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AXISAutomation.Solvers.FixtureConfiguration;
using AXISAutomation.Tools.Logging;
using AXISAutomation.Solvers.SectionSolving;
using AXISAutomation.Solvers.LightingEmittersLayout.BEAMCartridges;
using AXISAutomation.Solvers.LightingEmittersLayout.SCRTape;
using AXISAutomation.Solvers.LightingEmittersLayout.BEAMCartridges.Exceptions;
using AXISAutomation.Solvers.FixtureSetupCodeParser;
using AXISAutomation.Tools.DBConnection;

namespace BOM_MANAGER
{
    public class _BOM
    {
        public AXIS_AutomationEntitiesBOM DbConn;
        _FixtureConfiguration _fixtureConfiguration;
        String _FullOrderingCode;
        List<_BOMSection> BOM_Sections = new List<_BOMSection>();
        List<_FlatBOM> FlatBOM = new List<_FlatBOM>();
        List<_FlatBOM> Final_FlatBOM = new List<_FlatBOM>();

        AXIS_AutomationEntities FixtureConfigurtorDBConn = new AXIS_AutomationEntities();
        public Decimal TabSize = 0.25m;

        public _BOM(String _fixtureCode, AXIS_AutomationEntitiesBOM dbConn )
        {

            _FullOrderingCode = _fixtureCode;
            DbConn = dbConn;
            _fixtureConfiguration = new _FixtureConfiguration(FullOrderingCode, FixtureConfigurtorDBConn);

            //try
            //{                
                InitBOMSections();
            //}
            //catch
            //{
                
            //}


        }

        public _FixtureConfiguration FixtureConfiguration => _fixtureConfiguration;
        public _ProductTemplate ProductTemplate => FixtureConfiguration.CustomerRequest.ProductTemplate;
        public String FullOrderingCode => _FullOrderingCode;
        public Int32 SectionCount => FixtureConfiguration.Sections.Items.Count;

        private void InitBOMSections()
        { 
            foreach (_FixtureConfiguration._Section CurrentSolverSection in FixtureConfiguration.Sections.Items)
            {
                BOM_Sections.Add(new _BOMSection(this, CurrentSolverSection));
            }
        }

        public void FlatBOMList()
        {
            //Add Assembly(ies) 
            foreach (_BOMSection BomSectionItems in BOM_Sections)
            {
                foreach (_Assembly AssemblyItems in BomSectionItems._RootAssembly._Assemblies)
                {
                    FlatBOM.Add(new _FlatBOM(AssemblyItems));

                    foreach (_Part PartItems in AssemblyItems._Parts)
                    {
                        FlatBOM.Add(new _FlatBOM(PartItems));
                    }
                }
                //Add Part(s) at root assembly
                foreach (_Part PartItems in BomSectionItems._RootAssembly._Parts)
                {
                    FlatBOM.Add(new _FlatBOM(PartItems));
                }
            }
        }

        public void SummarizeExistingComponentInToRTB_IN(_RTFMessenger ApplicableParts)
        {
           
            foreach (_BOMSection BOMSectionItems in BOM_Sections)
            {
                BOMSectionItems.SummarizeExistingComponentInToRTB(ApplicableParts);
            }
            
        }

        public void SummarizeExistingComponentIntoRTB_FB(_RTFMessenger ApplicableParts)
        {
            var ApplicableFlatBOM  = FlatBOM.Where(S => S.Exist == true).GroupBy(G => G.EpicorName).Select(P => new { EpicorName = P.Key, Quantity = P.Sum(Q => Q.Quantity), EpicorType = P.First().EpicorType, IsIndented = P.First().IsIndented, FilterCount = P.First().FilterCount });

            var NonApplicableFlatBOM = FlatBOM.Where(S => S.Exist == false).GroupBy(G => G.EpicorName).Select(P => new { EpicorName = P.Key, EpicorType = P.First().EpicorType, IsIndented = P.First().IsIndented, FilterCount = P.First().FilterCount });

            foreach (var App in ApplicableFlatBOM)
            {
                if (!App.IsIndented)
                {
                    ApplicableParts.NewMessage().AddText(App.EpicorName).Tab(2.6m).AddText(App.EpicorType).Tab(4.2m).AddText(App.Quantity.ToString()).Log();
                }
                else
                {
                    ApplicableParts.NewMessage().Tab(0.25m).AddText(App.EpicorName).Tab(2.6m).AddText(App.EpicorType).Tab(4.2m).AddText(App.Quantity.ToString()).Log();
                }
            }

        }

        public void SummarizeNonExistingComponentIntoRTB(_RTFMessenger NonApplicableParts)
        {
            foreach (_BOMSection BOMSectionItems in BOM_Sections)
            {
                BOMSectionItems.SummarizeNonExistingCompToRTB(NonApplicableParts);
            }

        }

    }
}
