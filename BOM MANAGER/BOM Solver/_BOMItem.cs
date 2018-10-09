using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BOM_MANAGER;

namespace AXISAutomation.Solvers.BOM
{
    public interface IBOMItem
    {
        String EpicorName { get; }
        String EpicorType { get; }
        Int32 Quantity { get; set; }
        Boolean EpicorExists { get; }
        Int32 FilterCount { get; }
        Boolean IsIndented { get; }
    }

}
