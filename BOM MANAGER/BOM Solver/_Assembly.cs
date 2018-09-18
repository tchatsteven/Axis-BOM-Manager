using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AXISAutomation.Tools.Logging;
using System.Windows.Forms;

namespace BOM_MANAGER
{
    public class _Assembly
    {
        AssemblyView DbData;
        _BOMSection _MyBOMSection;
        _Assembly _ParentAssy;
        public List<_Assembly> _Assemblies = new List<_Assembly>();
        public List<_Part> _Parts = new List<_Part>();
        public List<_Filter> _Filters = new List<_Filter>();

        TreeNode _MyTreeNode;
        public Boolean Exists;
        public Int32? Quantity = 1;
        String _EpicorName = null;



        public _Assembly(AssemblyView dbData, _BOMSection bomSection, _Assembly parentAssy = null)
        {
            _ParentAssy = parentAssy;
            DbData = dbData;
            _MyBOMSection = bomSection;
            InitChildren();
            InitMyTreeNode();            
        }

        

        public Int32 ID => DbData.id;
        public String Name{ get { return  IsRootNode ? MyBOMSection.Name : DbData.AssemblyName; }  }
        public String EpicorName { get { return _EpicorName ?? Name; } } 
        public String Type => DbData.AssemblyTypeName;
        public String MyFixtureCode => MyBOMSection.MyFixtureCode;
        public Decimal IndentationDepth => IsRootNode ? 0m : ParentAssy.IndentationDepth + 1m;
        public Decimal TabSize => MyBOM.TabSize;
        public Boolean AssemblyExists
        {
            set => Exists = value;
            get => Exists && Quantity > 0;

        }
        //public Int32 Id => DbData.id;

        public AXIS_AutomationEntitiesBOM DbConn => MyBOMSection.DbConn;
        public _BOM MyBOM => MyBOMSection.MyBOM;
        public _BOMSection MyBOMSection => _MyBOMSection;
        public _Assembly ParentAssy => _ParentAssy;


        public TreeNode MyTreeNode => _MyTreeNode;

        public List<_Assembly> Assemblies => _Assemblies;

        public Boolean IsRootNode => DbData.ParentIDAtAssyAtAssy == null;


        private void InitChildren()
        {
            GetChildAssemblies();
            GetChildParts();
            Get_Filters();
            Process_Filter();
        }

        private void GetChildAssemblies()
        {
            DbConn.AssemblyViews.Where(a => a.ParentIDAtAssyAtAssy == ID).ToList().ForEach(o => _Assemblies.Add(new _Assembly(o, MyBOMSection, this)));
        }

        private void GetChildParts()
        {
            DbConn.PartViews.OrderBy(p=>p.PartName).Where(p => p.AssemblyID == ID).ToList().ForEach(p => _Parts.Add(new _Part(p, this)));
        }

        private void Get_Filters()
        {
            DbConn.PartRulesFilters.OrderBy(o => o.OrderOfExecution).Where(o => o.AssemblyID == ID && o.ProductCode == MyFixtureCode).ToList().ForEach(o => _Filters.Add(new _Filter(o, this)));
        }

        public void SetEpicorName(String epicorName)
        {
            _EpicorName = epicorName;
        }

        private void Process_Filter()
        {
            foreach (_Filter item in _Filters)
            {
                item.Run();
            }
        }

        public void InitMyTreeNode()
        {
            TreeNode treeNode = new TreeNode()
            {
                Text = DbData.AssemblyName,
                Tag = DbData
            };

            GetTreeNodeChildren(treeNode);
            _MyTreeNode= treeNode;            
        }

        void GetTreeNodeChildren(TreeNode parentTreeNode)
        {
            _Assemblies.ForEach(a => parentTreeNode.Nodes.Add(a.MyTreeNode));
            _Parts.ForEach(p => parentTreeNode.Nodes.Add(p.MyTreeNode));
        }


        public void SummarizeExistingComponentInToRTB(_RTFMessenger ApplicableParts)
        {
            ApplicableParts.NewMessage();
           
            if (IsRootNode)
            {
                ApplicableParts.CurrentMessage.AddBoldText(EpicorName + " Length: ").AddBoldText(MyBOMSection.Length.ToString()).Log();
            }
            else
            {
                if (Exists)
                {
                    ApplicableParts.CurrentMessage.Tab(0.2m).AddBoldText(EpicorName).Tab(2.6m).AddText(Type).Tab(4.2m).AddText(Quantity.ToString()).Log();

                }
              
            }

            foreach (_Assembly Assy in _Assemblies)
            {
                Assy.SummarizeExistingComponentInToRTB(ApplicableParts);
            }

            foreach (_Part PartItems in _Parts)
            {
                PartItems.SummarizeExistingComponentInToRTB(ApplicableParts);
            }
        }

        public void SummarizeNonExistingCompInToRTB(_RTFMessenger NonApplicableParts)
        {
            NonApplicableParts.NewMessage();

            if (IsRootNode)
            {
                NonApplicableParts.CurrentMessage.AddBoldText(EpicorName + " Length: ").AddBoldText(MyBOMSection.Length.ToString()).Log();
            }
            else
            {
                if (!Exists)
                {
                    NonApplicableParts.CurrentMessage.Tab(IndentationDepth * TabSize).AddBoldText(EpicorName);
                    if (_Filters.Count == 0)
                    {
                        NonApplicableParts.CurrentMessage.Tab(3m).AddText("NO RULE").IsError().Log();
                    }
                    else
                    {
                        NonApplicableParts.CurrentMessage.Tab(3m).AddText("NOT APPLICABLE").Log();
                    }
                }
                
            }

            foreach (_Assembly Assy in _Assemblies)
            {
                Assy.SummarizeNonExistingCompInToRTB(NonApplicableParts);
            }

            foreach (_Part PartItems in _Parts)
            {
                PartItems.SummarizeNonExistingCompInToRTB(NonApplicableParts);
            }
        }

    }
}
