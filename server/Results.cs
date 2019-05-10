using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace server
{
    public partial class Results : Form
    {
        int highScore;
        string winner;
        string scoreBoard;
        public Results(string winner, int highScore, string scoreBoard)
        {
            this.winner = winner;
            this.highScore = highScore;
            this.scoreBoard = scoreBoard;
            InitializeComponent();
        }

        private void Results_Load(object sender, EventArgs e)
        {
            AppendFormattedText(rtbScores, "Winner: ", Color.Black, false);
            AppendFormattedText(rtbScores, winner, Color.DarkBlue, true);
            AppendFormattedText(rtbScores, " [" + highScore + "]" + Environment.NewLine, Color.DarkBlue, false);
            updateScoreRTB(scoreBoard);
        }


        internal void updateScoreRTB(string s)
        {
            //rtbScores.Text = "";
            String[] scores = s.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string score in scores)
            {
                string[] pair = score.Split(new string[] { "`" }, StringSplitOptions.RemoveEmptyEntries);
                AppendFormattedText(rtbScores, pair[0] + " \t", Color.DarkBlue, true);
                AppendFormattedText(rtbScores, pair[1] + Environment.NewLine, Color.Firebrick, false);
            }
        }

        private void AppendFormattedText(RichTextBox rtb, string text, Color textColour, Boolean isBold)
        {
            int start = rtb.TextLength;
            rtb.AppendText(text);
            int end = rtb.TextLength; // now longer by length of appended text

            // Select text that was appended
            rtb.Select(start, end - start);


            rtb.SelectionColor = textColour;
            rtb.SelectionFont = new Font(
                 rtb.SelectionFont.FontFamily,
                 rtb.SelectionFont.Size,
                 (isBold ? FontStyle.Bold : FontStyle.Regular));


            // Unselect text
            rtb.ScrollToCaret();
            rtb.SelectionLength = 0;
        }

        private void tbClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
