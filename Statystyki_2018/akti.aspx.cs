using OfficeOpenXml;
using System;
using System.Data;
using System.Globalization;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Statystyki_2018
{
    public partial class akti : System.Web.UI.Page
    {
        public Class1 cl = new Class1();
        public common cm = new common();
        public tabele tb = new tabele();
        public dataReaders dr = new dataReaders();
        private const string tenPlik = "akti.aspx";

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
                DataTable tabelka01 = dr.generuj_dane_do_tabeli_wierszy2018(Date1.Date, Date2.Date, (string)Session["id_dzialu"], 2,16, 20, false, tenPlik);
                Session["tabelka002"] = tabelka01;
                tabela_1();
              
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
             
            }
            catch
            {
                Label11.Visible = false;
                infoLabel2.Visible = false;
               
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

            

            dT_02.Rows.Add(new Object[] { "1", "GW", "1", "1", "h", "60" });
            dT_02.Rows.Add(new Object[] { "1", "Gwo", "1", "1", "h", "60" });
            dT_02.Rows.Add(new Object[] { "1", "GWz", "1", "1", "h", "60" });
            dT_02.Rows.Add(new Object[] { "1", "rep", "1", "1", "h", "60" });
            dT_02.Rows.Add(new Object[] { "1", "rep", "1", "1", "h", "60" });
            dT_02.Rows.Add(new Object[] { "1", "rep", "1", "1", "h", "60" });
            dT_02.Rows.Add(new Object[] { "1", "razem", "1", "1", "h", "60" });
            dT_02.Rows.Add(new Object[] { "1", "GW", "1", "1", "h", "60" });
            dT_02.Rows.Add(new Object[] { "1", "Gwo", "1", "1", "h", "60" });
            dT_02.Rows.Add(new Object[] { "1", "GWz", "1", "1", "h", "60" });
            dT_02.Rows.Add(new Object[] { "1", "rep", "1", "1", "h", "60" });
            dT_02.Rows.Add(new Object[] { "1", "rep", "1", "1", "h", "60" });
            dT_02.Rows.Add(new Object[] { "1", "rep", "1", "1", "h", "60" });
            dT_02.Rows.Add(new Object[] { "1", "razem", "1", "1", "h", "60" });


            dT_02.Rows.Add(new Object[] { "2", "Lp ", "1", "2", "h", "45" });
            dT_02.Rows.Add(new Object[] { "2", "Imię i nazwisko ", "1", "2", "h", "45" });
            dT_02.Rows.Add(new Object[] { "2", "Wpływ spraw  ", "7", "1", "h", "45" });
            dT_02.Rows.Add(new Object[] { "2", "Załatwienia spraw", "7", "1", "h", "60" });
            dT_02.Rows.Add(new Object[] { "2", "Ilość sporządzonych uzasadnień", "1", "2", "h", "60" });


            return dT_02;
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
            { }
        }

        protected void Button3_Click(object sender, EventArgs e)
        {
            string path = Server.MapPath("Template") + "\\akti.xlsx";
            FileInfo existingFile = new FileInfo(path);
            string download = Server.MapPath("Template") + @"\akti";

            FileInfo fNewFile = new FileInfo(download + "_.xlsx");

            using (ExcelPackage MyExcel = new ExcelPackage(existingFile))
            {
               

                int rowik = 0;

                ExcelWorksheet MyWorksheet1 = MyExcel.Workbook.Worksheets[1];

                DataTable table = (DataTable)Session["tabelka001"];
                rowik = table.Rows.Count - 3;
                MyWorksheet1 = tb.tworzArkuszwExcle(MyExcel.Workbook.Worksheets[1], table, 16, 0, 4, true, false, false, false, false, false);

                MyWorksheet1 = tb.tworzArkuszwExcleBezSedziow(MyExcel.Workbook.Worksheets[1], (DataTable)Session["tabelka002"], 11, 15, 2, rowik + 7, false);

                tb.komorkaExcela(MyExcel.Workbook.Worksheets[1], rowik + 7, 1, "Pozostało z okresu poprzedniego", true, 0, 1);
                tb.komorkaExcela(MyExcel.Workbook.Worksheets[1], rowik + 8, 1, "Wpływ spraw", true, 0, 1);
                tb.komorkaExcela(MyExcel.Workbook.Worksheets[1], rowik + 9, 1, "Załatwienia", true, 0, 1);
                tb.komorkaExcela(MyExcel.Workbook.Worksheets[1], rowik + 10, 1, "Pozostało na okres następny:", true, 0, 1);
                tb.komorkaExcela(MyExcel.Workbook.Worksheets[1], rowik + 11, 1, "powyżej 3 do 6 miesiący", true, 0, 1);
                tb.komorkaExcela(MyExcel.Workbook.Worksheets[1], rowik + 12, 1, "powyżej 6 do 12 miesiący", true, 0,1);
                tb.komorkaExcela(MyExcel.Workbook.Worksheets[1], rowik + 13, 1, "powyżej 12m-cy do 2 lat", true, 0, 1);
                tb.komorkaExcela(MyExcel.Workbook.Worksheets[1], rowik + 14, 1, "powyżej 2 do 3 lat", true, 0, 1);
                tb.komorkaExcela(MyExcel.Workbook.Worksheets[1], rowik + 15, 1, "powyżej 3 do 5 lat", true, 0, 1);
                tb.komorkaExcela(MyExcel.Workbook.Worksheets[1], rowik + 16, 1, "powyżej 5 do 8 lat", true, 0, 1);
                tb.komorkaExcela(MyExcel.Workbook.Worksheets[1], rowik + 17, 1, "Ponad 8 lat", true, 0, 1);
         
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
                tb.makeSumRow((DataTable)Session["tabelka001"], e,1);
            }
        }

    
        public void AddNewRow(object sender, GridViewRowEventArgs e)
        {
            DataTable tabelka01 = (DataTable)Session["tabelka002"];

            GridView GridView1 = (GridView)sender;
            GridViewRow NewTotalRow = new GridViewRow(0, 0, DataControlRowType.DataRow, DataControlRowState.Insert);
           
            try
            {
                GridView1.Controls[0].Controls.AddAt(e.Row.RowIndex + rowIndex, tb.PodsumowanieTabeli((DataTable)Session["tabelka001"], 16, "borderAll center gray"));
            }
            catch (Exception ex)
            {
                cm.log.Error(tenPlik + " " + ex.Message);
            }
            
            string idtabeli = "2";
            string[] teksty00 = new string[] { "","GW", "Gwo", "GWz", "rep", "rep", "rep" ,"razem"};

            GridView1.Controls[0].Controls.AddAt(e.Row.RowIndex + rowIndex, tb.wierszTabeli(teksty00, 8, 1, "Kategorie spraw", 2, 1, "normal", "", false));

            int idWiersza = 1;
            GridView1.Controls[0].Controls.AddAt(e.Row.RowIndex + rowIndex, tb.wierszTabeli(tabelka01,8, idWiersza, idtabeli, "Pozostało z okresu poprzedniego", 2, 1, "normal", "borderTopLeft "));

            idWiersza = 2;
            GridView1.Controls[0].Controls.AddAt(e.Row.RowIndex + rowIndex, tb.wierszTabeli(tabelka01,8, idWiersza, idtabeli, "Wpływ spraw", 2, 1, "normal", "borderTopLeft "));

            idWiersza = 3;
            GridView1.Controls[0].Controls.AddAt(e.Row.RowIndex + rowIndex, tb.wierszTabeli(tabelka01,8, idWiersza, idtabeli, "Załatwienia", 2, 1, "normal", "borderTopLeft "));
            idWiersza = 4;
            GridView1.Controls[0].Controls.AddAt(e.Row.RowIndex + rowIndex, tb.wierszTabeli(tabelka01,8, idWiersza, idtabeli, "Pozostało na okres następny:", 2, 1, "normal", "borderTopLeft "));
            GridView1.Controls[0].Controls.AddAt(e.Row.RowIndex + rowIndex, tb.wierszTabeli(teksty00, 8, 1, "Kategorie spraw", 2, 1, "normal", "", false));

            idWiersza = 5;
            GridView1.Controls[0].Controls.AddAt(e.Row.RowIndex + rowIndex, tb.wierszTabeli(tabelka01,8, idWiersza, idtabeli, "powyżej  3 do 6 miesięcy", 2, 1, "normal", "borderTopLeft "));
            idWiersza = 6;
            GridView1.Controls[0].Controls.AddAt(e.Row.RowIndex + rowIndex, tb.wierszTabeli(tabelka01,8, idWiersza, idtabeli, "powyżej 6 do 12 miesięcy", 2, 1, "normal", "borderTopLeft "));
            idWiersza = 7;
            GridView1.Controls[0].Controls.AddAt(e.Row.RowIndex + rowIndex, tb.wierszTabeli(tabelka01,8, idWiersza, idtabeli, "Pow.12m-cy do 2 lat", 2, 1, "normal", "borderTopLeft "));
            idWiersza = 8;
            GridView1.Controls[0].Controls.AddAt(e.Row.RowIndex + rowIndex, tb.wierszTabeli(tabelka01,8, idWiersza, idtabeli, "Pow.2 do 3 lat", 2, 1, "normal", "borderTopLeft "));
            idWiersza = 9;
            GridView1.Controls[0].Controls.AddAt(e.Row.RowIndex + rowIndex, tb.wierszTabeli(tabelka01,8, idWiersza, idtabeli, "Pow.3 do 5 lat", 2, 1, "normal", "borderTopLeft "));
            idWiersza = 10;
            GridView1.Controls[0].Controls.AddAt(e.Row.RowIndex + rowIndex, tb.wierszTabeli(tabelka01,8, idWiersza, idtabeli, "Pow.5 do 8 lat", 2, 1, "normal", "borderTopLeft "));
            idWiersza = 11;
            GridView1.Controls[0].Controls.AddAt(e.Row.RowIndex + rowIndex, tb.wierszTabeli(tabelka01,8, idWiersza, idtabeli, "Ponad 8 lat", 2, 1, "normal", "borderTopLeft "));

        }
    }
}