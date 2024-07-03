using System;
using System.Data;
using DevExpress.Web;

namespace Statystyki_2018.UserControlls
{
    public partial class ZarzadzanieFormularzami : System.Web.UI.UserControl
    {
        private const string tenPlik = "zarzadzanie uprawnieniami";
        private common cm = new common();

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            wypelnijRodzaje();
            string idUsera = "0";
            try
            {
                cm.log.Info("zarzadzanie formularzami start");
                idUsera = (string)Session["identyfikatorUzytkownika"];
                cm.log.Info("zarzadzanie formularzami idUsera=" + idUsera.ToString());
            }
            catch (Exception ex)
            {
                ASPxGridView1.Visible = false;
                ASPxGridView2.Visible = false;
                ASPxGridView4.Visible = false;
                ASPxGridView5.Visible = false;
                ASPxGridView6.Visible = false;
                ASPxGridView7.Visible = false;
                cm.log.Error("zarzadzanie formularzami bład " + ex.Message);
            }
        }
        
        protected void wypelnijRodzaje()
        {
            DevExpress.Web.ListEditItem listEditItem = new DevExpress.Web.ListEditItem();

            if (CBRodzaje.Items.Count == 0)
            {
                listEditItem.Text = "Statystyki miesięczne";
                listEditItem.Value = (int)Rodzaje.miesieczne;
                CBRodzaje.Items.Add(listEditItem);
                
                listEditItem = new DevExpress.Web.ListEditItem
                {
                    Text = "MSS",
                    Value = (int)Rodzaje.MSS
                };
                CBRodzaje.Items.Add(listEditItem);


                listEditItem = new DevExpress.Web.ListEditItem
                {
                    Text = "Kontrolki",
                    Value = (int)Rodzaje.kontrolki
                };
                CBRodzaje.Items.Add(listEditItem);

                listEditItem = new DevExpress.Web.ListEditItem
                {
                    Text = "Kof",
                    Value = (int)Rodzaje.kof
                };
                CBRodzaje.Items.Add(listEditItem);

                listEditItem = new DevExpress.Web.ListEditItem
                {
                    Text = "Wyszukiwarka",
                    Value = (int)Rodzaje.wyszukiwarka
                };
                CBRodzaje.Items.Add(listEditItem);

                listEditItem = new DevExpress.Web.ListEditItem
                {
                    Text = "Pracownik",
                    Value = (int)Rodzaje.pracownik
                };
                listEditItem = new DevExpress.Web.ListEditItem
                {
                    Text = "Wymiana",
                    Value = (int)Rodzaje.wymiana
                };
                CBRodzaje.Items.Add(listEditItem);

                listEditItem = new DevExpress.Web.ListEditItem
                {
                    Text = "Kontrolki6X",
                    Value = (int)Rodzaje.kontrolki6X
                };
                CBRodzaje.Items.Add(listEditItem);

                CBRodzaje.SelectedIndex = 0;
                CBRodzaje.DataBind();
            }

            if (CBRodzaje.SelectedIndex == -1)
            {
                CBRodzaje.DataBind();
                CBRodzaje.SelectedIndex = 0;
            }
        }

        protected void startedycji(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            cm.log.Info("zarzadzanie formularzami start edycji" + e.EditingKeyValue.ToString());
        }

