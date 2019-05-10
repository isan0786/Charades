using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;


namespace ServiceAssembly
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "CommsService" in both code and config file together.
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single,
        ConcurrencyMode = ConcurrencyMode.Multiple,
        UseSynchronizationContext = false)]
    public class CommsService : ICommsService
    {
        private Game game;
        public Game Game
        {
            set { if (game == null) game = value; }
            get { return game; }
        }
        Dictionary<Client, ICommsServiceCallback> clients =
             new Dictionary<Client, ICommsServiceCallback>();


        object syncObj = new object();


        public ICommsServiceCallback CurrentCallback
        {
            get
            {
                return OperationContext.Current.
                       GetCallbackChannel<ICommsServiceCallback>();
            }
        }

        public Dictionary<Client, ICommsServiceCallback> Clients { get => clients; set => clients = value; }

        private Client SearchClientsByName(string name)
        {
            foreach (Client c in Clients.Keys)
            {
                if (c.Name == name)
                {
                    return c;
                }
            }
            return null;
        }
        public void DoWork(Client c, Game g)
        {
            c.Name = "Parteek";
        }

        public void Send(Message message)
        {
            //update game
            Console.WriteLine(message.Sender + ": " + message.Content);
            //update clients
            lock (syncObj)
            {
                try
                {

                    foreach (ICommsServiceCallback callback in Clients.Values)
                    {
                        callback.Receive(message);
                        Console.Write("+");
                    }
                }
                catch (Exception e)
                {
                    //lol
                }
                Console.WriteLine(".");
            }
        }

        public bool Connect(Client client)
        {
            if (game.Started)
            {
                return false;
            }

            if (!Clients.ContainsValue(CurrentCallback) && !(SearchClientsByName(client.Name) != null))
            {
                lock (syncObj)
                {
                    client.PlayedWords = new HashSet<string>();
                    Clients.Add(client, CurrentCallback);
                    game.addClient(client);

                    Console.WriteLine(client.Name + " joined");
                    foreach (Client key in Clients.Keys)
                    {
                        ICommsServiceCallback callback = Clients[key];
                        try
                        {

                            //   callback.RefreshClients(clientList);
                            // callback.UserJoin(client);
                        }
                        catch
                        {
                            //clients.Remove(key);
                            return false;
                        }

                    }

                }
                return true;
            }
            return false;
        }

        public void PlayWord(Client client, string word)
        {

            bool added = SearchClientsByName(client.Name).PlayedWords.Add(word);
            if (added)
            {
                SearchClientsByName(client.Name).Score += word.Length;
            }
            foreach (Client key in Clients.Keys)
            {
                ICommsServiceCallback callback = Clients[key];

                try
                {
                    callback.playedWord(client, word, added);//update chat tb
                    if (added)
                    {
                        callback.updateScore(getScores());//update score list
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }

            }

        }
        public string getScores()
        {
            StringBuilder sb = new StringBuilder();
            foreach (Client c in Clients.Keys)
            {
                sb.Append(c.Name + '`' + c.Score + ';');
            }
            return sb.ToString();
        }

        public void stopServer()
        {
            foreach (Client key in Clients.Keys)
            {
                ICommsServiceCallback callback = Clients[key];

                try
                {
                    callback.LogOut();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }

            }
        }
    }
}
