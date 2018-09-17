using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AXISAutomation.Tools.Logging;
using System.Windows.Forms;


namespace BOM_MANAGER
{
    public class _Part
    {
        PartView DbData;
        _Assembly _MyAssembly;
        public List<_Filter> Filters = new List<_Filter>();
        public Boolean Exists;
        public Int32? Quantity = 1;
        String _EpicorName = null;


        public _Part(PartView dbData, _Assembly myAssembly)
        {
            DbData = dbData;
            _MyAssembly = myAssembly;


            Get_Filters();
            Process_Filter();
        }

        public Decimal TabSize => MyBOM.TabSize;
        public Decimal IndentationDepth => _MyAssembly.IndentationDepth + 1m;
        public String Name   { get { return DbData.PartName; } }
        public String EpicorName { get { return _EpicorName ?? Name; } }
        public String Type => DbData.PartTypeName;
        public String Description => DbData.Description;
        public String MyFixtureCode => MyBOM.FixtureConfiguration.ProductCode;

        public AXIS_AutomationEntitiesBOM DbConn => MyBOM.DbConn;
        public _BOM MyBOM => MyAssembly.MyBOM;
        public _Assembly MyAssembly => _MyAssembly;
        public Boolean PartExists
        {
            set => Exists = value;
            get => Exists && Quantity > 0;

        }
        public TreeNode MyTreeNode
        {
            get
            {
                TreeNode treeNode = new TreeNode()
                {
                    Text = DbData.PartName,
                    Tag = DbData
                };

                return treeNode;
            }
        }



        private void Get_Filters()
        {
            DbConn.PartRulesFilters.Where(o => o.Part.Name == Name && o.ProductCode == MyFixtureCode).OrderBy(p=> p.OrderOfExecution).ToList().ForEach(o=> Filters.Add(new _Filter(o,this)));
        }

        public void SetEpicorName(String epicorName)
        {
            _EpicorName = epicorName;
        }

        private void Process_Filter()
        {
            foreach (_Filter item in Filters)
            {
                item.Run();
            }
        }



        public void SummarizeExistingComponentInToRTB(_RTFMessenger ApplicableParts)
        {
            if (Exists)
            {
                ApplicableParts.NewMessage().Tab(IndentationDepth * 0.2m ).AddText(EpicorName).Tab(2.6m).AddText(Type).Tab(4.2m).AddText(Quantity.ToString()).Log();

            }
          
        }

        public void SummarizeNonExistingCompInToRTB(_RTFMessenger NonApplicableParts)
        {
            if (!Exists)
            {
                NonApplicableParts.NewMessage().Tab(IndentationDepth * TabSize).AddText(EpicorName);

                if (Filters.Count == 0)
                {
                    NonApplicableParts.CurrentMessage.Tab(3m).AddText("NO RULE").IsError().Log();
                }
                else
                {
                    NonApplicableParts.CurrentMessage.Tab(3m).AddText("NOT APPLICABLE").Log();
                }
            }
            
        }


    }
}
