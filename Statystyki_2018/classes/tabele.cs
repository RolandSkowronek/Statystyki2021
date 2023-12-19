using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web.UI.WebControls;

namespace Statystyki_2018
{
    public class tabele
    {
        private common cm = new common();
        private dataReaders dr = new dataReaders();

        public TableCell HeaderCell_(string text, int columns, int rows)
        {
            TableCell HeaderCell = new TableCell();
            HeaderCell.Text = text;
            HeaderCell.ColumnSpan = columns;
            HeaderCell.RowSpan = rows;
            return HeaderCell;
        }

        public GridViewRow Grw(object sender)
        {
            GridViewRow HeaderGridRow = null;
            GridView HeaderGrid = (GridView)sender;
            HeaderGridRow = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert);
            HeaderGridRow.Font.Size = 7;
            HeaderGridRow.HorizontalAlign = HorizontalAlign.Center;
            HeaderGridRow.VerticalAlign = VerticalAlign.Top;
            return HeaderGridRow;
        }

        public DataTable makeSumRow(DataTable table, int ilKolumn)
        {
            DataTable wyjsciowa = new DataTable();
            for (int i = 0; i < ilKolumn; i++)
            {
                wyjsciowa.Columns.Add("d_" + i.ToString("D2"), typeof(double));
            }
            DataTable tabelka = tabellaLiczbowa(table);
            DataColumnCollection col = tabelka.Columns;

            object sumObject;
            DataRow wiersz = wyjsciowa.NewRow();
            for (int i = 1; i < ilKolumn; i++)
            {
                try
                {
                    string idkolumny = "d_" + (i).ToString("D2");

                    if (col.Contains(idkolumny))
                    {
                        sumObject = tabelka.Compute("Sum(" + idkolumny + ")", "");
                        wiersz[idkolumny] = double.Parse(sumObject.ToString());
                    }
                }
                catch (Exception ex)
                {
                    cm.log.Error("sumowanie w stopce : " + ex.Message);
                }
            }
            wyjsciowa.Rows.Add(wiersz);
            return wyjsciowa;
        }

        public DataTable makeSumRow(DataTable table, int ilKolumn, int dlugoscLinii)
        {
            //stworzenie tabeli przejściowej
            DataTable tabelaRobocza = new DataTable();
            for (int i = 1; i <= dlugoscLinii; i++)
            {
                string nazwaKolumny = "d_" + i.ToString("D2");
                tabelaRobocza.Columns.Add(nazwaKolumny, typeof(double));
            }

            foreach (DataRow jedenWiersz in table.Rows)
            {
                int index = 1;
                DataRow nowyWiersz = tabelaRobocza.NewRow();
                for (int i = 1; i <= ilKolumn; i++)
                {
                    string nazwaKolumny = "d_" + i.ToString("D2");
                    string nazwaKolumnySumy = "d_" + index.ToString("D2");
                    double wartosc = double.Parse(jedenWiersz[nazwaKolumny].ToString());
                    double wartoscSumowana = 0;
                    try
                    {
                        wartoscSumowana = double.Parse(nowyWiersz[nazwaKolumnySumy].ToString());
                    }
                    catch
                    { }
                    wartoscSumowana = wartoscSumowana + wartosc;
                    nowyWiersz[nazwaKolumnySumy] = wartoscSumowana;
                    if (index == dlugoscLinii)
                    {
                        index = 0;
                    }
                    index++;
                }
                tabelaRobocza.Rows.Add(nowyWiersz);
            }

            object sumObject;
            DataRow wiersz = tabelaRobocza.NewRow();
            for (int i = 1; i <= tabelaRobocza.Columns.Count; i++)
            {
                try
                {
                    string idkolumny = "d_" + (i).ToString("D2");

                    sumObject = tabelaRobocza.Compute("Sum(" + idkolumny + ")", "");
                    wiersz[idkolumny] = double.Parse(sumObject.ToString());
                }
                catch (Exception ex)
                {
                    cm.log.Error("sumowanie w stopce : " + ex.Message);
                }
            }
            tabelaRobocza.Rows.Clear();
            tabelaRobocza.Rows.Add(wiersz);
            return tabelaRobocza;
        }

        public void makeHeader(System.Web.UI.WebControls.GridView GridViewName, DataTable dT, System.Web.UI.WebControls.GridView GridViewX)
        {
            try
            {
                int row = 0;
                TableCell HeaderCell = new TableCell();
                GridViewRow HeaderGridRow = null;
                string hv = "h";
                Style stl = new Style();
                foreach (DataRow dR in dT.Rows)
                {
                    if (int.Parse(dR[0].ToString().Trim()) > row)
                    {
                        GridView HeaderGrid = (GridView)GridViewName;
                        HeaderGridRow = Grw(GridViewName);
                        row = int.Parse(dR[0].ToString().Trim());
                        try
                        {
                            hv = dR[4].ToString().Trim();
                        }
                        catch
                        { }
                    }
                    if (hv == "v")
                    {
                        stl.CssClass = "verticaltext";
                    }
                    else
                    {
                        stl.CssClass = "horizontaltext";
                    }

                    HeaderCell = new TableCell();
                    HeaderCell.Text = dR[1].ToString().Trim();
                    HeaderCell.Style.Clear();
                    HeaderCell.ApplyStyle(stl);
                    HeaderCell.ColumnSpan = int.Parse(dR[2].ToString().Trim());
                    HeaderCell.RowSpan = int.Parse(dR[3].ToString().Trim());
                    HeaderGridRow.Cells.Add(HeaderCell);
                    GridViewX.Controls[0].Controls.AddAt(0, HeaderGridRow);
                }
            }
            catch (Exception ex)
            {
                cm.log.Error("tabele.dll->makeHeader: " + ex.Message);
            } // end of try
        }

        public void makeHeader(DataTable dT, System.Web.UI.WebControls.GridView GridViewX)
        {
            System.Web.UI.WebControls.GridView sn = new System.Web.UI.WebControls.GridView();
            try
            {
                int row = 0;
                TableCell HeaderCell = new TableCell();
                GridViewRow HeaderGridRow = null;
                string hv = "h";
                Style stl = new Style();
                foreach (DataRow dR in dT.Rows)
                {
                    if (int.Parse(dR[0].ToString().Trim()) > row)
                    {
                        GridView HeaderGrid = (GridView)sn;
                        HeaderGridRow = Grw(sn);
                        row = int.Parse(dR[0].ToString().Trim());
                        try
                        {
                            hv = dR[4].ToString().Trim();
                        }
                        catch
                        { }
                    }
                    if (hv == "v")
                    {
                        stl.CssClass = "verticaltext";
                    }
                    else
                    {
                        stl.CssClass = "horizontaltext";
                    }

                    HeaderCell = new TableCell();
                    HeaderCell.Text = dR[1].ToString().Trim();
                    HeaderCell.Style.Clear();
                    HeaderCell.ApplyStyle(stl);
                    HeaderCell.ColumnSpan = int.Parse(dR[2].ToString().Trim());
                    HeaderCell.RowSpan = int.Parse(dR[3].ToString().Trim());
                    HeaderGridRow.Cells.Add(HeaderCell);
                    GridViewX.Controls[0].Controls.AddAt(0, HeaderGridRow);
                }
            }
            catch (Exception ex)
            {
                cm.log.Error("tabele.dll->makeHeader: " + ex.Message);
            } // end of try
        }

        public void makeHeaderT3(DataTable dT, System.Web.UI.WebControls.GridView GridViewX)
        {
            System.Web.UI.WebControls.GridView sn = new System.Web.UI.WebControls.GridView();
            try
            {
                int row = 0;
                TableCell HeaderCell = new TableCell();
                GridViewRow HeaderGridRow = null;
                string hv = "h";
                Style stl = new Style();
                foreach (DataRow dR in dT.Rows)
                {
                    if (int.Parse(dR[0].ToString().Trim()) > row)
                    {
                        GridView HeaderGrid = (GridView)sn;
                        HeaderGridRow = Grw(sn);
                        row = int.Parse(dR[0].ToString().Trim());
                        try
                        {
                            hv = dR[4].ToString().Trim();
                        }
                        catch
                        { }
                    }
                    if (hv == "v")
                    {
                        stl.CssClass = "spetialVertical";
                    }
                    else
                    {
                        stl.CssClass = "horizontaltext ";
                    }

                    HeaderCell = new TableCell();
                    HeaderCell.Text = dR[1].ToString().Trim();
                    HeaderCell.Style.Clear();
                    HeaderCell.ApplyStyle(stl);
                    HeaderCell.ColumnSpan = int.Parse(dR[2].ToString().Trim());
                    HeaderCell.RowSpan = int.Parse(dR[3].ToString().Trim());
                    HeaderGridRow.Cells.Add(HeaderCell);
                    GridViewX.Controls[0].Controls.AddAt(0, HeaderGridRow);
                }
            }
            catch (Exception ex)
            {
                cm.log.Error("tabele.dll->makeHeader: " + ex.Message);
            } // end of try
        }

        public DataTable tabellaLiczbowa(DataTable tabelaWejsciowa)
        {
            if (tabelaWejsciowa == null)
            {
                return null;
            }
            DataTable tabelaRobocza = new DataTable();
            int iloscKolumn = tabelaWejsciowa.Columns.Cast<DataColumn>().Count(c => c.ColumnName.StartsWith("d_"));

            for (int i = 1; i <= iloscKolumn; i++)
            {
                string nazwaKolumny = "d_" + i.ToString("D2");
                tabelaRobocza.Columns.Add(nazwaKolumny, typeof(double));
            }
            foreach (DataRow Drow in tabelaWejsciowa.Rows)
            {
                try
                {
                    try
                    {
                        if (Drow["nazwisko"].ToString().Trim() == "")
                        {
                            continue;
                        }
                    }
                    catch
                    {
                    }

                    DataRow wierszTymczasowy = tabelaRobocza.NewRow();
                    for (int i = 1; i <= iloscKolumn; i++)
                    {
                        string dana = string.Empty;
                        string nazwaKolumny = "d_" + i.ToString("D2");
                        double liczba = 0;
                        dana = Drow[nazwaKolumny].ToString();
                        if (string.IsNullOrEmpty(dana.Trim()))
                        {
                            dana = "0";
                        }
                        else
                        {
                            try
                            {
                                dana = dana.Replace(".", ",");
                                liczba = double.Parse(dana);
                            }
                            catch (Exception ex)
                            {
                                cm.log.Error("tabellaLiczbowa: " + ex.Message + " : " + dana.ToString());
                            }
                        }

                        wierszTymczasowy[nazwaKolumny] = liczba;
                    }
                    tabelaRobocza.Rows.Add(wierszTymczasowy);
                }
                catch (Exception ex)
                {
                    cm.log.Error("tabellaLiczbowa: " + ex.Message);
                }
            }

            return tabelaRobocza;
        }

