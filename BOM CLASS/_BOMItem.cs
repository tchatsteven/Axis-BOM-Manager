using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BOM_CLASS;

namespace AXISAutomation.Solvers.BOM
{
    public interface IBOMItem
    {
        String EpicorName { get; }
        String EpicorType { get; }
        Double Quantity { get; set; }
        Boolean EpicorExists { get; }
        Int32 FilterCount { get; }
        Boolean IsIndented { get; }
    }

}
