using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Media;
using Checkers.ServiceReference1;

namespace Checkers
{
    class MovmentValidation
    {
    
        private string RedSorce;
        private string BlackSorce;
        private Game game;

        public MovmentValidation()
        {}

        public MovmentValidation(string RedSorce, string BlackSorce,Game game) {
            this.BlackSorce = BlackSorce;
            this.RedSorce = RedSorce;
            this.game = game;
        }
    
        public string ValidateImagePlayer2(Image tempImage, Player CurrentPlayre)
        {
            string s = "";
            if (tempImage.Source.ToString().Equals(RedSorce) && CurrentPlayre.Color.Equals("Red"))
                s = "...............Red Click";
            else if (tempImage.Source.ToString().Equals(BlackSorce) && CurrentPlayre.Color.Equals("Black"))
                s = "...............Black Click";

            return s;
        }


        public bool ValidateImagePlayer(Image tempImage, Player CurrentPlayre)
        {
            if (tempImage.Source.ToString().Equals(RedSorce) && CurrentPlayre.Color.Equals("Red"))
                return true;
            else if (tempImage.Source.ToString().Equals(BlackSorce) && CurrentPlayre.Color.Equals("Black"))
                return true;
            return false;
        }

        public bool isTeamGame() {
            return !((game.Team1 == null) && (game.Team2 == null));
        }
    }
}
