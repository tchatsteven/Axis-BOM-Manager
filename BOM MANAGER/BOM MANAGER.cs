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
using AXISAutomation.Lookups.FixtureConfiguration;
using AXISAutomation.Tools.Logging;

namespace BOM_MANAGER
{
    public partial class BOM_MANAGER : Form
    {
        /****************************************************************************************************************************************************************/
        /*                                                                                                                                                              */
        /*                                                              PART / ASSEMBLY TAB                                                                             */
        /*                                                                                                                                                              */
        /****************************************************************************************************************************************************************/


        _RTFMessenger BomManagerFormMsg;
        _RTFMessenger PartRules;
        _RTFMessenger PartGen;
        _RTFMessenger PartGen_log2;
        AXIS_AutomationEntitiesBOM db;
        PartRulesFilter newPartRule;
        _BOM NewBOM;


        public BOM_MANAGER()
        {
            InitializeComponent();
            db = new AXIS_AutomationEntitiesBOM();
            BomManagerFormMsg = new _RTFMessenger(eventLog_richTextBox, 0, true) { DefaulSpaceAfter = 0 };
            PartRules = new _RTFMessenger(eventLog_PR_richTextBox, 0, true) { DefaulSpaceAfter = 0 };
            PartGen = new _RTFMessenger(Log_RichTextBox, 0, true) { DefaulSpaceAfter = 0 };
            PartGen_log2 = new _RTFMessenger(Log2_RichTextBox, 0, true) { DefaulSpaceAfter = 0 };
        }

