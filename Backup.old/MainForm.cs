using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace BOM_Data_Manager
{
    public partial class MainForm : Form
    {
        BOM_Templates_DataEntities db = new BOM_Templates_DataEntities();

        CancellationTokenSource keyStrokeStopTimer = new CancellationTokenSource();

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

            foreach (fixture newFixture in fixturesList)
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
            if (fixturesList.Count() == 0) { logMessage("Info", "Root nodes are up to date."); }
        }

        private void logMessage(String MessageType, String Message)
        {
            eventLogTextBox.AppendText(MessageType + ": " + Message + Environment.NewLine);
        }

        private void showOneColumn(Int32 TargetColumnIndex)
        {
            for (Int32 n = 1; n < tableLayoutPanel3.ColumnCount; n++)
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
            refreshTargetAssemblSelectionDataGridView();
            showOneColumn(2);
            logMessage("Info", "View set to Assemble Assemblies");
        }

        private void showAssembleBOMTemplatesPage_Click(object sender, EventArgs e)
        {
            showOneColumn(3);
            logMessage("Info", "View set to Assemble BOM Templates");
        }

        private void refreshPartsDataGridView()
        {
            var getData = db.parts.AsEnumerable().Where(o => RegexIsMatch(partsListFilterTextBox.Text, o.name, partsFilterIsCaseSensitive.Checked) || RegexIsMatch(partsListFilterTextBox.Text, o.description, partsFilterIsCaseSensitive.Checked));

            PartsListDataGridView.DataSource = getData.OrderBy(o => o.name).ToList();
            PartsListDataGridView.Columns.GetLastColumn(DataGridViewElementStates.Visible, DataGridViewElementStates.None).AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            logMessage("Info", "Parts List Refreshed");
        }

        private void refreshAssembliesDataGridView()
        {
            var getData = db.assemblies.AsEnumerable().Where(o => (RegexIsMatch(assembliesListFilterTextBox.Text, o.name, assembliesFilterIsCaseSensitive.Checked) || RegexIsMatch(assembliesListFilterTextBox.Text, o.description, assembliesFilterIsCaseSensitive.Checked)) && o.isRootAssembly == showRootAssembliesCheck.Checked);
            AssembliesListDataGridView.DataSource = getData.OrderBy(o => o.name).ToList();
            AssembliesListDataGridView.Columns.GetLastColumn(DataGridViewElementStates.Visible, DataGridViewElementStates.None).AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            AssembliesListDataGridView.Columns[1].ReadOnly = true;
            disableEnableAssemblyFunctionButtons();
            logMessage("Info", "Assemblies List Refreshed");
        }

        //private void typeToFilterDelayForAssembleAssembliesTargetList()
        //{
        //    Task.Delay(1000).ContinueWith(t => MessageBox.Show("refreshTargetAssemblSelectionDataGridView"));
        //}

        //private 

        private void refreshTargetAssemblSelectionDataGridView()
        {
            var getData = db.assemblies.AsEnumerable().Where(o => (RegexIsMatch(selectTargetAssemblyFilterTextBox.Text, o.name, selectTargetAssemblyIsCaseSensitive.Checked) || RegexIsMatch(selectTargetAssemblyFilterTextBox.Text, o.description, selectTargetAssemblyIsCaseSensitive.Checked)) && o.isRootAssembly != true);

            //targetAssemblySelectionDataGridView.DataSource = getData.OrderBy(o => o.name).ToList();
            //targetAssemblySelectionDataGridView.Columns.GetLastColumn(DataGridViewElementStates.Visible, DataGridViewElementStates.None).AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            // targetAssemblySelectionDataGridView.Columns[1].Visible = false;
            logMessage("Info", "Assembly Selection List Refreshed");
            listBox2.DataSource = getData.Select(n => n.name + " - " + n.description).ToList();
        }

        private bool RegexIsMatch(String RegexExpression, String SearchString, Boolean IsCaseSensitive)
        {
            try
            {
                RegexOptions caseSensitivity = IsCaseSensitive ? RegexOptions.None : RegexOptions.IgnoreCase;
                return new Regex(RegexExpression, caseSensitivity).Match(SearchString).Success;
            }
            catch
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

            createEditForm newForm = new createEditForm("part", "edit", partToEdit.name, partToEdit.description);
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

        private void selectTargetAssemblyFilterTextBox_TextChanged(object sender, EventArgs e)
        {
            refreshTargetAssemblSelectionDataGridView();
        }
    }

} 

