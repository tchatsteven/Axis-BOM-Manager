using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BOM_MANAGER;

namespace AXISAutomation.Solvers.BOM
{
    public class _Filter
    {
        PartRulesFilter DbData;
        Boolean IsAssembly = true;
        _Part _MyPart;
        _Assembly _MyAssembly;

        public _Filter(PartRulesFilter dbData, _Part myPart)
        {
            DbData = dbData;
            _MyPart = myPart;
            IsAssembly = false;

        }

        public _Filter(PartRulesFilter dbData, _Assembly myAssembly)
        {
            DbData = dbData;
            _MyAssembly = myAssembly;

        }

        public Boolean ComponentExist
        {
            get
            {
                return IsAssembly ? _MyAssembly.EpicorExists : _MyPart.EpicorExists;
            }
            set
            {
                if (IsAssembly)
                {
                    _MyAssembly.EpicorExists = value;
                }
                else
                {
                    _MyPart.EpicorExists = value;
                }
            }
        }
        public String ComponentName
        {
            get
            {
                return IsAssembly ? MyAssembly.Name : MyPart.Name;
            }
            //set
            //{
            //    if (IsAssembly)
            //    {
            //        MyAssembly.Name = value;
            //    }
            //    else
            //    {
            //        MyPart.Name = value;
            //    }
            //}
        }
        public String ComponentType
        {
            get
            {
                return IsAssembly ? MyAssembly.EpicorType : MyPart.EpicorType;
            }
        }
        public Int32 ComponentQuantity
        {
            get
            {
                return IsAssembly ? MyAssembly.Quantity : MyPart.Quantity;
            }
            set
            {
                if (IsAssembly)
                {
                    MyAssembly.Quantity = value;
                }
                else
                {
                    MyPart.Quantity = value;
                }
            }
        }

        public String FilterName => IsAssembly ? DbData.Assembly.Name : DbData.Part.Name;
        public String RenamingExpression => DbData.RenamingExpression.ExpressionName;
        public String FilterType => DbData.FilterType.FilterTypeName;
        public String FilterCategoryType => DbData.CategoryName;
        public String FilterBehaviour => DbData.FilterBehavior.Behavior;
        public Int32? FilterQuantity => DbData.Quantity; 
        public String FilterParameterList => DbData.PACAF_ID;
        public String FilterPartDescription => DbData.Part.Description;
        public String FilterDependableQtyType => DbData.DependableQuantity.DependableQuantityName;
        
        public Decimal Length => MyPart.MyAssembly.MyBOMSection.Length;

        public String Finish => IsAssembly ? MyAssembly.MyBOMSection.GetSelectedFinish_Category : MyPart.MyAssembly.MyBOMSection.GetSelectedFinish_Category;
        public String Finish_CustomValue => IsAssembly ? MyAssembly.MyBOMSection.GetSelectedFinish_CustomValue : MyPart.MyAssembly.MyBOMSection.GetSelectedFinish_CustomValue;
        public Int32 SectionCount => MyPart.MyBOM.SectionCount;
        public Boolean IsStart => MyPart.MyAssembly.MyBOMSection.IsAtStart;
        public Boolean IsEnd => IsAssembly ? MyAssembly.MyBOMSection.IsAtEnd : MyPart.MyAssembly.MyBOMSection.IsAtEnd;
        public Boolean IsMiddle => IsAssembly ? MyAssembly.MyBOMSection.IsAtMiddle : MyPart.MyAssembly.MyBOMSection.IsAtMiddle;
        public Boolean IsBlank => FilterPartDescription.Contains("BLANK");
        public Boolean IsLens => FilterPartDescription.Contains("LONG");

