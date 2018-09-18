using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BOM_MANAGER
{
    class _FlatBOM
    {
        public Boolean IsIndented = false;
        Boolean IsAssembly = true;

        _Assembly _MyAssembly;
        _Part _MyPart;

        public _FlatBOM(_Assembly assembly)
        {
            _MyAssembly = assembly;
        }

        public _FlatBOM(_Part part)
        {
            _MyPart = part;
            IsAssembly = false;
            CheckIndentation(part.MyTreeNode);
        }

        public _Assembly MyAssembly => _MyAssembly;
        public _Part MyPart => _MyPart;

        public String EpicorName => IsAssembly ? MyAssembly.EpicorName : MyPart.EpicorName;
        public String EpicorType => IsAssembly? MyAssembly.Type : MyPart.Type;
        public Int32? Quantity => IsAssembly ? MyAssembly.Quantity : MyPart.Quantity;
        public Boolean Exist => IsAssembly? MyAssembly.Exists : MyPart.Exists;
        public Int32 FilterCount => IsAssembly ? MyAssembly._Filters.Count : MyPart.Filters.Count;


        void CheckIndentation(TreeNode parentTreeNode)
        {
            PartView AssyTypeName = (PartView)parentTreeNode.Tag;
            if(!(AssyTypeName.AssemblyTypeName == "ROOT"))
            {
                IsIndented = true;
            }
        }

    }
}
