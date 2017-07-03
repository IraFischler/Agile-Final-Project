using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;


namespace WcfService4
{
    public partial class WebForm1 : System.Web.UI.Page
    {

        //private BindingSource TblBindingSource = new BindingSource();
        private SqlDataSource ds = new SqlDataSource();
        private SqlConnection con = new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\\Database1.mdf;Integrated Security=True");

        protected void Page_Load(object sender, EventArgs e)
        {
            object c = null;
            SqlCommand cmd = new SqlCommand("SELECT * FROM TblPlayers");
            try
            {
                con.Open();
                cmd.Connection = con;
                c = cmd.ExecuteReader();
                con.Close();
            }
            catch (Exception ex)
            {
                string b = ex.ToString();

                Panel2.Visible = false;
                Response.Write(b); 
                     con.Close();
            }

            GridView1.DataSource = c;
            /*
            string b = c.ToString();

            Panel2.Visible = false;
            Response.Write(b);*/
        }
    }
}