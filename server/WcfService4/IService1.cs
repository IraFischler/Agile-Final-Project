using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace WcfService4
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IService1
    {

       [OperationContract]
        string RegisterPlayer(string name, string email);
       

        [OperationContract]
        List<Player> getAllPlayers();

        [OperationContract]
        List<Game> getAllGames();

        [OperationContract]
        List<string> getAllGamesForCombo();

        [OperationContract]
        List<string> getAllPlayersNames();    

        [OperationContract]
        List<Game> GetAllGameOfPlayer(string name);

        [OperationContract]
        List<Player> GetAllPlayersOfGame(int gameId);


        [OperationContract]
        List<Move> GetAllMovesOfGame(int gameId);

        [OperationContract]
        Game GetGameById(int id);



        //////for rest
        [OperationContract]
         string RegisterPlayerWithTeam(string name, string email, string teamName);

        [OperationContract]
        string registerTeam(string teamName);

        [OperationContract]
        string getAllPlayers2();

        [OperationContract]
        string getAllGames2();

        [OperationContract]
        string getAllGamesForCombo2();

        [OperationContract]
        string getAllPlayersNames2();

        [OperationContract]
        string GetAllGameOfPlayer2(string name);

        [OperationContract]
        string GetAllPlayersOfGame2(string gameId);


        [OperationContract]
        string GetAllMovesOfGame2(string gameId);

        [OperationContract]
        string GetGameById2(string id);

        [OperationContract]
        string CountPlayerGames();

        [OperationContract]
        string DeletePlayer(string name);
        [OperationContract]
        string DeleteGame(string name);

        [OperationContract]
        string UpdatePlayer(string data);





    }


    // Use a data contract as illustrated in the sample below to add composite types to service operations.
    [DataContract]
    public class CompositeType
    {
        bool boolValue = true;
        string stringValue = "Hello ";

        [DataMember]
        public bool BoolValue
        {
            get { return boolValue; }
            set { boolValue = value; }
        }

        [DataMember]
        public string StringValue
        {
            get { return stringValue; }
            set { stringValue = value; }
        }
    }
}