        public void makeSumRow(DataTable table, GridViewRowEventArgs e, string tenplik)
        {
            makeSumRow(table, e);
        }

        public void makeSumRow(DataTable table, GridViewRowEventArgs e)
        {
            makeSumRow(table, e, 1, "Ogółem");
        }

        public void makeSumRow(DataTable table, GridViewRowEventArgs e, int przesuniecie)
        {
            makeSumRow(table, e, przesuniecie, "Ogółem");
        }

        public void makeSumRow(DataTable table, GridViewRowEventArgs e, int przesuniecie, string razem)
        {
            DataTable tabelka = tabellaLiczbowa(table);
            if (tabelka == null)
            {
                cm.log.Error("Brak danych do sumowania");
                return;
            }
            object sumObject;
            int ilKolumn = e.Row.Cells.Count;
            e.Row.Cells[0 + przesuniecie].Text = razem;
            for (int i = 1; i < e.Row.Cells.Count; i++)
            {
                try
                {
                    string idkolumny = "d_" + (i).ToString("D2");
                    sumObject = tabelka.Compute("Sum(" + idkolumny + ")", "");
                    e.Row.Cells[i + przesuniecie].Text = sumObject.ToString();
                    e.Row.Cells[i + przesuniecie].CssClass = "center normal";
                }
                catch (Exception ex)
                {
                    cm.log.Error("sumowanie w stopce : " + ex.Message);
                }
            }
        }

        public void makeSumRow(DataTable table, GridViewRowEventArgs e, int przesuniecie, int polaczenie)
        {
            DataTable tabelka = tabellaLiczbowa(table);
            if (tabelka == null)
            {
                cm.log.Error("Brak danych do sumowania");
                return;
            }
            object sumObject;
            int ilKolumn = e.Row.Cells.Count;
            e.Row.Cells[0].ColumnSpan = polaczenie;
            e.Row.Cells[0].Text = "Ogółem";
            try
            {
                for (int i = 1; i < polaczenie; i++)
                {
                    e.Row.Cells.RemoveAt(1);
                }
            }
            catch
            { }
            for (int i = 1; i < e.Row.Cells.Count; i++)
            {
                try
                {
                    string idkolumny = "d_" + (i).ToString("D2");
                    sumObject = tabelka.Compute("Sum(" + idkolumny + ")", "");
                    e.Row.Cells[i].Text = sumObject.ToString();
                    e.Row.Cells[i].CssClass = "center normal";
                }
                catch (Exception ex)
                {
                    cm.log.Error("sumowanie w stopce : " + ex.Message);
                }
            }
        }

        public GridViewRow PodsumowanieTabeli(DataTable dane, int iloscKolumn, string cssStyleDlaTabeli)
        {
            DataTable tabelka = tabellaLiczbowa(dane);
            if (tabelka == null)
            {
                cm.log.Error("Brak danych do sumowania");
                return null;
            }
            object sumObject;
            GridViewRow NewTotalRow = new GridViewRow(0, 0, DataControlRowType.DataRow, DataControlRowState.Insert);
            NewTotalRow.Cells.Add(cela("Razem", 1, 2, cssStyleDlaTabeli));
            for (int i = 1; i < iloscKolumn; i++)
            {
                try
                {
                    string idkolumny = "d_" + (i).ToString("D2");
                    cm.log.Info("idkolumny =" + idkolumny);
                    sumObject = tabelka.Compute("Sum(" + idkolumny + ")", "");

                    NewTotalRow.Cells.Add(cela(sumObject.ToString(), 1, 1, cssStyleDlaTabeli));
                }
                catch (Exception ex)
                {
                    cm.log.Error("sumowanie w stopce : " + ex.Message);
                }
            }
            return NewTotalRow;
        }

        public GridViewRow wierszTabeliOGLK(DataTable dane, int iloscKolumn, int idWiersza, string idtabeli, string tekst, int colSpan, int rowSpan, string CssStyleDlaTekstu, string cssStyleDlaTabeli)
        {
            if (dane == null)
            {
                return null;
            }
            GridViewRow NewTotalRow = new GridViewRow(0, 0, DataControlRowType.DataRow, DataControlRowState.Insert);
            NewTotalRow.Cells.Add(cela(tekst, rowSpan, colSpan, CssStyleDlaTekstu));
            DataRow jedenWiersz = dane.Rows[idWiersza - 1];

            for (int i = 1; i < iloscKolumn; i++)
            {
                try
                {
                    string nazwaKolumny = "d_" + i.ToString("D2");
                    NewTotalRow.Cells.Add(cela("<a class='" + CssStyleDlaTekstu + "' href=\"javascript: openPopup('popup.aspx?sesja=" + idWiersza.ToString().Trim() + "!" + idtabeli.ToString().Trim() + "!" + i.ToString().Trim() + "!3')\">" + jedenWiersz[nazwaKolumny].ToString().Trim() + "</a>", 1, 1, cssStyleDlaTabeli));
                }
                catch (Exception exz)
                {
                    cm.log.Error("Podtabela  : " + exz.Message);
                    try
                    {
                        NewTotalRow.Cells.Add(cela("<a class='" + CssStyleDlaTekstu + "' href=\"javascript: openPopup('popup.aspx?sesja=" + idWiersza.ToString().Trim() + "!" + idtabeli.ToString().Trim() + "!" + i.ToString().Trim() + "!3')\">0</a>", 1, 2, cssStyleDlaTabeli));
                    }
                    catch (Exception ex)
                    {
                        cm.log.Error("Podtabela  : " + ex.Message);
                    }
                }

            }
            try
            {

                NewTotalRow.Cells.Add(cela("<a class='" + CssStyleDlaTekstu + "' href=\"javascript: openPopup('popup.aspx?sesja=" + idWiersza.ToString().Trim() + "!" + idtabeli.ToString().Trim() + "!8!3')\">" + jedenWiersz["d_08"].ToString().Trim() + "</a>", 1, 1, cssStyleDlaTabeli));
            }
            catch (Exception exz)
            {
                cm.log.Error("Podtabela  : " + exz.Message);
                try
                {
                    NewTotalRow.Cells.Add(cela("<a class='" + CssStyleDlaTekstu + "' href=\"javascript: openPopup('popup.aspx?sesja=" + idWiersza.ToString().Trim() + "!" + idtabeli.ToString().Trim() + "!8!3')\">0</a>", 1, 1, cssStyleDlaTabeli));
                }
                catch (Exception ex)
                {
                    cm.log.Error("Podtabela  : " + ex.Message);
                }
            }
            return NewTotalRow;
        }// end of

        public GridViewRow wierszTabeliOGLK(DataTable dane, int iloscKolumn, int idWiersza, string idtabeli, string tekst, int colSpan, int rowSpan, string CssStyleDlaTekstu, string cssStyleDlaTabeli, string drugiText, int colSpanDrugi, int rowSpanDrugi, string cssStyleDrugi)
        {
            // nowy wiersz

            GridViewRow NewTotalRow = new GridViewRow(0, 0, DataControlRowType.DataRow, DataControlRowState.Insert);
            NewTotalRow.Cells.Add(cela(drugiText, colSpanDrugi, rowSpanDrugi, cssStyleDrugi));

            NewTotalRow.Cells.Add(cela(tekst, rowSpan, colSpan, CssStyleDlaTekstu));
            DataRow jedenWiersz = dane.Rows[idWiersza - 1];
            for (int i = 1; i < iloscKolumn; i++)
            {
                try
                {
                    string nazwaKolumny = "d_" + i.ToString("D2");
                    NewTotalRow.Cells.Add(cela("<a class='" + CssStyleDlaTekstu + "' href=\"javascript: openPopup('popup.aspx?sesja=" + idWiersza.ToString().Trim() + "!" + idtabeli.ToString().Trim() + "!" + i.ToString().Trim() + "!3')\">" + jedenWiersz[nazwaKolumny].ToString().Trim() + "</a>", 1, 1, cssStyleDlaTabeli));
                }
                catch (Exception ex)
                {
                    cm.log.Error("wierszTabeliGLK Podtabela  : " + ex.Message);
                }
            }
            try
            {

                NewTotalRow.Cells.Add(cela("<a class='" + CssStyleDlaTekstu + "' href=\"javascript: openPopup('popup.aspx?sesja=" + idWiersza.ToString().Trim() + "!" + idtabeli.ToString().Trim() + "!8!3')\">" + jedenWiersz["d_08"].ToString().Trim() + "</a>", 1, 1, cssStyleDlaTabeli));
            }
            catch (Exception ex)
            {
                cm.log.Error("Podtabela  : " + ex.Message);
            }
            return NewTotalRow;
        }