        protected void startEdycji(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            cm.log.Info("zarzadzanie formularzami start edycji" + e.EditingKeyValue.ToString());
        }
        protected void EdycjaUprawnien(string klucz,string nowaWartosc01,string nowaWartosc02)
        {

            cm.log.Info("zarzadzanie formularzami Edycja Keys[0] " + klucz);

            cm.log.Info("zarzadzanie formularzami Edycja e.NewValues[0]" + nowaWartosc01);
            cm.log.Info("zarzadzanie formularzami Edycja e.NewValues[1]" + nowaWartosc02);
            int uzytkownik = int.Parse((string)Session["identyfikatorUzytkownika"]);
            uprawnienia Uprawnienia = new uprawnienia();
            int wydzial = int.Parse(klucz);
            int rodzaj = 0;
            try
            {
                rodzaj = int.Parse(CBRodzaje.SelectedItem.Value.ToString());
            }
            catch (Exception)
            {
            }
            cm.log.Info(" CBRodzaje.SelectedItem.Value.ToString() " + CBRodzaje.SelectedItem.Value.ToString());

            if (int.Parse(nowaWartosc01) == 0)
            {
                switch (rodzaj)
                {
                    case 1: Uprawnienia.Miesieczne.usun(wydzial, uzytkownik); break;
                    case 2: Uprawnienia.Mss.usun(wydzial, uzytkownik); break;
                    case 3: Uprawnienia.kontrolki.usun(wydzial, uzytkownik); break;
                    case 4: Uprawnienia.kof.usun(wydzial, uzytkownik); break;
                    case 5: Uprawnienia.wyszukiwarka.usun(wydzial, uzytkownik); break;
                    case 6: Uprawnienia.pracownik.usun(wydzial, uzytkownik); break;
                    case 7: Uprawnienia.wymiana.usun(wydzial, uzytkownik); break;
                  //  case 8: Uprawnienia.kontrolki6x.usun(wydzial, uzytkownik); break;
                    default:
                        break;
                }
            }
            else
            {
                switch (rodzaj)
                {
                    case 1: Uprawnienia.Miesieczne.dodaj(wydzial, uzytkownik); break;
                    case 2: Uprawnienia.Mss.dodaj(wydzial, uzytkownik); break;
                    case 3: Uprawnienia.kontrolki.dodaj(wydzial, uzytkownik); break;
                    case 4: Uprawnienia.kof.dodaj(wydzial, uzytkownik); break;
                    case 5: Uprawnienia.wyszukiwarka.dodaj(wydzial, uzytkownik); break;
                    case 6: Uprawnienia.pracownik.dodaj(wydzial, uzytkownik); break;
                    case 7: Uprawnienia.wymiana.dodaj(wydzial, uzytkownik); break;
                 //   case 8: Uprawnienia.kontrolki6x.dodaj(wydzial, uzytkownik); break;
                    default:
                        break;
                }
            }
        }
        protected void Edycja(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            string klucz=   e.Keys[0].ToString();
            string nowaWartosc01 = e.NewValues[0].ToString();
            string nowaWartosc02 = e.NewValues[1].ToString();
            EdycjaUprawnien(klucz, nowaWartosc01, nowaWartosc02);
        }
        protected void PoEdycji(object sender, DevExpress.Web.Data.ASPxDataUpdatedEventArgs e)
        {
            cm.log.Info("zarzadzanie formularzami po edycji" + e.AffectedRecords.ToString());
        }

        private class uprawnienia
        {
            public miesieczne Miesieczne = new miesieczne();
            public MSS Mss = new MSS();
            public KOF kof = new KOF();
            public Kontrolki kontrolki = new Kontrolki();
            public Wyszukiwarka wyszukiwarka = new Wyszukiwarka();
            public Pracownik pracownik = new Pracownik();
            public Wymiana wymiana = new Wymiana();
            public Kontrolki6X kontrolki6x = new Kontrolki6X();
        }

        public class miesieczne : common
        {
            public void dodaj(int wydzial, int uzytkownik)
            {
                log.Info("dodawanie miesieczne");
                DataTable parametry = makeParameterTable();
                parametry.Rows.Add("@idWydzialu", wydzial);
                parametry.Rows.Add("@id_uzytkownika", uzytkownik);
                runQuerry(" IF NOT EXISTS(SELECT * FROM uprawnienia WHERE id_wydzialu = @idWydzialu AND rodzaj = 1 AND id_uzytkownika = @id_uzytkownika)    BEGIN       insert INTO uprawnienia (id_wydzialu,rodzaj,id_uzytkownika) VALUES  (@idWydzialu,1,@id_uzytkownika)    END", con_str, parametry);
            }

            public void usun(int wydzial, int uzytkownik)
            {
                log.Info("usuwanie miesieczne");
                DataTable parametry = makeParameterTable();
                parametry.Rows.Add("@idWydzialu", wydzial);
                parametry.Rows.Add("@id_uzytkownika", uzytkownik);
                runQuerry(" delete from uprawnienia where  id_wydzialu=@idWydzialu and rodzaj=1 and id_uzytkownika=@id_uzytkownika", con_str, parametry);
            }
        }
        public class Wymiana : common
        {
            public void dodaj(int wydzial, int uzytkownik)
            {
                log.Info("dodawanie miesieczne");
                DataTable parametry = makeParameterTable();
                parametry.Rows.Add("@idWydzialu", wydzial);
                parametry.Rows.Add("@id_uzytkownika", uzytkownik);
                runQuerry(" IF NOT EXISTS(SELECT * FROM uprawnienia WHERE id_wydzialu = @idWydzialu AND rodzaj = 7 AND id_uzytkownika = @id_uzytkownika)    BEGIN       insert INTO uprawnienia (id_wydzialu,rodzaj,id_uzytkownika) VALUES  (@idWydzialu,7,@id_uzytkownika)    END", con_str, parametry);
            }