//namespace FixtureSetupCodeParser
//    {
//        static class ParameterCustomType
//        {
//            static public String Get(String FullParameterCode)
//            {
//                try
//                {
//                    FixtureSetupCodeParserDbData DbData = new FixtureSetupCodeParserDbData();
//                    String CustomParameterString = new Regex(@"\(([^()]+)\)$").Match(FullParameterCode).Groups[1].Value.ToString();
//                    return DbData.ParameterCustomValueTypes.Where(o => o.Code == CustomParameterString).FirstOrDefault().ValueType.ToString();
//                }
//                catch
//                {
//                    return null;
//                }
//            }
//        }

//        public class _ProductTemplate
//        {
//            String _ProductCode;
//            List<Category> _Categories = new List<Category>();
//            FixtureSetupCodeParserDbData DbData = new FixtureSetupCodeParserDbData();
//            _FixtureSetupCodeBreakdown FixtureSetupCodeBreakdown;
//            public static List<_RTFMessage> ErrorMessages = new List<_RTFMessage>();
//            public _ProductTemplate(_FixtureSetupCodeBreakdown _fixtureSetupCodeBreakdown)
//            {
//                FixtureSetupCodeBreakdown = _fixtureSetupCodeBreakdown;
//                _ProductCode = FixtureSetupCodeBreakdown.ProductCode;
//                InitTemplate();
//            }

//            public String ProductCode
//            {
//                get { return _ProductCode; }
//            }

//            public Int32? ProductID
//            {
//                get
//                {
//                    try
//                    {
//                        return DbData.Fixtures.Where(o => o.Code == ProductCode).First().id;
//                    }
//                    catch
//                    {
//                        return null;
//                    }
//                }
//            }

//            public String Directionality
//            {
//                get { return DbData.Fixtures.Where(o => o.Code == ProductCode).First().Directionality; }
//            }

//            public String FixtureApplicationType
//            {
//                get
//                {
//                    try { return DbData.Fixtures.Where(o => o.Code == ProductCode).First().ApplicationType; }
//                    catch { return null; }
//                }
//            }

//            public String GouverningProject
//            {
//                get
//                {
//                    try
//                    {
//                        return DbData.Fixtures.Where(o => o.Code == ProductCode).First().DWGouverningProject;
//                    }
//                    catch
//                    {
//                        return null;
//                    }
//                }
//            }

//            public Boolean IsAvailable
//            {
//                get { return DbData.Fixtures.Any(o => o.Code == ProductCode); }
//            }

//            public Boolean IsEnabled
//            {
//                get { return DbData.Fixtures.Where(o => o.Code == ProductCode).First().IsDWEnabled; }
//            }

//            public List<Category> Categories
//            {
//                get { return _Categories; }
//            }

//            private void InitTemplate()
//            {
//                //get data from DB
//                List<ProductTemplate> ProductTemplateData = DbData.ProductTemplates.AsNoTracking().Where(o => o.FixtureCode == ProductCode).OrderBy(o => o.CAF_DisplayOrder).ThenBy(o => o.PAC_DisplayOrder).ToList();

//                foreach (ProductTemplate CurrentRow in ProductTemplateData)
//                {
//                    Int32 CurrentRowCategoryIndex = (Int32)CurrentRow.CAF_DisplayOrder - 1;
//                    Category CurrentCategory = null;

//                    try
//                    {
//                        CurrentCategory = Categories[CurrentRowCategoryIndex];//set currentCategory
//                    }
//                    catch
//                    {
//                        CurrentCategory = new Category(CurrentRow, this); //create new if try portion fails
//                        Categories.Add(CurrentCategory);  //set current as new
//                    }

//                    CurrentCategory.AddParameter(CurrentRow);
//                }

//                //Insert SP category at Display Order 0 this way the matching has Automatic categories to deal with
//                if (IsSP)
//                {
//                    Category SpCategory = new Category(SPNumber, this);
//                    Categories.Insert(0, SpCategory);
//                }
//            }

//            private Boolean IsSP
//            {
//                get { return FixtureSetupCodeBreakdown.IsSP; }
//            }