        public GridViewRow wierszTabeliOCZC(DataTable dane, int iloscKolumn, int idWiersza, string idtabeli, string tekst, int colSpan, int rowSpan, string CssStyleDlaTekstu, string cssStyleDlaTabeli)
        {
            if (dane == null)
            {
                return null;
            }
            GridViewRow NewTotalRow = new GridViewRow(0, 0, DataControlRowType.DataRow, DataControlRowState.Insert);
            NewTotalRow.Cells.Add(cela(tekst, rowSpan, colSpan, CssStyleDlaTekstu));
            DataRow jedenWiersz = dane.Rows[idWiersza - 1];

            for (int i = 1; i < iloscKolumn; i++)
            {
                try
                {
                    string nazwaKolumny = "d_" + i.ToString("D2");
                    NewTotalRow.Cells.Add(cela("<a class='" + CssStyleDlaTekstu + "' href=\"javascript: openPopup('popup.aspx?sesja=" + idWiersza.ToString().Trim() + "!" + idtabeli.ToString().Trim() + "!" + i.ToString().Trim() + "!3')\">" + jedenWiersz[nazwaKolumny].ToString().Trim() + "</a>", 1, 2, cssStyleDlaTabeli));
                }
                catch (Exception exz)
                {
                    cm.log.Error("Podtabela  : " + exz.Message);
                    try
                    {
                        NewTotalRow.Cells.Add(cela("<a class='" + CssStyleDlaTekstu + "' href=\"javascript: openPopup('popup.aspx?sesja=" + idWiersza.ToString().Trim() + "!" + idtabeli.ToString().Trim() + "!" + i.ToString().Trim() + "!3')\">0</a>", 1, 2, cssStyleDlaTabeli));
                    }
                    catch (Exception ex)
                    {
                        cm.log.Error("Podtabela  : " + ex.Message);
                    }
                }
              
            }
            try
            {
              
                NewTotalRow.Cells.Add(cela("<a class='" + CssStyleDlaTekstu + "' href=\"javascript: openPopup('popup.aspx?sesja=" + idWiersza.ToString().Trim() + "!" + idtabeli.ToString().Trim() + "!8!3')\">" + jedenWiersz["d_08"].ToString().Trim() + "</a>", 1, 1, cssStyleDlaTabeli));
            }
            catch (Exception exz)
            {
                cm.log.Error("Podtabela  : " + exz.Message);
                try
                {
                    NewTotalRow.Cells.Add(cela("<a class='" + CssStyleDlaTekstu + "' href=\"javascript: openPopup('popup.aspx?sesja=" + idWiersza.ToString().Trim() + "!" + idtabeli.ToString().Trim() + "!8!3')\">0</a>", 1, 1, cssStyleDlaTabeli));
                }
                catch (Exception ex)
                {
                    cm.log.Error("Podtabela  : " + ex.Message);
                }
            }
            return NewTotalRow;
        }// end of

        public GridViewRow wierszTabeliOCZC(DataTable dane, int iloscKolumn, int idWiersza, string idtabeli, string tekst, int colSpan, int rowSpan, string CssStyleDlaTekstu, string cssStyleDlaTabeli, string drugiText, int colSpanDrugi, int rowSpanDrugi, string cssStyleDrugi)
        {
            // nowy wiersz

            GridViewRow NewTotalRow = new GridViewRow(0, 0, DataControlRowType.DataRow, DataControlRowState.Insert);
            NewTotalRow.Cells.Add(cela(drugiText, colSpanDrugi, rowSpanDrugi, cssStyleDrugi));

            NewTotalRow.Cells.Add(cela(tekst, rowSpan, colSpan, CssStyleDlaTekstu));
            DataRow jedenWiersz = dane.Rows[idWiersza - 1];
            for (int i = 1; i < iloscKolumn; i++)
            {
                try
                {
                    string nazwaKolumny = "d_" + i.ToString("D2");
                    NewTotalRow.Cells.Add(cela("<a class='" + CssStyleDlaTekstu + "' href=\"javascript: openPopup('popup.aspx?sesja=" + idWiersza.ToString().Trim() + "!" + idtabeli.ToString().Trim() + "!" + i.ToString().Trim() + "!3')\">" + jedenWiersz[nazwaKolumny].ToString().Trim() + "</a>", 1, 2, cssStyleDlaTabeli));
                }
                catch (Exception ex)
                {
                    cm.log.Error("Podtabela  : " + ex.Message);
                }
            }
            try
            {
               
                NewTotalRow.Cells.Add(cela("<a class='" + CssStyleDlaTekstu + "' href=\"javascript: openPopup('popup.aspx?sesja=" + idWiersza.ToString().Trim() + "!" + idtabeli.ToString().Trim() + "!8!3')\">" + jedenWiersz["d_08"].ToString().Trim() + "</a>", 1, 1, cssStyleDlaTabeli));
            }
            catch (Exception ex)
            {
                cm.log.Error("Podtabela  : " + ex.Message);
            }
            return NewTotalRow;
        }

        //tabele pod dynamicznymi
        public GridViewRow wierszTabeli(DataTable dane, int iloscKolumn, int idWiersza, string idtabeli, string tekst, int colSpan, int rowSpan, string CssStyleDlaTekstu, string cssStyleDlaTabeli, string drugiText, int colSpanDrugi, int rowSpanDrugi, string cssStyleDrugi)
        {
            // nowy wiersz

            GridViewRow NewTotalRow = new GridViewRow(0, 0, DataControlRowType.DataRow, DataControlRowState.Insert);
            NewTotalRow.Cells.Add(cela(drugiText, colSpanDrugi, rowSpanDrugi, cssStyleDrugi));

            NewTotalRow.Cells.Add(cela(tekst, rowSpan, colSpan, CssStyleDlaTekstu));
            DataRow jedenWiersz = dane.Rows[idWiersza - 1];
            for (int i = 1; i < iloscKolumn; i++)
            {
                try
                {
                    string nazwaKolumny = "d_" + i.ToString("D2");
                    NewTotalRow.Cells.Add(cela("<a class='" + CssStyleDlaTekstu + "' href=\"javascript: openPopup('popup.aspx?sesja=" + idWiersza.ToString().Trim() + "!" + idtabeli.ToString().Trim() + "!" + i.ToString().Trim() + "!3')\">" + jedenWiersz[nazwaKolumny].ToString().Trim() + "</a>", 1, 1, cssStyleDlaTabeli));
                }
                catch (Exception ex)
                {
                    cm.log.Error("sumowanie w stopce : " + ex.Message);
                }
            }
            return NewTotalRow;
        }

        public GridViewRow wierszTabeli(DataTable dane, int iloscKolumn, int idWiersza, string idtabeli, string tekst, int colSpan, int rowSpan, string CssStyleDlaTekstu, string cssStyleDlaTabeli)
        {
            if (dane == null)
            {
                return null;
            }
            GridViewRow NewTotalRow = new GridViewRow(0, 0, DataControlRowType.DataRow, DataControlRowState.Insert);
            NewTotalRow.Cells.Add(cela(tekst, rowSpan, colSpan, CssStyleDlaTekstu));
            DataRow jedenWiersz = dane.Rows[idWiersza - 1];
            for (int i = 1; i < iloscKolumn; i++)
            {
                try
                {
                    string nazwaKolumny = "d_" + i.ToString("D2");
                    NewTotalRow.Cells.Add(cela("<a class='" + CssStyleDlaTekstu + "' href=\"javascript: openPopup('popup.aspx?sesja=" + idWiersza.ToString().Trim() + "!" + idtabeli.ToString().Trim() + "!" + i.ToString().Trim() + "!3')\">" + jedenWiersz[nazwaKolumny].ToString().Trim() + "</a>", 1, 1, cssStyleDlaTabeli));
                }
                catch
                {
                    try
                    {
                        NewTotalRow.Cells.Add(cela("<a class='" + CssStyleDlaTekstu + "' href=\"javascript: openPopup('popup.aspx?sesja=" + idWiersza.ToString().Trim() + "!" + idtabeli.ToString().Trim() + "!" + i.ToString().Trim() + "!3')\">0</a>", 1, 1, cssStyleDlaTabeli));
                    }
                    catch (Exception ex)
                    {
                        cm.log.Error("Podtabela  : " + ex.Message);
                    }
                }
            }
            return NewTotalRow;
        }// end of

        public GridViewRow wierszTabeli(DataTable dane, int iloscKolumn, int idWiersza, string idtabeli, string tekst, int colSpan, int rowSpan, string CssStyleDlaTekstu, string cssStyleDlaTabeli, bool ostatniaEdytowalna)
        {
            GridViewRow NewTotalRow = new GridViewRow(0, 0, DataControlRowType.DataRow, DataControlRowState.Insert);
            NewTotalRow.Cells.Add(cela(tekst, rowSpan, colSpan, CssStyleDlaTekstu));
            DataRow jedenWiersz = dane.Rows[idWiersza - 1];
            for (int i = 1; i < iloscKolumn; i++)
            {
                try
                {
                    string nazwaKolumny = "d_" + i.ToString("D2");
                    NewTotalRow.Cells.Add(cela("<a class='" + CssStyleDlaTekstu + "' href=\"javascript: openPopup('popup.aspx?sesja=" + idWiersza.ToString().Trim() + "!" + idtabeli.ToString().Trim() + "!" + i.ToString().Trim() + "!3')\">" + jedenWiersz[nazwaKolumny].ToString().Trim() + "</a>", 1, 1, cssStyleDlaTabeli));
                }
                catch (Exception ex)
                {
                    cm.log.Error("Podtabela  : " + ex.Message);
                }
            }
            if (ostatniaEdytowalna)
            {
                NewTotalRow.Cells.Add(cela("<input id = \"Text1\" type = \"text\" />", 1, 1, "borderTopLeft"));
            }
            return NewTotalRow;
        }// end of

        public TableCell cela(string text, int rowSpan, int colSpan, string cssClass)
        {
            TableCell HeaderCell = new TableCell();
            HeaderCell.Height = 10;
            HeaderCell.HorizontalAlign = HorizontalAlign.Center;
            HeaderCell.ColumnSpan = colSpan;
            HeaderCell.RowSpan = rowSpan;
            HeaderCell.CssClass = cssClass;
            HeaderCell.Text = text;
            return HeaderCell;
        }

        public ExcelWorksheet tworzArkuszwExcle(ExcelWorksheet Arkusz, DataTable daneDoArkusza, int iloscKolumn, int przesunięcieX, int przesuniecieY, bool lp, bool suma, bool stanowisko, bool funkcja, bool nazwiskoiImeieOsobno)
        {
            return tworzArkuszwExcle(Arkusz, daneDoArkusza, iloscKolumn, przesunięcieX, przesuniecieY, lp, suma, stanowisko, funkcja, nazwiskoiImeieOsobno, false);
        }

