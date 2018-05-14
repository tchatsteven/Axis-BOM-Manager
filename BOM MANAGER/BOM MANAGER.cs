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
    public partial class BOM_MANAGER : Form
    {
        RTFMessenger.RTFMessenger BomManagerFormMsg;
        AXIS_AutomationEntities db;

        public BOM_MANAGER()
        {
            InitializeComponent();
            db = new AXIS_AutomationEntities();
            BomManagerFormMsg = new RTFMessenger.RTFMessenger(eventLog_richTextBox, true) { DefaulSpaceAfter = 0 };
        }

        private void BOM_MANAGER_Load(object sender, EventArgs e)
        {
            GetFixtureId();
            RefreshDataGridView_Assemblies();
            RefreshDataGridView_Part();
        }
                
        private void RefreshDataGridView_Assemblies()
        {
            try
            {
                dataGridView_Ass.DataSource = db.AssemblyTypeAtAssemblies.ToList();

                //hide columns here
                dataGridView_Ass.Columns[0].Visible = false;
                dataGridView_Ass.Columns[2].Visible = false;
                dataGridView_Ass.Columns[3].Visible = false;

                BomManagerFormMsg.NewMessage().AddText("Assembly Table has been Populated").PrependMessageType().Log();

            }
            catch
            {
                BomManagerFormMsg.NewMessage().AddText("Refreshing Assembly Table failed").PrependMessageType().Log();
            }
            eventLog_richTextBox.ScrollToCaret();
        }

        private void RefreshDataGridView_Part()
        {
            try
            {
                dataGridView_Part.DataSource = db.PartViews.ToList();

                ////hide columns here
                dataGridView_Part.Columns[0].Visible = false;
                dataGridView_Part.Columns[1].Visible = false;
                dataGridView_Part.Columns[2].Visible = false;
                dataGridView_Part.Columns[3].Visible = false;
                dataGridView_Part.Columns[4].Visible = false;
                //Part_dataGridView.Columns[5].Visible = false;
                dataGridView_Part.Columns[6].Visible = false;
                dataGridView_Part.Columns[7].Visible = false;
                //Part_dataGridView.Columns[8].Visible = false;
                dataGridView_Part.Columns[9].Visible = false;
                dataGridView_Part.Columns[10].Visible = false;
                dataGridView_Part.Columns[11].Visible = false;
                dataGridView_Part.Columns[12].Visible = false;
                //Part_dataGridView.Columns[13].Visible = false;
                //dataGridView_Part.Columns[14].Visible = false;

                BomManagerFormMsg.NewMessage().AddText(" Part Table has been Populated").PrependMessageType().Log();

            }
            catch
            {
                BomManagerFormMsg.NewMessage().AddText("Refreshing Part Table failed").PrependMessageType().Log();
            }
            eventLog_richTextBox.ScrollToCaret();
        }

        private void NewAssembly_Click(object sender, EventArgs e)
        {
           
            CreatOrEditForm newAssemblyForm = new CreatOrEditForm();
           // AssemblyTypeForm NewAssemblyTypeForm = new AssemblyTypeForm();

            DialogResult NewForm = newAssemblyForm.ShowDialog();

            try
            {
                if (NewForm == DialogResult.OK)
                {                    
                    BomManagerFormMsg.NewMessage().AddText("Assembly Successfully Added").PrependMessageType().Log();
                }
                else if (NewForm == DialogResult.Cancel)
                {
                    BomManagerFormMsg.NewMessage().AddText("Adding New Assembly Cancelled").PrependMessageType().Log();
                }
                db = new AXIS_AutomationEntities();
                RefreshDataGridView_Assemblies();
            }
            catch
            {
                BomManagerFormMsg.NewMessage().AddText("Adding Assembly Failed").IsError().PrependMessageType().Log();
            }
            eventLog_richTextBox.ScrollToCaret();

        }

        private void EditAssembly_Click(object sender, EventArgs e)
        {
            String EditAssemblyName;
                       
            Int32 currentAssemblyId = Int32.Parse(dataGridView_Ass.SelectedCells[0].OwningRow.Cells[0].Value.ToString());
            Assembly AssemblyToEdit = db.Assemblies.Find(currentAssemblyId);
            EditAssemblyName = AssemblyToEdit.Name;

            CreatOrEditForm EditAssemblyForm = new CreatOrEditForm(EditAssemblyName);
            //AssemblyTypeForm EditAssemblyTypeForm = new AssemblyTypeForm();

            DialogResult NewForm = EditAssemblyForm.ShowDialog();
            try
            {
                if (NewForm == DialogResult.OK)
                {
                    BomManagerFormMsg.NewMessage().AddText("Assembly Successfully Edited").PrependMessageType().Log();
                    
                }
                else if (NewForm == DialogResult.Cancel)
                {
                    BomManagerFormMsg.NewMessage().AddText("Editing Assembly Cancelled").PrependMessageType().Log();
                }
                db = new AXIS_AutomationEntities();
                RefreshDataGridView_Assemblies();
            }
            catch
            {
                BomManagerFormMsg.NewMessage().AddText("Editing Assembly Failed").IsError().PrependMessageType().Log();
            }
            eventLog_richTextBox.ScrollToCaret();
        }

        private void DeleteAssembly_Click(object sender, EventArgs e)
        {         
            String DeletionAssyNames = String.Join(", ", dataGridView_Ass.SelectedRows.OfType<DataGridViewRow>().Select(o => o.Cells["Name"].Value.ToString()).ToList());

            String assNoun = dataGridView_Ass.SelectedRows.Count > 1 ? "assemblies" : "assembly";

            DialogResult DeletingMsg = MessageBox.Show(String.Format("Do you really want to delete {1} {0}", DeletionAssyNames, assNoun), "Confirm  Assembly Deletion", MessageBoxButtons.YesNo);

            if (DeletingMsg == DialogResult.Yes)
            {
                foreach (DataGridViewRow currentDeletionRow in dataGridView_Ass.SelectedRows)
                {
                    Delete_function(currentDeletionRow);
                }
                RefreshDataGridView_Assemblies();

                BomManagerFormMsg.NewMessage().AddText("Assembly has been deleted from  Database").PrependMessageType().Log();
            }
        }

        private void Delete_function(DataGridViewRow currentDeletionRow)
        {
            Int32 currentAssemblyId = Int32.Parse(currentDeletionRow.Cells["Assembly_ID"].Value.ToString());
            Assembly AssemblyToDelete = db.Assemblies.Find(currentAssemblyId);
            String assemblyNameToDelete = db.Assemblies.Find(currentAssemblyId).Name;

            db.Assemblies.Remove(db.Assemblies.Find(currentAssemblyId));
            string assemblyname = db.Assemblies.Find(currentAssemblyId).Name;
            db.SaveChanges();

        }

        private void GetFixtureId()
        {
            productID_comboBox.DataSource = db.Fixtures.ToList();
            productID_comboBox.DisplayMember = "Code";
            productID_comboBox.ValueMember = "id";            
            //BomManagerFormMsg.NewMessage().AddText("Product ID has been Populated").PrependMessageType().Log();
            //BomManagerFormMsg.NewMessage().AddText("Assembly Table has been Populated").PrependMessageType().Log();
            //BomManagerFormMsg.NewMessage().IndentHanging().AddText("Product ID has been Populated").IsWarning.PrependMessageType();
            //BomManagerFormMsg.CurrentMessage.NewLine().AddText("DIC1K");
            //BomManagerFormMsg.CurrentMessage.NewLine().AddText("DIC2K").Log();           

        }

        private void ProductID_comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            String MySelectedFixture = productID_comboBox.Text;

            /*////Filter the Assembly View table to keep only the selected Product ID CODE 
            ////To view it in the Part DataGridView.
            //List<AssemblyView> filteredResult = db.AssemblyViews.Where(o => o.Code == MySelectedFixture).ToList();
            ////Parts DataGridView
            //Part_dataGridView.DataSource = filteredResult;*/

            RefreshTreeView();
           
        }

        private void RefreshTreeView()
        {
            //Get the Product ID selected
            String MySelectedFixture = productID_comboBox.Text;

            //Filter the Assembly View table to keep only the selected Product ID CODE 
            //This Filtered list is stored as the DICTIONARY ID
            List<AssemblyView> filteredResult = db.AssemblyViews.Where(o => o.Code == MySelectedFixture).ToList();

            //Dictionary Created
            Dictionary<Int32, TreeNode> AssembliesDict = new Dictionary<Int32, TreeNode>();

            //Goes through each line in the filtered list to add data into the Dictionary 
            foreach(AssemblyView assemblyView in filteredResult)
            {
                //Check if the Name of assembly in view is ROOT, Assign the Assembly View Code as It Name
                //Else assign NodeName as assemblyView.Name and put the type in brackets beside it. 
                String NodeName = assemblyView.Name == "ROOT" ? assemblyView.Code : String.Format("{0} ({1})", assemblyView.Name, assemblyView.AssemblyType);

                //create new TreeNode instance to assign the NodeName and assemblyView as Tag
                TreeNode NewTreeNode = new TreeNode()
                {
                    Text = NodeName,
                    Tag = assemblyView
                };

                //Add every Row of the Filtered assemblyView to our Dictionary having each line with a Unique ID (TKEY VALUE)
                AssembliesDict.Add(assemblyView.AssemblyID, NewTreeNode);
            }

            //Goes through each line in the Dictionary according to their Key Value Pair and add TreeNodes
            foreach (KeyValuePair<Int32, TreeNode> DictItem in AssembliesDict)
            {
                //Since in the View we have Parent ID as NULL we want to add a node anytime the parent ID is not NULL 
                Int32 ParentIndex = ((AssemblyView)DictItem.Value.Tag).ParentID ?? 0;

                //This if Condition indent the Tree view if the Parent ID is not Null. 
                if(ParentIndex != 0)
                {
                    AssembliesDict[ParentIndex].Nodes.Add(DictItem.Value);
                }
            }

            Fixture_treeView.Nodes.Clear();
            try
            {
                //Add the tree Node to the Form
                TreeNode RootNode = AssembliesDict.Where(o=> ((AssemblyView)o.Value.Tag).ParentID == null).First().Value;
                Fixture_treeView.Nodes.Add(RootNode);
                Fixture_treeView.ExpandAll();
            }
            catch
            {

            }
            
        }
    }
}
