﻿using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Web.UI;
using System.Xml;

namespace Statystyki_2018
{
    public partial class Wymiana1 : System.Web.UI.Page
    {
        public ServiceReference test = new ServiceReference();
        public common cm = new common();
        public Class1 cl = new Class1();
        private dataReaders dR = new dataReaders();
        public string con_str = ConfigurationManager.ConnectionStrings["wap"].ConnectionString;
        public string con_str_wcyw = ConfigurationManager.ConnectionStrings["wcywConnectionString"].ConnectionString;
        public log_4_net log = new log_4_net();
        public tabele Tabele = new tabele();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                TextBox1.Text = "";
                lbRok.Items.Clear();
                int year = DateTime.Now.Year;
                for (int i = year; i > year - 30; i--)
                {
                    lbRok.Items.Add(i.ToString());
                    lbRok2.Items.Add(i.ToString());
                }
                lbRok.SelectedIndex = 0;
                DataTable parameters = cm.makeParameterTable();

                DataTable dT1 = cm.getDataTable("SELECT distinct [wartosc], [ConnectionString] FROM [konfig] WHERE ([klucz] = 'KonfigRodzajSprawy') ORDER BY wartosc", con_str, parameters, "Wymiana");
                lbRodzajSprawy.Items.Clear();
                foreach (DataRow item in dT1.Rows)
                {
                    string Text = item[0].ToString();
                    DevExpress.Web.ListEditItem listEditItem = new DevExpress.Web.ListEditItem()
                    {
                        Text = item[0].ToString(),
                        Value = item[1].ToString()
                    };
                    string value = item[1].ToString();
                    lbRodzajSprawy.Items.Add(Text, value);
                    lbRodzajSprawy2.Items.Add(Text, value);
                }
                if (lbRodzajSprawy.SelectedIndex == -1)
                {
                    lbRodzajSprawy.SelectedIndex = 0;
                }
                if (lbRodzajSprawy2.SelectedIndex == -1)
                {
                    lbRodzajSprawy2.SelectedIndex = 0;
                }
            }
        }

        protected void ASPxButton1_Click(object sender, EventArgs e)
        {
            string kwerendaOdczytujaca = string.Empty;
            string rodzaj = string.Empty;

            string connection = string.Empty;
            string url = string.Empty;
            string host = string.Empty;
            TextBox1.Text = "";

            stat2018.ServiceReference3.SerwisWymianySoapClient serwisWymianySoapClient = new stat2018.ServiceReference3.SerwisWymianySoapClient();

            rodzaj = lbRodzajSprawy.SelectedItem.Text.ToString();
            connection = lbRodzajSprawy.SelectedItem.Value.ToString();
            // walidacja po stronie clienta

            DataTable parametry = cm.makeParameterTable();
            parametry.Rows.Add("@rodzaj", rodzaj);
            DataTable kwerendaWalidująca = cm.getDataTable("SELECT distinct kwerendaOdczytujaca,  connection FROM wymiana where rodzaj = @rodzaj and typ=0", cm.con_str, parametry, "wymiana cleint: kwerendaWalidująca");
            if (kwerendaWalidująca == null)
            {
                log.Error("Wymiana odczyt danych: Brak kwerendy walidującej zapytanie po stronie klienta - rodzaj=0");
                TextBox1.Text = TextBox1.Text + "Wymiana odczyt danych: Brak kwerendy walidującej zapytanie po stronie klienta - rodzaj=0" + Environment.NewLine;
                return;
            }
            string kwerendaSprawdzajaca = kwerendaWalidująca.Rows[0][0].ToString();
            string CSkwerendySprawdzajacej = kwerendaWalidująca.Rows[0][1].ToString();

            parametry = cm.makeParameterTable();
            parametry.Rows.Add("@numer", TBNrSprawy.Text.Trim());
            parametry.Rows.Add("@rok", lbRok.SelectedItem.Text.Trim());
            parametry.Rows.Add("@rep", TBRepertorium.Text.Trim());
            parametry.Rows.Add("@wydzial", TBNrWydzialu.Text.Trim());
            string wynikWalidacji = cm.getQuerryValue(kwerendaSprawdzajaca, CSkwerendySprawdzajacej, parametry, "wymiana cleint");
            try
            {
                if (int.Parse(wynikWalidacji) == 0)
                {
                    log.Error("Brak spraw z repetytoriu, " + TBRepertorium.Text + ", Wydziału " + TBNrWydzialu.Text + ", o numerze " + TBNrSprawy.Text.Trim() + " z roku " + lbRok.SelectedItem.Text.Trim());
                    TextBox1.Text = TextBox1.Text + "Brak spraw z repetytoriu, " + TBRepertorium.Text + ", Wydziału " + TBNrWydzialu.Text + ", o numerze " + TBNrSprawy.Text.Trim() + " z roku " + lbRok.SelectedItem.Text.Trim() + Environment.NewLine;
                    return;
                }
            }
            catch (Exception ex)
            {
                log.Error("Wymiana: walidacja po stronie klienta " + ex.Message);
                TextBox1.Text = TextBox1.Text + "Wymiana: walidacja po stronie klienta " + ex.Message + Environment.NewLine;
                return;
            }
            // wyciągnięcie kwerendy i CS zapytujących

            parametry = cm.makeParameterTable();
            parametry.Rows.Add("@rodzaj", rodzaj);
            DataTable zestawZapytujacy = cm.getDataTable("SELECT distinct kwerendaOdczytujaca,  connection FROM wymiana where rodzaj = @rodzaj and typ=1", cm.con_str, parametry, "wymiana cleint: kwerendaWalidująca");

            string kwerendaZapytujaca = zestawZapytujacy.Rows[0][0].ToString();
            string CSkwerendyZapytujacej = zestawZapytujacy.Rows[0][1].ToString();

            //  serwisWymianySoapClient.Endpoint.Address = endpointAddress;
            // string wynik = string.Empty;
            DataTable wynik = new DataTable();
            try
            {
                wynik = serwisWymianySoapClient.DaneWXml(TBNrWydzialu.Text.Trim(), TBRepertorium.Text.Trim(), int.Parse(TBNrSprawy.Text.Trim()), rodzaj, CSkwerendyZapytujacej, int.Parse(lbRok.SelectedItem.Text.Trim()), kwerendaZapytujaca);
                //                              DaneWXml(string NrWydzialu       , string repertorium       , int nrSprawy                     , string rodzaj,  string connection, int rok, string kwerendaZapytujaca)
                //  TextBox1.Text = wynik;
            }
            catch (Exception ex)
            {
                log.Error("Wymiana odczyt danych: " + ex.Message);
                TextBox1.Text = TextBox1.Text + ex.Message + Environment.NewLine;
                return;
            }
            // rozkodować tabele;
            StringBuilder plikXml = new StringBuilder();

            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.AppendLine("<Odpowiedz>");
            foreach (DataRow item in wynik.Rows)
            {
                var cos2 = (Byte[])item[5];

                StringBuilder cos4 = new StringBuilder();
                foreach (var bajt in cos2)
                {
                    var cos5 = bajt.ToString("X");
                    cos4.Append(cos5);
                }

                //File.ReadAllBytes
                stringBuilder.AppendLine("<row>");
                stringBuilder.AppendLine("<wydzial>" + item[0] + "</wydzial>");
                stringBuilder.AppendLine("<repertorium>" + item[1] + "</repertorium>");

                stringBuilder.AppendLine("<numer>" + item[2] + " </numer>");
                stringBuilder.AppendLine("<rok> " + item[3] + " </rok>");
                stringBuilder.AppendLine("<dataOrzeczenia> " + item[4] + " </dataOrzeczenia>");
                stringBuilder.AppendLine("<msword> " + cos4 + " </msword>");
                stringBuilder.AppendLine("<rodzaj> " + item[6] + " </rodzaj>");

                stringBuilder.AppendLine("</row>");
            }
            stringBuilder.AppendLine("</Odpowiedz>");

            string path = Server.MapPath("Wymiana\\Tmp\\odpowiedz") + DateTime.Now.ToString().Replace(" ", "_").Replace(".", "_").Replace(":", "_") + ".xml";
            XmlDocument xdoc = new XmlDocument();
            try
            {
                xdoc.LoadXml(stringBuilder.ToString());
                xdoc.Save(path);
            }
            catch (Exception ex)
            {
                log.Error("Wymiana odczyt danych: " + ex.Message);
                TextBox1.Text = TextBox1.Text + ex.Message;
            }
            // zapis do bazy
            parametry = cm.makeParameterTable();
            parametry.Rows.Add("@rodzaj", rodzaj);
            DataTable kwerendaZapisujaca = cm.getDataTable("SELECT distinct kwerendaOdczytujaca,  connection FROM wymiana where rodzaj = @rodzaj and typ=2", cm.con_str, parametry, "wymiana cleint: kwerendaWalidująca");
            if (kwerendaWalidująca == null)
            {
                log.Error("Wymiana odczyt danych: Brak kwerendy Zapisującej zapytanie po stronie klienta - rodzaj=2");
                TextBox1.Text = TextBox1.Text + "Wymiana odczyt danych: Brak kwerendy Zapisującej zapytanie po stronie klienta - rodzaj=2" + Environment.NewLine;
                return;
            }
            string kwerendaZapisująca = kwerendaZapisujaca.Rows[0][0].ToString();
            string CSkwerendyZapisującej = kwerendaZapisujaca.Rows[0][1].ToString();

            foreach (DataRow pojedynczyWiersz in wynik.Rows)
            {
                var wydzial = pojedynczyWiersz["wydzial"];
                var repertorium = pojedynczyWiersz["repertorium"];
                var numerW = pojedynczyWiersz["numer"];
                var rok = pojedynczyWiersz["rok"];
                var d_orzecz = pojedynczyWiersz["d_orzecz"];
                var msword = pojedynczyWiersz["msword"];
                var rodzajw = pojedynczyWiersz["rodzaj"];

                var cos2 = (Byte[])msword;

                string base64String = Convert.ToBase64String(cos2);

                byte[] outputData = Convert.FromBase64String(base64String);

                parametry = cm.makeParameterTable();
                parametry.Rows.Add("@d_orzeczenia", d_orzecz.ToString());
                parametry.Rows.Add("@msword", outputData);
                parametry.Rows.Add("@typ_orzeczenia", rodzajw.ToString());
                parametry.Rows.Add("@rok", rok.ToString());
                parametry.Rows.Add("@numer", numerW.ToString());
                parametry.Rows.Add("@rep", repertorium.ToString());

                var conn = new SqlConnection(CSkwerendyZapisującej);
                using (SqlCommand sqlCmd = new SqlCommand(kwerendaZapisująca, conn))
                {
                    try
                    {
                        //log.Info("Open DB connection");
                        conn.Open();
                        //log.Info("DB connection is open");
                        SqlParameter photoParam = new SqlParameter("@msword", SqlDbType.Image);
                        photoParam.Value = outputData;
                        sqlCmd.Parameters.Add(photoParam);
                        sqlCmd.Parameters.Add("@d_orzeczenia", d_orzecz.ToString());
                        sqlCmd.Parameters.Add("@typ_orzeczenia", rodzajw.ToString());
                        sqlCmd.Parameters.Add("@rok", rok.ToString());
                        sqlCmd.Parameters.Add("@numer", numerW.ToString());
                        sqlCmd.Parameters.Add("@rep", repertorium.ToString());

                        //log.Info("Start querry execution");
                        sqlCmd.ExecuteScalar();
                        //log.Info("Execution done. ");
                        conn.Close();
                        //log.Info("Close DB connection");
                    }
                    catch (Exception ex)
                    {
                        TextBox1.Text = TextBox1.Text + "Wymiana odczyt danych: Bład podczas zapisu odpowiedzi do bazy danych  po stronie klienta - rodzaj=2: "+ex.Message + Environment.NewLine;

                        conn.Close();
                    }
                    //      cm.runQuerry(kwerendaZapisująca, CSkwerendyZapisującej, parametry, "Wymiana");
                }
            }

            TextBox1.Text = TextBox1.Text + " KOniec przekazywania danych";
        }

        protected void citiesTabPage_ActiveTabChanged(object source, DevExpress.Web.TabControlEventArgs e)
        {
        }

        protected void AnulujInstancja2Klik(object sender, EventArgs e)
        {
            lbRodzajSprawy2.SelectedIndex = 0;
            lbRok2.SelectedIndex = 0;
            TBNrSprawy2.Text = "";
            TBNrWydzialu2.Text = "";
            TBRepertorium2.Text = "";
        }

        protected void AnulujInstancja1Klik(object sender, EventArgs e)
        {
            lbRodzajSprawy.SelectedIndex = 0;
            lbRok.SelectedIndex = 0;
            TBNrSprawy.Text = "";
            TBNrWydzialu.Text = "";
            TBRepertorium.Text = "";
        }
    }
}