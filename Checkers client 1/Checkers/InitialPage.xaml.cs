using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Checkers.ServiceReference1;

using System.ServiceModel;

namespace Checkers
{
    /// <summary>
    /// Interaction logic for InitialPage.xaml
    /// </summary>
    public partial class InitialPage : Window, IService2Callback, IDisposable
    {

        private Service1Client client;
        private Service2Client proxy;


        private bool LoggedIn = false;
        private bool isTeamGame = false;

        private MainWindow window;
        private Game game;
        private Player CurrentPlayer;
        private Team CurrentTeam;


        public InitialPage()
        {
            InitializeComponent();
        }

        private void PvP_Click(object sender, RoutedEventArgs e)
        {
            if (LoggedIn)
            {
                if (isTeamGame)
                {
                    textBlock.Text = "looking for players...";
                    InstanceContext context = new InstanceContext(this);
                    proxy = new Service2Client(context);
                    proxy.SerchingForOponentTeam(CurrentTeam);
                }
                else
                {
                    string playerString = creatPlayerString(CurrentPlayer);
                    textBlock.Text = "looking for players...";
                    InstanceContext context = new InstanceContext(this);
                    proxy = new Service2Client(context);
                    proxy.SerchingForOponent(playerString);
                }
            }
        }

        private string creatPlayerString(Player player1)
        {
            string playerString = player1.id.ToString() + "*"
                                + player1.UserName + "*"
                                + player1.Email;
            return playerString;
        }

        private void PvE_Click(object sender, RoutedEventArgs e)
        {
            //for dibag
           // LoggedIn = true;
            //CurrentPlayer = new Player() { id = 213, UserName = "abc" };
            /////////

            if (LoggedIn)
            {
                CurrentPlayer.Color = "Red";
                game = new Game()
                {
                    Player1 = CurrentPlayer
                };
                window = new MainWindow(game, true, CurrentPlayer,null);
                window.Show();
            }
        }

        private void Abaut_Click(object sender, RoutedEventArgs e)
        {
            Abaut abaut = new Abaut();
            abaut.ShowDialog();

        }

        private void Restor_Click(object sender, RoutedEventArgs e)
        {
            GameToRestore win = new GameToRestore();
            win.Show();
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            logIn LW = new logIn();
            LW.Owner = this;
            LW.ShowDialog();
            string name = LW.name;
            string email = LW.email;
            InstanceContext context = new InstanceContext(this);
            proxy = new Service2Client(context);
            proxy.LogIn(name, email);
            //textBlock.Text = name + " " + email;
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public void CallBackFunc(string str)
        {
            //throw new NotImplementedException();
            textBlock.Text = str;
        }

        public void sendPlayer(string playerString)
        {
            //throw new NotImplementedException();            
            try
            {
                CurrentPlayer = playerStringToPlayer(playerString);
                LoggedIn = true;
                textBlock.Text = "User logged in: " + CurrentPlayer.UserName;
                TeamLogin.IsEnabled = false;
                TeamLogout.IsEnabled = false;
                Login.IsEnabled = false;
            }
            catch
            {
                LoggedIn = false;
                textBlock.Text = "wrong username or password";
            }
        }

        private Player playerStringToPlayer(string playerString)
        {
            Player p;
            string[] sArr = playerString.Split('@');
            try
            {
                p = new Player()
                {
                    id = Int32.Parse(sArr[0]),
                    UserName = sArr[1],
                    Email = sArr[2],
                    Color = sArr[3]
                };
                return p;
            }
            catch { return null; }
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            CurrentPlayer = null;
            LoggedIn = false;
            textBlock.Text = "";
            TeamLogin.IsEnabled = true;
            TeamLogout.IsEnabled = true;
            Login.IsEnabled = true;
        }

        public void sendGame(string gameString)
        {//1v1 pvp
            //throw new NotImplementedException();
            game = gameStringToGame(gameString);
            if (game.Player1.id == CurrentPlayer.id)
                CurrentPlayer.Color = game.Player1.Color;
            else
                CurrentPlayer.Color = game.Player2.Color;

            window = new MainWindow(game, false, CurrentPlayer,null);
            window.Show();
        }

        public void sendTeamGame(Game game)
        {//Team v Team pvp
            this.game = game;
            if (game.Team1.id == CurrentTeam.id)
            {
                foreach (Player p in CurrentTeam.PlayersInTeam)
                {
                    p.Color = game.Team1.PlayersInTeam[0].Color;
                }
                CurrentPlayer = game.Team1.PlayersInTeam[0];
            }
            else {
                foreach (Player p in CurrentTeam.PlayersInTeam)
                {
                    p.Color = game.Team2.PlayersInTeam[0].Color;
                }
                CurrentPlayer = game.Team2.PlayersInTeam[0];
            }
            window = new MainWindow(game, false, CurrentPlayer, CurrentTeam);
            window.Show();
        }

        private Game gameStringToGame(string gameString)
        {
            string[] sArr = gameString.Split('$');
            int gameID = Int32.Parse(sArr[0]);
            Player player1 = playerStringToPlayer(sArr[1]);
            Player player2 = playerStringToPlayer(sArr[2]);
            Game g = new Game()
            {
                id = gameID,
                Player1 = player1,
                Player2 = player2
            };
            return g;
        }

        public void ReseveMove(string moveString, string color)
        {
            //throw new NotImplementedException();

            window.ReseveMove(moveString, color);
        }

        private void TeamLogin_Click(object sender, RoutedEventArgs e)
        {
            TeamLogIn LW = new TeamLogIn();
            LW.Owner = this;
            LW.ShowDialog();
            string name = LW.TeamName;
            InstanceContext context = new InstanceContext(this);
            proxy = new Service2Client(context);
            proxy.LogInTeam(name);
        }

        public void sendTeam(Team t)
        {
            // throw new NotImplementedException();
            //throw new NotImplementedException();            
            try
            {
                CurrentTeam = t;
                LoggedIn = true;
                isTeamGame = true;
                textBlock.Text = "User logged in: " + CurrentTeam.TeamName;

                TeamLogin.IsEnabled = false;
                Logout.IsEnabled = false;
                Login.IsEnabled = false;
                PvE.IsEnabled = false;
                PvP.Content = "Team vs Team";

            }
            catch
            {
                LoggedIn = false;
                textBlock.Text = "wrong username or password";
            }
        }

        public void ReseveMoveTeam(ServiceReference1.Move m, string color)
        {
            // throw new NotImplementedException();
            window.ReseveMoveTeam(m, color);
        }

        private void TeamLogout_Click(object sender, RoutedEventArgs e)
        {
            CurrentTeam = null;
            LoggedIn = false;
            isTeamGame = false;
            textBlock.Text = "";

            TeamLogin.IsEnabled = true;
            Logout.IsEnabled = true;
            Login.IsEnabled = true;
            PvE.IsEnabled = true;
            PvP.Content = "Player vs Player";

        }
    }
}
