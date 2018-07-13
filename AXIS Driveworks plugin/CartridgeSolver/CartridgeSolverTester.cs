using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CartridgeSolver;
using System.Threading;

namespace BeamCartridgePicker
{
    public partial class CartridgePickerTesterForm : Form
    {
        public CartridgePickerTesterForm()
        {
            InitializeComponent();
        }

        private void Solve_Click(object sender, EventArgs e)
        {
            AllCartridgeSolutions Solution = new AllCartridgeSolutions();
            MainTextArea.Text = Solution.SummarizeBestSolution(Decimal.Parse(RequestedLengthTextBox.Text), SplitGapCheckBox.Checked);
        }

        private void RequestedLengthTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                Solve_Click(sender, e);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            AllCartridgeSolutions Solution = new AllCartridgeSolutions();
            MainTextArea.Text = Solution.SummarizeAllScores(Decimal.Parse(RequestedLengthTextBox.Text), SplitGapCheckBox.Checked);
        }

        private void TextInExcelButton_Click(object sender, EventArgs e)
        {
            //Thread t = new Thread(() => new CartridgeSolver.TestInExcel(TestProgressBar));
            //t.Start();
        }
    }
}
