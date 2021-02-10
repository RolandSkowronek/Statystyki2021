﻿using OfficeOpenXml;
using System;
using System.Data;
using System.Globalization;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Statystyki_2018
{
    public partial class oktp : System.Web.UI.Page
    {
        public Class1 cl = new Class1();
        public common cm = new common();
        public tabele tb = new tabele();
        public dataReaders dr = new dataReaders();

        private const string tenPlik = "oktp.aspx";

        private int storid = 0;
        private int rowIndex = 1;

        protected void Page_Load(object sender, EventArgs e)
        {
            string idWydzial = Request.QueryString["w"];
            try
            {
                if (idWydzial == null)
                {
                    Server.Transfer("default.aspx");
                    return;
                }
                bool dost = cm.dostep(idWydzial, (string)Session["identyfikatorUzytkownika"]);
                if (!dost)
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
                        Session["tabelka001"] = null;
                        Session["tabelka002"] = null;

                        var fileContents = System.IO.File.ReadAllText(Server.MapPath(@"~//version.txt"));    // file read with version
                        this.Title = "Statystyki " + fileContents.ToString().Trim();

                        odswiez();
                        makeLabels();
                    }
                }
            }
            catch
            {
                // Server.Transfer("default.aspx");
            }
        }// end of Page_Load

        protected void odswiez()
        {
            string idDzialu = (string)Session["id_dzialu"];
            string txt = string.Empty; //
            try
            {
                bool debug = cl.debug(int.Parse(idDzialu));
                cm.log.Info(tenPlik + ": rozpoczęcie tworzenia tabeli 2", debug);

                DataTable tabelka02 = dr.generuj_dane_do_tabeli_wierszy2018(Date1.Date, Date2.Date, (string)Session["id_dzialu"], 2, 11, 22, tenPlik);
                Session["tabelka002"] = tabelka02;

                cm.log.Info(tenPlik + ": rozpoczęcie tworzenia tabeli 1", debug);
                Session["tabelka001"] = dr.tworzTabele(int.Parse(idDzialu), 1, Date1.Date, Date2.Date, 23, GridView1, tenPlik);
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
                Label11.Visible = cl.debug(int.Parse(idDzialu));
                infoLabel2.Visible = cl.debug(int.Parse(idDzialu));
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

        private DataTable Naglowek_1()
        {
            DataTable dT_01 = new DataTable();
            dT_01.Columns.Clear();
            dT_01.Columns.Add("Column1", typeof(string));
            dT_01.Columns.Add("Column2", typeof(string));
            dT_01.Columns.Add("Column3", typeof(string));
            dT_01.Columns.Add("Column4", typeof(string));
            dT_01.Columns.Add("Column5", typeof(string));
            dT_01.Columns.Add("Column6", typeof(string));

            dT_01.Clear();
            for (int i = 0; i < 2; i++)
            {
                dT_01.Rows.Add(new Object[] { "1", "P", "1", "1" });
                dT_01.Rows.Add(new Object[] { "1", "Np.", "1", "1" });
                dT_01.Rows.Add(new Object[] { "1", "Po", "1", "1" });
                dT_01.Rows.Add(new Object[] { "1", "U", "1", "1" });
                dT_01.Rows.Add(new Object[] { "1", "Uo", "1", "1" });
                dT_01.Rows.Add(new Object[] { "1", "Pz", "1", "1" });
                dT_01.Rows.Add(new Object[] { "1", "Razem", "1", "1" });
            }

            dT_01.Rows.Add(new Object[] { "2", "L.p.", "1", "2" });

            dT_01.Rows.Add(new Object[] { "2", " Imię i nazwisko sędziego", "1", "2" });
            dT_01.Rows.Add(new Object[] { "2", "Ilość sesji", "1", "2" });

            dT_01.Rows.Add(new Object[] { "2", "Liczba Przesł.osób", "1", "2" });

            dT_01.Rows.Add(new Object[] { "2", "Wpływ", "7", "1" });
            dT_01.Rows.Add(new Object[] { "2", "Załatwienia", "7", "1" });
            dT_01.Rows.Add(new Object[] { "2", "Absencja w diach roboczych", "1", "2" });

            return dT_01;
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
                }
                else
                {
                    tabela2Label.Text = "Sprawozdanie z ruchu spraw w za okres od " + Date1.Text + " do  " + Date2.Text;
                }
            }
            catch
            {
            }
        }

        protected void Button3_Click(object sender, EventArgs e)
        {
            string path = Server.MapPath("Template") + "\\oktp.xlsx";
            FileInfo existingFile = new FileInfo(path);

            string download = Server.MapPath("Template") + @"\oktp";

            FileInfo fNewFile = new FileInfo(download + "_.xlsx");

            using (ExcelPackage MyExcel = new ExcelPackage(existingFile))
            {
                // pierwsza

                int rowik = 0;

                ExcelWorksheet MyWorksheet1 = MyExcel.Workbook.Worksheets[1];

                DataTable table = (DataTable)Session["tabelka001"];

                MyWorksheet1 = tb.tworzArkuszwExcle(MyExcel.Workbook.Worksheets[1], table, 18, 0, 5, true, true, false, false, false);

                int iloscWierszy = GridView1.Rows.Count;
                if (iloscWierszy > 0)
                {
                    for (int i = 0; i < iloscWierszy; i++)
                    {
                        TextBox txt = ((TextBox)GridView1.Rows[i].Cells[15].FindControl("TextBox1"));
                        if (txt != null)
                        {
                            string tekst = txt.Text;
                            tb.komorkaExcela(MyWorksheet1, i + 5, 19, tekst, false, 0, 0);
                            //wpisz text
                        }
                    }
                }
                rowik = table.Rows.Count + 3;

                // pod tabela z tebeli nr 2

                rowik = rowik + 3;
                tb.komorkaExcela(MyWorksheet1, rowik, 1, "Pozostało z poprzedniego okresu", true, 0, 3);
                tb.komorkaExcela(MyWorksheet1, rowik + 1, 1, "Wpłynęło", true, 0, 3);
                tb.komorkaExcela(MyWorksheet1, rowik + 2, 1, "Załatwiono", true, 0, 3);
                tb.komorkaExcela(MyWorksheet1, rowik + 3, 1, "Pozostało na następny okres", true, 0, 3);

                tb.komorkaExcela(MyWorksheet1, rowik + 4, 1, "Zaległości", true, 4, 1);

                tb.komorkaExcela(MyWorksheet1, rowik + 4, 3, "3 - 6 miesięcy", true, 0, 1);
                tb.komorkaExcela(MyWorksheet1, rowik + 5, 3, "6 - 12 miesięcy", true, 0, 1);
                tb.komorkaExcela(MyWorksheet1, rowik + 6, 3, "od 1 do 2 lat", true, 0, 1);
                tb.komorkaExcela(MyWorksheet1, rowik + 7, 3, "od 2 do 3 lat", true, 0, 1);
                tb.komorkaExcela(MyWorksheet1, rowik + 8, 3, "powyżej 3 lat", true, 0, 1);

                DataTable tabelka001 = (DataTable)Session["tabelka002"];
                for (int i = 0; i < 9; i++)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        try
                        {
                            MyWorksheet1.Cells[rowik + i, j + 4].Value = tabelka001.Rows[i][j].ToString();
                            MyWorksheet1.Cells[rowik + i, j + 4].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin, System.Drawing.Color.Black);
                        }
                        catch
                        { }
                    }
                }

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
                tb.makeHeader(Naglowek_1(), GridView1);
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
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                tb.makeSumRow((DataTable)Session["tabelka001"], e);
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                storid = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "id").ToString());
            }
        }

        // podtabela

        private GridViewRow wierszTabeli3(DataTable tabelka01, int idWiersza, string idtabeli, string tekst)
        {
            // nowy wiersz

            GridViewRow NewTotalRow = new GridViewRow(0, 0, DataControlRowType.DataRow, DataControlRowState.Insert);

            NewTotalRow.Cells.Add(tb.cela(tekst, 1, 2, "borderTopLeft  "));

            for (int i = 1; i < 8; i++)
            {
                NewTotalRow.Cells.Add(tb.cela("<a  href=\"javascript: openPopup('popup.aspx?sesja=" + idWiersza.ToString().Trim() + "!2!" + i.ToString().Trim() + "!3')\">" + tabelka01.Rows[idWiersza - 1][i].ToString().Trim() + "</a>", 1, 1, "borderTopLeft"));
            }

            return NewTotalRow;
        }

        private GridViewRow wierszTabela2(DataTable tabelka01, int idWiersza, string idtabeli, string tekst)
        {
            // nowy wiersz
            GridViewRow NewTotalRow = new GridViewRow(0, 0, DataControlRowType.DataRow, DataControlRowState.Insert);
            NewTotalRow.Cells.Add(tb.cela("Zaległości", 5, 2, "borderTopLeft col_240"));
            NewTotalRow.Cells.Add(tb.cela(tekst, 1, 2, "borderTopLeft  "));

            for (int i = 1; i < 8; i++)
            {
                NewTotalRow.Cells.Add(tb.cela("<a  href=\"javascript: openPopup('popup.aspx?sesja=" + idWiersza.ToString().Trim() + "!2!" + i.ToString().Trim() + "!3')\">" + tabelka01.Rows[idWiersza - 1][i].ToString().Trim() + "</a>", 1, 1, "borderTopLeft"));
            }

            return NewTotalRow;
        }

        public void AddNewRow(object sender, GridViewRowEventArgs e)
        {
            DataTable tabelka01 = (DataTable)Session["tabelka002"];
            DataTable tabelka02 = (DataTable)Session["tabelka001"];

            GridView GridView1 = (GridView)sender;
            GridViewRow NewTotalRow = new GridViewRow(0, 0, DataControlRowType.DataRow, DataControlRowState.Insert);
            try
            {
                GridView1.Controls[0].Controls.AddAt(e.Row.RowIndex + rowIndex, tb.PodsumowanieTabeli(tabelka02, 17, "borderAll center"));
            }
            catch
            {
            }

            string idtabeli = "2";

            // nowy wiersz
            int idWiersza = 1;
            GridView1.Controls[0].Controls.AddAt(e.Row.RowIndex + rowIndex, tb.wierszTabeli(tabelka01, 8, idWiersza, idtabeli, "Pozostało z poprzedniego okresu", 4, 1, "", "borderAll center"));

            idWiersza = 2;
            GridView1.Controls[0].Controls.AddAt(e.Row.RowIndex + rowIndex, tb.wierszTabeli(tabelka01, 8, idWiersza, idtabeli, "Wpłynęło", 4, 1, "", "borderAll center"));

            // nowy wiersz
            idWiersza = 3;
            GridView1.Controls[0].Controls.AddAt(e.Row.RowIndex + rowIndex, tb.wierszTabeli(tabelka01, 8, idWiersza, idtabeli, "Załatwiono", 4, 1, "", "borderAll center"));

            // nowy wiersz
            idWiersza = 4;
            GridView1.Controls[0].Controls.AddAt(e.Row.RowIndex + rowIndex, tb.wierszTabeli(tabelka01, 8, idWiersza, idtabeli, "Pozostało na następny okres", 4, 1, "", "borderAll center"));

            // nowy wiersz
            idWiersza = 5;
            GridView1.Controls[0].Controls.AddAt(e.Row.RowIndex + rowIndex, wierszTabela2(tabelka01, idWiersza, idtabeli, "3-6 m-cy"));
            // nowy wiersz
            idWiersza = 6;
            GridView1.Controls[0].Controls.AddAt(e.Row.RowIndex + rowIndex, wierszTabeli3(tabelka01, idWiersza, idtabeli, "6 - 12 miesięcy"));

            // nowy wiersz
            idWiersza = 7;
            GridView1.Controls[0].Controls.AddAt(e.Row.RowIndex + rowIndex, wierszTabeli3(tabelka01, idWiersza, idtabeli, "od 1 do 2 lat"));

            idWiersza = 8;
            GridView1.Controls[0].Controls.AddAt(e.Row.RowIndex + rowIndex, wierszTabeli3(tabelka01, idWiersza, idtabeli, "od 2 do 3 lat"));
            //wierszTabelia3
            idWiersza = 9;
            GridView1.Controls[0].Controls.AddAt(e.Row.RowIndex + rowIndex, wierszTabeli3(tabelka01, idWiersza, idtabeli, "powyżej 3 lat"));
        }

        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {
        }
    }
}