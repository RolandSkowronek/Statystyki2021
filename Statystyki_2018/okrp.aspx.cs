using OfficeOpenXml;
using System;
using System.Data;
using System.Globalization;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Statystyki_2018
{
    public partial class okrp : System.Web.UI.Page
    {
        public Class1 cl = new Class1();
        public common cm = new common();
        public tabele tb = new tabele();
        public dataReaders dr = new dataReaders();
        private const string tenPlik = "okrp.aspx";

        private int storid = 0;
        private int rowIndex = 1;

        protected void Page_Load(object sender, EventArgs e)
        {
             string idWydzial = Request.QueryString["w"]; Session["czesc"] = cm.nazwaFormularza(tenPlik, idWydzial) ;
            try
            {
                if (idWydzial == null)
                {
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
                        clearHedersSession();
                        makeHeader();
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

        protected void clearHedersSession()
        {
            Session["header_01"] = null;
            Session["header_02"] = null;
            Session["header_03"] = null;
            Session["header_04"] = null;
            Session["header_05"] = null;
            Session["header_06"] = null;
            Session["header_07"] = null;
            Session["header_08"] = null;
        }

        protected void odswiez()
        {
            string idDzialu = (string)Session["id_dzialu"];
            string txt = string.Empty; 
            cl.deleteRowTable();

            try
            {
                DataTable tabelka02 = dr.generuj_dane_do_tabeli_wierszy2018(Date1.Date, Date2.Date, (string)Session["id_dzialu"], 2, 11, 14, tenPlik);
                Session["tabelka002"] = tabelka02;

                //tabela 1
                try
                {
                    cm.log.Info(tenPlik + ": rozpoczęcie tworzenia tabeli 1");

                    DataTable Tabela1 = dr.generuj_dane_do_tabeli_sedziowskiej_2019(int.Parse(idDzialu), 1, Date1.Date, Date2.Date, 25, tenPlik);
                    Session["tabelka001"] = Tabela1;
                    Gridview1.DataSource = null;
                    Gridview1.DataSourceID = null;
                    Gridview1.DataSource = Tabela1;
                    Gridview1.DataBind();
                }
                catch (Exception ex)
                {
                    cm.log.Error(tenPlik + " tabela 1  " + ex.Message);
                }

                try
                {
                    cm.log.Info(tenPlik + ": rozpoczęcie tworzenia tabeli 3");

                    DataTable Tabela1 = dr.generuj_dane_do_tabeli_sedziowskiej_2019(int.Parse(idDzialu), 3, Date1.Date, Date2.Date, 18, tenPlik);
                    Session["tabelka003"] = Tabela1;
                    Gridview2.DataSource = null;
                    Gridview2.DataSourceID = null;
                    Gridview2.DataSource = Tabela1;
                    Gridview2.DataBind();
                }
                catch (Exception ex)
                {
                    cm.log.Error(tenPlik + " tabela 3 " + ex.Message);
                }
            }
            catch (Exception ex)
            {
                cm.log.Error(tenPlik + ex.Message);
            }

            // dopasowanie opisów
            makeLabels();

            try
            {
                Label11.Visible = cl.debug(int.Parse(idDzialu));
                infoLabel2.Visible = cl.debug(int.Parse(idDzialu));
                infoLabel3.Visible = cl.debug(int.Parse(idDzialu));
            }
            catch
            {
                Label11.Visible = false;
                infoLabel2.Visible = false;
                infoLabel3.Visible = false;
            }

            Label11.Text = txt;
            Label3.Text = cl.nazwaSadu((string)Session["id_dzialu"]);
        }

        #region "nagłowki tabel"

        protected void makeHeader()
        {
            #region tabela  1 (wierszowa)

            DataTable dT_01 = new DataTable();
            dT_01.Columns.Clear();
            dT_01.Columns.Add("Column1", typeof(string));
            dT_01.Columns.Add("Column2", typeof(string));
            dT_01.Columns.Add("Column3", typeof(string));
            dT_01.Columns.Add("Column4", typeof(string));
            dT_01.Columns.Add("Column5", typeof(string));
            dT_01.Columns.Add("Column6", typeof(string));

            DataTable dT_02 = new DataTable();
            dT_02.Columns.Clear();
            dT_02.Columns.Add("Column1", typeof(string));
            dT_02.Columns.Add("Column2", typeof(string));
            dT_02.Columns.Add("Column3", typeof(string));
            dT_02.Columns.Add("Column4", typeof(string));
            dT_02.Columns.Add("Column5", typeof(string));
            dT_02.Columns.Add("Column6", typeof(string));
            DataTable dT_03 = new DataTable();
            dT_03.Columns.Clear();
            dT_03.Columns.Add("Column1", typeof(string));
            dT_03.Columns.Add("Column2", typeof(string));
            dT_03.Columns.Add("Column3", typeof(string));
            dT_03.Columns.Add("Column4", typeof(string));
            dT_03.Columns.Add("Column5", typeof(string));
            dT_03.Columns.Add("Column6", typeof(string));
            DataTable dT_04 = new DataTable();
            dT_04.Columns.Clear();
            dT_04.Columns.Add("Column1", typeof(string));
            dT_04.Columns.Add("Column2", typeof(string));
            dT_04.Columns.Add("Column3", typeof(string));
            dT_04.Columns.Add("Column4", typeof(string));
            dT_04.Columns.Add("Column5", typeof(string));
            dT_04.Columns.Add("Column6", typeof(string));

            DataTable dT_05 = new DataTable();
            dT_05.Columns.Clear();
            dT_05.Columns.Add("Column1", typeof(string));
            dT_05.Columns.Add("Column2", typeof(string));
            dT_05.Columns.Add("Column3", typeof(string));
            dT_05.Columns.Add("Column4", typeof(string));
            dT_05.Columns.Add("Column5", typeof(string));
            dT_05.Columns.Add("Column6", typeof(string));

            dT_01.Clear();
            dT_01.Rows.Add(new Object[] { "1", "", "1", "1" });
            dT_01.Rows.Add(new Object[] { "1", "w tm wyroki", "1", "1" });
            dT_01.Rows.Add(new Object[] { "1", "", "1", "1" });
            dT_01.Rows.Add(new Object[] { "1", "w tm wyroki", "1", "1" });

            dT_01.Rows.Add(new Object[] { "2", "Urlopy", "1", "2" });
            dT_01.Rows.Add(new Object[] { "2", "Zwolnienia", "1", "2" });
            dT_01.Rows.Add(new Object[] { "2", "Razem", "1", "2" });
            dT_01.Rows.Add(new Object[] { "2", "W terminie ustawowym.", "1", "2" });
            dT_01.Rows.Add(new Object[] { "2", "Po terminie ustawowym", "1", "2" });
            dT_01.Rows.Add(new Object[] { "2", "W tym nieuspra-wiedliwione", "1", "2" });

            dT_01.Rows.Add(new Object[] { "2", "Razem", "1", "2" });
            dT_01.Rows.Add(new Object[] { "2", "W terminie ustawowym.", "1", "2" });
            dT_01.Rows.Add(new Object[] { "2", "Po terminie ustawowym", "1", "2" });
            dT_01.Rows.Add(new Object[] { "2", "W tym nieuspra-wiedliwione", "1", "2" });

            dT_01.Rows.Add(new Object[] { "2", "P", "2", "1" });
            dT_01.Rows.Add(new Object[] { "2", "Np", "1", "2" });
            dT_01.Rows.Add(new Object[] { "2", "Po ", "1", "2" });
            dT_01.Rows.Add(new Object[] { "2", "U", "2", "1" });
            dT_01.Rows.Add(new Object[] { "2", "Uo", "1", "2" });
            dT_01.Rows.Add(new Object[] { "2", "Pz", "1", "2" });
            dT_01.Rows.Add(new Object[] { "2", "Uz", "1", "2" });
            dT_01.Rows.Add(new Object[] { "2", "WSC", "1", "2" });
            dT_01.Rows.Add(new Object[] { "2", "Razem", "1", "2" });

            dT_01.Rows.Add(new Object[] { "3", "L.p.", "1", "3" });

            dT_01.Rows.Add(new Object[] { "3", "Funkcja", "1", "3" });
            dT_01.Rows.Add(new Object[] { "3", "Stanowisko", "1", "3" });
            dT_01.Rows.Add(new Object[] { "3", "Imie nazwisko", "1", "3" });
            dT_01.Rows.Add(new Object[] { "3", "Dni rozprawy", "1", "3" });
            dT_01.Rows.Add(new Object[] { "3", "Etat", "1", "3" });

            dT_01.Rows.Add(new Object[] { "3", "Nieobecności", "2", "1" });
            dT_01.Rows.Add(new Object[] { "3", "Terminowość sporządzonych uzasadnień", "4", "1" });
            dT_01.Rows.Add(new Object[] { "3", "Uzasadnienia wygłoszone", "1", "3" });
            dT_01.Rows.Add(new Object[] { "3", "Terminowość sporządzonych uzasadnień orzeczeń merytorycznych C i Ns", "4", "1" });
            dT_01.Rows.Add(new Object[] { "3", "Uzasadnienia wygłoszone", "1", "3" });

            dT_01.Rows.Add(new Object[] { "3", "ZAŁATWIENIA", "11", "1" });
            dT_01.Rows.Add(new Object[] { "3", "Uwagi", "1", "3" });

            Session["header_01"] = dT_01;

            #endregion tabela  1 (wierszowa)

            #region tabela  2 ()

            dT_02.Clear();

            dT_02.Rows.Add(new Object[] { "1", "Liczba", "1", "1", "h", "60" });//
            dT_02.Rows.Add(new Object[] { "1", "%", "1", "1", "h", "60" });//

            dT_02.Rows.Add(new Object[] { "2", "odroczonych (liczba % w stosunku do wyznaczonych", "2", "1", "h" });
            dT_02.Rows.Add(new Object[] { "2", "wyrok", "1", "2", "h" });
            dT_02.Rows.Add(new Object[] { "2", "wyrok zaoczny", "1", "2", "h" });
            dT_02.Rows.Add(new Object[] { "2", "zaw.", "1", "2", "h" });
            dT_02.Rows.Add(new Object[] { "2", "inne post.", "1", "2", "h" });
            dT_02.Rows.Add(new Object[] { "2", "inne", "1", "2", "h" });

            dT_02.Rows.Add(new Object[] { "3", "Liczba wyznaczonych spraw", "1", "3", "h" });
            dT_02.Rows.Add(new Object[] { "3", "załatwienia wyznaczeń", "7", "1", "h" });
            dT_02.Rows.Add(new Object[] { "3", "Liczba załatwień w bieżącym miesiącu", "1", "3", "h" });

            dT_02.Rows.Add(new Object[] { "4", "L.p.", "1", "4", "h", "60" });//
            dT_02.Rows.Add(new Object[] { "4", "Sędzia ", "1", "4", "h", "60" });
            dT_02.Rows.Add(new Object[] { "4", DateTime.Now.Month.ToString("D2") + "-" + DateTime.Now.Year.ToString(), "9", "1", "h", "60" });

            Session["header_02"] = dT_02;

            #endregion tabela  2 ()

            #region tabela  3 ()

            dT_03.Clear();

            for (int i = 0; i < 7; i++)
            {
                dT_03.Rows.Add(new Object[] { "1", "wpływ", "1", "1", "v" });
                dT_03.Rows.Add(new Object[] { "1", "pozostałość", "1", "1", "v" });
            }

            dT_03.Rows.Add(new Object[] { "2", "L.p.", "1", "2", "h" });
            dT_03.Rows.Add(new Object[] { "2", "Sędzia /referendarz", "1", "2", "h" });

            dT_03.Rows.Add(new Object[] { "2", "C", "2", "1", "h" });
            dT_03.Rows.Add(new Object[] { "2", "Cgg", "2", "1", "h" });
            dT_03.Rows.Add(new Object[] { "2", "Ns", "2", "1", "h" });
            dT_03.Rows.Add(new Object[] { "2", "Co", "2", "1", "h" });
            dT_03.Rows.Add(new Object[] { "2", "Nc", "2", "1", "h" });
            dT_03.Rows.Add(new Object[] { "2", "Cps", "2", "1", "h" });
            dT_03.Rows.Add(new Object[] { "2", "Wsc", "2", "1", "h" });
            dT_03.Rows.Add(new Object[] { "2", "Razem", "1", "2", "h" });

            dT_03.Rows.Add(new Object[] { "3", "Wielkość i stan referatów ( przy czym wielkość odpowiada liczbie spraw, które wpłyneły do referatu w okresie objętym notatką, a stan liczbę pozostałości).", "18", "1", "h" });

            Session["header_03"] = dT_03;

            #endregion tabela  3 ()
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
                    lbTabela2Title.Text = "Wydajność sędziów orzekających w Wydziale za miesiąc " + strMonthName + " " + Date2.Date.Year.ToString() + " roku.";
                    //Label2.Text= "Ewidencja spraw odroczonych  za miesiąc " + strMonthName + " " + Date2.Date.Year.ToString() + " roku.";
                }
                else
                {
                    tabela2Label.Text = "Sprawozdanie z ruchu spraw w za okres od " + Date1.Text + " do  " + Date2.Text;
                    lbTabela2Title.Text = "Wydajność sędziów orzekających w Wydziale za okres od" + Date1.Text + " do  " + Date2.Text;
                    //    Label2.Text = "Ewidencja spraw odroczonych za okres od " + Date1.Text + " do  " + Date2.Text;
                }
            }
            catch
            {
            }
        }

        protected void Button3_Click(object sender, EventArgs e)
        {
            FileInfo tenPlikExcela = new FileInfo(tenPlik);
            string path = Server.MapPath("Template") + "\\" + Path.GetFileNameWithoutExtension(tenPlikExcela.Name) + ".xlsx";
            FileInfo existingFile = new FileInfo(path);

            string download = Server.MapPath("Template") + @"\" + Path.GetFileNameWithoutExtension(tenPlikExcela.Name);

            FileInfo fNewFile = new FileInfo(download + "_.xlsx");

            // pierwsza tabelka

            using (ExcelPackage MyExcel = new ExcelPackage(existingFile))
            {
                // pierwsza

                ExcelWorksheet MyWorksheet1 = MyExcel.Workbook.Worksheets[1];
                DataTable table = (DataTable)Session["tabelka001"];

                MyWorksheet1 = tb.tworzArkuszwExcle(MyExcel.Workbook.Worksheets[1], table, 29, 0, 4, true, false, true, true, true);

                int rowik = table.Rows.Count;

                tb.komorkaExcela(MyWorksheet1, rowik + 4, 1, "Razem", true, 10, 1);
                tb.komorkaExcela(MyWorksheet1, rowik + 4, 3, "Zaległość z poprzedniego miesiąca", true, 0, 16);
                tb.komorkaExcela(MyWorksheet1, rowik + 5, 3, "Wpływ", true, 0, 16);
                tb.komorkaExcela(MyWorksheet1, rowik + 6, 3, "Załatwienia", true, 0, 16);
                tb.komorkaExcela(MyWorksheet1, rowik + 7, 3, "Pozostało na następny miesiąc", true, 0, 16);
                tb.komorkaExcela(MyWorksheet1, rowik + 8, 3, "powyżej 3-6 miesięcy ", true, 0, 16);
                tb.komorkaExcela(MyWorksheet1, rowik + 9, 3, "powyżej 6-12 miesięcy", true, 0, 16);
                tb.komorkaExcela(MyWorksheet1, rowik + 10, 3, "Ponad 12 miesięcy ", true, 0, 16);
                tb.komorkaExcela(MyWorksheet1, rowik + 11, 3, "z tego", true, 3, 1);
                tb.komorkaExcela(MyWorksheet1, rowik + 11, 5, "powyżej 12 miesięcy 2 lata ", true, 0, 14);
                tb.komorkaExcela(MyWorksheet1, rowik + 12, 5, "ponad 2 lata", true, 0, 14);
                tb.komorkaExcela(MyWorksheet1, rowik + 13, 5, "3 do 5 lat", true, 0, 14);
                tb.komorkaExcela(MyWorksheet1, rowik + 14, 5, "Ponad 5 lat", true, 0, 14);

                DataTable tabelka001 = (DataTable)Session["tabelka002"];
                try
                {
                    foreach (DataRow dR in tabelka001.Rows)
                    {
                        tb.komorkaExcela(MyWorksheet1, rowik + 4,  20, dR[0].ToString().Trim(), true, 0, 1);
                        tb.komorkaExcela(MyWorksheet1, rowik + 4, 22, dR[2].ToString().Trim(), true, 0, 0);
                        tb.komorkaExcela(MyWorksheet1, rowik + 4, 23, dR[3].ToString().Trim(), true, 0, 0);
                        tb.komorkaExcela(MyWorksheet1, rowik + 4, 24, dR[4].ToString().Trim(), true, 0, 1);
                        for (int i = 7; i < 13; i++)
                        {
                            try
                            {
                                tb.komorkaExcela(MyWorksheet1, rowik + 4, i + 18, dR[i - 2].ToString().Trim(), true, 0, 0);
                            }
                            catch
                            { }
                        }
                        
                        rowik++;
                    }
                }
                catch (Exception ex)
                {
                    cm.log.Error(tenPlik + " generowanie podtabeli " + ex.Message);
                }

                ExcelWorksheet MyWorksheet2 = MyExcel.Workbook.Worksheets[2];

                DataTable table2 = (DataTable)Session["tabelka003"];

                MyWorksheet1 = tb.tworzArkuszwExcle(MyExcel.Workbook.Worksheets[2], table2, 10, 0, 5, true, false, false, false, false);

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

        protected void LinkButton55_Click(object sender, EventArgs e)
        {
            makeLabels();
            odswiez();
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "print2", "JavaScript: window.print();", true);
            makeLabels();
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                DataTable table = ((DataView)tabela_1.Select(DataSourceSelectArguments.Empty)).ToTable();
                tb.makeSumRow(table, e);
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                storid = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "id").ToString());
            }
        }

        ///
        private GridViewRow wierszTabeli0(int idWiersza, string idtabeli, string tekst)
        {
            // nowy wiersz

            DataTable tabelka01 = (DataTable)Session["tabelka002"];
            GridViewRow NewTotalRow = new GridViewRow(0, 0, DataControlRowType.DataRow, DataControlRowState.Insert);
            NewTotalRow.Cells.Add(tb.cela("Razem", 11, 2, "borderTopLeft "));
            NewTotalRow.Cells.Add(tb.cela(tekst, 1, 16, "borderTopLeft "));

            NewTotalRow.Cells.Add(tb.cela("<a class='normal' href=\"javascript: openPopup('popup.aspx?sesja=" + (idWiersza + 1).ToString().Trim() + "!2!1!3')\">" + tabelka01.Rows[idWiersza][0].ToString().Trim() + "</a>", 1, 2, "borderTopLeft"));
            for (int i = 3; i < 5; i++)
            {
                NewTotalRow.Cells.Add(tb.cela("<a class='normal' href=\"javascript: openPopup('popup.aspx?sesja=" + (idWiersza + 1).ToString().Trim() + "!2!" + i.ToString() + "!5')\">" + tabelka01.Rows[idWiersza][i - 1].ToString().Trim() + "</a>", 1, 1, "borderTopLeft"));
            }
            NewTotalRow.Cells.Add(tb.cela("<a class='normal' href=\"javascript: openPopup('popup.aspx?sesja=" + (idWiersza + 1).ToString().Trim() + "!2!5!3')\">" + tabelka01.Rows[idWiersza][4].ToString().Trim() + "</a>", 1, 2, "borderTopLeft"));
            for (int i = 7; i < 11; i++)
            {
                NewTotalRow.Cells.Add(tb.cela("<a class='normal' href=\"javascript: openPopup('popup.aspx?sesja=" + (idWiersza + 1).ToString().Trim() + "!2!" + i.ToString() + "!5')\">" + tabelka01.Rows[idWiersza][i - 1].ToString().Trim() + "</a>", 1, 1, "borderTopLeft"));
            }
            NewTotalRow.Cells.Add(tb.cela("<a class='normal ' href=\"javascript: openPopup('popup.aspx?sesja=" + (idWiersza + 1).ToString().Trim() + "!2!11!5')\">" + tabelka01.Rows[idWiersza][10].ToString().Trim() + "</a>", 1, 1, "borderTopLeft", true));
            NewTotalRow.Cells.Add(tb.cela("<input id = \"Text1\" type = \"text\" />", 1, 1, "borderTopLeft"));
            return NewTotalRow;
        }

        private GridViewRow wierszTabeli1(int idWiersza, string idtabeli, string tekst)
        {
            // nowy wiersz

            DataTable tabelka01 = (DataTable)Session["tabelka002"];
            GridViewRow NewTotalRow = new GridViewRow(0, 0, DataControlRowType.DataRow, DataControlRowState.Insert);
            NewTotalRow.Cells.Add(tb.cela(tekst, 1, 16, "borderTopLeft "));

            NewTotalRow.Cells.Add(tb.cela("<a class='normal' href=\"javascript: openPopup('popup.aspx?sesja=" + (idWiersza + 1).ToString().Trim() + "!2!1!3')\">" + tabelka01.Rows[idWiersza][0].ToString().Trim() + "</a>", 1, 2, "borderTopLeft"));
            for (int i = 3; i < 5; i++)
            {
                NewTotalRow.Cells.Add(tb.cela("<a class='normal' href=\"javascript: openPopup('popup.aspx?sesja=" + (idWiersza + 1).ToString().Trim() + "!2!" + i.ToString() + "!5')\">" + tabelka01.Rows[idWiersza][i - 1].ToString().Trim() + "</a>", 1, 1, "borderTopLeft"));
            }
            NewTotalRow.Cells.Add(tb.cela("<a class='normal' href=\"javascript: openPopup('popup.aspx?sesja=" + (idWiersza + 1).ToString().Trim() + "!2!5!3')\">" + tabelka01.Rows[idWiersza][4].ToString().Trim() + "</a>", 1, 2, "borderTopLeft"));
            for (int i = 7; i < 11; i++)
            {
                NewTotalRow.Cells.Add(tb.cela("<a class='normal' href=\"javascript: openPopup('popup.aspx?sesja=" + (idWiersza + 1).ToString().Trim() + "!2!" + i.ToString() + "!5')\">" + tabelka01.Rows[idWiersza][i - 1].ToString().Trim() + "</a>", 1, 1, "borderTopLeft"));
            }
            NewTotalRow.Cells.Add(tb.cela("<a class='normal ' href=\"javascript: openPopup('popup.aspx?sesja=" + (idWiersza + 1).ToString().Trim() + "!2!11!5')\">" + tabelka01.Rows[idWiersza][10].ToString().Trim() + "</a>", 1, 1, "borderTopLeft", true));
            NewTotalRow.Cells.Add(tb.cela("<input id = \"Text1\" type = \"text\" />", 1, 1, "borderTopLeft"));
            return NewTotalRow;
        }

        private GridViewRow wierszTabeli2(int idWiersza, string idtabeli, string tekst)
        {
            // nowy wiersz

            DataTable tabelka01 = (DataTable)Session["tabelka002"];
            GridViewRow NewTotalRow = new GridViewRow(0, 0, DataControlRowType.DataRow, DataControlRowState.Insert);
            NewTotalRow.Cells.Add(tb.cela("z tego ", 4, 2, "borderTopLeft "));
            NewTotalRow.Cells.Add(tb.cela(tekst, 1, 14, "borderTopLeft "));
            NewTotalRow.Cells.Add(tb.cela("<a class='normal' href=\"javascript: openPopup('popup.aspx?sesja=" + (idWiersza + 1).ToString().Trim() + "!2!1!3')\">" + tabelka01.Rows[idWiersza][0].ToString().Trim() + "</a>", 1, 2, "borderTopLeft"));
            for (int i = 3; i < 5; i++)
            {
                NewTotalRow.Cells.Add(tb.cela("<a class='normal' href=\"javascript: openPopup('popup.aspx?sesja=" + (idWiersza + 1).ToString().Trim() + "!2!" + i.ToString() + "!5')\">" + tabelka01.Rows[idWiersza][i - 1].ToString().Trim() + "</a>", 1, 1, "borderTopLeft"));
            }
            NewTotalRow.Cells.Add(tb.cela("<a class='normal' href=\"javascript: openPopup('popup.aspx?sesja=" + (idWiersza + 1).ToString().Trim() + "!2!5!3')\">" + tabelka01.Rows[idWiersza][4].ToString().Trim() + "</a>", 1, 2, "borderTopLeft"));
            for (int i = 7; i < 11; i++)
            {
                NewTotalRow.Cells.Add(tb.cela("<a class='normal' href=\"javascript: openPopup('popup.aspx?sesja=" + (idWiersza + 1).ToString().Trim() + "!2!" + i.ToString() + "!5')\">" + tabelka01.Rows[idWiersza][i - 1].ToString().Trim() + "</a>", 1, 1, "borderTopLeft"));
            }
            NewTotalRow.Cells.Add(tb.cela("<a class='normal ' href=\"javascript: openPopup('popup.aspx?sesja=" + (idWiersza + 1).ToString().Trim() + "!2!11!5')\">" + tabelka01.Rows[idWiersza][10].ToString().Trim() + "</a>", 1, 1, "borderTopLeft", true));
            NewTotalRow.Cells.Add(tb.cela("<input id = \"Text1\" type = \"text\" />", 1, 1, "borderTopLeft"));

            return NewTotalRow;
        }

        private GridViewRow wierszTabeli3(int idWiersza, string idtabeli, string tekst)
        {
            // nowy wiersz
            GridViewRow NewTotalRow = new GridViewRow(0, 0, DataControlRowType.DataRow, DataControlRowState.Insert);

            try
            {
                DataTable tabelka01 = (DataTable)Session["tabelka002"];
                NewTotalRow.Cells.Add(tb.cela(tekst, 1, 14, "borderTopLeft  "));

                NewTotalRow.Cells.Add(tb.cela("<a class='normal' href=\"javascript: openPopup('popup.aspx?sesja=" + (idWiersza + 1).ToString().Trim() + "!2!1!3')\">" + tabelka01.Rows[idWiersza][0].ToString().Trim() + "</a>", 1, 2, "borderTopLeft"));
                for (int i = 3; i < 5; i++)
                {
                    NewTotalRow.Cells.Add(tb.cela("<a class='normal' href=\"javascript: openPopup('popup.aspx?sesja=" + (idWiersza + 1).ToString().Trim() + "!2!" + i.ToString() + "!5')\">" + tabelka01.Rows[idWiersza][i - 1].ToString().Trim() + "</a>", 1, 1, "borderTopLeft"));
                }
                NewTotalRow.Cells.Add(tb.cela("<a class='normal' href=\"javascript: openPopup('popup.aspx?sesja=" + (idWiersza + 1).ToString().Trim() + "!2!5!3')\">" + tabelka01.Rows[idWiersza][4].ToString().Trim() + "</a>", 1, 2, "borderTopLeft"));
                for (int i = 7; i < 11; i++)
                {
                    NewTotalRow.Cells.Add(tb.cela("<a class='normal' href=\"javascript: openPopup('popup.aspx?sesja=" + (idWiersza + 1).ToString().Trim() + "!2!" + i.ToString() + "!5')\">" + tabelka01.Rows[idWiersza][i - 1].ToString().Trim() + "</a>", 1, 1, "borderTopLeft"));
                }
                NewTotalRow.Cells.Add(tb.cela("<a class='normal ' href=\"javascript: openPopup('popup.aspx?sesja=" + (idWiersza + 1).ToString().Trim() + "!2!11!5')\">" + tabelka01.Rows[idWiersza][10].ToString().Trim() + "</a>", 1, 1, "borderTopLeft",true));
                NewTotalRow.Cells.Add(tb.cela("<input id = \"Text1\" type = \"text\" />", 1, 1, "borderTopLeft"));
            }
            catch (Exception ex)
            {
                cm.log.Error(tenPlik + "  wierszTabeli3: " + ex.Message);
            }

            return NewTotalRow;
        }

        public void AddNewRow(object sender, GridViewRowEventArgs e)
        {
            GridView GridView1 = (GridView)sender;
            GridViewRow NewTotalRow = new GridViewRow(0, 0, DataControlRowType.DataRow, DataControlRowState.Insert);

            string idtabeli = "2";

            // nowy wiersz
            int idWiersza = 0;
            GridView1.Controls[0].Controls.AddAt(e.Row.RowIndex + rowIndex, wierszTabeli0(idWiersza, idtabeli, "Zaległość z poprzedniego miesiąca"));

            idWiersza = 1;
            GridView1.Controls[0].Controls.AddAt(e.Row.RowIndex + rowIndex, wierszTabeli1(idWiersza, idtabeli, "Wpływ"));

            // nowy wiersz
            idWiersza = 2;
            GridView1.Controls[0].Controls.AddAt(e.Row.RowIndex + rowIndex, wierszTabeli1(idWiersza, idtabeli, "Załatwienia"));

            // nowy wiersz
            idWiersza = 3;
            GridView1.Controls[0].Controls.AddAt(e.Row.RowIndex + rowIndex, wierszTabeli1(idWiersza, idtabeli, "Pozostałość na następny miesiąc"));

            // nowy wiersz
            idWiersza = 4;
            GridView1.Controls[0].Controls.AddAt(e.Row.RowIndex + rowIndex, wierszTabeli1(idWiersza, idtabeli, "powyżej 3-6 miesięcy "));

            // nowy wiersz
            idWiersza = 5;
            GridView1.Controls[0].Controls.AddAt(e.Row.RowIndex + rowIndex, wierszTabeli1(idWiersza, idtabeli, "powyżej 6-12 miesięcy "));

            // nowy wiersz
            idWiersza = 6;
            GridView1.Controls[0].Controls.AddAt(e.Row.RowIndex + rowIndex, wierszTabeli1(idWiersza, idtabeli, "Ponad 12 miesięcy "));
            // z tego
            idWiersza = 7;

            GridView1.Controls[0].Controls.AddAt(e.Row.RowIndex + rowIndex, wierszTabeli2(idWiersza, idtabeli, "powyżej 12 miesięcy 2 lata "));
            idWiersza = 8;

            GridView1.Controls[0].Controls.AddAt(e.Row.RowIndex + rowIndex, wierszTabeli3(idWiersza, idtabeli, "ponad 2 lata"));
            idWiersza = 9;

            GridView1.Controls[0].Controls.AddAt(e.Row.RowIndex + rowIndex, wierszTabeli3(idWiersza, idtabeli, "3 do 5 lat"));

            idWiersza = 10;
            GridView1.Controls[0].Controls.AddAt(e.Row.RowIndex + rowIndex, wierszTabeli3(idWiersza, idtabeli, "Ponad  5 lat"));
        }

        protected void Gridview2_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                System.Web.UI.WebControls.GridView sn = new System.Web.UI.WebControls.GridView();
                DataTable dT = (DataTable)Session["header_02"];
                tb.makeHeader(sn, dT, Gridview2);
            }
        }

        protected void Gridview2_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                tb.makeSumRow((DataTable)Session["tabelka003"], e, 1);
            }
        }

        protected void Gridview1_RowCreated1(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                DataTable dT = (DataTable)Session["header_01"];
                tb.makeHeader(dT, Gridview1);
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

        protected void Gridview1_RowDataBound1(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                storid = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "id").ToString());
            }
        }
    }
}