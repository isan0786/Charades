using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;

namespace ServiceAssembly
{

    [DataContract]
    public class Client
    {
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public int Score { get; set; }
        public HashSet<string> PlayedWords { get => playedWords; set => playedWords = value; }

        public static bool operator |(Client a, Client b)
        {
            if (a.Name == b.Name)
            {
                return true;
            }
            return false;
        }

        HashSet<string> playedWords;

        public Client()
        {
            playedWords = new HashSet<string>();
            playedWords.Add("chalpya");
        }
    }

    [DataContract]
    public class Message
    {
        public Dictionary<string, string> content = new Dictionary<string, string>();

        [DataMember]
        public string Sender { get; set; }

        [DataMember]
        //public Dictionary<string, string> Content { get; set; }
        public string Content { get; set; }

    }

    [DataContract]
    public class Game
    {
        private List<Client> clientList = new List<Client>();
        private bool started = false;
        private Client currentTurn;
        public bool addClient(Client client)
        {
            if (!Started)
            {
                clientList.Add(client);
            }
            return !Started;
        }

        [DataMember]
        public List<Client> ClientList { get { return clientList; } }

        [DataMember]
        public bool Started { get => started; set => started = value; }
        public Client CurrentTurn { get => currentTurn; set => currentTurn = value; }
    }


}