        private void BOM_MANAGER_Load(object sender, EventArgs e)
        {
            GetFixtureId();
            SyncRootAssemblies();
            RefreshDataGridView_Assemblies();
            RefreshDataGridView_Part();

            BomManagerFormMsg.On = true;
            PartRules.On = true;
            PartGen.On = true;
            PartGen_log2.On = true;

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

                Load_Filter_Rules();

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

            foreach (Fixture currentFixture in allFixtures)
            {
                Boolean rootExists = db.AssemblyAtAssemblies.Any(o => o.FixtureID == currentFixture.id && o.ParentID == null);
                if (!rootExists)
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


                dataGridView_Part.DataSource = db.PartTypeAtParts.OrderBy(o => o.PartType).Where(o => o.FamilyName == Combotext).OrderBy(p => p.PartName).ToList();

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
                    db = new AXIS_AutomationEntitiesBOM();
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
                if (NewForm == DialogResult.OK)
                {
                    BomManagerFormMsg.NewMessage().AddText("Part Successfully Edited").PrependMessageType().Log();

                    db = new AXIS_AutomationEntitiesBOM();
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

            if (deletingMsg == DialogResult.Yes)
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
                        if (!myAssociation)
                        {
                            db.Assemblies.Remove(db.Assemblies.Find(currentAssemblyId));
                            string assemblyname = db.Assemblies.Find(currentAssemblyId).Name;
                            db.SaveChanges();

                            RefreshDataGridView_Assemblies();
                            RefreshTreeView();

                            //do not show message if assembly has not been deleted.
                            BomManagerFormMsg.NewMessage().AddText("Assembly : " + assemblyNameToDelete + " has been deleted from TreeView and Database").PrependMessageType().Log();
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
            String deletionPartName = String.Join(", ", dataGridView_Part.SelectedRows.OfType<DataGridViewRow>().Select(o => o.Cells["PartName"].Value.ToString()).ToList());
            String assNoun = dataGridView_Part.SelectedRows.Count > 1 ? "Parts" : "Part";

            DialogResult deletingMsg = MessageBox.Show(String.Format("Do you really want to delete {1} {0}", deletionPartName, assNoun), "Confirm Assembly Deletion", MessageBoxButtons.YesNo);

            if (deletingMsg == DialogResult.Yes)
            {
                try
                {
                    foreach (DataGridViewRow currentDeletionRow in dataGridView_Part.SelectedRows)
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
                    AddAssyToTreeView(currentSelectedRow, CurrentParentId, CurrentAssemblyId, CurrentFixtureCodeId);
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
                    BomManagerFormMsg.NewMessage().AddText("Assembly " + CurrentAssemblyViewName + " has been deleted from Tree view").PrependMessageType().Log();

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

                //Delete part in Rules List

                Int32 currentPartRefID = ((PartView)selectedNode.Tag).PartRefID;
                String currentFixtureID = ((PartView)selectedNode.Tag).Code;

                List<PartRulesFilter> deletionTargetRulesList = db.PartRulesFilters.Where(o => o.ProductCode == currentFixtureID && o.PartID == currentPartRefID).ToList();

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

                //Delete part in Rules List
                Int32 currentPartRefID = ((PartView)selectedNode.Tag).PartRefID;
                String currentFixtureID = ((PartView)selectedNode.Tag).Code;

                List<PartRulesFilter> deletionTargetRulesList = db.PartRulesFilters.Where(o => o.ProductCode == currentFixtureID && o.PartID == currentPartRefID).ToList();

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

        public String MySelectedFixture => db.PartRulesFilters.Find(CurrentPartId).PartName;

        private void RefreshTreeView()
        {
            //Int32 MySelectedFixture;
            String MySelectedFixture;

            if (BOM_tabControl.SelectedTab == tabPage2)
            {
                ////Int32 SCR_selected = 134;
                productID_comboBox.SelectedValue = 134;
                //MySelectedFixture = Int32.Parse(productID_comboBox.SelectedValue.ToString());
                //MySelectedFixture = productID_comboBox.SelectedValue.ToString();
                MySelectedFixture = productID_comboBox.Text;

            }
            else if (BOM_tabControl.SelectedTab == tabPage3)
            {
                productID_comboBox_PR.SelectedValue = 134;
                ////MySelectedFixture = productID_comboBox_PR.Text;
                //MySelectedFixture = Int32.Parse(productID_comboBox_PR.SelectedValue.ToString());
                //MySelectedFixture = productID_comboBox_PR.SelectedValue.ToString();
                MySelectedFixture = productID_comboBox_PR.Text;
            }

            else
            {
                MySelectedFixture = "SCR";
            }

            List<AssemblyView> assy_filteredResult = db.AssemblyViews.Where(o => o.Code == MySelectedFixture).ToList();
            List<PartView> part_filteredResult = db.PartViews.OrderBy(o => o.PartName).Where(p => p.Code == MySelectedFixture).ToList();

            Dictionary<Int32, TreeNode> AssembliesDict = new Dictionary<Int32, TreeNode>();

            foreach (AssemblyView assemblyView in assy_filteredResult)
            {
                Int32 assyIdAtAssemblyView = assemblyView.id;
                String assy_NodeName = assemblyView.Name == "ROOT" ? assemblyView.Code : String.Format("{0}   ({1})", assemblyView.Name, assemblyView.AssemblyType);
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
            Fixture_treeView_PartRule.Nodes.Clear();
            Fixture_treeView_PartGen.Nodes.Clear();

            if (BOM_tabControl.SelectedTab == tabPage2)
            {
                try
                {
                    //Add the tree Node to the TAB PARTS/ASSEMBLY
                    TreeNode Assy_RootNode = AssembliesDict.Where(o => ((AssemblyView)o.Value.Tag).ParentID == null).First().Value;
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
                    //Add the tree Node to the TAB PARTS RULES
                    TreeNode Assy_RootNode_PartRule = AssembliesDict.Where(o => ((AssemblyView)o.Value.Tag).ParentID == null).First().Value;

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
                    ////Add the tree Node to the TAB PARTS RULES

                    TreeNode Assy_RootNode_PartGen = AssembliesDict.Where(o => ((AssemblyView)o.Value.Tag).ParentID == null).First().Value;

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

        private void GetChildTreeNodes(TreeNode Parent_treeNode, List<PartView> part_filteredResult, Int32 currentAssyId)
        {

            foreach (PartView partview in part_filteredResult)
            {
                Int32 assyIdAtPartView = partview.AssRefID;

                if (currentAssyId == assyIdAtPartView)
                {
                    String part_NodeName = String.Format("{0}", partview.PartName);//   ({1})", partview.PartName, partview.PartType );// partview.Description);
                    //String part_NodeName = String.Format("{0} ({1})", partview.PartName, partview.PartType );// partview.Description);

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



        public String G_PartName => Fixture_treeView_PartRule.SelectedNode.Text;//db.PartRulesFilters.Find(CurrentPartId).PartName;

        public Int32 CurrentPartId => db.PartRulesFilters.Find(G_PartName).PartID;//Int32.Parse(DataGridView_Rules.SelectedCells[0].OwningRow.Cells[0].Value.ToString());

        public PartRulesFilter PartRuleToEdit => db.PartRulesFilters.Find(CurrentPartId);

        public String EditPartName => PartRuleToEdit.PartName;

        public String EditProductCode => PartRuleToEdit.ProductCode;

        public String EditCategoryName => PartRuleToEdit.CategoryName;

        public String EditParameterName => PartRuleToEdit.ParameterName;

        public Int32? EditQty => PartRuleToEdit.Quantity;

        public String EditPACAF_Id => PartRuleToEdit.PACAF_ID;

        public String EditFilterRule => PartRuleToEdit.FilterDependencyName;

        public Int32 EditProductCodeIndex => PartRuleToEdit.ProductID;


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



        //Combo Box Event Changes
        private void FilterType_comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (FilterType_comboBox1.Text == "NONE")//&& styles.Count == 0)
            {
                FILTER1_TableLayoutPanel.Visible = false;
                PACAF_TableLayOut1.Visible = false;
                Qty_TableLayOut1.Visible = false;
                EX_TableLayOut1.Visible = false;
                LN_TableLayOut1.Visible = false;

                Load_Category_ComboBox1();
                Load_Parameter_ListView1();
                Load_Behavior1();
                //Load_Quantity1();
            }

            else if (FilterType_comboBox1.Text == "PACAF")//&& styles.Count == 1)
            {
                FILTER1_TableLayoutPanel.RowStyles[0].Height = 256;
                FILTER1_TableLayoutPanel.RowStyles[1].Height = 0;
                FILTER1_TableLayoutPanel.RowStyles[2].Height = 0;
                FILTER1_TableLayoutPanel.RowStyles[3].Height = 0;

                FILTER1_TableLayoutPanel.Visible = true;
                PACAF_TableLayOut1.Visible = true;
                Qty_TableLayOut1.Visible = false;
                EX_TableLayOut1.Visible = false;
                LN_TableLayOut1.Visible = false;

                Load_Category_ComboBox1();
                Load_Parameter_ListView1();
                Load_Behavior1();
                //Load_Quantity1();
            }

            else if (FilterType_comboBox1.Text == "QUANTITY")// && styles.Count == 2)
            {
                FILTER1_TableLayoutPanel.RowStyles[0].Height = 0;
                FILTER1_TableLayoutPanel.RowStyles[1].Height = 72;
                FILTER1_TableLayoutPanel.RowStyles[2].Height = 0;
                FILTER1_TableLayoutPanel.RowStyles[3].Height = 0;

                FILTER1_TableLayoutPanel.Visible = true;
                PACAF_TableLayOut1.Visible = false;
                Qty_TableLayOut1.Visible = true;
                EX_TableLayOut1.Visible = false;
                LN_TableLayOut1.Visible = false;

                Load_Category_ComboBox1();
                Load_Parameter_ListView1();
                Load_Behavior1();
                //Load_Quantity1();

            }

            else if (FilterType_comboBox1.Text == "EX(8 OR 12)")//&& styles.Count == 3)
            {
                FILTER1_TableLayoutPanel.RowStyles[0].Height = 0;
                FILTER1_TableLayoutPanel.RowStyles[1].Height = 0;
                FILTER1_TableLayoutPanel.RowStyles[2].Height = 41;
                FILTER1_TableLayoutPanel.RowStyles[3].Height = 0;

                FILTER1_TableLayoutPanel.Visible = true;
                PACAF_TableLayOut1.Visible = false;
                Qty_TableLayOut1.Visible = false;
                EX_TableLayOut1.Visible = true;
                LN_TableLayOut1.Visible = false;

                Load_Category_ComboBox1();
                Load_Parameter_ListView1();
                Load_Behavior1();
                //Load_Quantity1();

            }

            else if (FilterType_comboBox1.Text == "LN(8 OR 12)")//&& styles.Count == 4)
            {
                FILTER1_TableLayoutPanel.RowStyles[0].Height = 0;
                FILTER1_TableLayoutPanel.RowStyles[1].Height = 0;
                FILTER1_TableLayoutPanel.RowStyles[2].Height = 0;
                FILTER1_TableLayoutPanel.RowStyles[3].Height = 41;

                FILTER1_TableLayoutPanel.Visible = true;
                PACAF_TableLayOut1.Visible = false;
                Qty_TableLayOut1.Visible = false;
                EX_TableLayOut1.Visible = false;
                LN_TableLayOut1.Visible = true;

                Load_Category_ComboBox1();
                Load_Parameter_ListView1();
                Load_Behavior1();
                //Load_Quantity1();
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
                LN_TableLayOut1.Visible = false;

                Load_Category_ComboBox1();
                Load_Parameter_ListView1();
                Load_Behavior1();
                //Load_Quantity1();
            }
        }

        private void FilterType_comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (FilterType_comboBox2.Text == "NONE")
            {
                FILTER2_TableLayoutPanel.Visible = false;
                PACAF_TableLayOut2.Visible = false;
                Qty_TableLayOut2.Visible = false;
                EX_TableLayOut2.Visible = false;
                LN_TableLayOut2.Visible = false;

                Load_Category_ComboBox2();
                Load_Parameter_ListView2();
                Load_Behavior2();
                //Load_Quantity2();
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
                LN_TableLayOut2.Visible = false;

                Load_Category_ComboBox2();
                Load_Parameter_ListView2();
                Load_Behavior2();
                //Load_Quantity2();
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
                LN_TableLayOut2.Visible = false;

                Load_Category_ComboBox2();
                Load_Parameter_ListView2();
                Load_Behavior2();
                //Load_Quantity2();

            }

            else if (FilterType_comboBox2.Text == "EX(8 OR 12)")
            {
                FILTER2_TableLayoutPanel.RowStyles[0].Height = 0;
                FILTER2_TableLayoutPanel.RowStyles[1].Height = 0;
                FILTER2_TableLayoutPanel.RowStyles[2].Height = 41;
                FILTER2_TableLayoutPanel.RowStyles[3].Height = 0;

                FILTER2_TableLayoutPanel.Visible = true;
                PACAF_TableLayOut2.Visible = false;
                Qty_TableLayOut2.Visible = false;
                EX_TableLayOut2.Visible = true;
                LN_TableLayOut2.Visible = false;

                Load_Category_ComboBox2();
                Load_Parameter_ListView2();
                Load_Behavior2();
                //Load_Quantity2();

            }

            else if (FilterType_comboBox2.Text == "LN(8 OR 12)")
            {
                FILTER2_TableLayoutPanel.RowStyles[0].Height = 0;
                FILTER2_TableLayoutPanel.RowStyles[1].Height = 0;
                FILTER2_TableLayoutPanel.RowStyles[2].Height = 0;
                FILTER2_TableLayoutPanel.RowStyles[3].Height = 41;

                FILTER2_TableLayoutPanel.Visible = true;
                PACAF_TableLayOut2.Visible = false;
                Qty_TableLayOut2.Visible = false;
                EX_TableLayOut2.Visible = false;
                LN_TableLayOut2.Visible = true;

                Load_Category_ComboBox2();
                Load_Parameter_ListView2();
                Load_Behavior2();
                //Load_Quantity2();
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
                LN_TableLayOut2.Visible = false;

                Load_Category_ComboBox2();
                Load_Parameter_ListView2();
                Load_Behavior2();
                //Load_Quantity2();
            }
        }

        private void FilterType_comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (FilterType_comboBox3.Text == "NONE")
            {
                FILTER3_TableLayoutPanel.Visible = false;
                PACAF_TableLayOut3.Visible = false;
                Qty_TableLayOut3.Visible = false;
                EX_TableLayOut3.Visible = false;
                LN_TableLayOut3.Visible = false;

                Load_Category_ComboBox3();
                Load_Parameter_ListView3();
                Load_Behavior3();
                //Load_Quantity3();
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
                LN_TableLayOut3.Visible = false;

                Load_Category_ComboBox3();
                Load_Parameter_ListView3();
                Load_Behavior3();
                //Load_Quantity3();
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
                LN_TableLayOut3.Visible = false;

                Load_Category_ComboBox3();
                Load_Parameter_ListView3();
                Load_Behavior3();
                //Load_Quantity3();

            }

            else if (FilterType_comboBox3.Text == "EX(8 OR 12)")
            {
                FILTER3_TableLayoutPanel.RowStyles[0].Height = 0;
                FILTER3_TableLayoutPanel.RowStyles[1].Height = 0;
                FILTER3_TableLayoutPanel.RowStyles[2].Height = 41;
                FILTER3_TableLayoutPanel.RowStyles[3].Height = 0;

                FILTER3_TableLayoutPanel.Visible = true;
                PACAF_TableLayOut3.Visible = false;
                Qty_TableLayOut3.Visible = false;
                EX_TableLayOut3.Visible = true;
                LN_TableLayOut3.Visible = false;

                Load_Category_ComboBox3();
                Load_Parameter_ListView3();
                Load_Behavior3();
                //Load_Quantity3();

            }

            else if (FilterType_comboBox3.Text == "LN(8 OR 12)")
            {
                FILTER3_TableLayoutPanel.RowStyles[0].Height = 0;
                FILTER3_TableLayoutPanel.RowStyles[1].Height = 0;
                FILTER3_TableLayoutPanel.RowStyles[2].Height = 0;
                FILTER3_TableLayoutPanel.RowStyles[3].Height = 41;

                FILTER3_TableLayoutPanel.Visible = true;
                PACAF_TableLayOut3.Visible = false;
                Qty_TableLayOut3.Visible = false;
                EX_TableLayOut3.Visible = false;
                LN_TableLayOut3.Visible = true;

                Load_Category_ComboBox3();
                Load_Parameter_ListView3();
                Load_Behavior3();
                //Load_Quantity3();
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
                LN_TableLayOut3.Visible = false;

                Load_Category_ComboBox3();
                Load_Parameter_ListView3();
                Load_Behavior3();
                //Load_Quantity3();
            }
        }

        private void FilterType_comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (FilterType_comboBox4.Text == "NONE")
            {
                FILTER4_TableLayoutPanel.Visible = false;
                PACAF_TableLayOut4.Visible = false;
                Qty_TableLayOut4.Visible = false;
                EX_TableLayOut4.Visible = false;
                LN_TableLayOut4.Visible = false;

                Load_Category_ComboBox4();
                Load_Parameter_ListView4();
                Load_Behavior4();
                //Load_Quantity4();
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
                LN_TableLayOut4.Visible = false;

                Load_Category_ComboBox4();
                Load_Parameter_ListView4();
                Load_Behavior4();
                //Load_Quantity4();
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
                LN_TableLayOut4.Visible = false;

                Load_Category_ComboBox4();
                Load_Parameter_ListView4();
                Load_Behavior4();
                //Load_Quantity4();
            }

            else if (FilterType_comboBox4.Text == "EX(8 OR 12)")
            {
                FILTER4_TableLayoutPanel.RowStyles[0].Height = 0;
                FILTER4_TableLayoutPanel.RowStyles[1].Height = 0;
                FILTER4_TableLayoutPanel.RowStyles[2].Height = 41;
                FILTER4_TableLayoutPanel.RowStyles[3].Height = 0;

                FILTER4_TableLayoutPanel.Visible = true;
                PACAF_TableLayOut4.Visible = false;
                Qty_TableLayOut4.Visible = false;
                EX_TableLayOut4.Visible = true;
                LN_TableLayOut4.Visible = false;

                Load_Category_ComboBox4();
                Load_Parameter_ListView4();
                Load_Behavior4();
                //Load_Quantity4();
            }

            else if (FilterType_comboBox4.Text == "LN(8 OR 12)")
            {
                FILTER4_TableLayoutPanel.RowStyles[0].Height = 0;
                FILTER4_TableLayoutPanel.RowStyles[1].Height = 0;
                FILTER4_TableLayoutPanel.RowStyles[2].Height = 0;
                FILTER4_TableLayoutPanel.RowStyles[3].Height = 41;

                FILTER4_TableLayoutPanel.Visible = true;
                PACAF_TableLayOut4.Visible = false;
                Qty_TableLayOut4.Visible = false;
                EX_TableLayOut4.Visible = false;
                LN_TableLayOut4.Visible = true;

                Load_Category_ComboBox4();
                Load_Parameter_ListView4();
                Load_Behavior4();
                //Load_Quantity4();
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
                LN_TableLayOut4.Visible = false;

                Load_Category_ComboBox4();
                Load_Parameter_ListView4();
                Load_Behavior4();
                //Load_Quantity4();
            }
        }

        private void CategoryPR_comboBoxFilter1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Load_Parameter_ListView1();
        }

        private void CategoryPR_comboBoxFilter2_SelectedIndexChanged(object sender, EventArgs e)
        {
            Load_Parameter_ListView2();
        }

        private void CategoryPR_comboBoxFilter3_SelectedIndexChanged(object sender, EventArgs e)
        {
            Load_Parameter_ListView3();
        }

        private void CategoryPR_comboBoxFilter4_SelectedIndexChanged(object sender, EventArgs e)
        {
            Load_Parameter_ListView4();
        }



        //Load PACAF's Filter
        private void Load_Filter_Rules()
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

        private void Load_Category_ComboBox1()
        {
            String productId_PR = productID_comboBox_PR.SelectedValue.ToString();

            if (FilterType_comboBox1.Text == "PACAF")
            {
                CategoryPR_comboBoxFilter1.DataSource = db.ProductTemplates.AsNoTracking().Where(o => o.FixtureId.ToString() == productId_PR && o.CategoryName != "PRODUCT ID").Select(o => new { o.CAF_Id, o.CategoryName, o.CAF_DisplayOrder }).Distinct().OrderBy(o => o.CAF_DisplayOrder).ToList();
                CategoryPR_comboBoxFilter1.ValueMember = "CAF_Id";
                CategoryPR_comboBoxFilter1.DisplayMember = "CategoryName";
            }

            else if (FilterType_comboBox1.Text != "PACAF")
            {
                CategoryPR_comboBoxFilter1.SelectedValue = -1;
            }
        }

        private void Load_Category_ComboBox2()
        {
            String productId_PR = productID_comboBox_PR.SelectedValue.ToString();

            if (FilterType_comboBox2.Text == "PACAF")
            {
                CategoryPR_comboBoxFilter2.DataSource = db.ProductTemplates.AsNoTracking().Where(o => o.FixtureId.ToString() == productId_PR && o.CategoryName != "PRODUCT ID").Select(o => new { o.CAF_Id, o.CategoryName, o.CAF_DisplayOrder }).Distinct().OrderBy(o => o.CAF_DisplayOrder).ToList();
                CategoryPR_comboBoxFilter2.ValueMember = "CAF_Id";
                CategoryPR_comboBoxFilter2.DisplayMember = "CategoryName";
            }

            else if (FilterType_comboBox2.Text != "PACAF")
            {
                CategoryPR_comboBoxFilter2.SelectedValue = -1;
            }
        }

        private void Load_Category_ComboBox3()
        {
            String productId_PR = productID_comboBox_PR.SelectedValue.ToString();

            if (FilterType_comboBox3.Text == "PACAF")
            {
                CategoryPR_comboBoxFilter3.DataSource = db.ProductTemplates.AsNoTracking().Where(o => o.FixtureId.ToString() == productId_PR && o.CategoryName != "PRODUCT ID").Select(o => new { o.CAF_Id, o.CategoryName, o.CAF_DisplayOrder }).Distinct().OrderBy(o => o.CAF_DisplayOrder).ToList();

                CategoryPR_comboBoxFilter3.ValueMember = "CAF_Id";
                CategoryPR_comboBoxFilter3.DisplayMember = "CategoryName";
            }
            else if (FilterType_comboBox3.Text != "PACAF")
            {
                CategoryPR_comboBoxFilter3.SelectedValue = -1;
            }
        }

        private void Load_Category_ComboBox4()
        {
            String productId_PR = productID_comboBox_PR.SelectedValue.ToString();

            if (FilterType_comboBox4.Text == "PACAF")
            {

                CategoryPR_comboBoxFilter4.DataSource = db.ProductTemplates.AsNoTracking().Where(o => o.FixtureId.ToString() == productId_PR && o.CategoryName != "PRODUCT ID").Select(o => new { o.CAF_Id, o.CategoryName, o.CAF_DisplayOrder }).Distinct().OrderBy(o => o.CAF_DisplayOrder).ToList();

                CategoryPR_comboBoxFilter4.ValueMember = "CAF_Id";
                CategoryPR_comboBoxFilter4.DisplayMember = "CategoryName";
            }
            else if (FilterType_comboBox4.Text != "PACAF")
            {
                CategoryPR_comboBoxFilter4.SelectedValue = -1;
            }
        }

        private void Load_Parameter_ListView1()
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
                        String selected_PartName = Fixture_treeView_PartRule.SelectedNode.Text;
                        Int32 partName_Index = Int32.Parse(db.Parts.Where(m => m.PartName == selected_PartName).First().id.ToString());

                        Int32 selected_ProductIdIndex = Int32.Parse(productID_comboBox_PR.SelectedValue.ToString());

                        Int32 selected_ExecutionOrder = 1;

                        Int32? CAF = db.PartRulesFilters.Where(o => o.PartID == partName_Index && o.ProductID == selected_ProductIdIndex && o.OrderOfExecution == selected_ExecutionOrder).First().CategoryID;

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

        private void Load_Parameter_ListView2()
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
                        String selected_PartName = Fixture_treeView_PartRule.SelectedNode.Text;
                        Int32 partName_Index = Int32.Parse(db.Parts.Where(m => m.PartName == selected_PartName).First().id.ToString());

                        Int32 selected_ProductIdIndex = Int32.Parse(productID_comboBox_PR.SelectedValue.ToString());

                        Int32 selected_ExecutionOrder = 2;

                        Int32? CAF = db.PartRulesFilters.Where(o => o.PartID == partName_Index && o.ProductID == selected_ProductIdIndex && o.OrderOfExecution == selected_ExecutionOrder).First().CategoryID;

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

        private void Load_Parameter_ListView3()
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
                        String selected_PartName = Fixture_treeView_PartRule.SelectedNode.Text;
                        Int32 partName_Index = Int32.Parse(db.Parts.Where(m => m.PartName == selected_PartName).First().id.ToString());

                        Int32 selected_ProductIdIndex = Int32.Parse(productID_comboBox_PR.SelectedValue.ToString());

                        Int32 selected_ExecutionOrder = 3;

                        Int32? CAF = db.PartRulesFilters.Where(o => o.PartID == partName_Index && o.ProductID == selected_ProductIdIndex && o.OrderOfExecution == selected_ExecutionOrder).First().CategoryID;

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

        private void Load_Parameter_ListView4()
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
                        String selected_PartName = Fixture_treeView_PartRule.SelectedNode.Text;
                        Int32 partName_Index = Int32.Parse(db.Parts.Where(m => m.PartName == selected_PartName).First().id.ToString());

                        Int32 selected_ProductIdIndex = Int32.Parse(productID_comboBox_PR.SelectedValue.ToString());

                        Int32 selected_ExecutionOrder = 4;

                        Int32? CAF = db.PartRulesFilters.Where(o => o.PartID == partName_Index && o.ProductID == selected_ProductIdIndex && o.OrderOfExecution == selected_ExecutionOrder).First().CategoryID;

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

        private void Load_Behavior1()
        {

            if (FilterType_comboBox1.Text == "PACAF")
            {
                FilterBehavior_ComboBox1.DataSource = db.FilterBehaviors.ToList();
                FilterBehavior_ComboBox1.ValueMember = "id";
                FilterBehavior_ComboBox1.DisplayMember = "Behavior";
            }
            else if (FilterType_comboBox1.Text != "PACAF")
            {
                FilterBehavior_ComboBox1.SelectedValue = -1;
            }

        }

        private void Load_Behavior2()
        {

            if (FilterType_comboBox2.Text == "PACAF")
            {
                FilterBehavior_ComboBox2.DataSource = db.FilterBehaviors.ToList();
                FilterBehavior_ComboBox2.ValueMember = "id";
                FilterBehavior_ComboBox2.DisplayMember = "Behavior";
            }
            else if (FilterType_comboBox2.Text != "PACAF")
            {
                FilterBehavior_ComboBox2.SelectedValue = -1;

            }
        }

        private void Load_Behavior3()
        {

            if (FilterType_comboBox3.Text == "PACAF")
            {
                FilterBehavior_ComboBox3.DataSource = db.FilterBehaviors.ToList();
                FilterBehavior_ComboBox3.ValueMember = "id";
                FilterBehavior_ComboBox3.DisplayMember = "Behavior";
            }
            else if (FilterType_comboBox3.Text != "PACAF")
            {
                FilterBehavior_ComboBox3.SelectedValue = -1;

            }
        }

        private void Load_Behavior4()
        {

            if (FilterType_comboBox4.Text == "PACAF")
            {
                FilterBehavior_ComboBox4.DataSource = db.FilterBehaviors.ToList();
                FilterBehavior_ComboBox4.ValueMember = "id";
                FilterBehavior_ComboBox4.DisplayMember = "Behavior";
            }
            else if (FilterType_comboBox4.Text != "PACAF")
            {
                FilterBehavior_ComboBox4.SelectedValue = -1;

            }
        }

        private void Load_Quantity1()
        {

            if (FilterType_comboBox1.Text == "QUANTITY")
            {
                try
                {
                    String selected_PartName = Fixture_treeView_PartRule.SelectedNode.Text;
                    Int32 selected_ProductIdIndex = Int32.Parse(productID_comboBox_PR.SelectedValue.ToString());
                    Int32 partName_Index = Int32.Parse(db.Parts.Where(m => m.PartName == selected_PartName).First().id.ToString());

                    Qty_textBox1.Text = db.PartRulesFilters.Where(o => o.PartID == partName_Index && o.ProductID == selected_ProductIdIndex && o.OrderOfExecution == 1).First().QuantityRule;

                    Qty_NumericUpDown1.Value = db.PartRulesFilters.Where(o => o.PartID == partName_Index && o.ProductID == selected_ProductIdIndex && o.OrderOfExecution == 1).First().Quantity ?? 0;

                }
                catch
                {

                }

            }
            else if (FilterType_comboBox1.Text != "QUANTITY")
            {
                Qty_textBox1.Text = "";
                Qty_NumericUpDown1.Value = 0;
            }
        }

        private void Load_Quantity2()
        {

            if (FilterType_comboBox2.Text == "QUANTITY")
            {
                try
                {
                    String selected_PartName = Fixture_treeView_PartRule.SelectedNode.Text;
                    Int32 selected_ProductIdIndex = Int32.Parse(productID_comboBox_PR.SelectedValue.ToString());
                    Int32 partName_Index = Int32.Parse(db.Parts.Where(m => m.PartName == selected_PartName).First().id.ToString());

                    Qty_textBox2.Text = db.PartRulesFilters.Where(o => o.PartID == partName_Index && o.ProductID == selected_ProductIdIndex && o.OrderOfExecution == 2).First().QuantityRule;

                    Qty_NumericUpDown2.Value = db.PartRulesFilters.Where(o => o.PartID == partName_Index && o.ProductID == selected_ProductIdIndex && o.OrderOfExecution == 2).First().Quantity ?? 0;

                }
                catch
                {

                }

            }
            else if (FilterType_comboBox2.Text != "QUANTITY")
            {
                Qty_textBox2.Text = "";
                Qty_NumericUpDown2.Value = 0;
            }
        }

        private void Load_Quantity3()
        {

            if (FilterType_comboBox3.Text == "QUANTITY")
            {
                try
                {
                    String selected_PartName = Fixture_treeView_PartRule.SelectedNode.Text;
                    Int32 selected_ProductIdIndex = Int32.Parse(productID_comboBox_PR.SelectedValue.ToString());
                    Int32 partName_Index = Int32.Parse(db.Parts.Where(m => m.PartName == selected_PartName).First().id.ToString());

                    Qty_textBox3.Text = db.PartRulesFilters.Where(o => o.PartID == partName_Index && o.ProductID == selected_ProductIdIndex && o.OrderOfExecution == 3).First().QuantityRule;

                    Qty_NumericUpDown3.Value = db.PartRulesFilters.Where(o => o.PartID == partName_Index && o.ProductID == selected_ProductIdIndex && o.OrderOfExecution == 3).First().Quantity ?? 0;

                }
                catch
                {

                }

            }
            else if (FilterType_comboBox3.Text != "QUANTITY")
            {
                Qty_textBox3.Text = "";
                Qty_NumericUpDown3.Value = 0;
            }
        }

        private void Load_Quantity4()
        {

            if (FilterType_comboBox4.Text == "QUANTITY")
            {
                try
                {
                    String selected_PartName = Fixture_treeView_PartRule.SelectedNode.Text;
                    Int32 selected_ProductIdIndex = Int32.Parse(productID_comboBox_PR.SelectedValue.ToString());
                    Int32 partName_Index = Int32.Parse(db.Parts.Where(m => m.PartName == selected_PartName).First().id.ToString());

                    Qty_textBox4.Text = db.PartRulesFilters.Where(o => o.PartID == partName_Index && o.ProductID == selected_ProductIdIndex && o.OrderOfExecution == 4).First().QuantityRule;

                    Qty_NumericUpDown4.Value = db.PartRulesFilters.Where(o => o.PartID == partName_Index && o.ProductID == selected_ProductIdIndex && o.OrderOfExecution == 4).First().Quantity ?? 0;

                }
                catch
                {

                }

            }
            else if (FilterType_comboBox4.Text != "QUANTITY")
            {
                Qty_textBox4.Text = "";
                Qty_NumericUpDown4.Value = 0;
            }
        }



        //Load Filter Types from DATABASE in to BOM MANAGER
        private void Load_FilterType1()
        {
            ParameterPR_ListView1.SelectedItems.Clear();

            String selected_PartName = Fixture_treeView_PartRule.SelectedNode.Text;

            String selected_ProductCode = productID_comboBox_PR.Text;
            Int32 selected_ProductIdIndex = Int32.Parse(productID_comboBox_PR.SelectedValue.ToString());

            Int32 selected_ExecutionOrder = 1;
            try
            {
                Int32 partName_Index = Int32.Parse(db.Parts.Where(m => m.PartName == selected_PartName).First().id.ToString());

                FilterType_comboBox1.SelectedValue = db.PartRulesFilters.Where(o => o.PartID == partName_Index && o.ProductID == selected_ProductIdIndex && o.OrderOfExecution == selected_ExecutionOrder).First().FilterTypeID;

                CategoryPR_comboBoxFilter1.SelectedValue = db.PartRulesFilters.Where(o => o.PartID == partName_Index && o.ProductID == selected_ProductIdIndex && o.OrderOfExecution == selected_ExecutionOrder).First().CategoryID;

                FilterBehavior_ComboBox1.SelectedValue = db.PartRulesFilters.Where(o => o.PartID == partName_Index && o.ProductID == selected_ProductIdIndex && o.OrderOfExecution == selected_ExecutionOrder).First().FilterDependencyID;

                //Enter Selected PARAMETER ID OR TEXT
                String parameterString = db.PartRulesFilters.Where(o => o.PartID == partName_Index && o.ProductID == selected_ProductIdIndex && o.OrderOfExecution == selected_ExecutionOrder).First().ParameterName;

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



                //Qty_textBox1.Text = db.PartRulesFilters.Where(o => o.PartID == partName_Index && o.ProductID == selected_ProductIdIndex && o.OrderOfExecution == selected_ExecutionOrder).First().QuantityRule;

                //Qty_NumericUpDown1.Value = db.PartRulesFilters.Where(o => o.PartID == partName_Index && o.ProductID == selected_ProductIdIndex && o.OrderOfExecution == selected_ExecutionOrder).First().Quantity ?? 0;

                //eventLog_PR_richTextBox.Clear();

                Load_Quantity1();

            }

            catch (NullReferenceException )
            {
                //PartRules.NewMessage().AddText(PartName + ": No PACAF Fixture Rules for Filter 1 Type exist. ").AddText(ex.Message).IsError().PrependMessageType().Log();
                //FilterType_comboBox1.SelectedValue = 1;
            }

            catch (InvalidOperationException ex1)
            {
                PartRules.NewMessage().AddText("No Rules for Filter 1 Type exist for selected part. ").AddText(ex1.Message).PrependMessageType().Log();
                FilterType_comboBox1.SelectedValue = 1;
            }

            catch (ArgumentNullException )
            {
                //PartRules.NewMessage().AddText("No Fixture Rules for selected part. ").AddText(ex2.Message).PrependMessageType().Log();
                //FilterType_comboBox1.SelectedValue = 1;
            }

        }

        private void Load_FilterType2()
        {
            ParameterPR_ListView2.SelectedItems.Clear();

            String selected_PartName = Fixture_treeView_PartRule.SelectedNode.Text;

            String selected_ProductCode = productID_comboBox_PR.Text;
            Int32 selected_ProductIdIndex = Int32.Parse(productID_comboBox_PR.SelectedValue.ToString());

            Int32 selected_ExecutionOrder = 2;


            try
            {
                Int32 partName_Index = Int32.Parse(db.Parts.Where(m => m.PartName == selected_PartName).First().id.ToString());

                FilterType_comboBox2.SelectedValue = db.PartRulesFilters.Where(o => o.PartID == partName_Index && o.ProductID == selected_ProductIdIndex && o.OrderOfExecution == selected_ExecutionOrder).First().FilterTypeID;

                CategoryPR_comboBoxFilter2.SelectedValue = db.PartRulesFilters.Where(o => o.PartID == partName_Index && o.ProductID == selected_ProductIdIndex && o.OrderOfExecution == selected_ExecutionOrder).First().CategoryID;

                FilterBehavior_ComboBox2.SelectedValue = db.PartRulesFilters.Where(o => o.PartID == partName_Index && o.ProductID == selected_ProductIdIndex && o.OrderOfExecution == selected_ExecutionOrder).First().FilterDependencyID;

                //Enter Selected PARAMETER ID OR TEXT
                String parameterString = db.PartRulesFilters.Where(o => o.PartID == partName_Index && o.ProductID == selected_ProductIdIndex && o.OrderOfExecution == selected_ExecutionOrder).First().ParameterName;

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

            catch (NullReferenceException )
            {
                //PartRules.NewMessage().AddText(PartName + ": No PACAF Fixture Rules for Filter 2 Type exist. ").AddText(ex.Message).IsError().PrependMessageType().Log();
                //FilterType_comboBox2.SelectedValue = 1;
            }

            catch (InvalidOperationException ex1)
            {
                PartRules.NewMessage().AddText("No Rules for Filter 2 Type exist for selected part. ").AddText(ex1.Message).PrependMessageType().Log();
                FilterType_comboBox2.SelectedValue = 1;
            }

            catch (ArgumentNullException )
            {
                //PartRules.NewMessage().AddText("No Fixture Rules for selected part. ").AddText(ex2.Message).PrependMessageType().Log();
                //FilterType_comboBox2.SelectedValue = 1;
            }


            //Qty_textBox2.Text = db.PartRulesFilters.Where(o => o.PartID == partName_Index && o.ProductID == selected_ProductIdIndex && o.OrderOfExecution == selected_ExecutionOrder).First().QuantityRule;

            //Qty_NumericUpDown2.Value = db.PartRulesFilters.Where(o => o.PartID == partName_Index && o.ProductID == selected_ProductIdIndex && o.OrderOfExecution == selected_ExecutionOrder).First().Quantity ?? 0;

            //eventLog_PR_richTextBox.Clear();

            Load_Quantity2();

        }

        private void Load_FilterType3()
        {
            ParameterPR_ListView3.SelectedItems.Clear();

            String selected_PartName = Fixture_treeView_PartRule.SelectedNode.Text;

            String selected_ProductCode = productID_comboBox_PR.Text;
            Int32 selected_ProductIdIndex = Int32.Parse(productID_comboBox_PR.SelectedValue.ToString());

            Int32 selected_ExecutionOrder = 3;

            try
            {
                Int32 partName_Index = Int32.Parse(db.Parts.Where(m => m.PartName == selected_PartName).First().id.ToString());

                FilterType_comboBox3.SelectedValue = db.PartRulesFilters.Where(o => o.PartID == partName_Index && o.ProductID == selected_ProductIdIndex && o.OrderOfExecution == selected_ExecutionOrder).First().FilterTypeID;

                CategoryPR_comboBoxFilter3.SelectedValue = db.PartRulesFilters.Where(o => o.PartID == partName_Index && o.ProductID == selected_ProductIdIndex && o.OrderOfExecution == selected_ExecutionOrder).First().CategoryID;

                FilterBehavior_ComboBox3.SelectedValue = db.PartRulesFilters.Where(o => o.PartID == partName_Index && o.ProductID == selected_ProductIdIndex && o.OrderOfExecution == selected_ExecutionOrder).First().FilterDependencyID;

                //Enter Selected PARAMETER ID OR TEXT
                String parameterString = db.PartRulesFilters.Where(o => o.PartID == partName_Index && o.ProductID == selected_ProductIdIndex && o.OrderOfExecution == selected_ExecutionOrder).First().ParameterName;

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

            catch (NullReferenceException )
            {
                //PartRules.NewMessage().AddText(PartName + ": No PACAF Fixture Rules for Filter 3 Type exist. ").AddText(ex.Message).IsError().PrependMessageType().Log();
                //FilterType_comboBox3.SelectedValue = 1;
            }

            catch (InvalidOperationException ex1)
            {
                PartRules.NewMessage().AddText("No Rules for Filter 3 Type exist for selected part. ").AddText(ex1.Message).PrependMessageType().Log();
                FilterType_comboBox3.SelectedValue = 1;

            }

            catch (ArgumentNullException )
            {
                //PartRules.NewMessage().AddText("No Fixture Rules for selected part. ").AddText(ex2.Message).PrependMessageType().Log();
                //FilterType_comboBox3.SelectedValue = 1;
            }


            //Qty_textBox3.Text = db.PartRulesFilters.Where(o => o.PartID == partName_Index && o.ProductID == selected_ProductIdIndex && o.OrderOfExecution == selected_ExecutionOrder).First().QuantityRule;

            //Qty_NumericUpDown3.Value = db.PartRulesFilters.Where(o => o.PartID == partName_Index && o.ProductID == selected_ProductIdIndex && o.OrderOfExecution == selected_ExecutionOrder).First().Quantity ?? 0;

            //eventLog_PR_richTextBox.Clear();

            Load_Quantity3();

        }

        private void Load_FilterType4()
        {
            ParameterPR_ListView4.SelectedItems.Clear();

            String selected_PartName = Fixture_treeView_PartRule.SelectedNode.Text;

            String selected_ProductCode = productID_comboBox_PR.Text;

            Int32 selected_ProductIdIndex = Int32.Parse(productID_comboBox_PR.SelectedValue.ToString());

            Int32 selected_ExecutionOrder = 4;

            try
            {
                Int32 partName_Index = Int32.Parse(db.Parts.Where(m => m.PartName == selected_PartName).First().id.ToString());

                FilterType_comboBox4.SelectedValue = db.PartRulesFilters.Where(o => o.PartID == partName_Index && o.ProductID == selected_ProductIdIndex && o.OrderOfExecution == selected_ExecutionOrder).First().FilterTypeID;

                CategoryPR_comboBoxFilter4.SelectedValue = db.PartRulesFilters.Where(o => o.PartID == partName_Index && o.ProductID == selected_ProductIdIndex && o.OrderOfExecution == selected_ExecutionOrder).First().CategoryID;

                FilterBehavior_ComboBox4.SelectedValue = db.PartRulesFilters.Where(o => o.PartID == partName_Index && o.ProductID == selected_ProductIdIndex && o.OrderOfExecution == selected_ExecutionOrder).First().FilterDependencyID;

                //Enter Selected PARAMETER ID OR TEXT
                String parameterString = db.PartRulesFilters.Where(o => o.PartID == partName_Index && o.ProductID == selected_ProductIdIndex && o.OrderOfExecution == selected_ExecutionOrder).First().ParameterName;

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

            catch (NullReferenceException )
            {
                //PartRules.NewMessage().AddText(PartName + ": No PACAF Fixture Rules for Filter 4 Type exist. ").AddText(ex.Message).IsError().PrependMessageType().Log();
                //FilterType_comboBox4.SelectedValue = 1;
            }

            catch (InvalidOperationException ex1)
            {
                PartRules.NewMessage().AddText("No Rules for Filter 4 Type exist for selected part. ").AddText(ex1.Message).PrependMessageType().Log();
                FilterType_comboBox4.SelectedValue = 1;
            }

            catch (ArgumentNullException )
            {
                //PartRules.NewMessage().AddText("No Fixture Rules for selected part. ").AddText(ex2.Message).PrependMessageType().Log();
                //FilterType_comboBox4.SelectedValue = 1;
            }

            //Qty_textBox4.Text = db.PartRulesFilters.Where(o => o.PartID == partName_Index && o.ProductID == selected_ProductIdIndex && o.OrderOfExecution == selected_ExecutionOrder).First().QuantityRule;

            //Qty_NumericUpDown4.Value = db.PartRulesFilters.Where(o => o.PartID == partName_Index && o.ProductID == selected_ProductIdIndex && o.OrderOfExecution == selected_ExecutionOrder).First().Quantity ?? 0;

            //eventLog_PR_richTextBox.Clear();

            Load_Quantity4();
        }



        //DATA GRID VIEW
        private void DataGridView_Rules_SelectionChanged(object sender, EventArgs e)
        {

        }

        private void RefreshDataGridView_PartRules()
        {
            try
            {
                DataGridView_Rules.DataSource = db.PartRulesFilters.Where(o => o.PartName == Fixture_treeView_PartRule.SelectedNode.Text).OrderBy(m => m.PartName).ToList();

                DataGridView_Rules.Columns["id"].Visible = false;
                DataGridView_Rules.Columns["PartID"].Visible = false;
                DataGridView_Rules.Columns["ProductID"].Visible = false;
                DataGridView_Rules.Columns["CategoryID"].Visible = true;
                DataGridView_Rules.Columns["ParameterID"].Visible = true;
                DataGridView_Rules.Columns["FilterDependencyID"].Visible = false;
                DataGridView_Rules.Columns["FilterBehavior"].Visible = false;
                DataGridView_Rules.Columns["FilterType"].Visible = false;
                DataGridView_Rules.Columns["Part"].Visible = false;

                DataGridView_Rules.AutoResizeColumns();
                DataGridView_Rules.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

                eventLog_richTextBox.ScrollToCaret();

            }
            catch (System.Reflection.TargetException ex)
            {
                PartRules.NewMessage().AddText("No Part Selected in TreeView. ").AddText(ex.Message).IsError().PrependMessageType().Log();
            }

            //catch
            //{
            //    //eventLog_PR_richTextBox.Clear();
            //    PartRules.NewMessage().AddText("Refreshing Assembly Table failed").PrependMessageType().Log();
            //}

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

            Load_FilterType1();
            Load_FilterType2();
            Load_FilterType3();
            Load_FilterType4();


        }



        //BUTTON (SAVE, EDIT)
        private void Save_PR_Click(object sender, EventArgs e)
        {
            eventLog_PR_richTextBox.Clear();
            String selected_PartName = Fixture_treeView_PartRule.SelectedNode.Text;
            Int32 partName_Index = Int32.Parse(db.Parts.Where(m => m.PartName == selected_PartName).First().id.ToString());

            String selected_ProductCode = productID_comboBox_PR.Text;
            Int32 selected_ProductIdIndex = Int32.Parse(productID_comboBox_PR.SelectedValue.ToString());

            bool FilterExists1 = db.PartRulesFilters.Any(o => o.PartName == selected_PartName && o.OrderOfExecution == 1);
            bool FilterExists2 = db.PartRulesFilters.Any(o => o.PartName == selected_PartName && o.OrderOfExecution == 2);
            bool FilterExists3 = db.PartRulesFilters.Any(o => o.PartName == selected_PartName && o.OrderOfExecution == 3);
            bool FilterExists4 = db.PartRulesFilters.Any(o => o.PartName == selected_PartName && o.OrderOfExecution == 4);

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

                Int32 orderOfExecution = 1;

                Int32? selected_Quantity;
                String quantityRule = Qty_textBox1.Text;
                if (Qty_NumericUpDown1.Value == 0)
                {
                    selected_Quantity = null;
                    quantityRule = null;
                }
                else
                {
                    selected_Quantity = Int32.Parse(Qty_NumericUpDown1.Value.ToString());
                }

                //Check if Filter Exists 
                //PartRulesFilter newPartRule;

                if (FilterExists1)
                {
                    newPartRule = db.PartRulesFilters.Where(p => p.PartName == selected_PartName && p.OrderOfExecution == orderOfExecution).First();
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
                    newPartRule.PartID = partName_Index;
                    newPartRule.PartName = selected_PartName;
                    newPartRule.ProductID = selected_ProductIdIndex;
                    newPartRule.ProductCode = selected_ProductCode;
                    newPartRule.CategoryID = selected_CategoryIndex;
                    newPartRule.CategoryName = selected_Category;
                    newPartRule.ParameterID = Selected_ParameterIndex;
                    newPartRule.ParameterName = selected_Parameter;
                    newPartRule.FilterTypeID = selected_FilterType;
                    newPartRule.OrderOfExecution = orderOfExecution;
                    newPartRule.PACAF_ID = selected_PACAF;
                    newPartRule.FilterDependencyID = selected_FilterDependency;
                    newPartRule.FilterDependencyName = selected_FilterDependencyName;
                    newPartRule.Quantity = selected_Quantity;
                    newPartRule.QuantityRule = quantityRule;

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
                eventLog_PR_richTextBox.Clear();

                newPartRule = db.PartRulesFilters.Where(o => o.PartName == selected_PartName && o.OrderOfExecution == 1).First();
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
                String selected_FilterDependencyName = FilterBehavior_ComboBox2.Text;
                if (FilterBehavior_ComboBox2.SelectedValue == null)
                {
                    selected_FilterDependency = null;
                    selected_FilterDependencyName = null;
                }
                else
                {
                    selected_FilterDependency = Int32.Parse(FilterBehavior_ComboBox2.SelectedValue.ToString());
                }

                Int32 orderOfExecution = 2;

                Int32? selected_Quantity;
                String quantityRule = Qty_textBox2.Text;
                if (Qty_NumericUpDown2.Value == 0)
                {
                    selected_Quantity = null;
                    quantityRule = null;
                }
                else
                {
                    selected_Quantity = Int32.Parse(Qty_NumericUpDown2.Value.ToString());
                }

                //PartRulesFilter newPartRule;

                if (FilterExists2)
                {
                    newPartRule = db.PartRulesFilters.Where(p => p.PartName == selected_PartName && p.OrderOfExecution == orderOfExecution).First();
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
                    newPartRule.PartID = partName_Index;
                    newPartRule.PartName = selected_PartName;
                    newPartRule.ProductID = selected_ProductIdIndex;
                    newPartRule.ProductCode = selected_ProductCode;
                    newPartRule.CategoryID = selected_CategoryIndex;
                    newPartRule.CategoryName = selected_Category;
                    newPartRule.ParameterID = Selected_ParameterIndex;
                    newPartRule.ParameterName = selected_Parameter;
                    newPartRule.FilterTypeID = selected_FilterType;
                    newPartRule.OrderOfExecution = orderOfExecution;
                    newPartRule.PACAF_ID = selected_PACAF;
                    newPartRule.FilterDependencyID = selected_FilterDependency;
                    newPartRule.FilterDependencyName = selected_FilterDependencyName;
                    newPartRule.Quantity = selected_Quantity;
                    newPartRule.QuantityRule = quantityRule;


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
                eventLog_PR_richTextBox.Clear();

                newPartRule = db.PartRulesFilters.Where(o => o.PartName == selected_PartName && o.OrderOfExecution == 2).First();
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
                String selected_FilterDependencyName = FilterBehavior_ComboBox3.Text;
                if (FilterBehavior_ComboBox3.SelectedValue == null)
                {
                    selected_FilterDependency = null;
                    selected_FilterDependencyName = null;
                }
                else
                {
                    selected_FilterDependency = Int32.Parse(FilterBehavior_ComboBox3.SelectedValue.ToString());
                }

                Int32 orderOfExecution = 3;

                Int32? selected_Quantity;
                String quantityRule = Qty_textBox3.Text;
                if (Qty_NumericUpDown3.Value == 0)
                {
                    selected_Quantity = null;
                    quantityRule = null;
                }
                else
                {
                    selected_Quantity = Int32.Parse(Qty_NumericUpDown3.Value.ToString());
                }

                //PartRulesFilter newPartRule;

                if (FilterExists3)
                {
                    newPartRule = db.PartRulesFilters.Where(p => p.PartName == selected_PartName && p.OrderOfExecution == orderOfExecution).First();
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
                    newPartRule.PartID = partName_Index;
                    newPartRule.PartName = selected_PartName;
                    newPartRule.ProductID = selected_ProductIdIndex;
                    newPartRule.ProductCode = selected_ProductCode;
                    newPartRule.CategoryID = selected_CategoryIndex;
                    newPartRule.CategoryName = selected_Category;
                    newPartRule.ParameterID = Selected_ParameterIndex;
                    newPartRule.ParameterName = selected_Parameter;
                    newPartRule.FilterTypeID = selected_FilterType;
                    newPartRule.OrderOfExecution = orderOfExecution;
                    newPartRule.PACAF_ID = selected_PACAF;
                    newPartRule.FilterDependencyID = selected_FilterDependency;
                    newPartRule.FilterDependencyName = selected_FilterDependencyName;
                    newPartRule.Quantity = selected_Quantity;
                    newPartRule.QuantityRule = quantityRule;

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
                eventLog_PR_richTextBox.Clear();

                newPartRule = db.PartRulesFilters.Where(o => o.PartName == selected_PartName && o.OrderOfExecution == 3).First();
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
                String selected_FilterDependencyName = FilterBehavior_ComboBox4.Text;
                if (FilterBehavior_ComboBox4.SelectedValue == null)
                {
                    selected_FilterDependency = null;
                    selected_FilterDependencyName = null;
                }
                else
                {
                    selected_FilterDependency = Int32.Parse(FilterBehavior_ComboBox4.SelectedValue.ToString());
                }

                Int32 orderOfExecution = 4;

                Int32? selected_Quantity;
                String quantityRule = Qty_textBox4.Text;
                if (Qty_NumericUpDown4.Value == 0)
                {
                    selected_Quantity = null;
                    quantityRule = null;
                }
                else
                {
                    selected_Quantity = Int32.Parse(Qty_NumericUpDown4.Value.ToString());
                }

                //PartRulesFilter newPartRule;

                if (FilterExists4)
                {
                    newPartRule = db.PartRulesFilters.Where(p => p.PartName == selected_PartName && p.OrderOfExecution == orderOfExecution).First();
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
                    newPartRule.PartID = partName_Index;
                    newPartRule.PartName = selected_PartName;
                    newPartRule.ProductID = selected_ProductIdIndex;
                    newPartRule.ProductCode = selected_ProductCode;
                    newPartRule.CategoryID = selected_CategoryIndex;
                    newPartRule.CategoryName = selected_Category;
                    newPartRule.ParameterID = Selected_ParameterIndex;
                    newPartRule.ParameterName = selected_Parameter;
                    newPartRule.FilterTypeID = selected_FilterType;
                    newPartRule.OrderOfExecution = orderOfExecution;
                    newPartRule.PACAF_ID = selected_PACAF;
                    newPartRule.FilterDependencyID = selected_FilterDependency;
                    newPartRule.FilterDependencyName = selected_FilterDependencyName;
                    newPartRule.Quantity = selected_Quantity;
                    newPartRule.QuantityRule = quantityRule;


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

            else if (FilterType_comboBox4.Text == "NONE" && FilterExists4)
            {
                eventLog_PR_richTextBox.Clear();

                newPartRule = db.PartRulesFilters.Where(o => o.PartName == selected_PartName && o.OrderOfExecution == 4).First();
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
            Qty_textBox1.Enabled = false;
            Qty_NumericUpDown1.Enabled = false;

        }

        private void Disable_Filter2()
        {
            FilterType_comboBox2.Enabled = false;
            CategoryPR_comboBoxFilter2.Enabled = false;
            ParameterPR_ListView2.Enabled = false;
            FilterBehavior_ComboBox2.Enabled = false;
            Qty_textBox2.Enabled = false;
            Qty_NumericUpDown2.Enabled = false;

        }

        private void Disable_Filter3()
        {
            FilterType_comboBox3.Enabled = false;
            CategoryPR_comboBoxFilter3.Enabled = false;
            ParameterPR_ListView3.Enabled = false;
            FilterBehavior_ComboBox3.Enabled = false;
            Qty_textBox3.Enabled = false;
            Qty_NumericUpDown3.Enabled = false;

        }

        private void Disable_Filter4()
        {
            FilterType_comboBox4.Enabled = false;
            CategoryPR_comboBoxFilter4.Enabled = false;
            ParameterPR_ListView4.Enabled = false;
            FilterBehavior_ComboBox4.Enabled = false;
            Qty_textBox4.Enabled = false;
            Qty_NumericUpDown4.Enabled = false;

        }

        private void Enable_Filter1()
        {
            FilterType_comboBox1.Enabled = true;
            CategoryPR_comboBoxFilter1.Enabled = true;
            ParameterPR_ListView1.Enabled = true;
            FilterBehavior_ComboBox1.Enabled = true;
            Qty_textBox1.Enabled = true;
            Qty_NumericUpDown1.Enabled = true;
        }

        private void Enable_Filter2()
        {
            FilterType_comboBox2.Enabled = true;
            CategoryPR_comboBoxFilter2.Enabled = true;
            ParameterPR_ListView2.Enabled = true;
            FilterBehavior_ComboBox2.Enabled = true;
            Qty_textBox2.Enabled = true;
            Qty_NumericUpDown2.Enabled = true;
        }

        private void Enable_Filter3()
        {
            FilterType_comboBox3.Enabled = true;
            CategoryPR_comboBoxFilter3.Enabled = true;
            ParameterPR_ListView3.Enabled = true;
            FilterBehavior_ComboBox3.Enabled = true;
            Qty_textBox3.Enabled = true;
            Qty_NumericUpDown3.Enabled = true;
        }

        private void Enable_Filter4()
        {
            FilterType_comboBox4.Enabled = true;
            CategoryPR_comboBoxFilter4.Enabled = true;
            ParameterPR_ListView4.Enabled = true;
            FilterBehavior_ComboBox4.Enabled = true;
            Qty_textBox4.Enabled = true;
            Qty_NumericUpDown4.Enabled = true;
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

            NewBOM = new _BOM(_FixtureSetupCode);
            NewBOM.TestconfigSection(PartGen_log2);
            // _FixtureConfiguration NewFixture = new _FixtureConfiguration(_FixtureSetupCode);
            NewBOM.FixtureConfiguration.ConfigureClientRequest();
            NewBOM.FixtureConfiguration.ConfigureSections();
            NewBOM.FixtureConfiguration.ConfigureCoverElements();

            NewBOM.FixtureConfiguration.CustomerRequest.Template.SummarizeIntoRTB(PartGen_log2);

            Log2_RichTextBox.SelectionStart = 0;
            Log2_RichTextBox.ScrollToCaret();
        }

        private void Match_Summary_Button_Click(object sender, EventArgs e)
        {
            Log2_RichTextBox.Clear();
            _FixtureConfiguration NewFixture = new _FixtureConfiguration(_FixtureSetupCode);
            NewFixture.ConfigureClientRequest();
            NewFixture.ConfigureSections();
            NewFixture.ConfigureCoverElements();

            NewFixture.CustomerRequest.Template.SummarizeMatchesIntoRTB(PartGen_log2);

            Log2_RichTextBox.SelectionStart = 0;
            Log2_RichTextBox.ScrollToCaret();

        }

        private void Solve_Mechanical_Button_Click(object sender, EventArgs e)
        {
            Log2_RichTextBox.Clear();
            _FixtureConfiguration NewFixture = new _FixtureConfiguration(_FixtureSetupCode);
            NewFixture.ConfigureClientRequest();
            NewFixture.ConfigureSections();
            NewFixture.ConfigureCoverElements();

            NewFixture.Sections.SummarizeMechanicalIntoRTB(PartGen_log2);

            Log2_RichTextBox.SelectionStart = 0;
            Log2_RichTextBox.ScrollToCaret();
        }

        private void GetPart_Button_Click(object sender, EventArgs e)
        {

            Log_RichTextBox.Clear();
            Log2_RichTextBox.Clear();

            NewBOM = new _BOM(_FixtureSetupCode);
            List<String> finalSectionParts = new List<String>();
            List<String> noneApplicableParts = new List<String>();

            NewBOM.TestconfigSection(PartGen);
            NewBOM.GetOrderingCodePACAFs();

            PartGen.NewMessage().SetSpaceAfter(0).AddText("Bill Of Material selected Code: ").AddBoldText(_FixtureSetupCode).Log();


            int totalSections = NewBOM.FixtureConfiguration.Sections.Count;

            Decimal currentSectionLength = 0;
            for (int sectionIncreament = 0; sectionIncreament < totalSections; sectionIncreament++)
            {
                //Function to go through treeview and add that to thr RichText Box
                String sectionDefinition = "Start|Middle|End";
                NewBOM.SummarizeBOMinIntoRTB(PartGen, sectionIncreament, ref currentSectionLength, ref sectionDefinition);
                Filter( PartGen, PartGen_log2, finalSectionParts, noneApplicableParts, currentSectionLength, sectionIncreament);

            }

            Log_RichTextBox.SelectionStart = 0;
            Log_RichTextBox.ScrollToCaret();
        }

        public void Filter( _RTFMessenger Log1_Messenger, _RTFMessenger Log2_Messenger, List<String> finalSectionParts, List<String> noneApplicableParts, Decimal currentSectionLength, Int32 sectionIncreament)
        {
            foreach (TreeNode ParentNode in Fixture_treeView_PartGen.Nodes)
            {
                foreach (TreeNode childNode in ParentNode.Nodes)
                {
                    String childNode_PartName = childNode.Text;
                    String childNode_PartType = ((PartView)childNode.Tag).PartType;
                    String childNode_ProductCode = NewBOM.FixtureConfiguration.ProductCode;
                    String unsuitableParts = "";
                    String[] parameter_List = { };
                    Int32 numberOfFilters = db.PartRulesFilters.Where(o => o.PartName == childNode_PartName && o.ProductCode == childNode_ProductCode).Count();
                    
                    String partNameToSave = "";
                    String partTypeToSave = "";
                    String QuantityToSave = "";
                    Boolean PACAFisAvailable = false;

                    if (numberOfFilters == 0)
                    {
                        bool exists = LineExists(childNode.Text);

                        if (!exists)
                        {
                            //PartGen_log2.NewMessage().AddBoldText(currentPartName).AddText(": CONTAINS NO RULES").IsError().Log();
                            unsuitableParts = childNode_PartName + ": CONTAINS NO RULES";
                            noneApplicableParts.Add(unsuitableParts);
                        }
                    }

                    else
                    {
                        try
                        {
                            for (int x = 1; x <= numberOfFilters; x++)
                            {
                                Int32 orderOfExecution = x;
                                Int32 filterType = db.PartRulesFilters.Where(o => o.PartName == childNode_PartName && o.ProductCode == childNode_ProductCode && o.OrderOfExecution == orderOfExecution).First().FilterTypeID;
                                if (filterType == 2)
                                {   
                                    //Add PACAF to list
                                    PACAF_Filter(ref partNameToSave, ref partTypeToSave, currentSectionLength, sectionIncreament, childNode, Log1_Messenger, orderOfExecution, ref finalSectionParts, noneApplicableParts,  PACAFisAvailable);

                                }
                                else if (filterType == 3)
                                {   //Add Quantity to Every part in List
                                    Quantity(childNode_ProductCode, partNameToSave, partTypeToSave, ref finalSectionParts);
                                }
                                else if (filterType == 4)
                                {
                                    //Code here
                                }
                                else if (filterType == 5)
                                {
                                    //Code here
                                }
                                else if (filterType == 6)
                                {
                                    Joiner_Qty(childNode_ProductCode, childNode_PartName, ref partNameToSave, ref partTypeToSave, ref finalSectionParts, childNode, sectionIncreament);
                                }

                                
                            }
                           
                        }
                        catch (InvalidOperationException )
                        {
                            //MessageBox.Show(" Error Check First Filter");
                        }

                    }

                }

            }

            //Print Final List of parts at section here
            foreach (String items in finalSectionParts)
            {
                Log1_Messenger.NewMessage().AddText(items).Log();

            }
            foreach (String items in noneApplicableParts)
            {
                Log2_Messenger.NewMessage().AddText(items).Log();
            }
            finalSectionParts.Clear();
            noneApplicableParts.Clear();


        }

        public void PACAF_Filter(ref String partNameToSave, ref String partTypeToSave, Decimal currentSectionLength,Int32 sectionIncreament,  TreeNode childNode, _RTFMessenger log1_Messenger, Int32 orderOfExecution, ref List<String> finalSectionParts, List<String> noneApplicableParts, Boolean PACAFisAvailable)
        {
            String parameterList = db.PartRulesFilters.Where(o => o.PartName == childNode.Text && o.ProductCode == NewBOM.FixtureConfiguration.ProductCode && o.OrderOfExecution == orderOfExecution).First().PACAF_ID;
            String GetFiterBehavior = db.PartRulesFilters.Where(o => o.PartName == childNode.Text && o.OrderOfExecution == orderOfExecution).First().FilterDependencyName.ToString();
            String[] parameterList_split = parameterList.Split('|');
            String unsuitableParts = "";

            foreach (String x in parameterList_split)
            {
                if (NewBOM.All_PACAFs.Any(t => t.Contains(x)))
                {
                    PACAFisAvailable = true;
                }
            }

            if (PACAFisAvailable && GetFiterBehavior == "INCLUSIVE")
            {
                // List of Parts
                // Select Extrusion and Lens
                String currentPart = childNode.Text;
                String currentPartType = ((PartView)childNode.Tag).PartType;
                String currentPartDescription = ((PartView)childNode.Tag).Description;
                bool IsBlank = currentPartDescription.Contains("BLANK");
                bool IsLens = currentPartDescription.Contains("LONG");

                if (currentPartType == "EXTRUSION"  && !IsBlank)
                {// Only Extrusions and Lens are considered. BLANKS will be treated as nnon extrusion
                    EXorLN_Selection(ref partNameToSave, ref partTypeToSave, currentSectionLength, currentPart, currentPartType, finalSectionParts);

                }
                else if (currentPartType == "LENS" && IsLens)
                {
                    EXorLN_Selection(ref partNameToSave, ref partTypeToSave, currentSectionLength, currentPart, currentPartType, finalSectionParts);
                }
                else if (currentPartType == "ENDCAP" || currentPartType == "ENDCAP LENS")
                {
                    EndCapDropLens_Selection(ref partNameToSave, ref partTypeToSave, currentPart, currentPartType, sectionIncreament, finalSectionParts);
                }
                else
                {
                    partNameToSave = currentPart;
                    partTypeToSave = currentPartType;
                    finalSectionParts.Add(String.Format(@"     {0}     {1}     ", partNameToSave, partTypeToSave));
                }
                // new function
                //ring.Format("{0}  {1}, old name, qty);

            }
            else if (PACAFisAvailable && GetFiterBehavior == "EXCLUSIVE")
            {
                // Remove selected part from list
                RemoveLine(finalSectionParts, childNode);
            }
            else
            {
                bool exists = LineExists(childNode.Text);

                if (!exists)
                {
                    unsuitableParts = String.Format(@"{0}: {1}", childNode.Text, "NOT APPLICABLE");
                    noneApplicableParts.Add(unsuitableParts);
                }
            }

        }

        public void Joiner_Qty(String productCode, String partName, ref String partNameToSave, ref String partTypeToSave, ref List<String> finalSectionParts, TreeNode childNode, Int32 sectionIncreament)
        {
            bool IsStart = NewBOM.FixtureConfiguration.Sections.Items[sectionIncreament].IsAtStart;
            bool IsEnd = NewBOM.FixtureConfiguration.Sections.Items[sectionIncreament].IsAtEnd;

            //String GetQuantity = Quantity(productCode, partName);

            String currentPart = String.Format(@"     {0}     {1}     ", childNode.Text, ((PartView)childNode.Tag).PartType);

            if (!IsEnd)
            {
                partNameToSave = childNode.Text;
                partTypeToSave = ((PartView)childNode.Tag).PartType;
                finalSectionParts.Add(currentPart);
            }
        }

        public bool LineExists(String PartName)
        {
            bool exists = false;

            for (int i = 0; i < Log2_RichTextBox.Lines.Count(); i++)
            {
                if (Log2_RichTextBox.Lines[i].Contains(PartName + ": CONTAINS NO RULES"))
                {
                    exists = true;
                    break;
                }
                if (Log2_RichTextBox.Lines[i].Contains(PartName + ": NOT APPLICABLE"))
                {
                    exists = true;
                    break;
                }
            }

            return exists;
        }

        public void EXorLN_Selection(ref String partNameToSave, ref String partTypeToSave, Decimal sectionLength, String currentPart, String currentPartType, List<String> finalSectionParts)
        {
            bool EX_8_FT = currentPart.Contains("-8-");
            bool LN_8_FT = currentPart.Contains("-102-");
            if ((sectionLength > 0 && sectionLength <= 20 || sectionLength > 37 && sectionLength <= 50 || sectionLength > 74 && sectionLength <= 100) && (EX_8_FT || LN_8_FT))
            {
                partNameToSave = currentPart;
                partTypeToSave = currentPartType;
                finalSectionParts.Add(String.Format(@"     {0}     {1}     ", partNameToSave, partTypeToSave));
            }
            else if((sectionLength > 20 && sectionLength <= 37 || sectionLength > 50 && sectionLength <= 74 || sectionLength > 100) && !(EX_8_FT || LN_8_FT))
            {
                partNameToSave = currentPart;
                partTypeToSave = currentPartType;
                finalSectionParts.Add(String.Format(@"     {0}     {1}     ", partNameToSave, partTypeToSave));
            }
            else
            {
                //Do nothing
            }

        }

        public void EndCapDropLens_Selection(ref String partNameToSave, ref String partTypeToSave, String currentPart, String currentPartType, Int32 sectionIncreament, List<String> finalSectionParts)
        {
            if(NewBOM.FixtureConfiguration.Sections.Items[sectionIncreament].IsAtStart || NewBOM.FixtureConfiguration.Sections.Items[sectionIncreament].IsAtEnd)
            {
                partNameToSave = currentPart;
                partTypeToSave = currentPartType;
                finalSectionParts.Add(String.Format(@"     {0}     {1}     ", partNameToSave, partTypeToSave));

            }
            else
            {
                // Do Nothing
            }

        }

        public void RemoveLine(List<String> finalSectionParts, TreeNode childNode)
        {
            foreach (String items in finalSectionParts)
            {
                if (items.Contains(String.Format(@"     {0}     {1}     ", childNode.Text, ((PartView)childNode.Tag).PartType)))
                {
                    //Remove here
                    finalSectionParts.Remove(items);
                }

            }
        }

        public void Quantity(String childNode_ProductCode, String childNode_PartName, String childNode_PartType, ref List<String> finalSectionParts)
        {
            String Part_Quantity = "";

            Part_Quantity = db.PartRulesFilters.Where(o => o.PartName == childNode_PartName && o.FilterTypeID == 3).First().Quantity.ToString();

            List<String> TempList = new List<string>();

            foreach (String item in finalSectionParts)
            {
                if (item.Equals(String.Format(@"     {0}     {1}     ", childNode_PartName, childNode_PartType)))
                {
                    TempList.Add(String.Format(@"       {0}       {1}", item, Part_Quantity));
                    finalSectionParts.Remove(item);
                    finalSectionParts.AddRange(TempList);
                }
            }

            
        }
    }
}

