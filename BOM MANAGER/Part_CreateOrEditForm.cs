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
    public partial class Part_CreateOrEditForm : Form
    {
        RTFMessenger.RTFMessenger EditFormMsg;
        AXIS_AutomationEntities db = new AXIS_AutomationEntities();
        Part NewPart = null;

        //Constructor for NEW BUTTON
        public Part_CreateOrEditForm()
        {
            InitializeComponent();
            EditFormMsg = new RTFMessenger.RTFMessenger(PartType_TextBox, true) { DefaulSpaceAfter = 0 };
            ValidateForm();
            LoadAssemblyType();
        }

        //Constructor for EDIT BUTTON
        public Part_CreateOrEditForm( String partNameToEdit)
        {
            InitializeComponent();
            EditFormMsg = new RTFMessenger.RTFMessenger(PartType_TextBox, true) { DefaulSpaceAfter = 0 };
            LoadAssemblyType();
            NewPartNameTextBox.Text = partNameToEdit;
            NewPart = db.Parts.Where(o => o.PartName == partNameToEdit).First();
            LoadAssemblyType();
        }

        private void LoadAssemblyType()
        {
            try
            {
                Part_Type_ComboBox.DataSource = db.PartTypes.ToList();
                Part_Type_ComboBox.DisplayMember = "PartType1";
                Part_Type_ComboBox.ValueMember = "id";
                //EditFormMsg.NewMessage().AddText("Part Type Successfully Loaded to ComboBox").PrependMessageType().Log();
            }
            catch
            {
                EditFormMsg.NewMessage().AddText("Part Type Failed to Load to ComboBox").PrependMessageType().Log();
            }
        }

        private void OKButton_Click(object sender, EventArgs e)
        {
            if (NewPart == null)
            {
                NewPart = new Part();
                db.Parts.Add(NewPart);
            }

            NewPart.PartName = NewPartNameTextBox.Text;
            NewPart.TypeID = (Int32)Part_Type_ComboBox.SelectedValue;


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
                NewPartTypeButton.Enabled = false;
                DeletePartTypeButton.Enabled = false;

            }
            else 
            {
                OKButton.Enabled = false;
                NewPartTypeButton.Enabled = true;
                DeletePartTypeButton.Enabled = true;
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
            get {
                Boolean MyTest = db.Parts.Any(o => o.PartName == NewPartNameTextBox.Text);
                if (MyTest)
                {
                    EditFormMsg.NewMessage().AddText("Part already Exists").IsError().PrependMessageType().Log();
                }
                return MyTest;
            }
        }

        private Boolean AssemblyNameFieldIsEmpty
        {
            get { return NewPartNameTextBox.Text == String.Empty; }
        }

        private void NewPartNameTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            ValidateForm();
        }

        private void NewPartTypeButton_Click(object sender, EventArgs e)

        {
            PartTypeForm NewPartForm = new PartTypeForm();
            DialogResult PartTypeForm = NewPartForm.ShowDialog();

            if(PartTypeForm == DialogResult.OK)
            {
                EditFormMsg.NewMessage().AddText("Part Type Successfully Added to DataBase").PrependMessageType().Log();
            }
            else if(PartTypeForm == DialogResult.Cancel)
            {
                EditFormMsg.NewMessage().AddText("Part Type not Added to Database").PrependMessageType().Log();
            }

            PartType_TextBox.ScrollToCaret();
            LoadAssemblyType();

        }

        private void DeletePartTypeButton_Click(object sender, EventArgs e)
        {
            Int32 currentPartTypeId = Int32.Parse(Part_Type_ComboBox.SelectedValue.ToString());
            String currentPartTypeName = Part_Type_ComboBox.Text;
            DialogResult dialogResult = MessageBox.Show("Are you sure you want to delete Part Tpye: " + currentPartTypeName + "?", "Confirm Part Type deletion", MessageBoxButtons.YesNo);

            if (dialogResult == DialogResult.Yes)
            {
                DeletePartTypeFromDB(currentPartTypeId);
            }
        }

        private void DeletePartTypeFromDB(Int32 PartTypeId)
        {
            db.PartTypes.Remove(db.PartTypes.Find(PartTypeId));
            String partTypeName = db.PartTypes.Find(PartTypeId).PartType1;
            db.SaveChanges();
            EditFormMsg.NewMessage().AddText("Part Type: " + partTypeName + " has been deleted from Database").PrependMessageType().Log();
            // reload Assembly Types after deletion from DB
            LoadAssemblyType();
        }

        
    }
}
