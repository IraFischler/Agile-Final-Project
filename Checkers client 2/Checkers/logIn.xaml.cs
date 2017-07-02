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

namespace Checkers
{
    /// <summary>
    /// Interaction logic for logIn.xaml
    /// </summary>
    public partial class logIn : Window
    {

        public string name { get; set; }
        public string email { get; set; }

        public logIn()
        {
            InitializeComponent();
        }

        private void login_Click(object sender, RoutedEventArgs e)
        {
            name = nameTex.Text;
            email = emailTex.Text;
            this.Close();
        }
    }
}