//            private String SPNumber
//            {
//                get { return FixtureSetupCodeBreakdown.SPNumber; }
//            }

//            public void SummarizeIntoRTB(_RTFMessenger messenger)
//            {
//                messenger.NewMessage().AddText(@"Fixture Setup Code Template for ").AddBoldText(ProductCode).AddText(@" is:").Log();

//                foreach (Category category in Categories)
//                {
//                    category.SummarizeIntoRTB(messenger);
//                }
//            }

//            public void SummarizeMatchesIntoRTB(_RTFMessage messenger)
//            {
//                foreach (Category category in Categories)
//                {
//                    category.SummarizeMatchedParameters(messenger);
//                }
//            }

//            public class Category
//            {
//                ProductTemplate Data;
//                public List<Parameter> TemplateParameters = new List<Parameter>();
//                public List<Parameter> SelectedParameters = new List<Parameter>();
//                private List<_RTFMessage> ErrorMessages = new List<_RTFMessage>();
//                _ProductTemplate productTemplate = null;

//                public Category(ProductTemplate data, _ProductTemplate fixutureReference)
//                {
//                    Data = data;
//                    productTemplate = fixutureReference;
//                }

//                public Category(String SPNumber, _ProductTemplate fixutureReference)
//                {
//                    Data = new ProductTemplate()
//                    {
//                        CategoryName = "SP",
//                        ParameterDescription = String.Format(@"Special order {0}", SPNumber),
//                        ParameterCode = String.Format(@"SP{0}", SPNumber),
//                        CategoryIsMultiselect = false,
//                        CategoryIsOptional = false,
//                        CategoryIsObsolete = false,
//                        CategoryType = "SP",
//                        CAF_DisplayOrder = 0,
//                        PAC_DisplayOrder = 0
//                    };

//                    AddParameter(Data);
//                    productTemplate = fixutureReference;
//                }


//                public void AddErrorMessage(_RTFMessage ErrorMessage)
//                {
//                    ErrorMessages.Add(ErrorMessage);
//                }

//                public List<_RTFMessage> GetErrors()
//                {
//                    CollectErrors();
//                    return ErrorMessages;
//                }

//                public void CollectErrors()
//                {
//                    if (!IsOptional && SelectedParameters.Count == 0) { ErrorMessages.Add(new _RTFMessage().IsError().AddBoldText(Name).AddBoldText(" category is not optional, but has no selection.")); }

//                    foreach (Parameter par in SelectedParameters)
//                    {
//                        ErrorMessages.AddRange(par.GetErrors());
//                    }
//                }

//                public String TypeDescription
//                {
//                    get { return new FixtureSetupCodeParserDbData().CategoryTypes.Where(o => o.Name == CategoryType).First().Description; }
//                }

//                public String Name
//                {
//                    get { return Data.CategoryName; }
//                }

//                public String ID
//                {
//                    get { return Data.CategoryId.ToString(); }
//                }

//                public int? CAF
//                {
//                    get { return Data.CAF_Id; }
//                }

//                public String CategoryType
//                {
//                    get { return Data.CategoryType; }
//                }

//                public Boolean IsOptional
//                {
//                    get { return (bool)Data.CategoryIsOptional; }
//                }

//                public Boolean IsMultiselect
//                {
//                    get { return (bool)Data.CategoryIsMultiselect; }
//                }

//                public Int32? ParserFallbackPACAF
//                {
//                    get { return Data.ParserFallbackParameterID; }
//                }

//                public Parameter FallbackParameter
//                {
//                    get { return TemplateParameters.Where(o => o.PACAFID == ParserFallbackPACAF).FirstOrDefault(); }
//                }

//                public void AddParameter(ProductTemplate ParamRow)
//                {
//                    TemplateParameters.Add(new Parameter(ParamRow));
//                }

//                public void AddSelectedParameter(Parameter param)
//                {
//                    SelectedParameters.Add(param);
//                }

//                public string ProductID
//                {
//                    get { return Data.FixtureCode; }
//                }

//                public Boolean ProductIsSP
//                {
//                    get { return productTemplate.IsSP; }
//                }

//                public String FixtureApplicationType
//                {
//                    get { return Data.FixtureApplicationType; }
//                }

//                public Boolean HasErrors
//                {
//                    get { return ErrorMessages.Count > 0; }
//                }

