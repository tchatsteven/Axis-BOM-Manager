using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BOM_Data_Manager
{
    public partial class createEditForm : Form
    {
        public String context, operation, componentName, componentDescription;
        public createEditForm(String Context, String Operation, String ComponentName = "", String ComponentDescription = "")
        {            
            InitializeComponent();
            context = Context;
            operation = Operation;            
            componentName = ComponentName;
            componentDescription = ComponentDescription;
            setAllLabeling();
        }

        private void setAllLabeling()
        {
            setTitle();
            setFrameTitle();
            setComponentNameTitle();
            setComponentDescriptionTitle();
            setComponentNameText();
            setComponentDescriptionText();
        }

        private void setTitle()
        {
            formTitle.Text = operation == "new" ? "Create New " + CapitalizeFirstCharacter(context) + ":" : "Edit " + CapitalizeFirstCharacter(context);
        }

        private void setFrameTitle()
        {           
            this.Text = context == "new" ? "Create New " + CapitalizeFirstCharacter(context) + ":" : "Edit " + CapitalizeFirstCharacter(context);
        }

        private void cancelCreatePartButton_Click(object sender, EventArgs e)
        {            
            this.Close();
        }

        private void createPartOKButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;          
            this.Close();
        }

        public String newComponentName
        {
            get { return componentNameTextBox.Text; }
        }

        public String newComponentDescription
        {
            get { return componentDescriptionTextBox.Text; }
        }

        public String CapitalizeFirstCharacter(String targetString)
        {
            return targetString[0].ToString().ToUpper() + targetString.Substring(1).ToLower();
        }

        private void setComponentNameTitle()
        {
            componentNameLabel.Text = CapitalizeFirstCharacter(context) + " Name";
        }

        private void createEditForm_KeyPress(object sender, KeyPressEventArgs e)
        {
            MessageBox.Show(e.KeyChar.ToString());
        }

        private void setComponentDescriptionTitle()
        {
            componentDescriptionLabel.Text = CapitalizeFirstCharacter(context) + " Description";
        }

        private void setComponentNameText()
        {
            componentNameTextBox.Text = componentName;
        }

        private void setComponentDescriptionText()
        {
            componentDescriptionTextBox.Text = componentDescription;
        }


    }
}
