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
    public partial class Rules_EditForm : Form
    {
        AXIS_AutomationEntitiesBOM db = new AXIS_AutomationEntitiesBOM();
        PartRulesFilter newPartRule = null;
        BOM_MANAGER ParentForm;

        //Default constructor
        public Rules_EditForm()
        {
            InitializeComponent();
            
        }

        public Rules_EditForm(BOM_MANAGER parentForm)
        {
            InitializeComponent();
            ParentForm = parentForm;
            LoadDataFromParentForm();
        }

        //constructor 
        //public Rules_EditForm(String editPartName, String editProductCode, String editCategoryName, String editParameterName, Int32 editPACAF_Id, Int32 editQty, String editFilterRule, Int32 editProductCodeIndex)
        //{
        //    InitializeComponent();
        //    //PartName_textBox.Text = editPartName;
        //    //ProductCode_textBox.Text = editProductCode;
        //    //PACAF_textBox.Text = editPACAF_Id.ToString();
        //    //QtyEdit_numericUpDown.Value = editQty;

        //    //LoadFilterRule(editFilterRule);
        //    //LoadCategory_ComboBox(editCategoryName, editProductCodeIndex);
        //    //LoadParameter_ComboBox(editParameterName, editProductCodeIndex);
        //    LoadDataFromParentForm();
        //}

        private void LoadDataFromParentForm()
        {
            PartName_textBox.Text = ParentForm.EditPartName;
            ProductCode_textBox.Text = ParentForm.EditProductCode;
            PACAF_textBox.Text = ParentForm.EditPACAF_Id.ToString();
            QtyEdit_numericUpDown.Value = (Int32)ParentForm.EditQty;

            LoadFilterRule(ParentForm.EditFilterRule);
            LoadCategory_ComboBox(ParentForm.EditCategoryName, ParentForm.EditProductCodeIndex);
            LoadParameter_ComboBox(ParentForm.EditParameterName, ParentForm.EditProductCodeIndex);
        }

        private void LoadFilterRule(String FilterRuleToEdit)
        {
            FilterRuleEdit_comboBox.DataSource = db.FilterBehaviors.ToList();
            FilterRuleEdit_comboBox.DisplayMember = "Behavior";
            FilterRuleEdit_comboBox.ValueMember = "id";
            FilterRuleEdit_comboBox.Text = FilterRuleToEdit;

        }

        private void LoadCategory_ComboBox( String CategoryName, Int32 ProductCode_Id)
        {
            CategoryName_comboBox.DataSource = db.ProductTemplates.AsNoTracking().Where(o => o.FixtureId.ToString() == ProductCode_Id.ToString() && o.FixtureCode != "PRODUCT ID").OrderBy(o => o.CAF_DisplayOrder).ToList();
            CategoryName_comboBox.ValueMember = "CAF_Id";
            CategoryName_comboBox.DisplayMember = "CategoryName";
            CategoryName_comboBox.Text = CategoryName;
        }

        private void LoadParameter_ComboBox(String ParameterName, Int32 ProductCode_Id)
        {
            ParameterName_comboBox.DataSource = db.ProductTemplates.Where(o => o.FixtureId.ToString() == ProductCode_Id.ToString() && o.CAF_Id.ToString() == CategoryName_comboBox.SelectedValue.ToString() && o.ParameterCode != null).Distinct().OrderBy(o => o.PAC_DisplayOrder).ToList();
            ParameterName_comboBox.ValueMember = "PAC_Id";
            ParameterName_comboBox.DisplayMember = "ParameterCode";
            ParameterName_comboBox.Text = ParameterName;
        }

        private void LoadParameter_ComboBox()
        {
            String productId = db.Fixtures.Where(O => O.Code == ProductCode_textBox.Text).First().id.ToString();
            String categoryId = CategoryName_comboBox.SelectedValue.ToString();
            
            ParameterName_comboBox.DataSource = db.ProductTemplates.Where(o => o.FixtureId.ToString() == productId && o.CAF_Id.ToString() == categoryId && o.ParameterCode != null).Distinct().OrderBy(o => o.PAC_DisplayOrder).ToList();
            ParameterName_comboBox.ValueMember = "PAC_Id";
            ParameterName_comboBox.DisplayMember = "ParameterCode";

            
        }

        private void GetPACAFID()
        {
            try
            {
                Int32 selectedProductId = Int32.Parse(db.Fixtures.Where(O => O.Code == ProductCode_textBox.Text).First().id.ToString());
                PACAF_textBox.Text = db.ProductTemplates.Where(o => o.FixtureId == selectedProductId && o.CAF_Id.ToString() == CategoryName_comboBox.SelectedValue.ToString() && o.PAC_Id.ToString() == ParameterName_comboBox.SelectedValue.ToString()).First().PAC_DisplayOrder.ToString();

            }
            catch
            {

            }

        }   

        private void CategoryName_comboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            LoadParameter_ComboBox();
        }

        private void ParameterName_comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            GetPACAFID();

        }

        private void Ok_EditRulesButton_Click(object sender, EventArgs e)
        {
                            
                Int32 RowID = Int32.Parse(ParentForm.DataGridView_Rules.CurrentRow.Cells["id"].Value.ToString());
                newPartRule = db.PartRulesFilters.Where(o => o.id == RowID).First();

                newPartRule.CategoryName = CategoryName_comboBox.Text;
                newPartRule.CategoryID = Int32.Parse(CategoryName_comboBox.SelectedValue.ToString());

                newPartRule.ParameterName = ParameterName_comboBox.Text;
                newPartRule.ParameterID = ParameterName_comboBox.SelectedValue.ToString();

                newPartRule.FilterDependencyID = Int32.Parse(FilterRuleEdit_comboBox.SelectedValue.ToString());
                newPartRule.FilterDependencyName = FilterRuleEdit_comboBox.Text;

                newPartRule.PACAF_ID = PACAF_textBox.Text;

                newPartRule.Quantity = Int32.Parse(QtyEdit_numericUpDown.Value.ToString());

                db.SaveChanges();
                DialogResult = DialogResult.OK;
                Close();

           
        }

        //public string PartName =>PartName_textBox.Text;
    }
}
