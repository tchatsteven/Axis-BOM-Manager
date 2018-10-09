using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AXISAutomation.Solvers.FixtureConfiguration;
using AXISAutomation.Tools.Logging;
using AXISAutomation.Solvers.SectionSolving;
using AXISAutomation.Solvers.LightingEmittersLayout.BEAMCartridges;
//using AXISAutomation.Solvers.LightingEmittersLayout.SCRTape;
using AXISAutomation.Solvers.LightingEmittersLayout.BEAMCartridges.Exceptions;
using AXISAutomation.Solvers.FixtureSetupCodeParser;
using AXISAutomation.Tools.DBConnection;
using BOM_CLASS;



namespace AXISAutomation.Solvers.BOM
{
    public class _BOM
    {
        public AutomationEntitiesBOM DbConn;
        _FixtureConfiguration _fixtureConfiguration;
        String _FullOrderingCode;
        List<_BOMSection> BOM_Sections = new List<_BOMSection>();
        List<IBOMItem> FlatBOM = new List<IBOMItem>();
      
        AXIS_AutomationEntities FixtureConfigurtorDBConn = new AXIS_AutomationEntities();
        public Decimal TabSize = 0.25m;

        public _BOM(String _fixtureCode, AutomationEntitiesBOM dbConn)
        {

            _FullOrderingCode = _fixtureCode;
            DbConn = dbConn;
            _fixtureConfiguration = new _FixtureConfiguration(FullOrderingCode, FixtureConfigurtorDBConn);

            //try
            //{
                InitBOMSections();
                FlatBOMList();
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
                    FlatBOM.Add((IBOMItem)AssemblyItems);

                    foreach (_Part PartItems in AssemblyItems._Parts)
                    {
                        FlatBOM.Add((IBOMItem)PartItems);
                    }
                }
                //Add Part(s) at root assembly
                foreach (_Part PartItems in BomSectionItems._RootAssembly._Parts)
                {
                    FlatBOM.Add((IBOMItem)PartItems);
                }
            }
        }


        public List<IBOMItem> CollapsedSectionsBOM
        {
            get
            {

                List<IBOMItem> ApplicableFlatBOM = FlatBOM.Where(S => S.EpicorExists == true).GroupBy(G => G.EpicorName).Select(gr => gr.First()).ToList();
                ApplicableFlatBOM.Where(o=> !o.IsIndented ).ToList().ForEach(i => i.Quantity = FlatBOM.Where(o => o.EpicorName == i.EpicorName).Select(oo => oo.Quantity).Sum());
                return ApplicableFlatBOM;
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
            foreach (IBOMItem BOMItems in CollapsedSectionsBOM)
            {
                if (!BOMItems.IsIndented)
                {
                    ApplicableParts.NewMessage().AddText(BOMItems.EpicorName).Tab(2.6m).AddText(BOMItems.EpicorType).Tab(4.2m).AddText(BOMItems.Quantity.ToString()).Log();
                }
                else
                {
                    ApplicableParts.NewMessage().Tab(0.25m).AddText(BOMItems.EpicorName).Tab(2.6m).AddText(BOMItems.EpicorType).Tab(4.2m).AddText(BOMItems.Quantity.ToString()).Log();
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
