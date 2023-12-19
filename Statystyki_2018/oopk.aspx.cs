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
    public partial class oopk : System.Web.UI.Page
    {
        public Class1 cl = new Class1();
        public common cm = new common();
        public tabele tb = new tabele();
        public dataReaders dr = new dataReaders();
        public devExpressXXL DevExpressXXL = new devExpressXXL();
        private const string tenPlik = "oopk.aspx";
        private const string tenPlikNazwa = "oopk";
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
            DataTable tabela = (DataTable)Session["tabelka001"];
            if (tabela == null)
            {
                return;
            }
            foreach (DataRow dr in tabela.Select($"id=0"))
                dr.Delete();
            // pierwsza tabelka

            using (ExcelPackage MyExcel = new ExcelPackage(existingFile))
            {
                ExcelWorksheet MyWorksheet1 = MyExcel.Workbook.Worksheets[1];

                // pierwsza

                MyWorksheet1 = tb.tworzArkuszwExcle(MyExcel.Workbook.Worksheets[1], tabela, 91, 0, 8, true, true, false, false, false);
           
               
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

            GridViewBandColumn kolumna_SprawyZakresuUbezpieczenSpolecznych = DevExpressXXL.GetBoundColumn("Wpływ");

            string[] teksty01 = new string[] { "Ogółem", "K", "W", "Kop", "Kp", "Ko karne", "Ko wykrocz.", "WSNk" };

            ASPxGridView1.Columns.Add(DevExpressXXL.podKolumna(teksty01, 1, idDzialu, false, szerokoscKolumny, "WPŁYW"));

            ASPxGridView1.Columns.Add(DevExpressXXL.SekcjaDwiePodKolumny(teksty01, "Załatwiono", 9, idDzialu, szerokoscKolumny));
            ASPxGridView1.Columns.Add(DevExpressXXL.podKolumna(teksty01, 25, idDzialu, false, szerokoscKolumny, "Załatwienia"));

            #region sesje odbyte przez sędziego

            GridViewBandColumn sesjeSedziego = DevExpressXXL.GetBoundColumn("sesje odbyte przez sędziego ");

            GridViewBandColumn wszystkieSesjeSedziego = DevExpressXXL.GetBoundColumn("(na potrzeby MS-S)");
            wszystkieSesjeSedziego.Columns.Add(DevExpressXXL.kolumnaDoTabeli("ogółem", "d_33", idDzialu, "", false, szerokoscKolumny));
            wszystkieSesjeSedziego.Columns.Add(DevExpressXXL.podKolumna(new string[] { "rozprawy", "posiedzenia jawne", "posiedzenia niejawne" }, 34, idDzialu, false, szerokoscKolumny, "z tego "));
            sesjeSedziego.Columns.Add(wszystkieSesjeSedziego);

            ASPxGridView1.Columns.Add(sesjeSedziego);

            #endregion sesje odbyte przez sędziego

            ASPxGridView1.Columns.Add(DevExpressXXL.podKolumna(teksty01, 37, idDzialu, false, szerokoscKolumny, "POZOSTAŁOŚĆ na następny m-c"));
            ASPxGridView1.Columns.Add(DevExpressXXL.podKolumna(new string[] { "Ogółem", "do 3 mie- sięcy", "pow. 3 do 6 m-cy", "pow. 6 do 12 m-cy", "pow.12 m-cy do 2 lat", "pow. 2 do 3 lat", "pow. 3 do 5 lat", "pow. 5 do 8 lat", "pow. 8 lat" }, 45, idDzialu, false, szerokoscKolumny, "pozostało spraw starych (wszystkie kategorie spraw)"));
            ASPxGridView1.Columns.Add(DevExpressXXL.podKolumna(new string[] { "Ogółem", "zakreś- lonych", "nie zakreś- lonych" }, 54, idDzialu, false, szerokoscKolumny, "stan spraw zawieszonych(wszystkie kategorie spraw, zgodnie z MS - S5r)"));

            GridViewBandColumn liczbaSporzadzonychUzasadnien = DevExpressXXL.podKolumna(new string[4] { "Łącznie", "w terminie ustawowym 14 dni", "razem po terminie ustawowym", "nie- usprawied- liwione" }, 57, idDzialu, false, szerokoscKolumny, "terminowość sporządzania uzasadnień na wniosek przez sędziów i referendarzy sądowych (zgodnie z MS - S5r, dz. 1.3.1.a + 1.3.1.c) * ");
            GridViewBandColumn PoUplywie = (DevExpressXXL.podKolumna(new string[8] { "1-14 dni", "w tym nieuspra -wiedliwione", "15-30 dni", "w tym nieuspra -wiedliwione", "powyżej 1 do 3 mies", "w tym nieuspra -wiedliwione", "ponad 3 mies", "w tym nieuspra -wiedliwione" }, 61, idDzialu, false, szerokoscKolumny, "po upływie terminiu ustawowego"));
            liczbaSporzadzonychUzasadnien.Columns.Add(PoUplywie);
            liczbaSporzadzonychUzasadnien.Columns.Add(DevExpressXXL.kolumnaDoTabeli("Uzasadnienia wygłoszone **/***", "d_69", idDzialu, "", false, szerokoscKolumny));
            liczbaSporzadzonychUzasadnien.Columns.Add(DevExpressXXL.kolumnaDoTabeli("Liczba spraw, do których wpłynął wniosek o transkrypcje uzasadnień wygłoszonych", "d_70", idDzialu, "", false, szerokoscKolumny));
            ASPxGridView1.Columns.Add(liczbaSporzadzonychUzasadnien);



            GridViewBandColumn liczbaSporzadzonychUzasadnien2 = DevExpressXXL.podKolumna(new string[4] { "Łącznie", "w terminie ustawowym 14 dni", "razem po terminie ustawowym", "nie- usprawied- liwione" }, 71, idDzialu, false, szerokoscKolumny, "terminowość sporządzania uzasadnień z urzędu  przez sędziów i referendarzy sądowych (zgodnie z MS - S5r, dz. 1.3.1.b + 1.3.1.d) * ");
            GridViewBandColumn PoUplywie2 = (DevExpressXXL.podKolumna(new string[8] { "1-14 dni", "w tym nieuspra -wiedliwione", "15-30 dni", "w tym nieuspra -wiedliwione", "powyżej 1 do 3 mies", "w tym nieuspra -wiedliwione", "ponad 3 mies", "w tym nieuspra -wiedliwione" }, 75, idDzialu, false, szerokoscKolumny, "po upływie terminiu ustawowego"));

            liczbaSporzadzonychUzasadnien2.Columns.Add(PoUplywie2);
            liczbaSporzadzonychUzasadnien2.Columns.Add(DevExpressXXL.kolumnaDoTabeli("Uzasadnienia wygłoszone **/***", "d_83", idDzialu, "", false, szerokoscKolumny));
            liczbaSporzadzonychUzasadnien2.Columns.Add(DevExpressXXL.kolumnaDoTabeli("Liczba spraw, do których wpłynął wniosek o transkrypcje uzasadnień wygłoszonych", "d_84", idDzialu, "", false, szerokoscKolumny));
            ASPxGridView1.Columns.Add(liczbaSporzadzonychUzasadnien2);



            GridViewBandColumn zalatwiono01skargiNaPrzewleklosc = DevExpressXXL.GetBoundColumn("skargi na przewlekłość");
            zalatwiono01skargiNaPrzewleklosc.Columns.Add(DevExpressXXL.kolumnaDoTabeli("wpływ", "d_85", idDzialu, "", false, szerokoscKolumny));

            zalatwiono01skargiNaPrzewleklosc.Columns.Add(DevExpressXXL.podKolumna(new string[2] { "ogółem", "uwzględniono" }, 86, idDzialu, false, szerokoscKolumny, "załatwiono"));
            zalatwiono01skargiNaPrzewleklosc.Columns.Add(DevExpressXXL.kolumnaDoTabeli("pozostałość", "d_88", idDzialu, "", false, szerokoscKolumny));
            ASPxGridView1.Columns.Add(zalatwiono01skargiNaPrzewleklosc);

            ASPxGridView1.Columns.Add(DevExpressXXL.kolumnaDoTabeli("Uwagi", "d_89", idDzialu, "", false, szerokoscKolumny));

            ASPxGridView1.TotalSummary.Clear();
            ASPxGridView1.TotalSummary.Add(DevExpressXXL.komorkaSumujaca("Ogółem"));
            for (int i = 1; i < 102; i++)
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