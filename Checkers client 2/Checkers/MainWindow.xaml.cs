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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Checkers.ServiceReference1;

using System.ServiceModel;
using System.Windows.Media.Animation;
using System.Collections;
using System.Threading;

namespace Checkers
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IService2Callback
    {

        /* animnations
         http://stackoverflow.com/questions/1013817/wpf-fade-animation
             */

        private Service1Client client;
        private Service2Client proxy;

        private Game game;
        private Player CurrentPlayre;
        private Player MainPlayer;

        private Image SelectedPiece = null;

        private bool PvE;

        private int OriginCol;
        private int OriginRow;
        private int TargetCol;
        private int TargetRow;

        private List<Move> AllMoves = new List<Move>();

        Brush BlackBackground;
        Brush WhiteBackground;

        private bool isTeamGame;
        private Team CurrentTeam;
        private int team1Index = 0;
        private int team2Index = 0;
        private Team MainTeam;

        private string RedSorce;
        private string BlackSorce;

        private MovmentValidation validator;

        public MainWindow(Game game, bool PvE, Player MainPlayer,Team MainTeam)
        {
            InitializeComponent();
            
            this.PvE = PvE;
            if (MainPlayer != null)
            {
                this.MainPlayer = MainPlayer;
                UserNameLable.Content = "User: " + MainPlayer.id + " " + MainPlayer.UserName;
            }
            if (MainTeam != null)
            {
                this.MainTeam = MainTeam;
                UserNameLable.Content = "User: " + MainTeam.id + " " + MainTeam.TeamName;
            }


            //CreateGameForDibbagind();          
            this.game = game;
            client = new Service1Client();

            if (PvE)
            {
                this.game.Player2 = new Player() { id = 99999999, UserName = "Computer", Color = "Black" };
            }
            
            BlackBackground = FirstStackPanel.Background;
            WhiteBackground = p2.Background;

            foreach (UIElement e in LastStackPanel.Children)
            {
                if (e.GetType() == typeof(Image))
                {
                    Image i = (Image)e;
                    RedSorce = i.Source.ToString();
                }
            }
            foreach (UIElement e in FirstStackPanel.Children)
            {
                if (e.GetType() == typeof(Image))
                {
                    Image i = (Image)e;
                    BlackSorce = i.Source.ToString();
                }
            }
            setOnClick();

            validator = new MovmentValidation(RedSorce, BlackSorce, game);

            if (validator.isTeamGame()) {
                isTeamGame = true;
                CurrentTeam = game.Team1;
                if(team1Index < CurrentTeam.PlayersInTeam.Count())
                 CurrentPlayre = CurrentTeam.PlayersInTeam[team1Index++];
                else
                {
                    team1Index = 0;
                    CurrentPlayre = CurrentTeam.PlayersInTeam[team1Index++];
                }
            }
            else
                CurrentPlayre = game.Player1;

        }
        /*
        public void CreateGameForDibbagind()
        {
            game = new Game()
            {
                Player2 = new Player() { id = 2, UserName = "Or", Color = "Black" },
                Player1 = new Player() { id = 3, UserName = "Maoz", Color = "Red" },
                id = 1,
                Team1 = null,
                Team2 = null,
                StartTime = DateTime.Now.ToShortTimeString()
            };
            CurrentPlayre = game.Player1;
        }*/

        public void setOnClick()
        {
            foreach (UIElement e in CheckersGrid.Children)
            {
                StackPanel p = (StackPanel)e;

                p.MouseDown += planel_click;
            }
        }

        public void planel_click(object sender, MouseButtonEventArgs e)
        {
            CheckGameEnd();
            List<StackPanel> MoveList = null;
            if (CurrentPlayre.id != MainPlayer.id)
            {
                return;
            }

            if (SelectedPiece == null)
            {//no image selected
                if (e.Source.GetType() == typeof(Image))
                {//select the image if it is leaagl
                    Image tempImage = (Image)e.Source;
                    if (validator.ValidateImagePlayer(tempImage, CurrentPlayre))
                    {
                        SelectedPiece = tempImage;
                        OriginCol = Convert.ToInt32(Math.Floor(e.MouseDevice.GetPosition(CheckersGrid).X / 100));
                        OriginRow = Convert.ToInt32(Math.Floor(e.MouseDevice.GetPosition(CheckersGrid).Y / 100));
                        statusLabel.Content = "Piece selected";
                        MoveList = getAllLegelMoves();
                    }

                }
            }
            else
            {
                checkIfHasePiecess();
                MoveList = getAllLegelMoves();
                if (MoveList.Count == 0)
                {//if there are no moves to make the player losses
                    if (isTeamGame) {
                        if (CurrentTeam.id == game.Team1.id)
                            endGame(game.Team2);
                        else
                            endGame(game.Team1);
                    }
                    else {
                        if (CurrentPlayre.id == game.Player1.id)
                            endGame(game.Player2);
                        else
                            endGame(game.Player1);
                    }

                
                        
                }
                if (e.Source.GetType() == typeof(Image))
                {//second select was an image
                    if (SelectedPiece == (Image)e.Source)
                    {//selected the same image for a second time. relese the select
                        SelectedPiece = null;
                        RestorBackground();
                        statusLabel.Content = "";
                    }
                }
                else if (e.Source.GetType() == typeof(StackPanel))
                {
                    StackPanel tempPalen = (StackPanel)e.Source;
                    if (MoveList.Contains(tempPalen))
                    {
                        TargetCol = Convert.ToInt32(Math.Floor(e.MouseDevice.GetPosition(CheckersGrid).X / 100));
                        TargetRow = Convert.ToInt32(Math.Floor(e.MouseDevice.GetPosition(CheckersGrid).Y / 100));
                        makeTheMove(SelectedPiece, tempPalen);
                        EndTurn();
                    }
                }
            }
        }

        private List<StackPanel> getAllLegelMoves()
        {
            List<StackPanel> MoveList = new List<StackPanel>();
            StackPanel panel = (StackPanel)SelectedPiece.Parent;
            MarkPanel(panel, Colors.Yellow);//set color of selected panel
            StackPanel tempP;

            //  int TargetCol = Convert.ToInt32(Math.Floor(e.MouseDevice.GetPosition(CheckersGrid).X / 100));
            //   int TargetRow = Convert.ToInt32(Math.Floor(e.MouseDevice.GetPosition(CheckersGrid).Y / 100));


            //for Red player
            if (CurrentPlayre.Color.Equals("Red"))
            {
                if ((0 < OriginCol) && (OriginCol < 3))
                {
                    tempP = (StackPanel)CheckersGrid.Children[((OriginCol - 1) + (OriginRow - 1) * 4)];
                    if (tempP.Children.Count == 0)
                    {
                        MoveList.Add(tempP);
                    }
                    else if (isEnemyUnit(tempP))
                    {
                        if (((OriginCol + 1) < 3) && ((OriginRow - 2) >= 0))//*(0 < OriginCol - 1) || ((OriginCol + 1) < 3))
                        {
                            tempP = (StackPanel)CheckersGrid.Children[((OriginCol + 2) + (OriginRow - 2) * 4)];
                            if (tempP.Children.Count == 0)
                                MoveList.Add(tempP);
                        }
                        if ((0 < OriginCol - 1) && ((OriginRow - 2) >= 0))// || ((OriginCol + 1) < 3))
                        {
                            tempP = (StackPanel)CheckersGrid.Children[((OriginCol - 2) + (OriginRow - 2) * 4)];
                            if (tempP.Children.Count == 0)
                                MoveList.Add(tempP);
                        }
                    }
                    tempP = (StackPanel)CheckersGrid.Children[((OriginCol + 1) + (OriginRow - 1) * 4)];
                    if (tempP.Children.Count == 0)
                        MoveList.Add(tempP);
                    else if (isEnemyUnit(tempP))
                    {
                        if (((OriginCol + 1) < 3) && ((OriginRow - 2) >= 0) )//*(0 < OriginCol - 1) || ((OriginCol + 1) < 3))
                        {
                            tempP = (StackPanel)CheckersGrid.Children[((OriginCol + 2) + (OriginRow - 2) * 4)];
                            if (tempP.Children.Count == 0)
                                MoveList.Add(tempP);
                        }
                        if ((0 < OriginCol - 1) && ((OriginRow - 2) >= 0))// || ((OriginCol + 1) < 3))
                        {
                            tempP = (StackPanel)CheckersGrid.Children[((OriginCol - 2) + (OriginRow - 2) * 4)];
                            if (tempP.Children.Count == 0)
                                MoveList.Add(tempP);
                        }
                    }
                }
                else if (OriginCol == 0)
                {
                    tempP = (StackPanel)CheckersGrid.Children[((OriginCol + 1) + (OriginRow - 1) * 4)];
                    if (tempP.Children.Count == 0)
                        MoveList.Add(tempP);
                    else if (isEnemyUnit(tempP))
                    {
                        tempP = (StackPanel)CheckersGrid.Children[((OriginCol + 2) + (OriginRow - 2) * 4)];
                        if (tempP.Children.Count == 0)
                            MoveList.Add(tempP);
                    }
                }
                else if (OriginCol == 3)
                {
                    tempP = (StackPanel)CheckersGrid.Children[((OriginCol - 1) + (OriginRow - 1) * 4)];
                    if (tempP.Children.Count == 0)
                        MoveList.Add(tempP);
                    else if (isEnemyUnit(tempP))
                    {
                        tempP = (StackPanel)CheckersGrid.Children[((OriginCol - 2) + (OriginRow - 2) * 4)];
                        if (tempP.Children.Count == 0)
                            MoveList.Add(tempP);
                    }
                }
            }
            //for Black player
            if (CurrentPlayre.Color.Equals("Black"))
            {
                if ((0 < OriginCol) && (OriginCol < 3))
                {
                    tempP = (StackPanel)CheckersGrid.Children[((OriginCol - 1) + (OriginRow + 1) * 4)];
                    if (tempP.Children.Count == 0)
                        MoveList.Add(tempP);
                    else if (isEnemyUnit(tempP))
                    {
                        if (((OriginCol + 1) < 3))//((OriginCol - 1) > 0) || ((OriginCol + 1) < 3))
                        {
                            tempP = (StackPanel)CheckersGrid.Children[((OriginCol + 2) + (OriginRow + 2) * 4)];
                            if (tempP.Children.Count == 0)
                                MoveList.Add(tempP);
                        }
                        if (((OriginCol - 1) > 0))// || ((OriginCol + 1) < 3))
                        {
                            tempP = (StackPanel)CheckersGrid.Children[((OriginCol - 2) + (OriginRow + 2) * 4)];
                            if (tempP.Children.Count == 0)
                                MoveList.Add(tempP);
                        }
                    }
                    tempP = (StackPanel)CheckersGrid.Children[((OriginCol + 1) + (OriginRow + 1) * 4)];
                    if (tempP.Children.Count == 0)
                        MoveList.Add(tempP);
                    else if (isEnemyUnit(tempP))
                    {
                        if (((OriginCol + 1) < 3))//((OriginCol - 1) > 0) || ((OriginCol + 1) < 3))
                        {
                            tempP = (StackPanel)CheckersGrid.Children[((OriginCol + 2) + (OriginRow + 2) * 4)];
                            if (tempP.Children.Count == 0)
                                MoveList.Add(tempP);
                        }
                        if (((OriginCol - 1) > 0))// || ((OriginCol + 1) < 3))
                        {
                            tempP = (StackPanel)CheckersGrid.Children[((OriginCol - 2) + (OriginRow + 2) * 4)];
                            if (tempP.Children.Count == 0)
                                MoveList.Add(tempP);
                        }
                    }
                }
                else if (OriginCol == 0)
                {
                    tempP = (StackPanel)CheckersGrid.Children[((OriginCol + 1) + (OriginRow + 1) * 4)];
                    if (tempP.Children.Count == 0)
                        MoveList.Add(tempP);
                    else if (isEnemyUnit(tempP))
                    {
                        tempP = (StackPanel)CheckersGrid.Children[((OriginCol + 2) + (OriginRow + 2) * 4)];
                        if (tempP.Children.Count == 0)
                            MoveList.Add(tempP);
                    }
                }
                else if (OriginCol == 3)
                {
                    tempP = (StackPanel)CheckersGrid.Children[((OriginCol - 1) + (OriginRow + 1) * 4)];
                    if (tempP.Children.Count == 0)
                        MoveList.Add(tempP);
                    else if (isEnemyUnit(tempP))
                    {
                        tempP = (StackPanel)CheckersGrid.Children[((OriginCol - 2) + (OriginRow + 2) * 4)];
                        if (tempP.Children.Count == 0)
                            MoveList.Add(tempP);
                    }
                }
            }
            foreach (StackPanel p in MoveList)
                MarkPanel(p, Colors.Green);

            return MoveList;
        }

        private bool isEnemyUnit(StackPanel panel)
        {
            Image i = (Image)panel.Children[0];

            if (CurrentPlayre.Color.Equals("Red"))
                return (i.Source.ToString().Equals(BlackSorce));
            else
                return (i.Source.ToString().Equals(RedSorce));
        }

        private void MarkPanel(StackPanel panel, Color color)
        {
            panel.Background = new SolidColorBrush(color);
        }
       

        private void makeTheMove(Image PieceToMove, StackPanel Target)
        {
           
            StackPanel originalPanel = (StackPanel)CheckersGrid.Children[(OriginCol + OriginRow * 4)];
            Image i = (Image)originalPanel.Children[0];


            FadeOutAnimation(PieceToMove);
            originalPanel.Children.RemoveAt(0);
            
            SaveMove();
            //eating move Red
            if (CurrentPlayre.Color.Equals("Red"))
            {
                if (CheckersGrid.Children.IndexOf(originalPanel) - 5 > CheckersGrid.Children.IndexOf(Target))
                {
                    StackPanel p;
                    statusLabel.Content = "red eat";
                    int mod = CheckersGrid.Children.IndexOf(originalPanel) % 4;
                    switch (mod)
                    {
                        case 0:
                            p = (StackPanel)(CheckersGrid.Children[(CheckersGrid.Children.IndexOf(originalPanel) - 3)]);
                            FadeOutAnimation((Image)(p.Children[0]));
                            p.Children.RemoveAt(0);
                            break;
                        case 1:
                            p = (StackPanel)(CheckersGrid.Children[(CheckersGrid.Children.IndexOf(originalPanel) - 3)]);
                            FadeOutAnimation((Image)(p.Children[0]));
                            p.Children.RemoveAt(0);
                            break;
                        case 2:
                            p = (StackPanel)(CheckersGrid.Children[(CheckersGrid.Children.IndexOf(originalPanel) - 5)]);
                            FadeOutAnimation((Image)(p.Children[0]));
                            p.Children.RemoveAt(0);
                            break;
                        case 3:
                            p = (StackPanel)(CheckersGrid.Children[(CheckersGrid.Children.IndexOf(originalPanel) - 5)]);
                            FadeOutAnimation((Image)(p.Children[0]));
                            p.Children.RemoveAt(0);
                            break;
                    }

                }
            }//eating move Red
            else if (CurrentPlayre.Color.Equals("Black"))
            {
                if (CheckersGrid.Children.IndexOf(originalPanel) + 5 < CheckersGrid.Children.IndexOf(Target))
                {
                    StackPanel p;
                    statusLabel.Content = "black eat";
                    int mod = CheckersGrid.Children.IndexOf(originalPanel) % 4;
                    switch (mod)
                    {
                        case 0:
                            p = (StackPanel)(CheckersGrid.Children[(CheckersGrid.Children.IndexOf(originalPanel) + 5)]);
                            FadeOutAnimation((Image)(p.Children[0]));
                            p.Children.RemoveAt(0);
                            break;
                        case 1:
                            p = (StackPanel)(CheckersGrid.Children[(CheckersGrid.Children.IndexOf(originalPanel) + 5)]);
                            FadeOutAnimation((Image)(p.Children[0]));
                            p.Children.RemoveAt(0);
                            break;
                        case 2:
                            p = (StackPanel)(CheckersGrid.Children[(CheckersGrid.Children.IndexOf(originalPanel) + 3)]);
                            FadeOutAnimation((Image)(p.Children[0]));
                            p.Children.RemoveAt(0);
                            break;
                        case 3:
                            p = (StackPanel)(CheckersGrid.Children[(CheckersGrid.Children.IndexOf(originalPanel) + 3)]);
                            FadeOutAnimation((Image)(p.Children[0]));
                            p.Children.RemoveAt(0);
                            break;
                    }
                }
            }
            Target.Children.Add(PieceToMove);
            FadeInAnimation(PieceToMove);
            
        }

        public void EndTurn()
        {
            CheckGameEnd();
            statusLabel.Content = "End turn";            
            if (!isTeamGame)
            {
                if (CurrentPlayre.id == game.Player1.id)
                    CurrentPlayre = game.Player2;
                else
                    CurrentPlayre = game.Player1;
            }
            else
            {
                if (CurrentTeam.id == game.Team1.id)
                {
                    CurrentTeam = game.Team2;
                    if (team2Index < CurrentTeam.PlayersInTeam.Count())
                    {
                        CurrentPlayre = CurrentTeam.PlayersInTeam[team2Index++];
                    }
                    else
                    {
                        team2Index = 0;
                        CurrentPlayre = CurrentTeam.PlayersInTeam[team2Index++];
                    }
                    
                }
                else
                {
                    CurrentTeam = game.Team1;
                    if (team1Index < CurrentTeam.PlayersInTeam.Count())
                        CurrentPlayre = CurrentTeam.PlayersInTeam[team1Index++];
                    else
                    {
                        team1Index = 0;
                        CurrentPlayre = CurrentTeam.PlayersInTeam[team1Index++];
                    }
                    
                }


                if (CurrentTeam.id == MainTeam.id) {
                    if (MainTeam.id == game.Team1.id)
                        MainPlayer = MainTeam.PlayersInTeam[team1Index - 1];
                    else
                        MainPlayer = MainTeam.PlayersInTeam[team2Index - 1];
                }                    
            }

            statusLabel.Content = "it's " + CurrentPlayre.UserName + " turn";
            SelectedPiece = null;
            RestorBackground();
            if (PvE && (CurrentPlayre.id == game.Player2.id))
                ComputerMove();
            CheckGameEnd();
        }

        private void ComputerMove()
        {
            bool flag = true;
            foreach (UIElement e in CheckersGrid.Children)
            {
                StackPanel p = (StackPanel)e;
                if (p.Children.Count > 0)
                {
                    Image i = (Image)p.Children[0];
                    if (i.Source.ToString().Equals(BlackSorce) && CurrentPlayre.Color.Equals("Black"))
                    {
                        SelectedPiece = i;
                        OriginCol = Grid.GetColumn(p);
                        OriginRow = Grid.GetRow(p);
                        List<StackPanel> MoveList = getAllLegelMoves();
                        if (MoveList.Count > 0)
                        {
                            Random rng = new Random();
                            StackPanel temp = MoveList.ElementAt(rng.Next(MoveList.Count));
                            TargetCol = Grid.GetColumn(temp);
                            TargetRow = Grid.GetRow(temp);
                            makeTheMove(SelectedPiece, temp);
                            flag = false;
                            EndTurn();
                        }
                    }
                }
            }
            if (flag) {
                if (isTeamGame)
                    endGame(game.Team1);
                else
                    endGame(game.Player1);
            }
        }

        private void RestorBackground()
        {
            foreach (UIElement e in CheckersGrid.Children)
            {
                StackPanel p = (StackPanel)e;
                if (!(p.Background == BlackBackground || p.Background == WhiteBackground))
                    MarkPanel(p, Colors.Black);
            }
        }

        public void CheckGameEnd()
        {
            if (CurrentPlayre.Color.Equals("Red"))
            {
                for (int i = 0; i < 4; i++)
                {
                    StackPanel p = (StackPanel)CheckersGrid.Children[i];
                    if (p.Children.Count == 1)
                    {
                        Image img = (Image)p.Children[0];
                        if (img.Source.ToString() == RedSorce)
                        {
                            if (isTeamGame)
                                endGame(CurrentTeam);
                            else
                                endGame(CurrentPlayre);
                        }
                    }
                }
            }
            else
            {
                for (int i = 28; i < 32; i++)
                {
                    StackPanel p = (StackPanel)CheckersGrid.Children[i];
                    if (p.Children.Count == 1)
                    {
                        Image img = (Image)p.Children[0];
                        if (img.Source.ToString() == BlackSorce)
                        {
                            if (isTeamGame)
                                endGame(CurrentTeam);
                            else
                                endGame(CurrentPlayre);
                        }
                    }
                }
            }
        }

        public void endGame(Player winner)
        {          
            statusLabel.Content = "Game end winner is: " + winner.UserName;            
            if (MainPlayer.Color == "Red")
            {
                game.WinnerId = winner.id;
                InstanceContext context = new InstanceContext(this);
                proxy = new Service2Client(context);
                proxy.endGameServer(game, MoveToMove());              
            }
           
            CurrentPlayre = new Player { UserName = "", id = -999 , Color = "" };
            statusLabel.Content = "Game end winner is: " + winner.UserName;

        }
        public void endGame(Team winner)
        {
            statusLabel.Content = "Game end winning Team is: Team " + winner.TeamName;
            if (MainTeam.id == game.Team1.id)
            {
                InstanceContext context = new InstanceContext(this);
                proxy = new Service2Client(context);
                proxy.endTeamGameServer(game, winner, MoveToMove());
            }
            CurrentPlayre = new Player {UserName = "", id = -999 ,Color = ""};
            CurrentTeam = new Team { TeamName = "", id = -9999 };
            statusLabel.Content = "Game end winning Team is: Team " + winner.TeamName;

        }
        private ServiceReference1.Move[] MoveToMove() {

            List<ServiceReference1.Move> ml = new List<ServiceReference1.Move>();
            foreach (Move m in AllMoves) {
                ml.Add(new ServiceReference1.Move {
                    GameOfMove = m.GameOfMove,
                    MoveId = m.MoveId,
                    MoveString = m.MoveString,
                    PlayerOfMove = m.PlayerOfMove
                });
            }

            return ml.ToArray();
        }

        public void checkIfHasePiecess()
        {
            //if a player hase no peacess he lost
            Image i = null;

            foreach (UIElement e in CheckersGrid.Children)
            {
                StackPanel p = (StackPanel)e;
                if (p.Children.Count > 0)
                {
                    i = (Image)p.Children[0];
                    if (i.Source.ToString().Equals(RedSorce) && CurrentPlayre.Color.Equals("Red"))
                        return;
                    else if (i.Source.ToString().Equals(BlackSorce) && CurrentPlayre.Color.Equals("Black"))
                        return;
                }
            }

            if (CurrentPlayre.id == game.Player1.id)
            {
                if (isTeamGame)
                    endGame(game.Team2);
                else
                    endGame(game.Player2);
            }
            else
            {
                if (isTeamGame)
                    endGame(game.Team1);
                else
                    endGame(game.Player1);
            }

        }


        private void SaveMove()
        {
            Move move = new Move()
            {
                GameOfMove = game,
                PlayerOfMove = CurrentPlayre,
                MoveString = OriginCol.ToString() + "*" + OriginRow.ToString() + "$" + TargetCol.ToString() + "*" + TargetRow.ToString()
            };//oCol@oRow$tCol@tRow

            AllMoves.Add(move);
            if(!PvE)
                SendMove(move);
        }

        private void SendMove(Move move)
        {
            InstanceContext context = new InstanceContext(this);
            proxy = new Service2Client(context);
            string sendingTo = null;
            if (!isTeamGame)
            {
                if (CurrentPlayre == game.Player1)
                    sendingTo = creatPlayerString(game.Player2);
                else
                    sendingTo = creatPlayerString(game.Player1);
                string FullMoveString = sendingTo + "&" + move.MoveString + "&" + move.PlayerOfMove.Color;
                proxy.sendMove(FullMoveString);
            }
            else {
                ServiceReference1.Move m = new ServiceReference1.Move
                                                                 {
                                                                      MoveId = move.MoveId,
                                                                      GameOfMove = move.GameOfMove,
                                                                      PlayerOfMove = move.PlayerOfMove,
                                                                      MoveString = move.MoveString
                                                                  };
                if (CurrentTeam.id == game.Team1.id)
                    proxy.sendMoveTeam(game.Team2, m, move.PlayerOfMove.Color);
                else
                    proxy.sendMoveTeam(game.Team1, m, move.PlayerOfMove.Color);

            }
        }

        private string creatPlayerString(Player player1)
        {
            string playerString = player1.id.ToString() + "*"
                                + player1.UserName + "*"
                                + player1.Email;
            return playerString;
        }

        public void CallBackFunc(string str)
        {
            throw new NotImplementedException();
        }

        public void sendPlayer(string playerString)
        {
            throw new NotImplementedException();
        }

        public void sendGame(string gameString)
        {
            throw new NotImplementedException();
        }

        public void ReseveMove(string moveString, string color)
        {
            // throw new NotImplementedException();
            Move m = new Move()
            {
                MoveString = moveString
            };
            m.PlayerOfMove = new Player() { Color = color };

            if (game.Player1.Color == m.PlayerOfMove.Color)
                m.PlayerOfMove.id = game.Player1.id;
            else
                m.PlayerOfMove.id = game.Player2.id;

            makeMove(m);
            AllMoves.Add(m);
            EndTurn();
        }
        public void ReseveMoveTeam(ServiceReference1.Move m, string color)
        {
            //throw new NotImplementedException();
            Move move = new Move
            {
                MoveString = m.MoveString,
                PlayerOfMove = m.PlayerOfMove,
                GameOfMove = game
            };
            move.PlayerOfMove.Color = color;
           
            makeMove(move);
            AllMoves.Add(move);
            EndTurn();

        }


        private void makeMove(Move m)
        {
            //oCol@oRow$tCol@tRow
            string[] moveString = m.MoveString.Split('$');
            string[] original = moveString[0].Split('*');
            string[] target = moveString[1].Split('*');

            int OriginCol = int.Parse(original[0]);
            int OriginRow = int.Parse(original[1]);

            int TargetCol = int.Parse(target[0]);
            int TargetRow = int.Parse(target[1]);


            StackPanel originalPanel = (StackPanel)CheckersGrid.Children[(OriginCol + OriginRow * 4)];
            StackPanel Target = (StackPanel)CheckersGrid.Children[(TargetCol + TargetRow * 4)];
            Image PieceToMove = (Image)originalPanel.Children[0];
            originalPanel.Children.RemoveAt(0);
            
            //eating move Red
            if (m.PlayerOfMove.Color.Equals("Red"))
            {
                if (CheckersGrid.Children.IndexOf(originalPanel) - 5 > CheckersGrid.Children.IndexOf(Target))
                {
                    StackPanel p;
                    statusLabel.Content = "red eat";
                    int mod = CheckersGrid.Children.IndexOf(originalPanel) % 4;
                    switch (mod)
                    {
                        case 0:
                            p = (StackPanel)(CheckersGrid.Children[(CheckersGrid.Children.IndexOf(originalPanel) - 3)]);
                            p.Children.RemoveAt(0);
                            break;
                        case 1:
                            p = (StackPanel)(CheckersGrid.Children[(CheckersGrid.Children.IndexOf(originalPanel) - 3)]);
                            p.Children.RemoveAt(0);
                            break;
                        case 2:
                            p = (StackPanel)(CheckersGrid.Children[(CheckersGrid.Children.IndexOf(originalPanel) - 5)]);
                            p.Children.RemoveAt(0);
                            break;
                        case 3:
                            p = (StackPanel)(CheckersGrid.Children[(CheckersGrid.Children.IndexOf(originalPanel) - 5)]);
                            p.Children.RemoveAt(0);
                            break;
                    }

                }
            }//eating move Red
            else if (m.PlayerOfMove.Color.Equals("Black"))
            {
                if (CheckersGrid.Children.IndexOf(originalPanel) + 5 < CheckersGrid.Children.IndexOf(Target))
                {
                    StackPanel p;
                    statusLabel.Content = "black eat";
                    int mod = CheckersGrid.Children.IndexOf(originalPanel) % 4;
                    switch (mod)
                    {
                        case 0:
                            p = (StackPanel)(CheckersGrid.Children[(CheckersGrid.Children.IndexOf(originalPanel) + 5)]);
                            p.Children.RemoveAt(0);
                            break;
                        case 1:
                            p = (StackPanel)(CheckersGrid.Children[(CheckersGrid.Children.IndexOf(originalPanel) + 5)]);
                            p.Children.RemoveAt(0);
                            break;
                        case 2:
                            p = (StackPanel)(CheckersGrid.Children[(CheckersGrid.Children.IndexOf(originalPanel) + 3)]);
                            p.Children.RemoveAt(0);
                            break;
                        case 3:
                            p = (StackPanel)(CheckersGrid.Children[(CheckersGrid.Children.IndexOf(originalPanel) + 3)]);
                            p.Children.RemoveAt(0);
                            break;
                    }
                }
            }

            Target.Children.Add(PieceToMove);

        }

       /* private string crateEndGameString(int winnerID)
        {
            //game,winner,moves
            string endGameString = creatGameString() + "*" + winnerID.ToString() + "*" + MoveListTOString();

            return endGameString;
        }

        private string MoveListTOString()
        {
            string MoveListString = "";
            foreach (Move m in AllMoves)
            {//moveString#playerOfMoveID#GameID
                MoveListString += m.MoveString + "#" + m.PlayerOfMove.id.ToString() + "#" + game.id.ToString();
                MoveListString += "%";
            }
            return MoveListString;
        }

        private string creatGameString()
        {
            string gameString;

            string player1String = creatPlayerString(game.Player1);
            string player2String = creatPlayerString(game.Player2);
            //id$p1s$p2s
            gameString = game.id.ToString() + "$" + player1String + "$" + player2String;

            return gameString;
        }*/




        public void sendTeam(Team t)
        {         
        }

        public void sendTeamGame(Game game)
        {
        }


        ////animations

        private void FadeOutAnimation(Image target)//, double newX, double newY)
        {////TODO: move to the on click           
            DoubleAnimation ani = new DoubleAnimation(0, TimeSpan.FromSeconds(0.25));            
        } 
        private void FadeInAnimation(Image target) {            
            DoubleAnimation ani = new DoubleAnimation(1, TimeSpan.FromSeconds(0.25));
            target.BeginAnimation(Image.OpacityProperty, ani);
            
        }

    

      
        ////////end of class
    }

}
