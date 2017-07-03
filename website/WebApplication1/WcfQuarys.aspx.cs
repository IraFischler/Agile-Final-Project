using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebApplication1.ServiceReference1;
using System.ComponentModel;


namespace WebApplication1
{
    public partial class WebForm2 : System.Web.UI.Page
    {

        private ServiceReference1.Service1Client client;
        
        protected void Page_Load(object sender, EventArgs e)
        {
            client = new ServiceReference1.Service1Client();
            try
            {
                if (!Page.IsPostBack)
                {
                    this.DropDownListPlayers.SelectedIndexChanged -= new System.EventHandler(DropDownListPlayers_SelectedIndexChanged);
                    this.DropDownListGames.SelectedIndexChanged -= new System.EventHandler(DropDownListGames_SelectedIndexChanged);

                    List<string> nList = client.getAllPlayersNames().ToList<string>();

                    DropDownListPlayers.DataSource = nList;
                    DropDownListPlayers.DataBind();


                    List<string> gList = client.getAllGamesForCombo().ToList<string>();

                    DropDownListGames.DataSource = gList;
                    DropDownListGames.DataBind();

                   this.DropDownListPlayers.SelectedIndexChanged += new System.EventHandler(DropDownListPlayers_SelectedIndexChanged);
                   this.DropDownListGames.SelectedIndexChanged += new System.EventHandler(DropDownListGames_SelectedIndexChanged);
                                    }
            }
            catch (Exception ex)
            {
                Panel2.Visible = false;
                Response.Write(ex.ToString());
            }
        }

        protected void DisplayAllPlayers(object sender, EventArgs e)
        {
            try
            {
                List<Player> pList = client.getAllPlayers().ToList<Player>();

                GridView1.DataSource = pList;
                GridView1.DataBind();
            }
            catch (Exception ex)
            {
                Panel2.Visible = false;
                Response.Write(ex.ToString());
            }

        }

        protected void Button1_Click(object sender, EventArgs e)
        { 
            List<object> list = new List<object>();// = new List<T>();
            string str = client.CountPlayerGames();
            string[] sArr = str.Split('^');
            foreach (string s in sArr) {
                string[] sa = s.Split(' ');
                var g = new {Id = sa[0],Name = sa[2] , Count = sa[1]};
                list.Add(g);
                GridView1.DataSource = list;
                GridView1.DataBind();
            }
        }

        protected void DisplayAllGames(object sender, EventArgs e)
        {
            try
            {
                List<Game> gList = client.getAllGames().ToList<Game>();

                GridView1.DataSource = gList;
                GridView1.DataBind();
            }
            catch (Exception ex)
            {
                Panel2.Visible = false;
                Response.Write(ex.ToString());
            }
        }

        protected void DropDownListPlayers_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {            
                List<Game> gList = client.GetAllGameOfPlayer((DropDownListPlayers.SelectedItem).ToString()).ToList<Game>();
                List<object> list = new List<object>();
                foreach (Game g in gList)
                {
                    var v = new { StartTime = g.StartTime,ID = g.id };
                    list.Add(v);                   
                }
                GridView1.DataSource = list;
                GridView1.DataBind();

            }
            catch (Exception ex)
            {
                Panel2.Visible = false;
                Response.Write(ex.ToString());
            }
        }

        protected void DropDownListGames_SelectedIndexChanged(object sender, EventArgs e)
        {
            string gameId = ((DropDownListGames.SelectedItem).ToString().Split(' '))[0].ToString();
            try {
                string ans = client.GetAllPlayersOfGame2(gameId);
                List<Player> pList = playerStringListToPlayersList(ans);
                List<object> list = new List<object>();
                foreach (Player p in pList)
                {
                    var v = new { Email = p.Email, ID = p.id, UserName = p.UserName};
                    list.Add(v);
                }
                GridView1.DataSource = list;
                GridView1.DataBind();

            }
            catch (Exception ex)
            {
                Panel2.Visible = false;
                Response.Write(ex);
            }
        }     

        protected void DeletePlayer_Click(object sender, EventArgs e)
        {         

            try
            {
                string pName = DropDownListPlayers.SelectedItem.ToString();
                List<string> nList = client.DeletePlayer(pName).Split('*').ToList<string>();


                DropDownListPlayers.DataSource = nList;
                DropDownListPlayers.DataBind();
            }
            catch (Exception ex)
            {
                Panel2.Visible = false;
                Response.Write(ex.ToString());
            }

        }

        private List<Player> playerStringListToPlayersList(string pListString) {

            string[] sArr = pListString.Split('*');
            List<Player> pList = new List<Player>();
            foreach (string s in sArr) {
                pList.Add(StringToPlayer(s));
            }
            return pList;
        }

        private Player StringToPlayer(string playrString) {
            string[] sArr = playrString.Split('@');
            Player p = new Player {
                id = Int32.Parse(sArr[0]),
                UserName = sArr[1],
                Email = sArr[2] + "@" + sArr[3]
            };
            return p;
        }

        protected void DeleteGame_Click(object sender, EventArgs e)
        {
            try
            {

                string gameId = ((DropDownListGames.SelectedItem).ToString().Split(' '))[0].ToString();
               
                client.DeleteGame(gameId);

                List<string> gList = client.getAllGamesForCombo().ToList<string>();

                DropDownListGames.DataSource = gList;
                DropDownListGames.DataBind();

                GridView1.DataSource = null;
                GridView1.DataBind();
            }
            catch (Exception ex)
            {
                Panel2.Visible = false;
                Response.Write(ex.ToString());
            }
        }

        protected void UpdatePlayer_Click(object sender, EventArgs e)
        {
            string name = UpdateName.Text;
            string email = UpdateEmail.Text;
            string pName = DropDownListPlayers.SelectedItem.ToString();

            List<string> nList = client.UpdatePlayer(name + " " +email + " " + pName).Split('*').ToList<string>();


            DropDownListPlayers.DataSource = nList;
            DropDownListPlayers.DataBind();

        }

        protected void Abaut_Click(object sender, EventArgs e)
        {

        }

        protected void WcfQuarys_Click(object sender, EventArgs e)
        {

        }
    }
}