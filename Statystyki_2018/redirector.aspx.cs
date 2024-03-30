using System;
using System.Data;
using System.Globalization;
using System.Web.UI;

namespace Statystyki_2018
{
    public partial class redirector : System.Web.UI.Page
    {
        private common cm = new common();

        protected void Page_Load(object sender, EventArgs e)
        {
            cm.log.Info("start redirektora  " + DateTime.Now.ToLongTimeString());

            if (string.IsNullOrEmpty((string)Session["identyfikatorUzytkownika"]))
            {
                Server.Transfer("default.aspx");
            }
            CultureInfo newCulture = (CultureInfo)CultureInfo.CurrentCulture.Clone();
            newCulture.DateTimeFormat = CultureInfo.GetCultureInfo("PL").DateTimeFormat;
            System.Threading.Thread.CurrentThread.CurrentCulture = newCulture;
            System.Threading.Thread.CurrentThread.CurrentUICulture = newCulture;
            string ktoreMenu = Request.QueryString["id"];
            string ktoraCzesc = (string)Session["elementMenu"];
            string typMenu = cm.getQuerryValue("select wartosc from konfig where klucz = 'typMenu'", cm.con_str, cm.makeParameterTable(), "SiteMAster");
            cm.log.Info("Typ menu " + typMenu);
            String IdentyfikatorUzytkownika = string.Empty;
            if (typMenu == "2")

            {
                try
                {
                    //cm.log.Info("Site1.Master: Przypisanie identyfikatora uzytkownika");
                    IdentyfikatorUzytkownika = (string)Session["identyfikatorUzytkownika"];

                    int userDigitalId = 0;
                    try
                    {
                        userDigitalId = int.Parse(IdentyfikatorUzytkownika);
                    }
                    catch
                    {
                        cm.log.Error("redirector->Page_Load:Brak przeniesienia z logowania identyfikatora numerycznego Użytkownika! ");
                    }
                    //cm.log.Info("Site1.Master: Identyfikatora uzytkownika: " + IdentyfikatorUzytkownika);
                }
                catch
                {
                    cm.log.Error("redirector->Page_Load: Nie przypisanoe identyfikatora użytkownika");
                }
                DataTable parametry = cm.makeParameterTable();
                parametry.Rows.Add("@identyfikatorUzytkownika", IdentyfikatorUzytkownika);

                PanelMenuGlowne.Visible = true;
                if (!string.IsNullOrEmpty(ktoreMenu))
                {
                    if (!string.IsNullOrEmpty((string)Session["elementMenu"]))
                    {
                        PanelMenuGlowne.Visible = false;
                        menuKategorii.Visible = true;
                        switch (ktoraCzesc)
                        {
                            case "Statystyczne":
                                {
                                    ElementyMenuStatystyczne();
                                }
                                break;

                            case "Kontrolki":
                                {
                                    ElementyMenuKontrolki();
                                }
                                break;

                            case "MSS":
                                {
                                    ElementyMenuMSS();
                                }
                                break;

                            case "Inne":
                                {
                                    ElementyMenuInne();
                                }
                                break;

                            default:
                                Session["elementMenu"] = "";
                                Session["czesc"] = "";
                                break;
                        }
                    }
                    else
                    {
                        PanelMenuGlowne.Visible = true;
                        menuKategorii.Visible = false;
                    }
                }
                else
                {
                    PanelMenuGlowne.Visible = true;
                    menuKategorii.Visible = false;
                }
                try
                {
                    string identyfikatorUzytkownika = (string)Session["identyfikatorUzytkownika"];

                    parametry.Rows.Add("@UserId", identyfikatorUzytkownika);

                    Button6.Visible = false;
                    if (cm.getQuerryValue("select admin from uzytkownik where ident = @UserId", cm.con_str, parametry, "SiteMAster") != "0")
                    {
                        Button6.Visible = true;
                    }
                }
                catch (Exception ex)
                {
                    cm.log.Error(ex.Message);
                }
            }

            if (!IsPostBack)
            {
                cardView.SearchPanelFilter = "";
                cardView.DataBind();
            }

        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            ElementyMenuStatystyczne();
        }

        protected void Button3_Click(object sender, EventArgs e)
        {
            ElementyMenuKontrolki();
        }

        protected void Button4_Click(object sender, EventArgs e)
        {
            ElementyMenuMSS();
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            ElementyMenuInne();
        }

        protected void Button5_Click(object sender, EventArgs e)
        {
            Session["elementMenu"] = "Wymiana";
            cardView.Visible = false;
            Server.Transfer("Wymiana1.aspx");
        }

      

