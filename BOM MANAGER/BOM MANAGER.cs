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
            SyncRootAssemblies();
            RefreshDataGridView_Assemblies();
            RefreshDataGridView_Part();
            RefreshTreeView();
            
        }

        private void GetFixtureId()
        {
            productID_comboBox.DataSource = db.Fixtures.ToList();
            productID_comboBox.ValueMember = "id";            
            productID_comboBox.DisplayMember = "Code";
            //BomManagerFormMsg.NewMessage().AddText("Product ID has been Populated").PrependMessageType().Log();
            //BomManagerFormMsg.NewMessage().AddText("Assembly Table has been Populated").PrependMessageType().Log();
            //BomManagerFormMsg.NewMessage().IndentHanging().AddText("Product ID has been Populated").IsWarning.PrependMessageType();
            //BomManagerFormMsg.CurrentMessage.NewLine().AddText("DIC1K");
            //BomManagerFormMsg.CurrentMessage.NewLine().AddText("DIC2K").Log();           

        }

        private void SyncRootAssemblies()
        {
            List<Fixture> allFixtures = db.Fixtures.ToList();
            Int32 rootAssyID = db.Assemblies.Where(o => o.Name == "ROOT").First().id;

            foreach ( Fixture currentFixture in allFixtures)
            {
                Boolean rootExists = db.AssemblyAtAssemblies.Any(o => o.FixtureID == currentFixture.id && o.ParentID == null);
                if(!rootExists)
                {
                    
                    AssemblyAtAssembly newRoot = new AssemblyAtAssembly()
                    {
                        FixtureID = currentFixture.id,
                        AssemblyID = rootAssyID,
               
                    };
                    db.AssemblyAtAssemblies.Add(newRoot);
                }                
            }
            db.SaveChanges();
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

                // BomManagerFormMsg.NewMessage().AddText("Assembly Table has been Populated").PrependMessageType().Log();
                string searchingFor = "ROOT";
                int rowIndex = 0;
                foreach (DataGridViewRow row in dataGridView_Ass.Rows)
                {
                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        if (cell.Value.ToString() == searchingFor)
                        {
                            rowIndex = row.Index;
                            dataGridView_Ass.Rows[rowIndex].Visible = false;
                        }
                    }
                }

                eventLog_richTextBox.ScrollToCaret();
            }
            catch
            {
                BomManagerFormMsg.NewMessage().AddText("Refreshing Assembly Table failed").PrependMessageType().Log();
            }
        }

        private void RefreshDataGridView_Part()
        {
            try
            {
                dataGridView_Part.DataSource = db.PartTypeAtParts.ToList();

                ////hide columns here
                dataGridView_Part.Columns[0].Visible = false;
                //dataGridView_Part.Columns[1].Visible = false;
                dataGridView_Part.Columns[2].Visible = false;
                dataGridView_Part.Columns[3].Visible = false;

                //BomManagerFormMsg.NewMessage().AddText(" Part Table has been Populated").PrependMessageType().Log();

                eventLog_richTextBox.ScrollToCaret();
            }
            catch
            {
                BomManagerFormMsg.NewMessage().AddText("Refreshing Part Table failed").PrependMessageType().Log();
            }
        }

        private void NewAssembly_Click(object sender, EventArgs e)
        {
           
            Assembly_CreatOrEditForm newAssemblyForm = new Assembly_CreatOrEditForm();
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
                eventLog_richTextBox.ScrollToCaret();
            }
            catch
            {
                BomManagerFormMsg.NewMessage().AddText("Adding Assembly Failed").IsError().PrependMessageType().Log();
            }

        }

        private void NewPart_Click(object sender, EventArgs e)
        {
            Part_CreateOrEditForm newPartForm = new Part_CreateOrEditForm();

            DialogResult NewForm = newPartForm.ShowDialog();

            try
            {
                if (NewForm == DialogResult.OK)
                {
                    BomManagerFormMsg.NewMessage().AddText("Part Successfully Added").PrependMessageType().Log();
                }
                else if (NewForm == DialogResult.Cancel)
                {
                    BomManagerFormMsg.NewMessage().AddText("Adding New Part Cancelled").PrependMessageType().Log();
                }
                db = new AXIS_AutomationEntities();
                RefreshDataGridView_Part();
                eventLog_richTextBox.ScrollToCaret();
            }
            catch
            {
                BomManagerFormMsg.NewMessage().AddText("Adding Part Failed").IsError().PrependMessageType().Log();
            }
        }
        
        private void EditAssembly_Click(object sender, EventArgs e)
        {
            String editAssemblyName;
                       
            Int32 currentAssemblyId = Int32.Parse(dataGridView_Ass.SelectedCells[0].OwningRow.Cells[0].Value.ToString());
            Assembly assemblyToEdit = db.Assemblies.Find(currentAssemblyId);
            editAssemblyName = assemblyToEdit.Name;

            Assembly_CreatOrEditForm editAssemblyForm = new Assembly_CreatOrEditForm(editAssemblyName);
            
            DialogResult NewForm = editAssemblyForm.ShowDialog();
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
                eventLog_richTextBox.ScrollToCaret();
            }
            catch
            {
                BomManagerFormMsg.NewMessage().AddText("Editing Assembly Failed").IsError().PrependMessageType().Log();
            }
        }

        private void EditPart_Click(object sender, EventArgs e)
        {
            String editPartName;

            Int32 currentPartId = Int32.Parse(dataGridView_Part.SelectedCells[0].OwningRow.Cells[0].Value.ToString());
            Part partToEdit = db.Parts.Find(currentPartId);
            editPartName = partToEdit.PartName;

            Part_CreateOrEditForm editPartForm = new Part_CreateOrEditForm(editPartName);

            DialogResult NewForm = editPartForm.ShowDialog();
            try
            {
                if(NewForm == DialogResult.OK)
                {
                    BomManagerFormMsg.NewMessage().AddText("Part Successfully Edited").PrependMessageType().Log();
                }
                else if (NewForm == DialogResult.Cancel)
                {
                    BomManagerFormMsg.NewMessage().AddText("Editing Part Cancelled").PrependMessageType().Log();
                }
                db = new AXIS_AutomationEntities();
                RefreshDataGridView_Part();
                eventLog_richTextBox.ScrollToCaret();
            }
            catch
            {
                BomManagerFormMsg.NewMessage().AddText("Editing Part Failed").IsError().PrependMessageType().Log();
            }

        }

        private void DeleteAssembly_Click(object sender, EventArgs e)
        {         
            String DeletionAssyNames = String.Join(", ", dataGridView_Ass.SelectedRows.OfType<DataGridViewRow>().Select(o => o.Cells["Name"].Value.ToString()).ToList());

            String assNoun = dataGridView_Ass.SelectedRows.Count > 1 ? "assemblies" : "assembly";

            DialogResult deletingMsg = MessageBox.Show(String.Format("Do you really want to delete {1} {0}", DeletionAssyNames, assNoun), "Confirm Assembly Deletion", MessageBoxButtons.YesNo);
            
            if (deletingMsg == DialogResult.Yes )
            {            

                try
                {
                    foreach (DataGridViewRow currentDeletionRow in dataGridView_Ass.SelectedRows)
                    {
                        //Delete_Function_Ass(currentDeletionRow);
                        Int32 currentAssemblyId = Int32.Parse(currentDeletionRow.Cells["Assembly_ID"].Value.ToString());
                        Assembly AssemblyToDelete = db.Assemblies.Find(currentAssemblyId);
                        String assemblyNameToDelete = db.Assemblies.Find(currentAssemblyId).Name;

                        Boolean myAssociation = db.AssemblyAtAssemblies.Any(o => o.AssemblyID == currentAssemblyId);
                        if(!myAssociation)
                        {
                            db.Assemblies.Remove(db.Assemblies.Find(currentAssemblyId));
                            string assemblyname = db.Assemblies.Find(currentAssemblyId).Name;
                            db.SaveChanges();

                            //do not show message if assembly has not been deleted.
                            BomManagerFormMsg.NewMessage().AddText("Assembly : "+ assemblyNameToDelete + " has been deleted from TreeView and Database").PrependMessageType().Log();
                        }
                        else
                        {
                            BomManagerFormMsg.NewMessage().AddText("Assembly: " + assemblyNameToDelete + " has not been deleted from TreeView and Database due to Association").IsError().PrependMessageType().Log();
                        }

                    }
                    RefreshDataGridView_Assemblies();
                    RefreshTreeView();

                    
                }
                catch
                {
                    BomManagerFormMsg.NewMessage().AddText("Assemblies with associations cannot be deleted from  Database").IsError().PrependMessageType().Log();
                }               
                
            }
        }

        private void DeletePart_Click(object sender, EventArgs e)
        {
            String deletionPartName = String.Join(", ",dataGridView_Part.SelectedRows.OfType<DataGridViewRow>().Select(o => o.Cells["PartName"].Value.ToString()).ToList());
            String assNoun = dataGridView_Part.SelectedRows.Count > 1 ? "Parts" : "Part";

            DialogResult deletingMsg = MessageBox.Show(String.Format("Do you really want to delete {1} {0}", deletionPartName, assNoun), "Confirm Assembly Deletion", MessageBoxButtons.YesNo);

            if(deletingMsg == DialogResult.Yes)
            {
                foreach(DataGridViewRow currentDeletionRow in dataGridView_Part.SelectedRows)
                {
                    Delete_Function_Part(currentDeletionRow);
                }
                RefreshDataGridView_Part();

                BomManagerFormMsg.NewMessage().AddText("Part has been deleted from Database").PrependMessageType().Log();
            }
        }

        private void Associationcheck()
        {
            if (CheckIsValid)
            {
                DeleteAssembly.Enabled = true;

            }
            else
            {
                DeleteAssembly.Enabled = false;

            }

        }

        private Boolean CheckIsValid
        {
            get
            {
                return (!AssemblyHasNoAssociations);
            }
        }

        private Boolean AssemblyHasNoAssociations
        {
            get
            {
                Int32 rowIndex = dataGridView_Ass.CurrentCell.RowIndex;
                Int32 Teststring = Int32.Parse(dataGridView_Ass.Rows[rowIndex].Cells[0].Value.ToString());
                Boolean myAssociation = db.AssemblyAtAssemblies.Any(o => o.AssemblyID == Teststring);
                if (myAssociation)
                {
                    //BomManagerFormMsg.NewMessage().AddText("Delete Button not Available due to association").IsWarning().PrependMessageType().Log();
                }
                return myAssociation;
            }
        }

        //private void Delete_Function_Ass(DataGridViewRow currentDeletionRow)
        //{
        //    Int32 currentAssemblyId = Int32.Parse(currentDeletionRow.Cells["Assembly_ID"].Value.ToString());
        //    Assembly AssemblyToDelete = db.Assemblies.Find(currentAssemblyId);
        //    String assemblyNameToDelete = db.Assemblies.Find(currentAssemblyId).Name;            

        //    db.Assemblies.Remove(db.Assemblies.Find(currentAssemblyId));
        //    string assemblyname = db.Assemblies.Find(currentAssemblyId).Name;
        //    //db.SaveChanges();

        //}

        private void Delete_Function_Part(DataGridViewRow currentDeletionRow)
        {
            Int32 currentPartId = Int32.Parse(currentDeletionRow.Cells["Part_ID"].Value.ToString());
            Part partToDelete = db.Parts.Find(currentPartId);
            String partNameToDelete = db.Parts.Find(currentPartId).PartName;

            db.Parts.Remove(db.Parts.Find(currentPartId));
            string assemblyname = db.Parts.Find(currentPartId).PartName;
            db.SaveChanges();

        }
        
        private void ProductID_comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            String MySelectedFixture = productID_comboBox.Text;
                        
            RefreshTreeView();
           
        }

        private void TabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            String MySelectedFixture = productID_comboBox.Text;
            RefreshDataGridView_Assemblies();
            RefreshTreeView();
            Associationcheck();
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

            //get parts table(create dictionary of parts), change to tree nodes, and insert tree node into parent from Dictionary AssembliesDict



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

        private void DataGridView_Part_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            string headerText = dataGridView_Part.Columns[e.ColumnIndex].HeaderText;

            try
            {
                if (headerText == "PartType")
                {
                    dataGridView_Part.DataSource = db.PartTypeAtParts.OrderBy(o => o.PartType).ToList();
                    BomManagerFormMsg.NewMessage().AddText("Table Sorted By Part Type").PrependMessageType().Log();
                }
                else if(headerText == "PartName" )
                {
                    dataGridView_Part.DataSource = db.PartTypeAtParts.OrderBy(o => o.PartName).ToList();
                    BomManagerFormMsg.NewMessage().AddText("Table Sorted By Part Name").PrependMessageType().Log();

                }
                else
                {
                    dataGridView_Part.DataSource = db.PartTypeAtParts.OrderBy(o => o.PartType_ID).ToList();
                    BomManagerFormMsg.NewMessage().AddText("Table Sorted By Product ID").PrependMessageType().Log();
                }               

            }
            catch
            {
                BomManagerFormMsg.NewMessage().AddText("Table Sorted By PartType failed").PrependMessageType().Log();
            }
                        
            eventLog_richTextBox.ScrollToCaret();
        }

        private void AddButton_AssyToAssy_TreeView_Click(object sender, EventArgs e)
        {
            try
            {
                AssemblyView SelectedAssemblyNode = (AssemblyView)Fixture_treeView.SelectedNode.Tag;
                Int32? CurrentParentId = SelectedAssemblyNode.ParentID;
                Int32? CurrentAssemblyId = SelectedAssemblyNode.AssemblyID;
                Int32? CurrentFixtureCodeId = SelectedAssemblyNode.FixtureID;

                foreach (DataGridViewRow currentSelectedRow in dataGridView_Ass.SelectedRows)
                {
                    //Function to Save Assy to parent
                    AddAssyToTreeView( currentSelectedRow, CurrentParentId, CurrentAssemblyId, CurrentFixtureCodeId);
                }
            }
            catch
            {
                BomManagerFormMsg.NewMessage().AddText("No Fixture Selected in the TreeView").IsError().PrependMessageType().Log();
            }
            RefreshTreeView();

        }

        private void AddAssyToTreeView(DataGridViewRow currentSelectedRow, Int32? ParentID, Int32? AssemblyId, Int32? FixtureCodeID)
        {
            Int32 currentAssemblyId = Int32.Parse(currentSelectedRow.Cells["Assembly_ID"].Value.ToString());

            String assemblyNameToAdd = db.Assemblies.Find(currentAssemblyId).Name;
            Boolean myAssociation = db.AssemblyAtAssemblies.Any(o => o.AssemblyID == currentAssemblyId && o.FixtureID == FixtureCodeID);

            //Check if Sub-Assembly is ROOT Assembly... if yues do not add
            if (currentAssemblyId != 14 && !myAssociation)
            {
                AssemblyAtAssembly NewAssemblyAtAssembly = new AssemblyAtAssembly();

                db.AssemblyAtAssemblies.Add(NewAssemblyAtAssembly);

                NewAssemblyAtAssembly.ParentID = (Int32)AssemblyId;
                NewAssemblyAtAssembly.AssemblyID = currentAssemblyId;
                NewAssemblyAtAssembly.FixtureID = (Int32)FixtureCodeID;
                db.SaveChanges();

                //do not show message if assembly has not been deleted.
                BomManagerFormMsg.NewMessage().AddText("Assembly : " + assemblyNameToAdd + " has been added to TreeView").PrependMessageType().Log();

                //Check Associations
                Associationcheck();
            }            
            else
            {
                BomManagerFormMsg.NewMessage().AddText("Assembly: " + assemblyNameToAdd + " has not been added to TreeView because an assembly cannot have itself as assembly").IsError().PrependMessageType().Log();
            }

        }

        private void RemoveButton_AssyToAssy_TreeView_Click(object sender, EventArgs e)
        {
            if (Fixture_treeView.SelectedNode.Parent == null)
            {
                BomManagerFormMsg.NewMessage().AddText("Root Assembly deletion is forbidden").IsError().PrependMessageType().Log();
            }
            else
            {
                try
                {
                    Recursive_Delete_Function(Fixture_treeView.SelectedNode);

                    db.SaveChanges();
                    //RefreshTreeView();
                    Fixture_treeView.SelectedNode.Remove();

                    //Association Check
                    Associationcheck();

                    BomManagerFormMsg.NewMessage().AddText("Assembly has been deleted from Tree view").PrependMessageType().Log();
                }
                catch
                {
                    BomManagerFormMsg.NewMessage().AddText("Assembly has not been deleted from Database").PrependMessageType().Log();
                }
            }
            
        }

        private void Recursive_Delete_Function(TreeNode selectedNode)
        {

            if (selectedNode.Nodes.Count != 0)
            {
                foreach (TreeNode childNode in selectedNode.Nodes)
                {
                    Recursive_Delete_Function(childNode);
                }               
            }

            //delete stuff
            Int32? currentAssemblyViewIndex = ((AssemblyView)selectedNode.Tag).T1_AssAtAssembly_ID;
            AssemblyAtAssembly deletionTarget = db.AssemblyAtAssemblies.Find(currentAssemblyViewIndex);
            if (deletionTarget.ParentID is null)
            {
                BomManagerFormMsg.NewMessage().AddText("Root Assembly deletion is forbidden").IsError().PrependMessageType().Log();
            }
            else
            {
                db.AssemblyAtAssemblies.Remove(deletionTarget);

            }

        }              

        private void DataGridView_Ass_MouseClick(object sender, MouseEventArgs e)
        {
            Associationcheck();
        }
    }
}
