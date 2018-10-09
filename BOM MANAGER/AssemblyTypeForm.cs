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
    public partial class AssemblyTypeForm : Form
    {
        AutomationEntitiesBOM db = new AutomationEntitiesBOM();
        public AssemblyTypeForm()
        {
            InitializeComponent();
            ValidateForm();
        }

        private void OKButton_Click(object sender, EventArgs e)
        {
            NewAssemblyTypeTextBox.CharacterCasing = CharacterCasing.Upper;
            AvailableAssemblyType NewAssemblyType = new AvailableAssemblyType()
            {
                Name = NewAssemblyTypeTextBox.Text

            };

            db.AvailableAssemblyTypes.Add(NewAssemblyType);
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
            get
            {
                if (NewAssemblyTypeTextBox.Text == String.Empty)
                {
                    return true;

                }
                else if (db.AvailableAssemblyTypes.Any(o=> o.Name == NewAssemblyTypeTextBox.Text))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
    

        }         

        private void NewAssemblyTypeTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            ValidateForm();
        }
    }
}
