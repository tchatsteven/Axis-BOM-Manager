using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using AXISAutomation.Solvers.FixtureConfiguration;
using AXISAutomation.Tools.Logging;
using AXISAutomation.Tools.DBConnection;

namespace BOM_MANAGER
{
    public partial class BOM_MANAGER : Form
    {
        /****************************************************************************************************************************************************************/
        /*                                                                                                                                                              */
        /*                                                              PART / ASSEMBLY TAB                                                                             */
        /*                                                                                                                                                              */
        /****************************************************************************************************************************************************************/


        PartRulesFilter newPartRule;

        _RTFMessenger BomManagerFormMsg;
        _RTFMessenger PartRules;
        _RTFMessenger ApplicableParts;
        _RTFMessenger NonApplicablePartSummary;
        _RTFMessenger Template;
        _RTFMessenger MatchSummary;
        _RTFMessenger SolveMechanical;

        AXIS_AutomationEntitiesBOM db;
        _BOM NewBOM;
        AXIS_AutomationEntities FixtureConfigurtorDBConn = new AXIS_AutomationEntities();


        public BOM_MANAGER()
        {
            InitializeComponent();
            db = new AXIS_AutomationEntitiesBOM();            
            BomManagerFormMsg = new _RTFMessenger(eventLog_richTextBox, 0, true) { DefaulSpaceAfter = 0 };//, On = true };//
            PartRules = new _RTFMessenger(eventLog_PR_richTextBox, 0, true) { DefaulSpaceAfter = 0 };//, On = true };
            ApplicableParts = new _RTFMessenger(Log_RichTextBox, 0, true) { DefaulSpaceAfter = 0 };//, On = true };
            NonApplicablePartSummary = new _RTFMessenger(Log2_RichTextBox, 0, true) { DefaulSpaceAfter = 0 };//, On = true };
            Template = new _RTFMessenger(Log2_RichTextBox, 0, true) { DefaulSpaceAfter = 0, On = true };
            MatchSummary = new _RTFMessenger(Log2_RichTextBox, 0, true) { DefaulSpaceAfter = 0, On = true };
            SolveMechanical = new _RTFMessenger(Log2_RichTextBox, 0, true) { DefaulSpaceAfter = 0, On = true };
        }

        private void BOM_MANAGER_Load(object sender, EventArgs e)
        {
            GetFixtureId();
            //SyncRootAssemblies();
            RefreshDataGridView_Assemblies();
            RefreshDataGridView_Part();

            BomManagerFormMsg.On = true;
            PartRules.On = true;
            ApplicableParts.On = true;
            NonApplicablePartSummary.On = true;

            RefreshTreeView();


        }

        private void TabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            String MySelectedFixture = productID_comboBox.Text;
            RefreshDataGridView_Assemblies();
            RefreshDataGridView_Part();
            RefreshTreeView();

            if (BOM_tabControl.SelectedTab == tabPage2)
            {
                AssyAssociationcheck();
                PartAssociationcheck();
            }


            //////////////////////////////////////PART Rules Tab////////////////////////////////////////////////
            else if (BOM_tabControl.SelectedTab == tabPage3)
            {                
                Disable_Filter1();
                Disable_Filter2();
                Disable_Filter3();
                Disable_Filter4();

                PartFilterCheckBox.Checked = true;
                RefreshDataGridView_PartRules();

                Filter_Rules();

                Save_PR.Enabled = false;
                Edit_PR.Enabled = false;
                eventLog_PR_richTextBox.Clear();
                PartRules.NewMessage().AddText("Select a Part to Assign/Edit Rules ").IsWarning().PrependMessageType().Log();

                DataGridView_Rules.DataSource = null;
                DataGridView_Rules.Refresh();

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

            ComboBoxPartType.DataSource = db.AvailablePartTypes.OrderBy(o=> o.Name).Select(o => o.Name).ToList();
            ComboBoxPartType.DisplayMember = "PartTypeName";

            //PART RULE TAB
            productID_comboBox_PR.DataSource = db.Fixtures.OrderBy(o => o.Code).ToList();
            productID_comboBox_PR.ValueMember = "id";
            productID_comboBox_PR.DisplayMember = "Code";

        }

        private void SyncRootAssemblies()
        {
            //List<Fixture> allFixtures = db.Fixtures.ToList();
            //Int32 rootAssyID = db.Assemblies.Where(o => o.Name == "ROOT").First().id;

            //foreach (Fixture currentFixture in allFixtures)
            //{
            //    Boolean rootExists = db.AssemblyAtAssemblies.Any(o => o.FixtureID == currentFixture.id && o.ParentID == null);
            //    if (!rootExists)
            //    {

            //        AssemblyAtAssembly newRoot = new AssemblyAtAssembly()
            //        {
            //            FixtureID = currentFixture.id,
            //            AssemblyID = rootAssyID,

            //        };
            //        db.AssemblyAtAssemblies.Add(newRoot);
            //    }
            //}
            //db.SaveChanges();
        }

        private void RefreshDataGridView_Assemblies()
        {
            try
            {
                dataGridView_Ass.DataSource = db.AssemblyTypeAtAssemblies.ToList();

                //hide columns here
                dataGridView_Ass.Columns["AssemblyID"].Visible = false;
                
                //BomManagerFormMsg.NewMessage().AddText("Assembly Table has been Populated").PrependMessageType().Log();
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
                AssyAssociationcheck();
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

                String Combotext = ComboBoxPartType.Text;


                dataGridView_Part.DataSource = db.PartTypeAtParts.Where(o => o.PartTypeName == Combotext).OrderBy(p => p.PartName).ToList();

                //hide columns here
                dataGridView_Part.Columns["PartID"].Visible = false;
                dataGridView_Part.Columns["TypeID"].Visible = false;
                dataGridView_Part.Columns["PartType_ID"].Visible = false;
                
                dataGridView_Part.AutoResizeColumns();

                PartAssociationcheck();
                eventLog_richTextBox.ScrollToCaret();
                eventLog_richTextBox.Clear();

            }
            catch
            {
                BomManagerFormMsg.NewMessage().AddText("Refreshing Part Table failed").PrependMessageType().Log();
            }
        }

