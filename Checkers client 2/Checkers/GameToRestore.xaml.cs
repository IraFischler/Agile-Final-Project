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
using System.Threading;


namespace Checkers
{
    /// <summary>
    /// Interaction logic for GameToRestore.xaml
    /// </summary>
    public partial class GameToRestore : Window
    {
        private Service1Client client;
        private Game game;
        private List<Move> mList;
        private List<string> gList;
        private int gameID;
        private bool flag = false;
        public GameToRestore()
        {
            InitializeComponent();
            mList = new List<Move>();
            client = new Service1Client();
            try {
                gList = client.getAllGamesForCombo().ToList<string>();
                comboBox.ItemsSource= gList;
            }
            catch (Exception ex) {
                dibagDialog.Text = ex.ToString();
            }
        }

        private void comboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            Thread thread = new Thread(new ThreadStart(getMoves));
            thread.IsBackground = true;
            string gameString = (sender as ComboBox).SelectedItem as string;
            try
            {               
                gameID = Int32.Parse(gameString.Split(' ')[0]);
                thread.Start();
                //Game game = client.GetGameById(gameID); 
               
            }
            catch (Exception ex){ dibagDialog.Text = ex.ToString(); }            
        }
        private void getMoves() {
            var ml = client.GetAllMovesOfGame(gameID);
            //sleep for 3s
            if (flag)
                Thread.Sleep(3000);
            mList = new List<Move>();
            foreach (var m in ml)
            {
                mList.Add(new Move { MoveId = m.MoveId, GameOfMove = m.GameOfMove, MoveString = m.MoveString, PlayerOfMove = m.PlayerOfMove });
            }

            game = client.GetGameById(gameID);
            //sleep fpr 3s
            if (flag)
                Thread.Sleep(3000);
            LaunchGame();
        }


        private void LaunchGame() {
            if (!Dispatcher.CheckAccess()) // CheckAccess returns true if you're on the dispatcher thread
            {
                Dispatcher.Invoke(new Action(LaunchGame));
                return;
            }
            restorGame win = new restorGame(game, (List<Move>)mList);
            win.Show();
        }

        private void HandleCheck(object sender, RoutedEventArgs e)
        {
            flag = true;
        }

        private void HandleUnchecked(object sender, RoutedEventArgs e)
        {
            flag = false;
        }

        
    }
}
