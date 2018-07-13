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
    public partial class AssemblyTypeForm : Form
    {
        AXIS_AutomationEntitiesBOM db = new AXIS_AutomationEntitiesBOM();
        public AssemblyTypeForm()
        {
            InitializeComponent();
            ValidateForm();
        }

        private void OKButton_Click(object sender, EventArgs e)
        {
            NewAssemblyTypeTextBox.CharacterCasing = CharacterCasing.Upper;
            AssemblyType NewAssemblyType = new AssemblyType()
            {                
                AssemblyType1 = NewAssemblyTypeTextBox.Text

            };

            db.AssemblyTypes.Add(NewAssemblyType);
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
            get { return NewAssemblyTypeTextBox.Text == String.Empty; }
        }         

        private void NewAssemblyTypeTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            ValidateForm();
        }
    }
}
