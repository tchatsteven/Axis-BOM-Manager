using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AXISAutomation.Lookups.FixtureConfiguration;

namespace BOM_MANAGER
{
    public partial class BOM_MANAGER : Form
    {
        private String FixtureSetupCode => FixtureSetupCode_TextBox.Text;

        private void GetPart_Button_Click(object sender, EventArgs e)
        {
            _Fixture NewFixture = new _Fixture(FixtureSetupCode);
            NewFixture.ConfigureClientRequest();
            NewFixture.ConfigureSections();
            NewFixture.ConfigureCoverElements();

            Int32 asdf = NewFixture.Sections.Count;

            //Log_RichTextBox.Clear();


        }

        private void Get_Template_Button_Click(object sender, EventArgs e)
        {            
            _Fixture NewFixture = new _Fixture(FixtureSetupCode);
            NewFixture.ConfigureClientRequest();
            NewFixture.ConfigureSections();
            NewFixture.ConfigureCoverElements();

            Log_RichTextBox.Clear();
            NewFixture.CustomerRequest.Template.SummarizeIntoRTB(PartGen);

            Log_RichTextBox.SelectionStart = 0;
            Log_RichTextBox.ScrollToCaret();
        }

        private void Match_Summary_Button_Click(object sender, EventArgs e)
        {
            _Fixture NewFixture = new _Fixture(FixtureSetupCode);
            NewFixture.ConfigureClientRequest();
            NewFixture.ConfigureSections();
            NewFixture.ConfigureCoverElements();

            Log_RichTextBox.Clear();
            NewFixture.CustomerRequest.Template.SummarizeMatchesIntoRTB(PartGen);

            Log_RichTextBox.SelectionStart = 0;
            Log_RichTextBox.ScrollToCaret();

        }

        private void Solve_Mechanical_Button_Click(object sender, EventArgs e)
        {
            _Fixture NewFixture = new _Fixture(FixtureSetupCode);
            NewFixture.ConfigureClientRequest();
            NewFixture.ConfigureSections();
            NewFixture.ConfigureCoverElements();

            Log_RichTextBox.Clear();
            NewFixture.Sections.SummarizeMechanicalIntoRTB(PartGen);

            Log_RichTextBox.SelectionStart = 0;
            Log_RichTextBox.ScrollToCaret();
        }

    }
}
