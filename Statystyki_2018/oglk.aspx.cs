/*
Last Update:
    - version 1.230205
Creation date: 2023-02-05

*/

using OfficeOpenXml;
using System;
using System.Data;
using System.Globalization;
using System.IO;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Statystyki_2018
{
    public partial class oglk : System.Web.UI.Page
    {
        public Class1 cl = new Class1();
        public common cm = new common();
        public tabele tb = new tabele();
        public dataReaders dr = new dataReaders();
        public XMLHeaders xMLHeaders = new XMLHeaders();

        private const string tenPlik = "oglk.aspx";

        private int storid = 0;
        private int rowIndex = 1;

        private const string tenPlikNazwa = "oglk";
        public string path = "";

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
                Session["id_dzialu"] = idWydzial;
                String IdentyfikatorUzytkownika = string.Empty;
                IdentyfikatorUzytkownika = (string)Session["identyfikatorUzytkownika"];
                DataTable parametry = cm.makeParameterTable();
                parametry.Rows.Add("@identyfikatorUzytkownika", IdentyfikatorUzytkownika);


                if (cm.getQuerryValue("select admin from uzytkownik where ident =@identyfikatorUzytkownika", cm.con_str, parametry) == "0" && !cm.dostep(idWydzial, (string)Session["identyfikatorUzytkownika"]))
                {
                    Server.Transfer("default.aspx?info='Użytkownik " + (string)Session["identyfikatorUzytkownika"] + " nie praw do działu nr " + idWydzial + "'");
                }
                path = Server.MapPath("~\\Template\\" + tenPlikNazwa + ".xlsx");
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
            }
            catch
            { }
            odswiez();
            debug();
        }// end of Page_Load

        private void debug()
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
                //   Label3.Text = cl.nazwaSadu((string)Session["id_dzialu"]);
                Label28.Text = cl.podajUzytkownika(User_id, domain);
                Label29.Text = DateTime.Now.ToLongDateString();
                Label30.Text = System.IO.File.ReadAllText(Server.MapPath(@"~//version.txt")).ToString().Trim();
            }
            catch
            { }
            try
            {
                string idDzialu = (string)Session["id_dzialu"];
                infoLabel1.Visible = cl.debug(int.Parse(idDzialu));
            }
            catch
            {
                infoLabel1.Visible = false;
            }
        }

        protected void Odswiez(object sender, EventArgs e)
        {
            odswiez();
        }

        protected void odswiez()
        {
            //odswiezenie danych
            makeHeader();
            tabela_1();
            tabela_3();
            Tabela_4();
            tabela_5();
        }

        protected void tabela_3()
        {
            string idDzialu = (string)Session["id_dzialu"];
            if (cl.debug(int.Parse(idDzialu)))
            {
                cm.log.Info(tenPlik + ": rozpoczęcie tworzenia tabeli 3");
            }
            Session["tabelka003"] = dr.tworzTabele(int.Parse(idDzialu), 3, Date1.Date, Date2.Date, 35, GridView3, tenPlik);
            GridView3.DataBind();
        }
        protected void tworzPlikExcell(object sender, EventArgs e)
        {
            // execel begin
            string path = Server.MapPath("Template") + "\\oglk.xlsx";
            FileInfo existingFile = new FileInfo(path);
            if (!existingFile.Exists)
            {
                return;
            }
            string download = Server.MapPath("Template") + @"\oglk";

            FileInfo fNewFile = new FileInfo(download + "_.xlsx");

            // pierwsza tabelka

            using (ExcelPackage MyExcel = new ExcelPackage(existingFile))
            {
                ExcelWorksheet MyWorksheet1 = MyExcel.Workbook.Worksheets[1];

               
                DataTable table1 = (DataTable)Session["tabelka001"];
                MyWorksheet1 = tb.tworzArkuszwExcle(MyExcel.Workbook.Worksheets[1], table1, 22, 0, 7,false, true, false, false, false);

                //pod tabela
                int rowik = table1.Rows.Count +7;

                tb.komorkaExcela(MyWorksheet1, rowik , 1, "Zaległość z poprzedniego miesiąca", true, 0, 5);
                tb.komorkaExcela(MyWorksheet1, rowik + 1, 1, "Wpływ", true, 0, 5);
                tb.komorkaExcela(MyWorksheet1, rowik + 2, 1, "Wpływ spraw do rozpoznania przez referendarzy sądowych", true, 0, 5);
                tb.komorkaExcela(MyWorksheet1, rowik + 3, 1, "Załatwienia", true, 0, 5);
                tb.komorkaExcela(MyWorksheet1, rowik + 4, 1, "Pozostało na następny miesiąc", true, 0, 5);
                tb.komorkaExcela(MyWorksheet1, rowik + 5, 1, "Zaległość", true, 6, 0);
                tb.komorkaExcela(MyWorksheet1, rowik + 5, 2, "do 3 miesięcy", true, 0, 4);
                tb.komorkaExcela(MyWorksheet1, rowik + 6, 2, "3-6 miesięcy", true, 0, 4);
                tb.komorkaExcela(MyWorksheet1, rowik + 7, 2, "6-12 miesięcy", true, 0, 4);
                tb.komorkaExcela(MyWorksheet1, rowik + 8, 2, "12-24 miesięcy (do 2 lat)", true, 0, 4);
                tb.komorkaExcela(MyWorksheet1, rowik + 9, 2, "24-36 miesięcy (2-3 lat)", true, 0, 4);
                tb.komorkaExcela(MyWorksheet1, rowik + 10, 2, "36-60 miesięcy (3-5 lat)", true, 0, 4);
                tb.komorkaExcela(MyWorksheet1, rowik + 11, 2, "powyżej 60 miesięcy (powyżej 5 lat)", true, 0, 4);
                DataTable tabelka001 = (DataTable)Session["tabelka002"];
                int licznik = 0;
                foreach (DataRow dR in tabelka001.Rows)
                {
                    
                    for (int i =0; i < 16; i++)
                    {
                       tb. komorkaExcela(MyWorksheet1, rowik , i+7 , dR[i ].ToString().Trim(), true, 0, 0, true, false);

                       
                    }
                     rowik++;
                    licznik++;
                    if (licznik > 11)
                    {
                        break;
                    }
                }

                
                DataTable tabelka004 = (DataTable)Session["tabelka004"];
                MyWorksheet1 = tb.tworzArkuszwExcleBezSedziow(MyExcel.Workbook.Worksheets[2], tabelka004,13,2, 2,6, false);



                
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
        }

        protected void tabela_1()
        {
            if (Session["id_dzialu"] == null)
            {
                return;
            }

            string idDzialu = (string)Session["id_dzialu"];
            if (cl.debug(int.Parse(idDzialu)))
            {
                //cm.log.Info(tenPlik + ": rozpoczęcie tworzenia tabeli 1");
            }
            DataTable tabelka01 = dr.generuj_dane_do_tabeli_sedziowskiej_2019(int.Parse(idDzialu), 1, Date1.Date, Date2.Date, 60, tenPlik);

            Session["tabelka001"] = tabelka01;

            gwTabela1.DataSource = null;
            gwTabela1.DataSourceID = null;
            gwTabela1.DataSource = tabelka01;
            gwTabela1.DataBind();
        }

        private DataTable tabela_2()
        {
            //dane do tabeli sumującej po tabelą nr 1
            DataTable tabelka01 = new DataTable();
            try
            {
                tabelka01 = dr.generuj_dane_do_tabeli_wierszy2018(Date1.Date, Date2.Date, (string)Session["id_dzialu"], 2, 20, 18, tenPlik);
                Session["tabelka002"] = tabelka01;
                //row 1
            }
            catch (Exception ex)
            {
                cm.log.Error(tenPlik + " " + ex.Message);
            }
            return tabelka01;
        }
        private void Tabela_4()
        {

            try
            {
                DataTable tabelka04 = dr.generuj_dane_do_tabeli_wierszy2018(Date1.Date, Date2.Date, ((string)Session["id_dzialu"]), 4, 11, 2, tenPlik);
                Session["tabelka004"] = tabelka04;

                // wypełnianie danych labeli
                tab_4_w01_c01.Text = tabelka04.Rows[0][0].ToString().Trim();
                tab_4_w01_c02.Text = tabelka04.Rows[0][1].ToString().Trim();

                tab_4_w02_c01.Text = tabelka04.Rows[1][0].ToString().Trim();
                tab_4_w02_c02.Text = tabelka04.Rows[1][1].ToString().Trim();

                tab_4_w03_c01.Text = tabelka04.Rows[2][0].ToString().Trim();
                tab_4_w03_c02.Text = tabelka04.Rows[2][1].ToString().Trim();

                tab_4_w04_c01.Text = tabelka04.Rows[3][0].ToString().Trim();
                tab_4_w04_c02.Text = tabelka04.Rows[3][1].ToString().Trim();

                tab_4_w05_c01.Text = tabelka04.Rows[4][0].ToString().Trim();
                tab_4_w05_c02.Text = tabelka04.Rows[4][1].ToString().Trim();

                tab_4_w06_c01.Text = tabelka04.Rows[5][0].ToString().Trim();
                tab_4_w06_c02.Text = tabelka04.Rows[5][1].ToString().Trim();

                tab_4_w07_c01.Text = tabelka04.Rows[6][0].ToString().Trim();
                tab_4_w07_c02.Text = tabelka04.Rows[6][1].ToString().Trim();

                tab_4_w08_c01.Text = tabelka04.Rows[7][0].ToString().Trim();
                tab_4_w08_c02.Text = tabelka04.Rows[7][1].ToString().Trim();

                tab_4_w09_c01.Text = tabelka04.Rows[8][0].ToString().Trim();
                tab_4_w09_c02.Text = tabelka04.Rows[8][1].ToString().Trim();

                tab_4_w10_c01.Text = tabelka04.Rows[9][0].ToString().Trim();
                tab_4_w10_c02.Text = tabelka04.Rows[9][1].ToString().Trim();

                tab_4_w11_c01.Text = tabelka04.Rows[10][0].ToString().Trim();
                tab_4_w11_c02.Text = tabelka04.Rows[10][1].ToString().Trim();
            }
            catch (Exception ex)
            {
                cm.log.Error(tenPlik + " " + ex.Message);
            }

        }

        protected void tabela_5()
        {
            string idDzialu = (string)Session["id_dzialu"];
            if (cl.debug(int.Parse(idDzialu)))
            {
                cm.log.Info(tenPlik + ": rozpoczęcie tworzenia tabeli 5");
            }
            Session["tabelka005"] = dr.tworzTabele(int.Parse(idDzialu), 5, Date1.Date, Date2.Date, 40, GridView5, tenPlik);
            GridView5.DataBind();
        }

        protected void naglowekTabeli_gwTabela1(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                string path = Server.MapPath("XMLHeaders") + "\\" + tenPlikNazwa + ".xml";
                xMLHeaders.getHeaderFromXML(path, gwTabela1);
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

        protected void gwTabela1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                storid = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "id").ToString());
            }
        }

        protected void GridView5_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                DataTable table = (DataTable)Session["tabelka005"];
                tb.makeSumRow(table, e, 1);
            }
        }
        protected void GridView5_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                DataTable dT = (DataTable)Session["header_05"];
                tb.makeHeader(dT, GridView5);
            }
        }
        public void AddNewRow(object sender, GridViewRowEventArgs e)
        {
            DataTable tabelka01 = tabela_2();

            GridView GridView1 = (GridView)sender;
           

            string idtabeli = "2";

            GridView1.Controls[0].Controls.AddAt(e.Row.RowIndex + rowIndex, tb.PodsumowanieTabeli((DataTable)Session["tabelka001"], 22, "normal borderTopLeft gray"));

            int idWiersza = 1;
            GridView1.Controls[0].Controls.AddAt(e.Row.RowIndex + rowIndex, tb.wierszTabeliOGLK(tabelka01, 16, idWiersza, idtabeli, "Zaległość z poprzedniego miesiąca", 7, 0, "", "borderAll center"));

            idWiersza = 2;
            GridView1.Controls[0].Controls.AddAt(e.Row.RowIndex + rowIndex, tb.wierszTabeliOGLK(tabelka01, 16, idWiersza, idtabeli, "Wpływ", 7, 0, "", "borderAll center"));

            idWiersza = 3;
            GridView1.Controls[0].Controls.AddAt(e.Row.RowIndex + rowIndex, tb.wierszTabeliOGLK(tabelka01, 16, idWiersza, idtabeli, "Wpływ spraw do rozpoznania przez referendarzy sądowych", 7, 0, "", "borderAll center "));

            idWiersza = 4;
            GridView1.Controls[0].Controls.AddAt(e.Row.RowIndex + rowIndex, tb.wierszTabeliOGLK(tabelka01, 16, idWiersza, idtabeli, "Załatwienia", 7, 0, "", "borderAll center "));

            idWiersza = 5;
            GridView1.Controls[0].Controls.AddAt(e.Row.RowIndex + rowIndex, tb.wierszTabeliOGLK(tabelka01, 16, idWiersza, idtabeli, "Pozostałość na następny miesiąc", 7, 0, "", "borderAll center"));
         
            idWiersza = 6;
            GridView1.Controls[0].Controls.AddAt(e.Row.RowIndex + rowIndex, tb.wierszTabeliOGLK(tabelka01, 16, idWiersza, idtabeli, "do 3 miesięcy", 6, 1, "", "borderAll center", "Zaległość", 7, 1, "borderAll center"));
       
            idWiersza = 7;
            
            GridView1.Controls[0].Controls.AddAt(e.Row.RowIndex + rowIndex, tb.wierszTabeliOGLK(tabelka01, 16, idWiersza, idtabeli, "3 - 6 miesięcy", 6, 0, "", "borderAll center"));

            idWiersza = 8;
            GridView1.Controls[0].Controls.AddAt(e.Row.RowIndex + rowIndex, tb.wierszTabeliOGLK(tabelka01, 16, idWiersza, idtabeli, "6-12 miesięcy", 6, 0, "", "borderAll center"));
            idWiersza = 9;
            GridView1.Controls[0].Controls.AddAt(e.Row.RowIndex + rowIndex, tb.wierszTabeliOGLK(tabelka01, 16, idWiersza, idtabeli, "12-24 miesięcy (do 2 lat)", 6, 0, "", "borderAll center"));
            idWiersza = 10;
            GridView1.Controls[0].Controls.AddAt(e.Row.RowIndex + rowIndex, tb.wierszTabeliOGLK(tabelka01, 16, idWiersza, idtabeli, "24-36 miesięcy (2-3 lat)", 6, 0, "", "borderAll center"));
            idWiersza = 11;
            GridView1.Controls[0].Controls.AddAt(e.Row.RowIndex + rowIndex, tb.wierszTabeliOGLK(tabelka01, 16, idWiersza, idtabeli, "36-60 miesięcy (3-5 lat)", 6, 0, "", "borderAll center"));
            idWiersza = 12;
            GridView1.Controls[0].Controls.AddAt(e.Row.RowIndex + rowIndex, tb.wierszTabeliOGLK(tabelka01, 16, idWiersza, idtabeli, "powyżej 60 miesięcy (powyżej 5 lat)", 6, 0, "", "borderAll center"));
        }

        protected void GridView3_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                DataTable dT = (DataTable)Session["header_03"];
                tb.makeHeader(dT, GridView3);
            }
        }

        protected void GridView3_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                DataTable table = (DataTable)Session["tabelka003"];
                tb.makeSumRow(table, e, 1);
            }
        }

        protected void makeHeader()
        {
            System.Web.UI.WebControls.GridView sn = new System.Web.UI.WebControls.GridView();

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

            DataTable dT_06 = new DataTable();
            dT_06.Columns.Clear();
            dT_06.Columns.Add("Column1", typeof(string));
            dT_06.Columns.Add("Column2", typeof(string));
            dT_06.Columns.Add("Column3", typeof(string));
            dT_06.Columns.Add("Column4", typeof(string));
            dT_06.Columns.Add("Column5", typeof(string));
            dT_06.Columns.Add("Column6", typeof(string));

            dT_01.Clear();
            dT_01.Rows.Add(new Object[] { "1", "C", "1", "1" });
            dT_01.Rows.Add(new Object[] { "1", "Ns", "1", "1" });
            dT_01.Rows.Add(new Object[] { "1", "Nsm", "1", "1" });
            dT_01.Rows.Add(new Object[] { "1", "Co", "1", "1" });
            dT_01.Rows.Add(new Object[] { "1", "Nmo", "1", "1" });
            dT_01.Rows.Add(new Object[] { "1", "Cps", "1", "1" });
            dT_01.Rows.Add(new Object[] { "1", "Nkd", "1", "1" });
            dT_01.Rows.Add(new Object[] { "1", "Łącznie", "1", "1" });
            dT_01.Rows.Add(new Object[] { "2", "Ruch spraw", "1", "2" });
            dT_01.Rows.Add(new Object[] { "2", "sprawy wg. repertoriów lub wykazów", "8", "1" });
            Session["header_01"] = dT_01;

            #endregion tabela  1 (wierszowa)

            #region tabela  2 ()

            dT_02.Clear();

            dT_02.Rows.Add(new Object[] { "1", "1", "2", "1", "h", "165" });//
            dT_02.Rows.Add(new Object[] { "1", "2", "1", "1", "h", "130" });//
            dT_02.Rows.Add(new Object[] { "1", "3", "1", "1", "h", "45" });//
            dT_02.Rows.Add(new Object[] { "1", "4", "1", "1", "h", "45" });//
            dT_02.Rows.Add(new Object[] { "1", "5", "1", "1", "h", "45" });//
            dT_02.Rows.Add(new Object[] { "1", "6", "1", "1", "h", "45" });//
            dT_02.Rows.Add(new Object[] { "1", "7", "1", "1", "h", "45" });//
            dT_02.Rows.Add(new Object[] { "1", "8", "1", "1", "h", "45" });//
            dT_02.Rows.Add(new Object[] { "1", "9", "1", "1", "h", "45" });//
            dT_02.Rows.Add(new Object[] { "1", "10", "1", "1", "h", "45" });//
            dT_02.Rows.Add(new Object[] { "1", "11", "1", "1", "h", "45" });//
            dT_02.Rows.Add(new Object[] { "1", "12", "1", "1", "h", "45" });//
            dT_02.Rows.Add(new Object[] { "1", "13", "1", "1", "h", "45" });//
            dT_02.Rows.Add(new Object[] { "1", "14", "1", "1", "h", "45" });//
            dT_02.Rows.Add(new Object[] { "1", "15", "1", "1", "h", "45" });//
            dT_02.Rows.Add(new Object[] { "1", "16", "1", "1", "h", "45" });//
            dT_02.Rows.Add(new Object[] { "1", "17", "1", "1", "h", "45" });//
            dT_02.Rows.Add(new Object[] { "1", "18", "1", "1", "h", "45" });//
            dT_02.Rows.Add(new Object[] { "1", "19", "1", "1", "h", "45" });//
            dT_02.Rows.Add(new Object[] { "1", "20", "1", "1", "h", "45" });//
            dT_02.Rows.Add(new Object[] { "1", "21", "1", "1", "h", "45" });//

            dT_02.Rows.Add(new Object[] { "2", "pub- liczne", "1", "1", "h" });//
            dT_02.Rows.Add(new Object[] { "2", "prywatki", "1", "1", "h" });//
            dT_02.Rows.Add(new Object[] { "2", "wyrokiem", "1", "1", "h", "45" });//
            dT_02.Rows.Add(new Object[] { "2", "nakazem", "1", "1", "h", "45" });//
            dT_02.Rows.Add(new Object[] { "2", "inne", "1", "1", "h", "45" });//
            dT_02.Rows.Add(new Object[] { "2", "Ko", "1", "1", "h", "45" });//
            dT_02.Rows.Add(new Object[] { "2", "1Ko", "1", "1", "h", "45" });//;

            dT_02.Rows.Add(new Object[] { "3", "K", "1", "2", "h", "10" });//
            dT_02.Rows.Add(new Object[] { "3", "z tego  ", "2", "1", "h", "90" });//
            dT_02.Rows.Add(new Object[] { "3", "załawione", "3", "1", "h", "135" });//
            dT_02.Rows.Add(new Object[] { "3", "Ko", "1", "2", "h", "45" });//
            dT_02.Rows.Add(new Object[] { "3", "z tego", "2", "1", "h", "90" });//
            dT_02.Rows.Add(new Object[] { "3", "Kp", "1", "2", "h", "45" });//
            dT_02.Rows.Add(new Object[] { "3", "W", "1", "2", "h", "45" });//
            dT_02.Rows.Add(new Object[] { "3", "w tym zała- twione na podst. art.. 93 kpw (NK)", "1", "2", "h", "45" });//

            dT_02.Rows.Add(new Object[] { "4", "Dni rozp- rawy", "1", "3", "h", "45" });//
            dT_02.Rows.Add(new Object[] { "4", "Dni posie- dzeń", "1", "3", "h", "45" });//
            dT_02.Rows.Add(new Object[] { "4", "Sprawy Karne", "10", "1", "h", "450" });//
            dT_02.Rows.Add(new Object[] { "4", "Sprawy Wykrocze- niowe", "2", "1", "h", "90" });//
            dT_02.Rows.Add(new Object[] { "4", "Kop", "1", "3", "h", "45" });//
            dT_02.Rows.Add(new Object[] { "4", "Sprawy karne i wykro- cze- niowe", "1", "3", "h", "45" });//

            dT_02.Rows.Add(new Object[] { "5", "L.p.", "1", "4", "h", "35" });

            dT_02.Rows.Add(new Object[] { "5", "Sędzia", "1", "4", "h", "130" });//Choroby I urlopy w dniach roboczych
            dT_02.Rows.Add(new Object[] { "5", "Choroby i urlopy w dniach roboczych", "1", "4", "h", "130" });//

            dT_02.Rows.Add(new Object[] { "5", "Ilość sesji", "2", "1", "h", "90" });
            dT_02.Rows.Add(new Object[] { "5", "Ilość spraw skier. na wokandy", "1", "4", "h", "45" });//

            dT_02.Rows.Add(new Object[] { "5", "Załatwienia", "14", "1", "h", "45" });//
            dT_02.Rows.Add(new Object[] { "5", "Med", "1", "4", "h", "45" });
            dT_02.Rows.Add(new Object[] { "5", "Sprawy zawie- szone", "1", "4", "h", "45" });
            Session["header_02"] = dT_02;

            #endregion tabela  2 ()

            #region tabela  3 ()

            dT_03.Clear();

            dT_03.Rows.Add(new Object[] { "1", "K", "1", "1", "h" });
            dT_03.Rows.Add(new Object[] { "1", "Ko", "1", "1", "h" });
            dT_03.Rows.Add(new Object[] { "1", "Kp", "1", "1", "h" });

            dT_03.Rows.Add(new Object[] { "1", "W   ", "1", "1", "h" });
            dT_03.Rows.Add(new Object[] { "1", "Kop", "1", "1", "h" });
            dT_03.Rows.Add(new Object[] { "1", "Ogółem", "1", "1", "h" });

            dT_03.Rows.Add(new Object[] { "2", "L.p.", "1", "2", "h" });
            dT_03.Rows.Add(new Object[] { "2", "Nazwisko i imię", "1", "2", "h" });
            dT_03.Rows.Add(new Object[] { "2", "Wpływ do repertorium/wykazu", "6", "1", "h" });

            Session["header_03"] = dT_03;

            #endregion tabela  3 ()

            #region tabela  4 ()

            dT_04.Clear();

            dT_04.Rows.Add(new Object[] { "1", "1-14 dni", "1", "1", "v" });
            dT_04.Rows.Add(new Object[] { "1", "%", "1", "1", "v" });
            dT_04.Rows.Add(new Object[] { "1", "w tym nieuspra-<br/>wiedliwione", "1", "1" });
            dT_04.Rows.Add(new Object[] { "1", "15-30 dni", "1", "1" });
            dT_04.Rows.Add(new Object[] { "1", "%", "1", "1", "v" });
            dT_04.Rows.Add(new Object[] { "1", "w tym nieuspra-<br/>wiedliwione", "1", "1" });
            dT_04.Rows.Add(new Object[] { "1", "pow. 1 do 3 mies.", "1", "1", "v" });
            dT_04.Rows.Add(new Object[] { "1", "%", "1", "1", "v" });
            dT_04.Rows.Add(new Object[] { "1", "w tym nieuspra-<br/>wiedliwione", "1", "1" });
            dT_04.Rows.Add(new Object[] { "1", "ponad 3 mies.", "1", "1" });
            dT_04.Rows.Add(new Object[] { "1", "%", "1", "1", "v" });
            dT_04.Rows.Add(new Object[] { "1", "w tym nieuspra-<br/>wiedliwione", "1", "1" });

            dT_04.Rows.Add(new Object[] { "1", "1-14 dni", "1", "1", "v" });

            dT_04.Rows.Add(new Object[] { "1", "15-30 dni", "1", "1" });

            dT_04.Rows.Add(new Object[] { "1", "pow. 1 do 3 mies.", "1", "1", "v" });

            dT_04.Rows.Add(new Object[] { "1", "ponad 3 mies.", "1", "1" });

            dT_04.Rows.Add(new Object[] { "1", "L", "1", "1", "v" });
            dT_04.Rows.Add(new Object[] { "1", "%", "1", "1" });

            #endregion tabela  4 ()

            #region tabela  5 ()

            dT_05.Clear();

            dT_05.Rows.Add(new Object[] { "1", "R", "1", "1", "h" });
            dT_05.Rows.Add(new Object[] { "1", "P", "1", "1", "h" });
            dT_05.Rows.Add(new Object[] { "1", "razem", "1", "1", "h" });

            dT_05.Rows.Add(new Object[] { "1", "K", "1", "1", "h" });
            dT_05.Rows.Add(new Object[] { "1", "Kop", "1", "1", "h" });
            dT_05.Rows.Add(new Object[] { "1", "Ko", "1", "1", "h" });
            dT_05.Rows.Add(new Object[] { "1", "Kp", "1", "1", "h" });
            dT_05.Rows.Add(new Object[] { "1", "W", "1", "1", "h" });
            dT_05.Rows.Add(new Object[] { "1", "Razem", "1", "1", "h" });

            dT_05.Rows.Add(new Object[] { "1", "K", "1", "1", "h" });
            dT_05.Rows.Add(new Object[] { "1", "W", "1", "1", "h" });
            dT_05.Rows.Add(new Object[] { "1", "K", "1", "1", "h" });
            dT_05.Rows.Add(new Object[] { "1", "W", "1", "1", "h" });

            dT_05.Rows.Add(new Object[] { "1", "K", "1", "1", "h" });
            dT_05.Rows.Add(new Object[] { "1", "Kop", "1", "1", "h" });
            dT_05.Rows.Add(new Object[] { "1", "Ko", "1", "1", "h" });
            dT_05.Rows.Add(new Object[] { "1", "Kp", "1", "1", "h" });
            dT_05.Rows.Add(new Object[] { "1", "W", "1", "1", "h" });
            dT_05.Rows.Add(new Object[] { "1", "Razem", "1", "1", "h" });

            dT_05.Rows.Add(new Object[] { "1", "K", "1", "1", "h" });
            dT_05.Rows.Add(new Object[] { "1", "Kop", "1", "1", "h" });
            dT_05.Rows.Add(new Object[] { "1", "Ko", "1", "1", "h" });
            dT_05.Rows.Add(new Object[] { "1", "Kp", "1", "1", "h" });
            dT_05.Rows.Add(new Object[] { "1", "W", "1", "1", "h" });
            dT_05.Rows.Add(new Object[] { "1", "Razem", "1", "1", "h" });

            dT_05.Rows.Add(new Object[] { "2", "L.p.", "1", "2", "h" });
            dT_05.Rows.Add(new Object[] { "2", "Imie i nazwisko sędziego", "1", "2", "h" });
            dT_05.Rows.Add(new Object[] { "2", "Efektywny okres oczekiwania", "1", "2", "h" });
            dT_05.Rows.Add(new Object[] { "2", "Ilość sesji ", "3", "1", "H" });
            dT_05.Rows.Add(new Object[] { "2", "Ilość wyznaczonych ", "6", "1", "H" });
            dT_05.Rows.Add(new Object[] { "2", "Ilość odroczeń ", "2", "1", "H" });

            dT_05.Rows.Add(new Object[] { "2", "Ilość przerw ", "2", "1", "H" });
            dT_05.Rows.Add(new Object[] { "2", "Załatwienia ", "6", "1", "H" });
            dT_05.Rows.Add(new Object[] { "2", "Średnio miesię- cznie ", "1", "2", "H" });
            dT_05.Rows.Add(new Object[] { "2", "Średnio miesię- cznie K", "1", "2", "H" });
            dT_05.Rows.Add(new Object[] { "2", "Stan referatów ", "6", "1", "H" });
            Session["header_05"] = dT_05;

            dT_06.Clear();
            dT_06.Rows.Add(new Object[] { "1", "K", "1", "1", "h" });
            dT_06.Rows.Add(new Object[] { "1", "Ko", "1", "1", "h" });
            dT_06.Rows.Add(new Object[] { "1", "Kop", "1", "1", "h" });
            dT_06.Rows.Add(new Object[] { "1", "Kp", "1", "1", "h" });
            dT_06.Rows.Add(new Object[] { "1", "W", "1", "1", "h" });
            dT_06.Rows.Add(new Object[] { "1", "Razem", "1", "1", "h" });

            dT_06.Rows.Add(new Object[] { "2", "L.p.", "1", "2", "h" });
            dT_06.Rows.Add(new Object[] { "2", "Imie i nazwisko sędziego", "1", "2", "h" });
            dT_06.Rows.Add(new Object[] { "2", "Stan referatów ", "6", "1", "H" });

            Session["header_06"] = dT_06;

            #endregion tabela  5 ()
        }
    }
}