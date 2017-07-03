using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace WcfService4
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService2" in both code and config file together.
    [ServiceContract(CallbackContract = typeof(CallBack))]
    public interface IService2
    {
        [OperationContract]
        void DoWork();

        [OperationContract(IsOneWay = true)]
        void LogIn(string name, string email);


        [OperationContract(IsOneWay = true)]
        void SerchingForOponent(string playerString);

        [OperationContract(IsOneWay = true)]
        void sendMove(string sendMove);

        [OperationContract(IsOneWay = true)]
        void endGameServer(Game g, List<Move> MoveList);//(string endGameString);

        [OperationContract(IsOneWay = true)]
        void endTeamGameServer(Game game,Team winner, List<Move> AllMoves);

        [OperationContract(IsOneWay = true)]
        void endGameServer2(Game g);

        [OperationContract(IsOneWay = true)]
        void LogInTeam(string name);

        [OperationContract(IsOneWay = true)]
        void SerchingForOponentTeam(Team team);

        [OperationContract(IsOneWay = true)]
        void sendMoveTeam(Team team, Move move,string color);






    }


    public interface CallBack
    {
        [OperationContract(IsOneWay = true)]
        void CallBackFunc(string str);

        [OperationContract(IsOneWay = true)]
        void sendPlayer(string playerString);

        [OperationContract(IsOneWay = true)]
        void sendGame(string gameString);


        [OperationContract(IsOneWay = true)]
        void ReseveMove(string moveString,string color);

        [OperationContract(IsOneWay = true)]
        void ReseveMoveTeam(Move m, string color);

        [OperationContract(IsOneWay = true)]
        void sendTeam(Team t);

        [OperationContract(IsOneWay = true)]
        void sendTeamGame(Game game);
     


    }
}
