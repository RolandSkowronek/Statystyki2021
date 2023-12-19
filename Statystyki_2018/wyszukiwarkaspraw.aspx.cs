using System;
using System.Data;

namespace Statystyki_2018
{

    public partial class wyszukiwarka2 : System.Web.UI.Page
    {
        public Class1 cl = new Class1();
        public wyszukiwarka w1 = new wyszukiwarka();
        protected void Page_Load(object sender, EventArgs e)
        {


            if (!IsPostBack)
            {
                listaSpraw.DataSource = null;
                listaSpraw.DataSourceID = null;
                listaSpraw.Columns.Clear();
                listaSpraw.DataBind();
                try
                {
                    bool czy = (bool)Session["initial"];
                    if (!czy)
                    {
                        listaSpraw.DataSource = null;
                        listaSpraw.DataSourceID = null;

                        listaSpraw.DataBind();
                    }
                }
                catch (Exception)
                {
                    listaSpraw.DataSource = null;
                    listaSpraw.DataSourceID = null;

                    listaSpraw.DataBind();

                }
                try
                {
                    // file read
                    try
                    {
                        var fileContents = System.IO.File.ReadAllText(Server.MapPath(@"~//version.txt"));
                        this.Title = "Wyszukiwarka spraw " + fileContents.ToString().Trim();

                    }
                    catch
                    {
                    }
                  
               
                }
                catch
                {
                    //Server.Transfer("default.aspx");
                }

            }

            ASPxComboBox1.Items.Clear();
            DataTable dT = new DataTable();
            dT = w1.PobierzOpisy("wyszukiwarka","wyszukiwarka");
          
            foreach (DataRow dR in dT.Rows)
            {
                string ident = dR[1].ToString().Trim();
                int id = 0;
                try
                {
                    id = 100 + int.Parse(ident);
                }
                catch (Exception ex)
                {
                    cl.log.Error("Wyszukiwarka wyszukiwarka2 " + ex.Message);
                }

                string user = string.Empty;
                string domain = string.Empty;
                try
                {
                    user = (string)Session["user_id"];
                    domain = (string)Session["damain"];
                }
                catch (Exception ex)
                {
                    cl.log.Error("Wyszukiwarka wyszukiwarka2 " + ex.Message);
                }

                string pozwolenie = cl.czy_dostepny(user, id.ToString(), domain);
                if (pozwolenie != "0")
                {
                    ASPxComboBox1.Items.Add(dR[0].ToString());
                }
            }
            
            if (ASPxComboBox1.SelectedIndex == -1)
            {
                ASPxComboBox1.SelectedIndex = 0;
            }
        }


        protected void makeGrid(string connectionString, string kwerenda)
        {

            // odczytaj dane
            try
            {
                SqlDataSource1.ConnectionString = connectionString;
            }
            catch (Exception ex)
            {
                cl.log.Error("Wyszukiwarka makeGrid " + ex.Message);
            }


            Session["imie"] = imie.Text.Trim();
            Session["nazwisko"] = Nazwisko.Text.Trim();
            if (string.IsNullOrEmpty (kwerenda))
            {
                return;
            }
            SqlDataSource1.SelectCommand = kwerenda;


            SqlDataSource1.SelectParameters.Clear();
            SqlDataSource1.SelectParameters.Add("nazwisko", Nazwisko.Text.Trim());
            SqlDataSource1.SelectParameters.Add("imie", imie.Text.Trim());
            SqlDataSource1.SelectParameters["nazwisko"].ConvertEmptyStringToNull = false;
            SqlDataSource1.SelectParameters["imie"].ConvertEmptyStringToNull = false;
            SqlDataSource1.SelectParameters["nazwisko"].DefaultValue = Nazwisko.Text.Trim();
            SqlDataSource1.SelectParameters["imie"].DefaultValue = imie.Text.Trim();
            try
            {
                listaSpraw.DataSourceID = null;
                SqlDataSource1.DataBind();
                SqlDataSource1.CancelSelectOnNullParameter = false;
                listaSpraw.DataSource = SqlDataSource1;
                listaSpraw.AutoGenerateColumns = true;
                listaSpraw.Columns.Clear();
                listaSpraw.DataBind();
            }
            catch (Exception ex)
            {
                cl.log.Error("Wyszukiwarka makeGrid " + ex.Message);
            }
        }

        protected void Grid_Load(object sender, EventArgs e)
        {
            try
            {
                if ((bool)Session["initial"])
                {
                    string kwerenda = string.Empty;
                    string connectionString = string.Empty;
                    try
                    {
                        kwerenda = (string)Session["kwerenda"];
                        connectionString = (string)Session["connectionString"];
                        makeGrid(connectionString, kwerenda);
                    }
                    catch (Exception ex)
                    {
                        cl.log.Error("Wyszukiwarka Grid_Load " + ex.Message);
                    }
                  
                }
            }
            catch (Exception ex)
            {
                cl.log.Error("Wyszukiwarka Grid_Load " + ex.Message);
            }
        }


        protected void szukaj(object sender, EventArgs e)
        {
            string nazwa = ASPxComboBox1.Items[ASPxComboBox1.SelectedIndex].Text.Trim();
            string connectionString = w1.PobierzConnectionString(nazwa,"wyszukiwarka");
            string kwerenda = w1.PobierzDana2(nazwa);
            Session["connectionString"] = connectionString;
            Session["kwerenda"] = kwerenda;
            Session["initial"] = true;
            makeGrid(connectionString, kwerenda);
        }

        protected void LinkButton2_Click(object sender, EventArgs e)
        {
            ASPxGridViewExporter1.Styles.AlternatingRowCell.BackColor = System.Drawing.Color.LightGray;
            ASPxGridViewExporter1.WritePdfToResponse("Lista Spraw", false, new DevExpress.XtraPrinting.PdfExportOptions() { ShowPrintDialogOnOpen = true });
        }
    }
}