/*
Last Update:
    - version 1.211201
Creation date: 2019-12-11

*/

using DevExpress.Web;
using OfficeOpenXml;
using System;
using System.Data;
using System.Globalization;
using System.IO;

namespace Statystyki_2018
{
    public partial class aopc : System.Web.UI.Page
    {
        public Class1 cl = new Class1();
        public common cm = new common();
        public tabele tb = new tabele();
        public dataReaders dr = new dataReaders();
        public devExpressXXL DevExpressXXL = new devExpressXXL();
        private const string tenPlik = "aopc.aspx";
        private const string tenPlikNazwa = "aopc";
        private string path = "";

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
                //Label3.Text = cl.nazwaSadu((string)Session["id_dzialu"]);
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
            if (Session["id_dzialu"] == null)
            {
                return;
            }

            //odswiezenie danych
            tabela_1();
          

            LabelNazwaSadu.Text = cl.nazwaSadu((string)Session["id_dzialu"]);
        }

        protected void tworzPlikExcell(object sender, EventArgs e)
        {
            // execel begin
            string path = Server.MapPath("Template") + "\\" + tenPlikNazwa + ".xlsx";
            FileInfo existingFile = new FileInfo(path);
            if (!existingFile.Exists)
            {
                return;
            }
            string download = Server.MapPath("Template") + @"\" + tenPlikNazwa + "";

            FileInfo fNewFile = new FileInfo(download + "_.xlsx");

            using (ExcelPackage MyExcel = new ExcelPackage(existingFile))
            {
                
                ExcelWorksheet MyWorksheet1 = tb.tworzArkuszwExcle(MyExcel.Workbook.Worksheets[1], (DataTable)Session["tabelka001"], 125, 0, 10, true, false, false, false, false);

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
            string idDzialu = (string)Session["id_dzialu"];
            if (cl.debug(int.Parse(idDzialu)))
            {
                cm.log.Info(tenPlik + ": rozpoczęcie tworzenia tabeli 1");
            }
            DataTable tabelka01 = DevExpressXXL.zLicznikiemKolumn(dr.konwertujNaPrzecinek(dr.generuj_dane_do_tabeli_sedziowskiej_2019(int.Parse(idDzialu), 5, Date1.Date, Date2.Date, 220, tenPlik)));

            Session["tabelka001"] = tabelka01;

            ASPxGridView1.DataSource = null;
            ASPxGridView1.DataSourceID = null;
            ASPxGridView1.AutoGenerateColumns = true;
            ASPxGridView1.DataSource = tabelka01;
            ASPxGridView1.DataBind();
            ASPxGridView1.KeyFieldName = "id_sedziego";
            ASPxGridView1.Columns.Clear();
            int szerokoscKolumny = 80;
            ASPxGridView1.Width = Panel1.Width;
            idDzialu = "5";
            ASPxGridView1.Columns.Add(DevExpressXXL.kolumnaDoTabeli("L.p.", "id", idDzialu, "", true, 36));
            ASPxGridView1.Columns.Add(DevExpressXXL.kolumnaDoTabeli("Imie i nazwisko", "Imienazwisko", idDzialu, "", true, 250));

            #region wpływ

            GridViewBandColumn kolumnaWplyw = DevExpressXXL.GetBoundColumn("Wpływ");
            kolumnaWplyw.Columns.Add(DevExpressXXL.kolumnaDoTabeli("Ogółem", "d_01", idDzialu, "", false, szerokoscKolumny));
            GridViewBandColumn kolumnaWplywC = DevExpressXXL.GetBoundColumn("C");
            kolumnaWplywC.Columns.Add(DevExpressXXL.kolumnaDoTabeli("ogółem", "d_02", idDzialu, "", false, szerokoscKolumny));
            kolumnaWplywC.Columns.Add(DevExpressXXL.podKolumna(new string[] { "o rozwód", "o separację" }, 3, idDzialu, false, szerokoscKolumny, "w tym"));
            kolumnaWplyw.Columns.Add(kolumnaWplywC);
            kolumnaWplyw.Columns.Add(DevExpressXXL.kolumnaDoTabeli("Ns ogółem", "d_05", idDzialu, "", false, szerokoscKolumny));
            kolumnaWplyw.Columns.Add(DevExpressXXL.kolumnaDoTabeli("Ns-rej", "d_06", idDzialu, "", false, szerokoscKolumny));
            kolumnaWplyw.Columns.Add(DevExpressXXL.kolumnaDoTabeli("Nc", "d_07", idDzialu, "", false, szerokoscKolumny));
            kolumnaWplyw.Columns.Add(DevExpressXXL.kolumnaDoTabeli("Co ogółem", "d_08", idDzialu, "", false, szerokoscKolumny));
            
            kolumnaWplyw.Columns.Add(DevExpressXXL.kolumnaDoTabeli("WSC", "d_09", idDzialu, "", false, szerokoscKolumny));
            kolumnaWplyw.Columns.Add(DevExpressXXL.kolumnaDoTabeli("WSNc", "d_10", idDzialu, "", false, szerokoscKolumny));
            kolumnaWplyw.Columns.Add(DevExpressXXL.kolumnaDoTabeli("Cz", "d_11", idDzialu, "", false, szerokoscKolumny));
            ASPxGridView1.Columns.Add(kolumnaWplyw);

            #endregion wpływ

            #region załatwiono

            GridViewBandColumn kolumnaZalatwiono = DevExpressXXL.GetBoundColumn("ZAŁATWIONO");
            kolumnaZalatwiono.Columns.Add(DevExpressXXL.podKolumna(new string[] { "na rozprawe", "na posiedzenie" }, 12, idDzialu, false, szerokoscKolumny, "Ogółem"));
            kolumnaZalatwiono.Columns.Add(DevExpressXXL.podKolumna(new string[] { "na rozprawe", "na posiedzenie" }, 14, idDzialu, false, szerokoscKolumny, "C"));
            GridViewBandColumn kolumnaZalatwionoWtym = DevExpressXXL.GetBoundColumn("w tym");
            kolumnaZalatwionoWtym.Columns.Add(DevExpressXXL.podKolumna(new string[] { "na rozprawe", "na posiedzenie" }, 16, idDzialu, false, szerokoscKolumny, "o rozwód"));
            kolumnaZalatwionoWtym.Columns.Add(DevExpressXXL.podKolumna(new string[] { "na rozprawe", "na posiedzenie" }, 18, idDzialu, false, szerokoscKolumny, "o separację"));
            kolumnaZalatwiono.Columns.Add(kolumnaZalatwionoWtym);
            
           
            kolumnaZalatwiono.Columns.Add(DevExpressXXL.podKolumna(new string[] { "na rozprawe", "na posiedzenie" }, 20, idDzialu, false, szerokoscKolumny, "Ns"));
            kolumnaZalatwiono.Columns.Add(DevExpressXXL.podKolumna(new string[] { "na rozprawe", "na posiedzenie" }, 22, idDzialu, false, szerokoscKolumny, "Ns-Rej"));
            kolumnaZalatwiono.Columns.Add(DevExpressXXL.podKolumna(new string[] { "na rozprawe", "na posiedzenie" }, 24, idDzialu, false, szerokoscKolumny, "Nc"));
            kolumnaZalatwiono.Columns.Add(DevExpressXXL.podKolumna(new string[] { "na rozprawe", "na posiedzenie" }, 26, idDzialu, false, szerokoscKolumny, "Co"));
            kolumnaZalatwiono.Columns.Add(DevExpressXXL.podKolumna(new string[] { "na rozprawe", "na posiedzenie" }, 28, idDzialu, false, szerokoscKolumny, "WSC"));
            kolumnaZalatwiono.Columns.Add(DevExpressXXL.podKolumna(new string[] { "na rozprawe", "na posiedzenie" }, 30, idDzialu, false, szerokoscKolumny, "WSCNc"));
            kolumnaZalatwiono.Columns.Add(DevExpressXXL.podKolumna(new string[] { "na rozprawe", "na posiedzenie" }, 32, idDzialu, false, szerokoscKolumny, "Cz"));

           
            ASPxGridView1.Columns.Add(kolumnaZalatwiono);

            #endregion załatwiono

            #region załatwienia

            GridViewBandColumn kolumnaZalatwienia = DevExpressXXL.GetBoundColumn("ZAŁATWIENIA");
            kolumnaZalatwienia.Columns.Add(DevExpressXXL.kolumnaDoTabeli("Ogółem", "d_34", idDzialu, "", false, szerokoscKolumny));
            GridViewBandColumn kolumnaZalatwieniaC = DevExpressXXL.GetBoundColumn("C");
            kolumnaZalatwieniaC.Columns.Add(DevExpressXXL.kolumnaDoTabeli("ogółem", "d_35", idDzialu, "", false, szerokoscKolumny));
            kolumnaZalatwieniaC.Columns.Add(DevExpressXXL.kolumnaDoTabeli("o rozwód", "d_36", idDzialu, "", false, szerokoscKolumny));
            kolumnaZalatwieniaC.Columns.Add(DevExpressXXL.kolumnaDoTabeli("o separację", "d_37", idDzialu, "", false, szerokoscKolumny));


            kolumnaZalatwienia.Columns.Add(kolumnaZalatwieniaC);
            kolumnaZalatwienia.Columns.Add(DevExpressXXL.kolumnaDoTabeli("Ns", "d_38", idDzialu, "", false, szerokoscKolumny));
            kolumnaZalatwienia.Columns.Add(DevExpressXXL.kolumnaDoTabeli("Ns-rej", "d_39", idDzialu, "", false, szerokoscKolumny));
            kolumnaZalatwienia.Columns.Add(DevExpressXXL.kolumnaDoTabeli("Nc", "d_40", idDzialu, "", false, szerokoscKolumny));
            kolumnaZalatwienia.Columns.Add(DevExpressXXL.kolumnaDoTabeli("Co", "d_41", idDzialu, "", false, szerokoscKolumny));
            kolumnaZalatwienia.Columns.Add(DevExpressXXL.kolumnaDoTabeli("WSC", "d_42", idDzialu, "", false, szerokoscKolumny));
            kolumnaZalatwienia.Columns.Add(DevExpressXXL.kolumnaDoTabeli("WSNc", "d_43", idDzialu, "", false, szerokoscKolumny));
            kolumnaZalatwienia.Columns.Add(DevExpressXXL.kolumnaDoTabeli("Cz", "d_44", idDzialu, "", false, szerokoscKolumny));
            ASPxGridView1.Columns.Add(kolumnaZalatwienia);

            #endregion załatwienia


            #region sesje odbyte przez sędziego

            GridViewBandColumn sesjeSedziego = DevExpressXXL.GetBoundColumn("sesje odbyte przez sędziego ");

            // GridViewBandColumn wszystkieSesjeSedziego = DevExpressXXL.GetBoundColumn("(na potrzeby MS-S)");
            sesjeSedziego.Columns.Add(DevExpressXXL.kolumnaDoTabeli("ogółem", "d_45", idDzialu, "", false, szerokoscKolumny));
            sesjeSedziego.Columns.Add(DevExpressXXL.podKolumna(new string[] { "rozprawy", "posiedzenia jawne", "posiedzenia niejawne" }, 46, idDzialu, false, szerokoscKolumny, "z tego "));
         //   sesjeSedziego.Columns.Add(wszystkieSesjeSedziego);

            ASPxGridView1.Columns.Add(sesjeSedziego);

            #endregion sesje odbyte przez sędziego



            #region pozostalosc
            GridViewBandColumn kolumnaPozostalosc = DevExpressXXL.GetBoundColumn("Pozostałość na następny miesiąc");
            kolumnaPozostalosc.Columns.Add(DevExpressXXL.kolumnaDoTabeli("Ogółem", "d_49", idDzialu, "", false, szerokoscKolumny));
            
            GridViewBandColumn kolumnaPozostaloscC = DevExpressXXL.GetBoundColumn("C");
            kolumnaPozostaloscC.Columns.Add(DevExpressXXL.kolumnaDoTabeli("ogółem", "d_50", idDzialu, "", false, szerokoscKolumny));

            GridViewBandColumn kolumnaPozostaloscCeTym = DevExpressXXL.GetBoundColumn("w tym");

            kolumnaPozostaloscCeTym.Columns.Add(DevExpressXXL.kolumnaDoTabeli("o rozwód", "d_51", idDzialu, "", false, szerokoscKolumny));
            kolumnaPozostaloscCeTym.Columns.Add(DevExpressXXL.kolumnaDoTabeli("o separacj", "d_52", idDzialu, "", false, szerokoscKolumny));
            kolumnaPozostaloscC.Columns.Add(kolumnaPozostaloscCeTym);
            kolumnaPozostalosc.Columns.Add(kolumnaPozostaloscC);

            kolumnaPozostalosc.Columns.Add(DevExpressXXL.kolumnaDoTabeli("Ns", "d_53", idDzialu, "", false, szerokoscKolumny));
            kolumnaPozostalosc.Columns.Add(DevExpressXXL.kolumnaDoTabeli("Ns-rej", "d_54", idDzialu, "", false, szerokoscKolumny));
            kolumnaPozostalosc.Columns.Add(DevExpressXXL.kolumnaDoTabeli("Nc", "d_55", idDzialu, "", false, szerokoscKolumny));
            kolumnaPozostalosc.Columns.Add(DevExpressXXL.kolumnaDoTabeli("Co", "d_56", idDzialu, "", false, szerokoscKolumny));
            kolumnaPozostalosc.Columns.Add(DevExpressXXL.kolumnaDoTabeli("WSC", "d_57", idDzialu, "", false, szerokoscKolumny));
            kolumnaPozostalosc.Columns.Add(DevExpressXXL.kolumnaDoTabeli("WSNc", "d_58", idDzialu, "", false, szerokoscKolumny));
            kolumnaPozostalosc.Columns.Add(DevExpressXXL.kolumnaDoTabeli("Cz", "d_59", idDzialu, "", false, szerokoscKolumny));
            ASPxGridView1.Columns.Add(kolumnaPozostalosc);

            #endregion pozostalosc

            
            ASPxGridView1.Columns.Add(DevExpressXXL.podKolumna(new string[] { "Ogółem", "do 3 mie- sięcy", "pow. 3 do 6 m-cy", "pow. 6 do 12 m-cy", "pow.12 m-cy do 2 lat", "pow. 2 do 3 lat", "pow. 3 do 5 lat", "pow. 5 do 8 lat", "pow. 8 lat" }, 60, idDzialu, false, szerokoscKolumny, "pozostało spraw starych (wszystkie kategorie spraw) pozostało spraw starych - wszystkie kategorie spraw(bez czasu trwania mediacji, zgodnie z dz. 2.1.1.1 MS - S1o)"));

            ASPxGridView1.Columns.Add(DevExpressXXL.podKolumna(new string[] { "Ogółem", "zakreś- lonych", "nie zakreś- lonych" }, 69, idDzialu, false, szerokoscKolumny, "stan spraw zawieszonych(wszystkie kategorie spraw, zgodnie z MS - S5r)"));

            GridViewBandColumn liczbaSporzadzonychUzasadnien = DevExpressXXL.podKolumna(new string[4] { "Łącznie", "w terminie ustawowym 14 dni", "razem po terminie ustawowym", "nie- usprawied- liwione" }, 72, idDzialu, false, szerokoscKolumny, "terminowość sporządzania uzasadnień	skargi na przewlekłość	UWAGI od dnia doręczenia sędziemu wniosku do sporządzenia uzasadnienia(zgodnie z MS-S1o, dz. 1.4.2)*");
            GridViewBandColumn PoUplywie = (DevExpressXXL.podKolumna(new string[8] { "1-14 dni", "w tym nieuspra -wiedliwione", "15-30 dni", "w tym nieuspra -wiedliwione", "powyżej 1 do 3 mies", "w tym nieuspra -wiedliwione", "ponad 3 mies", "w tym nieuspra -wiedliwione" }, 76, idDzialu, false, szerokoscKolumny, "po upływie terminiu ustawowego"));
            liczbaSporzadzonychUzasadnien.Columns.Add(PoUplywie);
                        
            liczbaSporzadzonychUzasadnien.Columns.Add(DevExpressXXL.kolumnaDoTabeli("Uzasadnienia wygłoszone **/***", "d_84", idDzialu, "", false, szerokoscKolumny));
            liczbaSporzadzonychUzasadnien.Columns.Add(DevExpressXXL.kolumnaDoTabeli("Liczba spraw, do których wpłynął wniosek o transkrypcje uzasadnień wygłoszonych", "d_85", idDzialu, "", false, szerokoscKolumny));
            ASPxGridView1.Columns.Add(liczbaSporzadzonychUzasadnien);

            GridViewBandColumn liczbaSporzadzonychUzasadnien2 = DevExpressXXL.podKolumna(new string[4] { "Łącznie", "w terminie ustawowym 14 dni", "razem po terminie ustawowym", "nie- usprawied- liwione" }, 86, idDzialu, false, szerokoscKolumny, "terminowość sporządzania uzasadnień z urzędu  przez sędziów i referendarzy sądowych (zgodnie z MS - S5r, dz. 1.3.1.b + 1.3.1.d) * ");
            GridViewBandColumn PoUplywie2 = (DevExpressXXL.podKolumna(new string[8] { "1-14 dni", "w tym nieuspra -wiedliwione", "15-30 dni", "w tym nieuspra -wiedliwione", "powyżej 1 do 3 mies", "w tym nieuspra -wiedliwione", "ponad 3 mies", "w tym nieuspra -wiedliwione" }, 90, idDzialu, false, szerokoscKolumny, "po upływie terminiu ustawowego"));

            liczbaSporzadzonychUzasadnien2.Columns.Add(PoUplywie2);
            liczbaSporzadzonychUzasadnien2.Columns.Add(DevExpressXXL.kolumnaDoTabeli("Uzasadnienia wygłoszone **/***", "d_98", idDzialu, "", false, szerokoscKolumny));
            liczbaSporzadzonychUzasadnien2.Columns.Add(DevExpressXXL.kolumnaDoTabeli("Liczba spraw, do których wpłynął wniosek o transkrypcje uzasadnień wygłoszonych", "d_99", idDzialu, "", false, szerokoscKolumny));
            ASPxGridView1.Columns.Add(liczbaSporzadzonychUzasadnien2);

            GridViewBandColumn zalatwiono01skargiNaPrzewleklosc = DevExpressXXL.GetBoundColumn("skargi na przewlekłość");
            zalatwiono01skargiNaPrzewleklosc.Columns.Add(DevExpressXXL.kolumnaDoTabeli("wpływ", "d_100", idDzialu, "", false, szerokoscKolumny));

            zalatwiono01skargiNaPrzewleklosc.Columns.Add(DevExpressXXL.podKolumna(new string[2] { "ogółem", "uwzględniono" }, 101, idDzialu, false, szerokoscKolumny, "załatwiono"));
            zalatwiono01skargiNaPrzewleklosc.Columns.Add(DevExpressXXL.kolumnaDoTabeli("pozostałość", "d_103", idDzialu, "", false, szerokoscKolumny));
            ASPxGridView1.Columns.Add(zalatwiono01skargiNaPrzewleklosc);

            ASPxGridView1.Columns.Add(DevExpressXXL.kolumnaDoTabeli("Uwagi", "d_104", idDzialu, "", false, szerokoscKolumny));

            ASPxGridView1.TotalSummary.Clear();
            ASPxGridView1.TotalSummary.Add(DevExpressXXL.komorkaSumujaca("Ogółem"));
            for (int i = 1; i < 109; i++)
            {
                ASPxGridView1.TotalSummary.Add(DevExpressXXL.komorkaSumujaca(i));
            }
        }

    

        protected void Suma(object sender, DevExpress.Data.CustomSummaryEventArgs e)
        {
            ASPxSummaryItem sumItem = (ASPxSummaryItem)e.Item;
        }

        protected void ASPxGridView1_SummaryDisplayText(object sender, ASPxGridViewSummaryDisplayTextEventArgs e)
        {
            try
            {
                if (e.Item.FieldName.Contains("d_"))
                {
                    double value = double.Parse(e.Value.ToString());
                    string field = e.Item.FieldName.Replace("d_", "");
                    value = value - double.Parse(field);
                    e.Text = value.ToString();
                }
            }
            catch
            { }
        }
    }
}