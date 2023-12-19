using OfficeOpenXml;
using System;
using System.Data;
using System.Globalization;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Statystyki_2018
{
    public partial class okrr : System.Web.UI.Page
    {
        public Class1 cl = new Class1();
        public pdfTables pdfT = new pdfTables();
        public common cm = new common();
        public tabele tabela = new tabele();
        public dataReaders dr = new dataReaders();
        public static string tenPlik = "okrr.aspx";
        protected bool flaga = false;

        private int storid = 0;
        private int rowIndex = 1;

        protected void Page_Load(object sender, EventArgs e)
        {
            string idWydzial =Request.QueryString["w"];
            try
            {
                if (idWydzial == null)
                {
                    Server.Transfer("default.aspx");
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

                    if (Date1.Text.Length == 0)
                    {
                        Date1.Date = DateTime.Parse(dTime.Year.ToString() + "-" + dTime.Month.ToString("D2") + "-01");
                    }

                    if (Date2.Text.Length == 0)
                    {
                        Date2.Date = DateTime.Parse(dTime.Year.ToString() + "-" + dTime.Month.ToString("D2") + "-" + DateTime.DaysInMonth(dTime.Year, dTime.Month).ToString("D2"));
                    }

                    Session["id_dzialu"] = idWydzial;
                    Session["data_1"] = Date1.Date.ToShortDateString();
                    Session["data_2"] = Date2.Date.ToShortDateString();

                    if (!IsPostBack)
                    {
                        var fileContents = System.IO.File.ReadAllText(Server.MapPath(@"~//version.txt"));    // file read with version
                        this.Title = "Statystyki " + fileContents.ToString().Trim();
                        clearHedersSession();
                        makeHeader();
                        odswierz();
                        makeLabels();
                    }
                }
            }
            catch
            {
               
            }
        }// end of Page_Load

        protected void clearHedersSession()
        {
            Session["header_01"] = null;
            Session["header_02"] = null;
            Session["header_03"] = null;
            Session["header_04"] = null;
            Session["header_05"] = null;
        }

        protected void odswierz()
        {
            string idDzialu = (string)Session["id_dzialu"];
            id_dzialu.Text = (string)Session["txt_dzialu"];
            try
            {
                cm.log.Info("okrr: start generowania tabeli 2");
                DataTable tabelka01 = dr.generuj_dane_do_tabeli_wierszy2018(Date1.Date, Date2.Date, (string)Session["id_dzialu"], 2, 20, 20, tenPlik);
                Session["tabelka002"] = tabelka01;
            }
            catch (Exception ex)
            {
                cm.log.Error(tenPlik + " Generowanie tabel 2" + ex.Message);
            }
            try
            {
                cm.log.Info("okrr: start generowania tabeli 1");
                DataTable tabela01 = Tabela01(int.Parse(idDzialu));
                Session["tabelka001"] = tabela01;
                GridView1.DataSource = null;
                GridView1.DataSourceID = null;
                GridView1.DataSource = tabela01;
                GridView1.DataBind();
            }
            catch (Exception ex)
            {
                cm.log.Error(tenPlik + " Generowanie tabel 1" + ex.Message);
            }
            try
            {
                cm.log.Info("okrr: start generowania tabeli 3");
                DataTable tabela03 = Tabela03(int.Parse(idDzialu));
                Session["tabelka003"] = tabela03;
                GridView3.DataSource = null;
                GridView3.DataSourceID = null;
                GridView3.DataSource = tabela03;
                GridView3.DataBind();
            }
            catch (Exception ex)
            {
                cm.log.Error(tenPlik + " Generowanie tabel 3" + ex.Message);
            }
            try
            {
                cm.log.Info("okrr: start generowania tabeli 4");
                DataTable tabela04 = Tabela04(int.Parse(idDzialu));
                Session["tabelka004"] = tabela04;
                GridView4.DataSource = null;
                GridView4.DataSourceID = null;
                GridView4.DataSource = tabela04;
                GridView4.DataBind();
            }
            catch (Exception ex)
            {
                cm.log.Error(tenPlik + " Generowanie tabel 4" + ex.Message);
            }
            try
            {
                cm.log.Info("okrr: start generowania tabeli 5");
                DataTable tabela05 = Tabela05(int.Parse(idDzialu));
                Session["tabelka005"] = tabela05;
                GridView5.DataSource = null;
                GridView5.DataSourceID = null;
                GridView5.DataSource = tabela05;
                GridView5.DataBind();
            }
            catch (Exception ex)
            {
                cm.log.Error(tenPlik + " Generowanie tabel 5" + ex.Message);
            }

            try
            {
                Label11.Visible = cl.debug(int.Parse(idDzialu));
                infoLabel2.Visible = cl.debug(int.Parse(idDzialu));
                infoLabel3.Visible = cl.debug(int.Parse(idDzialu));
                infoLabel4.Visible = cl.debug(int.Parse(idDzialu));
            }
            catch
            {
                Label11.Visible = false;
                infoLabel2.Visible = false;
                infoLabel3.Visible = false;
                infoLabel4.Visible = false;
            }

            Label3.Text = cl.nazwaSadu((string)Session["id_dzialu"]);
        }

        #region "nagłowki tabel"

        protected void makeHeader()
        {
            System.Web.UI.WebControls.GridView sn = new System.Web.UI.WebControls.GridView();

            DataTable dT_01 = new DataTable();
            dT_01.Columns.Clear();
            dT_01.Columns.Add("Column1", typeof(string));
            dT_01.Columns.Add("Column2", typeof(string));
            dT_01.Columns.Add("Column3", typeof(string));
            dT_01.Columns.Add("Column4", typeof(string));
            dT_01.Columns.Add("Column5", typeof(string));

            DataTable dT_02 = new DataTable();
            dT_02.Columns.Clear();
            dT_02.Columns.Add("Column1", typeof(string));
            dT_02.Columns.Add("Column2", typeof(string));
            dT_02.Columns.Add("Column3", typeof(string));
            dT_02.Columns.Add("Column4", typeof(string));
            dT_02.Columns.Add("Column5", typeof(string));

            DataTable dT_03 = new DataTable();
            dT_03.Columns.Clear();
            dT_03.Columns.Add("Column1", typeof(string));
            dT_03.Columns.Add("Column2", typeof(string));
            dT_03.Columns.Add("Column3", typeof(string));
            dT_03.Columns.Add("Column4", typeof(string));
            dT_03.Columns.Add("Column5", typeof(string));

            DataTable dT_04 = new DataTable();
            dT_04.Columns.Clear();
            dT_04.Columns.Add("Column1", typeof(string));
            dT_04.Columns.Add("Column2", typeof(string));
            dT_04.Columns.Add("Column3", typeof(string));
            dT_04.Columns.Add("Column4", typeof(string));
            dT_04.Columns.Add("Column5", typeof(string));
            DataTable dT_05 = new DataTable();
            dT_05.Columns.Clear();
            dT_05.Columns.Add("Column1", typeof(string));
            dT_05.Columns.Add("Column2", typeof(string));
            dT_05.Columns.Add("Column3", typeof(string));
            dT_05.Columns.Add("Column4", typeof(string));
            dT_05.Columns.Add("Column5", typeof(string));

            #region tabela  1

            dT_01.Clear();

            dT_01.Rows.Add(new Object[] { "1", "Symbol 003o", "1", "1", "h" });
            dT_01.Rows.Add(new Object[] { "1", "Wyrok", "1", "1", "h" });
            dT_01.Rows.Add(new Object[] { "1", "Ugoda", "1", "1", "h" });
            dT_01.Rows.Add(new Object[] { "2", "", "1", "2", "h" });
            dT_01.Rows.Add(new Object[] { "2", "w tym w sprawach karnych", "1", "2", "h" });

            dT_01.Rows.Add(new Object[] { "2", "Urlopy", "1", "2", "h" });
            dT_01.Rows.Add(new Object[] { "2", "Zwolnienia", "1", "2", "h" });

            dT_01.Rows.Add(new Object[] { "2", "Razem", "1", "2", "h" });
            dT_01.Rows.Add(new Object[] { "2", "w tym", "3", "1", "h" });
            dT_01.Rows.Add(new Object[] { "2", "dane wykazane co do spraw (sygnatur)", "4", "2", "h" });

            dT_01.Rows.Add(new Object[] { "3", "Razem", "1", "3", "h" });
            dT_01.Rows.Add(new Object[] { "3", "w terminie ustawowym", "1", "3", "h" });
            dT_01.Rows.Add(new Object[] { "3", "Po upływie terminu ustawowego", "1", "3", "h" });
            dT_01.Rows.Add(new Object[] { "3", "w tym nieuspra- wiedliwione", "1", "3", "h" });
            dT_01.Rows.Add(new Object[] { "3", "Nkd", "1", "3", "h" });

            dT_01.Rows.Add(new Object[] { "3", "RC", "4", "1", "h" });

            dT_01.Rows.Add(new Object[] { "3", "RNs", "1", "3", "h" });
            dT_01.Rows.Add(new Object[] { "3", "RNc", "1", "3", "h" });
            dT_01.Rows.Add(new Object[] { "3", "Nsm", "1", "3", "h" });
            dT_01.Rows.Add(new Object[] { "3", "RCo", "1", "3", "h" });
            dT_01.Rows.Add(new Object[] { "3", "RCps", "1", "3", "h" });
            dT_01.Rows.Add(new Object[] { "3", "Nmo", "1", "3", "h" });
            dT_01.Rows.Add(new Object[] { "3", "RCz", "1", "3", "h" });
            dT_01.Rows.Add(new Object[] { "3", "WSC", "1", "3", "h" });
            dT_01.Rows.Add(new Object[] { "3", "WSNc ", "1", "3", "h" });
            dT_01.Rows.Add(new Object[] { "4", "L.p.", "1", "4", "h" });
            dT_01.Rows.Add(new Object[] { "4", "Funkcja", "1", "4", "h" });
            dT_01.Rows.Add(new Object[] { "4", "Imię i Nazwisko / Stanowisko", "1", "4", "h" });
            dT_01.Rows.Add(new Object[] { "4", "Ilość wokand rozpoznanych", "1", "4", "h" });
            dT_01.Rows.Add(new Object[] { "4", "Sprawy wyznaczone ogółem", "2", "2" });
            dT_01.Rows.Add(new Object[] { "4", "Etat", "1", "4", "h" });
            dT_01.Rows.Add(new Object[] { "4", "Etat orzecz- niczy", "1", "4", "h" });
            dT_01.Rows.Add(new Object[] { "4", "Terminowość sporządzania orzeczeń", "4", "1", "h" });
            dT_01.Rows.Add(new Object[] { "4", "Uzasadnienia wygłoszone", "1", "4", "h" });
            dT_01.Rows.Add(new Object[] { "4", "Oddalono lub utrzymano w mocy apelacje / zażalenia", "1", "4", "h" });
            dT_01.Rows.Add(new Object[] { "4", "RCz - p1  oddalono lub utrzymano w mocy zażalenia", "1", "4", "h" });
            dT_01.Rows.Add(new Object[] { "4", "Nieobecności", "2", "2", "h" });
            dT_01.Rows.Add(new Object[] { "4", "Załatwienia", "14", "1", "h" });
            dT_01.Rows.Add(new Object[] { "4", "Razem", "1", "4", "h" });
            dT_01.Rows.Add(new Object[] { "4", "Alk", "1", "2", "h" });
            dT_01.Rows.Add(new Object[] { "4", "Op", "1", "2", "h" });
            dT_01.Rows.Add(new Object[] { "4", "Opm", "1", "2", "h" });
            dT_01.Rows.Add(new Object[] { "4", "Nw", "1", "2", "h" });
            dT_01.Rows.Add(new Object[] { "4", "Uwagi", "1", "4", "h" });

            Session["header_01"] = dT_01;
            //  makeHeader2(sn, dT_01);

            #endregion tabela  1

            #region tabela  2 ()

            dT_02.Clear();

            dT_02.Rows.Add(new Object[] { "1", "Opm", "1", "1", "h" });//
            dT_02.Rows.Add(new Object[] { "1", "Op", "1", "1", "h" });//
            dT_02.Rows.Add(new Object[] { "1", "Nw", "1", "1", "h" });//
            dT_02.Rows.Add(new Object[] { "1", "Alk", "1", "1", "h" });//

            dT_02.Rows.Add(new Object[] { "2", "Ilość posiedzeń wykonawczych", "1", "2", "h" });//
            dT_02.Rows.Add(new Object[] { "2", "Ilość spraw wyznaczonych", "4", "1", "h" });//

            dT_02.Rows.Add(new Object[] { "3", "L.p.", "1", "3", "h" });//
            dT_02.Rows.Add(new Object[] { "3", "Funkcja", "1", "3", "h" });//
            dT_02.Rows.Add(new Object[] { "3", "Stanowisko", "1", "3", "h" });//
            dT_02.Rows.Add(new Object[] { "3", "Imię", "1", "3", "h" });//
            dT_02.Rows.Add(new Object[] { "3", "Nazwisko", "1", "3", "h" });//
            dT_02.Rows.Add(new Object[] { "3", "Postępowanie wykonawcze", "5", "1", "h" });//
            dT_02.Rows.Add(new Object[] { "3", "Uwagi", "1", "3", "h" });//

            Session["header_02"] = dT_02;

            #endregion tabela  2 ()

            #region tabela  3 ()

            dT_03.Clear();
            for (int i = 0; i < 8; i++)
            {
                dT_03.Rows.Add(new Object[] { "1", "R", "1", "1", "h" });
                dT_03.Rows.Add(new Object[] { "1", "P", "1", "1", "h" });
            }

            dT_03.Rows.Add(new Object[] { "2", "RC", "1", "2", "h" });
            dT_03.Rows.Add(new Object[] { "2", "RNs", "1", "2", "h" });
            dT_03.Rows.Add(new Object[] { "2", "Rsm", "1", "2", "h" });
            dT_03.Rows.Add(new Object[] { "1", "Nkd", "2", "1", "h" });
            dT_03.Rows.Add(new Object[] { "1", "Nmo", "2", "1", "h" });
            dT_03.Rows.Add(new Object[] { "1", "RCo", "2", "1", "h" });
            dT_03.Rows.Add(new Object[] { "1", "RCps", "2", "1", "h" });
            dT_03.Rows.Add(new Object[] { "2", "RC w tym", "1", "2", "h" });
            dT_03.Rows.Add(new Object[] { "2", "RNs", "1", "2", "h" });
            dT_03.Rows.Add(new Object[] { "2", "Rsm", "1", "2", "h" });
            dT_03.Rows.Add(new Object[] { "1", "Nkd", "2", "1", "h" });
            dT_03.Rows.Add(new Object[] { "1", "Nmo", "2", "1", "h" });
            dT_03.Rows.Add(new Object[] { "1", "RCz", "2", "1", "h" });
            dT_03.Rows.Add(new Object[] { "1", "RCo", "2", "1", "h" });
            dT_03.Rows.Add(new Object[] { "1", "RCps", "2", "1", "h" });

            dT_03.Rows.Add(new Object[] { "3", "L.p.", "1", "3", "h" });
            dT_03.Rows.Add(new Object[] { "3", "Sędzia", "1", "3", "h" });
            dT_03.Rows.Add(new Object[] { "3", "Ilość spraw wyznaczonych w postępowaniu rozpoznawczym", "11", "1", "h" });
            dT_03.Rows.Add(new Object[] { "3", "Razem wyzna- czonych", "1", "3", "h" });
            dT_03.Rows.Add(new Object[] { "3", "Ilość spraw załatwionych w postępowaniu rozpoznawczym", "11", "1", "h" });
            dT_03.Rows.Add(new Object[] { "3", "Razem załatwionych", "1", "3", "h" });
            dT_03.Rows.Add(new Object[] { "3", "Ilość spraw odroczonych", "1", "3", "h" });

            Session["header_03"] = dT_03;
            //  makeHeader3(sn, dT_03);

            #endregion tabela  3 ()

            #region tabela  4 ()

            dT_04.Clear();
            for (int i = 0; i < 2; i++)
            {
                dT_04.Rows.Add(new Object[] { "1", "RC", "1", "1", "h" });
                dT_04.Rows.Add(new Object[] { "1", "RNs", "1", "1", "h" });
                dT_04.Rows.Add(new Object[] { "1", "Nsm", "1", "1" });
                dT_04.Rows.Add(new Object[] { "1", "Nkd", "1", "1" });

                dT_04.Rows.Add(new Object[] { "1", "Nmo", "1", "1" });
                dT_04.Rows.Add(new Object[] { "1", "RCo", "1", "1", "h" });
                dT_04.Rows.Add(new Object[] { "1", "RCps", "1", "1" });
            }

            dT_04.Rows.Add(new Object[] { "2", "L.p.", "1", "2", "h" });
            dT_04.Rows.Add(new Object[] { "2", "Nazwisko i imię sędziego", "1", "2" });
            dT_04.Rows.Add(new Object[] { "2", "Ilość spraw wyznaczonych na pos. niejawnym", "7", "1" });
            dT_04.Rows.Add(new Object[] { "2", "Razem wyznaczonych", "1", "2" });
            dT_04.Rows.Add(new Object[] { "2", "Ilość spraw załatwionych na pos. niejawnym", "7", "1", "h" });
            dT_04.Rows.Add(new Object[] { "2", "Razem Załatwionych", "1", "2", "h" });

            Session["header_04"] = dT_04;

            //  makeHeader6(sn, dT_05);

            #endregion tabela  4 ()

            #region tabela  5 ()

            dT_05.Clear();

            dT_05.Rows.Add(new Object[] { "1", "C", "1", "1" });
            dT_05.Rows.Add(new Object[] { "1", "Ns", "1", "1" });
            dT_05.Rows.Add(new Object[] { "1", "Co", "1", "1" });
            dT_05.Rows.Add(new Object[] { "1", "Nc", "1", "1" });

            dT_05.Rows.Add(new Object[] { "1", "Ns-Rej.", "1", "1" });
            dT_05.Rows.Add(new Object[] { "1", "Razem", "1", "1" });

            dT_05.Rows.Add(new Object[] { "2", "L.p.", "1", "2" });
            dT_05.Rows.Add(new Object[] { "2", "Imie i nazwisko sędziego", "1", "2" });
            dT_05.Rows.Add(new Object[] { "2", "Liczba załatwionych spraw od początku roku ", "6", "1" });

            Session["header_05"] = dT_05;

            //  makeHeader6(sn, dT_05);

            #endregion tabela  5 ()

            #region tabela  6 ()

            DataTable dT_06 = new DataTable();
            dT_06.Columns.Clear();
            dT_06.Columns.Add("Column1", typeof(string));
            dT_06.Columns.Add("Column2", typeof(string));
            dT_06.Columns.Add("Column3", typeof(string));
            dT_06.Columns.Add("Column4", typeof(string));
            dT_06.Clear();

            dT_06.Rows.Add(new Object[] { "1", "do 3 miesięcy", "1", "1" });
            dT_06.Rows.Add(new Object[] { "1", "suma powyżej 3 miesięcy", "1", "1" });
            dT_06.Rows.Add(new Object[] { "1", "powyżej 3 do 6 miesięcy", "1", "1" });
            dT_06.Rows.Add(new Object[] { "1", "powyżej 6 do 12 miesięcy", "1", "1" });
            dT_06.Rows.Add(new Object[] { "1", "suma powyżej 12 miesięcy", "1", "1" });
            dT_06.Rows.Add(new Object[] { "1", "powyżej 12 miesięcy do 2 lat", "1", "1" });
            dT_06.Rows.Add(new Object[] { "1", "powyżej 2 do 3 lat", "1", "1" });
            dT_06.Rows.Add(new Object[] { "1", "suma powyżej 3 lat", "1", "1" });
            dT_06.Rows.Add(new Object[] { "1", "powyżej 3 do 5 lat", "1", "1" });
            dT_06.Rows.Add(new Object[] { "1", "powyżej 5 do 8 lat", "1", "1" });
            dT_06.Rows.Add(new Object[] { "1", "ponad 8 lat", "1", "1" });

            dT_06.Rows.Add(new Object[] { "2", "Opis.", "1", "2" });

            dT_06.Rows.Add(new Object[] { "2", "Struktura pozostałości  -   liczba spraw  ( w tym zawieszone) ", "11", "1" });
            Session["header_06"] = dT_06;
            Session["header_07"] = dT_06;

            //     makeHeader7(sn, dT_06);

            #endregion tabela  6 ()

            #region tabela  8 ()

            DataTable dT_08 = new DataTable();
            dT_08.Columns.Clear();
            dT_08.Columns.Add("Column1", typeof(string));
            dT_08.Columns.Add("Column2", typeof(string));
            dT_08.Columns.Add("Column3", typeof(string));
            dT_08.Columns.Add("Column4", typeof(string));
            dT_08.Clear();

            dT_08.Rows.Add(new Object[] { "1", "C", "1", "1" });
            dT_08.Rows.Add(new Object[] { "1", "Ns", "1", "1" });
            dT_08.Rows.Add(new Object[] { "1", "Nsm", "1", "1" });
            dT_08.Rows.Add(new Object[] { "1", "Co", "1", "1" });
            dT_08.Rows.Add(new Object[] { "1", "Nmo", "1", "1" });
            dT_08.Rows.Add(new Object[] { "1", "Cps", "1", "1" });
            dT_08.Rows.Add(new Object[] { "1", "Nkd", "1", "1" });
            dT_08.Rows.Add(new Object[] { "1", "Łącznie", "1", "1" });
            dT_08.Rows.Add(new Object[] { "2", "L.p.", "1", "2" });
            dT_08.Rows.Add(new Object[] { "2", "Imię i Nazwisko", "1", "2" });
            dT_08.Rows.Add(new Object[] { "2", "Pozostało z na następny miesiąc", "8", "1" });

            dT_08.Rows.Add(new Object[] { "2", "W tym 3-6 miesięcy", "1", "2" });
            dT_08.Rows.Add(new Object[] { "2", "6-12 miesięcy", "1", "2" });
            dT_08.Rows.Add(new Object[] { "2", "Powyżej 12 miesięcy", "1", "2" });
            dT_08.Rows.Add(new Object[] { "2", "Powyżej 24 miesięcy", "1", "2" });

            //ILOŚĆ WOKAND ŁĄCZNIE
            Session["header_08"] = dT_08;
            //  makeHeader8(sn, dT_08);

            #endregion tabela  8 ()
        }

        protected void GridView3_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                DataTable dT = (DataTable)Session["header_03"];
                tabela.makeHeader(dT, GridView4);
            }
        }

        protected void GridView4_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                DataTable dT = (DataTable)Session["header_04"];
                tabela.makeHeader(dT, GridView5);
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

                id_dzialu.Text = (string)Session["txt_dzialu"];
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
                    lbtabela1.Text = "Informacja statystyczna z ruchu spraw za miesiąc:  " + strMonthName + " " + Date2.Date.Year.ToString() + " roku.";
                    lbTabela2.Text = "Wydajność pracy sędziów na posiedzeniu wykonawczym za miesiąc " + strMonthName + " " + Date2.Date.Year.ToString() + " roku.";
                    lbTabela3.Text = "Obciążenie i wydajność pracy sędziów - ROZPRAWA I POS. JAWNE (na sesjach) za miesiąc " + strMonthName + " " + Date2.Date.Year.ToString() + " roku.";
                    lbTabela4.Text = "Obciążenie i wydajność pracy sędziów – POSIEDZENIU NIEJAWNYM za miesiąc " + strMonthName + " " + Date2.Date.Year.ToString() + " roku.";
                }
                else
                {
                    lbtabela1.Text = "Informacja statystyczna z ruchu spraw za okres od:  " + Date1.Text + " do  " + Date2.Text;
                    lbTabela2.Text = "Wydajność pracy sędziów na posiedzeniu wykonawczym za okres od " + Date1.Text + " do  " + Date2.Text;
                    lbTabela3.Text = "Obciążenie i wydajność pracy sędziów - ROZPRAWA I POS. JAWNE (na sesjach) za okres od" + Date1.Text + " do  " + Date2.Text;
                    lbTabela4.Text = "Obciążenie i wydajność pracy sędziów – POSIEDZENIU NIEJAWNYM za" + Date1.Text + " do  " + Date2.Text;
                    //     Label15.Text = "Załatwienia za okres od " + Date1.Text + " do  " + Date2.Text;
                }
            }
            catch
            {
            }
        }

        protected void Button3_Click(object sender, EventArgs e)
        {
            string path = Server.MapPath("Template") + "\\okrr.xlsx";
            FileInfo existingFile = new FileInfo(path);

            string download = Server.MapPath("Template") + @"\okrr";
            FileInfo fNewFile = new FileInfo(download + "_.xlsx");

            // pierwsza tabelka

            DataTable tabelka001 = (DataTable)Session["tabelka002"];

            using (ExcelPackage MyExcel = new ExcelPackage(existingFile))
            {
                ExcelWorksheet MyWorksheet = MyExcel.Workbook.Worksheets[1];

                DataTable table1 = (DataTable)Session["tabelka001"];

                MyWorksheet = tabela.tworzArkuszwExcleOKRR(MyExcel.Workbook.Worksheets[1], table1, 33, 0, 10, true, false, true, true, true);

                //podtabel
                int przesuniecieWdol = table1.Rows.Count;
                tabela.komorkaExcela(MyWorksheet, przesuniecieWdol + 10, 1, "Razem", true, 10, 2, true, true);
                tabela.komorkaExcela(MyWorksheet, przesuniecieWdol + 10, 4, "Zaległości z poprzedniego miesiąca", true, 0, 14);
                tabela.komorkaExcela(MyWorksheet, przesuniecieWdol + 11, 4, "wpływ", true, 0, 14);
                tabela.komorkaExcela(MyWorksheet, przesuniecieWdol + 12, 4, "załatwienia", true, 0, 14);
                tabela.komorkaExcela(MyWorksheet, przesuniecieWdol + 13, 4, "pozostałość na następny miesiąc", true, 0, 14);
                tabela.komorkaExcela(MyWorksheet, przesuniecieWdol + 14, 4, "powyżej 3 do 6 miesięcy", true, 0, 14);
                tabela.komorkaExcela(MyWorksheet, przesuniecieWdol + 15, 4, "powyżej 6 do 12 miesięcy", true, 0, 14);
                tabela.komorkaExcela(MyWorksheet, przesuniecieWdol + 16, 4, "powyżej 12 miesięcy", true, 0, 14);
                tabela.komorkaExcela(MyWorksheet, przesuniecieWdol + 17, 4, "w tym", true, 3, 1, true, true);
                tabela.komorkaExcela(MyWorksheet, przesuniecieWdol + 17, 6, "powyżej 12 miesięcy - 2 lat", true, 0, 12);
                tabela.komorkaExcela(MyWorksheet, przesuniecieWdol + 18, 6, "powyżej 2 lat - 3 lat", true, 0, 12);
                tabela.komorkaExcela(MyWorksheet, przesuniecieWdol + 19, 6, "powyżej 3 lat - 5 lat", true, 0, 12);
                tabela.komorkaExcela(MyWorksheet, przesuniecieWdol + 20, 6, "powyżej 5 lat", true, 0, 12);

             
                try
                {
                    for (int i = 0; i < 11; i++)//wiersze
                    {
                        for (int j = 0; j < 4; j++)// kolunmny
                        {
                            tabela.komorkaExcela(MyWorksheet, przesuniecieWdol + 10 + i, j + 19, tabelka001.Rows[i][j ].ToString(), false, 0, 0, true, false);
                        }
                        tabela.komorkaExcela(MyWorksheet, przesuniecieWdol + 10 + i, 22, "", true, 10, 1, true, true);

                        for (int j = 4; j < 18; j++)// kolunmny
                        {
                            tabela.komorkaExcela(MyWorksheet, przesuniecieWdol + 10 + i, j + 20, tabelka001.Rows[i][j-1 ].ToString(), false, 0, 0, true, false);
                        }
                    }
                }
                catch (Exception ex)
                {
                    cm.log.Error(tenPlik + " Generwanie pod tabeli " + ex.Message);
                }

                DataTable table2 = (DataTable)Session["tabelka003"];
                ExcelWorksheet MyWorksheet2 = tabela.tworzArkuszwExcle(MyExcel.Workbook.Worksheets[2], table2, 11, 0, 9, true, true, true, true, true);

                DataTable table3 = (DataTable)Session["tabelka004"];
                ExcelWorksheet MyWorksheet3 = tabela.tworzArkuszwExcle(MyExcel.Workbook.Worksheets[4], table3, 26, 0, 5, true, true, false, false, false);

                DataTable table4 = (DataTable)Session["tabelka005"];
                ExcelWorksheet MyWorksheet4 = tabela.tworzArkuszwExcle(MyExcel.Workbook.Worksheets[5], table4, 17, 0, 4, true, true, false, false, false);

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

            odswierz();
        }

        protected void LinkButton54_Click(object sender, EventArgs e)
        {
            odswierz();
        }

        protected void GridView3_RowCreated1(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                DataTable dT = (DataTable)Session["header_02"];
                tabela.makeHeader(dT, GridView3);
            }
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                storid = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "id").ToString());

                if (!flaga)
                {
                    e.Row.Cells[32].ColumnSpan = 4;
                }
            }
        }

        protected void GridView4_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                DataTable table = (DataTable)Session["tabelka004"];
                tabela.makeSumRow(table, e, tenPlik);
            }
        }

        protected void GridView6_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                DataTable dT = (DataTable)Session["header_01"];
                tabela.makeHeader(dT, GridView1);
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

        //private mathods
        private DataTable Tabela01(int id_dzialu)
        {
            return dr.generuj_dane_do_tabeli_sedziowskiej_2019(id_dzialu, 1, Date1.Date, Date2.Date, 40, tenPlik);
        }

        private DataTable Tabela03(int id_dzialu)
        {
            return dr.generuj_dane_do_tabeli_sedziowskiej_2019(id_dzialu, 3, Date1.Date, Date2.Date, 29, tenPlik);
        }

        private DataTable Tabela04(int id_dzialu)
        {
            return dr.generuj_dane_do_tabeli_sedziowskiej_2019(id_dzialu, 4, Date1.Date, Date2.Date, 29, tenPlik);
        }

        private DataTable Tabela05(int id_dzialu)
        {
            return dr.generuj_dane_do_tabeli_sedziowskiej_2019(id_dzialu, 5, Date1.Date, Date2.Date, 29, tenPlik);
        }

        private DataTable Tabela06(int id_dzialu)
        {
            return dr.generuj_dane_do_tabeli_sedziowskiej_2019(id_dzialu, 6, Date1.Date, Date2.Date, 29, tenPlik);
        }

        private GridViewRow wierszTabeli(int idWiersza, string idtabeli, string tekst)
        {
            // nowy wiersz

            DataTable tabelka01 = (DataTable)Session["tabelka002"];
            GridViewRow NewTotalRow = new GridViewRow(0, 0, DataControlRowType.DataRow, DataControlRowState.Insert);
            NewTotalRow.Cells.Add(tabela.cela(tekst, 1, 14, "borderTopLeft "));
            NewTotalRow.Cells.Add(tabela.cela("<a class='normal' href=\"javascript: openPopup('popup.aspx?sesja=" + (idWiersza + 1).ToString().Trim() + "!2!1!3')\">" + tabelka01.Rows[idWiersza][0].ToString().Trim() + "</a>", 1, 1, "borderTopLeft"));
            NewTotalRow.Cells.Add(tabela.cela("<a class='normal' href=\"javascript: openPopup('popup.aspx?sesja=" + (idWiersza + 1).ToString().Trim() + "!2!2!3')\">" + tabelka01.Rows[idWiersza][1].ToString().Trim() + "</a>", 1, 1, "borderTopLeft"));
            NewTotalRow.Cells.Add(tabela.cela("<a class='normal' href=\"javascript: openPopup('popup.aspx?sesja=" + (idWiersza + 1).ToString().Trim() + "!2!3!3')\">" + tabelka01.Rows[idWiersza][2].ToString().Trim() + "</a>", 1, 1, "borderTopLeft"));
            for (int i = 4; i < 19; i++)
            {
                NewTotalRow.Cells.Add(tabela.cela("<a class='normal' href=\"javascript: openPopup('popup.aspx?sesja=" + (idWiersza + 1).ToString().Trim() + "!2!" + i.ToString() + "!3')\">" + tabelka01.Rows[idWiersza][i-1].ToString().Trim() + "</a>", 1, 1, "borderTopLeft"));
            }
          //  NewTotalRow.Cells.Add(tabela.cela("<a class='normal' href=\"javascript: openPopup('popup.aspx?sesja=" + (idWiersza + 1).ToString().Trim() + "!2!16!3')\">" + tabelka01.Rows[idWiersza][16].ToString().Trim() + "</a>", 1, 1, "borderTopLeft"));

            return NewTotalRow;
        }
      
        private GridViewRow wierszTabeli1(int idWiersza, string idtabeli, string tekst)
        {
            // nowy wiersz

            DataTable tabelka01 = (DataTable)Session["tabelka002"];
            GridViewRow NewTotalRow = new GridViewRow(0, 0, DataControlRowType.DataRow, DataControlRowState.Insert);
            NewTotalRow.Cells.Add(tabela.cela(tekst, 1, 14, "borderTopLeft "));

            NewTotalRow.Cells.Add(tabela.cela("<a class='normal' href=\"javascript: openPopup('popup.aspx?sesja=" + (idWiersza + 1).ToString().Trim() + "!2!1!3')\">" + tabelka01.Rows[idWiersza][0].ToString().Trim() + "</a>", 1, 1, "borderTopLeft"));
            NewTotalRow.Cells.Add(tabela.cela("<a class='normal' href=\"javascript: openPopup('popup.aspx?sesja=" + (idWiersza + 1).ToString().Trim() + "!2!2!3')\">" + tabelka01.Rows[idWiersza][1].ToString().Trim() + "</a>", 1, 1, "borderTopLeft"));
            NewTotalRow.Cells.Add(tabela.cela("<a class='normal' href=\"javascript: openPopup('popup.aspx?sesja=" + (idWiersza + 1).ToString().Trim() + "!2!3!3')\">" + tabelka01.Rows[idWiersza][2].ToString().Trim() + "</a>", 1, 1, "borderTopLeft"));
            for (int i = 4; i < 14; i++)
            {
                NewTotalRow.Cells.Add(tabela.cela("<a class='normal' href=\"javascript: openPopup('popup.aspx?sesja=" + (idWiersza + 1).ToString().Trim() + "!2!" + i.ToString() + "!3')\">" + tabelka01.Rows[idWiersza][i-1].ToString().Trim() + "</a>", 1, 1, "borderTopLeft"));
            }
            NewTotalRow.Cells.Add(tabela.cela("", 7, 4, "borderTopLeft gray"));
            NewTotalRow.Cells.Add(tabela.cela("<a class='normal' href=\"javascript: openPopup('popup.aspx?sesja=" + (idWiersza + 1).ToString().Trim() + "!2!17!3')\">" + tabelka01.Rows[idWiersza][16].ToString().Trim() + "</a>", 1, 1, "borderTopLeft"));
            return NewTotalRow;
        }

        private GridViewRow wierszTabeli11(int idWiersza, string idtabeli, string tekst)
        {
            // nowy wiersz

            DataTable tabelka01 = (DataTable)Session["tabelka002"];
            GridViewRow NewTotalRow = new GridViewRow(0, 0, DataControlRowType.DataRow, DataControlRowState.Insert);
            NewTotalRow.Cells.Add(tabela.cela(tekst, 1, 14, "borderTopLeft "));
            NewTotalRow.Cells.Add(tabela.cela("<a class='normal' href=\"javascript: openPopup('popup.aspx?sesja=" + (idWiersza + 1).ToString().Trim() + "!2!1!3')\">" + tabelka01.Rows[idWiersza][0].ToString().Trim() + "</a>", 1, 1, "borderTopLeft"));
            NewTotalRow.Cells.Add(tabela.cela("<a class='normal' href=\"javascript: openPopup('popup.aspx?sesja=" + (idWiersza + 1).ToString().Trim() + "!2!2!3')\">" + tabelka01.Rows[idWiersza][1].ToString().Trim() + "</a>", 1, 1, "borderTopLeft"));
            NewTotalRow.Cells.Add(tabela.cela("<a class='normal' href=\"javascript: openPopup('popup.aspx?sesja=" + (idWiersza + 1).ToString().Trim() + "!2!3!3')\">" + tabelka01.Rows[idWiersza][2].ToString().Trim() + "</a>", 1, 1, "borderTopLeft"));
            for (int i = 4; i < 14; i++)
            {
                NewTotalRow.Cells.Add(tabela.cela("<a class='normal' href=\"javascript: openPopup('popup.aspx?sesja=" + (idWiersza + 1).ToString().Trim() + "!2!" + i.ToString() + "!3')\">" + tabelka01.Rows[idWiersza][i-1].ToString().Trim() + "</a>", 1, 1, "borderTopLeft"));
            }
            
            NewTotalRow.Cells.Add(tabela.cela("<a class='normal' href=\"javascript: openPopup('popup.aspx?sesja=" + (idWiersza + 1).ToString().Trim() + "!2!17!3')\">" + tabelka01.Rows[idWiersza][16].ToString().Trim() + "</a>", 1, 1, "borderTopLeft"));
            return NewTotalRow;
        }

        private GridViewRow wierszTabeli2(int idWiersza, string idtabeli, string tekst)
        {
            // nowy wiersz

            DataTable tabelka01 = (DataTable)Session["tabelka002"];
            GridViewRow NewTotalRow = new GridViewRow(0, 0, DataControlRowType.DataRow, DataControlRowState.Insert);
            NewTotalRow.Cells.Add(tabela.cela("w tym", 4, 1, "borderTopLeft gray"));
            NewTotalRow.Cells.Add(tabela.cela(tekst, 1, 13, "borderTopLeft  "));
            NewTotalRow.Cells.Add(tabela.cela("<a class='normal' href=\"javascript: openPopup('popup.aspx?sesja=" + (idWiersza + 1).ToString().Trim() + "!2!1!3')\">" + tabelka01.Rows[idWiersza][0].ToString().Trim() + "</a>", 1, 1, "borderTopLeft"));
            NewTotalRow.Cells.Add(tabela.cela("<a class='normal' href=\"javascript: openPopup('popup.aspx?sesja=" + (idWiersza + 1).ToString().Trim() + "!2!2!3')\">" + tabelka01.Rows[idWiersza][1].ToString().Trim() + "</a>", 1, 1, "borderTopLeft"));
            NewTotalRow.Cells.Add(tabela.cela("<a class='normal' href=\"javascript: openPopup('popup.aspx?sesja=" + (idWiersza + 1).ToString().Trim() + "!2!3!3')\">" + tabelka01.Rows[idWiersza][2].ToString().Trim() + "</a>", 1, 1, "borderTopLeft"));
            for (int i = 4; i < 14; i++)
            {
                NewTotalRow.Cells.Add(tabela.cela("<a class='normal' href=\"javascript: openPopup('popup.aspx?sesja=" + (idWiersza + 1).ToString().Trim() + "!2!" + i.ToString() + "!3')\">" + tabelka01.Rows[idWiersza][i-1].ToString().Trim() + "</a>", 1, 1, "borderTopLeft"));
            }
            NewTotalRow.Cells.Add(tabela.cela("<a class='normal' href=\"javascript: openPopup('popup.aspx?sesja=" + (idWiersza + 1).ToString().Trim() + "!2!17!3')\">" + tabelka01.Rows[idWiersza][16].ToString().Trim() + "</a>", 1, 1, "borderTopLeft"));
            return NewTotalRow;
        }

        private GridViewRow wierszTabeli3(int idWiersza, string idtabeli, string tekst)
        {
            // nowy wiersz

            DataTable tabelka01 = (DataTable)Session["tabelka002"];
            GridViewRow NewTotalRow = new GridViewRow(0, 0, DataControlRowType.DataRow, DataControlRowState.Insert);

            NewTotalRow.Cells.Add(tabela.cela(tekst, 1, 13, "borderTopLeft"));
            NewTotalRow.Cells.Add(tabela.cela("<a class='normal' href=\"javascript: openPopup('popup.aspx?sesja=" + (idWiersza + 1).ToString().Trim() + "!2!1!3')\">" + tabelka01.Rows[idWiersza][0].ToString().Trim() + "</a>", 1, 1, "borderTopLeft"));
            NewTotalRow.Cells.Add(tabela.cela("<a class='normal' href=\"javascript: openPopup('popup.aspx?sesja=" + (idWiersza + 1).ToString().Trim() + "!2!2!3')\">" + tabelka01.Rows[idWiersza][1].ToString().Trim() + "</a>", 1, 1, "borderTopLeft"));
            NewTotalRow.Cells.Add(tabela.cela("<a class='normal' href=\"javascript: openPopup('popup.aspx?sesja=" + (idWiersza + 1).ToString().Trim() + "!2!3!3')\">" + tabelka01.Rows[idWiersza][2].ToString().Trim() + "</a>", 1, 1, "borderTopLeft"));
            for (int i = 4; i < 14; i++)
            {
                NewTotalRow.Cells.Add(tabela.cela("<a class='normal' href=\"javascript: openPopup('popup.aspx?sesja=" + (idWiersza + 1).ToString().Trim() + "!2!" + i.ToString() + "!3')\">" + tabelka01.Rows[idWiersza][i-1].ToString().Trim() + "</a>", 1, 1, "borderTopLeft"));
            }
              NewTotalRow.Cells.Add(tabela.cela("<a class='normal' href=\"javascript: openPopup('popup.aspx?sesja=" + (idWiersza + 1).ToString().Trim() + "!2!17!3')\">" + tabelka01.Rows[idWiersza][16].ToString().Trim() + "</a>", 1, 1, "borderTopLeft"));
            return NewTotalRow;
        }

        public void AddNewRow(object sender, GridViewRowEventArgs e)
        {
            GridView GridView1 = (GridView)sender;
            GridViewRow NewTotalRow = new GridViewRow(0, 0, DataControlRowType.DataRow, DataControlRowState.Insert);

            string idtabeli = "2";
            TableCell HeaderCell = tabela.cela("Razem", 11, 3, "borderTopLeft");
            HeaderCell.BackColor = System.Drawing.Color.LightGray;
            NewTotalRow.Cells.Add(HeaderCell);

            NewTotalRow.Cells.Add(tabela.cela("Zaległości z poprzedniego miesiąca", 1, 14, "borderTopLeft"));
            DataTable tabelka01 = (DataTable)Session["tabelka002"];

            int idWiersza = 0;

            NewTotalRow.Cells.Add(tabela.cela("<a class='normal' href=\"javascript: openPopup('popup.aspx?sesja=" + (idWiersza + 1).ToString().Trim() + "!2!1!3')\">" + tabelka01.Rows[idWiersza][0].ToString().Trim() + "</a>", 1, 1, "borderTopLeft"));
            NewTotalRow.Cells.Add(tabela.cela("<a class='normal' href=\"javascript: openPopup('popup.aspx?sesja=" + (idWiersza + 1).ToString().Trim() + "!2!2!3')\">" + tabelka01.Rows[idWiersza][1].ToString().Trim() + "</a>", 1, 1, "borderTopLeft"));
            NewTotalRow.Cells.Add(tabela.cela("<a class='normal' href=\"javascript: openPopup('popup.aspx?sesja=" + (idWiersza + 1).ToString().Trim() + "!2!3!3')\">" + tabelka01.Rows[idWiersza][2].ToString().Trim() + "</a>", 1, 1, "borderTopLeft"));

            NewTotalRow.Cells.Add(tabela.cela("", 11, 2, "borderTopLeft doubleXcross gray"));

            for (int i = 4; i < 19; i++)
            {
                NewTotalRow.Cells.Add(tabela.cela("<a class='normal' href=\"javascript: openPopup('popup.aspx?sesja=" + (idWiersza + 1).ToString().Trim() + "!2!" + i.ToString() + "!3')\">" + tabelka01.Rows[idWiersza][i-1].ToString().Trim() + "</a>", 1, 1, "borderTopLeft"));
            }
         //   NewTotalRow.Cells.Add(tabela.cela("<a class='normal' href=\"javascript: openPopup('popup.aspx?sesja=" + (idWiersza + 1).ToString().Trim() + "!2!16!3')\">" + tabelka01.Rows[idWiersza][16].ToString().Trim() + "</a>", 1, 1, "borderTopLeft"));

            GridView1.Controls[0].Controls.AddAt(e.Row.RowIndex + rowIndex, NewTotalRow);

            // nowy wiersz

            idWiersza = 1;
            GridView1.Controls[0].Controls.AddAt(e.Row.RowIndex + rowIndex, wierszTabeli(1, idtabeli, "wpływ"));

            // nowy wiersz
            idWiersza = 2;
            GridView1.Controls[0].Controls.AddAt(e.Row.RowIndex + rowIndex, wierszTabeli(2, idtabeli, "załatwienia"));

            // nowy wiersz
            idWiersza = 3;
            GridView1.Controls[0].Controls.AddAt(e.Row.RowIndex + rowIndex, wierszTabeli(3, idtabeli, "pozostałość na następny miesiąc"));

            // nowy wiersz
            idWiersza = 4;
            GridView1.Controls[0].Controls.AddAt(e.Row.RowIndex + rowIndex, wierszTabeli1(4, idtabeli, "powyżej 3 do 6 miesięcy"));

            // nowy wiersz
            idWiersza = 5;
            GridView1.Controls[0].Controls.AddAt(e.Row.RowIndex + rowIndex, wierszTabeli11(idWiersza, idtabeli, "powyżej 6 do 12 miesięcy"));

            // nowy wiersz
            idWiersza = 6;
            GridView1.Controls[0].Controls.AddAt(e.Row.RowIndex + rowIndex, wierszTabeli11(idWiersza, idtabeli, "powyżej 12 miesięcy"));

            // nowy wiersz
            idWiersza = 7;
            GridView1.Controls[0].Controls.AddAt(e.Row.RowIndex + rowIndex, wierszTabeli2(idWiersza, idtabeli, "powyżej 12 miesięcy - 2 lat"));
            // nowy wiersz
            idWiersza = 8;
         
            GridView1.Controls[0].Controls.AddAt(e.Row.RowIndex + rowIndex, wierszTabeli3(idWiersza, idtabeli, "powyżej 2 lat - 3 lat"));
            // nowy wiersz
            idWiersza = 9;
            GridView1.Controls[0].Controls.AddAt(e.Row.RowIndex + rowIndex, wierszTabeli3(idWiersza, idtabeli, "powyżej 3 lat - 5 lat"));

            // nowy wiersz
            idWiersza = 10;
            GridView1.Controls[0].Controls.AddAt(e.Row.RowIndex + rowIndex, wierszTabeli3(idWiersza, idtabeli, "ponad 5 lat"));
        }

        protected void StopkaTabeli03(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                DataTable table = (DataTable)Session["tabelka003"];
                tabela.makeSumRow(table, e, 5, 5);
            }
        }

        protected void StopkaTabeli04(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                DataTable table = (DataTable)Session["tabelka004"];
                tabela.makeSumRow(table, e, tenPlik);
            }
        }

        protected void StopkaTabeli05(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                DataTable table = (DataTable)Session["tabelka005"];
                tabela.makeSumRow(table, e, tenPlik);
            }
        }
    }
}