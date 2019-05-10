using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using client.SVC;

namespace client
{

    public class CallBackImlementation : SVC.ICommsServiceCallback
    {
        public void LogOut()
        {

            Program.form1.LogOut();
        }

        public void playedWord(Client c, string word, bool added)
        {
            Program.form1.TellPlayedWord(c.Name, word, added);
        }

        public void Receive(SVC.Message msg)
        {
            Program.form1.UpdateMesageList(msg);
        }

        public void RefreshClients(List<Client> clientList)
        {
            clientList = null;
        }

        public void StartGame(List<Client> clientList)
        {

        }

        public void updateScore(string s)
        {
            Console.WriteLine(s);
            Program.form1.updateScoreRTB(s);
           
            
        }

        public void UserJoin(Client client)
        {
            Program.form1.ShowUserJoin(client);
        }
    }
}
