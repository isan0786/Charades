using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ServiceAssembly;
namespace server
{
    public partial class ServerUI : Form
    {
        int gameid;
        DSTableAdapters.clientsTableAdapter taClient;
        DSTableAdapters.gamesTableAdapter taGame;
        DSTableAdapters.gamelogsTableAdapter tagameLog;
        DSTableAdapters.gameresultsTableAdapter tagameresults;
        public ServerUI()
        {
            InitializeComponent();
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            taClient = new DSTableAdapters.clientsTableAdapter();
            tagameLog = new DSTableAdapters.gamelogsTableAdapter();
            tagameresults = new DSTableAdapters.gameresultsTableAdapter();

            int highScore = 0;
            string winner = "";
            string scoreBoard = Program.Service.getScores();
            foreach (Client c in Program.Service.Clients.Keys)
            {
                taClient.InsertClientProc(c.Name.Trim().ToUpper());
                tagameresults.InsertGameResultsQuery(gameid, c.Name.ToUpper(), c.Score);

                if (c.Score > highScore)
                {
                    winner = c.Name;
                }

                foreach (string word in c.PlayedWords)
                {
                    tagameLog.InsertWordLogQuery(gameid, word.ToUpper(), c.Name.ToUpper());
                }
            }

            //update game winner
            taGame.UpdateWinnerQuery(winner, gameid);

            //stop all clients
            Program.Service.stopServer();

            Console.WriteLine("Server Closed");
            btnStop.Enabled = false;

            Results rForm = new Results(winner, highScore, scoreBoard);
            this.Hide();
            rForm.Show();

        }

        private void ServerUI_Load(object sender, EventArgs e)
        {

            taGame = new DSTableAdapters.gamesTableAdapter();

            // insert game record on start
            gameid = (int)taGame.InsertNewGameQuery(DateTime.Now, "STUB");
            Console.WriteLine("gid " + gameid);
            lblGameId.Text = "Game ID:   " + gameid;
            this.Text = "Server [Game#: " + gameid + "]";
        }
    }
}
