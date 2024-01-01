using OfficeOpenXml;
using System;
using System.Data;
using System.Globalization;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Statystyki_2018
{
    public partial class oglc : System.Web.UI.Page
    {
        public Class1 cl = new Class1();
        public common cm = new common();
        public tabele tb = new tabele();
        public dataReaders dr = new dataReaders();
        private const string tenPlik = "oglc.aspx";

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
                Server.Transfer("default.aspx");
            }
        }// end of Page_Load

        protected void odswiez()
        {
            if (Session["id_dzialu"] == null)
            {
                return;
            }

            try
            {
                DataTable tabelka01 = dr.generuj_dane_do_tabeli_wierszy2018(Date1.Date, Date2.Date, (string)Session["id_dzialu"], 2, 19, 20, false, tenPlik);
                Session["tabelka002"] = tabelka01;
                tabela_1();
                tabela_3();
                tabela_4();
            }
            catch (Exception ex)
            {
                cm.log.Error(tenPlik + " " + ex.Message);
            }

            // dopasowanie opisów

            try
            {
                Label11.Visible = cl.debug(int.Parse((string)Session["id_dzialu"]));
                infoLabel2.Visible = cl.debug(int.Parse((string)Session["id_dzialu"]));
                infoLabel3.Visible = cl.debug(int.Parse((string)Session["id_dzialu"]));
                infoLabel5.Visible = cl.debug(int.Parse((string)Session["id_dzialu"]));
            }
            catch
            {
                Label11.Visible = false;
                infoLabel2.Visible = false;
                infoLabel3.Visible = false;
                infoLabel5.Visible = false;
            }

            Label3.Text = cl.nazwaSadu((string)Session["id_dzialu"]);
        }

        protected void tabela_1()
        {
            string idDzialu = (string)Session["id_dzialu"];
            if (cl.debug(int.Parse(idDzialu)))
            {
                cm.log.Info(tenPlik + ": rozpoczęcie tworzenia tabeli 1");
            }
            DataTable tabelka01 = dr.generuj_dane_do_tabeli_sedziowskiej_2019(int.Parse(idDzialu), 1, Date1.Date, Date2.Date, 28, tenPlik);
            Session["tabelka001"] = tabelka01;
            gwTabela1.DataSource = null;
            gwTabela1.DataSourceID = null;
            gwTabela1.DataSource = tabelka01;
            gwTabela1.DataBind();
        }

        protected void tabela_3()
        {
            string idDzialu = (string)Session["id_dzialu"];
            if (cl.debug(int.Parse(idDzialu)))
            {
                cm.log.Info(tenPlik + ": rozpoczęcie tworzenia tabeli 3");
            }
            DataTable tabelka01 = dr.generuj_dane_do_tabeli_sedziowskiej_2019(int.Parse(idDzialu), 3, Date1.Date, Date2.Date, 35, tenPlik);
            Session["tabelka003"] = tabelka01;
            gwTabela3.DataSource = null;
            gwTabela3.DataSourceID = null;
            gwTabela3.DataSource = tabelka01;
            gwTabela3.DataBind();
        }

        protected void tabela_4()
        {
            string idDzialu = (string)Session["id_dzialu"];
            if (cl.debug(int.Parse(idDzialu)))
            {
                cm.log.Info(tenPlik + ": rozpoczęcie tworzenia tabeli 4");
            }
            DataTable tabelka01 = dr.generuj_dane_do_tabeli_sedziowskiej_2019(int.Parse(idDzialu), 4, Date1.Date, Date2.Date, 23, tenPlik);
            Session["tabelka004"] = tabelka01;
            gwTabela4.DataSource = null;
            gwTabela4.DataSourceID = null;
            gwTabela4.DataSource = tabelka01;
            gwTabela4.DataBind();
        }

        #region "nagłowki tabel"

        private DataTable SchematTabeliNaglowka()
        {
            DataTable dT_01 = new DataTable();
            dT_01.Columns.Clear();
            dT_01.Columns.Add("Column1", typeof(string));
            dT_01.Columns.Add("Column2", typeof(string));
            dT_01.Columns.Add("Column3", typeof(string));
            dT_01.Columns.Add("Column4", typeof(string));
            dT_01.Columns.Add("Column5", typeof(string));
            dT_01.Columns.Add("Column6", typeof(string));
            return dT_01;
        }

        private DataTable NaglowekTabeli_1()
        {
            DataTable dT_02 = SchematTabeliNaglowka();
            dT_02.Clear();

            dT_02.Rows.Add(new Object[] { "1", "C", "1", "1", "h", "60" });//
            dT_02.Rows.Add(new Object[] { "1", "zaw", "1", "1", "h", "60" });//
            dT_02.Rows.Add(new Object[] { "1", "Ns", "1", "1", "h", "60" });//
            dT_02.Rows.Add(new Object[] { "1", "zaw", "1", "1", "h", "60" });//

            dT_02.Rows.Add(new Object[] { "1", "Nc", "1", "1", "h", "60" });//
            dT_02.Rows.Add(new Object[] { "1", "zaw", "1", "1", "h", "60" });//
            dT_02.Rows.Add(new Object[] { "1", "Co", "1", "1", "h", "60" });//
            dT_02.Rows.Add(new Object[] { "1", "zaw", "1", "1", "h", "60" });//
            dT_02.Rows.Add(new Object[] { "1", "Cps", "1", "1", "h", "60" });//
            dT_02.Rows.Add(new Object[] { "1", "zaw", "1", "1", "h", "60" });//
            dT_02.Rows.Add(new Object[] { "1", "Cgg", "1", "1", "h", "60" });//
            dT_02.Rows.Add(new Object[] { "1", "zaw", "1", "1", "h", "60" });//
            dT_02.Rows.Add(new Object[] { "1", "WSC", "1", "1", "h", "60" });//
            dT_02.Rows.Add(new Object[] { "1", "zaw", "1", "1", "h", "60" });//
            dT_02.Rows.Add(new Object[] { "1", "CZ", "1", "1", "h", "60" });//
            dT_02.Rows.Add(new Object[] { "1", "N", "1", "1", "h", "60" });//
            dT_02.Rows.Add(new Object[] { "1", "Razem", "1", "1", "h", "60" });//
            dT_02.Rows.Add(new Object[] { "1", "zaw", "1", "1", "h", "60" });//

            dT_02.Rows.Add(new Object[] { "2", "L.p.", "1", "2", "h", "10" });//
            dT_02.Rows.Add(new Object[] { "2", "Nazwisko i imię Sędziego", "1", "2", "h", "130" });//
            dT_02.Rows.Add(new Object[] { "2", "Ilość rozpraw", "1", "2", "h", "60" });//
            dT_02.Rows.Add(new Object[] { "2", "ZAŁATWIENIA ", "18", "1", "h", "45" });//

            return dT_02;
        }

        private DataTable NaglowekTabeli_3()
        {
            DataTable dT_03 = SchematTabeliNaglowka();
            dT_03.Clear();
            for (int i = 0; i < 10; i++)
            {
                dT_03.Rows.Add(new Object[] { "1", "Na rozprawach", "1", "1", "v" });
                dT_03.Rows.Add(new Object[] { "1", "Na posiedzeniach", "1", "1", "v" });
            }

            dT_03.Rows.Add(new Object[] { "2", "Rozpraw", "1", "2", "h" });
            dT_03.Rows.Add(new Object[] { "2", "Posiedzeń", "1", "2", "h" });

            dT_03.Rows.Add(new Object[] { "2", "Razem", "1", "2", "h" });
            dT_03.Rows.Add(new Object[] { "2", "Na rozprawach", "1", "2", "h" });
            dT_03.Rows.Add(new Object[] { "2", "Na posiedzeniach", "1", "2", "h" });

            dT_03.Rows.Add(new Object[] { "2", "c", "2", "1", "h" });
            dT_03.Rows.Add(new Object[] { "2", "Ns", "2", "1", "h" });
            dT_03.Rows.Add(new Object[] { "2", "Nc", "2", "1", "h" });
            dT_03.Rows.Add(new Object[] { "2", "Co", "2", "1", "h" });
            dT_03.Rows.Add(new Object[] { "2", "Cps", "2", "1", "h" });
            dT_03.Rows.Add(new Object[] { "2", "Cgg", "2", "1", "h" });
            dT_03.Rows.Add(new Object[] { "2", "WSC", "2", "1", "h" });
            dT_03.Rows.Add(new Object[] { "2", "Cz", "2", "1", "h" });
            dT_03.Rows.Add(new Object[] { "2", "N", "2", "1", "h" });

            dT_03.Rows.Add(new Object[] { "3", "L.p.", "1", "3", "h" });
            dT_03.Rows.Add(new Object[] { "3", "Nazwisko i imię", "1", "3", "h" });
            dT_03.Rows.Add(new Object[] { "3", "Pozostałość na początek okresu kontrolnego", "1", "3", "h" });
            dT_03.Rows.Add(new Object[] { "3", "Wpływ", "1", "3", "h" });
            dT_03.Rows.Add(new Object[] { "3", "Średni miesieczny wpływ ( w faktycznym czasie pracy)", "1", "3", "h" });
            dT_03.Rows.Add(new Object[] { "3", "Efektywny czas pracy", "1", "3", "h" });

            dT_03.Rows.Add(new Object[] { "3", "Ilość sesji", "3", "1", "h" });
            dT_03.Rows.Add(new Object[] { "3", "Ilość wyznaczonych spraw", "2", "1", "h" });
            dT_03.Rows.Add(new Object[] { "3", "Ilość spraw odrocznych z rozpraw (bez publikacji!!!)", "1", "3", "h" });
            dT_03.Rows.Add(new Object[] { "3", "Wskaźnik odroczeń", "1", "3", "h" });
            dT_03.Rows.Add(new Object[] { "3", "Załatwienia", "18", "1", "h" });

            dT_03.Rows.Add(new Object[] { "3", "Razem", "2", "2", "h" });

            dT_03.Rows.Add(new Object[] { "3", "Średnio miesięcznie załatwienia w efektywnym czasie pracy", "1", "3", "h" });

            dT_03.Rows.Add(new Object[] { "3", "Pozostałość na koniec okresu kontrolnego", "1", "3", "h" });
            dT_03.Rows.Add(new Object[] { "3", "Wielkrotna", "1", "3", "h" });

            return dT_03;
        }

        private DataTable NaglowekTabeli_4()
        {
            DataTable dT_04 = SchematTabeliNaglowka();
            dT_04.Clear();

            dT_04.Rows.Add(new Object[] { "1", "L.p.", "1", "1", "h" });
            dT_04.Rows.Add(new Object[] { "1", "Imię i nazwisko referendarza", "1", "1", "h" });
            dT_04.Rows.Add(new Object[] { "1", "Przydział orzeczniczy", "1", "1" });
            dT_04.Rows.Add(new Object[] { "1", "Liczba odbytych sesji", "1", "1" });
            dT_04.Rows.Add(new Object[] { "1", "Liczba odbytych posiedzeń", "1", "1", "h" });
            dT_04.Rows.Add(new Object[] { "1", "Razem", "1", "1" });
            return dT_04;
        }

        protected void GridView3_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                tb.makeHeader(NaglowekTabeli_3(), gwTabela3);
            }
        }

        protected void GridView5_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                tb.makeHeader(NaglowekTabeli_4(), gwTabela4);
            }
        }

        #endregion "nagłowki tabel"

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
                    tabela2Label.Text = "Sprawozdanie z ruchu spraw w za miesiąc " + strMonthName + " " + Date2.Date.Year.ToString() + " roku.";
                    Label17.Text = "Wydajność sędziów orzekających w Wydziale za miesiąc " + strMonthName + " " + Date2.Date.Year.ToString() + " roku.";
                    //Label2.Text= "Ewidencja spraw odroczonych  za miesiąc " + strMonthName + " " + Date2.Date.Year.ToString() + " roku.";
                    Label15.Text = "Referendarze - sesje w miesiącu " + strMonthName + " " + Date2.Date.Year.ToString() + " roku.";
                }
                else
                {
                    tabela2Label.Text = "Sprawozdanie z ruchu spraw w za okres od " + Date1.Text + " do  " + Date2.Text;
                    Label17.Text = "Wydajność sędziów orzekających w Wydziale za okres od" + Date1.Text + " do  " + Date2.Text;
                    //    Label2.Text = "Ewidencja spraw odroczonych za okres od " + Date1.Text + " do  " + Date2.Text;
                    Label15.Text = "Referendarze - sesje w okresie od " + Date1.Text + " do  " + Date2.Text;
                }
            }
            catch
            { }
        }

        protected void Button3_Click(object sender, EventArgs e)
        {
            string path = Server.MapPath("Template") + "\\oglc.xlsx";
            FileInfo existingFile = new FileInfo(path);
            string download = Server.MapPath("Template") + @"\oglc";

            FileInfo fNewFile = new FileInfo(download + "_.xlsx");

            using (ExcelPackage MyExcel = new ExcelPackage(existingFile))
            {
               

                int rowik = 0;

                ExcelWorksheet MyWorksheet1 = MyExcel.Workbook.Worksheets[1];

                DataTable table = (DataTable)Session["tabelka001"];
                rowik = table.Rows.Count - 2;
                MyWorksheet1 = tb.tworzArkuszwExcle(MyExcel.Workbook.Worksheets[1], table, 18, 0, 5, false, false, false, false, false, false);

                MyWorksheet1 = tb.tworzArkuszwExcleBezSedziow(MyExcel.Workbook.Worksheets[1], (DataTable)Session["tabelka002"], 14, 16, 2, rowik + 7, false);

                tb.komorkaExcela(MyExcel.Workbook.Worksheets[1], rowik + 7, 1, "Zaległość z poprzedniego miesiąca", true, 0, 1);
                tb.komorkaExcela(MyExcel.Workbook.Worksheets[1], rowik + 8, 1, "Wpływ", true, 0, 1);
                tb.komorkaExcela(MyExcel.Workbook.Worksheets[1], rowik + 9, 1, "Załatwienia", true, 0, 1);
                tb.komorkaExcela(MyExcel.Workbook.Worksheets[1], rowik + 10, 1, "Pozostało na następny miesiąc", true, 0, 1);
                tb.komorkaExcela(MyExcel.Workbook.Worksheets[1], rowik + 11, 1, "Zaległość", true, 7, 0);
                tb.komorkaExcela(MyExcel.Workbook.Worksheets[1], rowik + 11, 2, "Odroczono", false, 0, 0);
                tb.komorkaExcela(MyExcel.Workbook.Worksheets[1], rowik + 12, 2, " 0-3 miesiący", false, 0, 0);
                tb.komorkaExcela(MyExcel.Workbook.Worksheets[1], rowik + 13, 2, " 3-6 miesięcy", false, 0, 0);
                tb.komorkaExcela(MyExcel.Workbook.Worksheets[1], rowik + 14, 2, " 6-12 miesięcy", false, 0, 0);
                tb.komorkaExcela(MyExcel.Workbook.Worksheets[1], rowik + 15, 2, " 12-24 miesięcy (do 2 lat)", false, 0, 0);
                tb.komorkaExcela(MyExcel.Workbook.Worksheets[1], rowik + 16, 2, " 24-36 miesięcy (do 2-3 lat)", false, 0, 0);
                tb.komorkaExcela(MyExcel.Workbook.Worksheets[1], rowik + 17, 2, " 36-60 miesięcy (3-5 lat)", false, 0, 0);
                tb.komorkaExcela(MyExcel.Workbook.Worksheets[1], rowik + 18, 2, " Powyżej 60 miesięcy (powyżej 5 lat)", false, 0, 0);

              

                MyWorksheet1 = tb.tworzArkuszwExcle(MyExcel.Workbook.Worksheets[2], (DataTable)Session["tabelka003"], 33, 0, 7, false, true, false, false, false, false);
                MyWorksheet1 = tb.tworzArkuszwExcle(MyExcel.Workbook.Worksheets[3], (DataTable)Session["tabelka004"], 5, 0, 6, true, true, false, false, false, false);

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

        protected void GridView1_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                tb.makeHeader(NaglowekTabeli_1(), gwTabela1);
            }
            else
            {
                if ((storid > 0) && (DataBinder.Eval(e.Row.DataItem, "id") == null))
                {
                    rowIndex = 0;
                    AddNewRow(sender, e);
                }
            }
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                storid = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "id").ToString());
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                //  tb.makeSumRow((DataTable)Session["tabelka001"], e,1);
            }
        }

        protected void GridView3_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                tb.makeSumRow((DataTable)Session["tabelka003"], e, 1);
            }
        }

        protected void GridView5_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                tb.makeSumRow((DataTable)Session["tabelka004"], e, 1);
            }
        }

        // podtabela

        public void AddNewRow(object sender, GridViewRowEventArgs e)
        {
            DataTable tabelka01 = (DataTable)Session["tabelka002"];

            GridView GridView1 = (GridView)sender;
            GridViewRow NewTotalRow = new GridViewRow(0, 0, DataControlRowType.DataRow, DataControlRowState.Insert);

            string idtabeli = "2";

            // nowy wiersz
            int idWiersza = 1;

            GridView1.Controls[0].Controls.AddAt(e.Row.RowIndex + rowIndex, tb.wierszTabeli(tabelka01, 19 , idWiersza, idtabeli, "Zaległość z poprzedniego miesiąca", 3, 1, "normal", "borderTopLeft "));

            idWiersza = 2;
            GridView1.Controls[0].Controls.AddAt(e.Row.RowIndex + rowIndex, tb.wierszTabeli(tabelka01, 19 , idWiersza, idtabeli, "Wpływ", 3, 1, "normal", "borderTopLeft "));

            // nowy wiersz
            idWiersza = 3;
            GridView1.Controls[0].Controls.AddAt(e.Row.RowIndex + rowIndex, tb.wierszTabeli(tabelka01, 19 , idWiersza, idtabeli, "Załatwienia", 3, 1, "normal", "borderTopLeft "));

            // nowy wiersz
            idWiersza = 4;
            GridView1.Controls[0].Controls.AddAt(e.Row.RowIndex + rowIndex, tb.wierszTabeli(tabelka01, 19 , idWiersza, idtabeli, "Pozostałość na następny miesiąc", 3, 1, "normal", "borderTopLeft "));

            // nowy wiersz
            idWiersza = 5;
            GridView1.Controls[0].Controls.AddAt(e.Row.RowIndex + rowIndex, tb.wierszTabeli(tabelka01, 19 , idWiersza, idtabeli, "Odroczono", 1, 1, "normal", "borderTopLeft col_60", "Zaległość", 8, 2, "borderTopLeft col_60"));

            // nowy wiersz
            idWiersza = 6;
            GridView1.Controls[0].Controls.AddAt(e.Row.RowIndex + rowIndex, tb.wierszTabeli(tabelka01, 19 , idWiersza, idtabeli, "0-3  miesięcy ", 1, 1, "normal", "borderTopLeft "));

            // nowy wiersz
            idWiersza = 7;
            GridView1.Controls[0].Controls.AddAt(e.Row.RowIndex + rowIndex, tb.wierszTabeli(tabelka01, 19 , idWiersza, idtabeli, "3-6  miesięcy ", 1, 1, "normal", "borderTopLeft "));

            // nowy wiersz
            idWiersza = 8;
            GridView1.Controls[0].Controls.AddAt(e.Row.RowIndex + rowIndex, tb.wierszTabeli(tabelka01, 19 , idWiersza, idtabeli, "6-12  miesięcy ", 1, 1, "normal", "borderTopLeft "));

            // nowy wiersz
            idWiersza = 9;
            GridView1.Controls[0].Controls.AddAt(e.Row.RowIndex + rowIndex, tb.wierszTabeli(tabelka01, 19 , idWiersza, idtabeli, "12-24  miesięcy (do 2 lat)", 1, 1, "normal", "borderTopLeft "));

            idWiersza = 10;
            GridView1.Controls[0].Controls.AddAt(e.Row.RowIndex + rowIndex, tb.wierszTabeli(tabelka01, 19 , idWiersza, idtabeli, "24-36  miesięcy (2-3 lat))", 1, 1, "normal", "borderTopLeft "));

            idWiersza = 11;
            GridView1.Controls[0].Controls.AddAt(e.Row.RowIndex + rowIndex, tb.wierszTabeli(tabelka01, 19 , idWiersza, idtabeli, "36-60  miesięcy (3-5 lat)", 1, 1, "normal", "borderTopLeft "));

            idWiersza = 12;
            GridView1.Controls[0].Controls.AddAt(e.Row.RowIndex + rowIndex, tb.wierszTabeli(tabelka01, 19 , idWiersza, idtabeli, "Powyżej 60 miesięcy (powyżej 5 lat)", 1, 1, "normal", "borderTopLeft "));
        }
    }
}