            public void usun(int wydzial, int uzytkownik)
            {
                log.Info("usuwanie miesieczne");
                DataTable parametry = makeParameterTable();
                parametry.Rows.Add("@idWydzialu", wydzial);
                parametry.Rows.Add("@id_uzytkownika", uzytkownik);
                runQuerry(" delete from uprawnienia where  id_wydzialu=@idWydzialu and rodzaj=7 and id_uzytkownika=@id_uzytkownika", con_str, parametry);
            }
        }
        public class MSS : common
        {
            public void dodaj(int wydzial, int uzytkownik)
            {
                log.Info("mss dodawanie");
                DataTable parametry = makeParameterTable();
                parametry.Rows.Add("@idWydzialu", wydzial);
                parametry.Rows.Add("@id_uzytkownika", uzytkownik);
                runQuerry(" IF NOT EXISTS(SELECT * FROM uprawnienia WHERE id_wydzialu = @idWydzialu AND rodzaj = 2 AND id_uzytkownika = @id_uzytkownika)    BEGIN       insert INTO uprawnienia (id_wydzialu,rodzaj,id_uzytkownika) VALUES  (@idWydzialu,2,@id_uzytkownika)    END", con_str, parametry);
            }

            public void usun(int wydzial, int uzytkownik)
            {
                log.Info("usuwanie MSS");
                DataTable parametry = makeParameterTable();
                parametry.Rows.Add("@idWydzialu", wydzial);
                parametry.Rows.Add("@id_uzytkownika", uzytkownik);
                runQuerry(" delete from uprawnienia where  id_wydzialu=@idWydzialu and rodzaj=2 and id_uzytkownika=@id_uzytkownika", con_str, parametry);
            }
        }

        public class KOF : common
        {
            public void dodaj(int wydzial, int uzytkownik)
            {
                log.Info("mss dodawanie");
                DataTable parametry = makeParameterTable();
                parametry.Rows.Add("@idWydzialu", wydzial);
                parametry.Rows.Add("@id_uzytkownika", uzytkownik);
                runQuerry(" IF NOT EXISTS(SELECT * FROM uprawnienia WHERE id_wydzialu = @idWydzialu AND rodzaj = 4 AND id_uzytkownika = @id_uzytkownika)    BEGIN       insert INTO uprawnienia (id_wydzialu,rodzaj,id_uzytkownika) VALUES  (@idWydzialu,4,@id_uzytkownika)    END", con_str, parametry);
            }

            public void usun(int wydzial, int uzytkownik)
            {
                log.Info("usuwanie MSS");
                DataTable parametry = makeParameterTable();
                parametry.Rows.Add("@idWydzialu", wydzial);
                parametry.Rows.Add("@id_uzytkownika", uzytkownik);
                runQuerry(" delete from uprawnienia where  id_wydzialu=@idWydzialu and rodzaj=4 and id_uzytkownika=@id_uzytkownika", con_str, parametry);
            }
        }
        public class Kontrolki : common
        {
            public void dodaj(int wydzial, int uzytkownik)
            {
                log.Info("Kontrolki dodawanie");
                DataTable parametry = makeParameterTable();
                parametry.Rows.Add("@idWydzialu", wydzial);
                parametry.Rows.Add("@id_uzytkownika", uzytkownik);
                runQuerry(" IF NOT EXISTS(SELECT * FROM uprawnienia WHERE id_wydzialu = @idWydzialu AND rodzaj = 3 AND id_uzytkownika = @id_uzytkownika)    BEGIN       insert INTO uprawnienia (id_wydzialu,rodzaj,id_uzytkownika) VALUES  (@idWydzialu,3,@id_uzytkownika)    END", con_str, parametry,"Dodawanie uprawnień dla KOF");
            }

            public void usun(int wydzial, int uzytkownik)
            {
                log.Info("usuwanie Kontrolki");
                DataTable parametry = makeParameterTable();
                parametry.Rows.Add("@idWydzialu", wydzial);
                parametry.Rows.Add("@id_uzytkownika", uzytkownik);
                runQuerry(" delete from uprawnienia where  id_wydzialu=@idWydzialu and rodzaj=3 and id_uzytkownika=@id_uzytkownika", con_str, parametry);
            }
        }
     
        public class Wyszukiwarka  : common
        {
            public void dodaj(int wydzial, int uzytkownik)
            {
                log.Info("Wyszukiwarka dodawanie");
                DataTable parametry = makeParameterTable();
                parametry.Rows.Add("@idWydzialu", wydzial);
                parametry.Rows.Add("@id_uzytkownika", uzytkownik);
                runQuerry(" IF NOT EXISTS(SELECT * FROM uprawnienia WHERE id_wydzialu = @idWydzialu AND rodzaj = 5 AND id_uzytkownika = @id_uzytkownika)    BEGIN       insert INTO uprawnienia (id_wydzialu,rodzaj,id_uzytkownika) VALUES  (@idWydzialu,5,@id_uzytkownika)    END", con_str, parametry);
            }