        List<Int32> AllSelectedPACAFS => IsAssembly ? MyAssembly.MyBOMSection.AllSelectedPACAFS : MyPart.MyAssembly.MyBOMSection.AllSelectedPACAFS;
        public _Part MyPart => _MyPart;
        public _Assembly MyAssembly => _MyAssembly;


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
                case "JOINER":
                    ProcessAsJoiner();
                    break;
                case "QUANTITY":
                    ProcessAsQuantity();
                    break;
                case "JOINER QTY":
                    ProcessAsJoinerQTY();
                    break;
                case "ENDCAP QTY":
                    ProcessAsEndCapQTY();
                    break;
                case "DEPENDABLE QTY":
                    ProcessAsDependableQty();
                    break;
                case "RENAMINGEXPRESSION":
                    ProcessAsRenamingExpression();
                    break;

            }
        }

        private void ProcessAsRenamingExpression()
        {
            if(IsAssembly)
            {
                MyAssembly.SetEpicorName( ReplaceRenamingExpressionTags(RenamingExpression));
            }
            else
            {
               MyPart.SetEpicorName(ReplaceRenamingExpressionTags(RenamingExpression));
            }
        }

        private String ReplaceRenamingExpressionTags(String RenamingExpression)
        {
            String ResultString = RenamingExpression;
            if (RenamingExpression.Contains("%PartName%"))
            {
                ResultString = ResultString.Replace("%PartName%",ComponentName);
            }

            if (RenamingExpression.Contains("%Length%"))
            {
                ResultString = ResultString.Replace("%Length%", Length_Expression());
            }

            if (RenamingExpression.Contains("%Paint%"))
            {
                ResultString = ResultString.Replace("%Paint%", Paint_Expression(Finish_CustomValue ?? Finish));
            }

            if (RenamingExpression.Contains("%LensFinish%"))
            {
                ResultString = ResultString.Replace("%LensFinish%", LensFinish_Expression());
            }

            if (RenamingExpression.Contains("%ColourTemperature%"))
            {
                ResultString = ResultString.Replace("%ColourTemperature%", "");
            }
            return ResultString;
        }

        private String Length_Expression()
        {
            bool IsExtrusion = ComponentType.Contains("EXTRUSION");
            bool IsLens = ComponentType.Contains("LENS");
            String RE_Length = null ;
           
            if ((Length > 26 && Length <= 37 || Length > 50 && Length <= 74 || Length > 100) && IsExtrusion)
            {
                RE_Length = "12";
            }
            else if ((Length <= 26 || Length > 37 && Length <= 50 || Length > 74 && Length <= 100) && IsExtrusion)
            {
                RE_Length = "8";
            }
            else if ((Length > 26 && Length <= 37 || Length > 50 && Length <= 74 || Length > 100) && IsLens)
            {
                RE_Length = "150";
            }
            else if ((Length <= 26 || Length > 37 && Length <= 50 || Length > 74 && Length <= 100) && IsLens)
            {
                RE_Length = "102";
            }
            return RE_Length;
        }

        private String Paint_Expression(String paint_Finish)
        {
            String RE_Paint = null;

            switch (paint_Finish)
            {
                case "BLACK": case "BLK": case "BK": case "B":
                    RE_Paint = "9005";
                    break;
                case "AP":
                    RE_Paint = "SL";
                    break;
                case "W":
                    RE_Paint = "WH";
                    break;
            }

            return RE_Paint;
        }

        private String LensFinish_Expression()
        {
            String RE_LensFinish = "SO";

            return RE_LensFinish;
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

        private void Inclusive()
        {
            String[] parameterList_split = FilterParameterList.Split('|');

            foreach (String x in parameterList_split)
            {
                if (AllSelectedPACAFS.Any(t => t.ToString() == x))
                {

                    //if ((ComponentType == "EXTRUSION" && !IsBlank) || (ComponentType == "LENS" && IsLens))
                    //{
                    //    EXorLN_Selection();
                    //}

                    ////else if (FilterCategoryType == "ENDCAP" || FilterCategoryType == "ENDCAP LENS")
                    ////{
                    ////    EndCapDropLens_Selection();
                    ////}

                    //else
                    //{
                        ComponentExist = true;
                    //}


                }
            }
        }

        private void Exclusive()
        {
            String[] parameterList_split = FilterParameterList.Split('|');

            foreach (String x in parameterList_split)
            {
                if (AllSelectedPACAFS.Any(t => t.ToString() == x))
                {
                    ComponentExist = false;
                }
            }
        }

        private void ProcessAsJoiner()
        {
            if (!IsEnd)
            {
                ComponentExist = true;
            }
            else if(IsEnd)
            {
                ComponentExist = false;
                ComponentQuantity = 0;
            }
        }

        private void ProcessAsJoinerQTY()
        {

            //Change quantity
            if (SectionCount > 1 && !IsEnd && (FilterName == "IP14116-UN" || FilterName == "IP14115-UN"))
            {
                ComponentQuantity = ComponentQuantity + 2;
            }
        }

        private void ProcessAsQuantity()
        {
            if (ComponentExist)
            {
                ComponentQuantity = (Int32)FilterQuantity;

            }
            else
            {
                ComponentQuantity = 0;
            }
        }

        private void ProcessAsEndCapQTY()
        {
            if (IsAssembly && MyAssembly.MyBOM.SectionCount < 2)
            {              
                ComponentQuantity = ComponentQuantity *2;
            }
            else if(!IsAssembly && MyPart.MyBOM.SectionCount < 2)
            {
                ComponentQuantity = ComponentQuantity *2;
            }
            else if (IsMiddle)
            {
                ComponentExist = false;
                ComponentQuantity = 0;
            }

        }

        private void ProcessAsDependableQty()
        {
            switch (FilterDependableQtyType)
            {
                case "DRIVER QUANTITY":
                    SolveDependableDriverQty();
                    break;
                case "REFLECTOR":
                    SolveDependableReflectorQty();
                    break;
                case "CARTRIDGE":
                    SolveDependableCatridgeQty();
                    break;
                case "T-LINER BRACKET":
                    SolveDependableT_Liner_BR();
                    break;
                case "SECTION LENGTH":
                    SolveDependableSection_Length();
                    break;
                case "POWER DROP":
                    SolveDependablePowerDrop();
                    break;
                case "MOUNTING":
                    SolveDependableMounting();
                    break;
            }

        }


        
        //Solvers
        private void SolveDependableDriverQty()
        {
            //Asuming that there is only 1 DRIVER in Each Section
            Int32 DriverQty = 1;

            ComponentQuantity = ComponentQuantity * DriverQty;

        }

        private void SolveDependableReflectorQty()
        {


        }

        private void SolveDependableCatridgeQty()
        {


        }

        private void SolveDependableT_Liner_BR()
        {
            //Get the quantity of T-Liner Brakets and multiply by 2 (2/ T-Liner) 
            if (Length > 24)
            {
                ComponentQuantity = ComponentQuantity * ((Int32)Math.Ceiling(Length / 24) - 1);

            }
            else
            {
                ComponentExist = false;
            }
        }

        private void SolveDependableSection_Length()
        {
            //Provide the number T-Liner according to the Section Length
            if (Length > 24)
            {
                ComponentQuantity = ((Int32)Math.Ceiling(Length / 24) - 1);
            }

            else
            {
                ComponentExist = false;
            }

        }

        private void SolveDependablePowerDrop()
        {
            //check from Nick's electrical bom class if sectionHasPowerDrop == true
            //Assuming that all sections have power drops
            Boolean sectionHasPowerDrop = true;

            if (sectionHasPowerDrop)
            {
                ComponentExist = true;
            }
            else
            {
                ComponentExist = false;
            }

        }

        private void SolveDependableMounting()
        {
            String[] ApplicableMountings = { "MFTB9", "MFTB15", "MFTG9", "MFTG15", "MFST" };

            foreach (String x in ApplicableMountings)
            {
                if (MyPart.MyAssembly.MyBOMSection.GetSelectedMounting_Category == x && Length > 24)
                {
                    ComponentQuantity = ComponentQuantity + ((Int32)Math.Ceiling(Length / 24) - 1);
                }
                //    else
                //    {
                //        MyPart.Exists = false;
                //        MyPart.MyAssembly.AssemblyExists = false;
                //    }
            }


        }

    }
}
