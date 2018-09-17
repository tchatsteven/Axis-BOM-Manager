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
    public partial class Part_CreateOrEditForm : Form
    {
        _RTFMessenger EditFormMsg;
        AXIS_AutomationEntitiesBOM db = new AXIS_AutomationEntitiesBOM();
        Part newPart = null;

        //Constructor for NEW BUTTON
        public Part_CreateOrEditForm()
        {
            InitializeComponent();
            EditFormMsg = new _RTFMessenger(PartType_TextBox, 0,true) { DefaulSpaceAfter = 0 };
            ValidateForm();
            LoadAssemblyType();
        }

        //Constructor for EDIT BUTTON
        public Part_CreateOrEditForm( String partNameToEdit, String description, String partTypeToEdit)//, String FamilyName)
        {
            InitializeComponent();
            EditFormMsg = new _RTFMessenger(PartType_TextBox, 0,true) { DefaulSpaceAfter = 0 };            
            NewPartNameTextBox.Text = partNameToEdit;
            PartNameDescriptionTextBox.Text = description;            
            newPart = db.Parts.Where(o => o.Name == partNameToEdit).First();
            LoadAssemblyType(partTypeToEdit);//, FamilyName);
        }

        private void LoadAssemblyType()
        {
            try
            {
                Part_Type_ComboBox.DataSource = db.AvailablePartTypes.OrderBy(o => o.Name).ToList();
                Part_Type_ComboBox.DisplayMember = "Name";
                Part_Type_ComboBox.ValueMember = "id";
                
            }
            catch
            {
                EditFormMsg.NewMessage().AddText("Part Type Failed to Load to ComboBox").PrependMessageType().Log();
            }
        }

        private void LoadAssemblyType(String PartToEditComboboxSelectedText)//, String FamilyNameToEditComboboxSelectedText)
        {
            try
            {
                Part_Type_ComboBox.DataSource = db.AvailablePartTypes.OrderBy(o=>o.Name).ToList();
                Part_Type_ComboBox.DisplayMember = "Name";
                Part_Type_ComboBox.ValueMember = "id";
                Part_Type_ComboBox.Text = PartToEditComboboxSelectedText;
                                
            }
            catch
            {
                EditFormMsg.NewMessage().AddText("Part Type Failed to Load to ComboBox").PrependMessageType().Log();
            }
        }

        private void OKButton_Click(object sender, EventArgs e)
        {
            if (newPart == null)
            {
                newPart = new Part();
                db.Parts.Add(newPart);
            }
            try
            {
                newPart.Name = NewPartNameTextBox.Text;
                newPart.Description = PartNameDescriptionTextBox.Text;
                newPart.TypeID = (Int32)Part_Type_ComboBox.SelectedValue;

                db.SaveChanges();
                DialogResult = DialogResult.OK;
                Close();

            }
            catch
            {
                EditFormMsg.NewMessage().AddText("Part not Added to Database").IsError().PrependMessageType().Log();
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
                    Boolean MyTest = db.Parts.Any(o => o.Name == NewPartNameTextBox.Text);
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
            PartType_TextBox.Clear();
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
                EditFormMsg.NewMessage().AddText("Part Type not Added to Database").IsError().PrependMessageType().Log();
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
            //Int32 PartTypeID = db.PartTypes.Find(PartTypeId).id;//comment this out
            String partTypeName = db.AvailablePartTypes.Find(PartTypeId).Name;
            Boolean HaveAssociation = db.Parts.Any(o=> PartTypeId == o.TypeID);

            if (!HaveAssociation)
            {
                db.AvailablePartTypes.Remove(db.AvailablePartTypes.Find(PartTypeId));
                //db.SaveChanges();
                EditFormMsg.NewMessage().AddText("Part Type " + partTypeName + " has been deleted from Database").PrependMessageType().Log();
                // reload Assembly Types after deletion from DB
                LoadAssemblyType();

            }
            else
            {
                EditFormMsg.NewMessage().AddText("Part Type " + partTypeName + " has not been deleted due to Association in Part Table").IsError().PrependMessageType().Log();
            }

        }

        
    }
}