//                public String ShortenedFilenameComponent
//                {
//                    get
//                    {
//                        if (CategoryType == "Length")
//                        {
//                            UnitParsing.UnitParser unitParser = new UnitParsing.UnitParser();
//                            String ClientSelection = SelectedParameters[0].BreakdownFullCode;
//                            String CustomValue = SelectedParameters[0].CustomValue;

//                            if (unitParser.IsValidLengthExpression(ClientSelection))
//                            {
//                                return unitParser.ConvertLengthExpressionToDecimalInch(ClientSelection).ToString();
//                            }
//                            else if (unitParser.IsValidLengthExpression(CustomValue))
//                            {
//                                return unitParser.ConvertLengthExpressionToDecimalInch(CustomValue).ToString();
//                            }

//                            return String.Join("+", SelectedParameters.Select(o => o.BreakdownFullCode).ToList());
//                        }
//                        else
//                        {
//                            return String.Join("+", SelectedParameters.Select(o => o.BreakdownFullCode).ToList());
//                        }
//                    }
//                }

//                public void SummarizeIntoRTB(_RTFMessage Messenger)
//                {
//                    String CategoryOptionality = (bool)IsOptional ? "Optional" : "Compulsory";
//                    String SelectionType = (bool)IsMultiselect ? "Multiselect" : "SingleSelect";

//                    Messenger.NewMessage().AddBoldText(Name).AddText(" (").AddText(CategoryOptionality).AddText(", ").AddText(SelectionType).AddText(", ").AddText(CategoryType).AddText(")");
//                    SummarizeParameters(Messenger);
//                    Messenger.Log();
//                }

//                public void SummarizeParameters(_RTFMessage Messenger)
//                {
//                    foreach (Parameter parameter in TemplateParameters)
//                    {
//                        parameter.SummarizeIntoRTB(Messenger);
//                    }
//                }

//                public void SummarizeMatchedParameters(_RTFMessage Messenger)
//                {
//                    String CategoryOptionality = (bool)IsOptional ? "Optional" : "Compulsory";
//                    String SelectionType = (bool)IsMultiselect ? "Multiselect" : "SingleSelect";

//                    Messenger.NewMessage().AddBoldText(Name).AddText(" (").AddText(CategoryOptionality).AddText(", ").AddText(SelectionType).AddText(", ").AddText(CategoryType).AddText(")");

//                    if (HasErrors) { Messenger.CurrentMessage.IsError(); }

//                    foreach (Parameter parameter in SelectedParameters)
//                    {
//                        parameter.SummarizeMatchedIntoRTB(Messenger);
//                    }

//                    if (SelectedParameters.Count == 0) { Messenger.CurrentMessage.NewLine().IndentHanging().AddBoldText("No selection."); }

//                    Messenger.Log();
//                }

//                public Boolean HasMatch
//                {
//                    get { return SelectedParameters.Count > 0; }
//                }
//            }

//            public class Parameter
//            {
//                ProductTemplate Data;
//                public _FixtureSetupCodeBreakdown.Parameter MatchingBreakDownParam = null;
//                public List<_RTFMessage> ErrorMessages = new List<_RTFMessage>();
//                ParameterMatcher parameterMatcher = new ParameterMatcher();

//                public String MatchType = null;

//                public Parameter(ProductTemplate data)
//                {
//                    Data = data;
//                    if (data.CategoryType == "ProductID")
//                    {
//                        data.ParameterCode = data.FixtureCode;
//                        data.ParameterDescription = data.FixtureCode;
//                    }
//                }

//                private void CollectErrors()
//                {
//                    if (NeedsCustomValue && !CustomValueIsValid)
//                    {
//                        if (HasCustomValue)
//                        {
//                            _RTFMessage ErrorMessage = new _RTFMessage().IsError().AddBoldText("Parameter ").AddBoldText(BreakdownFullCode).AddBoldText(" in category ").AddBoldText(CategoryName).AddBoldText(":").AddText(" Custom value is invalid.").AddText(" Parameter requires custom value of type ").AddText(CustomValueType).AddText(".");
//                            ErrorMessages.Add(ErrorMessage);
//                        }
//                        else
//                        {
//                            _RTFMessage ErrorMessage = new _RTFMessage().IsError().AddBoldText("Parameter ").AddBoldText(BreakdownFullCode).AddBoldText(" in category ").AddBoldText(CategoryName).AddBoldText(":").AddText(" Custom value not specified. Parameter requires a custom value of type ").AddText(CustomValueType).AddText(".");
//                            ErrorMessages.Add(ErrorMessage);
//                        }
//                    }