        private void NewAssembly_Click(object sender, EventArgs e)
        {

            Assembly_CreatOrEditForm newAssemblyForm = new Assembly_CreatOrEditForm();
            AssemblyTypeForm NewAssemblyTypeForm = new AssemblyTypeForm();

            DialogResult NewForm = newAssemblyForm.ShowDialog();

            try
            {
                if (NewForm == DialogResult.OK)
                {
                    BomManagerFormMsg.NewMessage().AddText("Assembly Successfully Added").PrependMessageType().Log();
                    db = new AXIS_AutomationEntitiesBOM();
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
                    db = new AXIS_AutomationEntitiesBOM();
                    RefreshDataGridView_Assemblies();
                    BomManagerFormMsg.NewMessage().AddText("Assembly Successfully Edited").PrependMessageType().Log();
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

        private void DeleteAssembly_Click(object sender, EventArgs e)
        {
            String DeletionAssyNames = String.Join(", ", dataGridView_Ass.SelectedRows.OfType<DataGridViewRow>().Select(o => o.Cells["AssemblyName"].Value.ToString()).ToList());

            String assNoun = dataGridView_Ass.SelectedRows.Count > 1 ? "assemblies" : "assembly";

            DialogResult deletingMsg = MessageBox.Show(String.Format("Do you really want to delete {1} {0}", DeletionAssyNames, assNoun), "Confirm Assembly Deletion", MessageBoxButtons.YesNo);

            if (deletingMsg == DialogResult.Yes)
            {

                try
                {
                    foreach (DataGridViewRow currentDeletionRow in dataGridView_Ass.SelectedRows)
                    {
                        //Delete_Function_Ass(currentDeletionRow);
                        Int32 currentAssemblyId = Int32.Parse(currentDeletionRow.Cells["AssemblyID"].Value.ToString());
                        Assembly AssemblyToDelete = db.Assemblies.Find(currentAssemblyId);
                        String assemblyNameToDelete = db.Assemblies.Find(currentAssemblyId).Name;

                        Boolean myAssociation = db.AssemblyAtAssemblies.Any(o => o.AssemblyID == currentAssemblyId);
                        if (!myAssociation)
                        {
                            db.Assemblies.Remove(db.Assemblies.Find(currentAssemblyId));
                            string assemblyname = db.Assemblies.Find(currentAssemblyId).Name;
                            db.SaveChanges();


                            //do not show message if assembly has not been deleted.
                            BomManagerFormMsg.NewMessage().AddText("Assembly : " + assemblyNameToDelete + " has been deleted from TreeView and Database").PrependMessageType().Log();
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
                try
                {
                    Int32 rowIndex = dataGridView_Ass.CurrentCell.RowIndex;
                    Int32 Teststring = Int32.Parse(dataGridView_Ass.Rows[rowIndex].Cells["AssemblyID"].Value.ToString());
                    Boolean myAssociation = db.AssemblyAtAssemblies.Any(o => o.AssemblyID == Teststring);
                    if (myAssociation)
                    {
                        //BomManagerFormMsg.NewMessage().AddText("Delete Button not Available due to assembly association").IsWarning().PrependMessageType().Log();
                    }
                    return myAssociation;

                }
                catch
                {
                    return false;
                }
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

                    db = new AXIS_AutomationEntitiesBOM();
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

        private void EditPart_Click(object sender, EventArgs e)
        {
            String editPartName;
            String editDescription;
            Int32 editPartTypeIndex;
            String editPartType;
           
            Int32 currentPartId = Int32.Parse(dataGridView_Part.SelectedCells[0].OwningRow.Cells[0].Value.ToString());
            Part partToEdit = db.Parts.Find(currentPartId);

            editDescription = partToEdit.Description;
            editPartName = partToEdit.Name;
            editPartTypeIndex = partToEdit.TypeID;

            AvailablePartType partTypeToEdit = db.AvailablePartTypes.Find(editPartTypeIndex);
            editPartType = partTypeToEdit.Name;
                      
            Part_CreateOrEditForm editPartForm = new Part_CreateOrEditForm(editPartName, editDescription, editPartType);

            DialogResult NewForm = editPartForm.ShowDialog();
            try
            {
                if (NewForm == DialogResult.OK)
                {

                    db = new AXIS_AutomationEntitiesBOM();
                    RefreshDataGridView_Part();

                    BomManagerFormMsg.NewMessage().AddText("Part Successfully Edited").PrependMessageType().Log();
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

        private void DeletePart_Click(object sender, EventArgs e)
        {
            String deletionPartName = String.Join(", ", dataGridView_Part.SelectedRows.OfType<DataGridViewRow>().Select(o => o.Cells["PartName"].Value.ToString()).ToList());
            String assNoun = dataGridView_Part.SelectedRows.Count > 1 ? "Parts" : "Part";

            DialogResult deletingMsg = MessageBox.Show(String.Format("Do you really want to delete {1} {0}", deletionPartName, assNoun), "Confirm Assembly Deletion", MessageBoxButtons.YesNo);

            if (deletingMsg == DialogResult.Yes)
            {
                try
                {
                    foreach (DataGridViewRow currentDeletionRow in dataGridView_Part.SelectedRows)
                    {
                        Int32 currentPartId = Int32.Parse(currentDeletionRow.Cells["PartID"].Value.ToString());
                        Part partToDelete = db.Parts.Find(currentPartId);
                        String partyNameToDelete = db.Parts.Find(currentPartId).Name;

                        Boolean myAssociation = db.PartAtAssemblies.Any(o => o.PartID == currentPartId);
                        if (!myAssociation)
                        {
                            
                            db.Parts.Remove(db.Parts.Find(currentPartId));
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
                try
                {
                    Int32 rowIndex = dataGridView_Part.CurrentCell.RowIndex;
                    Int32 Teststring = Int32.Parse(dataGridView_Part.Rows[rowIndex].Cells["PartID"].Value.ToString());
                    Boolean myAssociation = db.PartAtAssemblies.Any(o => o.PartID == Teststring);
                    if (myAssociation)
                    {
                        //BomManagerFormMsg.NewMessage().AddText("Delete Button not Available due to Part association").IsWarning().PrependMessageType().Log();
                    }
                    return myAssociation;

                }
                catch
                {
                    return false;
                }
                
            }
        }

        private void ComboBoxFixtureFamily_SelectedIndexChanged(object sender, EventArgs e)
        {
            RefreshDataGridView_Part();
            PartAssociationcheck();
        }

        private void AddButton_AssyToAssy_TreeView_Click(object sender, EventArgs e)
        {
            try
            {
                AssemblyView SelectedAssemblyNode = (AssemblyView)Fixture_treeView.SelectedNode.Tag;
                Int32? CurrentParentId = SelectedAssemblyNode.id;
                
                foreach (DataGridViewRow currentSelectedRow in dataGridView_Ass.SelectedRows)
                {
                    //Function to Save Assy to parent
                    AddAssyToTreeView(currentSelectedRow, CurrentParentId); 
                }
            }
            catch
            {
                BomManagerFormMsg.NewMessage().AddText("No Fixture Selected in the TreeView").IsError().PrependMessageType().Log();
            }
            RefreshTreeView();

        }

        private void AddAssyToTreeView(DataGridViewRow currentSelectedRow, Int32? ParentID)
        {
            Int32 currentAssemblyId = Int32.Parse(currentSelectedRow.Cells["AssemblyID"].Value.ToString());
            //Int32 currentAssemblyTypeId = Int32.Parse(currentSelectedRow.Cells["AssemblyTypeID"].Value.ToString());

            String assemblyNameToAdd = db.Assemblies.Find(currentAssemblyId).Name;
            Boolean myAssociation = db.AssemblyAtAssemblies.Any(o => o.AssemblyID == currentAssemblyId && o.ParentID == ParentID );

            //Check if Sub-Assembly is ROOT Assembly... if yues do not add
            if (!myAssociation)
            {
                AssemblyAtAssembly NewAssemblyAtAssembly = new AssemblyAtAssembly();

                db.AssemblyAtAssemblies.Add(NewAssemblyAtAssembly);

                NewAssemblyAtAssembly.ParentID = (Int32)ParentID;
                NewAssemblyAtAssembly.AssemblyID = currentAssemblyId;
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
                    RefreshTreeView();

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
            try
            {
                Int32? currentAssemblyViewIndex = ((AssemblyView)selectedNode.Tag).AssyAtAssyID;
                String CurrentAssemblyViewName = ((AssemblyView)selectedNode.Tag).AssemblyName;
                AssemblyAtAssembly deletionTarget = db.AssemblyAtAssemblies.Find(currentAssemblyViewIndex);
                //if (deletionTarget.ParentID is null)
                //{
                //    BomManagerFormMsg.NewMessage().AddText("Root Assembly deletion is forbidden").IsError().PrependMessageType().Log();
                //}
                //else
                //{
                    db.AssemblyAtAssemblies.Remove(deletionTarget);
                    BomManagerFormMsg.NewMessage().AddText("Assembly " + CurrentAssemblyViewName + " has been deleted from Tree view").PrependMessageType().Log();

                //}
            }
            catch
            {
                BomManagerFormMsg.NewMessage().AddText("Root Assembly deletion is forbidden").IsError().PrependMessageType().Log();
            }

        }

        private void Fixture_treeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            eventLog_richTextBox.Clear();
            TreeNode selectedNode = Fixture_treeView.SelectedNode;
            try
            {
                Int32 targetAssemblyId = ((AssemblyView)selectedNode.Tag).id;
                String targetAssemblyName = ((AssemblyView)selectedNode.Tag).AssemblyName;

                List<PartRulesFilter> AssyFilterToDelete = db.PartRulesFilters.Where(o => o.AssemblyID == targetAssemblyId && o.ProductCode == productID_comboBox.Text).ToList();
                if (AssyFilterToDelete.Count != 0)
                {
                    RemoveButton_AssyToAssy_TreeView.Enabled = false;
                    Remove_PartToAssy_TreeView.Enabled = false;
                    BomManagerFormMsg.NewMessage().AddText("Assembly cannot be removed from tree view due to Filter Association in Part Rules Tab").IsWarning().PrependMessageType().Log();
                }
                else
                {
                    RemoveButton_AssyToAssy_TreeView.Enabled = true;
                    Remove_PartToAssy_TreeView.Enabled = true;
                }
            }
            catch
            {

            }

            try
            {
                Int32 targetPartId = ((PartView)selectedNode.Tag).PartID;
                String targetPartName = ((PartView)selectedNode.Tag).PartName;

                List <PartRulesFilter> PartFilterToDelete = db.PartRulesFilters.Where(o => o.PartID == targetPartId && o.ProductCode == productID_comboBox.Text).ToList();
                if (PartFilterToDelete.Count != 0)
                {
                    RemoveButton_AssyToAssy_TreeView.Enabled = false;
                    Remove_PartToAssy_TreeView.Enabled = false;
                    BomManagerFormMsg.NewMessage().AddText("Part cannot be removed from tree view due to Filter Association in Part Rules Tab").IsWarning().PrependMessageType().Log();
                }
                else
                {
                    RemoveButton_AssyToAssy_TreeView.Enabled = true;
                    Remove_PartToAssy_TreeView.Enabled = true;
                }

            }
            catch
            {

            }

            
            

           
        }

        private void AddButton_PartToAssy_TreeView_Click(object sender, EventArgs e)
        {
            try
            {
                AssemblyView SelectedAssemblyNode = (AssemblyView)Fixture_treeView.SelectedNode.Tag;

                Int32? CurrentAssemblyId = SelectedAssemblyNode.id;
                //Int32? CurrentAssemblyId = SelectedAssemblyNode.id;
                //Int32? CurrentFixtureCodeId = SelectedAssemblyNode.FixtureID;

                foreach (DataGridViewRow currentSelectedRow in dataGridView_Part.SelectedRows)
                {
                    //Function to Save Assy to parent
                    AddPartsToTreeView(currentSelectedRow, CurrentAssemblyId);//, CurrentAssemblyId, CurrentFixtureCodeId);
                }
            }
            catch
            {
                BomManagerFormMsg.NewMessage().AddText("No Fixture/Assembly Selected in the TreeView. Parts cannot be added to other Parts.").IsError().PrependMessageType().Log();
            }
            RefreshTreeView();

        }

        private void AddPartsToTreeView(DataGridViewRow currentSelectedRow, Int32? assemblyId)
        {
            Int32 currentPartId = Int32.Parse(currentSelectedRow.Cells["PartID"].Value.ToString());
            //PartTypeAtParts
            String PartNameToAdd = db.Parts.Find(currentPartId).Name;

            try
            {
                PartAtAssembly NewPartAtAssembly = new PartAtAssembly();

                db.PartAtAssemblies.Add(NewPartAtAssembly);

                NewPartAtAssembly.PartID = currentPartId;
                NewPartAtAssembly.AssemblyID = (Int32)assemblyId;
                //NewPartAtAssembly.FixtureID = (Int32)FixtureId;
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
                Int32 currentId = ((PartView)selectedNode.Tag).id;
                String currentPartNameId = ((PartView)selectedNode.Tag).PartName;
                PartAtAssembly deletionTarget = db.PartAtAssemblies.Find(currentId);

                //Delete part in Rules List
                Int32 currentPartID = ((PartView)selectedNode.Tag).PartID;
                String currentFixtureID = ((PartView)selectedNode.Tag).ProductCode;

                List<PartRulesFilter> deletionTargetRulesList = db.PartRulesFilters.Where(o => o.ProductCode == currentFixtureID && o.PartID == currentPartID).ToList();

                foreach (var test in deletionTargetRulesList)
                {
                    Int32 deletionIndex = Int32.Parse(test.id.ToString());
                    db.PartRulesFilters.Remove(db.PartRulesFilters.Find(deletionIndex));
                }



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

        private void DataGridView_Ass_MouseClick(object sender, MouseEventArgs e)
        {
            AssyAssociationcheck();

        }

        private void DataGridView_Part_MouseClick(object sender, MouseEventArgs e)
        {
            PartAssociationcheck();
        }




        public String MySelectedFixture
        {
            get
            {
                String MySelectedFixture;                

                if (BOM_tabControl.SelectedTab == tabPage2)
                {                    
                    MySelectedFixture = productID_comboBox.Text;

                }
                else if (BOM_tabControl.SelectedTab == tabPage3)
                {                   
                    MySelectedFixture = productID_comboBox_PR.Text;
                }
                else
                {
                    string FixCode = FixtureSetupCode_TextBox.Text;
                    string[] Fixcodesplit = FixCode.Split('-');
                    MySelectedFixture = Fixcodesplit[0];
                }
                return MySelectedFixture;
            }
        }

        public TreeNode GetRootTreeNode(String FixtureCode)
        {
            AssemblyView RootAssy = db.AssemblyViews.Where(ass => ass.AssemblyName == FixtureCode).First();
            return GetAssemblyTreeNode(RootAssy);
        }

        public TreeNode GetAssemblyTreeNode(AssemblyView targetAssy)
        {
            TreeNode treeNode = new TreeNode()
            {
                Text = targetAssy.AssemblyName,
                Tag = targetAssy
            };

            GetMyChildren(treeNode);
            return treeNode;
        }

        public TreeNode GetPartTreeNode(PartView targetPart)
        {
            TreeNode treeNode = new TreeNode()
            {
                Text = targetPart.PartName,
                Tag = targetPart
            };
            
            return treeNode;
        }

        public void GetMyChildren(TreeNode parentTreeNode)
        {
            AssemblyView ParentAssy = (AssemblyView)parentTreeNode.Tag;
            db.AssemblyViews.OrderBy(ass=>ass.AssemblyName).Where(ass => ass.ParentIDAtAssyAtAssy == ParentAssy.id).ToList().ForEach(a => parentTreeNode.Nodes.Add(GetAssemblyTreeNode(a)));
            db.PartViews.OrderBy(p=>p.PartName).Where(p => p.AssemblyID == ParentAssy.id).ToList().ForEach(p => parentTreeNode.Nodes.Add(GetPartTreeNode(p)));
        }




        private void RefreshTreeView()
        {           
            Fixture_treeView.Nodes.Clear();
            Fixture_treeView_PartRule.Nodes.Clear();
            Fixture_treeView_PartGen.Nodes.Clear();

            if (BOM_tabControl.SelectedTab == tabPage2)
            {
                try
                {                    
                    TreeNode Assy_RootNode = GetRootTreeNode(MySelectedFixture);
                    Fixture_treeView.Nodes.Add(Assy_RootNode);
                    Fixture_treeView.ExpandAll();

                    Fixture_treeView.Nodes[0].EnsureVisible();

                }
                catch
                {
                    BomManagerFormMsg.NewMessage().AddText("Tree view adding failed").IsError().PrependMessageType().Log();
                }

            }

            else if (BOM_tabControl.SelectedTab == tabPage3)
            {
                try
                {                    
                    TreeNode Assy_RootNode_PartRule = GetRootTreeNode(MySelectedFixture);
                    Fixture_treeView_PartRule.Nodes.Add(Assy_RootNode_PartRule);
                    Fixture_treeView_PartRule.ExpandAll();

                    Fixture_treeView_PartRule.Nodes[0].EnsureVisible();

                    //Select FirstNode in the TreeView.
                    //if (Assy_RootNode_PartRule.Index == 0)
                    //{

                    //    Fixture_treeView_PartRule.SelectedNode = Assy_RootNode_PartRule.NextVisibleNode;
                    //}


                }
                catch
                {
                    PartRules.NewMessage().AddText("Tree view adding failed").IsError().PrependMessageType().Log();
                }
            }

            else if (BOM_tabControl.SelectedTab == tabPage1)
            {
                try
                {
                    TreeNode Assy_RootNode_PartGen = GetRootTreeNode(MySelectedFixture);

                    Fixture_treeView_PartGen.Nodes.Add(Assy_RootNode_PartGen);
                    Fixture_treeView_PartGen.ExpandAll();

                    Fixture_treeView_PartGen.Nodes[0].EnsureVisible();

                }
                catch
                {
                    PartRules.NewMessage().AddText("Tree view adding failed").IsError().PrependMessageType().Log();
                }
            }

        }


     



        /****************************************************************************************************************************************************************/
        /*                                                                                                                                                              */
        /*                                                              PART RULES TAB                                                                                  */
        /*                                                                                                                                                              */
        /****************************************************************************************************************************************************************/



        public String G_PartName => Fixture_treeView_PartRule.SelectedNode.Text;//db.PartRulesFilters.Find(CurrentPartId).PartName;

        public Int32? CurrentPartId => db.PartRulesFilters.Find(G_PartName).PartID;//Int32.Parse(DataGridView_Rules.SelectedCells[0].OwningRow.Cells[0].Value.ToString());

        public PartRulesFilter PartRuleToEdit => db.PartRulesFilters.Find(CurrentPartId);

        public String EditPartName => PartRuleToEdit.Part.Name;

        public String EditProductCode => PartRuleToEdit.ProductCode;

        public String EditCategoryName => PartRuleToEdit.CategoryName;

        public String EditParameterName => PartRuleToEdit.ParameterName;

        public Int32? EditQty => PartRuleToEdit.Quantity;

        public String EditPACAF_Id => PartRuleToEdit.PACAF_ID;

        public String EditFilterRule => PartRuleToEdit.FilterBehavior.Behavior;

        public Int32 EditProductCodeIndex => PartRuleToEdit.ProductID;


        // Filter Data
        public String Filter_PartName => Fixture_treeView_PartRule.SelectedNode.Text;

        public String Filter_ProductCode => productID_comboBox_PR.Text;

        public Int32 Filter_ProductIdIndex => Int32.Parse(productID_comboBox_PR.SelectedValue.ToString());

        public String selected_PartName;

        public String selected_AssemblyName;

        public Int32? part_Index
        {
            get
            {
                try
                {
                    selected_PartName = Filter_PartName;
                    return Int32.Parse(db.Parts.Where(m => m.Name == Filter_PartName).First().id.ToString());
                }
                catch
                {
                    selected_PartName = null;
                    return null;
                }
            }



        }

        public Int32? assembly_Index
        {
            get
            {
                try
                {
                    selected_AssemblyName = Filter_PartName;
                    return Int32.Parse(db.Assemblies.Where(m => m.Name == Filter_PartName).First().id.ToString());
                }
                catch
                {
                    selected_AssemblyName = null;
                    return null;

                }
            }

        }










        //ComboBox Event Changes
        private void ProductID_comboBox_PR_SelectionChangeCommitted(object sender, EventArgs e)
        {
            // change  different 
            //Filters according 
            //to the selection 
            //in the filter type selected

            RefreshTreeView();

            //Load_Category_ComboBox();
            //Load_Parameter_ComboBox();
            RefreshDataGridView_PartRules();

        }

        private void FilterType_comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Rules_ComboBox1();
        }

        private void FilterType_comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            Rules_ComboBox2();

        }

        private void FilterType_comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            Rules_ComboBox3();

        }

        private void FilterType_comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            Rules_ComboBox4();

        }

        private void CategoryPR_comboBoxFilter1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Parameter_ListView1();
        }

        private void CategoryPR_comboBoxFilter2_SelectedIndexChanged(object sender, EventArgs e)
        {
            Parameter_ListView2();
        }

        private void CategoryPR_comboBoxFilter3_SelectedIndexChanged(object sender, EventArgs e)
        {
            Parameter_ListView3();
        }

        private void CategoryPR_comboBoxFilter4_SelectedIndexChanged(object sender, EventArgs e)
        {
            Parameter_ListView4();
        }



        //Load DATASOURCES
        private void Filter_Rules()
        {
            FilterType_comboBox1.DataSource = db.FilterTypes.ToList();
            FilterType_comboBox1.ValueMember = "id";
            FilterType_comboBox1.DisplayMember = "FilterTypeName";

            FilterType_comboBox2.DataSource = db.FilterTypes.ToList();
            FilterType_comboBox2.ValueMember = "id";
            FilterType_comboBox2.DisplayMember = "FilterTypeName";

            FilterType_comboBox3.DataSource = db.FilterTypes.ToList();
            FilterType_comboBox3.ValueMember = "id";
            FilterType_comboBox3.DisplayMember = "FilterTypeName";

            FilterType_comboBox4.DataSource = db.FilterTypes.ToList();
            FilterType_comboBox4.ValueMember = "id";
            FilterType_comboBox4.DisplayMember = "FilterTypeName";

        }

        private void Rules_ComboBox1()
        {
            Filter1_Visibility();

            //String productId_PR = productID_comboBox_PR.SelectedValue.ToString();

            if (FilterType_comboBox1.Text == "PACAF")
            {              
                //Category ComboBox
                CategoryPR_comboBoxFilter1.DataSource = db.ProductTemplates.AsNoTracking().Where(o => o.FixtureId == Filter_ProductIdIndex && o.CategoryName != "PRODUCT ID").Select(o => new { o.CAF_Id, o.CategoryName, o.CAF_DisplayOrder }).Distinct().OrderBy(o => o.CAF_DisplayOrder).ToList();
                CategoryPR_comboBoxFilter1.ValueMember = "CAF_Id";
                CategoryPR_comboBoxFilter1.DisplayMember = "CategoryName";

                //Behavior ComboBox
                FilterBehavior_ComboBox1.DataSource = db.FilterBehaviors.ToList();
                FilterBehavior_ComboBox1.ValueMember = "id";
                FilterBehavior_ComboBox1.DisplayMember = "Behavior";

            }

            else if (FilterType_comboBox1.Text != "PACAF")
            {
                CategoryPR_comboBoxFilter1.SelectedValue = -1;
                FilterBehavior_ComboBox1.SelectedValue = -1;

            }

            if (FilterType_comboBox1.Text == "DEPENDABLE QTY")
            {               
                //Dependable ComboBox
                DependableQtyPR_comboBoxFilter1.DataSource = db.DependableQuantities.OrderBy(o => o.DependableQuantityName).ToList();
                DependableQtyPR_comboBoxFilter1.ValueMember = "id";
                DependableQtyPR_comboBoxFilter1.DisplayMember = "DependableQuantityName";

            }

            else if (FilterType_comboBox1.Text != "DEPENDABLE QTY")
            {
                DependableQtyPR_comboBoxFilter1.SelectedValue = -1;

            }

            if (FilterType_comboBox1.Text == "RENAMINGEXPRESSION")
            {
                //Dependable ComboBox
                RE_comboBox1.DataSource = db.RenamingExpressions.OrderBy(o => o.id).ToList();
                RE_comboBox1.ValueMember = "id";
                RE_comboBox1.DisplayMember = "ExpressionName";

            }

            else if (FilterType_comboBox1.Text != "RENAMINGEXPRESSION")
            {
                RE_comboBox1.SelectedValue = -1;

            }

        }

        private void Rules_ComboBox2()
        {
            Filter2_Visibility();

            if (FilterType_comboBox2.Text == "PACAF")
            {
                //Category ComboBox
                CategoryPR_comboBoxFilter2.DataSource = db.ProductTemplates.AsNoTracking().Where(o => o.FixtureId == Filter_ProductIdIndex && o.CategoryName != "PRODUCT ID").Select(o => new { o.CAF_Id, o.CategoryName, o.CAF_DisplayOrder }).Distinct().OrderBy(o => o.CAF_DisplayOrder).ToList();
                CategoryPR_comboBoxFilter2.ValueMember = "CAF_Id";
                CategoryPR_comboBoxFilter2.DisplayMember = "CategoryName";

                //Behavior ComboBox
                FilterBehavior_ComboBox2.DataSource = db.FilterBehaviors.ToList();
                FilterBehavior_ComboBox2.ValueMember = "id";
                FilterBehavior_ComboBox2.DisplayMember = "Behavior";
            }

            else if (FilterType_comboBox2.Text != "PACAF")
            {
                CategoryPR_comboBoxFilter2.SelectedValue = -1;
                FilterBehavior_ComboBox2.SelectedValue = -1;

            }

            if (FilterType_comboBox2.Text == "DEPENDABLE QTY")
            {
                //Dependable ComboBox
                DependableQtyPR_comboBoxFilter2.DataSource = db.DependableQuantities.OrderBy(o => o.DependableQuantityName).ToList();
                DependableQtyPR_comboBoxFilter2.ValueMember = "id";
                DependableQtyPR_comboBoxFilter2.DisplayMember = "DependableQuantityName";

            }

            else if (FilterType_comboBox2.Text != "DEPENDABLE QTY")
            {
                DependableQtyPR_comboBoxFilter2.SelectedValue = -1;

            }

            if (FilterType_comboBox2.Text == "RENAMINGEXPRESSION")
            {
                //Dependable ComboBox
                RE_comboBox2.DataSource = db.RenamingExpressions.OrderBy(o => o.id).ToList();
                RE_comboBox2.ValueMember = "id";
                RE_comboBox2.DisplayMember = "ExpressionName";

            }

            else if (FilterType_comboBox2.Text != "RENAMINGEXPRESSION")
            {
                RE_comboBox2.SelectedValue = -1;

            }
        }

        private void Rules_ComboBox3()
        {
            Filter3_Visibility();

            if (FilterType_comboBox3.Text == "PACAF")
            {
                //Category ComboBox
                CategoryPR_comboBoxFilter3.DataSource = db.ProductTemplates.AsNoTracking().Where(o => o.FixtureId == Filter_ProductIdIndex && o.CategoryName != "PRODUCT ID").Select(o => new { o.CAF_Id, o.CategoryName, o.CAF_DisplayOrder }).Distinct().OrderBy(o => o.CAF_DisplayOrder).ToList();

                CategoryPR_comboBoxFilter3.ValueMember = "CAF_Id";
                CategoryPR_comboBoxFilter3.DisplayMember = "CategoryName";

                //Behavior ComboBox
                FilterBehavior_ComboBox3.DataSource = db.FilterBehaviors.ToList();
                FilterBehavior_ComboBox3.ValueMember = "id";
                FilterBehavior_ComboBox3.DisplayMember = "Behavior";
            }

            else if (FilterType_comboBox3.Text != "PACAF")
            {
                CategoryPR_comboBoxFilter3.SelectedValue = -1;
                FilterBehavior_ComboBox3.SelectedValue = -1;

            }

            if (FilterType_comboBox3.Text == "DEPENDABLE QTY")
            {
                //Dependable ComboBox
                DependableQtyPR_comboBoxFilter3.DataSource = db.DependableQuantities.OrderBy(o => o.DependableQuantityName).ToList();
                DependableQtyPR_comboBoxFilter3.ValueMember = "id";
                DependableQtyPR_comboBoxFilter3.DisplayMember = "DependableQuantityName";

            }

            else if (FilterType_comboBox3.Text != "DEPENDABLE QTY")
            {
                DependableQtyPR_comboBoxFilter3.SelectedValue = -1;

            }

            if (FilterType_comboBox3.Text == "RENAMINGEXPRESSION")
            {
                //Dependable ComboBox
                RE_comboBox3.DataSource = db.RenamingExpressions.OrderBy(o => o.id).ToList();
                RE_comboBox3.ValueMember = "id";
                RE_comboBox3.DisplayMember = "ExpressionName";

            }

            else if (FilterType_comboBox3.Text != "RENAMINGEXPRESSION")
            {
                RE_comboBox3.SelectedValue = -1;

            }
        }

        private void Rules_ComboBox4()
        {
            Filter4_Visibility();

            if (FilterType_comboBox4.Text == "PACAF")
            {
                //Category ComboBox
                CategoryPR_comboBoxFilter4.DataSource = db.ProductTemplates.AsNoTracking().Where(o => o.FixtureId == Filter_ProductIdIndex && o.CategoryName != "PRODUCT ID").Select(o => new { o.CAF_Id, o.CategoryName, o.CAF_DisplayOrder }).Distinct().OrderBy(o => o.CAF_DisplayOrder).ToList();

                CategoryPR_comboBoxFilter4.ValueMember = "CAF_Id";
                CategoryPR_comboBoxFilter4.DisplayMember = "CategoryName";

                //Behavior ComboBox
                FilterBehavior_ComboBox4.DataSource = db.FilterBehaviors.ToList();
                FilterBehavior_ComboBox4.ValueMember = "id";
                FilterBehavior_ComboBox4.DisplayMember = "Behavior";
            }

            else if (FilterType_comboBox4.Text != "PACAF")
            {
                CategoryPR_comboBoxFilter4.SelectedValue = -1;
                FilterBehavior_ComboBox4.SelectedValue = -1;

            }

            if (FilterType_comboBox4.Text == "DEPENDABLE QTY")
            {
                //Dependable ComboBox
                DependableQtyPR_comboBoxFilter4.DataSource = db.DependableQuantities.OrderBy(o => o.DependableQuantityName).ToList();
                DependableQtyPR_comboBoxFilter4.ValueMember = "id";
                DependableQtyPR_comboBoxFilter4.DisplayMember = "DependableQuantityName";

            }

            else if (FilterType_comboBox4.Text != "DEPENDABLE QTY")
            {
                DependableQtyPR_comboBoxFilter4.SelectedValue = -1;

            }

            if (FilterType_comboBox4.Text == "RENAMINGEXPRESSION")
            {
                //Dependable ComboBox
                RE_comboBox4.DataSource = db.RenamingExpressions.OrderBy(o => o.id).ToList();
                RE_comboBox4.ValueMember = "id";
                RE_comboBox4.DisplayMember = "ExpressionName";

            }

            else if (FilterType_comboBox4.Text != "RENAMINGEXPRESSION")
            {
                RE_comboBox4.SelectedValue = -1;

            }
        }



        private void Parameter_ListView1()
        {
            if (FilterType_comboBox1.Text == "PACAF")
            {
                ParameterPR_ListView1.Clear();

                //Check if Rule exist for that filter
                if (DataGridView_Rules.Rows.Count == 0 || editButtonClicked == true)
                {
                    try
                    {
                        Int32 CAF = Int32.Parse(CategoryPR_comboBoxFilter1.SelectedValue.ToString());
                        db.CategoryAtFixtures.Find(CAF).ParameterAtCategoryAtFixtures.OrderBy(o => o.DisplayOrder).ToList().ForEach(p => ParameterPR_ListView1.Items.Add(p.Parameter.Code, p.id));//+ " (" + p.id + ") " ));
                    }
                    catch
                    {

                    }

                }
                else
                {
                    try
                    {                        

                        Int32? CAF = db.PartRulesFilters.Where(o => (o.PartID == part_Index && o.AssemblyID == assembly_Index) && o.ProductID == Filter_ProductIdIndex && o.OrderOfExecution == 1).First().CategoryID;

                        db.CategoryAtFixtures.Find(CAF).ParameterAtCategoryAtFixtures.OrderBy(o => o.DisplayOrder).ToList().ForEach(p => ParameterPR_ListView1.Items.Add(p.Parameter.Code, p.id));

                    }
                    catch
                    {

                    }
                }
            }

            else if (FilterType_comboBox1.Text != "PACAF")
            {
                ParameterPR_ListView1.Clear();

            }
        }

        private void Parameter_ListView2()
        {
            if (FilterType_comboBox2.Text == "PACAF")
            {
                ParameterPR_ListView2.Clear();
                if (DataGridView_Rules.Rows.Count == 0 || editButtonClicked == true)
                {
                    try
                    {
                        Int32 CAF = Int32.Parse(CategoryPR_comboBoxFilter2.SelectedValue.ToString());
                        db.CategoryAtFixtures.Find(CAF).ParameterAtCategoryAtFixtures.OrderBy(o => o.DisplayOrder).ToList().ForEach(p => ParameterPR_ListView2.Items.Add(p.Parameter.Code, p.id));//+ " (" + p.id + ") " ));
                    }
                    catch
                    {

                    }
                }
                else
                {
                    try
                    {                       

                        Int32? CAF = db.PartRulesFilters.Where(o => (o.PartID == part_Index && o.AssemblyID == assembly_Index) && o.ProductID == Filter_ProductIdIndex && o.OrderOfExecution == 2).First().CategoryID;

                        db.CategoryAtFixtures.Find(CAF).ParameterAtCategoryAtFixtures.OrderBy(o => o.DisplayOrder).ToList().ForEach(p => ParameterPR_ListView2.Items.Add(p.Parameter.Code, p.id));

                    }
                    catch
                    {

                    }
                }
            }

            else if (FilterType_comboBox2.Text != "PACAF")
            {
                ParameterPR_ListView2.Clear();

            }
        }

        private void Parameter_ListView3()
        {
            if (FilterType_comboBox3.Text == "PACAF")
            {
                ParameterPR_ListView3.Clear();
                if (DataGridView_Rules.Rows.Count == 0 || editButtonClicked == true)
                {
                    try
                    {
                        Int32 CAF = Int32.Parse(CategoryPR_comboBoxFilter3.SelectedValue.ToString());
                        db.CategoryAtFixtures.Find(CAF).ParameterAtCategoryAtFixtures.OrderBy(o => o.DisplayOrder).ToList().ForEach(p => ParameterPR_ListView3.Items.Add(p.Parameter.Code, p.id));//+ " (" + p.id + ") " ));
                    }
                    catch
                    {

                    }
                }
                else
                {
                    try
                    {
                       
                        Int32? CAF = db.PartRulesFilters.Where(o => (o.PartID == part_Index && o.AssemblyID == assembly_Index) && o.ProductID == Filter_ProductIdIndex && o.OrderOfExecution == 3).First().CategoryID;

                        db.CategoryAtFixtures.Find(CAF).ParameterAtCategoryAtFixtures.OrderBy(o => o.DisplayOrder).ToList().ForEach(p => ParameterPR_ListView3.Items.Add(p.Parameter.Code, p.id));

                    }
                    catch
                    {

                    }
                }
            }

            else if (FilterType_comboBox3.Text != "PACAF")
            {
                ParameterPR_ListView3.Clear();

            }
        }

        private void Parameter_ListView4()
        {
            if (FilterType_comboBox4.Text == "PACAF")
            {
                ParameterPR_ListView4.Clear();

                //Check if Rule exist for that filter
                if (DataGridView_Rules.Rows.Count == 0 || editButtonClicked == true)
                {
                    try
                    {
                        Int32 CAF = Int32.Parse(CategoryPR_comboBoxFilter4.SelectedValue.ToString());
                        db.CategoryAtFixtures.Find(CAF).ParameterAtCategoryAtFixtures.OrderBy(o => o.DisplayOrder).ToList().ForEach(p => ParameterPR_ListView4.Items.Add(p.Parameter.Code, p.id));
                    }
                    catch
                    {

                    }

                }
                else
                {
                    try
                    {                        
                        Int32? CAF = db.PartRulesFilters.Where(o => (o.PartID == part_Index && o.AssemblyID == assembly_Index) && o.ProductID == Filter_ProductIdIndex && o.OrderOfExecution == 4).First().CategoryID;

                        db.CategoryAtFixtures.Find(CAF).ParameterAtCategoryAtFixtures.OrderBy(o => o.DisplayOrder).ToList().ForEach(p => ParameterPR_ListView4.Items.Add(p.Parameter.Code, p.id));

                    }
                    catch
                    {

                    }
                }
            }

            else if (FilterType_comboBox4.Text != "PACAF")
            {
                ParameterPR_ListView4.Clear();

            }
        }



        //Load Filter Types from DATABASE in to BOM MANAGER
        private void Load_FilterType1FromDB()
        {
            ParameterPR_ListView1.SelectedItems.Clear();
            
            try
            {
                FilterType_comboBox1.SelectedValue = db.PartRulesFilters.Where(o => (o.AssemblyID == assembly_Index && o.PartID == part_Index) && o.ProductID == Filter_ProductIdIndex && o.OrderOfExecution == 1).First().FilterTypeID;

            }

            catch (NullReferenceException)
            {
                //PartRules.NewMessage().AddText(PartName + ": No PACAF Fixture Rules for Filter 1 Type exist. ").AddText(ex.Message).IsError().PrependMessageType().Log();
                //FilterType_comboBox1.SelectedValue = 1;
            }

            catch (InvalidOperationException ex1)
            {
                PartRules.NewMessage().AddText("No Rules for Filter 1 Type exist for selected part. ").AddText(ex1.Message).PrependMessageType().Log();
                FilterType_comboBox1.SelectedValue = 1;
            }

            catch (ArgumentNullException)
            {
                //PartRules.NewMessage().AddText("No Fixture Rules for selected part. ").AddText(ex2.Message).PrependMessageType().Log();
                //FilterType_comboBox1.SelectedValue = 1;
            }

            if (FilterType_comboBox1.Text == "NONE")
            {
                Filter1_Visibility();
                Load_ParameterSelect1();
                Load_DependableQuantityORRenamingExpression1();
                Load_Quantity1();
            }

            else if (FilterType_comboBox1.Text == "PACAF")
            {
                Filter1_Visibility();
                Load_ParameterSelect1();
                Load_DependableQuantityORRenamingExpression1();
                Load_Quantity1();
            }

            else if (FilterType_comboBox1.Text == "QUANTITY")// && styles.Count == 2)
            {
                Filter1_Visibility();
                Load_ParameterSelect1();
                Load_DependableQuantityORRenamingExpression1();
                Load_Quantity1();

            }

            else if (FilterType_comboBox1.Text == "JOINER QTY")
            {
                Filter1_Visibility();
                Load_ParameterSelect1();
                Load_DependableQuantityORRenamingExpression1();
                Load_Quantity1();
            }

            else if (FilterType_comboBox1.Text == "JOINER")
            {
                Filter1_Visibility();
                Load_ParameterSelect1();
                Load_DependableQuantityORRenamingExpression1();
                Load_Quantity1();
            }

            else if (FilterType_comboBox1.Text == "ENDCAP QTY")
            {
                Filter1_Visibility();
                Load_ParameterSelect1();
                Load_DependableQuantityORRenamingExpression1();
                Load_Quantity1();
            }

            else if (FilterType_comboBox1.Text == "DEPENDABLE QTY")//&& styles.Count == 3)
            {
                Filter1_Visibility();
                Load_ParameterSelect1();
                Load_DependableQuantityORRenamingExpression1();
                Load_Quantity1();

            }

            else if (FilterType_comboBox1.Text == "RENAMINGEXPRESSION")
            {
                Filter1_Visibility();
                Load_ParameterSelect1();
                Load_DependableQuantityORRenamingExpression1();
                Load_Quantity1();

            }

        }

        private void Load_FilterType2FromDB()
        {
            ParameterPR_ListView2.SelectedItems.Clear();
       
            try
            {              
                FilterType_comboBox2.SelectedValue = db.PartRulesFilters.Where(o => (o.PartID == part_Index && o.AssemblyID == assembly_Index) && o.ProductID == Filter_ProductIdIndex && o.OrderOfExecution == 2).First().FilterTypeID;

            }

            catch (NullReferenceException)
            {
                //PartRules.NewMessage().AddText(PartName + ": No PACAF Fixture Rules for Filter 2 Type exist. ").AddText(ex.Message).IsError().PrependMessageType().Log();
                //FilterType_comboBox2.SelectedValue = 1;
            }

            catch (InvalidOperationException ex1)
            {
                PartRules.NewMessage().AddText("No Rules for Filter 2 Type exist for selected part. ").AddText(ex1.Message).PrependMessageType().Log();
                FilterType_comboBox2.SelectedValue = 1;
            }

            catch (ArgumentNullException)
            {
                //PartRules.NewMessage().AddText("No Fixture Rules for selected part. ").AddText(ex2.Message).PrependMessageType().Log();
                //FilterType_comboBox2.SelectedValue = 1;
            }

            if (FilterType_comboBox2.Text == "NONE")
            {
                Filter2_Visibility();
                Load_ParameterSelect2();
                Load_DependableQuantityORRenamingExpression2();
                Load_Quantity2();
            }

            else if (FilterType_comboBox2.Text == "PACAF")
            {
                Filter2_Visibility();
                Load_ParameterSelect2();
                Load_DependableQuantityORRenamingExpression2();
                Load_Quantity2();
            }

            else if (FilterType_comboBox2.Text == "QUANTITY")
            {
                Filter2_Visibility();
                Load_ParameterSelect2();
                Load_DependableQuantityORRenamingExpression2();
                Load_Quantity2();

            }

            else if (FilterType_comboBox2.Text == "JOINER QTY")
            {
                Filter2_Visibility();
                Load_ParameterSelect2();
                Load_DependableQuantityORRenamingExpression2();
                Load_Quantity2();
            }

            else if (FilterType_comboBox2.Text == "JOINER")
            {
                Filter2_Visibility();
                Load_ParameterSelect2();
                Load_DependableQuantityORRenamingExpression2();
                Load_Quantity2();
            }

            else if (FilterType_comboBox2.Text == "ENDCAP QTY")
            {
                Filter2_Visibility();
                Load_ParameterSelect2();
                Load_DependableQuantityORRenamingExpression2();
                Load_Quantity2();
            }

            else if (FilterType_comboBox2.Text == "DEPENDABLE QTY")//&& styles.Count == 3)
            {
                Filter2_Visibility();
                Load_ParameterSelect2();
                Load_DependableQuantityORRenamingExpression2();
                Load_Quantity2();

            }

            else if (FilterType_comboBox1.Text == "RENAMINGEXPRESSION")
            {
                Filter2_Visibility();
                Load_ParameterSelect2();
                Load_DependableQuantityORRenamingExpression2();
                Load_Quantity2();

            }

        }

        private void Load_FilterType3FromDB()
        {
            ParameterPR_ListView3.SelectedItems.Clear();

            try
            {                

                FilterType_comboBox3.SelectedValue = db.PartRulesFilters.Where(o => (o.PartID == part_Index && o.AssemblyID == assembly_Index) && o.ProductID == Filter_ProductIdIndex && o.OrderOfExecution == 3).First().FilterTypeID;

            }

            catch (NullReferenceException)
            {
                //PartRules.NewMessage().AddText(PartName + ": No PACAF Fixture Rules for Filter 3 Type exist. ").AddText(ex.Message).IsError().PrependMessageType().Log();
                //FilterType_comboBox3.SelectedValue = 1;
            }

            catch (InvalidOperationException ex1)
            {
                PartRules.NewMessage().AddText("No Rules for Filter 3 Type exist for selected part. ").AddText(ex1.Message).PrependMessageType().Log();
                FilterType_comboBox3.SelectedValue = 1;

            }

            catch (ArgumentNullException)
            {
                //PartRules.NewMessage().AddText("No Fixture Rules for selected part. ").AddText(ex2.Message).PrependMessageType().Log();
                //FilterType_comboBox3.SelectedValue = 1;
            }

            if (FilterType_comboBox3.Text == "NONE")
            {
                Filter3_Visibility();
                Load_ParameterSelect3();
                Load_DependableQuantityORRenamingExpression3();
                Load_Quantity3();
            }

            else if (FilterType_comboBox3.Text == "PACAF")
            {
                Filter3_Visibility();
                Load_ParameterSelect3();
                Load_DependableQuantityORRenamingExpression3();
                Load_Quantity3();
            }

            else if (FilterType_comboBox3.Text == "QUANTITY")
            {
                Filter3_Visibility();
                Load_ParameterSelect3();
                Load_DependableQuantityORRenamingExpression3();
                Load_Quantity3();

            }

            else if (FilterType_comboBox3.Text == "JOINER QTY")
            {
                Filter3_Visibility();
                Load_ParameterSelect3();
                Load_DependableQuantityORRenamingExpression3();
                Load_Quantity3();
            }

            else if (FilterType_comboBox3.Text == "JOINER")
            {
                Filter3_Visibility();
                Load_ParameterSelect3();
                Load_DependableQuantityORRenamingExpression3();
                Load_Quantity3();
            }

            else if (FilterType_comboBox3.Text == "ENDCAP QTY")
            {
                Filter3_Visibility();
                Load_ParameterSelect3();
                Load_DependableQuantityORRenamingExpression3();
                Load_Quantity3();
            }

            else if (FilterType_comboBox3.Text == "DEPENDABLE QTY")//&& styles.Count == 3)
            {
                Filter3_Visibility();
                Load_ParameterSelect3();
                Load_DependableQuantityORRenamingExpression3();
                Load_Quantity3();

            }

            else if (FilterType_comboBox1.Text == "RENAMINGEXPRESSION")
            {
                Filter3_Visibility();
                Load_ParameterSelect3();
                Load_DependableQuantityORRenamingExpression3();
                Load_Quantity3();

            }

        }

        private void Load_FilterType4FromDB()
        {
            ParameterPR_ListView4.SelectedItems.Clear();

            try
            {               
                FilterType_comboBox4.SelectedValue = db.PartRulesFilters.Where(o => (o.PartID == part_Index && o.AssemblyID == assembly_Index) && o.ProductID == Filter_ProductIdIndex && o.OrderOfExecution == 4).First().FilterTypeID;

            }

            catch (NullReferenceException)
            {
                //PartRules.NewMessage().AddText(PartName + ": No PACAF Fixture Rules for Filter 4 Type exist. ").AddText(ex.Message).IsError().PrependMessageType().Log();
                //FilterType_comboBox4.SelectedValue = 1;
            }

            catch (InvalidOperationException ex1)
            {
                PartRules.NewMessage().AddText("No Rules for Filter 4 Type exist for selected part. ").AddText(ex1.Message).PrependMessageType().Log();
                FilterType_comboBox4.SelectedValue = 1;
            }

            catch (ArgumentNullException)
            {
                //PartRules.NewMessage().AddText("No Fixture Rules for selected part. ").AddText(ex2.Message).PrependMessageType().Log();
                //FilterType_comboBox4.SelectedValue = 1;
            }

            if (FilterType_comboBox4.Text == "NONE")
            {
                Filter4_Visibility();
                Load_ParameterSelect4();
                Load_DependableQuantityORRenamingExpression4();
                Load_Quantity4();
            }

            else if (FilterType_comboBox4.Text == "PACAF")
            {
                Filter4_Visibility();
                Load_ParameterSelect4();
                Load_DependableQuantityORRenamingExpression4();
                Load_Quantity4();
            }

            else if (FilterType_comboBox4.Text == "QUANTITY")
            {
                Filter4_Visibility();
                Load_ParameterSelect4();
                Load_DependableQuantityORRenamingExpression4();
                Load_Quantity4();
            }

            else if (FilterType_comboBox4.Text == "JOINER QTY")
            {
                Filter4_Visibility();
                Load_ParameterSelect4();
                Load_DependableQuantityORRenamingExpression4();
                Load_Quantity4();
            }

            else if (FilterType_comboBox4.Text == "JOINER")
            {
                Filter4_Visibility();
                Load_ParameterSelect4();
                Load_DependableQuantityORRenamingExpression4();
                Load_Quantity4();
            }

            else if (FilterType_comboBox4.Text == "ENDCAP QTY")
            {
                Filter4_Visibility();
                Load_ParameterSelect4();
                Load_DependableQuantityORRenamingExpression4();
                Load_Quantity4();
            }

            else if (FilterType_comboBox4.Text == "DEPENDABLE QTY")//&& styles.Count == 3)
            {
                Filter4_Visibility();
                Load_ParameterSelect4();
                Load_DependableQuantityORRenamingExpression4();
                Load_Quantity4();

            }

            else if (FilterType_comboBox4.Text == "RENAMINGEXPRESSION")
            {
                Filter4_Visibility();
                Load_ParameterSelect4();
                Load_DependableQuantityORRenamingExpression4();
                Load_Quantity4();

            }

        }



        private void Load_ParameterSelect1()
        {
            if (FilterType_comboBox1.Text == "PACAF")
            {

                CategoryPR_comboBoxFilter1.SelectedValue = db.PartRulesFilters.Where(o => (o.PartID == part_Index && o.AssemblyID == assembly_Index) && o.ProductID == Filter_ProductIdIndex && o.OrderOfExecution == 1).First().CategoryID;

                FilterBehavior_ComboBox1.SelectedValue = db.PartRulesFilters.Where(o => (o.PartID == part_Index && o.AssemblyID == assembly_Index) && o.ProductID == Filter_ProductIdIndex && o.OrderOfExecution == 1).First().FilterBehaviorID;

                //Enter Selected PARAMETER ID OR TEXT
                String parameterString = db.PartRulesFilters.Where(o => (o.PartID == part_Index && o.AssemblyID == assembly_Index) && o.ProductID == Filter_ProductIdIndex && o.OrderOfExecution == 1).First().ParameterName;

                String[] parameters = parameterString.Split('|');

                int count = 0;

                foreach (ListViewItem currentItems in ParameterPR_ListView1.Items)
                {
                    foreach (String items in parameters)
                    {
                        if (currentItems.Text == items)
                        {
                            ParameterPR_ListView1.Items[count].Selected = true;
                        }
                    }
                    count++;

                }
            }

            else if (FilterType_comboBox1.Text != "PACAF")
            {
                CategoryPR_comboBoxFilter1.SelectedValue = -1;
            }
        }

        private void Load_ParameterSelect2()
        {

            if (FilterType_comboBox2.Text == "PACAF")
            {                
                CategoryPR_comboBoxFilter2.SelectedValue = db.PartRulesFilters.Where(o => (o.PartID == part_Index && o.AssemblyID == assembly_Index) && o.ProductID == Filter_ProductIdIndex && o.OrderOfExecution == 2).First().CategoryID;

                FilterBehavior_ComboBox2.SelectedValue = db.PartRulesFilters.Where(o => (o.PartID == part_Index && o.AssemblyID == assembly_Index) && o.ProductID == Filter_ProductIdIndex && o.OrderOfExecution == 2).First().FilterBehaviorID;

                //Enter Selected PARAMETER ID OR TEXT
                String parameterString = db.PartRulesFilters.Where(o => (o.PartID == part_Index && o.AssemblyID == assembly_Index) && o.ProductID == Filter_ProductIdIndex && o.OrderOfExecution == 2).First().ParameterName;

                String[] parameters = parameterString.Split('|');

                int count = 0;

                foreach (ListViewItem currentItems in ParameterPR_ListView2.Items)
                {
                    foreach (String items in parameters)
                    {
                        if (currentItems.Text == items)
                        {
                            ParameterPR_ListView2.Items[count].Selected = true;
                        }
                    }
                    count++;

                }
            }

            else if (FilterType_comboBox2.Text != "PACAF")
            {
                CategoryPR_comboBoxFilter2.SelectedValue = -1;
            }
        }

        private void Load_ParameterSelect3()
        {
            if (FilterType_comboBox3.Text == "PACAF")
            {               
                CategoryPR_comboBoxFilter3.SelectedValue = db.PartRulesFilters.Where(o => (o.PartID == part_Index && o.AssemblyID == assembly_Index) && o.ProductID == Filter_ProductIdIndex && o.OrderOfExecution == 3).First().CategoryID;

                FilterBehavior_ComboBox3.SelectedValue = db.PartRulesFilters.Where(o => (o.PartID == part_Index && o.AssemblyID == assembly_Index) && o.ProductID == Filter_ProductIdIndex && o.OrderOfExecution == 3).First().FilterBehaviorID;

                //Enter Selected PARAMETER ID OR TEXT
                String parameterString = db.PartRulesFilters.Where(o => (o.PartID == part_Index && o.AssemblyID == assembly_Index) && o.ProductID == Filter_ProductIdIndex && o.OrderOfExecution == 3).First().ParameterName;

                String[] parameters = parameterString.Split('|');

                int count = 0;

                foreach (ListViewItem currentItems in ParameterPR_ListView3.Items)
                {
                    foreach (String items in parameters)
                    {
                        if (currentItems.Text == items)
                        {
                            ParameterPR_ListView3.Items[count].Selected = true;
                        }
                    }
                    count++;

                }
            }

            else if (FilterType_comboBox3.Text != "PACAF")
            {
                CategoryPR_comboBoxFilter3.SelectedValue = -1;
            }
        }

        private void Load_ParameterSelect4()
        {
            if (FilterType_comboBox4.Text == "PACAF")
            {
                CategoryPR_comboBoxFilter4.SelectedValue = db.PartRulesFilters.Where(o => (o.PartID == part_Index && o.AssemblyID == assembly_Index) && o.ProductID == Filter_ProductIdIndex && o.OrderOfExecution == 4).First().CategoryID;

                FilterBehavior_ComboBox4.SelectedValue = db.PartRulesFilters.Where(o => (o.PartID == part_Index && o.AssemblyID == assembly_Index) && o.ProductID == Filter_ProductIdIndex && o.OrderOfExecution == 4).First().FilterBehaviorID;

                //Enter Selected PARAMETER ID OR TEXT
                String parameterString = db.PartRulesFilters.Where(o => (o.PartID == part_Index && o.AssemblyID == assembly_Index) && o.ProductID == Filter_ProductIdIndex && o.OrderOfExecution == 4).First().ParameterName;

                String[] parameters = parameterString.Split('|');

                int count = 0;

                foreach (ListViewItem currentItems in ParameterPR_ListView4.Items)
                {
                    foreach (String items in parameters)
                    {
                        if (currentItems.Text == items)
                        {
                            ParameterPR_ListView4.Items[count].Selected = true;
                        }
                    }
                    count++;

                }

            }

            else if (FilterType_comboBox4.Text != "PACAF")
            {
                CategoryPR_comboBoxFilter4.SelectedValue = -1;
            }

        }



        private void Load_Quantity1()
        {

            if (FilterType_comboBox1.Text == "QUANTITY")
            {
                try
                {
                    Qty_NumericUpDown1.Value = db.PartRulesFilters.Where(o => (o.PartID == part_Index && o.AssemblyID == assembly_Index) && o.ProductID == Filter_ProductIdIndex && o.OrderOfExecution == 1).First().Quantity ?? 0;

                }
                catch
                {

                }

            }
            else if (FilterType_comboBox1.Text != "QUANTITY")
            {
                Qty_NumericUpDown1.Value = 0;
            }
        }

        private void Load_Quantity2()
        {

            if (FilterType_comboBox2.Text == "QUANTITY")
            {
                try
                {                   
                    Qty_NumericUpDown2.Value = db.PartRulesFilters.Where(o => (o.PartID == part_Index && o.AssemblyID == assembly_Index)&& o.ProductID == Filter_ProductIdIndex && o.OrderOfExecution == 2).First().Quantity ?? 0;

                }
                catch
                {

                }

            }
            else if (FilterType_comboBox2.Text != "QUANTITY")
            {
                Qty_NumericUpDown2.Value = 0;
            }
        }

        private void Load_Quantity3()
        {

            if (FilterType_comboBox3.Text == "QUANTITY")
            {
                try
                {                   
                    Qty_NumericUpDown3.Value = db.PartRulesFilters.Where(o => (o.PartID == part_Index && o.AssemblyID == assembly_Index) && o.ProductID == Filter_ProductIdIndex && o.OrderOfExecution == 3).First().Quantity ?? 0;

                }
                catch
                {

                }

            }
            else if (FilterType_comboBox3.Text != "QUANTITY")
            {
                Qty_NumericUpDown3.Value = 0;
            }
        }

        private void Load_Quantity4()
        {

            if (FilterType_comboBox4.Text == "QUANTITY")
            {
                try
                {
                    Qty_NumericUpDown4.Value = db.PartRulesFilters.Where(o => (o.PartID == part_Index && o.AssemblyID == assembly_Index) && o.ProductID == Filter_ProductIdIndex && o.OrderOfExecution == 4).First().Quantity ?? 0;

                }
                catch
                {

                }

            }
            else if (FilterType_comboBox4.Text != "QUANTITY")
            {
                Qty_NumericUpDown4.Value = 0;
            }
        }



        private void Load_DependableQuantityORRenamingExpression1()
        {

            if (FilterType_comboBox1.Text == "DEPENDABLE QTY")
            {
                try
                {
                    DependableQtyPR_comboBoxFilter1.SelectedValue = db.PartRulesFilters.Where(o => (o.PartID == part_Index && o.AssemblyID == assembly_Index) && o.ProductID == Filter_ProductIdIndex && o.OrderOfExecution == 1).First().DependableQuantityID;

                }
                catch
                {

                }
            }
            else if (FilterType_comboBox1.Text != "DEPENDABLE QTY")
            {
                DependableQtyPR_comboBoxFilter1.SelectedValue = -1;

            }

            if (FilterType_comboBox1.Text == "RENAMINGEXPRESSION")
            {
                try
                {
                    RE_comboBox1.SelectedValue = db.PartRulesFilters.Where(o => (o.PartID == part_Index && o.AssemblyID == assembly_Index) && o.ProductID == Filter_ProductIdIndex && o.OrderOfExecution == 1).First().RenamingExpressionID;

                }
                catch
                {

                }
            }
            else if (FilterType_comboBox1.Text != "RENAMINGEXPRESSION")
            {
                RE_comboBox1.SelectedValue = -1;

            }

        }

        private void Load_DependableQuantityORRenamingExpression2()
        {
            if (FilterType_comboBox2.Text == "DEPENDABLE QTY")
            {
                try
                {
                    DependableQtyPR_comboBoxFilter2.SelectedValue = db.PartRulesFilters.Where(o => (o.PartID == part_Index && o.AssemblyID == assembly_Index) && o.ProductID == Filter_ProductIdIndex && o.OrderOfExecution == 2).First().DependableQuantityID;

                }
                catch
                {

                }
            }
            else if (FilterType_comboBox2.Text != "DEPENDABLE QTY")
            {
                DependableQtyPR_comboBoxFilter2.SelectedValue = -1;

            }

            if (FilterType_comboBox2.Text == "RENAMINGEXPRESSION")
            {
                try
                {
                    RE_comboBox2.SelectedValue = db.PartRulesFilters.Where(o => (o.PartID == part_Index && o.AssemblyID == assembly_Index) && o.ProductID == Filter_ProductIdIndex && o.OrderOfExecution == 2).First().RenamingExpressionID;

                }
                catch
                {

                }
            }
            else if (FilterType_comboBox2.Text != "RENAMINGEXPRESSION")
            {
                RE_comboBox2.SelectedValue = -1;

            }

        }

        private void Load_DependableQuantityORRenamingExpression3()
        {
            if (FilterType_comboBox3.Text == "DEPENDABLE QTY")
            {
                try
                {
                    DependableQtyPR_comboBoxFilter3.SelectedValue = db.PartRulesFilters.Where(o => (o.PartID == part_Index && o.AssemblyID == assembly_Index) && o.ProductID == Filter_ProductIdIndex && o.OrderOfExecution == 3).First().DependableQuantityID;

                }
                catch
                {

                }
            }
            else if (FilterType_comboBox3.Text != "DEPENDABLE QTY")
            {
                DependableQtyPR_comboBoxFilter3.SelectedValue = -1;

            }

            if (FilterType_comboBox3.Text == "RENAMINGEXPRESSION")
            {
                try
                {
                    RE_comboBox3.SelectedValue = db.PartRulesFilters.Where(o => (o.PartID == part_Index && o.AssemblyID == assembly_Index) && o.ProductID == Filter_ProductIdIndex && o.OrderOfExecution == 3).First().RenamingExpressionID;

                }
                catch
                {

                }
            }
            else if (FilterType_comboBox3.Text != "RENAMINGEXPRESSION")
            {
                RE_comboBox3.SelectedValue = -1;

            }

        }

        private void Load_DependableQuantityORRenamingExpression4()
        {
            if (FilterType_comboBox4.Text == "DEPENDABLE QTY")
            {
                try
                {
                    DependableQtyPR_comboBoxFilter4.SelectedValue = db.PartRulesFilters.Where(o => (o.PartID == part_Index && o.AssemblyID == assembly_Index) && o.ProductID == Filter_ProductIdIndex && o.OrderOfExecution == 4).First().DependableQuantityID;

                }
                catch
                {

                }
            }
            else if (FilterType_comboBox4.Text != "DEPENDABLE QTY")
            {
                DependableQtyPR_comboBoxFilter4.SelectedValue = -1;

            }

            if (FilterType_comboBox4.Text == "RENAMINGEXPRESSION")
            {
                try
                {
                    RE_comboBox4.SelectedValue = db.PartRulesFilters.Where(o => (o.PartID == part_Index && o.AssemblyID == assembly_Index) && o.ProductID == Filter_ProductIdIndex && o.OrderOfExecution == 4).First().RenamingExpressionID;

                }
                catch
                {

                }
            }
            else if (FilterType_comboBox4.Text != "RENAMINGEXPRESSION")
            {
                RE_comboBox4.SelectedValue = -1;

            }

        }


        //DATA GRID VIEW
        private void DataGridView_Rules_SelectionChanged(object sender, EventArgs e)
        {

        }

        private void RefreshDataGridView_PartRules()
        {
            try
            {
                DataGridView_Rules.DataSource = db.PartRulesFilters.Where(o => (o.Part.Name == Fixture_treeView_PartRule.SelectedNode.Text || o.Assembly.Name == Fixture_treeView_PartRule.SelectedNode.Text) && o.ProductCode == productID_comboBox_PR.Text).OrderBy(m => m.OrderOfExecution).ToList();

                DataGridView_Rules.Columns["id"].Visible = false;
                DataGridView_Rules.Columns["PartID"].Visible = true;
                DataGridView_Rules.Columns["AssemblyID"].Visible = true;
                DataGridView_Rules.Columns["ProductID"].Visible = false;
                DataGridView_Rules.Columns["CategoryID"].Visible = false;
                DataGridView_Rules.Columns["ParameterID"].Visible = false;
                DataGridView_Rules.Columns["FilterBehaviorID"].Visible = true;
                DataGridView_Rules.Columns["FilterType"].Visible = false;
                DataGridView_Rules.Columns["FilterBehavior"].Visible = false;
                DataGridView_Rules.Columns["RenamingExpression"].Visible = false;
                DataGridView_Rules.Columns["Part"].Visible = false;
                DataGridView_Rules.Columns["Assembly"].Visible = false;
                DataGridView_Rules.Columns["DependableQuantity"].Visible = false;

                DataGridView_Rules.AutoResizeColumns();
                DataGridView_Rules.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

                eventLog_richTextBox.ScrollToCaret();

            }
            catch (System.Reflection.TargetException ex)
            {
                //PartRules.NewMessage().AddText("No Part Selected in TreeView. ").AddText(ex.Message).IsError().PrependMessageType().Log();
            }

           
            DataGridView_Disable_Button();
        }

        private void DataGridView_Disable_Button()
        {
            if (DataGridView_Rules.Rows.Count == 0)
            {
                Save_PR.Enabled = true;
                Edit_PR.Enabled = false;

            }
            else
            {
                Save_PR.Enabled = false;
                Edit_PR.Enabled = true;

            }
        }



        //TREE VIEW
        private void Fixture_treeView_PartRule_AfterSelect(object sender, TreeViewEventArgs e)
        {
            eventLog_PR_richTextBox.Clear();
            RefreshDataGridView_PartRules();

            if (DataGridView_Rules.Rows.Count == 0)
            {
                Enable_Filter1();
                Enable_Filter2();
                Enable_Filter3();
                Enable_Filter4();

            }
            else
            {
                Disable_Filter1();
                Disable_Filter2();
                Disable_Filter3();
                Disable_Filter4();

            }

            Load_FilterType1FromDB();
            Load_FilterType2FromDB();
            Load_FilterType3FromDB();
            Load_FilterType4FromDB();


        }



        //BUTTON (SAVE, EDIT)
        private void Save_PR_Click(object sender, EventArgs e)
        {
            eventLog_PR_richTextBox.Clear();

            bool FilterExists1 = db.PartRulesFilters.Any(o => (o.Part.Name == selected_PartName && o.Assembly.Name == selected_AssemblyName) && o.OrderOfExecution == 1 && o.ProductCode == productID_comboBox_PR.Text);
            bool FilterExists2 = db.PartRulesFilters.Any(o => (o.Part.Name == selected_PartName && o.Assembly.Name == selected_AssemblyName) && o.OrderOfExecution == 2 && o.ProductCode == productID_comboBox_PR.Text);
            bool FilterExists3 = db.PartRulesFilters.Any(o => (o.Part.Name == selected_PartName && o.Assembly.Name == selected_AssemblyName) && o.OrderOfExecution == 3 && o.ProductCode == productID_comboBox_PR.Text);
            bool FilterExists4 = db.PartRulesFilters.Any(o => (o.Part.Name == selected_PartName && o.Assembly.Name == selected_AssemblyName) && o.OrderOfExecution == 4 && o.ProductCode == productID_comboBox_PR.Text);

            if (FilterType_comboBox1.Text != "NONE")
            {

                String selected_Category = CategoryPR_comboBoxFilter1.Text;
                Int32? selected_CategoryIndex;
                if (CategoryPR_comboBoxFilter1.SelectedValue == null)
                {
                    selected_Category = null;
                    selected_CategoryIndex = null;
                }
                else
                {
                    selected_CategoryIndex = Int32.Parse(CategoryPR_comboBoxFilter1.SelectedValue.ToString());

                }

                String selected_Parameter = String.Join("|", from item in ParameterPR_ListView1.SelectedItems.Cast<ListViewItem>() select item.Text);// + @"|");
                String Selected_ParameterIndex;
                if (selected_Parameter == "")
                {
                    selected_Parameter = null;
                    Selected_ParameterIndex = null;
                }
                else
                {
                    Selected_ParameterIndex = String.Join("|", from item in ParameterPR_ListView1.SelectedItems.Cast<ListViewItem>() select item.ImageIndex);// + @"|");
                }

                Int32 selected_FilterType = Int32.Parse(FilterType_comboBox1.SelectedValue.ToString());
                String selected_PACAF = Selected_ParameterIndex;

                Int32? selected_FilterDependency;
                String selected_FilterDependencyName = FilterBehavior_ComboBox1.Text;
                if (FilterBehavior_ComboBox1.SelectedValue == null)
                {
                    selected_FilterDependency = null;
                    selected_FilterDependencyName = null;
                }
                else
                {
                    selected_FilterDependency = Int32.Parse(FilterBehavior_ComboBox1.SelectedValue.ToString());
                }

                Int32? selected_RenamingExpression;
                if (RE_comboBox1.SelectedValue == null)
                {
                    selected_RenamingExpression = null;
                }
                else
                {
                    selected_RenamingExpression = Int32.Parse(RE_comboBox1.SelectedValue.ToString());
                }

                Int32 orderOfExecution = 1;

                Int32? selected_Quantity;
                if (Qty_NumericUpDown1.Value == 0)
                {
                    selected_Quantity = null;
                }
                else
                {
                    selected_Quantity = Int32.Parse(Qty_NumericUpDown1.Value.ToString());
                }

                Int32? selected_DependableQuantityID;
                if (DependableQtyPR_comboBoxFilter1.SelectedValue == null)
                {
                    selected_DependableQuantityID = null;
                }
                else
                {
                    selected_DependableQuantityID = Int32.Parse(DependableQtyPR_comboBoxFilter1.SelectedValue.ToString());

                }

                //Check if Filter Exists 
                //PartRulesFilter newPartRule;

                if (FilterExists1)
                {
                    newPartRule = db.PartRulesFilters.Where(o => (o.Part.Name == selected_PartName && o.Assembly.Name == selected_AssemblyName) && o.OrderOfExecution == orderOfExecution).First();
                }
                else
                {
                    newPartRule = new PartRulesFilter();
                    db.PartRulesFilters.Add(newPartRule);
                }



                if (FilterType_comboBox1.Text == "PACAF" && selected_Parameter == null)
                {
                    PartRules.NewMessage().AddText("PACAF Filter 1 must have selected Parameters. Part not Added/Edited ").IsError().PrependMessageType().Log();
                }

                else
                {
                    newPartRule.PartID = part_Index;
                    //newPartRule.PartName = selected_PartName;
                    newPartRule.AssemblyID = assembly_Index;
                    //newPartRule.AssemblyName = selected_AssemblyName;
                    newPartRule.ProductID = Filter_ProductIdIndex;
                    newPartRule.ProductCode = Filter_ProductCode;
                    newPartRule.CategoryID = selected_CategoryIndex;
                    newPartRule.CategoryName = selected_Category;
                    newPartRule.ParameterID = Selected_ParameterIndex;
                    newPartRule.ParameterName = selected_Parameter;
                    newPartRule.FilterTypeID = selected_FilterType;
                    newPartRule.OrderOfExecution = orderOfExecution;
                    newPartRule.PACAF_ID = selected_PACAF;
                    newPartRule.FilterBehaviorID = selected_FilterDependency;
                    newPartRule.RenamingExpressionID = selected_RenamingExpression;
                    newPartRule.Quantity = selected_Quantity;
                    newPartRule.DependableQuantityID = selected_DependableQuantityID;

                    try
                    {
                        db.SaveChanges();
                        PartRules.NewMessage().AddText(G_PartName + ": Filter Type 1 Added/Edited to Part Filter Table.").PrependMessageType().Log();


                    }
                    catch
                    {
                        PartRules.NewMessage().AddText(G_PartName + ": Filter Type 1 not Added/Edited to Part Filter Table.").IsError().PrependMessageType().Log();
                    }

                    RefreshDataGridView_PartRules();

                }


            }

            else if (FilterType_comboBox1.Text == "NONE" && FilterExists1)
            {
                newPartRule = db.PartRulesFilters.Where(o => (o.Part.Name == selected_PartName && o.Assembly.Name == selected_AssemblyName) && o.OrderOfExecution == 1 && o.ProductCode == productID_comboBox_PR.Text).First();
                db.PartRulesFilters.Remove(newPartRule);

                try
                {

                    db.SaveChanges();
                    PartRules.NewMessage().AddText(G_PartName + ": Filter Type 1 Removed From DataBase. ").PrependMessageType().Log();

                }
                catch
                {
                    PartRules.NewMessage().AddText(G_PartName + ": Filter Type 1 not Removed From DataBase. ").IsError().PrependMessageType().Log();
                }
                RefreshDataGridView_PartRules();

            }

            if (FilterType_comboBox2.Text != "NONE")
            {

                String selected_Category = CategoryPR_comboBoxFilter2.Text;
                Int32? selected_CategoryIndex;
                if (CategoryPR_comboBoxFilter2.SelectedValue == null)
                {
                    selected_Category = null;
                    selected_CategoryIndex = null;
                }
                else
                {
                    selected_CategoryIndex = Int32.Parse(CategoryPR_comboBoxFilter2.SelectedValue.ToString());

                }

                String selected_Parameter = String.Join("|", from item in ParameterPR_ListView2.SelectedItems.Cast<ListViewItem>() select item.Text);// + @"|");
                String Selected_ParameterIndex;
                if (selected_Parameter == "")
                {
                    selected_Parameter = null;
                    Selected_ParameterIndex = null;
                }
                else
                {
                    Selected_ParameterIndex = String.Join("|", from item in ParameterPR_ListView2.SelectedItems.Cast<ListViewItem>() select item.ImageIndex);// + @"|");
                }

                Int32 selected_FilterType = Int32.Parse(FilterType_comboBox2.SelectedValue.ToString());
                String selected_PACAF = Selected_ParameterIndex;

                Int32? selected_FilterDependency;
                if (FilterBehavior_ComboBox2.SelectedValue == null)
                {
                    selected_FilterDependency = null;
                }
                else
                {
                    selected_FilterDependency = Int32.Parse(FilterBehavior_ComboBox2.SelectedValue.ToString());
                }

                Int32? selected_RenamingExpression;
                if (RE_comboBox2.SelectedValue == null)
                {
                    selected_RenamingExpression = null;
                }
                else
                {
                    selected_RenamingExpression = Int32.Parse(RE_comboBox2.SelectedValue.ToString());
                }

                Int32 orderOfExecution = 2;

                Int32? selected_Quantity;
                if (Qty_NumericUpDown2.Value == 0)
                {
                    selected_Quantity = null;
                }
                else
                {
                    selected_Quantity = Int32.Parse(Qty_NumericUpDown2.Value.ToString());
                }

                Int32? selected_DependableQuantityID;
                if (DependableQtyPR_comboBoxFilter2.SelectedValue == null)
                {
                    selected_DependableQuantityID = null;
                }
                else
                {
                    selected_DependableQuantityID = Int32.Parse(DependableQtyPR_comboBoxFilter2.SelectedValue.ToString());

                }

                //PartRulesFilter newPartRule;

                if (FilterExists2)
                {
                    newPartRule = db.PartRulesFilters.Where(o => (o.Part.Name == selected_PartName && o.Assembly.Name == selected_AssemblyName) && o.OrderOfExecution == orderOfExecution).First();
                }
                else
                {
                    newPartRule = new PartRulesFilter();
                    db.PartRulesFilters.Add(newPartRule);
                }



                if (FilterType_comboBox2.Text == "PACAF" && selected_Parameter == null)
                {
                    PartRules.NewMessage().AddText("PACAF Filter 2 must have selected Parameters.Part not Added / Edited ").IsError().PrependMessageType().Log();
                }

                else
                {
                    newPartRule.PartID = part_Index;
                    //newPartRule.PartName = selected_PartName;
                    newPartRule.AssemblyID = assembly_Index;
                    //newPartRule.AssemblyName = selected_AssemblyName;
                    newPartRule.ProductID = Filter_ProductIdIndex;
                    newPartRule.ProductCode = Filter_ProductCode;
                    newPartRule.CategoryID = selected_CategoryIndex;
                    newPartRule.CategoryName = selected_Category;
                    newPartRule.ParameterID = Selected_ParameterIndex;
                    newPartRule.ParameterName = selected_Parameter;
                    newPartRule.FilterTypeID = selected_FilterType;
                    newPartRule.OrderOfExecution = orderOfExecution;
                    newPartRule.PACAF_ID = selected_PACAF;
                    newPartRule.FilterBehaviorID = selected_FilterDependency;
                    newPartRule.RenamingExpressionID = selected_RenamingExpression;
                    newPartRule.Quantity = selected_Quantity;
                    newPartRule.DependableQuantityID = selected_DependableQuantityID;


                    try
                    {
                        db.SaveChanges();
                        PartRules.NewMessage().AddText(G_PartName + ": Filter Type 2 Added/Edited to Part Filter Table.").PrependMessageType().Log();


                    }
                    catch
                    {
                        PartRules.NewMessage().AddText(G_PartName + ": Filter Type 2 not Added/Edited to Part Filter Table.").IsError().PrependMessageType().Log();
                    }

                    RefreshDataGridView_PartRules();
                }

            }

            else if (FilterType_comboBox2.Text == "NONE" && FilterExists2)
            {
                newPartRule = db.PartRulesFilters.Where(o => (o.Part.Name == selected_PartName && o.Assembly.Name == selected_AssemblyName) && o.OrderOfExecution == 2 && o.ProductCode == productID_comboBox_PR.Text).First();
                db.PartRulesFilters.Remove(newPartRule);

                try
                {

                    db.SaveChanges();
                    PartRules.NewMessage().AddText(G_PartName + ": Filter Type 2 Removed From DataBase. ").PrependMessageType().Log();

                }
                catch
                {
                    PartRules.NewMessage().AddText(G_PartName + ": Filter Type 2 not Removed From DataBase. ").IsError().PrependMessageType().Log();
                }
                RefreshDataGridView_PartRules();

            }

            if (FilterType_comboBox3.Text != "NONE")
            {

                String selected_Category = CategoryPR_comboBoxFilter3.Text;
                Int32? selected_CategoryIndex;
                if (CategoryPR_comboBoxFilter3.SelectedValue == null)
                {
                    selected_Category = null;
                    selected_CategoryIndex = null;
                }
                else
                {
                    selected_CategoryIndex = Int32.Parse(CategoryPR_comboBoxFilter3.SelectedValue.ToString());

                }

                String selected_Parameter = String.Join("|", from item in ParameterPR_ListView3.SelectedItems.Cast<ListViewItem>() select item.Text);// + @"|");
                String Selected_ParameterIndex;
                if (selected_Parameter == "")
                {
                    selected_Parameter = null;
                    Selected_ParameterIndex = null;
                }
                else
                {
                    Selected_ParameterIndex = String.Join("|", from item in ParameterPR_ListView3.SelectedItems.Cast<ListViewItem>() select item.ImageIndex);// + @"|");
                }

                Int32 selected_FilterType = Int32.Parse(FilterType_comboBox3.SelectedValue.ToString());
                String selected_PACAF = Selected_ParameterIndex;

                Int32? selected_FilterDependency;
                if (FilterBehavior_ComboBox3.SelectedValue == null)
                {
                    selected_FilterDependency = null;
                }
                else
                {
                    selected_FilterDependency = Int32.Parse(FilterBehavior_ComboBox3.SelectedValue.ToString());
                }

                Int32? selected_RenamingExpression;
                if (RE_comboBox3.SelectedValue == null)
                {
                    selected_RenamingExpression = null;
                }
                else
                {
                    selected_RenamingExpression = Int32.Parse(RE_comboBox3.SelectedValue.ToString());
                }

                Int32 orderOfExecution = 3;

                Int32? selected_Quantity;
                if (Qty_NumericUpDown3.Value == 0)
                {
                    selected_Quantity = null;
                }
                else
                {
                    selected_Quantity = Int32.Parse(Qty_NumericUpDown3.Value.ToString());
                }

                Int32? selected_DependableQuantityID;
                if (DependableQtyPR_comboBoxFilter3.SelectedValue == null)
                {
                    selected_DependableQuantityID = null;
                }
                else
                {
                    selected_DependableQuantityID = Int32.Parse(DependableQtyPR_comboBoxFilter3.SelectedValue.ToString());

                }

                //PartRulesFilter newPartRule;

                if (FilterExists3)
                {
                    newPartRule = db.PartRulesFilters.Where(p => (p.Part.Name == selected_PartName && p.Assembly.Name == selected_AssemblyName) && p.OrderOfExecution == orderOfExecution).First();
                }
                else
                {
                    newPartRule = new PartRulesFilter();
                    db.PartRulesFilters.Add(newPartRule);
                }



                if (FilterType_comboBox3.Text == "PACAF" && selected_Parameter == null)
                {
                    PartRules.NewMessage().AddText("PACAF Filter 3 must have selected Parameters. Part not Added/Edited ").IsError().PrependMessageType().Log();
                }

                else
                {
                    newPartRule.PartID = part_Index;
                    //newPartRule.PartName = selected_PartName;
                    newPartRule.AssemblyID = assembly_Index;
                    //newPartRule.AssemblyName = selected_AssemblyName;
                    newPartRule.ProductID = Filter_ProductIdIndex;
                    newPartRule.ProductCode = Filter_ProductCode;
                    newPartRule.CategoryID = selected_CategoryIndex;
                    newPartRule.CategoryName = selected_Category;
                    newPartRule.ParameterID = Selected_ParameterIndex;
                    newPartRule.ParameterName = selected_Parameter;
                    newPartRule.FilterTypeID = selected_FilterType;
                    newPartRule.OrderOfExecution = orderOfExecution;
                    newPartRule.PACAF_ID = selected_PACAF;
                    newPartRule.FilterBehaviorID = selected_FilterDependency;
                    newPartRule.RenamingExpressionID = selected_RenamingExpression;
                    newPartRule.Quantity = selected_Quantity;
                    newPartRule.DependableQuantityID = selected_DependableQuantityID;


                    try
                    {
                        db.SaveChanges();
                        PartRules.NewMessage().AddText(G_PartName + ": Filter Type 3 Added/Edited to Part Filter Table.").PrependMessageType().Log();

                    }
                    catch
                    {
                        PartRules.NewMessage().AddText(G_PartName + ": Filter Type 3 not Added/Edited to Part Filter Table.").IsError().PrependMessageType().Log();
                    }

                    RefreshDataGridView_PartRules();
                }

            }

            else if (FilterType_comboBox3.Text == "NONE" && FilterExists3)
            {
                newPartRule = db.PartRulesFilters.Where(o => (o.Part.Name == selected_PartName && o.Assembly.Name == selected_AssemblyName) && o.OrderOfExecution == 3 && o.ProductCode == productID_comboBox_PR.Text).First();
                db.PartRulesFilters.Remove(newPartRule);

                try
                {

                    db.SaveChanges();
                    PartRules.NewMessage().AddText(G_PartName + ": Filter Type 3 Removed From DataBase. ").PrependMessageType().Log();

                }
                catch
                {
                    PartRules.NewMessage().AddText(G_PartName + ": Filter Type 3 not Removed From DataBase. ").IsError().PrependMessageType().Log();
                }
                RefreshDataGridView_PartRules();

            }

            if (FilterType_comboBox4.Text != "NONE")
            {

                String selected_Category = CategoryPR_comboBoxFilter4.Text;
                Int32? selected_CategoryIndex;
                if (CategoryPR_comboBoxFilter4.SelectedValue == null)
                {
                    selected_Category = null;
                    selected_CategoryIndex = null;
                }
                else
                {
                    selected_CategoryIndex = Int32.Parse(CategoryPR_comboBoxFilter4.SelectedValue.ToString());

                }

                String selected_Parameter = String.Join("|", from item in ParameterPR_ListView4.SelectedItems.Cast<ListViewItem>() select item.Text);// + @"|");
                String Selected_ParameterIndex;
                if (selected_Parameter == "")
                {
                    selected_Parameter = null;
                    Selected_ParameterIndex = null;
                }
                else
                {
                    Selected_ParameterIndex = String.Join("|", from item in ParameterPR_ListView4.SelectedItems.Cast<ListViewItem>() select item.ImageIndex);// + @"|");
                }

                Int32 selected_FilterType = Int32.Parse(FilterType_comboBox4.SelectedValue.ToString());
                String selected_PACAF = Selected_ParameterIndex;

                Int32? selected_FilterDependency;
                if (FilterBehavior_ComboBox4.SelectedValue == null)
                {
                    selected_FilterDependency = null;
                }
                else
                {
                    selected_FilterDependency = Int32.Parse(FilterBehavior_ComboBox4.SelectedValue.ToString());
                }

                Int32? selected_RenamingExpression;
                if (RE_comboBox4.SelectedValue == null)
                {
                    selected_RenamingExpression = null;
                }
                else
                {
                    selected_RenamingExpression = Int32.Parse(RE_comboBox4.SelectedValue.ToString());
                }

                Int32 orderOfExecution = 4;

                Int32? selected_Quantity;
                if (Qty_NumericUpDown4.Value == 0)
                {
                    selected_Quantity = null;
                }
                else
                {
                    selected_Quantity = Int32.Parse(Qty_NumericUpDown4.Value.ToString());
                }


                Int32? selected_DependableQuantityID;
                if (DependableQtyPR_comboBoxFilter4.SelectedValue == null)
                {
                    selected_DependableQuantityID = null;
                }
                else
                {
                    selected_DependableQuantityID = Int32.Parse(DependableQtyPR_comboBoxFilter4.SelectedValue.ToString());

                }

                //PartRulesFilter newPartRule;

                if (FilterExists4)
                {
                    newPartRule = db.PartRulesFilters.Where(p => (p.Part.Name == selected_PartName && p.Assembly.Name == selected_AssemblyName) && p.OrderOfExecution == orderOfExecution).First();
                }
                else
                {
                    newPartRule = new PartRulesFilter();
                    db.PartRulesFilters.Add(newPartRule);
                }



                if (FilterType_comboBox4.Text == "PACAF" && selected_Parameter == null)
                {
                    PartRules.NewMessage().AddText("PACAF Filter 4 must have selected Parameters. Part not Added/Edited ").IsError().PrependMessageType().Log();
                }

                else
                {
                    newPartRule.PartID = part_Index;
                    //newPartRule.PartName = selected_PartName;
                    newPartRule.AssemblyID = assembly_Index;
                    //newPartRule.AssemblyName = selected_AssemblyName;
                    newPartRule.ProductID = Filter_ProductIdIndex;
                    newPartRule.ProductCode = Filter_ProductCode;
                    newPartRule.CategoryID = selected_CategoryIndex;
                    newPartRule.CategoryName = selected_Category;
                    newPartRule.ParameterID = Selected_ParameterIndex;
                    newPartRule.ParameterName = selected_Parameter;
                    newPartRule.FilterTypeID = selected_FilterType;
                    newPartRule.OrderOfExecution = orderOfExecution;
                    newPartRule.PACAF_ID = selected_PACAF;
                    newPartRule.FilterBehaviorID = selected_FilterDependency;
                    newPartRule.RenamingExpressionID = selected_RenamingExpression;
                    newPartRule.Quantity = selected_Quantity;
                    newPartRule.DependableQuantityID = selected_DependableQuantityID;


                    try
                    {
                        db.SaveChanges();
                        PartRules.NewMessage().AddText(G_PartName + ": Filter Type 4 Added/Edited to Part Filter Table.").PrependMessageType().Log();

                    }
                    catch
                    {
                        PartRules.NewMessage().AddText(G_PartName + ": Filter Type 4 not Added/Edited to Part Filter Table.").IsError().PrependMessageType().Log();
                    }

                    RefreshDataGridView_PartRules();
                }


            }

            else if (FilterType_comboBox4.Text == "NONE" && FilterExists4)
            {
                newPartRule = db.PartRulesFilters.Where(o => (o.Part.Name == selected_PartName && o.Assembly.Name == selected_AssemblyName) && o.OrderOfExecution == 4 && o.ProductCode == productID_comboBox_PR.Text).First();
                db.PartRulesFilters.Remove(newPartRule);

                try
                {

                    db.SaveChanges();
                    PartRules.NewMessage().AddText(G_PartName + ": Filter Type 4 Removed From DataBase. ").PrependMessageType().Log();

                }
                catch
                {
                    PartRules.NewMessage().AddText(G_PartName + ": Filter Type 4 not Removed From DataBase. ").IsError().PrependMessageType().Log();
                }
                RefreshDataGridView_PartRules();

            }


            Edit_PR.Enabled = true;
            Save_PR.Enabled = false;
            Disable_Filter1();
            Disable_Filter2();
            Disable_Filter3();
            Disable_Filter4();

        }

        private bool editButtonClicked = false;

        private void Edit_PR_Click(object sender, EventArgs e)
        {
            editButtonClicked = true;
            Save_PR.Enabled = true;
            Edit_PR.Enabled = false;
            Enable_Filter1();
            Enable_Filter2();
            Enable_Filter3();
            Enable_Filter4();


        }



        //ENABLE OR DISABLE Filter
        private void Disable_Filter1()
        {
            FilterType_comboBox1.Enabled = false;
            CategoryPR_comboBoxFilter1.Enabled = false;
            ParameterPR_ListView1.Enabled = false;
            FilterBehavior_ComboBox1.Enabled = false;
            Qty_NumericUpDown1.Enabled = false;
            DependableQtyPR_comboBoxFilter1.Enabled = false;
            RE_comboBox1.Enabled = false;
        }

        private void Disable_Filter2()
        {
            FilterType_comboBox2.Enabled = false;
            CategoryPR_comboBoxFilter2.Enabled = false;
            ParameterPR_ListView2.Enabled = false;
            FilterBehavior_ComboBox2.Enabled = false;
            Qty_NumericUpDown2.Enabled = false;
            DependableQtyPR_comboBoxFilter2.Enabled = false;
            RE_comboBox2.Enabled = false;
        }

        private void Disable_Filter3()
        {
            FilterType_comboBox3.Enabled = false;
            CategoryPR_comboBoxFilter3.Enabled = false;
            ParameterPR_ListView3.Enabled = false;
            FilterBehavior_ComboBox3.Enabled = false;
            Qty_NumericUpDown3.Enabled = false;
            DependableQtyPR_comboBoxFilter3.Enabled = false;
            RE_comboBox3.Enabled = false;
        }

        private void Disable_Filter4()
        {
            FilterType_comboBox4.Enabled = false;
            CategoryPR_comboBoxFilter4.Enabled = false;
            ParameterPR_ListView4.Enabled = false;
            FilterBehavior_ComboBox4.Enabled = false;
            Qty_NumericUpDown4.Enabled = false;
            DependableQtyPR_comboBoxFilter4.Enabled = false;
            RE_comboBox4.Enabled = false;
        }

        private void Enable_Filter1()
        {
            FilterType_comboBox1.Enabled = true;
            CategoryPR_comboBoxFilter1.Enabled = true;
            ParameterPR_ListView1.Enabled = true;
            FilterBehavior_ComboBox1.Enabled = true;
            Qty_NumericUpDown1.Enabled = true;
            DependableQtyPR_comboBoxFilter1.Enabled = true;
            RE_comboBox1.Enabled = true;
        }

        private void Enable_Filter2()
        {
            FilterType_comboBox2.Enabled = true;
            CategoryPR_comboBoxFilter2.Enabled = true;
            ParameterPR_ListView2.Enabled = true;
            FilterBehavior_ComboBox2.Enabled = true;
            Qty_NumericUpDown2.Enabled = true;
            DependableQtyPR_comboBoxFilter2.Enabled = true;
            RE_comboBox2.Enabled = true;
        }

        private void Enable_Filter3()
        {
            FilterType_comboBox3.Enabled = true;
            CategoryPR_comboBoxFilter3.Enabled = true;
            ParameterPR_ListView3.Enabled = true;
            FilterBehavior_ComboBox3.Enabled = true;
            Qty_NumericUpDown3.Enabled = true;
            DependableQtyPR_comboBoxFilter3.Enabled = true;
            RE_comboBox3.Enabled = true;
        }

        private void Enable_Filter4()
        {
            FilterType_comboBox4.Enabled = true;
            CategoryPR_comboBoxFilter4.Enabled = true;
            ParameterPR_ListView4.Enabled = true;
            FilterBehavior_ComboBox4.Enabled = true;
            Qty_NumericUpDown4.Enabled = true;
            DependableQtyPR_comboBoxFilter4.Enabled = true;
            RE_comboBox4.Enabled = true;
        }



        //Rules Interface Visibility
        private void Filter1_Visibility()
        {
            if (FilterType_comboBox1.Text == "NONE")
            {

                FILTER1_TableLayoutPanel.Visible = false;
                PACAF_TableLayOut1.Visible = false;
                Qty_TableLayOut1.Visible = false;
                EX_TableLayOut1.Visible = false;
                RE_TableLayOut1.Visible = false;
            }

            else if (FilterType_comboBox1.Text == "PACAF")
            {
                FILTER1_TableLayoutPanel.RowStyles[0].Height = 256;
                FILTER1_TableLayoutPanel.RowStyles[1].Height = 0;
                FILTER1_TableLayoutPanel.RowStyles[2].Height = 0;
                FILTER1_TableLayoutPanel.RowStyles[3].Height = 0;

                FILTER1_TableLayoutPanel.Visible = true;
                PACAF_TableLayOut1.Visible = true;
                Qty_TableLayOut1.Visible = false;
                EX_TableLayOut1.Visible = false;
                RE_TableLayOut1.Visible = false;
            }

            else if (FilterType_comboBox1.Text == "QUANTITY")
            {
                FILTER1_TableLayoutPanel.RowStyles[0].Height = 0;
                FILTER1_TableLayoutPanel.RowStyles[1].Height = 72;
                FILTER1_TableLayoutPanel.RowStyles[2].Height = 0;
                FILTER1_TableLayoutPanel.RowStyles[3].Height = 0;

                FILTER1_TableLayoutPanel.Visible = true;
                PACAF_TableLayOut1.Visible = false;
                Qty_TableLayOut1.Visible = true;
                EX_TableLayOut1.Visible = false;
                RE_TableLayOut1.Visible = false;

            }

            else if (FilterType_comboBox1.Text == "JOINER QTY")
            {
                FILTER1_TableLayoutPanel.RowStyles[0].Height = 0;
                FILTER1_TableLayoutPanel.RowStyles[1].Height = 0;
                FILTER1_TableLayoutPanel.RowStyles[2].Height = 0;
                FILTER1_TableLayoutPanel.RowStyles[3].Height = 0;

                FILTER1_TableLayoutPanel.Visible = true;
                PACAF_TableLayOut1.Visible = false;
                Qty_TableLayOut1.Visible = false;
                EX_TableLayOut1.Visible = false;
                RE_TableLayOut1.Visible = false;
            }

            else if (FilterType_comboBox1.Text == "JOINER")
            {
                FILTER1_TableLayoutPanel.RowStyles[0].Height = 0;
                FILTER1_TableLayoutPanel.RowStyles[1].Height = 0;
                FILTER1_TableLayoutPanel.RowStyles[2].Height = 0;
                FILTER1_TableLayoutPanel.RowStyles[3].Height = 0;

                FILTER1_TableLayoutPanel.Visible = true;
                PACAF_TableLayOut1.Visible = false;
                Qty_TableLayOut1.Visible = false;
                EX_TableLayOut1.Visible = false;
                RE_TableLayOut1.Visible = false;
            }

            else if (FilterType_comboBox1.Text == "ENDCAP QTY")
            {
                FILTER1_TableLayoutPanel.RowStyles[0].Height = 0;
                FILTER1_TableLayoutPanel.RowStyles[1].Height = 0;
                FILTER1_TableLayoutPanel.RowStyles[2].Height = 0;
                FILTER1_TableLayoutPanel.RowStyles[3].Height = 0;

                FILTER1_TableLayoutPanel.Visible = true;
                PACAF_TableLayOut1.Visible = false;
                Qty_TableLayOut1.Visible = false;
                EX_TableLayOut1.Visible = false;
                RE_TableLayOut1.Visible = false;
            }

            else if (FilterType_comboBox1.Text == "DEPENDABLE QTY")
            {
                FILTER1_TableLayoutPanel.RowStyles[0].Height = 0;
                FILTER1_TableLayoutPanel.RowStyles[1].Height = 0;
                FILTER1_TableLayoutPanel.RowStyles[2].Height = 41;
                FILTER1_TableLayoutPanel.RowStyles[3].Height = 0;

                FILTER1_TableLayoutPanel.Visible = true;
                PACAF_TableLayOut1.Visible = false;
                Qty_TableLayOut1.Visible = false;
                EX_TableLayOut1.Visible = true;
                RE_TableLayOut1.Visible = false;
            }

            else if (FilterType_comboBox1.Text == "RENAMINGEXPRESSION")
            {
                FILTER1_TableLayoutPanel.RowStyles[0].Height = 0;
                FILTER1_TableLayoutPanel.RowStyles[1].Height = 0;
                FILTER1_TableLayoutPanel.RowStyles[2].Height = 0;
                FILTER1_TableLayoutPanel.RowStyles[3].Height = 41;

                FILTER1_TableLayoutPanel.Visible = true;
                PACAF_TableLayOut1.Visible = false;
                Qty_TableLayOut1.Visible = false;
                EX_TableLayOut1.Visible = false;
                RE_TableLayOut1.Visible = true;
            }

        }

        private void Filter2_Visibility()
        {
            if (FilterType_comboBox2.Text == "NONE")
            {
                FILTER2_TableLayoutPanel.Visible = false;
                PACAF_TableLayOut2.Visible = false;
                Qty_TableLayOut2.Visible = false;
                EX_TableLayOut2.Visible = false;
                RE_TableLayOut2.Visible = false;

            }

            else if (FilterType_comboBox2.Text == "PACAF")
            {
                FILTER2_TableLayoutPanel.RowStyles[0].Height = 256;
                FILTER2_TableLayoutPanel.RowStyles[1].Height = 0;
                FILTER2_TableLayoutPanel.RowStyles[2].Height = 0;
                FILTER2_TableLayoutPanel.RowStyles[3].Height = 0;

                FILTER2_TableLayoutPanel.Visible = true;
                PACAF_TableLayOut2.Visible = true;
                Qty_TableLayOut2.Visible = false;
                EX_TableLayOut2.Visible = false;
                RE_TableLayOut2.Visible = false;

            }

            else if (FilterType_comboBox2.Text == "QUANTITY")
            {
                FILTER2_TableLayoutPanel.RowStyles[0].Height = 0;
                FILTER2_TableLayoutPanel.RowStyles[1].Height = 72;
                FILTER2_TableLayoutPanel.RowStyles[2].Height = 0;
                FILTER2_TableLayoutPanel.RowStyles[3].Height = 0;

                FILTER2_TableLayoutPanel.Visible = true;
                PACAF_TableLayOut2.Visible = false;
                Qty_TableLayOut2.Visible = true;
                EX_TableLayOut2.Visible = false;
                RE_TableLayOut2.Visible = false;

            }

            else if (FilterType_comboBox2.Text == "JOINER QTY")
            {
                FILTER2_TableLayoutPanel.RowStyles[0].Height = 0;
                FILTER2_TableLayoutPanel.RowStyles[1].Height = 0;
                FILTER2_TableLayoutPanel.RowStyles[2].Height = 0;
                FILTER2_TableLayoutPanel.RowStyles[3].Height = 0;

                FILTER2_TableLayoutPanel.Visible = true;
                PACAF_TableLayOut2.Visible = false;
                Qty_TableLayOut2.Visible = false;
                EX_TableLayOut2.Visible = false;
                RE_TableLayOut2.Visible = false;

            }

            else if (FilterType_comboBox2.Text == "JOINER")
            {
                FILTER2_TableLayoutPanel.RowStyles[0].Height = 0;
                FILTER2_TableLayoutPanel.RowStyles[1].Height = 0;
                FILTER2_TableLayoutPanel.RowStyles[2].Height = 0;
                FILTER2_TableLayoutPanel.RowStyles[3].Height = 0;

                FILTER2_TableLayoutPanel.Visible = true;
                PACAF_TableLayOut2.Visible = false;
                Qty_TableLayOut2.Visible = false;
                EX_TableLayOut2.Visible = false;
                RE_TableLayOut2.Visible = false;

            }

            else if (FilterType_comboBox2.Text == "ENDCAP QTY")
            {
                FILTER2_TableLayoutPanel.RowStyles[0].Height = 0;
                FILTER2_TableLayoutPanel.RowStyles[1].Height = 0;
                FILTER2_TableLayoutPanel.RowStyles[2].Height = 0;
                FILTER2_TableLayoutPanel.RowStyles[3].Height = 0;

                FILTER2_TableLayoutPanel.Visible = true;
                PACAF_TableLayOut2.Visible = false;
                Qty_TableLayOut2.Visible = false;
                EX_TableLayOut2.Visible = false;
                RE_TableLayOut2.Visible = false;

            }

            else if (FilterType_comboBox2.Text == "DEPENDABLE QTY")
            {
                FILTER2_TableLayoutPanel.RowStyles[0].Height = 0;
                FILTER2_TableLayoutPanel.RowStyles[1].Height = 0;
                FILTER2_TableLayoutPanel.RowStyles[2].Height = 41;
                FILTER2_TableLayoutPanel.RowStyles[3].Height = 0;

                FILTER2_TableLayoutPanel.Visible = true;
                PACAF_TableLayOut2.Visible = false;
                Qty_TableLayOut2.Visible = false;
                EX_TableLayOut2.Visible = true;
                RE_TableLayOut2.Visible = false;

            }

            else if (FilterType_comboBox2.Text == "RENAMINGEXPRESSION")
            {
                FILTER2_TableLayoutPanel.RowStyles[0].Height = 0;
                FILTER2_TableLayoutPanel.RowStyles[1].Height = 0;
                FILTER2_TableLayoutPanel.RowStyles[2].Height = 0;
                FILTER2_TableLayoutPanel.RowStyles[3].Height = 41;

                FILTER2_TableLayoutPanel.Visible = true;
                PACAF_TableLayOut2.Visible = false;
                Qty_TableLayOut2.Visible = false;
                EX_TableLayOut2.Visible = false;
                RE_TableLayOut2.Visible = true;
            }
        }

        private void Filter3_Visibility()
        {
            if (FilterType_comboBox3.Text == "NONE")
            {
                FILTER3_TableLayoutPanel.Visible = false;
                PACAF_TableLayOut3.Visible = false;
                Qty_TableLayOut3.Visible = false;
                EX_TableLayOut3.Visible = false;
                RE_TableLayOut3.Visible = false;

            }

            else if (FilterType_comboBox3.Text == "PACAF")
            {
                FILTER3_TableLayoutPanel.RowStyles[0].Height = 256;
                FILTER3_TableLayoutPanel.RowStyles[1].Height = 0;
                FILTER3_TableLayoutPanel.RowStyles[2].Height = 0;
                FILTER3_TableLayoutPanel.RowStyles[3].Height = 0;

                FILTER3_TableLayoutPanel.Visible = true;
                PACAF_TableLayOut3.Visible = true;
                Qty_TableLayOut3.Visible = false;
                EX_TableLayOut3.Visible = false;
                RE_TableLayOut3.Visible = false;

            }

            else if (FilterType_comboBox3.Text == "QUANTITY")
            {
                FILTER3_TableLayoutPanel.RowStyles[0].Height = 0;
                FILTER3_TableLayoutPanel.RowStyles[1].Height = 72;
                FILTER3_TableLayoutPanel.RowStyles[2].Height = 0;
                FILTER3_TableLayoutPanel.RowStyles[3].Height = 0;

                FILTER3_TableLayoutPanel.Visible = true;
                PACAF_TableLayOut3.Visible = false;
                Qty_TableLayOut3.Visible = true;
                EX_TableLayOut3.Visible = false;
                RE_TableLayOut3.Visible = false;

            }

            else if (FilterType_comboBox3.Text == "JOINER QTY")
            {
                FILTER3_TableLayoutPanel.RowStyles[0].Height = 0;
                FILTER3_TableLayoutPanel.RowStyles[1].Height = 0;
                FILTER3_TableLayoutPanel.RowStyles[2].Height = 0;
                FILTER3_TableLayoutPanel.RowStyles[3].Height = 0;

                FILTER3_TableLayoutPanel.Visible = true;
                PACAF_TableLayOut3.Visible = false;
                Qty_TableLayOut3.Visible = false;
                EX_TableLayOut3.Visible = false;
                RE_TableLayOut3.Visible = false;

            }

            else if (FilterType_comboBox3.Text == "JOINER")
            {
                FILTER3_TableLayoutPanel.RowStyles[0].Height = 0;
                FILTER3_TableLayoutPanel.RowStyles[1].Height = 0;
                FILTER3_TableLayoutPanel.RowStyles[2].Height = 0;
                FILTER3_TableLayoutPanel.RowStyles[3].Height = 0;

                FILTER3_TableLayoutPanel.Visible = true;
                PACAF_TableLayOut3.Visible = false;
                Qty_TableLayOut3.Visible = false;
                EX_TableLayOut3.Visible = false;
                RE_TableLayOut3.Visible = false;

            }

            else if (FilterType_comboBox3.Text == "ENDCAP QTY")
            {
                FILTER3_TableLayoutPanel.RowStyles[0].Height = 0;
                FILTER3_TableLayoutPanel.RowStyles[1].Height = 0;
                FILTER3_TableLayoutPanel.RowStyles[2].Height = 0;
                FILTER3_TableLayoutPanel.RowStyles[3].Height = 0;

                FILTER3_TableLayoutPanel.Visible = true;
                PACAF_TableLayOut3.Visible = false;
                Qty_TableLayOut3.Visible = false;
                EX_TableLayOut3.Visible = false;
                RE_TableLayOut3.Visible = false;

            }

            else if (FilterType_comboBox3.Text == "DEPENDABLE QTY")
            {
                FILTER3_TableLayoutPanel.RowStyles[0].Height = 0;
                FILTER3_TableLayoutPanel.RowStyles[1].Height = 0;
                FILTER3_TableLayoutPanel.RowStyles[2].Height = 41;
                FILTER3_TableLayoutPanel.RowStyles[3].Height = 0;

                FILTER3_TableLayoutPanel.Visible = true;
                PACAF_TableLayOut3.Visible = false;
                Qty_TableLayOut3.Visible = false;
                EX_TableLayOut3.Visible = true;
                RE_TableLayOut3.Visible = false;

            }

            else if (FilterType_comboBox3.Text == "RENAMINGEXPRESSION")
            {
                FILTER3_TableLayoutPanel.RowStyles[0].Height = 0;
                FILTER3_TableLayoutPanel.RowStyles[1].Height = 0;
                FILTER3_TableLayoutPanel.RowStyles[2].Height = 0;
                FILTER3_TableLayoutPanel.RowStyles[3].Height = 41;

                FILTER3_TableLayoutPanel.Visible = true;
                PACAF_TableLayOut3.Visible = false;
                Qty_TableLayOut3.Visible = false;
                EX_TableLayOut3.Visible = false;
                RE_TableLayOut3.Visible = true;
            }
        }

        private void Filter4_Visibility()
        {
            if (FilterType_comboBox4.Text == "NONE")
            {
                FILTER4_TableLayoutPanel.Visible = false;
                PACAF_TableLayOut4.Visible = false;
                Qty_TableLayOut4.Visible = false;
                EX_TableLayOut4.Visible = false;
                RE_TableLayOut4.Visible = false;

            }

            else if (FilterType_comboBox4.Text == "PACAF")
            {
                FILTER4_TableLayoutPanel.RowStyles[0].Height = 256;
                FILTER4_TableLayoutPanel.RowStyles[1].Height = 0;
                FILTER4_TableLayoutPanel.RowStyles[2].Height = 0;
                FILTER4_TableLayoutPanel.RowStyles[3].Height = 0;

                FILTER4_TableLayoutPanel.Visible = true;
                PACAF_TableLayOut4.Visible = true;
                Qty_TableLayOut4.Visible = false;
                EX_TableLayOut4.Visible = false;
                RE_TableLayOut4.Visible = false;

            }

            else if (FilterType_comboBox4.Text == "QUANTITY")
            {
                FILTER4_TableLayoutPanel.RowStyles[0].Height = 0;
                FILTER4_TableLayoutPanel.RowStyles[1].Height = 72;
                FILTER4_TableLayoutPanel.RowStyles[2].Height = 0;
                FILTER4_TableLayoutPanel.RowStyles[3].Height = 0;

                FILTER4_TableLayoutPanel.Visible = true;
                PACAF_TableLayOut4.Visible = false;
                Qty_TableLayOut4.Visible = true;
                EX_TableLayOut4.Visible = false;
                RE_TableLayOut4.Visible = false;

            }

            else if (FilterType_comboBox4.Text == "JOINER QTY")
            {
                FILTER4_TableLayoutPanel.RowStyles[0].Height = 0;
                FILTER4_TableLayoutPanel.RowStyles[1].Height = 0;
                FILTER4_TableLayoutPanel.RowStyles[2].Height = 0;
                FILTER4_TableLayoutPanel.RowStyles[3].Height = 0;

                FILTER4_TableLayoutPanel.Visible = true;
                PACAF_TableLayOut4.Visible = false;
                Qty_TableLayOut4.Visible = false;
                EX_TableLayOut4.Visible = false;
                RE_TableLayOut4.Visible = false;

            }

            else if (FilterType_comboBox4.Text == "JOINER")
            {
                FILTER4_TableLayoutPanel.RowStyles[0].Height = 0;
                FILTER4_TableLayoutPanel.RowStyles[1].Height = 0;
                FILTER4_TableLayoutPanel.RowStyles[2].Height = 0;
                FILTER4_TableLayoutPanel.RowStyles[3].Height = 0;

                FILTER4_TableLayoutPanel.Visible = true;
                PACAF_TableLayOut4.Visible = false;
                Qty_TableLayOut4.Visible = false;
                EX_TableLayOut4.Visible = false;
                RE_TableLayOut4.Visible = false;

            }

            else if (FilterType_comboBox4.Text == "ENDCAP QTY")
            {
                FILTER4_TableLayoutPanel.RowStyles[0].Height = 0;
                FILTER4_TableLayoutPanel.RowStyles[1].Height = 0;
                FILTER4_TableLayoutPanel.RowStyles[2].Height = 0;
                FILTER4_TableLayoutPanel.RowStyles[3].Height = 0;

                FILTER4_TableLayoutPanel.Visible = true;
                PACAF_TableLayOut4.Visible = false;
                Qty_TableLayOut4.Visible = false;
                EX_TableLayOut4.Visible = false;
                RE_TableLayOut4.Visible = false;

            }

            else if (FilterType_comboBox4.Text == "DEPENDABLE QTY")//&& styles.Count == 3)
            {
                FILTER4_TableLayoutPanel.RowStyles[0].Height = 0;
                FILTER4_TableLayoutPanel.RowStyles[1].Height = 0;
                FILTER4_TableLayoutPanel.RowStyles[2].Height = 41;
                FILTER4_TableLayoutPanel.RowStyles[3].Height = 0;

                FILTER4_TableLayoutPanel.Visible = true;
                PACAF_TableLayOut4.Visible = false;
                Qty_TableLayOut4.Visible = false;
                EX_TableLayOut4.Visible = true;
                RE_TableLayOut4.Visible = false;

            }

            else if (FilterType_comboBox4.Text == "RENAMINGEXPRESSION")
            {
                FILTER4_TableLayoutPanel.RowStyles[0].Height = 0;
                FILTER4_TableLayoutPanel.RowStyles[1].Height = 0;
                FILTER4_TableLayoutPanel.RowStyles[2].Height = 0;
                FILTER4_TableLayoutPanel.RowStyles[3].Height = 41;

                FILTER4_TableLayoutPanel.Visible = true;
                PACAF_TableLayOut4.Visible = false;
                Qty_TableLayOut4.Visible = false;
                EX_TableLayOut4.Visible = false;
                RE_TableLayOut4.Visible = true;
            }
        }

       

        /****************************************************************************************************************************************************************/
        /*                                                                                                                                                              */
        /*                                                              PART Gen Tester TAB                                                                             */
        /*                                                                                                                                                              */
        /****************************************************************************************************************************************************************/





        public String _FixtureSetupCode
        {
            get
            {
                return FixtureSetupCode_TextBox.Text;
            }
        }

        private void Get_Template_Button_Click(object sender, EventArgs e)
        {
            Log2_RichTextBox.Clear();

            _FixtureConfiguration FixtureConfiguration = new _FixtureConfiguration(FixtureSetupCode_TextBox.Text, FixtureConfigurtorDBConn);

            FixtureConfiguration.CustomerRequest.Template.SummarizeIntoRTB(Template);

            Template.MoveCursorToStart();

        }

        private void Match_Summary_Button_Click(object sender, EventArgs e)
        {
            Log2_RichTextBox.Clear();
            
            _FixtureConfiguration NewFixture = new _FixtureConfiguration(FixtureSetupCode_TextBox.Text, FixtureConfigurtorDBConn);
            

            NewFixture.CustomerRequest.Template.SummarizeMatchesIntoRTB(MatchSummary);

            MatchSummary.MoveCursorToStart();


        }

        private void Solve_Mechanical_Button_Click(object sender, EventArgs e)
        {
            Log2_RichTextBox.Clear();
            _FixtureConfiguration NewFixture = new _FixtureConfiguration(FixtureSetupCode_TextBox.Text, FixtureConfigurtorDBConn);
            
            NewFixture.Sections.SummarizeMechanicalIntoRTB(SolveMechanical);

            SolveMechanical.MoveCursorToStart();

        }

        private void FixtureSetupCode_TextBox_TextChanged(object sender, EventArgs e)
        {

            RefreshTreeView();
        }

        private void GetIndentedBOM_Button_Click(object sender, EventArgs e)
        {
            Log_RichTextBox.Clear();
            Log2_RichTextBox.Clear();
            NewBOM = new _BOM(FixtureSetupCode_TextBox.Text, db);

            ApplicableParts.NewMessage().SetSpaceAfter(0).AddBoldText("Bill Of Material selected Code: ").AddBoldText(_FixtureSetupCode).NewLine().Log();

            NewBOM.SummarizeExistingComponentInToRTB_IN(ApplicableParts);
            NewBOM.SummarizeNonExistingComponentIntoRTB(NonApplicablePartSummary);

            ApplicableParts.MoveCursorToStart();
            NonApplicablePartSummary.MoveCursorToStart();
        }

        private void GetFlatBOM_Button_Click(object sender, EventArgs e)
        {
            Log_RichTextBox.Clear();
            Log2_RichTextBox.Clear();
            NewBOM = new _BOM(FixtureSetupCode_TextBox.Text, db);

            ApplicableParts.NewMessage().SetSpaceAfter(0).AddBoldText("Bill Of Material selected Code: ").AddBoldText(_FixtureSetupCode).NewLine().Log();


            NewBOM.FlatBOMList();
            NewBOM.SummarizeExistingComponentIntoRTB_FB(ApplicableParts);
            NewBOM.SummarizeNonExistingComponentIntoRTB(NonApplicablePartSummary);


            ApplicableParts.MoveCursorToStart();
            NonApplicablePartSummary.MoveCursorToStart();
        }
    }
}

