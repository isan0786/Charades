using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using client.SVC;
using System.Data.SqlClient;

namespace client
{
    public partial class Form1 : Form
    {
        //SVC.CommsServiceClient serviceClient;
        //SVC.Client client;
        //SVC.Message message = new SVC.Message();

        TextBox[,] textBoxes;

        #region MyMethods


        void InitializeXtraComponent()
        {
            textBoxes = new TextBox[10, 10];
            lblName.Text = Program.Client.Name;
        }

        private TextBox createTextBox()
        {
            TextBox tb = new TextBox
            {
                MaxLength = 1,
                Cursor = System.Windows.Forms.Cursors.Cross,
                Anchor = AnchorStyles.Top,
                Dock = DockStyle.Fill,
            };

            tb.Anchor = AnchorStyles.Right;
            tb.Anchor = AnchorStyles.Bottom;
            tb.Anchor = AnchorStyles.Left;

            return tb;
        }

        public void sendMessage()
        {
            SVC.Message m = new SVC.Message();
            m.Sender = Program.Client.Name;
            m.Content = tbmessageSend.Text;
            Program.serviceClient.Send(m);
        }

        public void UpdateMesageList(SVC.Message m)
        {
            int len = rtbMessages.TextLength;
            rtbMessages.AppendText(m.Sender + ": " + m.Content + Environment.NewLine);

            Console.WriteLine(rtbMessages.TextLength);
            rtbMessages.Select(len, m.Sender.Length + 1);
            rtbMessages.SelectionFont = new Font(rtbMessages.Font, FontStyle.Bold);
            rtbMessages.SelectionColor = Color.BlueViolet;

            //  rtbMessages.SelectionStart = rtbMessages.TextLength;
            rtbMessages.ScrollToCaret();
        }

        public void ShowUserJoin(SVC.Client c)
        {
            rtbMessages.AppendText(Environment.NewLine + rtbMessages.Text + c.Name + " \\bjoined\\b0 " + Environment.NewLine);
        }

        internal void updateScoreRTB(string s)
        {
            rtbScores.Text = "";
            String[] scores = s.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string score in scores)
            {
                string[] pair = score.Split(new string[] { "`" }, StringSplitOptions.RemoveEmptyEntries);
                AppendFormattedText(rtbScores, pair[0] + " \t", Color.DarkBlue, true);
                AppendFormattedText(rtbScores, pair[1] + Environment.NewLine, Color.Firebrick, false);
            }
        }

        internal void TellPlayedWord(string name, string word, bool added)
        {
            AppendFormattedText(rtbMessages, "Server: ", Color.BlueViolet, true);
            if (added)
            {
                AppendFormattedText(rtbMessages, name + " played " + word, Color.DarkGreen, false);
            }
            else
            {
                AppendFormattedText(rtbMessages, name + " repeated " + word, Color.Maroon, false);
            }
            AppendFormattedText(rtbMessages, Environment.NewLine, Color.BlueViolet, true);
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

            rtb.ScrollToCaret();

            // Unselect text
            rtb.SelectionLength = 0;
        }


        public void DisableControls(Control con)
        {

            foreach (Control c in con.Controls)
            {
                DisableControls(c);
            }
            con.Enabled = false;
        }

        private void EnableControls(Control con)
        {
            if (con != null)
            {
                con.Enabled = true;
                EnableControls(con.Parent);
            }
        }
        #endregion


        public Form1()
        {

            InitializeComponent();
            InitializeXtraComponent();

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'dSW.words' table. You can move, or remove it, as needed.
            this.wordsTableAdapter.Fill(this.dSW.words);

            this.Text = "Client UI [" + Program.Client.Name + "]";

            for (int i = 0; i < textBoxes.GetLength(0); i++)
            {
                for (int j = 0; j < textBoxes.GetLength(1); j++)
                {
                    scribleGrid.Controls.Add(createTextBox(), i, j);
                }
            }
        }


        #region UI_Events




        private void btnSendMsg_Click(object sender, EventArgs e)
        {
            sendMessage();
            tbmessageSend.Clear();
        }


        private void btnSearch_Click(object sender, EventArgs e)
        {
            wordsBindingSource.Filter = wordsTableAdapter.GetData().Columns[0] + " like '%" + tbSearch.Text + "%'";
        }

        private void btnPlay_Click(object sender, EventArgs e)
        {
            string s = dataGridView1.CurrentRow.Cells[0].Value.ToString();
            Program.serviceClient.PlayWord(Program.Client, s);

        }

        internal void LogOut()
        {
            AppendFormattedText(rtbMessages, Environment.NewLine + Environment.NewLine + "Server: ", Color.BlueViolet, true);
            AppendFormattedText(rtbMessages, " Go HOME ", Color.DarkMagenta, true);
            DisableControls(this);
            EnableControls(btnPlay);
            btnPlay.Text = "EXIT";


            btnPlay.Click += new System.EventHandler(btnPlay_Close);
            btnPlay.Click -= btnPlay_Click;
        }

        private void btnPlay_Close(object sender, EventArgs e)
        {
            this.Close();
            Application.Exit();
        }


        #endregion
    }
}