        protected void Button7_Click(object sender, EventArgs e)
        {
            Server.Transfer(" default.aspx?logout=true");
        }

        private void zaladujDaneDoMenu(string kwerenda, string IdentyfikatorUzytkownika)
        {
            cardView.Visible = true;
            cardView.FilterExpression = "";
            cardView.SearchPanelFilter = "";
            DataTable parametry = cm.makeParameterTable();
            parametry.Rows.Add("@identyfikatorUzytkownika", IdentyfikatorUzytkownika);
            DataTable statystyczne = new DataTable();
            statystyczne = cm.getDataTable(kwerenda, cm.con_str, parametry, "redirector");
            cardView.DataSource = null;
            cardView.DataSourceID = null;
            cardView.DataSource = statystyczne;
            cardView.DataBind();
        }


        private void zaladujDaneDoMenuInne(DataTable pozycje)
        {
            cardView.Visible = true;
            cardView.FilterExpression = "";
            cardView.SearchPanelFilter = "";

            cardView.DataSource = null;
            cardView.DataSourceID = null;
            cardView.DataSource = pozycje;
            cardView.DataBind();
        }

        protected void select(object sender, EventArgs e)
        {
            string nazwa = cardView.GetCardValuesByKeyValue("nazwa").ToString();
            Session["czesc"] = nazwa;
        }

        private void ElementyMenuStatystyczne()
        {
            int stat_Count = 0;
            try
            {
                stat_Count = (int)Session["stat_Count"];
            }
            catch 
            {}
            stat_Count += 1;

            DataTable parametry = cm.makeParameterTable();
            parametry.Rows.Add("@identyfikatorUzytkownika", (string)Session["identyfikatorUzytkownika"]);
            
            Session["elementMenu"] = "Statystyczne";
            Session["czesc"] = "";
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "SetText", "JavaScript:SetText('Statystyczne');", true);

            string admin = "0";

            try
            {
                admin = cm.getQuerryValue("select admin from uzytkownik where ident =@identyfikatorUzytkownika", cm.con_str, parametry);
                //log.Info("Header: Użytkownik ma prawa administracyjne");
            }
            catch
            { }
            string kwerenda = "SELECT DISTINCT wydzialy.ident as wydzial , wydzialy.nazwa as nazwa, wydzialy.plik as plik FROM wydzialy INNER JOIN uprawnienia ON wydzialy.ident = uprawnienia.id_wydzialu WHERE(uprawnienia.rodzaj = 1) AND(uprawnienia.id_uzytkownika = @identyfikatorUzytkownika) order by wydzialy.nazwa";

            if (admin == "1")
            {             
                kwerenda = "SELECT DISTINCT wydzialy.ident as wydzial, wydzialy.nazwa as nazwa, wydzialy.plik as plik  FROM wydzialy order by wydzialy.nazwa";
            }

            Session["pozycjaMenu"] = kwerenda;
            PanelMenuGlowne.Visible = false;
            zaladujDaneDoMenu(kwerenda, (string)Session["identyfikatorUzytkownika"]);
            menuKategorii.Visible = true;
            cardView.DataBind();
            cardView.DoCardValidation();
            String lastKwerenda = kwerenda;
            kwerenda = "";
            zaladujDaneDoMenu(kwerenda, (string)Session["identyfikatorUzytkownika"]);
            menuKategorii.Visible = true;
            cardView.DataBind();
            zaladujDaneDoMenu(lastKwerenda, (string)Session["identyfikatorUzytkownika"]);
            menuKategorii.Visible = true;
            cardView.DataBind();
            if (stat_Count == 1)
            {
                Session["stat_Count"] = stat_Count;
                string nazwa = (string)Session["elementMenu"];
                Server.Transfer("redirector.aspx?id=" + nazwa);
            }
            
        }

        private void ElementyMenuMSS()
        {
            int stat_Count = 0;
            try
            {
                stat_Count = (int)Session["stat_Count"];
            }
            catch
            { }
            stat_Count += 1;
            String IdentyfikatorUzytkownika = string.Empty;
            IdentyfikatorUzytkownika = (string)Session["identyfikatorUzytkownika"];
            DataTable parametry = cm.makeParameterTable();
            parametry.Rows.Add("@identyfikatorUzytkownika", IdentyfikatorUzytkownika);
            Session["elementMenu"] = "MSS";
            Session["czesc"] = "";
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "print2", "JavaScript:SetText('MSS');", true);