        public ExcelWorksheet tworzArkuszwExcle(ExcelWorksheet Arkusz, DataTable daneDoArkusza, int iloscKolumn, int przesunięcieX, int przesuniecieY, bool lp, bool suma, bool stanowisko, bool funkcja, bool nazwiskoiImeieOsobno, bool obramowanieOststniej)
        {
            if (daneDoArkusza == null)
            {
                cm.log.Error("Bład: Brak danych do Arkusza ");
                return Arkusz;
            }
            try
            {
                int wiersz = przesuniecieY;
                int dod = 0;
                foreach (DataRow dR in daneDoArkusza.Rows)
                {
                    int dodatek = 0;
                    if (lp)
                    {
                        try
                        {
                            dodatek++;

                            Arkusz.Cells[wiersz, przesunięcieX + dodatek].Style.ShrinkToFit = true;
                            Arkusz.Cells[wiersz, przesunięcieX + dodatek].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin, System.Drawing.Color.Black);
                            Arkusz.Cells[wiersz, przesunięcieX + dodatek].Value = wiersz - przesuniecieY + 1;
                        }
                        catch (Exception ex)
                        {
                            cm.log.Error("tworzArkuszwExcle- lp " + ex.Message);
                        }
                    }
                    if (stanowisko)
                    {
                        try
                        {
                            dodatek++;
                            Arkusz.Cells[wiersz, przesunięcieX + dodatek].Style.ShrinkToFit = true;
                            Arkusz.Cells[wiersz, przesunięcieX + dodatek].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin, System.Drawing.Color.Black);

                            string value = (dR["stanowisko"].ToString().Trim());
                            Arkusz.Cells[wiersz, przesunięcieX + dodatek].Value = value;
                        }
                        catch (Exception ex)
                        {
                            Arkusz.Cells[wiersz, przesunięcieX + dodatek].Value = "";
                            cm.log.Error("tworzArkuszwExcle- stanowisko " + ex.Message);
                        }
                    }
                    if (funkcja)
                    {
                        try
                        {
                            dodatek++;
                            Arkusz.Cells[wiersz, przesunięcieX + dodatek].Style.ShrinkToFit = true;
                            Arkusz.Cells[wiersz, przesunięcieX + dodatek].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin, System.Drawing.Color.Green);

                            string value = (dR["funkcja"].ToString().Trim());
                            Arkusz.Cells[wiersz, przesunięcieX + dodatek].Value = value;
                        }
                        catch (Exception ex)
                        {
                            Arkusz.Cells[wiersz, przesunięcieX + dodatek].Value = "";
                            cm.log.Error("tworzArkuszwExcle- lp " + ex.Message);
                        }
                    }
                    if (nazwiskoiImeieOsobno)
                    {
                        dodatek++;
                        try
                        {
                            Arkusz.Cells[wiersz, przesunięcieX + dodatek].Style.ShrinkToFit = true;
                            Arkusz.Cells[wiersz, przesunięcieX + dodatek].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin, System.Drawing.Color.Green);
                            string value = (dR["nazwisko"].ToString().Trim());
                            Arkusz.Cells[wiersz, przesunięcieX + dodatek].Value = value;
                            dodatek++;
                            Arkusz.Cells[wiersz, przesunięcieX + dodatek].Style.ShrinkToFit = true;
                            Arkusz.Cells[wiersz, przesunięcieX + dodatek].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin, System.Drawing.Color.Green);
                            value = (dR["imie"].ToString().Trim());
                            Arkusz.Cells[wiersz, przesunięcieX + dodatek].Value = value;
                        }
                        catch (Exception ex)
                        {
                            Arkusz.Cells[wiersz, przesunięcieX + dodatek].Value = "";
                            cm.log.Error("tworzArkuszwExcle- lp " + ex.Message);
                        }
                    }
                    else
                    {
                        try
                        {
                            dodatek++;
                            string value = dR["imie"].ToString().Trim() + " " + dR["nazwisko"].ToString().Trim();
                            Arkusz.Cells[wiersz, przesunięcieX + dodatek].Value = value;
                            Arkusz.Cells[wiersz, przesunięcieX + dodatek].Style.ShrinkToFit = true;
                            Arkusz.Cells[wiersz, przesunięcieX + dodatek].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin, System.Drawing.Color.Black);
                        }
                        catch (Exception ex)
                        {
                            cm.log.Error("tworzArkuszwExcle- lp " + ex.Message);
                        }
                    }

                    for (int i = 1; i < iloscKolumn; i++)
                    {
                        try
                        {
                            string colunmName = "d_" + (i).ToString("D2");
                            try
                            {
                                double value = double.Parse(dR[colunmName].ToString().Trim());
                                Arkusz.Cells[wiersz, i + przesunięcieX + dodatek].Value = value;
                            }
                            catch
                            {
                                Arkusz.Cells[wiersz, i + przesunięcieX + dodatek].Value = (dR[colunmName].ToString().Trim());
                            }
                            Arkusz.Cells[wiersz, przesunięcieX + dodatek + i].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin, System.Drawing.Color.Black);
                            Arkusz.Cells[wiersz, i + przesunięcieX + dodatek].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        }
                        catch (Exception ex)
                        {
                            cm.log.Error("Excell wpisywanie danych" + ex.Message);
                        }
                    }
                    if (obramowanieOststniej)
                    {
                        Arkusz.Cells[wiersz, przesunięcieX + dodatek + iloscKolumn].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin, System.Drawing.Color.Black);
                        Arkusz.Cells[wiersz, iloscKolumn + przesunięcieX + dodatek].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    }
                    wiersz++;
                    dod = dodatek;
                }

                if (suma)
                {
                    DataTable tabelka = tabellaLiczbowa(daneDoArkusza);
                    object sumObject;

                    Arkusz.Cells[wiersz, przesunięcieX + dod].Value = "Razem";
                    Arkusz.Cells[wiersz, przesunięcieX + dod].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin, System.Drawing.Color.Black);
                    Arkusz.Cells[wiersz, przesunięcieX + dod].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    for (int i = 1; i < iloscKolumn; i++)
                    {
                        try
                        {
                            string idkolumny = "d_" + (i).ToString("D2");
                            sumObject = tabelka.Compute("Sum(" + idkolumny + ")", "");
                            Arkusz.Cells[wiersz, i + przesunięcieX + dod].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin, System.Drawing.Color.Black);
                            Arkusz.Cells[wiersz, i + przesunięcieX + dod].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            Arkusz.Cells[wiersz, i + przesunięcieX + dod].Value = (sumObject.ToString());
                        }
                        catch (Exception ecx)
                        {
                            string mes = ecx.Message;
                            Arkusz.Cells[wiersz, i + przesunięcieX + dod].Value = mes;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                cm.log.Error("Excell all" + ex.Message);
            }

            return Arkusz;
        }

        public ExcelWorksheet tworzArkuszwExcle(ExcelWorksheet Arkusz, DataTable daneDoArkusza, int iloscKolumn, int przesunięcieX, int przesuniecieY, bool lp, bool suma, bool stanowisko, bool funkcja, bool nazwiskoiImeieOsobno, bool obramowanieOststniej, bool pustaKolumnaZaNazwiskiem)
        {
            if (daneDoArkusza == null)
            {
                cm.log.Error("Bład: Brak danych do Arkusza ");
                return Arkusz;
            }
            try
            {
                int wiersz = przesuniecieY;
                int dod = 0;
                foreach (DataRow dR in daneDoArkusza.Rows)
                {
                    int dodatek = 0;
                    if (lp)
                    {
                        try
                        {
                            dodatek++;

                            Arkusz.Cells[wiersz, przesunięcieX + dodatek].Style.ShrinkToFit = true;
                            Arkusz.Cells[wiersz, przesunięcieX + dodatek].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin, System.Drawing.Color.Black);
                            Arkusz.Cells[wiersz, przesunięcieX + dodatek].Value = wiersz - przesuniecieY + 1;
                        }
                        catch (Exception ex)
                        {
                            cm.log.Error("tworzArkuszwExcle- lp " + ex.Message);
                        }
                    }
                    if (stanowisko)
                    {
                        try
                        {
                            dodatek++;
                            Arkusz.Cells[wiersz, przesunięcieX + dodatek].Style.ShrinkToFit = true;
                            Arkusz.Cells[wiersz, przesunięcieX + dodatek].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin, System.Drawing.Color.Black);

                            string value = (dR["stanowisko"].ToString().Trim());
                            Arkusz.Cells[wiersz, przesunięcieX + dodatek].Value = value;
                        }
                        catch (Exception ex)
                        {
                            Arkusz.Cells[wiersz, przesunięcieX + dodatek].Value = "";
                            cm.log.Error("tworzArkuszwExcle- stanowisko " + ex.Message);
                        }
                    }
                    if (funkcja)
                    {
                        try
                        {
                            dodatek++;
                            Arkusz.Cells[wiersz, przesunięcieX + dodatek].Style.ShrinkToFit = true;
                            Arkusz.Cells[wiersz, przesunięcieX + dodatek].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin, System.Drawing.Color.Green);

                            string value = (dR["funkcja"].ToString().Trim());
                            Arkusz.Cells[wiersz, przesunięcieX + dodatek].Value = value;
                        }
                        catch (Exception ex)
                        {
                            Arkusz.Cells[wiersz, przesunięcieX + dodatek].Value = "";
                            cm.log.Error("tworzArkuszwExcle- lp " + ex.Message);
                        }
                    }
                    if (nazwiskoiImeieOsobno)
                    {
                        dodatek++;
                        try
                        {
                            Arkusz.Cells[wiersz, przesunięcieX + dodatek].Style.ShrinkToFit = true;
                            Arkusz.Cells[wiersz, przesunięcieX + dodatek].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin, System.Drawing.Color.Green);
                            string value = (dR["nazwisko"].ToString().Trim());
                            Arkusz.Cells[wiersz, przesunięcieX + dodatek].Value = value;
                            dodatek++;
                            Arkusz.Cells[wiersz, przesunięcieX + dodatek].Style.ShrinkToFit = true;
                            Arkusz.Cells[wiersz, przesunięcieX + dodatek].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin, System.Drawing.Color.Green);
                            value = (dR["imie"].ToString().Trim());
                            Arkusz.Cells[wiersz, przesunięcieX + dodatek].Value = value;
                        }
                        catch (Exception ex)
                        {
                            Arkusz.Cells[wiersz, przesunięcieX + dodatek].Value = "";
                            cm.log.Error("tworzArkuszwExcle- lp " + ex.Message);
                        }
                    }
                    else
                    {
                        try
                        {
                            dodatek++;
                            string value = dR["imie"].ToString().Trim() + " " + dR["nazwisko"].ToString().Trim();
                            Arkusz.Cells[wiersz, przesunięcieX + dodatek].Value = value;
                            Arkusz.Cells[wiersz, przesunięcieX + dodatek].Style.ShrinkToFit = true;
                            Arkusz.Cells[wiersz, przesunięcieX + dodatek].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin, System.Drawing.Color.Black);
                        }
                        catch (Exception ex)
                        {
                            cm.log.Error("tworzArkuszwExcle- lp " + ex.Message);
                        }
                    }

                    if (pustaKolumnaZaNazwiskiem)
                    {
                        przesunięcieX = przesunięcieX + 1;
                    }

                    for (int i = 1; i < iloscKolumn; i++)
                    {
                        try
                        {
                            string colunmName = "d_" + (i).ToString("D2");
                            try
                            {
                                double value = double.Parse(dR[colunmName].ToString().Trim());
                                Arkusz.Cells[wiersz, i + przesunięcieX + dodatek].Value = value;
                            }
                            catch
                            {
                                Arkusz.Cells[wiersz, i + przesunięcieX + dodatek].Value = (dR[colunmName].ToString().Trim());
                            }
                            Arkusz.Cells[wiersz, przesunięcieX + dodatek + i].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin, System.Drawing.Color.Black);
                            Arkusz.Cells[wiersz, i + przesunięcieX + dodatek].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        }
                        catch (Exception ex)
                        {
                            cm.log.Error("Excell wpisywanie danych" + ex.Message);
                        }
                    }
                    if (obramowanieOststniej)
                    {
                        Arkusz.Cells[wiersz, przesunięcieX + dodatek + iloscKolumn].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin, System.Drawing.Color.Black);
                        Arkusz.Cells[wiersz, iloscKolumn + przesunięcieX + dodatek].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    }
                    wiersz++;
                    dod = dodatek;
                }

                if (suma)
                {
                    DataTable tabelka = tabellaLiczbowa(daneDoArkusza);
                    object sumObject;

                    Arkusz.Cells[wiersz, przesunięcieX + dod].Value = "Razem";
                    Arkusz.Cells[wiersz, przesunięcieX + dod].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin, System.Drawing.Color.Black);
                    Arkusz.Cells[wiersz, przesunięcieX + dod].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    for (int i = 1; i < iloscKolumn; i++)
                    {
                        try
                        {
                            string idkolumny = "d_" + (i).ToString("D2");
                            sumObject = tabelka.Compute("Sum(" + idkolumny + ")", "");
                            Arkusz.Cells[wiersz, i + przesunięcieX + dod].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin, System.Drawing.Color.Black);
                            Arkusz.Cells[wiersz, i + przesunięcieX + dod].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            Arkusz.Cells[wiersz, i + przesunięcieX + dod].Value = (sumObject.ToString());
                        }
                        catch (Exception ecx)
                        {
                            string mes = ecx.Message;
                            Arkusz.Cells[wiersz, i + przesunięcieX + dod].Value = mes;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                cm.log.Error("Excell all" + ex.Message);
            }

            return Arkusz;
        }

        public ExcelWorksheet tworzArkuszwExcle(ExcelWorksheet Arkusz, DataTable daneDoArkusza, int iloscKolumn, int przesunięcieX, int przesuniecieY, bool lp, bool suma, bool stanowisko, bool funkcja, bool nazwiskoiImeieOsobno, bool obramowanieOststniej, bool przesuniecieiteracji, bool mp)
        {
            if (daneDoArkusza == null)
            {
                cm.log.Error("Bład: Brak danych do Arkusza ");
                return Arkusz;
            }
            try
            {
                int wiersz = przesuniecieY;
                int dod = 0;
                foreach (DataRow dR in daneDoArkusza.Rows)
                {
                    if (dR["nazwisko"].ToString().Trim() == "")
                    {
                        continue;
                    }
                    int dodatek = 0;
                    if (lp)
                    {
                        try
                        {
                            dodatek++;

                            if (!przesuniecieiteracji)
                            {
                                komorkaExcela(Arkusz, wiersz + 1, przesunięcieX + dodatek, (wiersz - przesuniecieY + 1).ToString(), false, 0, 0, true, false);
                            }
                            else
                            {
                                komorkaExcela(Arkusz, wiersz + 1, przesunięcieX + dodatek, (wiersz - przesuniecieY + 1).ToString(), false, 0, 0, true, false);
                            }
                        }
                        catch (Exception ex)
                        {
                            cm.log.Error("tworzArkuszwExcle- lp " + ex.Message);
                        }
                    }
                    if (stanowisko)
                    {
                        try
                        {
                            dodatek++;
                            Arkusz.Cells[wiersz, przesunięcieX + dodatek].Style.ShrinkToFit = true;
                            Arkusz.Cells[wiersz, przesunięcieX + dodatek].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin, System.Drawing.Color.Black);

                            string value = (dR["stanowisko"].ToString().Trim());
                            Arkusz.Cells[wiersz, przesunięcieX + dodatek].Value = value;
                        }
                        catch (Exception ex)
                        {
                            Arkusz.Cells[wiersz, przesunięcieX + dodatek].Value = "";
                            cm.log.Error("tworzArkuszwExcle- stanowisko " + ex.Message);
                        }
                    }
                    if (funkcja)
                    {
                        try
                        {
                            dodatek++;
                            Arkusz.Cells[wiersz, przesunięcieX + dodatek].Style.ShrinkToFit = true;
                            Arkusz.Cells[wiersz, przesunięcieX + dodatek].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin, System.Drawing.Color.Green);

                            string value = (dR["funkcja"].ToString().Trim());
                            Arkusz.Cells[wiersz, przesunięcieX + dodatek].Value = value;
                        }
                        catch (Exception ex)
                        {
                            Arkusz.Cells[wiersz, przesunięcieX + dodatek].Value = "";
                            cm.log.Error("tworzArkuszwExcle- lp " + ex.Message);
                        }
                    }
                    if (nazwiskoiImeieOsobno)
                    {
                        dodatek++;
                        try
                        {
                            Arkusz.Cells[wiersz, przesunięcieX + dodatek].Style.ShrinkToFit = true;
                            Arkusz.Cells[wiersz, przesunięcieX + dodatek].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin, System.Drawing.Color.Green);
                            string value = (dR["nazwisko"].ToString().Trim());
                            Arkusz.Cells[wiersz, przesunięcieX + dodatek].Value = value;
                            dodatek++;
                            Arkusz.Cells[wiersz, przesunięcieX + dodatek].Style.ShrinkToFit = true;
                            Arkusz.Cells[wiersz, przesunięcieX + dodatek].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin, System.Drawing.Color.Green);
                            value = (dR["imie"].ToString().Trim());
                            Arkusz.Cells[wiersz, przesunięcieX + dodatek + 1].Value = value;
                        }
                        catch (Exception ex)
                        {
                            Arkusz.Cells[wiersz, przesunięcieX + dodatek + 1].Value = "";
                            cm.log.Error("tworzArkuszwExcle- lp " + ex.Message);
                        }
                    }
                    else
                    {
                        try
                        {
                            dodatek++;
                            string value = dR["imie"].ToString().Trim() + " " + dR["nazwisko"].ToString().Trim();
                            if (!przesuniecieiteracji)
                            {
                                komorkaExcela(Arkusz, wiersz, przesunięcieX + dodatek, value, false, 0, 0, false, false);
                            }
                            else
                            {
                                komorkaExcela(Arkusz, wiersz + 1, przesunięcieX + dodatek, value, false, 0, 0, false, false);
                            }
                        }
                        catch (Exception ex)
                        {
                            cm.log.Error("tworzArkuszwExcle- imie nazwisko razem " + ex.Message);
                        }
                    }

                    for (int i = 1; i < iloscKolumn; i++)
                    {
                        try
                        {
                            string colunmName = "d_" + (i).ToString("D2");
                            try
                            {
                                komorkaExcela(Arkusz, wiersz + 1, i + przesunięcieX + dodatek, (dR[colunmName].ToString().Trim()), false, 0, 0, true, false);
                            }
                            catch
                            {
                                Arkusz.Cells[wiersz + 1, i + przesunięcieX + dodatek].Value = "0";
                            }
                        }
                        catch
                        {
                            Arkusz.Cells[wiersz + 1, i + przesunięcieX + dodatek].Value = "0";
                        }
                    }
                    if (obramowanieOststniej)
                    {
                        Arkusz.Cells[wiersz, przesunięcieX + dodatek + iloscKolumn].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin, System.Drawing.Color.Black);
                        Arkusz.Cells[wiersz, iloscKolumn + przesunięcieX + dodatek].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    }
                    wiersz++;
                    dod = dodatek;
                }

                if (suma)
                {
                    DataTable tabelka = tabellaLiczbowa(daneDoArkusza);
                    object sumObject;
                    if (mp)
                    {
                        wiersz = wiersz + 1;
                    }
                    komorkaExcela(Arkusz, wiersz, przesunięcieX + dod, "Razem", false, 0, 0);
                    for (int i = 1; i <= iloscKolumn + 1; i++)
                    {
                        try
                        {
                            string idkolumny = "d_" + (i).ToString("D2");
                            sumObject = tabelka.Compute("Sum(" + idkolumny + ")", "");
                            komorkaExcela(Arkusz, wiersz, i + przesunięcieX + dod, sumObject.ToString(), false, 0, 0, true, false);
                        }
                        catch (Exception ecx)
                        {
                            cm.log.Error("tworzArkuszwExcle- suma " + ecx.Message);
                        }
                    }
                }
                if (mp)
                {
                    for (int i = 1; i < iloscKolumn; i++)
                    {
                        komorkaExcela(Arkusz, przesuniecieY, przesunięcieX + 2 + i, i.ToString(), false, 0, 0, true, false);
                    }
                }
            }
            catch (Exception ex)
            {
                cm.log.Error("Excell all" + ex.Message);
            }

            return Arkusz;
        }

        public ExcelWorksheet tworzArkuszwExcle(ExcelWorksheet Arkusz, DataTable daneDoArkusza, int iloscKolumn, int przesunięcieX, int przesuniecieY, bool lp, bool suma, bool stanowisko, bool funkcja, bool nazwiskoiImeieOsobno, bool obramowanieOststniej, string Linia01, string Linia02, string Linia03)
        {
            Arkusz = tworzArkuszwExcle(Arkusz, daneDoArkusza, iloscKolumn, przesunięcieX, przesuniecieY, lp, suma, stanowisko, funkcja, nazwiskoiImeieOsobno, obramowanieOststniej);

            Arkusz.Cells[1, 1].Value = Linia01; ;
            Arkusz.Cells[2, 1].Value = Linia02; ;
            Arkusz.Cells[3, 1].Value = Linia03; ;

            return Arkusz;
        }

        public ExcelWorksheet tworzArkuszwExcleOKRR(ExcelWorksheet Arkusz, DataTable daneDoArkusza, int iloscKolumn, int przesunięcieX, int przesuniecieY, bool lp, bool suma, bool stanowisko, bool funkcja, bool nazwiskoiImeieOsobno)
        {
            try
            {
                int wiersz = przesuniecieY;
                int dod = 0;
                foreach (DataRow dR in daneDoArkusza.Rows)
                {
                    int dodatek = 0;
                    if (lp)
                    {
                        dodatek++;
                        Arkusz.Cells[wiersz, przesunięcieX + dodatek].Style.ShrinkToFit = true;
                        Arkusz.Cells[wiersz, przesunięcieX + dodatek].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin, System.Drawing.Color.Black);
                        Arkusz.Cells[wiersz, przesunięcieX + dodatek].Value = wiersz - przesuniecieY + 1;
                        Arkusz.Cells[wiersz, przesunięcieX + dodatek].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    }
                    if (stanowisko)
                    {
                        dodatek++;
                        Arkusz.Cells[wiersz, przesunięcieX + dodatek].Style.ShrinkToFit = true;
                        Arkusz.Cells[wiersz, przesunięcieX + dodatek].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin, System.Drawing.Color.Black);
                        try
                        {
                            string value = (dR["stanowisko"].ToString().Trim());
                            Arkusz.Cells[wiersz, przesunięcieX + dodatek].Value = value;
                        }
                        catch (Exception)
                        {
                            Arkusz.Cells[wiersz, przesunięcieX + dodatek].Value = "";
                        }
                    }
                    if (funkcja)
                    {
                        dodatek++;
                        Arkusz.Cells[wiersz, przesunięcieX + dodatek].Style.ShrinkToFit = true;
                        Arkusz.Cells[wiersz, przesunięcieX + dodatek].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin, System.Drawing.Color.Green);
                        try
                        {
                            string value = (dR["funkcja"].ToString().Trim());
                            Arkusz.Cells[wiersz, przesunięcieX + dodatek].Value = value;
                        }
                        catch (Exception)
                        {
                            Arkusz.Cells[wiersz, przesunięcieX + dodatek].Value = "";
                        }
                    }
                    if (nazwiskoiImeieOsobno)
                    {
                        dodatek++;
                        try
                        {
                            Arkusz.Cells[wiersz, przesunięcieX + dodatek].Style.ShrinkToFit = true;
                            Arkusz.Cells[wiersz, przesunięcieX + dodatek].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin, System.Drawing.Color.Green);
                            string value = (dR["nazwisko"].ToString().Trim());
                            Arkusz.Cells[wiersz, przesunięcieX + dodatek].Value = value;
                            dodatek++;
                            Arkusz.Cells[wiersz, przesunięcieX + dodatek].Style.ShrinkToFit = true;
                            Arkusz.Cells[wiersz, przesunięcieX + dodatek].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin, System.Drawing.Color.Green);
                            value = (dR["imie"].ToString().Trim());
                            Arkusz.Cells[wiersz, przesunięcieX + dodatek].Value = value;
                        }
                        catch (Exception)
                        {
                            Arkusz.Cells[wiersz, przesunięcieX + dodatek].Value = "";
                        }
                    }
                    else
                    {
                        try
                        {
                            dodatek++;
                            string value = dR["imie"].ToString().Trim() + " " + dR["nazwisko"].ToString().Trim();
                            Arkusz.Cells[wiersz, przesunięcieX + dodatek].Value = value;
                            Arkusz.Cells[wiersz, przesunięcieX + dodatek].Style.ShrinkToFit = true;
                            Arkusz.Cells[wiersz, przesunięcieX + dodatek].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin, System.Drawing.Color.Black);
                        }
                        catch
                        { }
                    }

                    for (int i = 1; i < iloscKolumn; i++)
                    {
                        try
                        {
                            string colunmName = "d_" + (i).ToString("D2");
                            try
                            {
                                if (i == 16)
                                {
                                    Arkusz.Cells[wiersz, przesunięcieX + dodatek + i].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                                    Arkusz.Cells[wiersz, przesunięcieX + dodatek + i].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin, System.Drawing.Color.Black);
                                    Arkusz.Cells[wiersz, i + przesunięcieX + dodatek].Value = "";
                                }
                                if (i == 33)
                                {
                                    Arkusz.Cells[wiersz, 33, wiersz, 36].Merge = true;
                                    double value = double.Parse(dR[colunmName].ToString().Trim());
                                    Arkusz.Cells[wiersz, przesunięcieX + dodatek + i].Value = value;
                                    Arkusz.Cells[wiersz, 33, wiersz, 36].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                                    Arkusz.Cells[wiersz, 33, wiersz, 36].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin, System.Drawing.Color.Black);
                                    //                                    Arkusz.Cells[wiersz, przesunięcieX + dodatek + i].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                                    //                                  Arkusz.Cells[wiersz, przesunięcieX + dodatek + i].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin, System.Drawing.Color.Black);
                                }
                                if (i < 33)
                                {
                                    double value = double.Parse(dR[colunmName].ToString().Trim());
                                    Arkusz.Cells[wiersz, i + przesunięcieX + dodatek].Value = value;
                                    Arkusz.Cells[wiersz, przesunięcieX + dodatek].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                                    Arkusz.Cells[wiersz, przesunięcieX + dodatek + i].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin, System.Drawing.Color.Black);
                                }
                                if (i == 37)
                                {
                                    double value = double.Parse(dR[colunmName].ToString().Trim());
                                    Arkusz.Cells[wiersz, przesunięcieX + dodatek + i].Value = value;
                                    Arkusz.Cells[wiersz, przesunięcieX + dodatek + i].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                                    Arkusz.Cells[wiersz, przesunięcieX + dodatek + i].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin, System.Drawing.Color.Black);
                                }
                                if (i > 38)
                                {
                                    continue;
                                }
                            }
                            catch
                            {
                                Arkusz.Cells[wiersz, przesunięcieX + dodatek + i].Value = (dR[colunmName].ToString().Trim());

                                Arkusz.Cells[wiersz, przesunięcieX + dodatek + i].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                                Arkusz.Cells[wiersz, przesunięcieX + dodatek + i].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin, System.Drawing.Color.Black);
                            }
                        }
                        catch (Exception ex)
                        {
                            cm.log.Error("Excell " + ex.Message);
                        }
                    }
                    wiersz++;
                    dod = dodatek;
                }

                if (suma)
                {
                    DataTable tabelka = tabellaLiczbowa(daneDoArkusza);
                    object sumObject;

                    Arkusz.Cells[wiersz, przesunięcieX + dod].Value = "Razem";
                    Arkusz.Cells[wiersz, przesunięcieX + dod].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin, System.Drawing.Color.Black);
                    Arkusz.Cells[wiersz, przesunięcieX + dod].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    for (int i = 1; i < iloscKolumn; i++)
                    {
                        try
                        {
                            string idkolumny = "d_" + (i).ToString("D2");
                            sumObject = tabelka.Compute("Sum(" + idkolumny + ")", "");
                            Arkusz.Cells[wiersz, i + przesunięcieX + dod].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin, System.Drawing.Color.Black);
                            Arkusz.Cells[wiersz, i + przesunięcieX + dod].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            Arkusz.Cells[wiersz, i + przesunięcieX + dod].Value = (sumObject.ToString());
                        }
                        catch (Exception ecx)
                        {
                            string mes = ecx.Message;
                            Arkusz.Cells[wiersz, i + przesunięcieX + dod].Value = mes;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                cm.log.Error("Excell " + ex.Message);
            }

            return Arkusz;
        }

        public ExcelWorksheet tworzArkuszwExcelPrzestawiony(ExcelWorksheet Arkusz, DataTable daneDoArkusza, int iloscKolumn, int przesunięcieX, int przesuniecieY, bool lp, bool suma, bool stanowisko, bool funkcja, bool nazwiskoiImeieOsobno)
        {
            int wiersz = przesuniecieY;

            try
            {
                foreach (DataRow dR in daneDoArkusza.Rows)
                {
                    int dodatek = 0;
                    if (lp)
                    {
                        dodatek++;
                        Arkusz.Cells[wiersz, przesunięcieX + dodatek].Style.ShrinkToFit = true;
                        Arkusz.Cells[wiersz, przesunięcieX + dodatek].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin, System.Drawing.Color.Black);
                        Arkusz.Cells[wiersz, przesunięcieX + dodatek].Value = wiersz - przesuniecieY + 1;
                    }
                    if (funkcja)
                    {
                        dodatek++;
                        Arkusz.Cells[wiersz, przesunięcieX + dodatek].Style.ShrinkToFit = true;
                        Arkusz.Cells[wiersz, przesunięcieX + dodatek].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin, System.Drawing.Color.Green);
                        try
                        {
                            string value = (dR["funkcja"].ToString().Trim());
                            Arkusz.Cells[wiersz, przesunięcieX + dodatek].Value = value;
                        }
                        catch (Exception)
                        {
                            Arkusz.Cells[wiersz, przesunięcieX + dodatek].Value = "";
                        }
                    }
                    if (stanowisko)
                    {
                        dodatek++;
                        Arkusz.Cells[wiersz, przesunięcieX + dodatek].Style.ShrinkToFit = true;
                        Arkusz.Cells[wiersz, przesunięcieX + dodatek].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin, System.Drawing.Color.Black);
                        try
                        {
                            string value = (dR["stanowisko"].ToString().Trim());
                            Arkusz.Cells[wiersz, przesunięcieX + dodatek].Value = value;
                        }
                        catch (Exception)
                        {
                            Arkusz.Cells[wiersz, przesunięcieX + dodatek].Value = "";
                        }
                    }

                    if (nazwiskoiImeieOsobno)
                    {
                        dodatek++;
                        try
                        {
                            Arkusz.Cells[wiersz, przesunięcieX + dodatek].Style.ShrinkToFit = true;
                            Arkusz.Cells[wiersz, przesunięcieX + dodatek].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin, System.Drawing.Color.Green);
                            string value = (dR["nazwisko"].ToString().Trim());
                            Arkusz.Cells[wiersz, przesunięcieX + dodatek].Value = value;
                            dodatek++;
                            Arkusz.Cells[wiersz, przesunięcieX + dodatek].Style.ShrinkToFit = true;
                            Arkusz.Cells[wiersz, przesunięcieX + dodatek].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin, System.Drawing.Color.Green);
                            value = (dR["imie"].ToString().Trim());
                            Arkusz.Cells[wiersz, przesunięcieX + dodatek].Value = value;
                        }
                        catch (Exception)
                        {
                            Arkusz.Cells[wiersz, przesunięcieX + dodatek].Value = "";
                        }
                    }
                    else
                    {
                        try
                        {
                            dodatek++;
                            string value = dR["imie"].ToString().Trim() + " " + dR["nazwisko"].ToString().Trim();
                            Arkusz.Cells[wiersz, przesunięcieX + dodatek].Value = value;
                            Arkusz.Cells[wiersz, przesunięcieX + dodatek].Style.ShrinkToFit = true;
                            Arkusz.Cells[wiersz, przesunięcieX + dodatek].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin, System.Drawing.Color.Black);
                        }
                        catch
                        { }
                    }

                    for (int i = 0; i < iloscKolumn; i++)
                    {
                        try
                        {
                            string colunmName = "d_" + i.ToString("D2");
                            try
                            {
                                double value = double.Parse(dR[colunmName].ToString().Trim());
                                Arkusz.Cells[wiersz, i + przesunięcieX + dodatek].Value = value;
                            }
                            catch
                            {
                                Arkusz.Cells[wiersz, i + przesunięcieX + dodatek].Value = (dR[colunmName].ToString().Trim());
                            }
                            Arkusz.Cells[wiersz, przesunięcieX + dodatek + i].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin, System.Drawing.Color.Black);
                        }
                        catch (Exception ex)
                        {
                            cm.log.Error("Excell " + ex.Message);
                        }
                    }
                    wiersz++;
                }
            }
            catch
            {
            }
            // tu dodać sumę
            return Arkusz;
        }

        public ExcelWorksheet tworzArkuszwExcleBezSedziow(ExcelWorksheet Arkusz, DataTable daneDoArkusza, int iloscwierszy, int iloscKolumn, int przesunięcieX, int przesuniecieY, bool zerowaKolumna)
        {
            cm.log.Info("Excell ilosc wierszy " + iloscwierszy.ToString());
            cm.log.Info("Excell ilosc Kolumn " + iloscKolumn.ToString());
            if (daneDoArkusza == null)
            {
                cm.log.Error("Excell Brak danych do tabeli ");
                return null;
            }
            try
            {
                daneDoArkusza.Columns.Remove("id_");
            }
            catch
            {
            }
            try
            {
                daneDoArkusza.Columns.Remove("idTabeli");
            }
            catch
            {
            }
            try
            {
                daneDoArkusza.Columns.Remove("opis");
            }
            catch
            {
            }
            int startowaKolumna = !zerowaKolumna ? 1 : 0;
            int wiersz = przesuniecieY;
            for (int i = 0; i < iloscwierszy; i++)
            {
                for (int j = startowaKolumna; j <= iloscKolumn; j++)
                {
                    if (i < daneDoArkusza.Rows.Count)
                    {
                        DataRow row = daneDoArkusza.Rows[i];
                        string nazwaKolumny = "d_" + (j).ToString("D2");
                        try
                        {
                            string value = row[nazwaKolumny].ToString();//daneDoArkusza.Rows[i][j].ToString().Trim();
                            cm.log.Info("wiersz: " + i.ToString() + " kolumna" + j.ToString() + " wartosc  " + value);
                            komorkaExcela(Arkusz, i + przesuniecieY, przesunięcieX + j, value, false, 0, 0);
                        }
                        catch (Exception ex)
                        {
                            cm.log.Error("tworzArkuszwExcleBezSedziow " + ex.Message);
                        }
                    }
                }
            }
            return Arkusz;
        }

        public ExcelWorksheet tworzArkuszwExcleBezSedziowZopisem(ExcelWorksheet Arkusz, DataTable daneDoArkusza, int iloscwierszy, int iloscKolumn, int przesunięcieX, int przesuniecieY, bool zerowaKolumna)
        {
            cm.log.Info("Excell ilosc wierszy " + iloscwierszy.ToString());
            cm.log.Info("Excell ilosc Kolumn " + iloscKolumn.ToString());
            if (daneDoArkusza == null)
            {
                cm.log.Error("Excell Brak danych do tabeli ");
                return null;
            }
            try
            {
                daneDoArkusza.Columns.Remove("id_");
            }
            catch
            {
            }
            try
            {
                daneDoArkusza.Columns.Remove("idTabeli");
            }
            catch
            {
            }

            int startowaKolumna = !zerowaKolumna ? 1 : 0;
            int wiersz = przesuniecieY;
            for (int i = 0; i < iloscwierszy; i++)
            {
                try
                {
                    DataRow row = daneDoArkusza.Rows[i];
                    string opis = row["opis"].ToString();

                    komorkaExcela(Arkusz, i + przesuniecieY, przesunięcieX, opis, false, 0, 0);
                    for (int j = startowaKolumna; j <= iloscKolumn; j++)
                    {
                        if (i < daneDoArkusza.Rows.Count)
                        {
                            if (j == 0)
                            {
                                continue;
                            }
                            try
                            {
                                string nazwaKolumny = "d_" + (j).ToString("D2");

                                string value = row[nazwaKolumny].ToString();//daneDoArkusza.Rows[i][j].ToString().Trim();
                                cm.log.Info("wiersz: " + i.ToString() + " kolumna" + j.ToString() + " wartosc  " + value);
                                komorkaExcela(Arkusz, i + przesuniecieY, przesunięcieX + j, value, false, 0, 0);
                            }
                            catch (Exception)
                            {
                                throw;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    cm.log.Error("tworzArkuszwExcleBezSedziow " + ex.Message);
                }
            }
            return Arkusz;
        }

        public ExcelWorksheet tworzArkuszwExcleBezSedziow(ExcelWorksheet Arkusz, DataTable daneDoArkusza, int iloscwierszy, int iloscKolumn, int przesunięcieX, int przesuniecieY, bool zerowaKolumna, int wierszPoczatkowy, int wierszKoncowy)
        {
            cm.log.Info("Excell ilosc wierszy " + iloscwierszy.ToString());
            cm.log.Info("Excell ilosc Kolumn " + iloscKolumn.ToString());
            if (daneDoArkusza == null)
            {
                cm.log.Error("Excell Brak danych do tabeli ");
                return null;
            }
            int startowaKolumna = !zerowaKolumna ? 1 : 0;
            int wiersz = przesuniecieY;
            for (int i = 0; i < iloscwierszy; i++)
            {
                if ((i >= wierszPoczatkowy) && (i <= wierszKoncowy))
                {
                    for (int j = startowaKolumna; j <= iloscKolumn; j++)
                    {
                        try
                        {
                            string value = daneDoArkusza.Rows[i][j].ToString().Trim();
                            cm.log.Info("wiersz: " + i.ToString() + " kolumna" + j.ToString() + " wartosc  " + value);
                            komorkaExcela(Arkusz, i + przesuniecieY, przesunięcieX + j, value, false, 0, 0);
                        }
                        catch (Exception ex)
                        {
                            cm.log.Error("tworzArkuszwExcleBezSedziow " + ex.Message);
                        }
                    }
                }
            }
            return Arkusz;
        }

        public ExcelWorksheet tworznaglowki(ExcelWorksheet Arkusz, DataTable daneDoArkusza, int iloscwierszy, int przesunięcieX, int przesuniecieY, string tekstNadTabela)
        {
            komorkaExcela(Arkusz, 1, 2, tekstNadTabela, false, 0, 0);
            int wiersz = przesuniecieY;
            for (int i = 1; i < iloscwierszy + 1; i++)
            {
                try
                {
                    komorkaExcela(Arkusz, przesuniecieY, i + przesunięcieX, daneDoArkusza.Rows[i - 1][1].ToString().Trim(), false, 0, 0, true, false);
                }
                catch (Exception ex)
                {
                    cm.log.Error("KP tworznaglowki " + ex.Message);
                }
            }
            return Arkusz;
        }

        public ExcelWorksheet tworzArkuszwExcleBezSedziowMK(ExcelWorksheet Arkusz, DataTable daneDoArkusza, int iloscwierszy, int iloscKolumn, int przesunięcieX, int przesuniecieY, bool zerowaKolumna, int idWydzialu, int idTabeliNum, string tenPlik)
        {
            try
            {
                int startowaKolumna = 0;
                if (!zerowaKolumna)
                {
                    startowaKolumna = 1;
                }

                komorkaExcela(Arkusz, 1, 1, dr.generuj_naglowki_nad_tabela(idWydzialu, idTabeliNum, tenPlik), false, 0, 0, true, false);
                int wiersz = przesuniecieY;
                DataTable naglowek = dr.generuj_naglowki_nad_tabelaMK(idWydzialu, idTabeliNum, tenPlik);
                for (int j = 0; j < iloscKolumn; j++)
                {
                    try
                    {
                        string txt = naglowek.Rows[j][0].ToString();
                        komorkaExcela(Arkusz, przesuniecieY + 1, przesunięcieX + j + startowaKolumna, txt, false, 0, 0, true, false);
                        // cm.log.Info("KM Generowanie pliku Excel Tabela: " + idTabeliNum + " wiersz: " + i.ToString() + " kolumna" + j.ToString() + " wartosc  " + txt);
                    }
                    catch (Exception ex)
                    {
                        cm.log.Error("Mk tworzArkuszwExcleBezSedziowMK 01 ThreadAbortException " + ex.Message);
                    }
                }
                for (int i = 1; i < iloscwierszy; i++)
                {
                    for (int j = startowaKolumna; j < iloscKolumn; j++)
                    {
                        try
                        {
                            string txt = dr.wyciagnijWartosc(daneDoArkusza, "idWydzial=" + idWydzialu + " and idTabeli='" + idTabeliNum.ToString() + "' and idWiersza ='" + i.ToString() + "' and idkolumny='" + j.ToString() + "'", tenPlik);
                            komorkaExcela(Arkusz, i + przesuniecieY + 1, przesunięcieX + j, txt, false, 0, 0, true, false);
                            cm.log.Info("KM Generowanie pliku Excel Tabela: " + idTabeliNum + " wiersz: " + i.ToString() + " kolumna" + j.ToString() + " wartosc  " + txt);
                        }
                        catch (ThreadAbortException ex)
                        {
                            cm.log.Error("Mk tworzArkuszwExcleBezSedziowMK 01 ThreadAbortException " + ex.Message);
                        }
                        catch (Exception ex)
                        {
                            Arkusz.Cells[i + przesuniecieY, przesunięcieX + j].Value = "";
                            cm.log.Error("tworzArkuszwExcleBezSedziowMK " + ex.Message);
                        }
                    }
                }
            }
            catch (ThreadAbortException ex)
            {
                cm.log.Error("Mk tworzArkuszwExcleBezSedziowMK 02 ThreadAbortException " + ex.Message);
            }
            catch (Exception ex)
            {
                cm.log.Error("tworzArkuszwExcleBezSedziowMK " + ex.Message);
            }

            return Arkusz;
        }

        public void komorkaExcela(ExcelWorksheet Arkusz, int wiersz, int kolumna, string tekst, bool zlaczenie, int rowSpan, int colSpan)
        {
            komorkaExcela(Arkusz, wiersz, kolumna, tekst, zlaczenie, rowSpan, colSpan, false, false);
        }

        public void komorkaExcela(ExcelWorksheet Arkusz, int wiersz, int kolumna, string tekst, bool zlaczenie, int rowSpan, int colSpan, bool wycentrowanie, bool wyszarzenie)
        {
            if (zlaczenie)
            {
                try
                {
                    Arkusz.Cells[wiersz, kolumna, wiersz + rowSpan, kolumna + colSpan].Merge = true;
                    Arkusz.Cells[wiersz, kolumna, wiersz + rowSpan, kolumna + colSpan].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin, System.Drawing.Color.Black);
                    Arkusz.Cells[wiersz, kolumna, wiersz + rowSpan, kolumna + colSpan].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                }
                catch (Exception ex)
                {
                    cm.log.Error("komorkaExcela merge " + ex.Message);
                }
            }
            try
            {
                Arkusz.Cells[wiersz, kolumna].Style.ShrinkToFit = true;
                Arkusz.Cells[wiersz, kolumna].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin, System.Drawing.Color.Black);
                if (wycentrowanie)
                {
                    Arkusz.Cells[wiersz, kolumna].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                }
                Arkusz.Cells[wiersz, kolumna].Value = tekst;
                if (wyszarzenie)
                {
                    Arkusz.Cells[wiersz, kolumna].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    Arkusz.Cells[wiersz, kolumna].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Gray);
                }
            }
            catch (Exception ex)
            {
                cm.log.Error("komorkaExcela merge " + ex.Message);
            }

        }

        public DataTable naglowek(string plik, int numerArkusza)
        {
            if (string.IsNullOrEmpty(plik.Trim()))
            {
                return null;
            }
            IList<string> komorki = new List<string>();

            DataTable schematNaglowka = new DataTable();
            schematNaglowka.Columns.Add("wiersz", typeof(int));
            schematNaglowka.Columns.Add("kolumna", typeof(int));
            schematNaglowka.Columns.Add("text", typeof(string));
            schematNaglowka.Columns.Add("rowSpan", typeof(int));
            schematNaglowka.Columns.Add("colSpan", typeof(int));

            var package = new ExcelPackage(new FileInfo(plik));
            using (package)
            {
                int iloscZakladek = package.Workbook.Worksheets.Count;
                if (iloscZakladek >= numerArkusza)
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[numerArkusza];

                    int rows = worksheet.Dimension.End.Row;
                    int columns = worksheet.Dimension.End.Column;

                    for (int i = 1; i <= rows; i++)
                    {
                        for (int j = 1; j <= columns; j++)
                        {
                            object baseE = worksheet.Cells[i, j];
                            ExcelCellBase celka = (ExcelCellBase)baseE;

                            bool polaczony = (bool)celka.GetType().GetProperty("Merge").GetValue(celka, null);
                            var kolumny = celka.GetType().GetProperty("Columns").GetValue(celka, null);
                            var wiersze = celka.GetType().GetProperty("Rows").GetValue(celka, null);
                            var text = celka.GetType().GetProperty("Value").GetValue(celka, null);

                            DataRow komorka = schematNaglowka.NewRow();
                            if (polaczony && text != null)
                            {
                                IList<int> lista = okreslKomorke(i, j, rows, columns, worksheet);

                                komorka["wiersz"] = i;
                                komorka["kolumna"] = j;
                                komorka["text"] = text;
                                komorka["colSpan"] = lista[0].ToString();
                                komorka["rowSpan"] = lista[1].ToString();

                                schematNaglowka.Rows.Add(komorka);
                                int k = lista[1];
                                if (k > 1)
                                {
                                    j = (j + k) - 1;
                                }
                            }
                            else
                            {
                                komorka["wiersz"] = i;
                                komorka["kolumna"] = j;
                                komorka["text"] = text;
                                komorka["colSpan"] = 1;
                                komorka["rowSpan"] = 1;
                                if (text != null)
                                {
                                    schematNaglowka.Rows.Add(komorka);
                                }
                            }
                        }
                    }
                }
            }

            DataTable dT_01 = new DataTable();
            dT_01.Columns.Clear();
            dT_01.Columns.Add("Column1", typeof(string));
            dT_01.Columns.Add("Column2", typeof(string));
            dT_01.Columns.Add("Column3", typeof(string));
            dT_01.Columns.Add("Column4", typeof(string));
            dT_01.Columns.Add("Column5", typeof(string));

            // max ilosc wierszy
            var max = schematNaglowka.Rows.OfType<DataRow>().Select(row => row["wiersz"]).Max();

            if (max != null)
            {
                int wiersz = 0;
                for (int i = (int)max; i >= 0; i--)
                {
                    wiersz++;
                    //wyciągnij dane tylko z wierszem
                    string selectString = "wiersz=" + i.ToString();
                    DataRow[] jedenWiersz = schematNaglowka.Select(selectString);
                    foreach (var komorka in jedenWiersz)
                    {
                        dT_01.Rows.Add(new Object[] { wiersz.ToString(), komorka["text"], komorka["rowSpan"], komorka["colSpan"], "h" });
                    }
                }
            }

            return dT_01;
        }

        protected IList<int> okreslKomorke(int wierszPoczatkowy, int kolumnaPoczatkowa, int iloscWierszy, int iloscKolumn, ExcelWorksheet worksheet)
        {
            IList<int> wyniki = new List<int>();
            int rowSpan = 0;
            int colSpan = 0;

            bool mergedY = false;

            for (int i = wierszPoczatkowy; i <= iloscWierszy + 1; i++)
            {
                object baseE = worksheet.Cells[i, kolumnaPoczatkowa];

                ExcelCellBase celka = (ExcelCellBase)baseE;
                bool polaczony = (bool)celka.GetType().GetProperty("Merge").GetValue(celka, null);
                var text = celka.GetType().GetProperty("Value").GetValue(celka, null);
                if (!polaczony)
                {
                    break;
                }
                else
                {
                    if (mergedY)
                    {
                        if (text != null)
                        {
                            break;
                        }
                    }
                    mergedY = true;
                }
                rowSpan++;
            }
            bool mergedX = false;
            for (int j = kolumnaPoczatkowa; j <= iloscKolumn + 1; j++)
            {
                object baseE = worksheet.Cells[wierszPoczatkowy, j];

                ExcelCellBase celka = (ExcelCellBase)baseE;
                bool polaczony = (bool)celka.GetType().GetProperty("Merge").GetValue(celka, null);
                var text = celka.GetType().GetProperty("Value").GetValue(celka, null);
                if (!polaczony)
                {
                    break;
                }
                else
                {
                    if (mergedX)
                    {
                        if (text != null)
                        {
                            break;
                        }
                    }
                    mergedX = true;
                }
                colSpan++;
            }
            wyniki.Add(rowSpan);
            wyniki.Add(colSpan);
            return wyniki;
        }

        public DataTable SchematTabelinaglowkowej()
        {
            DataTable tabelaNaglowkowa = new DataTable();
            tabelaNaglowkowa.Columns.Clear();
            tabelaNaglowkowa.Columns.Add("wiersz", typeof(string));
            tabelaNaglowkowa.Columns.Add("text", typeof(string));
            tabelaNaglowkowa.Columns.Add("Column3", typeof(string));
            tabelaNaglowkowa.Columns.Add("Column4", typeof(string));
            tabelaNaglowkowa.Columns.Add("Column5", typeof(string));
            tabelaNaglowkowa.Columns.Add("Column6", typeof(string));
            return tabelaNaglowkowa;
        }

        public string komorkaHTML(string text, int colspan, int rowspan, string style)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<td ");
            if (!string.IsNullOrEmpty(style.Trim()))
            {
                builder.Append(" class='" + style + "' ");
            }
            if (rowspan > 0)
            {
                builder.Append(" rowspan='" + rowspan + "' ");
            }
            if (colspan > 0)
            {
                builder.Append(" colspan='" + colspan + "' ");
            }
            builder.AppendLine(">");
            builder.AppendLine("<p>" + text + "</p>");
            builder.AppendLine("</td>");
            return builder.ToString();
        }

        public string komorkaHTMLbezP(string text, int colspan, int rowspan, string style)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<td ");
            if (!string.IsNullOrEmpty(style.Trim()))
            {
                builder.Append(" class='" + style + "' ");
            }
            if (rowspan > 0)
            {
                builder.Append(" rowspan='" + rowspan + "' ");
            }
            if (colspan > 0)
            {
                builder.Append(" colspan='" + colspan + "' ");
            }
            builder.AppendLine(">");

            builder.AppendLine(text);

            builder.AppendLine("</td>");
            return builder.ToString();
        }
    }
}