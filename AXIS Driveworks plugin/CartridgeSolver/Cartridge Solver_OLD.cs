using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cartridge_Calculator
{
    static class Settings
    {
        public static decimal AcceptableGap = 0.25m;
        public static int NomOfCalculationRounds = 4;
        public static decimal ShortestAcceptableCartridge = 20m;
        public static decimal InstallationGap = 0.02m;
        public static decimal LongestDistanceToCalculate = 149.875m;
    }

    public class Cartridge_Definitions
    {
        public Cartridge_Definitions()//Class Constructor
        {
            PopulateLists();
        }

        public Cartridge GetLongestCartridge(decimal RequestedLength = -100) //Function Returns the longests cartridge according to the requested length. If no length is specified, returens the longest of all. 
        {
            if (RequestedLength == -100)
            {
                return AllCartridges.OrderByDescending(x => x.Length).First();
            }
            else
            {
                IEnumerable<Cartridge> test = AllCartridges.Where(o => o.Length < RequestedLength);
                if (test.ToList().Count != 0)
                {
                    return test.OrderByDescending(x => x.Length).First();
                }
                else
                {
                    return null;
                }
            }
        }
        public Cartridge GetShortestCartridge()
        {
            return AllCartridges.OrderByDescending(x => x.Length).Last();
        }

        public decimal GetLongestCartridgeLengthWithAcceptableGap(decimal RequestedLength = 0)
        {
            if (GetLongestCartridge() != null)
            {
                return GetLongestCartridge().Length + Settings.AcceptableGap;
            }
            else
            {
                return 0m;
            }
        }

        public List<Cartridge> AllCartridges = new List<Cartridge>(); //Declare List for All Cartridges
        public List<Cartridge> LongestOfSeries = new List<Cartridge>(); //Declare List for longest Cartridges per type (C1, C2, C3, C4...)
        public List<Cartridge> EvaluateIn1StSeriesOnly = new List<Cartridge>();// Cartridge to add as possibility, but only in 1st round

        public void PopulateLists() //Put Data into lists defined above
        {
            AllCartridges.Add(new Cartridge(Name: "C4-1", Length: 11.433m, BoardSetup: "100"));
            AllCartridges.Add(new Cartridge(Name: "C1-1", Length: 13.361m, BoardSetup: "101"));
            AllCartridges.Add(new Cartridge(Name: "C1-2", Length: 15.281m, BoardSetup: "102"));
            AllCartridges.Add(new Cartridge(Name: "C1-3", Length: 17.201m, BoardSetup: "103"));
            AllCartridges.Add(new Cartridge(Name: "C1-4", Length: 19.121m, BoardSetup: "104"));
            AllCartridges.Add(new Cartridge(Name: "C1-5", Length: 21.042m, BoardSetup: "105"));
            AllCartridges.Add(new Cartridge(Name: "C4-2", Length: 22.874m, BoardSetup: "200"));
            AllCartridges.Add(new Cartridge(Name: "C5-1", Length: 23.95m, BoardSetup: "010"));
            AllCartridges.Add(new Cartridge(Name: "C6-1", Length: 23.969m, BoardSetup: "010"));
            AllCartridges.Add(new Cartridge(Name: "C1-6", Length: 24.802m, BoardSetup: "201"));
            AllCartridges.Add(new Cartridge(Name: "C2-1", Length: 25.873m, BoardSetup: "011"));
            AllCartridges.Add(new Cartridge(Name: "C1-7", Length: 26.772m, BoardSetup: "202"));
            AllCartridges.Add(new Cartridge(Name: "C2-2", Length: 27.793m, BoardSetup: "012"));
            AllCartridges.Add(new Cartridge(Name: "C1-8", Length: 28.642m, BoardSetup: "203"));
            AllCartridges.Add(new Cartridge(Name: "C2-3", Length: 29.713m, BoardSetup: "013"));
            AllCartridges.Add(new Cartridge(Name: "C1-9", Length: 30.562m, BoardSetup: "204"));
            AllCartridges.Add(new Cartridge(Name: "C2-4", Length: 31.633m, BoardSetup: "014"));
            AllCartridges.Add(new Cartridge(Name: "C1-10", Length: 32.483m, BoardSetup: "205"));
            AllCartridges.Add(new Cartridge(Name: "C2-5", Length: 33.545m, BoardSetup: "015"));
            AllCartridges.Add(new Cartridge(Name: "C4-3", Length: 34.307m, BoardSetup: "300", NeedsCutting: false));
            AllCartridges.Add(new Cartridge(Name: "C7-1", Length: 35.397m, BoardSetup: "110"));
            AllCartridges.Add(new Cartridge(Name: "C1-11", Length: 36.243m, BoardSetup: "301"));
            AllCartridges.Add(new Cartridge(Name: "C3-8", Length: 37.313m, BoardSetup: "111"));
            AllCartridges.Add(new Cartridge(Name: "C1-12", Length: 38.163m, BoardSetup: "302"));
            AllCartridges.Add(new Cartridge(Name: "C3-9", Length: 39.233m, BoardSetup: "112"));
            AllCartridges.Add(new Cartridge(Name: "C1-13", Length: 40.083m, BoardSetup: "303"));
            AllCartridges.Add(new Cartridge(Name: "C3-10", Length: 41.153m, BoardSetup: "113"));
            AllCartridges.Add(new Cartridge(Name: "C1-14", Length: 42.003m, BoardSetup: "304"));
            AllCartridges.Add(new Cartridge(Name: "C3-11", Length: 43.073m, BoardSetup: "114"));
            AllCartridges.Add(new Cartridge(Name: "C1-15", Length: 43.924m, BoardSetup: "305"));
            AllCartridges.Add(new Cartridge(Name: "C3-12", Length: 44.994m, BoardSetup: "115"));
            AllCartridges.Add(new Cartridge(Name: "C1-16", Length: 45.764m, BoardSetup: "400"));
            AllCartridges.Add(new Cartridge(Name: "C7-2", Length: 46.841m, BoardSetup: "210", NeedsCutting: false));
            AllCartridges.Add(new Cartridge(Name: "C1-17", Length: 47.684m, BoardSetup: "401"));
            AllCartridges.Add(new Cartridge(Name: "C5-2", Length: 47.9m, BoardSetup: "020", NeedsCutting: false));
            AllCartridges.Add(new Cartridge(Name: "C6-2", Length: 47.938m, BoardSetup: "020", NeedsCutting: false));
            AllCartridges.Add(new Cartridge(Name: "C3-2", Length: 48.757m, BoardSetup: "211"));
            AllCartridges.Add(new Cartridge(Name: "C1-18", Length: 49.604m, BoardSetup: "402"));
            AllCartridges.Add(new Cartridge(Name: "C2-6", Length: 49.817m, BoardSetup: "021"));
            AllCartridges.Add(new Cartridge(Name: "C3-3", Length: 50.677m, BoardSetup: "212"));
            AllCartridges.Add(new Cartridge(Name: "C1-19", Length: 51.524m, BoardSetup: "403"));
            AllCartridges.Add(new Cartridge(Name: "C2-7", Length: 51.737m, BoardSetup: "022"));
            AllCartridges.Add(new Cartridge(Name: "C3-4", Length: 52.597m, BoardSetup: "213"));
            AllCartridges.Add(new Cartridge(Name: "C1-20", Length: 53.444m, BoardSetup: "404"));
            AllCartridges.Add(new Cartridge(Name: "C2-8", Length: 53.657m, BoardSetup: "023"));
            AllCartridges.Add(new Cartridge(Name: "C3-5", Length: 54.517m, BoardSetup: "214"));
            AllCartridges.Add(new Cartridge(Name: "C1-21", Length: 55.365m, BoardSetup: "405", NeedsCutting: false));
            AllCartridges.Add(new Cartridge(Name: "C2-9", Length: 55.577m, BoardSetup: "024"));
            AllCartridges.Add(new Cartridge(Name: "C3-6", Length: 56.438m, BoardSetup: "215", NeedsCutting: false));
            AllCartridges.Add(new Cartridge(Name: "C2-10", Length: 57.489m, BoardSetup: "025", NeedsCutting: false));


            LongestOfSeries.Add(new Cartridge(Name: "C1-21", Length: 55.365m, BoardSetup: "405", NeedsCutting: false));
            LongestOfSeries.Add(new Cartridge(Name: "C2-10", Length: 57.489m, BoardSetup: "025", NeedsCutting: false));
            LongestOfSeries.Add(new Cartridge(Name: "C3-6", Length: 56.438m, BoardSetup: "215", NeedsCutting: false));
            LongestOfSeries.Add(new Cartridge(Name: "C4-3", Length: 34.307m, BoardSetup: "300", NeedsCutting: false));
            LongestOfSeries.Add(new Cartridge(Name: "C5-2", Length: 47.900m, BoardSetup: "020", NeedsCutting: false));
            LongestOfSeries.Add(new Cartridge(Name: "C6-2", Length: 47.938m, BoardSetup: "020", NeedsCutting: false));
            LongestOfSeries.Add(new Cartridge(Name: "C7-2", Length: 46.841m, BoardSetup: "210", NeedsCutting: false));
            EvaluateIn1StSeriesOnly.Add(new Cartridge(Name: "C1-17", Length: 47.684m, BoardSetup: "401"));
        }
    }

    public class Cartridge : Part//Cartridge Definition Class 
    {
        //private string name;
        private decimal length;
        private bool needsCutting;
        public List<LEDBoard> LEDBoards = new List<LEDBoard>();

        public Cartridge(string Name, decimal Length, string BoardSetup, bool NeedsCutting = true) : base(Name)
        {
            //this.Name = Name;
            length = Length;
            boardSetup = BoardSetup;
            this.needsCutting = NeedsCutting;
        }

        public decimal Length
        {
            get { return length; }
            set { length = value; }
        }

        public bool NeedsCutting
        {
            get { return this.needsCutting; }
            set { this.needsCutting = value; }
        }

        public string boardSetup
        {
            get
            {
                List<string> BoardSetupList = new List<string>();
                if (LEDBoards.Count(i => i.Name == "L1") != 0) { BoardSetupList.Add(LEDBoards.Count(i => i.Name == "L1").ToString() + "L1"); }
                if (LEDBoards.Count(i => i.Name == "L2") != 0) { BoardSetupList.Add(LEDBoards.Count(i => i.Name == "L2").ToString() + "L2"); }
                if (LEDBoards.Count(i => i.Name == "S2") != 0) { BoardSetupList.Add(LEDBoards.Count(i => i.Name == "S2").ToString() + "S2"); }

                return String.Join("+", BoardSetupList);
            }
            set
            {
                char[] BoardsArray = value.ToCharArray();
                foreach (int index in Enumerable.Range(0, Int32.Parse(BoardsArray[0].ToString()))) { this.LEDBoards.Add(new LEDBoard("L1")); }
                foreach (int index in Enumerable.Range(0, Int32.Parse(BoardsArray[1].ToString()))) { this.LEDBoards.Add(new LEDBoard("L2")); }
                foreach (int index in Enumerable.Range(0, Int32.Parse(BoardsArray[2].ToString()))) { this.LEDBoards.Add(new LEDBoard("S2")); }
            }
        }

        public int NumberOfParallelConnections
        {
            get
            {
                List<int> NumberOfParallelConnections = new List<int>();
                if (LEDBoards.Count(i => i.Name == "L1") != 0) { NumberOfParallelConnections.Add(LEDBoards.Count(i => i.Name == "L1") * 6); }
                if (LEDBoards.Count(i => i.Name == "L2") != 0) { NumberOfParallelConnections.Add(LEDBoards.Count(i => i.Name == "L2") * 12); }
                if (LEDBoards.Count(i => i.Name == "S2") != 0) { NumberOfParallelConnections.Add(LEDBoards.Count(i => i.Name == "S2")); }
                return NumberOfParallelConnections.Sum();
            }
        }
    }

    public class LEDBoard : Part
    {
        public LEDBoard(string Name) : base(Name)
        {
        }
    }

    public class Part
    {
        private string name; // partName; 
        public Part(string Name)
        {
            this.name = Name;
        }

        public string Name { get { return this.name; } set { this.name = value; } }
        //public string PartName {  get { return this.partName; }  set { this.partName = value; } }
    }

    public class CartridgeSolver
    {
        List<Round> rounds = new List<Round>();

        public void SolveFor(string AvailableExtrusionLength = "0", Boolean SolveForLastSection = false)
        {
            Cartridge_Definitions CD = new Cartridge_Definitions();
            decimal availableExtrusionLength = this.ValidateInput(AvailableExtrusionLength);

            if (availableExtrusionLength < CD.GetShortestCartridge().Length)
            {
                Console.Clear();
                Console.WriteLine("Requested length of " + availableExtrusionLength + " inches is shorter than shortest cartridge.");
                return;
            }
            else if (availableExtrusionLength > Settings.LongestDistanceToCalculate)
            {
                Console.Clear();
                Console.WriteLine("Maximum length for calculation is " + Settings.LongestDistanceToCalculate + " inches limited by the max length of extrusion.");
                return;
            }

            while (this.rounds.Count < Settings.NomOfCalculationRounds)
            {
                Round PreviousRound = null;
                if (this.rounds.Count != 0) { PreviousRound = this.rounds[this.rounds.Count - 1]; }
                this.rounds.Add(new Round(availableExtrusionLength, PreviousRound, SolveForLastSection));
            }
        }

        public string PrintReport()
        {
            if (rounds.Count != 0)
            {
                Console.Clear();
                int Counter = 0;
                Round R = this.rounds[this.rounds.Count - 1]; //get latest round

                //List<Solution> OrderedSolutions = R.Solutions.OrderBy(o => o.Gap).Take(10).ToList();//order list by gap length; keep only top 10 results
                List<Solution> OrderedSolutions = R.Solutions.OrderBy(o => o.Score + (1 - o.Gap)).ToList();//order list by gap length;

                Solution BestSolution = R.Solutions.OrderByDescending(o => o.Score + (1 - o.Gap)).First();

                Console.WriteLine("========================\n========================");

                String Header = String.Format("{0, -26}{1, -15}{2, -8}{3, -12}{4, -11}{5,-10}{6,-13}{7, -15}{8, 7}", "Name", "Total Length", "Gap", "Gap Score", "Smallest C", "Needs Cut", "FavourC5C6C7", "Min. DarkSpot", "Total");
                Console.WriteLine(Header);

                foreach (Solution S in OrderedSolutions)
                {
                    String Name = String.Join(" ", S.Items.Select(o => o.Name));//24 chars                    
                    String TotalLength = S.Items.Sum(ll => ll.Length).ToString(); //7 chars
                    String Gap = S.Gap.ToString().ToString(); //5 chars
                    String GapScore = S.GapScore.ToString();
                    String SmallestCartridge = S.ShortestCartridgeScore.ToString();
                    String NeedsCutting = S.NeedsCuttingScore.ToString();
                    String FavourC5C6C7 = S.FavourC5C6C7Score.ToString();
                    String MinimumDarkSpotScore = S.MinimumDarkSpotScore.ToString();
                    String TotalScore = S.Score.ToString();

                    String result = String.Format("{0, -26}{1, -15}{2, -8}{3, -12}{4, -11}{5,-10}{6,-13}{7, -15}{8, 7}", Name, TotalLength, Gap, GapScore, SmallestCartridge, NeedsCutting, FavourC5C6C7, MinimumDarkSpotScore, TotalScore);
                    Console.WriteLine(result);
                    Counter++;
                }
                return "For the requested length of: " + BestSolution.AvailableLength + "\n" + BestSolution.Name + " @ " + BestSolution.Length + "\" with the score of: " + BestSolution.Score + " and having the smallest gap from all with same score";
            }
            else
            {
                return "No possible setup for requested length";
            }
        }

        public Solution GetSolution()
        {
            if (rounds.Count != 0)
            {
                Round R = this.rounds[this.rounds.Count - 1]; //get latest round  
                return R.Solutions.OrderByDescending(o => o.Score + (1 - o.Gap)).Take(56).First();
            }
            else
            {
                return new Solution();
            }
        }

        private decimal ValidateInput(string Input)
        {
            try
            {
                decimal InputDecimal = Convert.ToDecimal(Input);
                //return 150 >= InputDecimal && 0 < InputDecimal ? InputDecimal : 0;
                return InputDecimal;
            }
            catch
            {
                return 0;
            }
        }
    }

    public class Round
    {
        public List<Solution> Solutions = new List<Solution>();
        public Cartridge_Definitions CD = new Cartridge_Definitions();

        public Round(decimal AvailableExtrusionLength, Round PreviousRound, Boolean SolveForLastSection = false) //Constructor add 
        {
            this.SolveFor(AvailableExtrusionLength, PreviousRound, SolveForLastSection);
        }

        public void SolveFor(decimal AvailableExtrusionLength, Round PreviousRound = null, Boolean SolveForLastSection = false)
        {
            if (PreviousRound == null)
            {
                if (CD.GetLongestCartridgeLengthWithAcceptableGap() > AvailableExtrusionLength)
                {
                    Solution S = new Solution();
                    S.SolveForLastSection = SolveForLastSection;
                    S.Add(CD.GetLongestCartridge(AvailableExtrusionLength));
                    S.IsSolved = true;
                    S.AvailableLength = AvailableExtrusionLength;
                    this.Solutions.Add(S);
                }
                else
                {
                    foreach (Cartridge C in CD.LongestOfSeries)
                    {
                        Solution S = new Solution();
                        S.SolveForLastSection = SolveForLastSection;
                        S.Add(C);
                        S.AvailableLength = AvailableExtrusionLength;
                        this.Solutions.Add(S);
                    }
                    foreach (Cartridge CC in CD.EvaluateIn1StSeriesOnly)
                    {
                        Solution SS = new Solution();
                        SS.SolveForLastSection = SolveForLastSection;
                        SS.Add(CC);
                        SS.AvailableLength = AvailableExtrusionLength;
                        this.Solutions.Add(SS);
                    }
                }
            }
            else
            {
                foreach (Solution oldS in PreviousRound.Solutions)
                {

                    Solution S = new Solution();
                    S.SolveForLastSection = SolveForLastSection;
                    S.AvailableLength = AvailableExtrusionLength;
                    foreach (Cartridge C in oldS.Items) { S.Add(C); }//transfer cartridges from old solution

                    bool TooShort = (AvailableExtrusionLength - S.Length) < CD.GetShortestCartridge().Length;

                    if (oldS.IsSolved != true && !TooShort)
                    {
                        if (CD.GetLongestCartridge(AvailableExtrusionLength - S.Length).Length < CD.GetLongestCartridge().Length)
                        {
                            S.Add(CD.GetLongestCartridge(AvailableExtrusionLength - S.Length));
                            S.AvailableLength = AvailableExtrusionLength;
                            S.IsSolved = true;
                            this.Solutions.Add(S);
                        }
                        else
                        {
                            foreach (Cartridge CC in CD.LongestOfSeries)
                            {
                                Solution SS = new Solution();
                                SS.SolveForLastSection = SolveForLastSection;
                                foreach (Cartridge CCC in S.Items) { SS.Add(CCC); }
                                SS.Add(CC);
                                SS.AvailableLength = AvailableExtrusionLength;
                                this.Solutions.Add(SS);
                            }
                        }
                    }
                    else
                    {
                        S.IsSolved = true;
                        this.Solutions.Add(S);
                    }
                }
            }
        }
    }

    public class Solution
    {
        private Boolean solveForLastSection;
        public List<Cartridge> Items = new List<Cartridge>();
        public bool IsSolved = false;
        private decimal availableLength = 0;

        public Boolean SolveForLastSection { get { return this.solveForLastSection; } set { this.solveForLastSection = value; } }

        public decimal Length { get { return this.Items.Where(Item => Item != null).Sum(Item => Item.Length); } }

        public decimal Gap { get { return this.SolveForLastSection ? this.availableLength - this.Length : (this.availableLength - this.Length) / 2m; } }

        public decimal AvailableLength { get { return this.availableLength; } set { this.availableLength = value; } }

        //public string GetAmperages(string FixtureCode, double DesiredLumens, double DesiredCRI, string LensType)
        //{
        //    List<double> CartridgeAmperages = new List<double>();
        //    AmperageSolver TempAmperageSolver = new AmperageSolver(FixtureCode);

        //    foreach (Cartridge Cart in Items)
        //    {
        //        CartridgeAmperages.Add(TempAmperageSolver.SolveFor(DesiredLumens, DesiredCRI, LensType, Cart.NumberOfParallelConnections));
        //    }
        //    return String.Join("|", CartridgeAmperages);
        //}

        //public string GetNumberOfParallelConnections(string FixtureCode, double DesiredLumens, double DesiredCRI, string LensType)
        //{
        //    List<double> CartridgeAmperages = new List<double>();
        //    AmperageSolver TempAmperageSolver = new AmperageSolver(FixtureCode);

        //    foreach (Cartridge Cart in Items)
        //    {
        //        CartridgeAmperages.Add(Cart.NumberOfParallelConnections);
        //    }
        //    return String.Join("|", CartridgeAmperages);
        //}

        public decimal Score
        {
            get
            {
                List<decimal> Scores = new List<decimal>() { 1m, this.ShortestCartridgeScore, this.GapScore, this.NeedsCuttingScore, this.FavourC5C6C7Score, this.MinimumDarkSpotScore };
                return Scores.Sum();
            }
        }

        public decimal ShortestCartridgeScore
        {
            get
            {
                Decimal Score = this.Items.Where(Item => Item.Length < Settings.ShortestAcceptableCartridge).Count() > 0 ? -1m : 1m;
                return Score;
            }
        }

        public decimal GapScore
        {
            get
            {
                Dictionary<decimal, decimal> Ranges = new Dictionary<decimal, decimal>();
                Ranges.Add(0m, 0m);
                Ranges.Add(0.25m, -1m);
                Ranges.Add(0.3125m, -3m);
                Ranges.Add(1m, -6m);

                return Ranges.Where(o => (this.Gap) > o.Key).OrderBy(o => o.Value).First().Value;
            }
        }

        public decimal NeedsCuttingScore
        {
            get
            {
                return this.Items.Where(o => o.NeedsCutting == true).Count() != 0 ? 0m : 1m;
            }
        }

        public decimal FavourC5C6C7Score
        {
            get
            {
                return this.Items.Where(o => o.Name.Contains("C5") || o.Name.Contains("C6") || o.Name.Contains("C7")).Count() != 0 ? 1m : 0m;
            }
        }

        public decimal MinimumDarkSpotScore
        {
            get { return ((this.Items.Count() + 1) * Settings.InstallationGap) > (this.Gap) ? -10m : 0m; }
        }

        public string Name
        {
            get
            {
                return String.Join(" ", this.Items.Select(o => o.Name));
            }
        }

        public void Add(Cartridge C)
        {
            this.Items.Add(C);
        }
    }
}