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
    public class _BOM
    {
        public AXIS_AutomationEntitiesBOM DbConn;
        _FixtureConfiguration _fixtureConfiguration;
        _ProductTemplate _productTemplate;
        String _FullOrderingCode;
        List<_BOMSection> BOM_Sections = new List<_BOMSection>();
       //public static List<_RTFMessage> Message = new List<_RTFMessage>();

        public _BOM(String _fixtureCode, AXIS_AutomationEntitiesBOM dbConn)
        {

            _FullOrderingCode = _fixtureCode;
            DbConn = dbConn;
            _fixtureConfiguration = new _FixtureConfiguration(FullOrderingCode);
            _fixtureConfiguration.ConfigureClientRequest();
            _fixtureConfiguration.ConfigureSections();
            _fixtureConfiguration.ConfigureCoverElements();
            InitBOMSections();
            
        }

        public _FixtureConfiguration FixtureConfiguration => _fixtureConfiguration;
        public _ProductTemplate ProductTemplate => _productTemplate;
        public String FullOrderingCode => _FullOrderingCode;

        private void InitBOMSections()
        { 
            foreach (_Section CurrentSolverSection in FixtureConfiguration.Sections.Items)
            {
                BOM_Sections.Add(new _BOMSection(this, CurrentSolverSection));
            }
        }

        public void SummarizeBOMInToRTB(_RTFMessenger ApplicableParts, _RTFMessenger NonApplicableParts)
        {
            foreach (_BOMSection BOMSectionItems in BOM_Sections)
            {
                BOMSectionItems.SummarizeBOMSectionsToRTB(ApplicableParts, NonApplicableParts);
            }

        }

    }
}