            string kwerenda = string.Empty;
            parametry.Rows.Add("@identyfikatorUzytkownika", IdentyfikatorUzytkownika);
            // czy admin
            if (cm.getQuerryValue("select admin from uzytkownik where ident =@identyfikatorUzytkownika", cm.con_str, parametry) == "1")
            {
                kwerenda = "SELECT DISTINCT wydzialy_mss.ident as wydzial , wydzialy_mss.nazwa as nazwa, wydzialy_mss.plik as plik FROM wydzialy_mss order by wydzialy_mss.nazwa";                                 // admin
            }
            else
            {
                kwerenda = "SELECT DISTINCT wydzialy_mss.ident as wydzial, wydzialy_mss.nazwa as nazwa, wydzialy_mss.plik as plik FROM wydzialy_mss INNER JOIN uprawnienia ON wydzialy_mss.ident = uprawnienia.id_wydzialu WHERE(uprawnienia.rodzaj = 2) AND(uprawnienia.id_uzytkownika = @identyfikatorUzytkownika) order by wydzialy_mss.nazwa";
            }

            Session["pozycjaMenu"] = kwerenda;
            zaladujDaneDoMenu(kwerenda, IdentyfikatorUzytkownika);
            PanelMenuGlowne.Visible = false;
            menuKategorii.Visible = true;
            if (stat_Count == 1)
            {
                Session["stat_Count"] = stat_Count;
                string nazwa = (string)Session["elementMenu"];
                Server.Transfer("redirector.aspx?id=" + nazwa);
            }
            cardView.DataBind();
        }

        private void ElementyMenuKontrolki()
        {
            int stat_Count = 0;
            try
            {
                stat_Count = (int)Session["stat_Count"];
            }
            catch
            { }
            stat_Count += 1;
            DataTable parametry = cm.makeParameterTable();
            parametry.Rows.Add("@identyfikatorUzytkownika", (string)Session["identyfikatorUzytkownika"]);

            Session["elementMenu"] = "Kontrolki";
            Session["czesc"] = "";
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "SetText", "JavaScript:SetText('Kontrolki');", true);

            string admin = "0";

            try
            {
                admin = cm.getQuerryValue("select admin from uzytkownik where ident =@identyfikatorUzytkownika", cm.con_str, parametry);
                //log.Info("Header: Użytkownik ma prawa administracyjne");
            }
            catch
            { }
            string kwerenda = "SELECT DISTINCT  konfig.ident AS wydzial,konfig.opis AS nazwa, 'kontrolka2022.aspx' AS plik FROM konfig RIGHT OUTER JOIN   uprawnienia ON konfig.ident = uprawnienia.id_wydzialu WHERE(konfig.klucz = N'kontrolka') AND(uprawnienia.id_uzytkownika = @identyfikatorUzytkownika) AND(uprawnienia.rodzaj = 3) order by opis";

            if (admin == "1")
            {
                string odp = cm.getQuerryValue("SELECT        COUNT(id_uzytkownika) AS Expr1 FROM uprawnienia WHERE ( id_uzytkownika = @identyfikatorUzytkownika) AND (rodzaj = 3)", cm.con_str, parametry);
                if (odp=="0")
                {
                    kwerenda = "";
                    return;
                }
                kwerenda = "SELECT ident as wydzial, opis as nazwa, 'kontrolka2022.aspx' as plik FROM konfig  WHERE(klucz = 'kontrolka') order by opis";
            }

            Session["pozycjaMenu"] = kwerenda;
            PanelMenuGlowne.Visible = false;
            zaladujDaneDoMenu(kwerenda, (string)Session["identyfikatorUzytkownika"]);
            menuKategorii.Visible = true;

            if (stat_Count == 1)
            {
                Session["stat_Count"] = stat_Count;
                string nazwa = (string)Session["elementMenu"];
                Server.Transfer("redirector.aspx?id=" + nazwa);
            }
            cardView.DataBind();

        }

        private void ElementyMenuInne()
        {
            int stat_Count = 0;
            try
            {
                stat_Count = (int)Session["stat_Count"];
            }
            catch
            { }
            stat_Count += 1;
            String IdentyfikatorUzytkownika = string.Empty;
            IdentyfikatorUzytkownika = (string)Session["identyfikatorUzytkownika"];
            DataTable parametry = cm.makeParameterTable();

            string kwerenda = string.Empty;

            DataTable pozycje = new DataTable();
            DataColumn dt = new DataColumn();
            dt.ColumnName = "wydzial";
            dt.DataType = typeof(int);
            pozycje.Columns.Add(dt);
            dt = new DataColumn();
            dt.ColumnName = "nazwa";
            dt.DataType = typeof(string);
            pozycje.Columns.Add(dt);
            dt = new DataColumn();
            dt.ColumnName = "plik";
            dt.DataType = typeof(string);
            pozycje.Columns.Add(dt);
            try
            {
                parametry.Rows.Add("@identyfikatorUzytkownika", int.Parse(IdentyfikatorUzytkownika));
                int iloscUprawnienOceny = int.Parse(cm.getQuerryValue("SELECT COUNT(*) FROM  uprawnienia WHERE (rodzaj = 6) and id_uzytkownika=@identyfikatorUzytkownika", cm.con_str, parametry, "Header - sprawdzanie oceny pracownika"));
                if (iloscUprawnienOceny > 0)
                {
                    DataRow dr = pozycje.NewRow();
                    dr[0] = 1;
                    dr[1] = "Ocena pracownika Nowa";
                    dr[2] = "ocenaPracownika.aspx";
                    pozycje.Rows.Add(dr);
                }
                int iloscUprawnienKOF = int.Parse(cm.getQuerryValue("SELECT COUNT(*) FROM  uprawnienia WHERE (rodzaj = 4) and id_uzytkownika=@identyfikatorUzytkownika", cm.con_str, parametry, "Header - sprawdzanie oceny pracownika"));
                if (iloscUprawnienKOF > 0)
                {
                    DataRow dr = pozycje.NewRow();
                    dr[0] = 1;
                    dr[1] = "Kontrolka KOF";
                    dr[2] = "kof.aspx";
                    pozycje.Rows.Add(dr);
                }
                int iloscUprawnienInne = int.Parse(cm.getQuerryValue("SELECT        COUNT(konfig.ident) AS Expr1 FROM            konfig INNER JOIN uprawnienia ON konfig.ident = uprawnienia.id_wydzialu WHERE(uprawnienia.rodzaj = 5) AND(uprawnienia.id_uzytkownika = @identyfikatorUzytkownika)", cm.con_str, parametry, "Header - sprawdzanie oceny pracownika"));
                
                if (iloscUprawnienInne > 0)
                {
                    //                kwerenda = "SELECT DISTINCT konfig.ident, konfig.opis, konfig.wartosc, konfig.klucz FROM uprawnienia INNER JOIN konfig ON uprawnienia.id_wydzialu  = konfig.ident WHERE        (uprawnienia.id_uzytkownika = @identyfikatorUzytkownika) AND (uprawnienia.rodzaj =3 )  AND (rtrim(konfig.klucz) = 'kontrolka') order by konfig.opis";

                    DataRow dr = pozycje.NewRow();
                    dr[0] = 1;
                    dr[1] = "Wyszukiwarka";
                    dr[2] = "wyszukiwarkaspraw.aspx";
                    pozycje.Rows.Add(dr);
                }
                if (pozycje.Rows.Count > 0)
                {
                    Session["elementMenu"] = "Inne";
                    Session["czesc"] = "";
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "SetText", "JavaScript:SetText('Inne');", true);
                    Session["pozycjaMenu"] = kwerenda;
                    zaladujDaneDoMenuInne(pozycje);
                    PanelMenuGlowne.Visible = false;
                    menuKategorii.Visible = true;
                }
            }
            catch (Exception ex)
            {
                cm.log.Error(ex.Message);
            }
            if (stat_Count == 1)
            {
                Session["stat_Count"] = stat_Count;
                string nazwa = (string)Session["elementMenu"];
                Server.Transfer("redirector.aspx?id=" + nazwa);
            }
            cardView.DataBind();
        }

        protected void Administracja_Click(object sender, EventArgs e)
        {
            int stat_Count = 0;
            try
            {
                stat_Count = (int)Session["stat_Count"];
            }
            catch
            { }
            stat_Count += 1;
            String IdentyfikatorUzytkownika = string.Empty;
            IdentyfikatorUzytkownika = (string)Session["identyfikatorUzytkownika"];
            DataTable parametry = cm.makeParameterTable();
            parametry.Rows.Add("@identyfikatorUzytkownika", IdentyfikatorUzytkownika);
            Session["elementMenu"] = "Administracja";
            Session["czesc"] = "";
            if (cm.getQuerryValue("select admin from uzytkownik where ident =@identyfikatorUzytkownika", cm.con_str, parametry) == "1")
            {
                Session["elementMenu"] = "Administracja";
                cardView.Visible = false;
                Server.Transfer("adm.aspx");
            }
            if (stat_Count == 1)
            {
                Session["stat_Count"] = stat_Count;
                string nazwa = (string)Session["elementMenu"];
                Server.Transfer("redirector.aspx?id=" + nazwa);
            }

        }
    }

    
}