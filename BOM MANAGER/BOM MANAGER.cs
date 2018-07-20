using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        AXIS_AutomationEntitiesBOM db;
        PartRule newPartRule;
        //_FixtureConfiguration NewFixture;
        //List<String> All_PACAFs;
        _BOM NewBOM;
        //_Sections NewSections;
        //_Section NewSection;

        public BOM_MANAGER()
        {
            InitializeComponent();
            db = new AXIS_AutomationEntitiesBOM();
            BomManagerFormMsg = new _RTFMessenger(eventLog_richTextBox, 0, true) { DefaulSpaceAfter = 0 };
            PartRules = new _RTFMessenger(eventLog_PR_richTextBox, 0, true) { DefaulSpaceAfter = 0 };
            PartGen = new _RTFMessenger(Log_RichTextBox, 0, true) { DefaulSpaceAfter = 0 };
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
                PartFilterCheckBox.Checked = false;
                Load_Category_ComboBox();
                Load_Parameter_ComboBox();
                Load_Filter_Behavior();
                RefreshDataGridView_PartRules();
                DisableButton();

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

                List<PartRule> deletionTargetRulesList = db.PartRules.Where(o => o.ProductCode == currentFixtureID && o.PartID == currentPartRefID).ToList();

                foreach (var test in deletionTargetRulesList)
                {
                    Int32 deletionIndex = Int32.Parse(test.id.ToString());
                    db.PartRules.Remove(db.PartRules.Find(deletionIndex));
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

                List<PartRule> deletionTargetRulesList = db.PartRules.Where(o => o.ProductCode == currentFixtureID && o.PartID == currentPartRefID).ToList();

                foreach (var test in deletionTargetRulesList)
                {
                    Int32 deletionIndex = Int32.Parse(test.id.ToString());
                    db.PartRules.Remove(db.PartRules.Find(deletionIndex));
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

        public String MySelectedFixture => db.PartRules.Find(CurrentPartId).PartName;

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

                    //TreeNode Assy_RootNode_PartGen = AssembliesDict.Where(o => ((AssemblyView)o.Value.Tag).ParentID == null).First().Value;
                    //Fixture_treeView_PartGen.Nodes.Add(Assy_RootNode_PartGen);
                    //Fixture_treeView_PartGen.ExpandAll();

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






        public String PartName => db.PartRules.Find(CurrentPartId).PartName;

        public Int32 CurrentPartId => Int32.Parse(DataGridView_Rules.SelectedCells[0].OwningRow.Cells[0].Value.ToString());

        public PartRule PartRuleToEdit => db.PartRules.Find(CurrentPartId);

        public String EditPartName => PartRuleToEdit.PartName;

        public String EditProductCode => PartRuleToEdit.ProductCode;

        public String EditCategoryName => PartRuleToEdit.CategoryName;

        public String EditParameterName => PartRuleToEdit.ParameterName;

        public Int32 EditQty => PartRuleToEdit.Quantity ?? 0;

        public Int32 EditPACAF_Id => PartRuleToEdit.PACAF_ID.Value;

        public String EditFilterRule => PartRuleToEdit.FirstFilterDependencyName;

        public Int32 EditProductCodeIndex => PartRuleToEdit.ProductID ?? 0;











        private void ProductID_comboBox_PR_SelectionChangeCommitted(object sender, EventArgs e)
        {
            RefreshTreeView();
            Load_Category_ComboBox();
            Load_Parameter_ComboBox();
            RefreshDataGridView_PartRules();
            DisableButton();
        }

        private void RefreshDataGridView_PartRules()
        {
            try
            {
                DataGridView_Rules.DataSource = db.PartRules.Where(o => o.ProductCode == productID_comboBox_PR.Text).OrderBy(m => m.PartName).ToList();

                DataGridView_Rules.Columns[0].Visible = false;
                DataGridView_Rules.Columns[1].Visible = false;
                DataGridView_Rules.Columns[3].Visible = false;
                DataGridView_Rules.Columns[5].Visible = false;
                DataGridView_Rules.Columns[7].Visible = false;
                DataGridView_Rules.Columns[13].Visible = false;
                DataGridView_Rules.Columns[14].Visible = false;

                DataGridView_Rules.AutoResizeColumns();
                DataGridView_Rules.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

                if (PartFilterCheckBox.Checked is true)
                {
                    DataGridView_Rules.DataSource = db.PartRules.Where(o => o.PartName == Fixture_treeView_PartRule.SelectedNode.Text).OrderBy(m => m.PartName).ToList();

                    DataGridView_Rules.Columns[0].Visible = false;
                    DataGridView_Rules.Columns[1].Visible = false;
                    DataGridView_Rules.Columns[3].Visible = false;
                    DataGridView_Rules.Columns[5].Visible = false;
                    DataGridView_Rules.Columns[7].Visible = false;
                    DataGridView_Rules.Columns[13].Visible = false;
                    DataGridView_Rules.Columns[14].Visible = false;

                    DataGridView_Rules.AutoResizeColumns();
                    DataGridView_Rules.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                }
                else
                {
                    //eventLog_PR_richTextBox.Clear();
                }

                BomManagerFormMsg.NewMessage().AddText("Assembly Table has been Populated").PrependMessageType().Log();
                eventLog_richTextBox.ScrollToCaret();

            }
            catch (System.Reflection.TargetException ex)
            {
                //eventLog_PR_richTextBox.Clear();
                PartRules.NewMessage().AddText("No Part Selected in TreeView. ").AddText(ex.Message).IsError().PrependMessageType().Log();
            }
            catch
            {
                //eventLog_PR_richTextBox.Clear();
                BomManagerFormMsg.NewMessage().AddText("Refreshing Assembly Table failed").PrependMessageType().Log();
            }
        }

        private void DisableButton()
        {
            if (DataGridView_Rules.Rows.Count == 0)
            {
                Edit_PR.Enabled = false;
            }
            else
            {
                Edit_PR.Enabled = true;
            }
        }

        private void Load_Category_ComboBox()
        {
            String productId_PR = productID_comboBox_PR.SelectedValue.ToString();
            CategoryPR_comboBox.DataSource = db.CategoryAtFixtureViews.AsNoTracking().Where(o => o.FixtureId.ToString() == productId_PR && o.Name != "PRODUCT ID").OrderBy(o => o.DisplayOrder).ToList();
            CategoryPR_comboBox.ValueMember = "CategoryId";
            CategoryPR_comboBox.DisplayMember = "Name";

            CategoryPR_comboBoxFilter2.DataSource = db.CategoryAtFixtureViews.AsNoTracking().Where(o => o.FixtureId.ToString() == productId_PR && o.Name != "PRODUCT ID").OrderBy(o => o.DisplayOrder).ToList();
            CategoryPR_comboBoxFilter2.ValueMember = "CategoryId";
            CategoryPR_comboBoxFilter2.DisplayMember = "Name";

        }

        private void Load_Parameter_ComboBox()
        {
            String productId_PR = productID_comboBox_PR.SelectedValue.ToString();
            String categoryId_PR1 = CategoryPR_comboBox.SelectedValue.ToString();
            String categoryId_PR2 = CategoryPR_comboBoxFilter2.SelectedValue.ToString();

            ParameterPR_comboBox.DataSource = db.ParameterAtCategoryAtFixtureViews.Where(o => o.id.ToString() == productId_PR && o.CategoryId.ToString() == categoryId_PR1 && o.ParameterCode != null).Distinct().OrderBy(o => o.DisplayOrder_Id).ToList();
            ParameterPR_comboBox.ValueMember = "ParameterId";
            ParameterPR_comboBox.DisplayMember = "ParameterCode";

            ParameterPR_comboBoxFilter2.DataSource = db.ParameterAtCategoryAtFixtureViews.Where(o => o.id.ToString() == productId_PR && o.CategoryId.ToString() == categoryId_PR2 && o.ParameterCode != null).Distinct().OrderBy(o => o.DisplayOrder_Id).ToList();
            ParameterPR_comboBoxFilter2.ValueMember = "ParameterId";
            ParameterPR_comboBoxFilter2.DisplayMember = "ParameterCode";

        }

        private void Load_Filter_Behavior()
        {
            FilterBehavior_ComboBox.DataSource = db.FilterBehaviors.ToList();
            FilterBehavior_ComboBox.ValueMember = "id";
            FilterBehavior_ComboBox.DisplayMember = "Behavior";

        }

        private void ComboBoxCategory_PR_SelectionChangeCommitted(object sender, EventArgs e)
        {
            Load_Parameter_ComboBox();

        }

        private void CategoryPR_comboBoxFilter2_SelectionChangeCommitted(object sender, EventArgs e)
        {
            Load_Parameter_ComboBox();

        }

        private void Fixture_treeView_PR_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (PartFilterCheckBox.Checked is true)
            {
                RefreshDataGridView_PartRules();

            }
        }

        private void PartFilterCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            RefreshDataGridView_PartRules();
        }

        private void Save_PR_Click(object sender, EventArgs e)
        {
            try
            {
                String selectedPartName = Fixture_treeView_PartRule.SelectedNode.Text;
                Int32 partNameIndex = Int32.Parse(db.Parts.Where(m => m.PartName == selectedPartName).First().id.ToString());

                String selectedCategory = CategoryPR_comboBox.Text;
                Int32 selectedCategoryIndex = Int32.Parse(CategoryPR_comboBox.SelectedValue.ToString());

                String selectedProductId = productID_comboBox_PR.Text;
                Int32 selectedProductIdIndex = Int32.Parse(productID_comboBox_PR.SelectedValue.ToString());

                String selectedParameter = ParameterPR_comboBox.Text;
                Int32 SelectedParameterIndex = Int32.Parse(ParameterPR_comboBox.SelectedValue.ToString());

                Int32 selectedFilterID = Int32.Parse(FilterBehavior_ComboBox.SelectedValue.ToString());
                String selectedFilterName = FilterBehavior_ComboBox.Text;

                Int32 PACAFid = Int32.Parse(db.ParameterAtCategoryAtFixtureViews.Where(o => o.FixtureId == selectedProductIdIndex && o.CategoryId == selectedCategoryIndex && o.ParameterId == SelectedParameterIndex).First().ParAtCatAtFix_ID.ToString());

                PartRule newPartRule = new PartRule();

                newPartRule.PartName = selectedPartName;
                newPartRule.PartID = partNameIndex;

                newPartRule.CategoryName = selectedCategory;
                newPartRule.CategoryID = selectedCategoryIndex;

                newPartRule.ProductCode = selectedProductId;
                newPartRule.ProductID = selectedProductIdIndex;

                newPartRule.ParameterName = selectedParameter;
                newPartRule.ParameterID = SelectedParameterIndex;

                newPartRule.FirstFilterDependencyID = selectedFilterID;
                newPartRule.FirstFilterDependencyName = selectedFilterName;

                newPartRule.PACAF_ID = PACAFid;
                newPartRule.Quantity = Int32.Parse(Qty_NumericUpDown.Value.ToString());

                db.PartRules.Add(newPartRule);
                db.SaveChanges();
                RefreshDataGridView_PartRules();


                PartRules.NewMessage().AddText("Part Rules for  " + selectedProductId + ",  " + selectedCategory + ",  " + selectedParameter + ",  " + selectedPartName + "  added to Database").PrependMessageType().Log();
            }

            catch (NullReferenceException ex)
            {

                PartRules.NewMessage().AddText("No Part Selected in TreeView. ").AddText(ex.Message).IsError().PrependMessageType().Log();

            }

            catch (InvalidOperationException ex1)
            {
                PartRules.NewMessage().AddText("Invalid Part Selected (TreeView Parent cannot be Selected). ").AddText(ex1.Message).IsError().PrependMessageType().Log();

            }

        }

        private void Edit_PR_Click(object sender, EventArgs e)
        {
            ////String editPartName = "";
            ////String editProductCode = "";
            ////String editCategoryName = "";
            ////String editParameterName = "";
            ////String editFilterRule;

            ////Int32 editPACAF_Id ;
            ////Int32 editQty;
            ////Int32 editProductCodeIndex;

            //try
            //{
            //    //Int32 currentPartId = Int32.Parse(dataGridView_Rules.SelectedCells[0].OwningRow.Cells[0].Value.ToString());
            //    //PartRule partRuleToEdit = db.PartRules.Find(currentPartId);//.Find(currentPartId);

            //    //editPartName = partRuleToEdit.PartName;
            //    //editProductCode = partRuleToEdit.ProductCode;
            //    //editCategoryName = partRuleToEdit.CategoryName;
            //    //editParameterName = partRuleToEdit.ParameterName;
            //    //editPACAF_Id = partRuleToEdit.PACAF_ID.Value;

            //    //editQty = partRuleToEdit.Quantity ?? 0;
            //    //editFilterRule = partRuleToEdit.FirstFilterDependencyName;
            //    //editProductCodeIndex = partRuleToEdit.ProductID ??0;


            //}

            //catch (System.ArgumentOutOfRangeException ex1)
            //{
            //    PartRules.NewMessage().AddText("No data In the DataGridView for Current Selected Fixture. ").AddText(ex1.Message).IsError().PrependMessageType().Log();
            //    return;
            //}


            //Rules_EditForm editPartRulesForm = new Rules_EditForm(editPartName, editProductCode, editCategoryName, editParameterName, editPACAF_Id, editQty, editFilterRule, editProductCodeIndex);
            Rules_EditForm editPartRulesForm = new Rules_EditForm(this);


            DialogResult NewForm = editPartRulesForm.ShowDialog();
            //editPartRulesForm.PartName;
            try
            {


                if (NewForm == DialogResult.OK)
                {
                    PartRules.NewMessage().AddText("Part Rule Successfully Edited").PrependMessageType().Log();

                    db = new AXIS_AutomationEntitiesBOM();
                    RefreshDataGridView_PartRules();

                }
                else if (NewForm == DialogResult.Cancel)
                {
                    PartRules.NewMessage().AddText("Editing Part Rule Cancelled").PrependMessageType().Log();
                }
                eventLog_richTextBox.ScrollToCaret();
            }
            catch
            {
                PartRules.NewMessage().AddText("Editing Part Failed").IsError().PrependMessageType().Log();
            }
        }

        private void Delete_PR_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow currentDeletionRow in DataGridView_Rules.SelectedRows)
            {
                try
                {
                    Int32 currentPartId = Int32.Parse(currentDeletionRow.Cells["id"].Value.ToString());
                    String partyNameToDelete = currentDeletionRow.Cells["PartName"].Value.ToString();

                    db.PartRules.Remove(db.PartRules.Find(currentPartId));
                    db.SaveChanges();

                    PartRules.NewMessage().AddText("Part: " + partyNameToDelete + " has been deleted from Database ").PrependMessageType().Log();

                }
                catch
                {
                    PartRules.NewMessage().AddText("Part has not been deleted from Database").IsError().PrependMessageType().Log();

                }
            }
            RefreshDataGridView_PartRules();

        }

        private void RefreshTablePR_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow PACAF_ROW in DataGridView_Rules.Rows)
            {
                //if ( PACAF_ROW.Cells["PACAF_ID"] == null)
                //{
                //}
                try
                {
                    String selectedPartName = PACAF_ROW.Cells["PartName"].Value.ToString();
                    Int32 partNameIndex = Int32.Parse(db.Parts.Where(m => m.PartName == selectedPartName).First().id.ToString());

                    Int32 selectedCategoryIndex = Int32.Parse(PACAF_ROW.Cells["CategoryID"].Value.ToString());
                    String selectedCategory = PACAF_ROW.Cells["CategoryName"].Value.ToString();

                    Int32 selectedProductIdIndex = Int32.Parse(PACAF_ROW.Cells["ProductID"].Value.ToString());
                    String selectedProductId = PACAF_ROW.Cells["ProductCode"].Value.ToString();

                    Int32 SelectedParameterIndex = Int32.Parse(PACAF_ROW.Cells["ParameterID"].Value.ToString());
                    String selectedParameter = PACAF_ROW.Cells["ParameterName"].Value.ToString();

                    Int32 PACAF_ID = Int32.Parse(db.ParameterAtCategoryAtFixtureViews.Where(o => o.FixtureId == selectedProductIdIndex && o.CategoryId == selectedCategoryIndex && o.ParameterId == SelectedParameterIndex).First().ParAtCatAtFix_ID.ToString());

                    Int32 RowID = Int32.Parse(PACAF_ROW.Cells["id"].Value.ToString());

                    newPartRule = db.PartRules.Where(o => o.id == RowID).First();
                    newPartRule.PACAF_ID = PACAF_ID;

                    db.SaveChanges();

                    PartRules.NewMessage().AddText("PACAF for  " + selectedProductId + ",  " + selectedCategory + ",  " + selectedParameter + ",  " + selectedPartName + "  added to Database").PrependMessageType().Log();
                }

                catch
                {

                    PartRules.NewMessage().AddText("No PACAF entered ").IsError().PrependMessageType().Log();

                }
            }
            RefreshDataGridView_PartRules();
        }
       
        private void AdditionalFilter_CheckBox_CheckedChanged(object sender, EventArgs e)
        {




        }

        private void DataGridView_Rules_SelectionChanged(object sender, EventArgs e)
        {



        }



        /****************************************************************************************************************************************************************/
        /*                                                                                                                                                              */
        /*                                                              PART Gen Tester TAB                                                                                  */
        /*                                                                                                                                                              */
        /****************************************************************************************************************************************************************/



        //public String _FixtureSetupCode => FixtureSetupCode_TextBox.Text;
        public String _FixtureSetupCode
        {
            get
            {
                return FixtureSetupCode_TextBox.Text;
            }
        }

         

        //public string getselectedproductid_category => newfixture.selection.productid.selectionbasevalue;
        //public string getselectedlumen_category => newfixture.selection.lumensdirect.selectionbasevalue;
        //public string getselectedcri_category => newfixture.selection.cri.selectionbasevalue;
        //public string getselectedcolortemp_category => newfixture.selection.colortemperature.selectionbasevalue;
        //public string getselectedlength_category => newfixture.selection.length.selectionbasevalue;
        //public string getselectedfinish_category => newfixture.selection.finish.selectionbasevalue;
        //public string getselectedvoltage_category => newfixture.selection.voltage.selectionbasevalue;
        //public string getselecteddriver_category => newfixture.selection.driver.selectionbasevalue;
        //public string getselectedcircuits_category => newfixture.selection.circuits.selectionbasevalue;
        //public string getselectedmounting_category => newfixture.selection.mounting.selectionbasevalue;
        //public string getselectedbattery_category => newfixture.selection.battery.selectionbasevalue;
        //public string getselectedother_category => newfixture.selection.other.selectionbasevalue;
        //public string getselectediccontrol_category => newfixture.selection.ic.selectionbasevalue;
        //public string getselectedcustom_category => newfixture.selection.custom.selectionbasevalue;

        //public Int32 getSelectedProductID_PACAF => Int32.Parse(NewFixture.Selection.ProductID.SelectionPACAF);
        //public Int32 getSelectedLumen_PACAF => Int32.Parse(NewFixture.Selection.LumensDirect.SelectionPACAF);
        //public Int32 getSelectedCRI_PACAF => Int32.Parse(NewFixture.Selection.CRI.SelectionPACAF);
        //public Int32 getSelectedColorTemp_PACAF => Int32.Parse(NewFixture.Selection.ColorTemperature.SelectionPACAF);
        //public Int32 getSelectedLength_PACAF => Int32.Parse(NewFixture.Selection.Length.SelectionPACAF);
        //public Int32 getSelectedFinish_PACAF => Int32.Parse(NewFixture.Selection.Finish.SelectionPACAF);
        //public Int32 getSelectedVoltage_PACAF => Int32.Parse(NewFixture.Selection.Voltage.SelectionPACAF);
        //public Int32 getSelectedDriver_PACAF => Int32.Parse(NewFixture.Selection.Driver.SelectionPACAF);
        //public Int32 getSelectedCircuits_PACAF => Int32.Parse(NewFixture.Selection.Circuits.SelectionPACAF);
        //public Int32 getSelectedMounting_PACAF => Int32.Parse(NewFixture.Selection.Mounting.SelectionPACAF);
        //public Int32 getSelectedBattery_PACAF => Int32.Parse(NewFixture.Selection.Battery.SelectionPACAF);
        //public Int32 getSelectedOther_PACAF => Int32.Parse(NewFixture.Selection.Other.SelectionPACAF);
        //public Int32 getSelectedICControl_PACAF => Int32.Parse(NewFixture.Selection.IC.SelectionPACAF);
        //public Int32 getSelectedCustom_PACAF => Int32.Parse(NewFixture.Selection.Custom.SelectionPACAF);



        
        private void GetPart_Button_Click(object sender, EventArgs e)
        {
           
            Log_RichTextBox.Clear();

            // _Fixture NewFixture = new _Fixture(_FixtureSetupCode);
            NewBOM = new _BOM(_FixtureSetupCode);
                        
            NewBOM.TestconfigSection(PartGen);
            NewBOM.GetOrderingCodePACAFs();

            PartGen.NewMessage().SetSpaceAfter(0).AddText("Bill Of Material selected Code: ").AddBoldText(_FixtureSetupCode).Log();


            int totalSections = NewBOM.FixtureConfiguration.Sections.Count;

            for (int sectionIncreament = 0; sectionIncreament < totalSections; sectionIncreament++)
            {
                //Function to go through treeview and add that to thr RichText Box

                NewBOM.SummarizeBOMinIntoRTB(PartGen, sectionIncreament);
                FirstFilter(PartGen);

            }



            Log_RichTextBox.SelectionStart = 0;
            Log_RichTextBox.ScrollToCaret();
        }

        private void Get_Template_Button_Click(object sender, EventArgs e)
        {
            Log_RichTextBox.Clear();

            NewBOM = new _BOM(_FixtureSetupCode);
            NewBOM.TestconfigSection(PartGen);
            // _FixtureConfiguration NewFixture = new _FixtureConfiguration(_FixtureSetupCode);
            NewBOM.FixtureConfiguration.ConfigureClientRequest();
            NewBOM.FixtureConfiguration.ConfigureSections();
            NewBOM.FixtureConfiguration.ConfigureCoverElements();

            NewBOM.FixtureConfiguration.CustomerRequest.Template.SummarizeIntoRTB(PartGen);

            Log_RichTextBox.SelectionStart = 0;
            Log_RichTextBox.ScrollToCaret();
        }

        private void Match_Summary_Button_Click(object sender, EventArgs e)
        {
            _FixtureConfiguration NewFixture = new _FixtureConfiguration(_FixtureSetupCode);
            NewFixture.ConfigureClientRequest();
            NewFixture.ConfigureSections();
            NewFixture.ConfigureCoverElements();

            Log_RichTextBox.Clear();
            NewFixture.CustomerRequest.Template.SummarizeMatchesIntoRTB(PartGen);

            Log_RichTextBox.SelectionStart = 0;
            Log_RichTextBox.ScrollToCaret();

        }

        private void Solve_Mechanical_Button_Click(object sender, EventArgs e)
        {
            _FixtureConfiguration NewFixture = new _FixtureConfiguration(_FixtureSetupCode);
            NewFixture.ConfigureClientRequest();
            NewFixture.ConfigureSections();
            NewFixture.ConfigureCoverElements();

            Log_RichTextBox.Clear();
            NewFixture.Sections.SummarizeMechanicalIntoRTB(PartGen);

            Log_RichTextBox.SelectionStart = 0;
            Log_RichTextBox.ScrollToCaret();
        }

        public void FirstFilter(_RTFMessenger Messenger)
        {

            foreach (TreeNode ParentNode in Fixture_treeView_PartGen.Nodes)
            {
                List<String> PartNameList = new List<String>();

                foreach (TreeNode childNode in ParentNode.Nodes)
                {

                    try
                    {   
                        /**/
                        String currentPartName = childNode.Text;
                        String currentProductId = ParentNode.Text;

                        /**/
                        //Int32 Current_PACAF = Int32.Parse(db.PartRules.Where(o => o.PartName == currentPartName).First().PACAF_ID.ToString());
                        List<String> Current_PACAFs = db.PartRules.Where(o=>o.PartName == currentPartName && o.ProductCode == currentProductId).Select(p=>p.PACAF_ID.ToString()).ToList();

                        /*Filter*/
                        Boolean PACAFisAvailable = NewBOM.All_PACAFs.Any(t => Current_PACAFs.Contains(t));
                        String GetFiterBehavior = db.PartRules.Where(o => o.PartName == currentPartName).First().FirstFilterDependencyName.ToString();
                        String GetQuantity = db.PartRules.Where(o => o.PartName == currentPartName).First().Quantity.ToString(); ;

                        if (PACAFisAvailable && GetFiterBehavior == "INCLUSIVE")
                        {
                            PartNameList.Add(childNode.Text);
                            Messenger.NewMessage().AddText(String.Format(@"     {0}          {1}          {2}", childNode.Text, ((PartView)childNode.Tag).PartType, GetQuantity)).Log();

                        }

                    }
                    catch
                    {

                    }

                }

            }
        }

    }
}

 