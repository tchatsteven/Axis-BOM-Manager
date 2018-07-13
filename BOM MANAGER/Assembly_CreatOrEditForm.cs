using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AXISAutomation.Tools.Logging;

namespace BOM_MANAGER
{
    public partial class Assembly_CreatOrEditForm : Form
    {
        _RTFMessenger EditFormMsg;
        AXIS_AutomationEntitiesBOM db = new AXIS_AutomationEntitiesBOM();
        Assembly NewAssembly = null;

        //Constructor for NEW BUTTON
        public Assembly_CreatOrEditForm( )
        {            
            InitializeComponent();
            EditFormMsg = new _RTFMessenger(AssyType_TextBox,0, true) { DefaulSpaceAfter = 0 }; 
            ValidateForm();
            LoadAssemblyType();
            
        }

        //Constructor for EDITING assembly
        public Assembly_CreatOrEditForm( String AssemblyNameToEdit )
        {
            InitializeComponent();
            EditFormMsg = new _RTFMessenger(AssyType_TextBox, 0,true) { DefaulSpaceAfter = 0 };
            LoadAssemblyType();
            NewAssemblyNameTextBox.Text = AssemblyNameToEdit;
            NewAssembly = db.Assemblies.Where(o => o.Name == AssemblyNameToEdit).First();
            LoadAssemblyType();
        }

        private void LoadAssemblyType()
        {
            try
            {
                Assy_Type_ComboBox.DataSource = db.AssemblyTypes.OrderBy(o=>o.AssemblyType1).Where( o=> o.AssemblyType1 != "RT").ToList();
                Assy_Type_ComboBox.DisplayMember = "AssemblyType1";
                Assy_Type_ComboBox.ValueMember = "id";
                Assy_Type_ComboBox.Items.Remove("RT");
                //EditFormMsg.NewMessage().AddText("Assembly Type Successfully Loaded").PrependMessageType().Log();
            }
            catch
            {
                EditFormMsg.NewMessage().AddText("Assembly Type Failed to Load").PrependMessageType().Log();
            }
        }   

        private void OKButton_Click(object sender, EventArgs e)
        {
            if (NewAssembly == null) {
                NewAssembly = new Assembly();
                db.Assemblies.Add(NewAssembly);
            }
            try
            {
                NewAssembly.Name = NewAssemblyNameTextBox.Text;
                NewAssembly.AssemblyTypeID = (Int32)Assy_Type_ComboBox.SelectedValue;


                db.SaveChanges();
                DialogResult = DialogResult.OK;
                Close();
            }
            catch
            {
                EditFormMsg.NewMessage().AddText("Assembly not Added to Database").IsError().PrependMessageType().Log();
            }
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
            else
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
                return (!AssemblyNameFieldIsEmpty && !NameExists);
            }
        }

        private Boolean NameExists
        {
            get
            {
                Boolean MyTest = db.Assemblies.Any(o => o.Name == NewAssemblyNameTextBox.Text);
                if (MyTest)
                {
                    EditFormMsg.NewMessage().AddText("Assembly already Exists").IsError().PrependMessageType().Log();
                }
                return MyTest;
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
