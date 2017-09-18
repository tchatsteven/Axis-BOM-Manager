using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace BOM_Data_Manager
{
    public partial class MainForm : Form
    {
        BOM_Templates_DataEntities db = new BOM_Templates_DataEntities();
        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            importRootAssemblies();
            showOneColumn(1);
            refreshPartsDataGridView();
            refreshAssembliesDataGridView();
            logMessage("Info", "AXIS BOM Data Manager Loaded.");
        }

        private void importRootAssemblies()
        {
            OrderingCodesEntities1 OCEntities = new OrderingCodesEntities1();
            List<fixture> fixturesList = OCEntities.fixtures.ToList();

            HashSet<String> existingRoots = new HashSet<string>(db.assemblies.Select(o => o.name).ToList());

            fixturesList.RemoveAll(e => existingRoots.Contains(e.code));

            foreach(fixture newFixture in fixturesList)
            {               
                assembly newAssembly = new assembly();
                newAssembly.name = newFixture.code;
                newAssembly.description = newFixture.description;
                newAssembly.isRootAssembly = true;
                newAssembly.id = -1;
                db.assemblies.Add(newAssembly);
                db.SaveChanges();
                logMessage("Info", newFixture.code + " automatically added as ROOT assembly.");
                refreshAssembliesDataGridView();     
            }
            if(fixturesList.Count() == 0) { logMessage("Info", "Root nodes are up to date."); }
        }

        private void logMessage(String MessageType, String Message)
        {
            eventLogTextBox.AppendText( MessageType + ": " + Message + Environment.NewLine );
        }       

        private void showOneColumn(Int32 TargetColumnIndex)
        {
            for(Int32 n = 1; n < tableLayoutPanel3.ColumnCount; n++)
            {
                tableLayoutPanel3.ColumnStyles[n].SizeType = SizeType.Absolute;
                tableLayoutPanel3.ColumnStyles[n].Width = 0;
            }

            tableLayoutPanel3.ColumnStyles[TargetColumnIndex].SizeType = SizeType.Percent;
            tableLayoutPanel3.ColumnStyles[TargetColumnIndex].Width = 100;
        }       

        private void showManagePartsButton_Click(object sender, EventArgs e)
        {
            showOneColumn(1);
            logMessage("Info", "View set to Create/Edit Parts and Assemblies");
        }

        private void showManageAssembliesButton_Click(object sender, EventArgs e)
        {
            showOneColumn(2);
            logMessage("Info", "View set to Assemble Assemblies");
        }

        private void refreshPartsDataGridView()
        {            
            var getData = db.parts.AsEnumerable().Where(o => RegexIsMatch(partsListFilterTextBox.Text, o.name, partsFilterIsCaseSensitive.Checked) || RegexIsMatch(partsListFilterTextBox.Text, o.description, partsFilterIsCaseSensitive.Checked) );
            PartsListDataGridView.DataSource = getData.OrderBy(o => o.name).ToList();
            PartsListDataGridView.Columns.GetLastColumn(DataGridViewElementStates.Visible, DataGridViewElementStates.None).AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            logMessage("Info", "Parts List Refreshed");
        }

        private void refreshAssembliesDataGridView()
        {            
            var getData = db.assemblies.AsEnumerable().Where(o => ( RegexIsMatch(assembliesListFilterTextBox.Text, o.name, assembliesFilterIsCaseSensitive.Checked) || RegexIsMatch(assembliesListFilterTextBox.Text, o.description, assembliesFilterIsCaseSensitive.Checked)) && o.isRootAssembly == showRootAssembliesCheck.Checked);
            AssembliesListDataGridView.DataSource = getData.OrderBy(o=> o.name).ToList();
            AssembliesListDataGridView.Columns.GetLastColumn(DataGridViewElementStates.Visible, DataGridViewElementStates.None).AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            AssembliesListDataGridView.Columns[1].ReadOnly = true;
            disableEnableAssemblyFunctionButtons();
            logMessage("Info", "Assemblies List Refreshed");
        }

        private bool RegexIsMatch(String RegexExpression, String SearchString, Boolean IsCaseSensitive)
        {
            try
            {
                RegexOptions caseSensitivity = IsCaseSensitive ?  RegexOptions.None : RegexOptions.IgnoreCase;
                return new Regex(RegexExpression, caseSensitivity ).Match(SearchString).Success;
            }catch
            {
                return true;
            }
        }

        private void partsListFilterTextBox_TextChanged(object sender, EventArgs e)
        {
            refreshPartsDataGridView();
        }       

        private void deletePartButton_Click(object sender, EventArgs e)
        {
            Int32 selectedRowIndex = PartsListDataGridView.SelectedCells[0].RowIndex;

            DialogResult dialogResult = MessageBox.Show("Are you sure you want to delete part: " + PartsListDataGridView.Rows[selectedRowIndex].Cells[1].Value + " ?", "Confirm part deletion", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {               
                Int32 selectedItemDBIndex = Int32.Parse(PartsListDataGridView.Rows[selectedRowIndex].Cells[0].Value.ToString());
                deletePartFromDB(selectedItemDBIndex);
            }  
        }
        private void deletePartFromDB(Int32 partId)
        {
            db.parts.Remove(db.parts.Find(partId));
            String PartName = db.parts.Find(partId).name;
            db.SaveChanges();           
            logMessage("Info", "Parts " + PartName + " deleted.");
            refreshPartsDataGridView();
        }        

        private void deleteAssemblyFromDB(Int32 assemblyId)
        {
            db.assemblies.Remove(db.assemblies.Find(assemblyId));
            String assemblyName = db.assemblies.Find(assemblyId).name;
            db.SaveChanges();
            logMessage("Info", "Assembly " + assemblyName + " deleted.");
            refreshAssembliesDataGridView();
        }

        private void createNewPartButton_Click(object sender, EventArgs e)
        {
            createEditForm newForm = new createEditForm("part", "new");

            newForm.ShowDialog();
            if (newForm.DialogResult == DialogResult.OK)
            {
                part newPart = new part();
                newPart.name = newForm.newComponentName;
                newPart.description = newForm.newComponentDescription;
                newPart.id = -1;
                db.parts.Add(newPart);
                db.SaveChanges();
                logMessage("Info", newForm.newComponentName + " part added to database.");
                refreshPartsDataGridView();
            }
        }

        private void editPartButton_Click(object sender, EventArgs e)
        {            
            Int32 currentPartId = Int32.Parse(PartsListDataGridView.SelectedCells[0].OwningRow.Cells[0].Value.ToString());
            part partToEdit = db.parts.Find(currentPartId);

            createEditForm newForm = new createEditForm( "part", "edit", partToEdit.name, partToEdit.description);            
            newForm.ShowDialog();

            if (newForm.DialogResult == DialogResult.OK)
            {
                partToEdit.name = newForm.newComponentName;
                partToEdit.description = newForm.newComponentDescription;
                db.SaveChanges();
                logMessage("Info", newForm.newComponentName + " part edited.");
                refreshPartsDataGridView();
            }
        }

        private void partsFilterIsCaseSensitive_CheckedChanged(object sender, EventArgs e)
        {
            refreshPartsDataGridView();
        }

        private void tableLayoutPanel5_Paint(object sender, PaintEventArgs e)
        {

        }

        private void createNewAssemblyButton_Click(object sender, EventArgs e)
        {
            createEditForm newForm = new createEditForm("assembly", "new");

            newForm.ShowDialog();
            if (newForm.DialogResult == DialogResult.OK)
            {
                assembly newAssembly = new assembly();
                newAssembly.name = newForm.newComponentName;
                newAssembly.description = newForm.newComponentDescription;
                newAssembly.id = -1;
                db.assemblies.Add(newAssembly);
                db.SaveChanges();
                logMessage("Info", newForm.newComponentName + " assembly added to database.");
                refreshAssembliesDataGridView();
            }
        }

        private void assembliesListFilterTextBox_TextChanged(object sender, EventArgs e)
        {
            refreshAssembliesDataGridView();
        }

        private void assembliesFilterIsCaseSensitive_CheckedChanged(object sender, EventArgs e)
        {
            refreshAssembliesDataGridView();
        }

        private void EditAssemblyButton_Click(object sender, EventArgs e)
        {
            Int32 currentAssemblyId = Int32.Parse(AssembliesListDataGridView.SelectedCells[0].OwningRow.Cells[0].Value.ToString());
            assembly assemblyToEdit = db.assemblies.Find(currentAssemblyId);

            createEditForm newForm = new createEditForm("assembly", "edit", assemblyToEdit.name, assemblyToEdit.description);
            newForm.ShowDialog();

            if (newForm.DialogResult == DialogResult.OK)
            {
                assemblyToEdit.name = newForm.newComponentName;
                assemblyToEdit.description = newForm.newComponentDescription;
                db.SaveChanges();
                logMessage("Info", newForm.newComponentName + " assembly edited.");
                refreshAssembliesDataGridView();
            }
        }

        private void DeleteAssemblyButton_Click(object sender, EventArgs e)
        {
            String selectionName = AssembliesListDataGridView.SelectedCells[0].OwningRow.Cells[2].Value.ToString();           

            DialogResult dialogResult = MessageBox.Show("Are you sure you want to delete assembly: " + selectionName + " ?", "Confirm assembly deletion", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                Int32 selectedItemDBIndex = Int32.Parse(AssembliesListDataGridView.SelectedCells[0].OwningRow.Cells[0].Value.ToString());
                deleteAssemblyFromDB(selectedItemDBIndex);
            }
        }

        private void AssembliesListDataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            disableEnableAssemblyFunctionButtons();
        }

        private void disableEnableAssemblyFunctionButtons()
        {
            try
            {
                if ((bool)AssembliesListDataGridView.SelectedCells[0].OwningRow.Cells[1].Value.Equals(true))
                {
                    EditAssemblyButton.Enabled = false;
                    DeleteAssemblyButton.Enabled = false;
                }
                else
                {

                    EditAssemblyButton.Enabled = true;
                    DeleteAssemblyButton.Enabled = true;
                }
            }
            catch { }
        }

        private void showRootAssembliesCheck_CheckedChanged(object sender, EventArgs e)
        {
            refreshAssembliesDataGridView();
        }       

        private void tableLayoutPanel1_Click(object sender, EventArgs e)
        {
            logMessage("Mr. Clicky", "Feeling Clicky?");
        }
    }
}
