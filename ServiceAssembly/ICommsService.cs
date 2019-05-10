using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace ServiceAssembly
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "ICommsService" in both code and config file together.
    [ServiceContract (SessionMode = SessionMode.Required, 
        CallbackContract =typeof(ICommsServiceCallback))]
    public interface ICommsService
    {
        [OperationContract(IsOneWay = true)]
        void DoWork( Client c, Game g);

        [OperationContract(IsOneWay = true)]
        void Send(Message msg);

        [OperationContract(IsInitiating = true)]
        bool Connect(Client c);

        [OperationContract(IsOneWay = true)]
        void PlayWord(Client c, string word);
    }

    public interface ICommsServiceCallback
    {
        [OperationContract(IsOneWay = true)]
        void Receive( Message msg);


        [OperationContract(IsOneWay = true)]
        void StartGame(List<Client> clientList);

        [OperationContract(IsOneWay = true)]
        void RefreshClients(List<Client> clientList);

        [OperationContract(IsOneWay = true)]
        void UserJoin(Client client);


        [OperationContract(IsOneWay = true)]
        void updateScore(string s);

        [OperationContract(IsOneWay = true)]
        void playedWord(Client c, string word, bool added);

        [OperationContract(IsTerminating = true)]
        void LogOut();
    }
}
