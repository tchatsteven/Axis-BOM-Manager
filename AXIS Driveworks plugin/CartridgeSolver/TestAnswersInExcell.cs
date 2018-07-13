using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Excel = Microsoft.Office.Interop.Excel;
using System.Reflection;
using System.Windows.Forms;
using System.Threading;

namespace CartridgeSolver
{
    class TestInExcel
    {
        public TestInExcel(ProgressBar progressBar)
        {
            //OPEN SOURCE BEAM EXCEL SOLVER DECLARATIONS
            Excel.Application SourceAppInstance;
            Excel._Workbook SourceWorkbook;
            Excel._Worksheet SourceSheet;

            //OPEN TARGET SHEET DECLARATIONS
            Excel.Application TargetAppInstance;
            Excel._Workbook TargetAppWorkbook;
            Excel._Worksheet TargetSheet;
        
            //OPEN SOURCE BEAM EXCEL SOLVER 
            SourceAppInstance = new Excel.Application();
            SourceWorkbook = SourceAppInstance.Workbooks.Open(@"Z:\RESEARCH + DEVELOPMENT\6 DW\Dev\AXIS Driveworks plugin\CartridgeSolver\Reference Excell Files\Beam Cartridge Configurator - Release 16 Feb 2018.xlsx");
            //SourceSheet = (Excel._Worksheet)SourceWorkbook.Sheets[0];
            SourceSheet = SourceWorkbook.ActiveSheet;

            //OPEN TARGET SHEET  
            TargetAppInstance = new Excel.Application();
            TargetAppWorkbook = (Excel._Workbook)(TargetAppInstance.Workbooks.Add(Missing.Value));
            TargetSheet = TargetAppWorkbook.ActiveSheet;
            

            Test(SourceSheet, TargetSheet, progressBar);


            TargetSheet.Cells.Columns["A:H"].AutoFit();
            
            SourceWorkbook.Close(0);
            SourceAppInstance.Quit();

            TargetAppInstance.Visible = true;// show excell after all the calculations are done. Muchos faster!
        }

        public void Test(Excel._Worksheet SourceWorksheet, Excel._Worksheet TargetWorksheet, ProgressBar progressBar)
        {
            int AnswerRow = 2;
            TargetWorksheet.Cells[1, 1] = "Length";
            TargetWorksheet.Cells[1, 2] = "Excel Solver";
            TargetWorksheet.Cells[1, 3] = "C# Solver";
            TargetWorksheet.Cells[1, 4] = "Answers Match";

            Decimal TestMinValue = 20m;
            Decimal TestMaxValue = 149.75m;
            Decimal TestPeriod = 1m /32m;

            progressBar.Invoke(new Action( () => progressBar.Minimum = Convert.ToInt32(TestMinValue)));
            progressBar.Invoke(new Action(() => progressBar.Maximum = Convert.ToInt32(TestMaxValue)));
            progressBar.Invoke(new Action(() => progressBar.Value = Convert.ToInt32(TestMinValue)));

            for (Decimal n= TestMinValue; n <= TestMaxValue; n += TestPeriod)
            {
                SourceWorksheet.Cells[8, 12] = n;

                List<String> CartridgeNames = new List<String>();

                for (int ci = 6; ci <= 9; ci++){CartridgeNames.Add(SourceWorksheet.Cells[ci, 18].Text);}

                TargetWorksheet.Cells[AnswerRow, 1] = n;
                TargetWorksheet.Cells[AnswerRow, 2] = String.Join("+", CartridgeNames.Where(o => o != "-").ToList());

                AllCartridgeSolutions Solution = new AllCartridgeSolutions();
                Solution.BestSolutionCartridges(n, true);

                TargetWorksheet.Cells[AnswerRow, 3] = Solution.BestSolutionCartridges(n, true);

                TargetWorksheet.Cells[AnswerRow, 4] = String.Format(@"=B{0}=C{0}", AnswerRow);

                AnswerRow++;

                progressBar.Invoke(new Action(() => progressBar.Value = Convert.ToInt32(n)));
            }

            progressBar.Invoke(new Action(() => progressBar.Value = Convert.ToInt32(TestMinValue)));
        }
    }
}
