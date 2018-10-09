using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BOM_CLASS;

namespace BOM_MANAGER
{
    public partial class PartTypeForm : Form
    {
        AutomationEntitiesBOM db = new AutomationEntitiesBOM();
        public PartTypeForm()
        {
            InitializeComponent();
            ValidateForm();
        }

        private void OKButton_Click(object sender, EventArgs e)
        {
            AvailablePartType NewPartType = new AvailablePartType()
            {
                Name = NewPartTypeTextBox.Text                

            };

            db.AvailablePartTypes.Add(NewPartType);
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
