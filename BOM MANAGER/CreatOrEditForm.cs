﻿using System;
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
    public partial class CreatOrEditForm : Form
    {
        RTFMessenger.RTFMessenger EditFormMsg;
        AXIS_AutomationEntities db = new AXIS_AutomationEntities();
        Assembly NewAssembly = null;

        //Constructor for NEW
        public CreatOrEditForm( )
        {            
            InitializeComponent();
            EditFormMsg = new RTFMessenger.RTFMessenger(AssyType_TextBox, true) { DefaulSpaceAfter = 0 }; 
            ValidateForm();
            LoadAssemblyType();
            
        }

        //Constructor for EDITING assembly
        public CreatOrEditForm( String AssemblyNameToEdit )
        {
            InitializeComponent();
            EditFormMsg = new RTFMessenger.RTFMessenger(AssyType_TextBox, true) { DefaulSpaceAfter = 0 };
            LoadAssemblyType();
            NewAssemblyNameTextBox.Text = AssemblyNameToEdit;
            NewAssembly = db.Assemblies.Where(o => o.Name == AssemblyNameToEdit).First();
        }

        private void LoadAssemblyType()
        {
            Assy_Type_ComboBox.DataSource = db.AssemblyTypes.ToList();
            Assy_Type_ComboBox.DisplayMember = "AssemblyType1";
            Assy_Type_ComboBox.ValueMember = "id";
        }

        private void OKButton_Click(object sender, EventArgs e)
        {
            if (NewAssembly == null) {
                NewAssembly = new Assembly();
                db.Assemblies.Add(NewAssembly);
            }

            NewAssembly.Name = NewAssemblyNameTextBox.Text;
            NewAssembly.AssemblyTypeID = (Int32)Assy_Type_ComboBox.SelectedValue;

            
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
            if (FormIsValid)
            {
                OKButton.Enabled = true;
                NewAssemblyTypeButton.Enabled = false;
                DeleteAssemblyTypeButton.Enabled = false;

            }
            else if(!FormIsValid)
            {
                OKButton.Enabled =  false;
                NewAssemblyTypeButton.Enabled = true;
                DeleteAssemblyTypeButton.Enabled = true;
            }
            //OKButton.Enabled = FormIsValid;
           
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
            get { return NewAssemblyNameTextBox.Text == String.Empty; }
        }

        private void NewAssemblyNameTextBox_KeyUp(object sender, KeyEventArgs e)
        {            
            ValidateForm( );
        }

        private void NewAssemblyTypeButton_Click(object sender, EventArgs e)
        {
            AssemblyTypeForm NewAssemblyTypeForm = new AssemblyTypeForm();
            DialogResult AssemblyTypeForm = NewAssemblyTypeForm.ShowDialog();

            if (AssemblyTypeForm == DialogResult.OK)
            {

                EditFormMsg.NewMessage().AddText("Assembly Type Successfully Added").PrependMessageType().Log();
            }
            else if(AssemblyTypeForm == DialogResult.Cancel)
            {
                EditFormMsg.NewMessage().AddText("Assembly Type not Added to Database").PrependMessageType().Log();
            }
            AssyType_TextBox.ScrollToCaret();
            //Reload Assy_Type_Table after updates
            LoadAssemblyType();

        }

        private void DeleteAssemblyTypeButton_Click(object sender, EventArgs e)
        {
            Int32 currentAssyTypeId = Int32.Parse(Assy_Type_ComboBox.SelectedValue.ToString());
            String currentAssyTypeName = Assy_Type_ComboBox.Text;
            DialogResult dialogResult = MessageBox.Show ("Are you sure you want to delete Assembly Tpye: " + currentAssyTypeName + "?", "Confirm Assembly Type deletion", MessageBoxButtons.YesNo);

            if (dialogResult == DialogResult.Yes)
            {
                DeleteAssemblyTypeFromDB(currentAssyTypeId);                
            }
        }

        private void DeleteAssemblyTypeFromDB(Int32 assemblyTypeId)
        {
            db.AssemblyTypes.Remove(db.AssemblyTypes.Find(assemblyTypeId));
            String assemblyTypeName = db.AssemblyTypes.Find(assemblyTypeId).AssemblyType1;
            db.SaveChanges();
            EditFormMsg.NewMessage().AddText("Assembly Type: " + assemblyTypeName +" has been deleted from Database").PrependMessageType().Log();
           // reload Assembly Types after deletion from DB
            LoadAssemblyType();
        }

    }
}