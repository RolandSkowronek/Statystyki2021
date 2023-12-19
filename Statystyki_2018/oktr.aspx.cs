using OfficeOpenXml;
using System;
using System.Data;
using System.Globalization;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Statystyki_2018
{
    public partial class oktr : System.Web.UI.Page
    {
        public Class1 cl = new Class1();
        public common cm = new common();
        public tabele tabela = new tabele();
        public dataReaders dr = new dataReaders();
        private const string tenPlik = "oktr.aspx";

        private int storid = 0;
        private int rowIndex = 1;

        protected void Page_Load(object sender, EventArgs e)
        {
             string idWydzial = Request.QueryString["w"]; Session["czesc"] = cm.nazwaFormularza(tenPlik, idWydzial) ;
            try
            {
                if (idWydzial == null)
                {
                    Server.Transfer("default.aspx");
                    return;
                }
                String IdentyfikatorUzytkownika = string.Empty;
                IdentyfikatorUzytkownika = (string)Session["identyfikatorUzytkownika"];
                DataTable parametry = cm.makeParameterTable();
                parametry.Rows.Add("@identyfikatorUzytkownika", IdentyfikatorUzytkownika);


                if (cm.getQuerryValue("select admin from uzytkownik where ident =@identyfikatorUzytkownika", cm.con_str, parametry) == "0" && !cm.dostep(idWydzial, (string)Session["identyfikatorUzytkownika"]))
                {
                    Server.Transfer("default.aspx?info='Użytkownik " + (string)Session["identyfikatorUzytkownika"] + " nie praw do działu nr " + idWydzial + "'");
                }
                else
                {
                    CultureInfo newCulture = (CultureInfo)CultureInfo.CurrentCulture.Clone();
                    newCulture.DateTimeFormat = CultureInfo.GetCultureInfo("PL").DateTimeFormat;
                    System.Threading.Thread.CurrentThread.CurrentCulture = newCulture;
                    System.Threading.Thread.CurrentThread.CurrentUICulture = newCulture;
                    DateTime dTime = DateTime.Now.AddMonths(-1); ;

                    if (Date1.Text.Length == 0) Date1.Date = DateTime.Parse(dTime.Year.ToString() + "-" + dTime.Month.ToString("D2") + "-01");
                    if (Date2.Text.Length == 0) Date2.Date = DateTime.Parse(dTime.Year.ToString() + "-" + dTime.Month.ToString("D2") + "-" + DateTime.DaysInMonth(dTime.Year, dTime.Month).ToString("D2"));
                    Session["id_dzialu"] = idWydzial;
                    Session["data_1"] = Date1.Date.ToShortDateString();
                    Session["data_2"] = Date2.Date.ToShortDateString();

                    if (!IsPostBack)
                    {
                        var fileContents = System.IO.File.ReadAllText(Server.MapPath(@"~//version.txt"));    // file read with version
                        this.Title = "Statystyki " + fileContents.ToString().Trim();

                        odswiez();
                        makeLabels();
                    }
                }
            }
            catch
            {
                //  Server.Transfer("default.aspx");
            }
        }// end of Page_Load

        protected void odswiez()
        {
            string dzial = (string)Session["id_dzialu"];
            string txt = string.Empty;

            try
            {
                DataTable tabelka01 = dr.generuj_dane_do_tabeli_wierszy2018(Date1.Date, Date2.Date, (string)Session["id_dzialu"], 2, 10, 20, false, tenPlik);
                Session["tabelka002"] = tabelka01;
            }
            catch
            { }
            try
            {
                DataTable tabelka01 = dr.generuj_dane_do_tabeli_wierszy2018(Date1.Date, Date2.Date, (string)Session["id_dzialu"], 4, 10, 25, false, tenPlik);
                Session["tabelka004"] = tabelka01;
            }
            catch
            { }
            try
            {
                DataTable tabelka01 = dr.generuj_dane_do_tabeli_wierszy2018(Date1.Date, Date2.Date, (string)Session["id_dzialu"], 6, 10, 25, false, tenPlik);
                Session["tabelka006"] = tabelka01;
            }
            catch
            { }
            try
            {
                //cm.log.Info(tenPlik + ": rozpoczęcie tworzenia tabeli 1");
                Session["tabelka001"] = dr.tworzTabele(int.Parse(dzial), 1, Date1.Date, Date2.Date, 30, GridView1, tenPlik);
                GridView1.DataBind();
               
            }
            catch (Exception ex)
            {
                cm.log.Error(tenPlik + " " + ex.Message);
            }

            // dopasowanie opisów
            makeLabels();

            try
            {
                Label11.Visible = cl.debug(int.Parse(dzial));
                infoLabel2.Visible = cl.debug(int.Parse(dzial));
              
            }
            catch
            {
                Label11.Visible = false;
                infoLabel2.Visible = false;
               
            }

            Label11.Text = txt;
            Label3.Text = cl.nazwaSadu((string)Session["id_dzialu"]);
        }

        #region "nagłowki tabel"

        protected void makeLabels()
        {
            try
            {
                string User_id = string.Empty;
                string domain = string.Empty;
                try
                {
                    User_id = (string)Session["user_id"];
                    domain = (string)Session["damain"];
                }
                catch
                { }
                Label3.Text = cl.nazwaSadu((string)Session["id_dzialu"]);

                //	id_dzialu.Text = (string)Session["txt_dzialu"];
                Label28.Text = cl.podajUzytkownika(User_id, domain);
                Label29.Text = DateTime.Now.ToLongDateString();
                try
                {
                    Label30.Text = System.IO.File.ReadAllText(Server.MapPath(@"~//version.txt")).ToString().Trim();
                }
                catch
                { }

                string strMonthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(Date2.Date.Month);
                int last_day = DateTime.DaysInMonth(Date2.Date.Year, Date2.Date.Month);
                if (((Date1.Date.Day == 1) && (Date2.Date.Day == last_day)) && ((Date1.Date.Month == Date2.Date.Month)))
                {
                    // cały miesiąc
                    tabela2Label.Text = "Sprawozdanie za miesiąc " + strMonthName + " " + Date2.Date.Year.ToString() + " roku.";
                   
                }
                else
                {
                    tabela2Label.Text = "Sprawozdanie  za okres od " + Date1.Text + " do  " + Date2.Text;
                
                }
            }
            catch
            { }
        }

        private DataTable naglowkTabeli01()
        {
            #region tabela  1

            DataTable dT_01 = new DataTable();
            dT_01.Columns.Clear();
            dT_01.Columns.Add("Column1", typeof(string));
            dT_01.Columns.Add("Column2", typeof(string));
            dT_01.Columns.Add("Column3", typeof(string));
            dT_01.Columns.Add("Column4", typeof(string));
            dT_01.Columns.Add("Column5", typeof(string));
            dT_01.Columns.Add("Column6", typeof(string));
            dT_01.Rows.Add(new Object[] { "1", "rozprawy", "1", "1" });
            dT_01.Rows.Add(new Object[] { "1", "posiedzenia", "1", "1" });
            for (int i = 0; i < 2; i++)
            {
                
                dT_01.Rows.Add(new Object[] { "1", "Nkd", "1", "1" });
                dT_01.Rows.Add(new Object[] { "1", "Nmo", "1", "1" });
                dT_01.Rows.Add(new Object[] { "1", "Nsm", "1", "1" });
                dT_01.Rows.Add(new Object[] { "1", "Ns", "1", "1" });
                dT_01.Rows.Add(new Object[] { "1", "RC", "1", "1" });
                dT_01.Rows.Add(new Object[] { "1", "RNs", "1", "1" });
                dT_01.Rows.Add(new Object[] { "1", "RCo", "1", "1" });
                dT_01.Rows.Add(new Object[] { "1", "RCps", "1", "1" });
                dT_01.Rows.Add(new Object[] { "1", "RCz", "1", "1" });
                dT_01.Rows.Add(new Object[] { "1", "Razem", "1", "1" });
            }
           
           
            dT_01.Rows.Add(new Object[] { "2", "L.p.", "1", "2", "h" });
            dT_01.Rows.Add(new Object[] { "2", "Nazwisko i imię sędziego wg funkcji w Wydziale", "1", "2", "h" });
            dT_01.Rows.Add(new Object[] { "2", "podstawowy wskaźnik przydziału", "1", "2", "h" });
            dT_01.Rows.Add(new Object[] { "2", "Ilość sesji", "2", "1", "h" });
            dT_01.Rows.Add(new Object[] { "2", "Liczba przesłuchanych osób", "1", "2", "h" });
            dT_01.Rows.Add(new Object[] { "2", "WPŁYW", "10", "1", "h" });
            
            dT_01.Rows.Add(new Object[] { "2", "ZAŁATWIENIA", "10", "1", "h" });
            
            dT_01.Rows.Add(new Object[] { "2", "Absencja w dniach roboczych", "1", "2", "h" });

            return dT_01;

            #endregion tabela  1
        }

      
       

        #endregion "nagłowki tabel"

        protected void Button3_Click(object sender, EventArgs e)
        {
            string path = Server.MapPath("Template") + "\\oktr.xlsx";
            FileInfo existingFile = new FileInfo(path);
            string download = Server.MapPath("Template") + @"\oktr";
            FileInfo fNewFile = new FileInfo(download + "_.xlsx");

            DataTable table = (DataTable)Session["tabelka001"];
            using (ExcelPackage MyExcel = new ExcelPackage(existingFile))
            {
                // pierwsza
                int rowik = 0;
                ExcelWorksheet MyWorksheet1 = MyExcel.Workbook.Worksheets[1];

                MyWorksheet1 = tabela.tworzArkuszwExcle(MyExcel.Workbook.Worksheets[1], (DataTable)Session["tabelka001"], 28, 0, 6, true, false, false, false, false);
                rowik = table.Rows.Count - 3;

                tabela.komorkaExcela(MyExcel.Workbook.Worksheets[1], rowik + 7, 2, "pozostało z okresu poprzedniego", true, 0, 4);
                tabela.komorkaExcela(MyExcel.Workbook.Worksheets[1], rowik + 8, 2, "Wpływ spraw", true, 0, 5);
                tabela.komorkaExcela(MyExcel.Workbook.Worksheets[1], rowik + 9, 2, "Załatwienia", true, 0, 5);
                tabela.komorkaExcela(MyExcel.Workbook.Worksheets[1], rowik + 10, 2, "pozostało na okres następny", true, 0, 5);
                tabela.komorkaExcela(MyExcel.Workbook.Worksheets[1], rowik + 11, 2, "w tym", true, 4, 1);
                tabela.komorkaExcela(MyExcel.Workbook.Worksheets[1], rowik + 11, 5, "pow 3-6 miesiący", true, 0, 1);
                tabela.komorkaExcela(MyExcel.Workbook.Worksheets[1], rowik + 12, 5, "pow 6-12 miesięcy", true, 0, 1);
                tabela.komorkaExcela(MyExcel.Workbook.Worksheets[1], rowik + 13, 5, "1-2 lat", true, 0, 1);
                tabela.komorkaExcela(MyExcel.Workbook.Worksheets[1], rowik + 14, 5, "2 do 3 lat", true, 0, 1);
                tabela.komorkaExcela(MyExcel.Workbook.Worksheets[1], rowik + 15, 5, "pow. 3 lat", true, 0, 1);

                MyWorksheet1 = tabela.tworzArkuszwExcleBezSedziow(MyExcel.Workbook.Worksheets[1], (DataTable)Session["tabelka002"], 9, 18, 6, rowik + 7, false);


                try
                {
                    MyExcel.SaveAs(fNewFile);

                    this.Response.Clear();
                    this.Response.ContentType = "application/vnd.ms-excel";
                    this.Response.AddHeader("Content-Disposition", "attachment;filename=" + fNewFile.Name);
                    this.Response.WriteFile(fNewFile.FullName);
                    this.Response.End();
                }
                catch (Exception ex)
                {
                    cm.log.Error(tenPlik + " " + ex.Message);
                }
            }//end of using

            odswiez();
        }

        protected void LinkButton54_Click(object sender, EventArgs e)
        {
            odswiez();
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                storid = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "id").ToString());
            }
        }

        protected void GridView3_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                storid = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "id").ToString());
            }
        }

        protected void GridView2_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                storid = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "id").ToString());
            }
        }

        private void AddNewRow(object sender, int iloscKolumn, int idTabeli, DataTable tabelkaZdanymi, GridViewRowEventArgs e)
        {
            GridView GridView1 = (GridView)sender;
            GridViewRow NewTotalRow = new GridViewRow(0, 0, DataControlRowType.DataRow, DataControlRowState.Insert);
            try
            {
                GridView1.Controls[0].Controls.AddAt(e.Row.RowIndex + rowIndex, tabela.PodsumowanieTabeli((DataTable)Session["tabelka001"], 25, "borderAll center gray"));
            }
            catch (Exception ex)
            {
                cm.log.Error(tenPlik + " " + ex.Message);
            }
            podtabela(idTabeli, iloscKolumn, tabelkaZdanymi, sender, e);
        }

        private void AddNewRow2(object sender, int iloscKolumn, int idTabeli, DataTable tabelkaZdanymi, GridViewRowEventArgs e)
        {
            podtabela2(idTabeli, iloscKolumn, tabelkaZdanymi, sender, e);
        }


        private void podtabela(int idTabeli, int IloscKolumn, DataTable tabelkaZdanymi, object sender, GridViewRowEventArgs e)
        {
            GridView GridView1 = (GridView)sender;
            GridViewRow NewTotalRow = new GridViewRow(0, 0, DataControlRowType.DataRow, DataControlRowState.Insert);
            string idtabeli = idTabeli.ToString();

            int idWiersza = 1;
            GridView1.Controls[0].Controls.AddAt(e.Row.RowIndex + rowIndex, tabela.wierszTabeli(tabelkaZdanymi, IloscKolumn, idWiersza, idtabeli, "Pozostało z okresu poprzedniego", 6, 1, "normal", "borderTopLeft col_35 normal"));

            idWiersza = 2;
            GridView1.Controls[0].Controls.AddAt(e.Row.RowIndex + rowIndex, tabela.wierszTabeli(tabelkaZdanymi, IloscKolumn, idWiersza, idtabeli, "Wpływ spraw", 6, 1, "normal", "borderTopLeft col_35 normal"));

            idWiersza = 3;
            GridView1.Controls[0].Controls.AddAt(e.Row.RowIndex + rowIndex, tabela.wierszTabeli(tabelkaZdanymi, IloscKolumn, idWiersza, idtabeli, "Załatwienie", 6, 1, "normal", "borderTopLeft col_35 normal"));

            idWiersza = 4;
            GridView1.Controls[0].Controls.AddAt(e.Row.RowIndex + rowIndex, tabela.wierszTabeli(tabelkaZdanymi, IloscKolumn, idWiersza, idtabeli, "Pozostało na okres następny", 6, 1, "normal", "borderTopLeft col_35 normal"));

            idWiersza = 5;
            GridView1.Controls[0].Controls.AddAt(e.Row.RowIndex + rowIndex, tabela.wierszTabeli(tabelkaZdanymi, IloscKolumn, idWiersza, idtabeli, "powyżej 3 m-cy do 6 m-cy", 3, 1, "normal", "borderTopLeft col_35 normal", "ZALEGŁOŚCI", 7, 3, "borderTopLeft normal"));

            idWiersza = 6;
            GridView1.Controls[0].Controls.AddAt(e.Row.RowIndex + rowIndex, tabela.wierszTabeli(tabelkaZdanymi, IloscKolumn, idWiersza, idtabeli, "powyżej 6 m-cy do 12 m-cy", 3, 1, "normal", "borderTopLeft col_35 normal"));

            idWiersza = 7;
            GridView1.Controls[0].Controls.AddAt(e.Row.RowIndex + rowIndex, tabela.wierszTabeli(tabelkaZdanymi, IloscKolumn, idWiersza, idtabeli, "powyżej 12 m-cy do 2 lat", 3, 1, "normal", "borderTopLeft col_35 normal"));

            idWiersza = 8;
            GridView1.Controls[0].Controls.AddAt(e.Row.RowIndex + rowIndex, tabela.wierszTabeli(tabelkaZdanymi, IloscKolumn, idWiersza, idtabeli, "powyżej 2 lat do 3 lat", 3, 1, "normal", "borderTopLeft col_35 normal"));

            idWiersza = 9;
            GridView1.Controls[0].Controls.AddAt(e.Row.RowIndex + rowIndex, tabela.wierszTabeli(tabelkaZdanymi, IloscKolumn, idWiersza, idtabeli, "powyżej 3 lat", 3, 1, "normal", "borderTopLeft col_35 normal"));
        }

        private void podtabela2(int idTabeli, int IloscKolumn, DataTable tabelkaZdanymi, object sender, GridViewRowEventArgs e)
        {
            GridView GridView1 = (GridView)sender;
            GridViewRow NewTotalRow = new GridViewRow(0, 0, DataControlRowType.DataRow, DataControlRowState.Insert);
            string idtabeli = idTabeli.ToString();

            int idWiersza = 1;
            GridView1.Controls[0].Controls.AddAt(e.Row.RowIndex + rowIndex, tabela.wierszTabeli(tabelkaZdanymi, IloscKolumn, idWiersza, idtabeli, "pozostało z okresu poprzedniego", 4, 1, "normal", "borderTopLeft col_35 normal"));

            idWiersza = 2;
            GridView1.Controls[0].Controls.AddAt(e.Row.RowIndex + rowIndex, tabela.wierszTabeli(tabelkaZdanymi, IloscKolumn, idWiersza, idtabeli, "Wpływ spraw", 4, 1, "normal", "borderTopLeft col_35 normal"));

            idWiersza = 3;
            GridView1.Controls[0].Controls.AddAt(e.Row.RowIndex + rowIndex, tabela.wierszTabeli(tabelkaZdanymi, IloscKolumn, idWiersza, idtabeli, "Załatwienie", 4, 1, "normal", "borderTopLeft col_35 normal"));

            idWiersza = 4;
            GridView1.Controls[0].Controls.AddAt(e.Row.RowIndex + rowIndex, tabela.wierszTabeli(tabelkaZdanymi, IloscKolumn, idWiersza, idtabeli, "pozostało na okres następny", 4, 1, "normal", "borderTopLeft col_35 normal"));

            idWiersza = 5;
            GridView1.Controls[0].Controls.AddAt(e.Row.RowIndex + rowIndex, tabela.wierszTabeli(tabelkaZdanymi, IloscKolumn, idWiersza, idtabeli, "pow 3-6 miesiący ", 1, 1, "normal", "borderTopLeft col_35 normal", "w tym", 7, 3, "borderTopLeft normal"));

            idWiersza = 6;
            GridView1.Controls[0].Controls.AddAt(e.Row.RowIndex + rowIndex, tabela.wierszTabeli(tabelkaZdanymi, IloscKolumn, idWiersza, idtabeli, " pow 6-12 miesięcy", 1, 1, "normal", "borderTopLeft col_35 normal"));

            idWiersza = 7;
            GridView1.Controls[0].Controls.AddAt(e.Row.RowIndex + rowIndex, tabela.wierszTabeli(tabelkaZdanymi, IloscKolumn, idWiersza, idtabeli, "1-2 lat", 1, 1, "normal", "borderTopLeft col_35 normal"));

            idWiersza = 8;
            GridView1.Controls[0].Controls.AddAt(e.Row.RowIndex + rowIndex, tabela.wierszTabeli(tabelkaZdanymi, IloscKolumn, idWiersza, idtabeli, "2 do 3 lat)", 1, 1, "normal", "borderTopLeft col_35 normal"));

            idWiersza = 9;
            GridView1.Controls[0].Controls.AddAt(e.Row.RowIndex + rowIndex, tabela.wierszTabeli(tabelkaZdanymi, IloscKolumn, idWiersza, idtabeli, "pow. 3 lat", 1, 1, "normal", "borderTopLeft col_35 normal"));
        }


        protected void GridView1_GenerowanieNaglowka(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                tabela.makeHeader(naglowkTabeli01(), GridView1);
            }
            else
            {
                if ((storid > 0) && (DataBinder.Eval(e.Row.DataItem, "id") == null))
                {
                    rowIndex = 0;
                    AddNewRow(sender, 11, 2, (DataTable)Session["tabelka002"], e);
                }
            }
        }

       
    }
}