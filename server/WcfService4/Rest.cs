using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace WcfService4
{
    [ServiceContract]
    public interface Rest
    {

       /* [OperationContract]
       // [WebGet(UriTemplate = "/RegisterPlayer/{name}/{email}")]
        string RegisterPlayer(string name, string email);*/


        [OperationContract]
        //[WebGet(UriTemplate = "/getAllPlayers")]
        string getAllPlayers();

        [OperationContract]
        //[WebGet(UriTemplate = "/getAllGames")]
        string getAllGames();

        [OperationContract]
        //[WebGet(UriTemplate = "/getAllGamesForCombo")]
        string getAllGamesForCombo();

        [OperationContract]
        //[WebGet(UriTemplate = "/getAllPlayersNames") ] 
        string getAllPlayersNames();



        [OperationContract]
        //[WebGet(UriTemplate = "/GetAllGameOfPlayer/{name}")]
        string GetAllGameOfPlayer(string name);

        [OperationContract]
        //[WebGet(UriTemplate = "/GetAllPlayersOfGame/{gameId}")]
        string GetAllPlayersOfGame(string gameId);


        [OperationContract]
        //[WebGet(UriTemplate = "/GetAllMovesOfGame/{gameId}")]
        string GetAllMovesOfGame(string gameId);

        [OperationContract]
        //[WebGet(UriTemplate = "/GetAllMovesOfGame/{name}/{email}/{teamName}")]
        string RegisterPlayerWithTeam(string name, string email, string teamName);

        [OperationContract]
        //[WebGet(UriTemplate = "/GetAllMovesOfGame/{name}/{email}/{teamName}")]
        string registerTeam(string teamName);

    }
}