            public void usun(int wydzial, int uzytkownik)
            {
                log.Info("Wyszukiwarka Kontrolki");
                DataTable parametry = makeParameterTable();
                parametry.Rows.Add("@idWydzialu", wydzial);
                parametry.Rows.Add("@id_uzytkownika", uzytkownik);
                runQuerry(" delete from uprawnienia where  id_wydzialu=@idWydzialu and rodzaj=5 and id_uzytkownika=@id_uzytkownika", con_str, parametry);
            }
        }

        public class Pracownik : common
        {
            public void dodaj(int wydzial, int uzytkownik)
            {
                log.Info("Wyszukiwarka dodawanie");
                DataTable parametry = makeParameterTable();
                parametry.Rows.Add("@idWydzialu", wydzial);
                parametry.Rows.Add("@id_uzytkownika", uzytkownik);
                runQuerry(" IF NOT EXISTS(SELECT * FROM uprawnienia WHERE id_wydzialu = @idWydzialu AND rodzaj = 6 AND id_uzytkownika = @id_uzytkownika)    BEGIN       insert INTO uprawnienia (id_wydzialu,rodzaj,id_uzytkownika) VALUES  (@idWydzialu,6,@id_uzytkownika)    END", con_str, parametry);
            }

            public void usun(int wydzial, int uzytkownik)
            {
                log.Info("Wyszukiwarka Kontrolki");
                DataTable parametry = makeParameterTable();
                parametry.Rows.Add("@idWydzialu", wydzial);
                parametry.Rows.Add("@id_uzytkownika", uzytkownik);
                runQuerry(" delete from uprawnienia where  id_wydzialu=@idWydzialu and rodzaj=6 and id_uzytkownika=@id_uzytkownika", con_str, parametry);
            }
        }

        public class Kontrolki6X : common
        {
            public void dodaj(int wydzial, int uzytkownik)
            {
                log.Info("Kontrolki6X dodawanie");
                DataTable parametry = makeParameterTable();
                parametry.Rows.Add("@idWydzialu", wydzial);
                parametry.Rows.Add("@id_uzytkownika", uzytkownik);
                runQuerry(" IF NOT EXISTS(SELECT * FROM uprawnienia WHERE id_wydzialu = @idWydzialu AND rodzaj = 7 AND id_uzytkownika = @id_uzytkownika)    BEGIN       insert INTO uprawnienia (id_wydzialu,rodzaj,id_uzytkownika) VALUES  (@idWydzialu,3,@id_uzytkownika)    END", con_str, parametry, "Dodawanie uprawnień dla KOF");
            }

            public void usun(int wydzial, int uzytkownik)
            {
                log.Info("usuwanie Kontrolki");
                DataTable parametry = makeParameterTable();
                parametry.Rows.Add("@idWydzialu", wydzial);
                parametry.Rows.Add("@id_uzytkownika", uzytkownik);
                runQuerry(" delete from uprawnienia where  id_wydzialu=@idWydzialu and rodzaj=7 and id_uzytkownika=@id_uzytkownika", con_str, parametry);
            }
        }
        protected void EdycjaPracownika(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            string klucz = e.Keys[0].ToString();
            string nowaWartosc01 = e.NewValues[0].ToString();
            string nowaWartosc02 = e.NewValues[1].ToString();
            EdycjaUprawnien(klucz, nowaWartosc01, nowaWartosc02);
        }

        protected void EdycjaWyszukiwarki(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {

            string klucz = e.Keys[0].ToString();
            string nowaWartosc01 = e.NewValues[0].ToString();
            string nowaWartosc02 = e.NewValues[1].ToString();
            EdycjaUprawnien(klucz, nowaWartosc01, nowaWartosc02);
        }

        protected void EdycjaKof(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {

            string klucz = e.Keys[0].ToString();
            string nowaWartosc01 = e.NewValues[0].ToString();
            string nowaWartosc02 = e.NewValues[1].ToString();
            EdycjaUprawnien(klucz, nowaWartosc01, nowaWartosc02);
        }

        protected void EdycjaMSS(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {

            string klucz = e.Keys[0].ToString();
            string nowaWartosc01 = e.NewValues[0].ToString();
            string nowaWartosc02 = e.NewValues[1].ToString();
            EdycjaUprawnien(klucz, nowaWartosc01, nowaWartosc02);
        }

        protected void EdycjaKontrolki(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {

            string klucz = e.Keys[0].ToString();
            string nowaWartosc01 = e.NewValues[0].ToString();
            string nowaWartosc02 = e.NewValues[1].ToString();
            EdycjaUprawnien(klucz, nowaWartosc01, nowaWartosc02);
        }

      
    }
}