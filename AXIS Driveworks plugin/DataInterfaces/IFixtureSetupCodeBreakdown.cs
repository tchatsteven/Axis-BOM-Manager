using System;
using System.Collections.Generic;
using System.Text;

namespace DataInterfaces
{
    public interface IFixtureSetupCodeBreakdown
    {
        List<String> Dra { get; set; }
    }

    public interface IParameter
    {
        String FullCode { get; set; }
        List<String> SplitAtExtras { get; set; }
        List<IParameter> ExtraParameters { get; set; }
        String CustomPropertyValue { get; set; }
        String BaseParameter { get; }
    }
}
