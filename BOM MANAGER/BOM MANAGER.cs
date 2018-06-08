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
        /****************************************************************************************************************************************************************/
        /*                                                                                                                                                              */
        /*                                                              PART / ASSEMBLY TAB                                                                             */
        /*                                                                                                                                                              */
        /****************************************************************************************************************************************************************/


        RTFMessenger.RTFMessenger BomManagerFormMsg;
        RTFMessenger.RTFMessenger PartRules;
        AXIS_AutomationEntities db;
                
        public BOM_MANAGER()
        {
            InitializeComponent();
            db = new AXIS_AutomationEntities();
            BomManagerFormMsg = new RTFMessenger.RTFMessenger(eventLog_richTextBox, true) { DefaulSpaceAfter = 0 };
            PartRules = new RTFMessenger.RTFMessenger(eventLog_PR_richTextBox, true) { DefaulSpaceAfter = 0 };
        }

        private void BOM_MANAGER_Load(object sender, EventArgs e)
        { 
            GetFixtureId();
            SyncRootAssemblies();
            RefreshDataGridView_Assemblies();
            RefreshDataGridView_Part();
                        
        }

        private void TabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            String MySelectedFixture = productID_comboBox.Text;
            RefreshDataGridView_Assemblies();
            RefreshDataGridView_Part();
            RefreshTreeView();
            
            if ( tabControl1.SelectedTab == tabPage2)
            {
                AssyAssociationcheck();
                PartAssociationcheck();
            }


            //////////////////////////////////////PART Rules Tab////////////////////////////////////////////////
            else if (tabControl1.SelectedTab == tabPage3)
            {                
                Load_Category_ComboBox();
                Load_Parameter_ComboBox();
            }
           

            
        }
        
        private void ProductID_comboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            RefreshTreeView();

        }
        
        private void GetFixtureId()
        {
            productID_comboBox.DataSource = db.Fixtures.OrderBy(o => o.Code).ToList();
            productID_comboBox.ValueMember = "id";            
            productID_comboBox.DisplayMember = "Code";

            ComboBoxFixtureFamily.DataSource = db.FamilyNames.Select(o => o.FamilyName1).ToList();
            ComboBoxFixtureFamily.DisplayMember = "FamilyName1";

            //PART RULE TAB
            productID_comboBox_PR.DataSource = db.Fixtures.OrderBy(o => o.Code).ToList();
            productID_comboBox_PR.ValueMember = "id";
            productID_comboBox_PR.DisplayMember = "Code";
                        
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
               
                String Combotext = ComboBoxFixtureFamily.Text;


                dataGridView_Part.DataSource = db.PartTypeAtParts.OrderBy(o => o.PartType).Where(o=>o.FamilyName == Combotext).OrderBy(p=>p.PartName).ToList();

                ////hide columns here
                dataGridView_Part.Columns[0].Visible = false;
                //dataGridView_Part.Columns[1].Visible = false;
                dataGridView_Part.Columns[2].Visible = false;
                dataGridView_Part.Columns[3].Visible = false;
                //dataGridView_Part.Columns[4].Visible = false;
                dataGridView_Part.Columns[5].Visible = false;
                //dataGridView_Part.Columns[6].Visible = false;
                dataGridView_Part.Columns[7].Visible = false;
                //dataGridView_Part.Columns[8].Visible = false;
                dataGridView_Part.Columns[9].Visible = false;
                dataGridView_Part.AutoResizeColumns();

                
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
                    db = new AXIS_AutomationEntities();
                    RefreshDataGridView_Assemblies();
                    eventLog_richTextBox.ScrollToCaret();
                }
                else if (NewForm == DialogResult.Cancel)
                {
                    BomManagerFormMsg.NewMessage().AddText("Adding New Assembly Cancelled").PrependMessageType().Log();
                }
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

                    db = new AXIS_AutomationEntities();
                    RefreshDataGridView_Part();
                }
                else if (NewForm == DialogResult.Cancel)
                {
                    BomManagerFormMsg.NewMessage().AddText("Adding New Part Cancelled").PrependMessageType().Log();
                }

                //Int32 index = 0;
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
                    db = new AXIS_AutomationEntities();
                    RefreshDataGridView_Assemblies();
                    RefreshTreeView();
                    
                }
                else if (NewForm == DialogResult.Cancel)
                {
                    BomManagerFormMsg.NewMessage().AddText("Editing Assembly Cancelled").PrependMessageType().Log();
                }
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
            String editDescription;
            Int32 editPartTypeIndex;
            String editPartType;
            String editFamilyName;
            Int32 editFamilyNameIndex;
                                 
            Int32 currentPartId = Int32.Parse(dataGridView_Part.SelectedCells[0].OwningRow.Cells[0].Value.ToString());
            Part partToEdit = db.Parts.Find(currentPartId);//.Find(currentPartId);

            editDescription = partToEdit.Description;
            editPartName = partToEdit.PartName;
            editPartTypeIndex = partToEdit.TypeID ?? 0;
            editFamilyNameIndex = partToEdit.FixFamilyID ?? 0;

            PartType partTypeToEdit = db.PartTypes.Find(editPartTypeIndex);
            editPartType = partTypeToEdit.PartType1;

            FamilyName familytypeToEdit = db.FamilyNames.Find(editFamilyNameIndex);
            editFamilyName = familytypeToEdit.FamilyName1;

            Part_CreateOrEditForm editPartForm = new Part_CreateOrEditForm(editPartName, editDescription, editPartType, editFamilyName);

            DialogResult NewForm = editPartForm.ShowDialog();
            try
            {
                if(NewForm == DialogResult.OK)
                {
                    BomManagerFormMsg.NewMessage().AddText("Part Successfully Edited").PrependMessageType().Log();

                    db = new AXIS_AutomationEntities();
                    RefreshDataGridView_Part();
                    
                    RefreshTreeView();
                }
                else if (NewForm == DialogResult.Cancel)
                {
                    BomManagerFormMsg.NewMessage().AddText("Editing Part Cancelled").PrependMessageType().Log();
                }
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

                            RefreshDataGridView_Assemblies();
                            RefreshTreeView();

                            //do not show message if assembly has not been deleted.
                            BomManagerFormMsg.NewMessage().AddText("Assembly : "+ assemblyNameToDelete + " has been deleted from TreeView and Database").PrependMessageType().Log();
                        }
                        else
                        {
                            BomManagerFormMsg.NewMessage().AddText("Assembly: " + assemblyNameToDelete + " has not been deleted from TreeView and Database due to Association").IsError().PrependMessageType().Log();
                        }

                    }

                    
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
                try
                {
                    foreach(DataGridViewRow currentDeletionRow in dataGridView_Part.SelectedRows)
                    {
                        //Delete_Function_Ass(currentDeletionRow);
                        Int32 currentPartId = Int32.Parse(currentDeletionRow.Cells["Part_ID"].Value.ToString());
                        Part partToDelete = db.Parts.Find(currentPartId);
                        String partyNameToDelete = db.Parts.Find(currentPartId).PartName;

                        Boolean myAssociation = db.PartAtAssemblies.Any(o => o.PartRefID == currentPartId);
                        if (!myAssociation)
                        {
                            //Int32 currentPartId = Int32.Parse(currentDeletionRow.Cells["Part_ID"].Value.ToString());
                            //Part partToDelete = db.Parts.Find(currentPartId);
                            //String partNameToDelete = db.Parts.Find(currentPartId).PartName;

                            db.Parts.Remove(db.Parts.Find(currentPartId));
                            string assemblyname = db.Parts.Find(currentPartId).PartName;
                            db.SaveChanges();

                            RefreshDataGridView_Part();
                            RefreshTreeView();

                            BomManagerFormMsg.NewMessage().AddText("Part: " + partyNameToDelete + " has been deleted from TreeView and Database due to Association").PrependMessageType().Log();

                        }
                        else
                        {
                            BomManagerFormMsg.NewMessage().AddText("Part: " + partyNameToDelete + " has not been deleted from TreeView and Database due to Association").IsError().PrependMessageType().Log();
                        }
                    }
                    

                    

                }
                catch
                {
                    BomManagerFormMsg.NewMessage().AddText("Parts with associations cannot be deleted from  Database").IsError().PrependMessageType().Log();
                }
            }
        }

        private void AssyAssociationcheck()
        {
            if (CheckAssyIsValid)
            {
                DeleteAssembly.Enabled = true;

            }
            else
            {
                DeleteAssembly.Enabled = false;

            }

        }

        private void PartAssociationcheck()
        {
            if (CheckPartIsValid)
            {
                DeletePart.Enabled = true;

            }
            else
            {
                DeletePart.Enabled = false;

            }

        }

        private Boolean CheckAssyIsValid
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
                Int32 Teststring = Int32.Parse(dataGridView_Ass.Rows[rowIndex].Cells["Assembly_ID"].Value.ToString());
                Boolean myAssociation = db.AssemblyAtAssemblies.Any(o => o.AssemblyID == Teststring);
                if (myAssociation)
                {
                    //BomManagerFormMsg.NewMessage().AddText("Delete Button not Available due to assembly association").IsWarning().PrependMessageType().Log();
                }
                return myAssociation;
            }
        }

        private Boolean CheckPartIsValid
        {
            get
            {
                return (!PartsHasNoAssociations);
            }
        }

        private Boolean PartsHasNoAssociations
        {
            get
            {                
                Int32 rowIndex = dataGridView_Part.CurrentCell.RowIndex;
                Int32 Teststring = Int32.Parse(dataGridView_Part.Rows[rowIndex].Cells["Part_ID"].Value.ToString());
                Boolean myAssociation = db.PartAtAssemblies.Any(o => o.PartRefID == Teststring);
                if (myAssociation)
                {
                    //BomManagerFormMsg.NewMessage().AddText("Delete Button not Available due to Part association").IsWarning().PrependMessageType().Log();
                }
                return myAssociation;
            }
        }                         
       
        private void ComboBoxFixtureFamily_SelectedIndexChanged(object sender, EventArgs e)
        {
            RefreshDataGridView_Part();
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
                AssyAssociationcheck();
                PartAssociationcheck();
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
                    Assy_Recursive_Delete_Function(Fixture_treeView.SelectedNode);

                    db.SaveChanges();
                    //RefreshTreeView();
                    Fixture_treeView.SelectedNode.Remove();

                    //Association Check
                    AssyAssociationcheck();
                    PartAssociationcheck();

                    //BomManagerFormMsg.NewMessage().AddText("Assembly has been deleted from Tree view").PrependMessageType().Log();
                }
                catch
                {
                    BomManagerFormMsg.NewMessage().AddText("Assembly has not been deleted from Database. Remove Associated Parts First").IsError().PrependMessageType().Log();
                }
            }
            
        }

        private void Assy_Recursive_Delete_Function(TreeNode selectedNode)
        {

            if (selectedNode.Nodes.Count != 0)
            {
                foreach (TreeNode childNode in selectedNode.Nodes)
                {
                    Assy_Recursive_Delete_Function(childNode);
                }               
            }

            try
            {
                Int32? currentAssemblyViewIndex = ((AssemblyView)selectedNode.Tag).id;
                String CurrentAssemblyViewName = ((AssemblyView)selectedNode.Tag).Name;
                AssemblyAtAssembly deletionTarget = db.AssemblyAtAssemblies.Find(currentAssemblyViewIndex);
                if (deletionTarget.ParentID is null)
                {
                    BomManagerFormMsg.NewMessage().AddText("Root Assembly deletion is forbidden").IsError().PrependMessageType().Log();
                }
                else
                {
                    db.AssemblyAtAssemblies.Remove(deletionTarget);
                    BomManagerFormMsg.NewMessage().AddText("Assembly " + CurrentAssemblyViewName  + " has been deleted from Tree view").PrependMessageType().Log();

                }                
            }
            catch
            {
                //BomManagerFormMsg.NewMessage().AddText("Root Assembly deletion is forbidden").IsError().PrependMessageType().Log();
            }
            try
            {
                Int32 currentPartId = ((PartView)selectedNode.Tag).id;
                String currentPartNameId = ((PartView)selectedNode.Tag).PartName;
                PartAtAssembly deletionTarget = db.PartAtAssemblies.Find(currentPartId);

                db.PartAtAssemblies.Remove(deletionTarget);

                BomManagerFormMsg.NewMessage().AddText("Parts " + currentPartNameId + " has been deleted from Tree view").PrependMessageType().Log();
            }
            catch
            {

            }

        }              

        private void DataGridView_Ass_MouseClick(object sender, MouseEventArgs e)
        {
            AssyAssociationcheck();
            
        }

        private void DataGridView_Part_MouseClick(object sender, MouseEventArgs e)
        {
            PartAssociationcheck();
        }

        private void AddButton_PartToAssy_TreeView_Click(object sender, EventArgs e)
        {
            try
            {
                AssemblyView SelectedAssemblyNode = (AssemblyView)Fixture_treeView.SelectedNode.Tag;
                Int32? CurrentParentId = SelectedAssemblyNode.ParentID;
                Int32? CurrentAssemblyId = SelectedAssemblyNode.id;
                Int32? CurrentFixtureCodeId = SelectedAssemblyNode.FixtureID;

                foreach (DataGridViewRow currentSelectedRow in dataGridView_Part.SelectedRows)
                {
                    //Function to Save Assy to parent
                    AddPartsToTreeView(currentSelectedRow, CurrentParentId, CurrentAssemblyId, CurrentFixtureCodeId);
                }
            }
            catch
            {
                BomManagerFormMsg.NewMessage().AddText("No Fixture/Assembly Selected in the TreeView. Parts cannot be added to other Parts.").IsError().PrependMessageType().Log();
            }
            RefreshTreeView();

        }

        private void AddPartsToTreeView(DataGridViewRow currentSelectedRow, Int32? parentId, Int32? assemblyId, Int32? FixtureId)
        {
            Int32 currentPartId = Int32.Parse(currentSelectedRow.Cells["Part_ID"].Value.ToString());
            //PartTypeAtParts
            String PartNameToAdd = db.Parts.Find(currentPartId).PartName;

            try
            {
                PartAtAssembly NewPartAtAssembly = new PartAtAssembly();

                db.PartAtAssemblies.Add(NewPartAtAssembly);

                NewPartAtAssembly.PartRefID = currentPartId;
                NewPartAtAssembly.AssRefID = (Int32)assemblyId;
                NewPartAtAssembly.FixtureID = (Int32)FixtureId;
                db.SaveChanges();

                //do not show message if assembly has not been deleted.
                BomManagerFormMsg.NewMessage().AddText("Part : " + PartNameToAdd + " has been added to TreeView and DataBase").PrependMessageType().Log();

                //Check Associations
                AssyAssociationcheck();
                PartAssociationcheck();
            }
            catch
            {
                BomManagerFormMsg.NewMessage().AddText("Part: " + PartNameToAdd + " has not been added").IsError().PrependMessageType().Log();
            }
        }

        private void Remove_PartToAssy_TreeView_Click(object sender, EventArgs e)
        {            
            try
            {
                Part_Recursive_Delete_Function(Fixture_treeView.SelectedNode);

                db.SaveChanges();
                              
                RefreshTreeView();
                //Fixture_treeView.SelectedNode.Remove();

                //Association Check
                AssyAssociationcheck();
                PartAssociationcheck();
            }
            catch
            {
                BomManagerFormMsg.NewMessage().AddText("Part has not been deleted from treeview or Database").IsError().PrependMessageType().Log();
            }
            
        }

        private void Part_Recursive_Delete_Function(TreeNode selectedNode)
        {
            Int32 numberOfInnerNodes = selectedNode.Nodes.Count;

            foreach (TreeNode childNode in selectedNode.Nodes)
            {
                Part_Recursive_Delete_Function(childNode);
            }

            try
            {
                Int32 currentPartId = ((PartView)selectedNode.Tag).id;
                String currentPartNameId = ((PartView)selectedNode.Tag).PartName;
                PartAtAssembly deletionTarget = db.PartAtAssemblies.Find(currentPartId);
                
                db.PartAtAssemblies.Remove(deletionTarget);

                BomManagerFormMsg.NewMessage().AddText("Parts " + currentPartNameId + " has been deleted from Tree view").PrependMessageType().Log();
            }

            catch
            {
                //BomManagerFormMsg.NewMessage().AddText("Parts " /*+ currentPartNameId*/ + " has not been deleted from Tree view").PrependMessageType().Log();
            }

            try
            {
                Int32? currentAssemblyViewIndex = ((AssemblyView)selectedNode.Tag).id;
                AssemblyAtAssembly deletionTarget = db.AssemblyAtAssemblies.Find(currentAssemblyViewIndex);
            }

            catch
            {
                //BomManagerFormMsg.NewMessage().AddText("Assembly has not been deleted from Tree view").PrependMessageType().Log();
            }

        }
        
        private void RefreshTreeView()
        {
            String MySelectedFixture;

            if (tabControl1.SelectedTab == tabPage2)
            {
                MySelectedFixture = productID_comboBox.Text;
            }
            else 
            {
                MySelectedFixture = productID_comboBox_PR.Text;
            }
           
            List<AssemblyView> assy_filteredResult = db.AssemblyViews.Where(o => o.Code == MySelectedFixture).ToList();
            List<PartView> part_filteredResult = db.PartViews.OrderBy(o => o.PartName).Where(p => p.Code == MySelectedFixture).ToList();

            Dictionary<Int32, TreeNode> AssembliesDict = new Dictionary<Int32, TreeNode>();
            
            foreach (AssemblyView assemblyView in assy_filteredResult)
            {
                Int32 assyIdAtAssemblyView = assemblyView.id;
                String assy_NodeName = assemblyView.Name == "ROOT" ? assemblyView.Code : String.Format("{0} ({1})", assemblyView.Name, assemblyView.AssemblyType);
                TreeNode Parent_treeNode = new TreeNode()
                {
                    Text = assy_NodeName,
                    Tag = assemblyView
                };

                //Recursive Function for childnodes
                GetChildTreeNodes(Parent_treeNode, part_filteredResult, assyIdAtAssemblyView);
                AssembliesDict.Add(assemblyView.AssemblyID, Parent_treeNode);
            }                       

            foreach (KeyValuePair<Int32, TreeNode> DictItem in AssembliesDict)
            {
                Int32 ParentIndex = ((AssemblyView)DictItem.Value.Tag).ParentID ?? 0;

                if (ParentIndex != 0)
                {
                    AssembliesDict[ParentIndex].Nodes.Add(DictItem.Value);

                }
            }

            Fixture_treeView.Nodes.Clear();
            Fixture_treeView_PR.Nodes.Clear();

            if (tabControl1.SelectedTab == tabPage2)
            {
                try
                {
                    //Add the tree Node to the TAB PARTS/ASSEMBLY
                    TreeNode Assy_RootNode = AssembliesDict.Where(o => ((AssemblyView)o.Value.Tag).ParentID == null).First().Value;
                    Fixture_treeView.Nodes.Add(Assy_RootNode);
                    Fixture_treeView.ExpandAll();

                }
                catch
                {
                    BomManagerFormMsg.NewMessage().AddText("Tree view adding failed").IsError().PrependMessageType().Log();
                }

            }

            else if (tabControl1.SelectedTab == tabPage3)
            {
                try
                {
                    //Add the tree Node to the TAB PARTS RULES
                    TreeNode Assy_RootNode_PR = AssembliesDict.Where(o => ((AssemblyView)o.Value.Tag).ParentID == null).First().Value;
                    Fixture_treeView_PR.Nodes.Add(Assy_RootNode_PR);
                    Fixture_treeView_PR.ExpandAll();

                }
                catch
                {
                    PartRules.NewMessage().AddText("Tree view adding failed").IsError().PrependMessageType().Log();
                }
            }

        }

        private void GetChildTreeNodes(TreeNode Parent_treeNode, List<PartView> part_filteredResult, Int32 currentAssyId)
        {

            foreach (PartView partview in part_filteredResult)
            {
                Int32 assyIdAtPartView = partview.AssRefID;
                                
                if(currentAssyId == assyIdAtPartView)
                {
                    String part_NodeName = String.Format("{0} - {1}", partview.PartName, partview.PartType );// partview.Description);
                    TreeNode childTreeNode = new TreeNode();
                    childTreeNode.Text = part_NodeName;
                    childTreeNode.Tag = partview;

                    Parent_treeNode.Nodes.Add(childTreeNode);
                }
                                
            }
            Dictionary<Int32, TreeNode> PartsDict = new Dictionary<Int32, TreeNode>();

            foreach (KeyValuePair<Int32, TreeNode> PartItem in PartsDict)
            {
                Int32 ParentIndex = ((PartView)PartItem.Value.Tag).id;

                if (ParentIndex != 0)
                {
                    PartsDict[ParentIndex].Nodes.Add(PartItem.Value);

                }
            }      
            
        }
        



        /****************************************************************************************************************************************************************/
        /*                                                                                                                                                              */
        /*                                                              PART RULES TAB                                                                                  */
        /*                                                                                                                                                              */
        /****************************************************************************************************************************************************************/



        private void ProductID_comboBox_PR_SelectionChangeCommitted(object sender, EventArgs e)
        {
            RefreshTreeView();            
            Load_Category_ComboBox();
            Load_Parameter_ComboBox();
        }

        private void Load_Category_ComboBox()
        {
            String productId_PR = productID_comboBox_PR.SelectedValue.ToString();
            CategoryPR_comboBox.DataSource = db.CategoryAtFixtureViews.AsNoTracking().Where(o => o.FixtureId.ToString() == productId_PR ).OrderBy(o => o.DisplayOrder).ToList();
            CategoryPR_comboBox.ValueMember = "CategoryId";
            CategoryPR_comboBox.DisplayMember = "Name";

           
        }

        private void Load_Parameter_ComboBox()
        {
            String productId_PR = productID_comboBox_PR.SelectedValue.ToString();
            String categoryId_PR = CategoryPR_comboBox.SelectedValue.ToString();
            ParameterPR_comboBox.DataSource = db.ParameterAtCategoryAtFixtureViews.Where(o => o.id.ToString() == productId_PR  && o.CategoryId.ToString() == categoryId_PR).ToList();
            ParameterPR_comboBox.ValueMember = "ParameterId";
            ParameterPR_comboBox.DisplayMember = "ParameterCode";

            if (CategoryPR_comboBox.Text == "PRODUCT ID")
            {
                ParameterPR_comboBox.DataSource = db.Fixtures.Where(o => o.id.ToString() == productId_PR).ToList();
                ParameterPR_comboBox.ValueMember = "id";
                ParameterPR_comboBox.DisplayMember = "Code";
            }
        }

        private void ComboBoxCategory_PR_SelectionChangeCommitted(object sender, EventArgs e)
        {

            Load_Parameter_ComboBox();
        }
    }
}
