﻿using DevExpress.Utils;
using DevExpress.Web;
using DevExpress.XtraPrinting;
using System;
using System.Data;
using System.Drawing;
using System.Web.DynamicData;
using System.Web.UI.WebControls;

namespace Statystyki_2018
{
    public partial class kontrolka2022 : System.Web.UI.Page
    {
        public wyszukiwarka w1 = new wyszukiwarka();
        public common cm = new common();
        public Class1 cl = new Class1();

        protected void Page_Load(object sender, EventArgs e)
        {
            //Bind the grid only once
            if (!IsPostBack)
            {
                // if (Session["valueX"] == null)
                //  {
                Session["valueX"] = Request.QueryString["w"];
                //  }
                DateTime dTime = DateTime.Now.AddMonths(-1);

                string ident = (string)Session["valueX"];
                if (string.IsNullOrEmpty(ident))
                {
                    Server.Transfer("default.aspx");
                    return;
                }

                DataTable parameters = cm.makeParameterTable();
                parameters.Rows.Add("@ident", ident);
                string nazwaKontrolki = string.Empty;
                DateTime dataOd = DateTime.Parse(dTime.Year.ToString() + "-" + dTime.Month.ToString("D2") + "-01");
                try
                {
                    dataOd = DateTime.Parse(cm.getQuerryValue("SELECT Data_od FROM konfig  WHERE (ident = @ident)", cm.con_str, parameters));
                }
                catch
                { }
                try
                {
                    nazwaKontrolki = cm.getQuerryValue("SELECT Opis FROM konfig  WHERE (ident = @ident)", cm.con_str, parameters);
                }
                catch
                { }

                Session["czesc"] = nazwaKontrolki;  
                if (data1.Text.Length == 0)
                {
                    data1.Date = dataOd;
                }

                if (data2.Text.Length == 0)
                {
                    data2.Date = DateTime.Parse(dTime.Year.ToString() + "-" + dTime.Month.ToString("D2") + "-" + DateTime.DaysInMonth(dTime.Year, dTime.Month).ToString("D2"));
                }

                grid.DataBind();

                DataBindX();

            }
        }

        protected void szukaj(object sender, EventArgs e)
        {
            grid.DataBind();
        }

        protected void Druk(object sender, EventArgs e)
        {
            string ident = (string)Session["valueX"];

            ASPxGridViewExporter1.Landscape = true;

            DataTable parameters = cm.makeParameterTable();
            parameters.Rows.Add("@ident", ident);
            string nazwa = string.Empty;

            try
            {
                nazwa = cm.getQuerryValue("SELECT opis FROM konfig  WHERE (ident = @ident)", cm.con_str, parameters);
            }
            catch
            { }
            ASPxGridViewExporter1.ReportHeader = nazwa;
            Session["exporter"] = ASPxGridViewExporter1;
            ASPxGridViewExporter1.LeftMargin = 5;
            ASPxGridViewExporter1.RightMargin = 5;
            ASPxGridViewExporter1.TopMargin = 0;
            ASPxGridViewExporter1.BottomMargin = 0;
            ASPxGridViewExporter1.WritePdfToResponse("kontrolka-" + DateTime.Now.ToShortDateString());
            //   ScriptManager.RegisterStartupScript(Page, Page.GetType(), "print2", "JavaScript:window.open('kontrolkaDruk.aspx')", true);
        }

        protected void dataBinding(object sender, EventArgs e)
        {
            DataBindX();
        }

        private DataTable GetTable(DateTime dataPoczatkowa, DateTime dataKoncowa, string ident, string tenPlik)
        {
            DataTable parameters = cm.makeParameterTable();

            parameters.Rows.Add("@ident", ident);
            string kw = cm.getQuerryValue("SELECT wartosc FROM            konfig  WHERE        (ident = @ident)", cm.con_str, parameters);
            string cs = cm.getQuerryValue("SELECT ConnectionString FROM            konfig  WHERE        (ident = @ident)", cm.con_str, parameters);

            parameters.Rows.Add("@data_1", cl.KonwertujDate(data1.Date));
            parameters.Rows.Add("@data_2", cl.KonwertujDate(data2.Date));

            DataTable dT = new DataTable();
            try
            {
                dT = cm.getDataTable(kw, cs, parameters, tenPlik);
                int ilr = dT.Rows.Count;
            }
            catch
            {
            }

            return dT;
        }