//                    if (!NeedsCustomValue && HasCustomValue)
//                    {
//                        _RTFMessage ErrorMessage = new _RTFMessage().IsError().AddBoldText("Parameter ").AddBoldText(BreakdownFullCode).AddBoldText(" in category ").AddBoldText(CategoryName).AddBoldText(":").AddText(" Parameter requires no custom value, but a custom value of ").AddText(CustomValue).AddText(" is specified.");
//                        ErrorMessages.Add(ErrorMessage);
//                    }
//                }

//                public List<_RTFMessage> GetErrors()
//                {
//                    ErrorMessages.Clear();
//                    CollectErrors();
//                    return ErrorMessages;
//                }

//                public Int32 PACAFID
//                {
//                    get { return (Int32)Data.PAC_Id; }
//                }

//                public String TemplateFullCode
//                {
//                    get { return Data.ParameterCode; }
//                }
//                public String BreakdownFullCode
//                {
//                    get { return MatchingBreakDownParam.FullCode; }
//                }

//                public String BaseValue
//                {
//                    get { return new Regex(@"^[^(]+", RegexOptions.IgnoreCase).Match(TemplateFullCode).Groups[0].Value; }
//                }

//                public String CustomValueType
//                {
//                    get { return ParameterCustomType.Get(TemplateFullCode); }
//                }

//                public Boolean NeedsCustomValue
//                {
//                    get { return CustomValueType != null; }
//                }

//                public Boolean CustomValueIsValid
//                {
//                    get { return parameterMatcher.ValidateCustomValueType(CustomValue, CustomValueType); }
//                }

//                public String CustomValue
//                {
//                    get { return MatchType == "Category Fallback Parameter" ? MatchingBreakDownParam.FullCode : MatchingBreakDownParam.CustomValue; }
//                }

//                public String Description
//                {
//                    get { return Data.ParameterDescription; }
//                }

//                public Boolean HasCustomValue
//                {
//                    get { return MatchingBreakDownParam.HasCustomValue; }
//                }
//                public String CategoryType
//                {
//                    get { return Data.CategoryType; }
//                }

//                public Int32 DisplayOrder
//                {
//                    get { return (Int32)Data.PAC_DisplayOrder; }
//                }

//                public String CategoryName
//                {
//                    get { return Data.CategoryName; }
//                }

//                public void SummarizeIntoRTB(_RTFMessage Messenger)
//                {
//                    Messenger.CurrentMessage.NewLine().IndentHanging().AddText("Parameter ").AddBoldText(TemplateFullCode).AddText(" (").AddText(Description).AddText(")");
//                }

//                public void SummarizeMatchedIntoRTB(_RTFMessage Messenger)
//                {
//                    //summary
//                    Messenger.CurrentMessage.NewLine().IndentHanging().AddText("Parameter ").AddBoldText(TemplateFullCode).AddText(" (").AddText(Description).AddText(")");

//                    if (HasCustomValue) { Messenger.CurrentMessage.NewLine().AddText("Param Custom Value: ").AddBoldText(CustomValue); }
//                    Messenger.CurrentMessage.NewLine().AddText("Match type: ").AddBoldText(MatchType);

//                    //checking for errors
//                    if (NeedsCustomValue && !CustomValueIsValid)
//                    {
//                        if (HasCustomValue)
//                        {
//                            Messenger.CurrentMessage.IsError().NewLine().AddBoldText("Parameter custom value is invalid.").AddBoldText(" Parameter requires custom value of type ").AddBoldText(CustomValueType).AddBoldText(".");
//                        }
//                        else
//                        {
//                            Messenger.CurrentMessage.IsError().NewLine().AddBoldText("Parameter requires a custom value of type ").AddBoldText(CustomValueType).AddBoldText(".");
//                        }
//                    }

//                    if (!NeedsCustomValue && HasCustomValue)
//                    {
//                        Messenger.CurrentMessage.IsError().NewLine().AddBoldText("Parameter requires no custom value, but a custom value is specified.");
//                    }
//                }
//            }
//        }
//    }

//}
