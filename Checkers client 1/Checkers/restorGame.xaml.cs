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
using System.Windows.Threading;
using System.Windows.Media.Animation;

namespace Checkers
{
    /// <summary>
    /// Interaction logic for restorGame.xaml
    /// </summary>
    public partial class restorGame : Window
    {
        private Game game;
        private int indexe = 0; 
        private List<Move> AllMoves = new List<Move>();
        DispatcherTimer dispatcherTimer = new DispatcherTimer();
        public restorGame(Game game, List<Move> AllMoves)
        {
            InitializeComponent();
            this.game = game;
            this.AllMoves = AllMoves;

            dispatcherTimer.Tick += new EventHandler(runMove);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            dispatcherTimer.Start();
        }

        private void runMove(object sender, EventArgs e) {//run all game
            if (indexe < AllMoves.Count)
                makeMove(AllMoves[indexe++]);
        }

        private void makeMove(Move m) {
            //oCol@oRow$tCol@tRow
            string[] moveString = m.MoveString.Split('$');
            string[] original = moveString[0].Split('@');
            string[] target = moveString[1].Split('@');

            int OriginCol = int.Parse(original[0]);
            int OriginRow = int.Parse(original[1]);

            int TargetCol = int.Parse(target[0]);
            int TargetRow = int.Parse(target[1]);


            StackPanel originalPanel = (StackPanel)CheckersGrid.Children[(OriginCol + OriginRow * 4)];
            StackPanel Target = (StackPanel)CheckersGrid.Children[(TargetCol + TargetRow * 4)];
            Image PieceToMove = (Image)originalPanel.Children[0];


            FadeOutAnimation(PieceToMove);

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
            FadeInAnimation(PieceToMove);
        }



        ////animations

        private void FadeOutAnimation(Image target)//, double newX, double newY)
        {////TODO: move to the on click           
            DoubleAnimation ani = new DoubleAnimation(0, TimeSpan.FromSeconds(0.25));
        }
        private void FadeInAnimation(Image target)
        {
            DoubleAnimation ani = new DoubleAnimation(1, TimeSpan.FromSeconds(0.25));
            target.BeginAnimation(Image.OpacityProperty, ani);

        }


    }
}
