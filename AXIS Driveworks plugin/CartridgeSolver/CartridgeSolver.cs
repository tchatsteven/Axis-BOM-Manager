using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CartridgeSolver
{
    static class Settings
    {
        public static decimal LongestDistanceToCalculate = 149.875m;
        public static class BEAM
        {
            public static decimal AcceptableGap = 0.25m;
            public static int NomOfCalculationRounds = 4;
            public static decimal ShortestAcceptableCartridge = 20m;
            public static decimal InstallationGap = 0.02m;
        }
    }

    public class _BoardCartridge
    {
        GenericCartridge Data;
        Int32 _Amperage;
        public _BoardsLayout BoardsLayout;

        public _BoardCartridge(GenericCartridge data)
        {
            Data = data;
            BoardsLayout = new _BoardsLayout(this);
            _Amperage = 500;
        }

        public Int32 ID
        {
            get { return Data.CartID; }
        }

        public Int32 Serie
        {
            get { return Data.Serie; }
        }

        public Int32 InSerieID
        {
            get { return Data.InSerieID; }
        }

        public String Name
        {
            get { return String.Format(@"C{0}-{1}", Serie, InSerieID); }
        }

        public Decimal Length
        {
            get { return Data.Length; }
        }

        public Boolean NeedsCutting
        {
            get { return Data.NeedsCutting; }
        }   
        
        public String Amperage
        {
            get { return String.Format(@"{0}mA", _Amperage); }
            set { _Amperage = Convert.ToInt32(value); }
        }

        public String Summary
        {
            get { return String.Format(@"{0}@{1}", Name, Amperage); }
        }
    }

    public class AllCartridgeSolutions
    {
        CartridgeSolverDbDataConnection DBData = new CartridgeSolverDbDataConnection();

        List<_BoardCartridgesLayout> Solutions = new List<_BoardCartridgesLayout>();
        List<_BoardCartridge> AllCartridges = new List<_BoardCartridge>();
        public Decimal RequestedLength = 0m;
        public Boolean SplitGap = false;
        UnitParsing.UnitParser unitParser;

        public AllCartridgeSolutions()
        {
            DBData.GenericCartridges.ToList().ForEach( o => AllCartridges.Add(new _BoardCartridge(o) ));
            unitParser = new UnitParsing.UnitParser();
        }


        public decimal ValidateAvailableLength(String availableLength)
        {
            decimal AvailableLength = unitParser.ConvertLengthExpressionToDecimalInch(availableLength);
            if (AvailableLength > Settings.LongestDistanceToCalculate) { throw new CartridgePickerExceptions.RequestedLengthTooLong(); }
            return AvailableLength;
        }

        public Decimal LongestCartridgeLength
        {
            get { return AllCartridges.OrderByDescending(o => o.Length).First().Length; }
        }       

        public void GenerateAllPermutations()
        {
            if(RequestedLength < LongestCartridgeLength + 0.25m)
            {               
                _BoardCartridge SolutionCartridge = AllCartridges.Where(o => o.Length < RequestedLength).OrderByDescending(n => n.Length).First();
                Solutions.Add(new _BoardCartridgesLayout(SolutionCartridge, RequestedLength, SplitGap) { });
                return;
            }
            else
            {
                List<_BoardCartridge> FirstRoundCartridges = AllCartridges.GroupBy(o => o.Serie).Select(n => n.OrderByDescending(r => r.Length).First()).ToList();
                FirstRoundCartridges.Add(AllCartridges.Where(o => o.Serie == 1 && o.InSerieID == 17).First());
                List<_BoardCartridge> LongestOfSeries = AllCartridges.GroupBy(o => o.Serie).Select(n => n.OrderByDescending(r => r.Length).First()).ToList();


                for (Int32 CurrentSolutionIndex = 0; CurrentSolutionIndex < 56; CurrentSolutionIndex++)
                {
                    _BoardCartridgesLayout CurrentSolution = new _BoardCartridgesLayout(RequestedLength, SplitGap);
                    _BoardCartridge FirstColumnCartridge, SecondColumnCartridge, ThirdColumnCartridge, FourthColumnCartridge;

                    //FirstColumn
                    Int32 FirstColumnCartridgeIndex = (Int32)Math.Floor(Decimal.Parse(CurrentSolutionIndex.ToString()) / 7m);
                    FirstColumnCartridge = FirstRoundCartridges[FirstColumnCartridgeIndex];
                    CurrentSolution.AddCartridge(FirstColumnCartridge);

                    //SecondColumn
                    if (RequestedLength - CurrentSolution.MyLength < LongestCartridgeLength)
                    {
                        try
                        {
                            SecondColumnCartridge = AllCartridges.Where(o => o.Length < RequestedLength - CurrentSolution.MyLength).OrderByDescending(n => n.Length).First();
                            CurrentSolution.AddCartridge(SecondColumnCartridge);
                            Solutions.Add(CurrentSolution);
                            continue;
                        }
                        catch
                        {
                            SecondColumnCartridge = null;
                        }
                    }
                    else
                    {
                        Int32 SecondColumnCartridgeIndex = (Int32)Math.Floor(Decimal.Parse(CurrentSolutionIndex.ToString()) % 7m);
                        SecondColumnCartridge = LongestOfSeries[SecondColumnCartridgeIndex];
                        CurrentSolution.AddCartridge(SecondColumnCartridge);
                    }                    

                    //ThirdColumn
                    if (RequestedLength - CurrentSolution.MyLength < LongestCartridgeLength)
                    {
                        try
                        {
                            ThirdColumnCartridge = AllCartridges.Where(o => o.Length < RequestedLength - CurrentSolution.MyLength).OrderByDescending(n => n.Length).First();
                            CurrentSolution.AddCartridge(ThirdColumnCartridge);
                            Solutions.Add(CurrentSolution);
                            continue;
                        }
                        catch
                        {
                            ThirdColumnCartridge = null;
                        }
                    }
                    else
                    {
                        Int32 ThirdColumnCartridgeIndex = (Int32)Math.Floor(Decimal.Parse(CurrentSolutionIndex.ToString()) % 7m);
                        ThirdColumnCartridge = LongestOfSeries[ThirdColumnCartridgeIndex];
                        CurrentSolution.AddCartridge(ThirdColumnCartridge);
                    }

                    //FourthColumn
                    if (RequestedLength - CurrentSolution.MyLength < LongestCartridgeLength)
                    {
                        try
                        {
                            FourthColumnCartridge = AllCartridges.Where(o => o.Length < RequestedLength - CurrentSolution.MyLength).OrderByDescending(n => n.Length).First();
                            CurrentSolution.AddCartridge(FourthColumnCartridge);
                            Solutions.Add(CurrentSolution);
                            continue;
                        }
                        catch
                        {
                            FourthColumnCartridge = null;
                        }
                    }
                    else
                    {
                        Int32 FourthColumnCartridgeIndex = (Int32)Math.Floor(Decimal.Parse(CurrentSolutionIndex.ToString()) % 7m);
                        FourthColumnCartridge = LongestOfSeries[FourthColumnCartridgeIndex];
                        CurrentSolution.AddCartridge(FourthColumnCartridge);
                    }

                    Solutions.Add(CurrentSolution);
                }
            } 
        }       

        public String SummarizeAllScores(Decimal requestedLength, Boolean splitGap)
        {
            RequestedLength = requestedLength;
            SplitGap = splitGap;
            GenerateAllPermutations();
            return String.Join(Environment.NewLine, Solutions.OrderBy(n=> n.Items[0].Serie).Select(o=>o.CartridgeScoreSummary).ToList());
        }

        public String SummarizeBestSolution(Decimal requestedLength, Boolean splitGap)
        {
            RequestedLength = requestedLength;
            SplitGap = splitGap;
            GenerateAllPermutations();
            Int32 TopScore = Solutions.OrderByDescending(n => n.MyScore).First().MyScore;
            return Solutions.Where(n => n.MyScore == TopScore).OrderBy(o=> o.GapLength).ThenBy(n=> n.Items[0].Serie).Select(o => o.CartridgeScoreSummary).First();
        }

        public String BestSolutionCartridges(Decimal requestedLength, Boolean splitGap)
        {
            RequestedLength = requestedLength;
            SplitGap = splitGap;
            GenerateAllPermutations();
            Int32 TopScore = Solutions.OrderByDescending(n => n.MyScore).First().MyScore;
            return Solutions.Where(n => n.MyScore == TopScore).OrderBy(o => o.GapLength).ThenBy(n => n.Items[0].Serie).Select(o => o.CartridgesNamesOnlySummary).First();
        }

        public Decimal ShortestCartridgeLength
        {
            get { return AllCartridges.OrderBy(o => o.Length).First().Length; }
        }

        public _BoardCartridgesLayout Solve(Decimal requestedLength, Boolean splitGap)
        {            
            if (requestedLength >= ShortestCartridgeLength)
            {
                RequestedLength = requestedLength;
                SplitGap = splitGap;
                GenerateAllPermutations();
                Int32 TopScore = Solutions.OrderByDescending(n => n.MyScore).First().MyScore;
                return Solutions.Where(n => n.MyScore == TopScore).OrderBy(o => o.GapLength).ThenBy(n => n.Items[0].Serie).First();
            }
            else
            {
                throw new CartridgePickerExceptions.RequestedLengthTooShort();
            }
        }
    }

    public class _BoardCartridgesLayout
    {       
        public List<_BoardCartridge> Items = new List<_BoardCartridge>();
        Decimal RequestedLength;
        Boolean SplitGap;

        public _BoardCartridgesLayout()
        {
        }

        public _BoardCartridgesLayout( Decimal requestedLength, Boolean splitGap)
        {
            RequestedLength = requestedLength;
            SplitGap = splitGap;      
        }
        public _BoardCartridgesLayout(_BoardCartridge NewCartridge, Decimal requestedLength, Boolean splitGap)
        {
            RequestedLength = requestedLength;
            SplitGap = splitGap;
            Items.Add(NewCartridge);
        }

        public _BoardCartridgesLayout(List<_BoardCartridge> NewCartridges, Decimal requestedLength, Boolean splitGap)
        {
            RequestedLength = requestedLength;
            SplitGap = splitGap;
            Items.AddRange(NewCartridges);
        }

        public void AddCartridge(_BoardCartridge newCartridge)
        {
            Items.Add(newCartridge);
        }        

        public Int32 GetMyScore(decimal availableLength, Boolean splitGap = false)
        {
            return StartingScore + Scoring_PreferenceBySeries + Scoring_NeedsCutting + Score_DiscriminateAgainstShortCartridges + Scoring_GapScore + MinimumDarkspotScore;
        }

        public Decimal MyLength
        {
            get { return Items.Select(o => o.Length).Sum(); }
        }       

        public decimal GapLength
        {
            get { return SplitGap ? (RequestedLength - MyLength) / 2 : RequestedLength - MyLength; }
        }        

        public Int32 Scoring_GapScore
        {
            get
            {
                Dictionary<decimal, Int32> Ranges = new Dictionary<decimal, Int32>
                {
                    { -1000m, 0 },
                    { 0.25m, -1 },
                    { 0.3125m, -3 },
                    { 1m, -6 }
                };
                return Ranges.Where(o => (GapLength) > o.Key).OrderBy(o => o.Value).First().Value;
            }
        }

        public Int32 MinimumDarkspotScore
        {
            get { return GapLength < ((CartridgeCount + 1) * Settings.BEAM.InstallationGap) ? -10 : 0; }
        }

        public Int32 CartridgeCount
        {
            get { return Items.Count(); }
        }

        public Int32 Scoring_PreferenceBySeries
        {

            get{ return Items.Any(o => o.Serie == 5 || o.Serie == 6 || o.Serie == 7) ? 1 : 0; }
        }

        public Int32 Scoring_NeedsCutting
        {
            get{return Items.Any(o => o.NeedsCutting) ? 0 : 1; }
        }

        public Int32 Score_DiscriminateAgainstShortCartridges
        {
            get { return Items.Any(o => o.Length < Settings.BEAM.ShortestAcceptableCartridge) ? -1 : 1; }
        }

        public Int32 MyScore
        {
            get { return StartingScore + Scoring_PreferenceBySeries + Scoring_NeedsCutting + Score_DiscriminateAgainstShortCartridges + Scoring_GapScore + MinimumDarkspotScore; }
        }

        public Int32 StartingScore
        {
            get { return 1; }
        }

        public string CartridgesNamesOnlySummary
        {
            get { return String.Join(", ", Items.Select(o => o.Name).ToList()); }
        }        

        public string CartridgeScoreSummary
        {
            get { return String.Format(@"Contained Cartridges: {0}, OverallLength: {1}, MyGap: {2}, My Score: {3} (GapScore:{4}, NoSmallCartridge: {5}, NoCuts: {6}, Favour567: {7}, MinimumDarkSpot: {8})", CartridgesNamesOnlySummary, MyLength, GapLength, MyScore, Scoring_GapScore, Score_DiscriminateAgainstShortCartridges, Scoring_NeedsCutting, Scoring_PreferenceBySeries, MinimumDarkspotScore); }
        }   
        
        public string CartridgeSummary
        {
            get{ return String.Join(", ", Items.Select(o => o.Summary).ToList()); }
        }
    }

    public class _Board
    {
        GenericBoardsData Data;
        public _Board(GenericBoardsData genericBoardsData)
        {
            Data = genericBoardsData;
        }

        public String Name
        {
            get { return Data.Name; }
        }

        public Decimal Length
        {
            get { return (Decimal)Data.Length; }
        }
    }

    public class _BoardsLayout
    {
        public List<_Board> Items = new List<_Board>();
        CartridgeSolverDbDataConnection DBData = new CartridgeSolverDbDataConnection();
        _BoardCartridge MyCartridge;
        public _BoardsLayout(_BoardCartridge cartridge)
        {
            MyCartridge = cartridge;
            DBData.GenericBoardAtGenericCartridges.Where(o => o.GenericCartridge == MyCartridge.ID).OrderBy(n => n.id).ToList().ForEach(r => AddBoards(genericBoardID: r.GenericBoard,quantity: r.Quantity));
        }

        private void AddBoards(Int32 genericBoardID, Int32 quantity)
        {
            for(Int32 n = 0; n < quantity; n++)
            {
                GenericBoardsData currentGenericBoard = DBData.GenericBoardsDatas.Find(genericBoardID);
                Items.Add(new _Board(currentGenericBoard) );
            }
        }
    }    
}

