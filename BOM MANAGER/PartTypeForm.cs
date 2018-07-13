using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BOM_MANAGER
{
    public partial class PartTypeForm : Form
    {
        AXIS_AutomationEntitiesBOM db = new AXIS_AutomationEntitiesBOM();
        public PartTypeForm()
        {
            InitializeComponent();
            ValidateForm();
        }

        private void OKButton_Click(object sender, EventArgs e)
        {
            PartType NewPartType = new PartType()
            {
                PartType1 = NewPartTypeTextBox.Text                

            };

            db.PartTypes.Add(NewPartType);
            db.SaveChanges();
            DialogResult = DialogResult.OK;
            Close();
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void ValidateForm()
        {
            OKButton.Enabled = FormIsValid;
        }

        private Boolean FormIsValid
        {
            get
            {
                return !AssemblyNameFieldIsEmpty;
            }
        }

        private Boolean AssemblyNameFieldIsEmpty
        {
            get { return NewPartTypeTextBox.Text == String.Empty; }
        }

        private void NewPartTypeTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            ValidateForm();
        }
    }
}
