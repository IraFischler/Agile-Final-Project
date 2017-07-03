using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebApplication1.ServiceReference1;


namespace WebApplication1
{
    public partial class RegisterForm : System.Web.UI.Page
    {
        private ServiceReference1.Service1Client client;
        private int numOfPlayers;

        protected void Page_Load(object sender, EventArgs e)
        {
            client = new ServiceReference1.Service1Client();

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            Validate();
          
            if (Page.IsValid)
            {
                string name = TextBoxName.Text;
                string email = TextBoxEmail.Text;

                string b = client.RegisterPlayer(name, email);

                TextBoxName.Text = "";
                TextBoxEmail.Text = "";

            }
        }

   

        protected void NumOfPlayers_TextChanged(object sender, EventArgs e)
        {
            numOfPlayers = Int32.Parse((sender as TextBox).Text);
        }

     

        protected void TeamGameButton_Click(object sender, EventArgs e)
        {
            TextBox1.Enabled = false;
            if (Page.IsValid)
            {
                int num = Int32.Parse(TextBox1.Text);
                if (num > 0)
                {
                    string name = TextBoxName.Text;
                    string email = TextBoxEmail.Text;
                    string TeamName = TextBox2.Text;

                    string b = client.RegisterPlayerWithTeam(name, email, TeamName);

                    TextBox1.Text = (--num).ToString();
                    TextBoxName.Text = "";
                    TextBoxEmail.Text = "";
                }
            }
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            string TeamName = TextBox2.Text;
            client.registerTeam(TeamName);
            Button2.Enabled = false;
            TextBox2.Enabled = false;
        }
    }
}
