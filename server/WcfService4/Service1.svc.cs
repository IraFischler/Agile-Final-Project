using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Data.SqlClient;
using System.Data;




namespace WcfService4
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class Service1 :  IService2, IService1
    {

        private const string connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\\Database1.mdf;Integrated Security=True";
        // private System.Data.Common.DbConnection con;// = DataBase.Connection();
        private SqlCommand cmd;



        /***/
 
        /***/


        /*
         soap - basic
         */

        public string RegisterPlayer(string name, string email)
        {

            // db.TblPlayers.InsertOnSubmit(new TblPlayer { } );
            // db.SubmitChanges();
            //list<TblPlayer> l  = db.TblPlayers.Where(p => p.PlayerId == 2);//coloction 

            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    //con = DataBase.Connection;
                    con.Open();
                    cmd = new SqlCommand("insert into TblPlayers(PlayerName,PlayerMail) values(@PlayerName,@PlayerMail)", con);
                    //cmd.Parameters.AddWithValue("@PlayerId", id);
                    cmd.Parameters.AddWithValue("@PlayerName", name);
                    cmd.Parameters.AddWithValue("@PlayerMail", email);
                    cmd.ExecuteNonQuery();
                    con.Close();

                    string s =  name + " " + email;//for dibbaging
                    return s;
                }
            }
            catch (Exception e)
            {
                return e.ToString();
            }
        }

        public List<Player> getAllPlayers()
        {
            return getPlayers("SELECT * FROM TblPlayers");
        }

        public List<string> getAllPlayersNames()
        {
            List<string> nList = new List<string>();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SELECT PlayerName FROM TblPlayers");
                try
                {
                    con.Open();
                    cmd.Connection = con;
                    var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        string n = reader["PlayerName"].ToString();
                        nList.Add(n);
                    }
                    con.Close();
                }
                catch (Exception ex)
                {
                    con.Close();
                    return null;
                }
                return nList;
            }

        }

        public List<Player> GetAllPlayersOfGame(int gameId)
        {
           return getPlayers("SELECT * FROM TblPlayers WHERE PlayerId IN (SELECT IdPlayer FROM TblPlayersGames WHERE IdGame =" + gameId +")");
        }

        public List<Game> getAllGames()
        {
            return getGames("SELECT * FROM TblGames");

        }
        public List<string> getAllGamesForCombo()
        {
            try
            {
                List<string> nList = new List<string>();
                List<Game> gl = getAllGames();
                foreach (Game g in gl)
                {
                    List<Player> pList = GetAllPlayersOfGame(g.Id);
                    string names = "";
                    foreach (Player p in pList) {
                        names += p.UserName + " ";
                    }
                    nList.Add(g.Id.ToString() + " " + g.StartTime + " " + names );// + " " + g.WinnerId.ToString() + " " + g.WinnerName);
                }
                return nList;
            }
            catch { return null; }
        }

        public List<Game> GetAllGameOfPlayer(string name)
        {
            return getGames(String.Format("SELECT * FROM TblGames WHERE GameId IN (SELECT IdGame FROM TblPlayersGames WHERE PlayerName = '" + name + "')"));
        }
        private List<Game> getGames(string query)
        {
            List<Game> gList = new List<Game>();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(query);
                try
                {
                    con.Open();
                    cmd.Connection = con;
                    var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Game g = new Game();
                        g.Id = Int32.Parse(reader["GameId"].ToString());
                        g.StartTime = reader["StartTime"].ToString();
                        //g.WinnerId = Int32.Parse(reader["WinnerId"].ToString());
                        //g.WinnerName = reader["WinnerName"].ToString();

                        gList.Add(g);
                    }
                    con.Close();
                }
                catch (Exception ex)
                {
                    con.Close();
                    return null;// ex.ToString();
                }
                return gList;//gList;
            }


        }


        private List<Player> getPlayers(string query)
        {
            /* var plist = from p in db.TblPlayers
                         select new Player { id = p.PlayerId };*/
            List<Player> pList = new List<Player>();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(query);//"SELECT * FROM TblPlayers");
                try
                {
                    con.Open();
                    cmd.Connection = con;
                    var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Player p = new Player();
                        p.Id = Int32.Parse(reader["PlayerId"].ToString());
                        p.UserName = reader["PlayerName"].ToString();
                        p.Email = reader["PlayerMail"].ToString();
                        pList.Add(p);
                    }
                    con.Close();
                }
                catch (Exception ex)
                {
                    con.Close();
                    return null;
                }
                return pList;
            }
        }

        public List<Move> GetAllMovesOfGame(int gameId)
        {
            List<Move> mList = new List<Move>();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM TblMoves WHERE GameId =" + gameId);//"SELECT * FROM TblPlayers");
                try
                {
                    int counter = 0;
                    con.Open();
                    cmd.Connection = con;
                    var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Move m = new Move()
                        {
                            MoveId = Int32.Parse(reader["MoveId"].ToString()),
                            GameOfMove = new Game() { Id = Int32.Parse(reader["GameId"].ToString()) },
                            PlayerOfMove = new Player() { id = Int32.Parse(reader["PlayerId"].ToString()) },
                            MoveString = reader["Move"].ToString()

                        };
                        if (counter % 2 == 0)
                            m.PlayerOfMove.Color = "Red";
                        else
                            m.PlayerOfMove.Color = "Bleck";

                        mList.Add(m);
                    }
                    con.Close();
                }
                catch (Exception ex)
                {
                    con.Close();
                    return null;
                }
                return mList;
            }

        }
        private Player getPlayer(int id)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                Player p = null;
                SqlCommand cmd = new SqlCommand("SELECT * FROM TblPlayers WHERE PlayerId = " + id);
                try
                {
                    con.Open();
                    cmd.Connection = con;
                    var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        p = new Player();
                        p.Id = Int32.Parse(reader["PlayerId"].ToString());
                        p.UserName = reader["PlayerName"].ToString();
                        p.Email = reader["PlayerMail"].ToString();
                    }
                    con.Close();
                }
                catch (Exception ex)
                {
                    con.Close();
                    return null;
                }
                return p;
            }
        }
        public Game GetGameById(int id)
        {
            List<Player> pl = GetAllPlayersOfGame(id);
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                Game g = null;
                SqlCommand cmd = new SqlCommand("SELECT * FROM TblGames WHERE GameId = " + id);
                try
                {
                    con.Open();
                    cmd.Connection = con;
                    var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        g = new Game();
                        g.Id = Int32.Parse(reader["GameId"].ToString());
                        g.StartTime = reader["StartTime"].ToString();
                        g.Player1 = pl[0];
                        g.Player2 = pl[1];
                                            
                    }
                    con.Close();
                }
                catch (Exception ex)
                {
                    con.Close();
                    return null;// ex.ToString();
                }
                return g;
            }
        }
        //end of Soap



        /*
         * Duplex
         */
        private static Dictionary<Player, CallBack> RunningDictionary = new Dictionary<Player, CallBack>();
        private static Dictionary<Team, CallBack> RunningTeamDictionary = new Dictionary<Team, CallBack>();




        public void DoWork()
        {
        }

        void IService2.LogIn(string name, string email)
        {

            CallBack callBack = OperationContext.Current.GetCallbackChannel<CallBack>();

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string q = "SELECT * FROM TblPlayers WHERE PlayerName = '" + name + "'" + "AND PlayerMail = '" + email + "'";
                SqlCommand cmd = new SqlCommand(q);//"SELECT * FROM TblPlayers");
                try
                {
                    string p = "dibug";
                    con.Open();
                    cmd.Connection = con;
                    var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        p = reader["PlayerId"].ToString() + "@"
                            + reader["PlayerName"].ToString() + "@"
                            + reader["PlayerMail"].ToString();
                    }
                    callBack.sendPlayer(p);
                    con.Close();

                }
                catch (Exception ex)
                {
                    callBack.CallBackFunc("wrong username or password");
                }
            }
        }
        public void LogInTeam(string name)
        {
            int id = -1;
            CallBack callBack = OperationContext.Current.GetCallbackChannel<CallBack>();

            using (SqlConnection con = new SqlConnection(connectionString))
            {

                string q = "SELECT TeamId FROM TblTeams WHERE TeamName = '" + name + "'";
                SqlCommand cmd = new SqlCommand(q);//"SELECT * FROM TblPlayers");
                try
                {
                    con.Open();
                    cmd.Connection = con;
                    var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        id = Int32.Parse(reader["TeamId"].ToString());
                    }
                    con.Close();

                    List<int> pIdList = new List<int>();
                    q = "SELECT PlayerId FROM TblTeamPlayers WHERE TeamId = " + id;
                    cmd = new SqlCommand(q);
                    con.Open();
                    cmd.Connection = con;
                    reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        pIdList.Add(Int32.Parse(reader["PlayerId"].ToString()));
                    }
                    List<Player> pList = new List<Player>();
                    foreach (int i in pIdList) {
                        pList.Add(getPlayer(i));
                    }

                    Team team = new Team
                    {
                        Id = id,
                        PlayersInTeam = pList,
                        TeamName = name 
                    };
                    callBack.sendTeam(team);
                }
                catch (Exception ex)
                {
                    callBack.CallBackFunc(ex.ToString());
                }
            }
        }


        void IService2.SerchingForOponent(string playerString)
        {
            CallBack cb = OperationContext.Current.GetCallbackChannel<CallBack>();
            
            cb.CallBackFunc("starting the serch");
            Player player = strToPlayer(playerString);
            try
            {                
                RunningDictionary.Add(player, OperationContext.Current.GetCallbackChannel<CallBack>());
            }
            catch
            {
                RunningDictionary.Remove(player);
               /// cb.CallBackFunc("starting failed please try again");
                RunningDictionary.Add(player, OperationContext.Current.GetCallbackChannel<CallBack>());
            }
            try
            {
                if ((RunningDictionary.Count > 0) && (RunningDictionary.Count % 2 == 0))
                {

                    cb.CallBackFunc("oponent found");
                    Game game = new Game()
                    {
                        Player1 = RunningDictionary.Keys.ElementAt(RunningDictionary.Count - 2),
                        Player2 = player,
                        StartTime = DateTime.Now.ToString()
                    };
                    game.Player1.Color = "Red";
                    game.Player2.Color = "Black";


                    int gameID = whriteGameToDB(game);
                    string gameString = creatGameString(game);

                    RunningDictionary[game.Player1].sendGame(gameString);
                    RunningDictionary[game.Player2].sendGame(gameString);
                }
            }
            catch { }
        }

        public void SerchingForOponentTeam(Team team) {
            CallBack cb = OperationContext.Current.GetCallbackChannel<CallBack>();

            cb.CallBackFunc("starting the serch");
            try
            {
                RunningTeamDictionary.Add(team, OperationContext.Current.GetCallbackChannel<CallBack>());
            }
            catch
            {
                RunningTeamDictionary.Remove(team);
                /// cb.CallBackFunc("starting failed please try again");
                RunningTeamDictionary.Add(team, OperationContext.Current.GetCallbackChannel<CallBack>());
            }
            try
            {
                if ((RunningTeamDictionary.Count > 0) && (RunningTeamDictionary.Count % 2 == 0))
                {

                    cb.CallBackFunc("oponent found");
                    Game game = new Game()
                    {
                        Team1 = RunningTeamDictionary.Keys.ElementAt(RunningTeamDictionary.Count - 2),
                        Team2 = team,
                        StartTime = DateTime.Now.ToString()
                    };
                    List<Player> pl = new List<Player>();
                    foreach (Player p in game.Team1.PlayersInTeam) {
                        p.Color = "Red";
                        pl.Add(p);
                    }
                    foreach (Player p in game.Team2.PlayersInTeam)
                    {
                        p.Color = "Black";
                        pl.Add(p);
                    }

                    int gameID = whriteGameToDB(game);
                    game.Id = gameID;
                    foreach (Player p in pl) {
                        TblPlayersGamesIn(game,p);
                    }
                    

                    RunningTeamDictionary[game.Team1].sendTeamGame(game);
                    RunningTeamDictionary[game.Team2].sendTeamGame(game);
                }
            }
            catch(Exception ex) {
                cb.CallBackFunc(ex.ToString());
            }
        }

        public void sendMove(string sendMove)
        {
            // throw new NotImplementedException();
            string[] sArr = sendMove.Split('&');
            Player p = strToPlayer(sArr[0]);
            RunningDictionary[p].ReseveMove(sArr[1], sArr[2]);
        }
        public void sendMoveTeam(Team team, Move move,string color) {
            RunningTeamDictionary[team].ReseveMoveTeam(move, color);
        }

        private int whriteGameToDB(Game game)
        {
            int gameID = -1;
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    //con = DataBase.Connection;
                    con.Open();
                    cmd = new SqlCommand("insert into TblGames(StartTime) values(@StartTime)", con);
                    cmd.Parameters.AddWithValue("@StartTime", game.StartTime);
                    cmd.ExecuteNonQuery();
                    con.Close();
                    con.Open();
                    cmd = new SqlCommand("SELECT GameId FROM TblGames WHERE StartTime = '" + game.StartTime + "'", con);
                    var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        gameID = Int32.Parse(reader["GameId"].ToString());
                        game.Id = gameID;
                    }
                    con.Close();
                    TblPlayersGamesIn(game, game.Player1);
                    TblPlayersGamesIn(game, game.Player2);


                    return gameID;
                }
            }
            catch (Exception e)
            {
                return -1;
            }

        }


        void TblPlayersGamesIn(Game game, Player player)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    cmd = new SqlCommand("insert into TblPlayersGames(IdPlayer,IdGame,PlayerName) values(@IdPlayer,@IdGame,@PlayerName)", con);
                    cmd.Parameters.AddWithValue("@IdPlayer", player.id);
                    cmd.Parameters.AddWithValue("@IdGame", game.Id);
                    cmd.Parameters.AddWithValue("@PlayerName", player.UserName);
                    cmd.ExecuteNonQuery();
                    con.Close();
                    con.Open();
                }
            }
            catch { }
        }

        private string creatPlayerString(Player player1)
        {
            string playerString = player1.id.ToString() + "@"
                                + player1.UserName + "@"
                                + player1.Email + "@"
                                + player1.Color;
            return playerString;
        }


        private string creatGameString(Game game)
        {
            string gameString;

            string player1String = creatPlayerString(game.Player1);
            string player2String = creatPlayerString(game.Player2);

            gameString = game.Id.ToString() + "$" + player1String + "$" + player2String;

            return gameString;
        }

        private Player strToPlayer(string playerString)
        {
            string[] sArr = playerString.Split('*');
            try
            {
                Player p = new Player()
                {
                    UserName = sArr[1],
                    Email = sArr[2]
                };
                p.Id = Int32.Parse(sArr[0]);
                return p;

            }
            catch
            { return null; }
        }

        public void endGameServer(Game g, List<Move> MoveList)
        {
            //game,winner,moves
            // string[] stringArr = endGameString.Split('+');
            //Game g = stringToGame(stringArr[0]);
            int winnerID = g.WinnerId;//Int32.Parse(stringArr[1]);
            string Pname;
            if (g.Player1.id == winnerID)
                Pname = g.Player1.UserName;
            else
                Pname = g.Player2.UserName;

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                //con = DataBase.Connection;
                con.Open();

                cmd = new SqlCommand("UPDATE TblGames SET WinnerId = @WinnerId, WinnerName = @WinnerName WHERE GameId = " + g.Id, con);
                cmd.Parameters.AddWithValue("@WinnerId", winnerID);
                cmd.Parameters.AddWithValue("@WinnerName", Pname);
                cmd.ExecuteNonQuery();
                con.Close();
            }

            //moveString#playerOfMoveID#GameID
            /*List<Move> MoveList = new List<Move>();
            string[] moveliststring = stringArr[2].Split('%');
            for (int i = 0; i < moveliststring.Length - 1; i++)
            {
                string[] sa = moveliststring[i].Split('#');

                MoveList.Add(new Move()
                {
                    GameOfMove = new Game() { Id = Int32.Parse(sa[2]) },
                    PlayerOfMove = new Player() { id = Int32.Parse(sa[1]) },
                    MoveString = sa[0]
                });
            }*/
            insertMovesToDB(MoveList);
        }

        public void endTeamGameServer(Game game, Team winner, List<Move> AllMoves) {

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                //con = DataBase.Connection;
                con.Open();
                cmd = new SqlCommand("UPDATE TblGames SET WinnerTeamId = @WinnerId, WinnerTeamName = @WinnerName WHERE GameId = " + game.Id, con);
                cmd.Parameters.AddWithValue("@WinnerId", winner.Id);
                cmd.Parameters.AddWithValue("@WinnerName", winner.TeamName);
                cmd.ExecuteNonQuery();
                con.Close();
            }
            insertMovesToDB(AllMoves);
        }

        public void endGameServer2(Game g) { }

        private void insertMovesToDB(List<Move> MoveList)
        {

            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    //con = DataBase.Connection;
                    con.Open();
                    foreach (Move m in MoveList)
                      {                          
                          cmd = new SqlCommand("insert into TblMoves(Move,PlayerId,GameId) values(@Move,@PlayerId,@GameId)", con);
                          cmd.Parameters.AddWithValue("@Move", m.MoveString);
                          cmd.Parameters.AddWithValue("@PlayerId", m.PlayerOfMove.id);
                          cmd.Parameters.AddWithValue("@GameId", m.GameOfMove.Id);
                          cmd.ExecuteNonQuery();                          
                      }
                    con.Close();
                    /*con.Open();
                    for (int i = 0; i < MoveList.Count;i++) {
                        cmd = new SqlCommand("insert into TblMoves(Move,PlayerId,GameId) values(@Move,@PlayerId,@GameId)", con);
                        cmd.Parameters.AddWithValue("@Move", MoveList[i].MoveString);
                        cmd.Parameters.AddWithValue("@PlayerId", MoveList[i].PlayerOfMove.id);
                        cmd.Parameters.AddWithValue("@GameId", MoveList[i].GameOfMove.Id);
                        cmd.ExecuteNonQuery();
                    }
                    con.Close();*/

                    List<int> MoveIdArr = new List<int>();
                    List<int> PlayerIdArr = new List<int>();

                    con.Open();
                    cmd = new SqlCommand("SELECT MoveId,PlayerId FROM TblMoves WHERE GameId = " + MoveList[0].GameOfMove.Id, con);
                    var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        MoveIdArr.Add(Int32.Parse(reader["MoveId"].ToString()));
                        PlayerIdArr.Add(Int32.Parse(reader["PlayerId"].ToString()));
                    }
                    con.Close();
                    con.Open();
                    for (int i = 0; i < MoveIdArr.Count; i++)
                    {
                        SqlCommand cmdd = new SqlCommand("insert into TblPlayersMoves(IdMove,IdPlayer) values(@IdMove,@IdPlayer)", con);
                        cmdd.Parameters.AddWithValue("@IdMove", MoveIdArr[i]);
                        cmdd.Parameters.AddWithValue("@IdPlayer", PlayerIdArr[i]);
                        cmdd.ExecuteNonQuery();
                    }
                    con.Close();

                }
            }
            catch (Exception e)
            {

                string s = e.ToString();

                return;

            }


        }


        private Game stringToGame(string gameString)
        {
            //id$p1s$p2s
            string[] sArr = gameString.Split('$');
            Game g = new Game()
            {
                Id = Int32.Parse(sArr[0]),
                Player1 = strToPlayer(sArr[1]),
                Player2 = strToPlayer(sArr[2])
            };
            return g;

        }
        //end of Duplex


        // REST

        
       

        public string RegisterPlayerWithTeam(string name, string email, string teamName)
        {
            // throw new NotImplementedException();
            RegisterPlayer(name, email);
            int teamID = GetTeamID(teamName);
            int playerID = GetPlayerID(name);
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    //con = DataBase.Connection;
                    con.Open();
                    cmd = new SqlCommand("insert into TblTeamPlayers(TeamId,PlayerId) values(@TeamId,@PlayerId)", con);
                    cmd.Parameters.AddWithValue("@TeamId", teamID);
                    cmd.Parameters.AddWithValue("@PlayerId", playerID);
                    cmd.ExecuteNonQuery();
                    con.Close();
                    return "Ok";
                }
            }
            catch (Exception e)
            {
                return e.ToString();
            }


        }
        private int GetTeamID(string teamName) {
                int teamId = -1;
                try
                {
                    using (SqlConnection con = new SqlConnection(connectionString))
                    {
                        //con = DataBase.Connection;                        
                        con.Open();
                        cmd = new SqlCommand("SELECT TeamId FROM TblTeams WHERE TeamName = '" + teamName + "'", con);
                        var reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            teamId = Int32.Parse(reader["TeamId"].ToString());
                        }
                        con.Close();
                    }
                }
                catch (Exception e)
                {
                    e.ToString();
                }
                return teamId;

            }
        private int GetPlayerID(string playerName)
        {
            int playerId = -1;
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    //con = DataBase.Connection;                        
                    con.Open();
                    cmd = new SqlCommand("SELECT PlayerId FROM TblPlayers WHERE PlayerName = '" + playerName + "'", con);
                    var reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        playerId = Int32.Parse(reader["PlayerId"].ToString());
                    }
                    con.Close();
                }
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return playerId;

        }
        public string registerTeam(string teamName) {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    //con = DataBase.Connection;
                    con.Open();
                    cmd = new SqlCommand("insert into TblTeams(TeamName) values(@TeamName)", con);
                    cmd.Parameters.AddWithValue("@TeamName", teamName);
                    cmd.ExecuteNonQuery();
                    con.Close();
                    return "Ok";
                }
            }
            catch (Exception e)
            {
               return e.ToString();
            }

        }      

        public string getAllPlayers2()
        {
            List<Player> pl = getAllPlayers();
            string s = "";
            foreach (Player p in pl) {
                s += creatPlayerString(p) + "*";
            }
            return s.Substring(0, s.Length - 1);
        }       

        public string getAllGames2()
        {
            List<Game> gl = getAllGames();
            string s = "";
            foreach (Game g in gl) {
                s += creatGameString(g) +"*";
            }
            return s.Substring(0, s.Length - 1);
        }

        public string getAllGamesForCombo2()
        {
            List<Game> gl = getAllGames();
            string s = "";
            foreach (Game g in gl)
            {
                s += g.Id.ToString() + " " + g.StartTime + "*";
            }
            return s.Substring(0, s.Length - 1);
        }

        public string getAllPlayersNames2()
        {
            List<string> sl = getAllPlayersNames();
            string s = "";
            foreach (string str in sl) {
                s += str + "*";
            }
            return s.Substring(0, s.Length - 1);
        }

        public string GetAllGameOfPlayer2(string name)
        {
            List<Game> gl = GetAllGameOfPlayer(name);
            string s = "";
            foreach (Game g in gl) {
                s += creatGameString(g) + "*";
            }
            return s.Substring(0, s.Length - 1);
        }

        public string GetAllPlayersOfGame2(string gameId)
        {
            try
            {
                int id = Int32.Parse(gameId);
                List<Player> pl = GetAllPlayersOfGame(id);
                string s = "";
                foreach (Player p in pl)
                {
                    s += creatPlayerString(p) + "*";
                }
                return s.Substring(0,s.Length-1);
            }
            catch (Exception ex) {
                return ex.ToString();
            }
        }

        public string GetAllMovesOfGame2(string gameId)
        {
            int id = Int32.Parse(gameId);
            List<Move> ml = GetAllMovesOfGame(id);
            string s = "";
            foreach (Move m in ml) {
                s += creatMoveString(m) + "*";
            }
            return s.Substring(0, s.Length - 1);
        }

        public string GetGameById2(string id)
        {
            int gameId = Int32.Parse(id);
            Game g = GetGameById(gameId);
            return creatGameString(g);
        }


        private string creatMoveString(Move m)
        {
            string s = "";

            s += creatGameString(m.GameOfMove) + "{}";
            s += creatPlayerString(m.PlayerOfMove) + "{}";
            s += m.MoveId.ToString() + "{}";
            s += m.MoveString;

            return s;
        }

        public string DeletePlayer(string name) {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    //con = DataBase.Connection;
                    con.Open();
                    cmd = new SqlCommand("DELETE FROM TblPlayers WHERE PlayerName = '" + name + "'", con);
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
            catch (Exception e)
            {
                return e.ToString();
            }

            return getAllPlayersNames2();
        }

        public string DeleteGame(string gameid)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    //con = DataBase.Connection;
                    int id = Int32.Parse(gameid);
                    con.Open();
                    cmd = new SqlCommand("DELETE FROM TblGames WHERE GameId = " + id, con);
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
            catch (Exception e)
            {
                return e.ToString();
            }

            return getAllGamesForCombo2();
        }


        public string CountPlayerGames() {
           
            string s = "";
            List<Player> pl = getAllPlayers();
            foreach (Player p in pl) {               
                s += p.id.ToString() + " " + GetAllGameOfPlayer(p.UserName).Count.ToString() +" "+ p.UserName + "^"; 
            }

            /*
            using (SqlConnection con = new SqlConnection(connectionString))
            {                
                SqlCommand cmd = new SqlCommand("SELECT COUNT(IdGame),IdPlayer  FROM TblPlayersGames GROUP BY IdPlayer");//"SELECT * FROM TblPlayers");
                try
                {
                    con.Open();
                    cmd.Connection = con;
                    var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        s += reader["IdPlayer"].ToString();//+ " " + reader["COUNT(IdGame)"].ToString() + "^";
                    }
                    con.Close();
                }
                catch (Exception ex)
                {
                    con.Close();
                    return ex.ToString();
                }                
            }*/
            return s.Substring(0, s.Length - 1);
        }

        public string UpdatePlayer(string data) {
            string name = data.Split(' ')[0];
            string email = data.Split(' ')[1];
            string pName = data.Split(' ')[2];

            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    //con = DataBase.Connection;
                    con.Open();
                    cmd = new SqlCommand("UPDATE TblPlayers SET PlayerName = '" + name + "'," + "PlayerMail = '" + email + "' WHERE PlayerName = '" + pName + "'", con);
                    cmd.ExecuteNonQuery();
                    con.Close();
                    con.Open();
                    cmd = new SqlCommand("UPDATE TblPlayersGames SET PlayerName = '" + name + "' WHERE PlayerName = '" + pName + "'", con);
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
            catch (Exception e)
            {
                return e.ToString();
            }

            return getAllPlayersNames2();


        }


        /////////end of class
    }
}