        protected void Excell(object sender, EventArgs e)
        {
            ASPxGridViewExporter1.WriteXlsxToResponse("kontrolka - " + DateTime.Now.ToShortDateString());
        }

        protected void ASPxGridViewExporter1_RenderBrick(object sender, DevExpress.Web.ASPxGridViewExportRenderingEventArgs e)
        {
            StringFormat sFormat = new StringFormat(StringFormatFlags.NoWrap);
            BrickStringFormat brickSFormat = new BrickStringFormat(sFormat);
            e.XlsxFormatString = sFormat.ToString();
        }

        private void DataBindX()
        {
            grid.SettingsBehavior.AllowEllipsisInText = true; 
            var fontWeight = grid.Font.Size;
            string ident = (string)Session["valueX"];
            if (string.IsNullOrEmpty(ident))
            {
                return;
            }
            DataTable dane = GetTable(data1.Date, data2.Date, ident, "Kontrolka nowa");
            DataTable daneNew = new DataTable();

            if (dane != null)
            {
                daneNew.Columns.Add(new DataColumn { ColumnName = "Lp" });
                daneNew.Merge(dane, false, MissingSchemaAction.Add);
            }

            grid.DataSource = daneNew;
            try
            {
                grid.SettingsPager.PageSize = int.Parse(cm.odczytajWartosc("kontrolka_wiersze"));
            }
            catch
            {
                grid.SettingsPager.PageSize = 500;
            }
            DataTable parameters = cm.makeParameterTable();
            parameters.Rows.Add("@ident", ident);

            grid.Styles.AlternatingRow.Enabled = DefaultBoolean.True;

            string matrixszerokosci = string.Empty;

            int szerokoscKolumny = 0;
             int rozmiarCzcionki = 0;
             int szerokosctabeli = 0;
            try
            {
                matrixszerokosci = cm.getQuerryValue("SELECT macierzszerokosci FROM            konfig  WHERE        (ident = @ident)", cm.con_str, parameters);
            }
            catch
            {}


            try
             {
                      szerokoscKolumny = int.Parse(cm.getQuerryValue("SELECT szerokoscKolumny FROM            konfig  WHERE        (ident = @ident)", cm.con_str, parameters));
             }
             catch
             {
                 szerokoscKolumny = 50;
             }
             try
             {
                 rozmiarCzcionki = int.Parse(cm.getQuerryValue("SELECT rozmiarczcionki FROM            konfig  WHERE        (ident = @ident)", cm.con_str, parameters));
             }
             catch
             {
                 rozmiarCzcionki = 10;
             }
           /*  try
             {
                 szerokosctabeli = int.Parse(cm.getQuerryValue("SELECT szerokosctabeli FROM            konfig  WHERE        (ident = @ident)", cm.con_str, parameters));
             }
             catch
             {
                 szerokosctabeli = 1150;
             }*/
             cm.log.Info("Kontrolka -rozmiar czcionki: " + rozmiarCzcionki.ToString());
             cm.log.Info("Kontrolka -szerokosc Kolumny: " + szerokoscKolumny.ToString());
             cm.log.Info("Kontrolka -szerokosc tabeli: " + szerokosctabeli.ToString());
             Session["rozmiarCzcionki"] = rozmiarCzcionki;
             Session["szerokoscKolumny"] = szerokoscKolumny;
             Session["szerokosctabeli"] = szerokosctabeli;

            if (string.IsNullOrEmpty(matrixszerokosci))
            {
                // brak macierzy

                int columnCounter = 0;
                foreach (GridViewDataColumn dCol in grid.Columns)
                {
                    string name = dCol.Name;
                    Type typ = dCol.GetType();
                    Type typRef = typeof(DevExpress.Web.GridViewDataDateColumn);
                    GridViewDataColumn id = new GridViewDataColumn();
                    id.FieldName = name;

                    cm.log.Info("kontrolka reftype: " + typRef.FullName);
                    cm.log.Info("kontrolka type: " + typ.FullName);


                    if (szerokoscKolumny > 0)
                    {
                        dCol.MinWidth = szerokoscKolumny;

                    }
                    if (columnCounter == 0)
                    {
                        dCol.MinWidth = 50;
                        dCol.FixedStyle = GridViewColumnFixedStyle.Left;
                    }
                    if (typ == typRef)
                    {
                        grid.DataColumns[name].SettingsHeaderFilter.Mode = GridHeaderFilterMode.DateRangePicker;
                        grid.DataColumns[name].Settings.AllowHeaderFilter = DevExpress.Utils.DefaultBoolean.True;
                        dCol.MinWidth = 50;

                    }

                    if (dCol is GridViewDataColumn)
                    {
                        ((GridViewDataColumn)dCol).Settings.AutoFilterCondition = AutoFilterCondition.Contains;
                    }


                    dCol.CellStyle.Wrap = DefaultBoolean.False;
                    dCol.HeaderStyle.Wrap = DefaultBoolean.True;

                    columnCounter ++;

                }
               
            }
            else 
            {
                // jest macierz

                
                string[] matrixszerokosciMatrix = matrixszerokosci.Split(',');

                int columnCounter = 0;
                foreach (GridViewDataColumn dCol in grid.Columns)
                {
                    string name = dCol.Name;
                    Type typ = dCol.GetType();
                    Type typRef = typeof(DevExpress.Web.GridViewDataDateColumn);
                    GridViewDataColumn id = new GridViewDataColumn();
                    id.FieldName = name;

                    cm.log.Info("kontrolka reftype: " + typRef.FullName);
                    cm.log.Info("kontrolka type: " + typ.FullName);

                    if (columnCounter>0)
                    {
                        try
                        {
                            int tempWidth = int.Parse( matrixszerokosciMatrix[columnCounter - 1]);
                            dCol.MinWidth = tempWidth;
                            dCol.Width = tempWidth;
                        }
                        catch 
                        {

                            dCol.MinWidth = 50 ;
                        }
                    }
                  
                    if (columnCounter == 0)
                    {
                        dCol.MinWidth = 35;
                        dCol.Width = 35;
                        dCol.FixedStyle = GridViewColumnFixedStyle.Left;
                    }
                    if (typ == typRef)
                    {
                        grid.DataColumns[name].SettingsHeaderFilter.Mode = GridHeaderFilterMode.DateRangePicker;
                        grid.DataColumns[name].Settings.AllowHeaderFilter = DevExpress.Utils.DefaultBoolean.True;
                     
                    }

                    if (dCol is GridViewDataColumn)
                    {
                        ((GridViewDataColumn)dCol).Settings.AutoFilterCondition = AutoFilterCondition.Contains;
                    }

                    dCol.CellStyle.Wrap = DefaultBoolean.False;
                    dCol.HeaderStyle.Wrap = DefaultBoolean.True;

                    columnCounter++;

                }
              

            }


            
            ASPxGridViewExporter1.DataBind();
        }
        private int ColumnLenght(DataTable dataTable, int ColumnNumber)
        {
            int maxLenght = 0;

            foreach (DataRow row in dataTable.Rows) 
            {
                int tmpMaxLenght = 0;
                string cellValue = row[ColumnNumber].ToString();
                if (cellValue != null)
                {
                    tmpMaxLenght= cellValue.Length;
                    if (tmpMaxLenght>maxLenght)
                    {
                        maxLenght = tmpMaxLenght;
                    }
                }
            
            }

            return maxLenght;
        }
        protected void grid_CustomColumnDisplayText(object sender, ASPxGridViewColumnDisplayTextEventArgs e)
        {
            if (e.Column.FieldName == "Lp")
            {
                e.DisplayText = (e.VisibleIndex + 1).ToString();
            }

        }

        protected void gridView_HtmlRowCreated(object sender, ASPxGridViewTableRowEventArgs e)
        {

        }

    }
}