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

                //BomManagerFormMsg.NewMessage().AddText("Assembly Table has been Populated").PrependMessageType().Log();
                
            }
            catch
            {
                BomManagerFormMsg.NewMessage().AddText("Refreshing Assembly Table failed").PrependMessageType().Log();
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
            String MySelectedFixture = productID_comboBox.Text;//GetItemText(this.productID_comboBox.SelectedItem);

            List<AssemblyView> filteredResult = db.AssemblyViews.Where(o => o.Code == MySelectedFixture).ToList();

            Part_dataGridView.DataSource = filteredResult;

            RefreshTreeView();
            //    TreeView treeView = new TreeView();
            //    AssemblyView AssemblyViewslist = new AssemblyView();

            //    DataTable MyAssemblyviewFilter = db.AssemblyViews.AsEnumerable().Where(o => o.Code<string>("") == productID_comboBox.SelectedText).copy();

            //DataRow[] RowsInAssemblyViews = db.AssemblyViews.Select(String.Format("ParentID = '{NULL}'", ParentID));

            //Fixture_treeView.Nodes.Add(newNode);
        }

        private void RefreshTreeView()
        {
            String MySelectedFixture = productID_comboBox.Text;

            List<AssemblyView> filteredResult = db.AssemblyViews.Where(o => o.Code == MySelectedFixture).ToList();

            Dictionary<Int32, TreeNode> AssembliesDict = new Dictionary<Int32, TreeNode>();

            foreach(AssemblyView assemblyView in filteredResult)
            {

                String NodeName = assemblyView.Name == "ROOT" ? assemblyView.Code : String.Format("{0} ({1})", assemblyView.Name, assemblyView.AssemblyType);
                TreeNode NewTreeNode = new TreeNode()
                {
                    Text = NodeName,
                    Tag = assemblyView
                };


                AssembliesDict.Add(assemblyView.AssemblyID, NewTreeNode);
            }


            foreach(KeyValuePair<Int32, TreeNode> DictItem in AssembliesDict)
            {
                Int32 ParentIndex = ((AssemblyView)DictItem.Value.Tag).ParentID ?? 0;

                if(ParentIndex != 0)
                {
                    AssembliesDict[ParentIndex].Nodes.Add(DictItem.Value);
                }
            }

            Fixture_treeView.Nodes.Clear();
            try
            {
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
