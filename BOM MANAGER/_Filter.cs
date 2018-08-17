using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOM_MANAGER
{
    public class _Filter
    {
        PartRulesFilter DbData;
       
        _Part _MyPart;

        public _Filter(PartRulesFilter dbData, _Part myPart)
        {
            DbData = dbData;
            _MyPart = myPart;

            //Process();
        }

        public _Part MyPart => _MyPart;
        
        public String FilterType => DbData.FilterType.FilterTypeName;
        public String FilterBehaviour => DbData.FilterDependencyName;
        public Int32? FilterQuantity => DbData.Quantity;
        public String ParameterList => DbData.PACAF_ID;
        public String PartName => DbData.Part.PartName;
        public String PartDescription => DbData.Part.Description;
        public String PartType => DbData.Part.PartType.PartType1;
        public Decimal Length => MyPart.MyAssembly.MyBOMSection.Length;
    

        public Boolean IsStart => MyPart.MyAssembly.MyBOMSection.IsAtStart;
        public Boolean IsEnd => MyPart.MyAssembly.MyBOMSection.IsAtEnd;
        public Boolean IsMiddle => MyPart.MyAssembly.MyBOMSection.IsAtMiddle;
        public Boolean IsBlank => PartDescription.Contains("BLANK");
        public Boolean IsLens => PartDescription.Contains("LONG");

        List<String> AllSelectedPACAFS => MyPart.MyAssembly.MyBOMSection.AllSelectedPACAFS;

        public void Run()
        {
            Process();
        }

        private void Process()
        {
            switch (FilterType)
            {
                case "PACAF":
                    ProcessAsPACAF();
                    break;
                case "JOINER QTY":
                    ProcessAsJoinerQTY();
                    break;
                case "QUANTITY":
                    ProcessAsQuantity();
                    break;
                case "ENDCAP QTY":
                    ProcessEndCapQTY();
                    break;

            }
        }


        private void ProcessAsPACAF()
        {

            if (FilterBehaviour == "INCLUSIVE")
            {
                Inclusive();
            }

            else if (FilterBehaviour == "EXCLUSIVE")
            {

                Exclusive();
            }

        }

        private void ProcessAsJoinerQTY()
        {                       
                     
            if (!IsEnd)
            {
                MyPart.Exists = true;

            }
        }

        private void ProcessAsQuantity()
        {
            MyPart.Quantity = FilterQuantity;
        }

        private void ProcessEndCapQTY()
        {
            //Process quantity for end cap here.
        }

        private void Inclusive()
        {
            String[] parameterList_split = ParameterList.Split('|');

            foreach (String x in parameterList_split)
            {
                if (AllSelectedPACAFS.Any(t => t.Contains(x)))
                {

                    if ((PartType == "EXTRUSION" && !IsBlank) || (PartType == "LENS" && IsLens))
                    {
                        EXorLN_Selection();
                    }

                    else if (PartType == "ENDCAP" || PartType == "ENDCAP LENS")
                    {
                        EndCapDropLens_Selection();
                    }

                    else
                    {
                        MyPart.Exists = true;
                    }

                    
                }
            }
        }

        private void Exclusive()
        {
            MyPart.Exists = false;
        }

        private void EXorLN_Selection()
        {
            bool EX_8_FT = PartName.Contains("-8-");
            bool LN_8_FT = PartName.Contains("-102-");
            if ((Length > 0 && Length <= 20 || Length > 37 && Length <= 50 || Length > 74 && Length <= 100) && (EX_8_FT || LN_8_FT))
            {
                MyPart.Exists = true;
            }
            else if ((Length > 20 && Length <= 37 || Length > 50 && Length <= 74 || Length > 100) && !(EX_8_FT || LN_8_FT))
            {
                MyPart.Exists = true;
            }
            else
            {
                MyPart.Exists = false;
            }
        }

        private void EndCapDropLens_Selection()
        {
            if (IsStart || IsEnd)
            {
                MyPart.Exists = true;
            }

            else
            {
                MyPart.Exists = false;
            }
        }




    }
}
