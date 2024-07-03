using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace Statystyki_2018
{
    public enum Rodzaje
    {
        admin = 0,// (możliwość nadawania uprawnień)
        miesieczne = 1,
        MSS = 2,
        kontrolki = 3,
        kof = 4,
        wyszukiwarka = 5,
        pracownik = 6,
        wymiana=7,
        potwierdzenie = 1,
        kontrolki6X = 8
    }

    public class Class1
    {
        public static BaseFont NewFont = BaseFont.CreateFont(Environment.GetEnvironmentVariable("SystemRoot") + "\\fonts\\sylfaen.ttf", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);

        private Font fontPL1 = new Font(NewFont, 10f, Font.NORMAL, BaseColor.BLACK);
        public Font plFont1 = new Font(NewFont, 10f, Font.NORMAL, BaseColor.BLACK);
        public Font plFont = new Font(NewFont, 10f, Font.NORMAL, BaseColor.BLACK);
        public Font plFont2 = new Font(NewFont, 10f, Font.NORMAL, BaseColor.BLACK);
        public Font plFontBIG = new Font(NewFont, 15, Font.NORMAL, BaseColor.BLACK);
        public Font plFont3 = new Font(NewFont, 15, Font.NORMAL, BaseColor.BLACK);
        public log_4_net log = new log_4_net();
        public common Common = new common();

        public string con_str = ConfigurationManager.ConnectionStrings["wap"].ConnectionString;
        public string con_str_wcyw = ConfigurationManager.ConnectionStrings["wcywConnectionString"].ConnectionString;
       

        private string getColumnName(int i)
        {
            string txt = string.Empty;
            txt = i < 10 ? "d_0" + i.ToString().Trim() : "d_" + i.ToString().Trim();
            return txt;
        }

        //====================================================================================================================================
        public string wyciagnij_sedziego(string id_sedziego)
        {
            DataTable parameters = Common.makeParameterTable();
            parameters.Rows.Add("@id_sedziego", id_sedziego.Trim());
            string sedzia = Common.getQuerryValue("SELECT distinct imie + ' ' + nazwisko   FROM tbl_statystyki_tbl_02 where id_sedziego=@id_sedziego ", con_str, parameters);

            //=========================

            parameters = Common.makeParameterTable();
            parameters.Rows.Add("@id_sedziego", id_sedziego.Trim());
            string stanowisko = Common.getQuerryValue("SELECT distinct  (COALESCE( stanowisko ,'') ) as sedzia  FROM tbl_statystyki_tbl_02 where id_sedziego=@id_sedziego ", con_str, parameters, "");

            return sedzia + " " + stanowisko;
        }// end of wyciagnij_kwerende

        public string wyciagnij_sedziegoXXL(string id_sedziego)
        {
            DataTable parameters = Common.makeParameterTable();
            parameters.Rows.Add("@id_sedziego", id_sedziego.Trim());
            string sedzia = Common.getQuerryValue("SELECT distinct imie + ' ' + nazwisko   FROM tbl_statystyki_tbl_x5 where id_sedziego=@id_sedziego ", con_str, parameters);

            parameters = Common.makeParameterTable();
            parameters.Rows.Add("@id_sedziego", id_sedziego.Trim());
            string stanowisko = Common.getQuerryValue("SELECT distinct  (COALESCE( stanowisko ,'') ) as sedzia  FROM tbl_statystyki_tbl_x5 where id_sedziego=@id_sedziego ", con_str, parameters);

            return sedzia + " " + stanowisko;
        }// end of wyciagnij_sedziegoXXL
        public string wyciagnij_sedziegoXXL(string id_sedziego,string sc)
        {
            DataTable parameters = Common.makeParameterTable();
            parameters.Rows.Add("@ident", id_sedziego.Trim());
            string sedzia = Common.getQuerryValue("SELECT distinct    imie +' '+ nazwisko as tensedzia FROM         sedzia where ident=@ident", sc, parameters);

           
            return sedzia ;
        }// end of wyciagnij_kwewyciagnij_sedziegoXXLrende
        public string wyciagnij_tytul(string tabela, string kolumna, string id_dzialu)
        {
            DataTable parameters = Common.makeParameterTable();
            parameters.Rows.Add("@kolumna", kolumna.Trim());
            parameters.Rows.Add("@tabela", tabela.Trim());
            parameters.Rows.Add("@dzial", id_dzialu.Trim());

            return Common.getQuerryValue("SELECT  opis FROM  kwerendy where id_tabeli=@tabela and id_kolumny=@kolumna and id_wydzial=@dzial", con_str, parameters);
        }// end of wyciagnij_tytul

        public string wyciagnij_tytul(string tabela, string kolumna, string id_dzialu, string id_wiersza)
        {
            DataTable parameters = Common.makeParameterTable();
            parameters.Rows.Add("@kolumna", kolumna.Trim());
            parameters.Rows.Add("@tabela", tabela.Trim());
            parameters.Rows.Add("@dzial", id_dzialu.Trim());
            parameters.Rows.Add("@id_wiersza", id_wiersza.Trim());

            return Common.getQuerryValue("SELECT  opis FROM  kwerendy where id_tabeli=@tabela and id_kolumny=@kolumna and id_wydzial=@dzial and id_wiersza=@id_wiersza", con_str, parameters);
        }// end of wyciagnij_kwerende

        public string czy_dostepny(string user, string id_wydzialu, string domain)
        {
            var conn = new SqlConnection(con_str);

            SqlCommand sqlCmd = new SqlCommand();
            // user nr
            string result = "";
            using (sqlCmd = new SqlCommand())
            {
                switch (domain)
                {
                    case "1":
                        {
                            sqlCmd = new SqlCommand("SELECT distinct  [ident]       FROM uzytkownik  where   [login_domenowy] =@name", conn);
                        }
                        break;

                    default:
                        {
                            sqlCmd = new SqlCommand("SELECT distinct  [ident]       FROM uzytkownik  where   [login] =@name", conn);
                        }
                        break;
                }
                try
                {
                    conn.Open();
                    sqlCmd.Parameters.AddWithValue("@name", user.Trim());
                    result = sqlCmd.ExecuteScalar().ToString();
                    conn.Close();
                }
                catch (Exception ex)
                {
                    conn.Close();
                    return ex.Message.ToString();
                }
            }

            //pozwolenie

            DataTable parameters = Common.makeParameterTable();
            parameters.Rows.Add("@id_wydzialu", id_wydzialu.Trim());
            parameters.Rows.Add("@user_id", result);
            var answer = Common.getQuerryValue("SELECT COUNT(*) FROM     uprawnienia WHERE  (id_uzytkownika = @user_id) AND (id_wydzialu =@id_wydzialu)", con_str, parameters);

            return answer;
        }// czy_dostepny
        public string czy_dostepnaWyszukiwarka(string user, string id_wydzialu, string domain)
        {
            var conn = new SqlConnection(con_str);

            SqlCommand sqlCmd = new SqlCommand();
            // user nr
            string result = "";
            using (sqlCmd = new SqlCommand())
            {
                switch (domain)
                {
                    case "1":
                        {
                            sqlCmd = new SqlCommand("SELECT distinct  [ident]       FROM uzytkownik  where   [login_domenowy] =@name", conn);
                        }
                        break;

                    default:
                        {
                            sqlCmd = new SqlCommand("SELECT distinct  [ident]       FROM uzytkownik  where   [login] =@name", conn);
                        }
                        break;
                }
                try
                {
                    conn.Open();
                    sqlCmd.Parameters.AddWithValue("@name", user.Trim());
                    result = sqlCmd.ExecuteScalar().ToString();
                    conn.Close();
                }
                catch (Exception ex)
                {
                    conn.Close();
                    return "0";
                }
            }

            //pozwolenie

            DataTable parameters = Common.makeParameterTable();
            parameters.Rows.Add("@id_wydzialu", id_wydzialu.Trim());
            parameters.Rows.Add("@user_id", result);
            var answer = Common.getQuerryValue("select count (*)  FROM   uprawnienia WHERE rodzaj = 5 AND id_uzytkownika = @user_id and id_wydzialu = @id_wydzialu", con_str, parameters);

            return answer;
        }// czy_dostepny
        public DataSet pod_tabela(string cs, string kwerenda, string poczatek, string koniec, string id_sedziego)
        {
            var conn = new SqlConnection(cs);
            DataSet dsMenu = new DataSet();
            if (id_sedziego == null) { id_sedziego = "0"; }
            try
            {
                conn.Open();
                SqlDataAdapter daMenu = new SqlDataAdapter
                {
                    SelectCommand = new SqlCommand(kwerenda, conn)
                };

                daMenu.SelectCommand.Parameters.AddWithValue("@data_1", poczatek);
                daMenu.SelectCommand.Parameters.AddWithValue("@data_2", koniec);
                daMenu.SelectCommand.Parameters.AddWithValue("@id_sedziego", id_sedziego);
                daMenu.Fill(dsMenu);
                conn.Close();
                return dsMenu;
            }
            catch (Exception ex)
            {
                Common.log.Error("popup pod_tabela " + ex.Message);
                conn.Close();
            }

            return null;
        }

        public string podajConnectionString(int id_dzialu)
        {
            DataTable parametry = Common.makeParameterTable();
            parametry.Rows.Add("@id_dzialu", id_dzialu);
            return Common.getQuerryValue("SELECT distinct cs FROM wydzialy where ident=@id_dzialu", con_str, parametry);
        }

        private DataTable PodajListeKwerend(int idWydzial, int idTabeli, int typ, string tenPlik)
        {
            string kwerenda = "SELECT  id_kolumny, id_wiersza, kwerenda, podglad, opis FROM kwerendy WHERE (id_tabeli = @id_tabeli) and id_wydzial=@id_wydzial";
            DataTable parametry = Common.makeParameterTable();
            parametry.Rows.Add("@id_wydzial", idWydzial);
            parametry.Rows.Add("@id_tabeli", idTabeli);
            return Common.getDataTable(kwerenda, con_str, parametry, tenPlik);
        }

        public string podajKwerende(int id_dzialu, int id_wiersza, int id_kolumny, int id_tabeli)
        {
            DataTable parametry = Common.makeParameterTable();
            parametry.Rows.Add("@id_wydzial", id_dzialu);
            parametry.Rows.Add("@id_kolumny", id_kolumny);
            parametry.Rows.Add("@id_wiersza", id_wiersza);
            parametry.Rows.Add("@id_tabeli", id_tabeli);

            return Common.getQuerryValue("SELECT distinct kwerenda FROM kwerendy where id_wydzial=@id_wydzial and id_tabeli=@id_tabeli and id_kolumny=@id_kolumny and id_wiersza=@id_wiersza ", con_str, parametry);
        }

        public string podajKwerendePodgladu(int id_dzialu, int id_wiersza, int id_kolumny, int id_tabeli)
        {
            DataTable parametry = Common.makeParameterTable();
            parametry.Rows.Add("@id_wydzial", id_dzialu);
            parametry.Rows.Add("@id_kolumny", id_kolumny);
            parametry.Rows.Add("@id_wiersza", id_wiersza);
            parametry.Rows.Add("@id_tabeli", id_tabeli);

            return Common.getQuerryValue("SELECT distinct podglad FROM kwerendy where id_wydzial=@id_wydzial and id_tabeli=@id_tabeli and id_kolumny=@id_kolumny and id_wiersza=@id_wiersza ", con_str, parametry);
        }

        public string podajUzytkownika(string id_uzytkownika, string domain)
        {
            var conn = new SqlConnection(con_str);
            SqlCommand sqlCmd = new SqlCommand();
            string result = string.Empty;
            using (sqlCmd = new SqlCommand())
            {
                switch (domain)
                {
                    case "1":
                        {
                            sqlCmd = new SqlCommand("SELECT  imie + ' ' + nazwisko FROM     uzytkownik where  login_domenowy=@id_", conn);
                        }
                        break;

                    default:
                        {
                            sqlCmd = new SqlCommand("SELECT  imie + ' ' + nazwisko FROM     uzytkownik where  login=@id_", conn);
                        }
                        break;
                }
                try
                {
                    conn.Open();

                    sqlCmd.Parameters.AddWithValue("@id_", id_uzytkownika);
                    string odp = sqlCmd.ExecuteScalar().ToString().Trim();
                    conn.Close();
                    return odp;
                }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                {
                    conn.Close();
                    result = string.Empty;
                }
            }

            return result;
        }

        public string wyciagnijDane(int id_dzialu, int id_wiersza, int id_kolumny, DateTime poczatek, DateTime koniec, int id_tabeli, string tenplik)
        {
            string result = string.Empty;
            string cs = podajConnectionString(id_dzialu);
            string kw = podajKwerende(id_dzialu, id_wiersza, id_kolumny, id_tabeli);

            if ((!string.IsNullOrEmpty(cs)) && (!string.IsNullOrEmpty(kw)))
            {
                DataTable parametry = Common.makeParameterTable();
                parametry.Rows.Add("@data_1", KonwertujDate(poczatek));
                parametry.Rows.Add("@data_2", KonwertujDate(koniec));
                result = Common.getQuerryValue(kw, cs, parametry, tenplik);
            }
            return result;
        }

        public string wyciagnijDane(string kw, int id_dzialu, DateTime poczatek, DateTime koniec, string tenplik)
        {
            string result = string.Empty;
            string cs = podajConnectionString(id_dzialu);

            if ((!string.IsNullOrEmpty(cs)) && (!string.IsNullOrEmpty(kw)))
            {
                DataTable parametry = Common.makeParameterTable();
                parametry.Rows.Add("@data_1", KonwertujDate(poczatek));
                parametry.Rows.Add("@data_2", KonwertujDate(koniec));
                result = Common.getQuerryValue(kw, cs, parametry, tenplik);
            }
            return result;
        }

        public void tworzWiersz(int id, string opis, int id_dzialu, int id_tabeli, string tenplik)
        {
            DataTable parametry = Common.makeParameterTable();
            parametry.Rows.Add("@opis", opis);
            parametry.Rows.Add("@id", id);
            parametry.Rows.Add("@id_dzialu", id_dzialu);
            parametry.Rows.Add("@id_tabeli", id_tabeli);
            Common.runQuerry("insert into tbl_statystyki_tbl_01 (opis,id_,id_dzialu,id_tabeli) values (@opis,@id,@id_dzialu,@id_tabeli)", con_str, parametry, tenplik);
        }

        public void updateWiersz(int kolumna, int id, string opis, int id_tabeli, string tenplik)
        {
            string txt = getColumnName(kolumna);
            // skasowanie tabeli i wcyw

            DataTable parametry = Common.makeParameterTable();
            parametry.Rows.Add("@opis", opis);
            parametry.Rows.Add("@id", id);
            parametry.Rows.Add("@id_tabeli", id_tabeli);
            Common.runQuerry("update tbl_statystyki_tbl_01 set " + txt + "=@opis where id_=@id and id_tabeli=@id_tabeli", con_str, parametry, tenplik);
        }

        public void deleteRowTable()
        {
            Common.runQuerry("delete from  tbl_statystyki_tbl_01", con_str);
        }// end of deleteRowTable

        public string generuj_dane_do_tabeli_wierszy(DateTime poczatek, DateTime koniec, string id_dzialu, int id_tabeli, string tenPlik)

        {
            string kwerenda = string.Empty;
            DataSet dsMenu = new DataSet();
            string opis = string.Empty;
            try
            {
                // int ilosc_wierszy = PodajIloscWierszy(int.ps(id_dzialu), id_tabeli);
                for (int i = 1; i <= 30; i++) //po wierszach
                {
                    for (int j = 0; j <= 30; j++)
                    {
                        try
                        {
                            string dana = wyciagnijDane(int.Parse(id_dzialu), i, j, poczatek, koniec, id_tabeli, tenPlik);
                            if (j == 0)
                            {
                                // poczatek wiersza
                                if (string.IsNullOrEmpty(dana.Trim()) != true)
                                {
                                    tworzWiersz(i, dana, int.Parse(id_dzialu), id_tabeli, tenPlik);
                                }
                            }
                            else
                            {
                                //srodek wiersza
                                if (!string.IsNullOrEmpty(dana.Trim()))
                                {
                                    updateWiersz(j, i, dana, id_tabeli, tenPlik);
                                }
                            }
                        }
                        catch
                        { } // end of try
                    }
                }
            }
            catch
            { }

            return "1";
        }// end of generuj_dane_do_tabeli

        public DataTable generuj_dane_do_tabeli_wierszy(DateTime poczatek, DateTime koniec, string id_dzialu, int id_tabeli, int iloscWierszy, int iloscKolumn, string tenPlik)

        {
            DataTable tabelaWyjsciowa = new DataTable();
            tabelaWyjsciowa.Columns.Add("id_", typeof(int));
            tabelaWyjsciowa.Columns.Add("id_tabeli", typeof(int));
            tabelaWyjsciowa.Columns.Add("opis", typeof(string));
            for (int i = 1; i < iloscKolumn; i++)
            {
                tabelaWyjsciowa.Columns.Add(getColumnName(i), typeof(string));
            }
            string kwerenda = string.Empty;
            DataSet dsMenu = new DataSet();
            string opis = string.Empty;
            try
            {
                // int ilosc_wierszy = PodajIloscWierszy(int.ps(id_dzialu), id_tabeli);
                for (int i = 1; i <= iloscWierszy; i++) //po wierszach
                {
                    // tworz wiersz z tabeli
                    DataRow wiersz = tabelaWyjsciowa.NewRow();
                    wiersz["id_"] = i.ToString();
                    wiersz["id_tabeli"] = id_tabeli;
                    for (int j = 0; j <= iloscKolumn; j++)
                    {
                        try
                        {
                            string dana = wyciagnijDane(int.Parse(id_dzialu), i, j, poczatek, koniec, id_tabeli, tenPlik);
                            if (string.IsNullOrEmpty(dana.Trim()) == true) continue;
                            switch (j)
                            {
                                case 0:
                                    wiersz["opis"] = dana.Trim();
                                    break;
                                default:
                                    wiersz[getColumnName(j)] = dana.Trim();
                                    break;
                            }
                        }
                        catch
                        { } // end of try
                    }
                    tabelaWyjsciowa.Rows.Add(wiersz);
                }
            }
            catch 
            { }

            return tabelaWyjsciowa;
        }// end of generuj_dane_do_tabeli

        public DataTable generuj_dane_do_tabeli_wierszy_przestawnych1(DateTime poczatek, DateTime koniec, string id_dzialu, int id_tabeli, int id_pozycji, string tenPlik)
        {
            // DataSet dsMenu = new DataSet();
            string opis = string.Empty;
            DataTable tab_1000 = new DataTable();
            tab_1000.Columns.Add("id_", typeof(String));
            tab_1000.Columns.Add("opis", typeof(String));
            tab_1000.Columns.Add("d_01", typeof(int));
            tab_1000.Columns.Add("d_02", typeof(int));
            tab_1000.Columns.Add("d_03", typeof(int));
            tab_1000.Columns.Add("d_04", typeof(int));
            tab_1000.Columns.Add("d_05", typeof(int));
            tab_1000.Columns.Add("d_06", typeof(int));
            tab_1000.Columns.Add("d_07", typeof(int));
            tab_1000.Columns.Add("id_tabeli", typeof(int));

            try
            {
                DataTable listaKwerend = PodajListeKwerend(int.Parse(id_dzialu), id_tabeli, 0, tenPlik);
                log.Info("generuj_dane_do_tabeli_wierszy_przestawnych1 ilosc kwerend " + listaKwerend.Rows.Count.ToString());
                if (listaKwerend == null)
                {
                    return null;
                }

                // int ilosc_wierszy = PodajIloscWierszy(int.ps(id_dzialu), id_tabeli);
                for (int i = 1; i <= 15; i++) //po wierszach
                {
                    DataRow dR = tab_1000.NewRow();

                    for (int j = 0; j <= 7; j++)
                    {
                        try
                        {
                            string querryString = "id_wiersza=" + i.ToString().Trim() + " and id_kolumny=" + j.ToString();

                            DataRow[] results = listaKwerend.Select(querryString);
                            if (results.Length == 0)
                            {
                                continue;
                            }
                            DataRow wiersz = results[0];
                            string kwerenda = wiersz[2].ToString();

                            dR[0] = i.ToString();
                            string dana = wyciagnijDane(kwerenda, int.Parse(id_dzialu), poczatek, koniec, tenPlik);

                            if (j != 0)
                            {
                                if (string.IsNullOrEmpty(dana.Trim()) == true)
                                {
                                    dana = "0";
                                }
                                else
                                {
                                    dR[j + 1] = dana;
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            log.Error("tabele przestawne " + ex.Message);
                        } // end of try
                        dR["id_tabeli"] = id_tabeli;
                    }
                    if (!string.IsNullOrEmpty(dR[1].ToString().Trim()))
                    {
                        tab_1000.Rows.Add(dR);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("tabele przestawne ostatni " + ex.Message);
            } // end of try

            return tab_1000;
        }// end of generuj_dane_do_tabeli

        public DataTable generuj_dane_do_tabeli_przestawnych(int id_dzialu, int id_tabeli, DateTime poczatek, DateTime koniec, string tenPlik)
        {
            DataTable tab_1000 = new DataTable();
            tab_1000.Columns.Add("id_sedziego", typeof(int));
            tab_1000.Columns.Add("opis", typeof(String));
            for (int i = 1; i < 8; i++)
            {
                DataColumn aa = new DataColumn
                {
                    DataType = typeof(int),
                    DefaultValue = 0,
                    ColumnName = "d_0" + i.ToString()
                };
                tab_1000.Columns.Add(aa);
            }

            tab_1000.Columns.Add("id_tabeli", typeof(int));

            string status = string.Empty;
            status = status + "pompowanie danch do tabeli: " + id_tabeli.ToString() + "<br>";
            var conn = new SqlConnection(con_str);
            string kwerenda = string.Empty;
            DataTable parameters = Common.makeParameterTable();

            parameters.Rows.Add("@id_dzialu", id_dzialu);
            parameters.Rows.Add("@id_tabeli", id_tabeli);

            DataTable ddT = Common.getDataTable("SELECT distinct id_kolumny,[kwerenda] FROM [kwerendy] where id_tabeli=@id_tabeli and id_wydzial=@id_dzialu order by id_kolumny", con_str, parameters, tenPlik);
            int il_wierszy = 0;
            try
            {
                il_wierszy = ddT.Rows.Count;
            }
            catch { }
            string cs = podajConnectionString(id_dzialu);
            if (il_wierszy == 0)
            {
                // brak kwerend odcztującch
                status = status + "brak kwerend odcztujących" + "<br>";
            }
            else
            {
                // sa kwerendy
                status = status + "są kwerendy odcztujące, il: " + ddT.Rows.Count.ToString() + "<br>";
                //getTable
                int rowId = 0;
                try
                {
                    foreach (DataRow dRow in ddT.Rows)
                    {
                        rowId++;
                        string id_kol = dRow[0].ToString().Trim();
                        string kwe = dRow[1].ToString().Trim();

                        ////############################################  ladowanie danych tabela 2 ##############################
                        // odczyt sedziów
                        parameters = Common.makeParameterTable();

                        parameters.Rows.Add("@id_dzialu", id_dzialu);
                        parameters.Rows.Add("@id_tabeli", id_tabeli);
                        parameters.Rows.Add("@data_1", KonwertujDate(poczatek));
                        parameters.Rows.Add("@data_2", KonwertujDate(koniec));

                        ddT = Common.getDataTable(kwe, cs, parameters, tenPlik);
                        //pętla ładująca dane dane sedzw
                        int lp = 0;
                        foreach (DataRow dR in ddT.Rows)
                        {
                            switch (id_kol)
                            {
                                case "0":
                                    {
                                        lp++;
                                        DataRow dRN = tab_1000.NewRow();

                                        dRN[1] = dR[1].ToString().Trim() + " " + dR[2].ToString().Trim();
                                        dRN[0] = dR[0].ToString().Trim();
                                        for (int i = 2; i < 10; i++)
                                        {
                                            dRN[i] = 0;
                                        }

                                        dRN[9] = id_tabeli;
                                        tab_1000.Rows.Add(dRN);
                                        // załadowanie danych do pierwszych kolumn
                                    }
                                    break;

                                default:
                                    {
                                        //++++++++++++++++
                                        try
                                        {
                                            DataRow[] result = tab_1000.Select("id_sedziego = " + dR[1].ToString().Trim());
                                            foreach (DataRow row in result)
                                            {
                                                DataRow newRow = tab_1000.NewRow();
                                                for (int i = 0; i < 8; i++)
                                                {
                                                    newRow[i] = row[i];
                                                }
                                                // newRow = row;
                                                tab_1000.Rows.Remove(row);// wywala rowa

                                                //   newRow.BeginEdit();
                                                newRow[int.Parse(id_kol) + 1] = dR[0].ToString().Trim();
                                                newRow["id_tabeli"] = id_tabeli;
                                                // newRow.AcceptChanges();
                                                tab_1000.Rows.Add(newRow);
                                            }
                                        }
                                        catch
                                        { }

                                        //++++++++++++++++++
                                    }
                                    break;
                            }
                        }
                    }
                }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                { }//end of try
            }// end of if

            return tab_1000;
        }// end of generuj_dane_do_tabeli_3

        public DataTable generuj_dane_do_tabeli_przestawnych(int id_dzialu, int id_tabeli, int iloscKolumn, DateTime poczatek, DateTime koniec, string tenPlik)
        {
            DataTable tab_1000 = new DataTable();
            //      tab_1000.Columns.Add("id_sedziego", typeof(int));
            tab_1000.Columns.Add("opis", typeof(String));
            for (int i = 1; i < iloscKolumn; i++)
            {
                DataColumn aa = new DataColumn
                {
                    DataType = typeof(int),
                    DefaultValue = 0,
                    ColumnName = "d_0" + i.ToString()
                };
                tab_1000.Columns.Add(aa);
            }

            tab_1000.Columns.Add("id_tabeli", typeof(int));

            string status = string.Empty;
            status = status + "pompowanie danch do tabeli: " + id_tabeli.ToString() + "<br>";
            var conn = new SqlConnection(con_str);
            string kwerenda = string.Empty;
            DataTable parameters = Common.makeParameterTable();

            parameters.Rows.Add("@id_dzialu", id_dzialu);
            parameters.Rows.Add("@id_tabeli", id_tabeli);

            DataTable ddT = Common.getDataTable("SELECT distinct id_kolumny,[kwerenda] FROM [kwerendy] where id_tabeli=@id_tabeli and id_wydzial=@id_dzialu order by id_kolumny", con_str, parameters, tenPlik);
            int il_wierszy = 0;
            try
            {
                il_wierszy = ddT.Rows.Count;
            }
            catch { }
            string cs = podajConnectionString(id_dzialu);
            if (il_wierszy == 0)
            {
                // brak kwerend odcztującch
                status = status + "brak kwerend odcztujących" + "<br>";
            }
            else
            {
                // sa kwerendy
                status = status + "są kwerendy odcztujące, il: " + ddT.Rows.Count.ToString() + "<br>";
                //getTable
                int rowId = 0;
                try
                {
                    foreach (DataRow dRow in ddT.Rows)
                    {
                        rowId++;
                        string id_kol = dRow[0].ToString().Trim();
                        string kwe = dRow[1].ToString().Trim();

                        ////############################################  ladowanie danych tabela 2 ##############################
                        // odczyt sedziów
                        parameters = Common.makeParameterTable();

                        parameters.Rows.Add("@id_dzialu", id_dzialu);
                        parameters.Rows.Add("@id_tabeli", id_tabeli);
                        parameters.Rows.Add("@data_1", KonwertujDate(poczatek));
                        parameters.Rows.Add("@data_2", KonwertujDate(koniec));

                        ddT = Common.getDataTable(kwe, cs, parameters, tenPlik);
                        //pętla ładująca dane dane sedzw
                        int lp = 0;
                        foreach (DataRow dR in ddT.Rows)
                        {
                            switch (id_kol)
                            {
                                case "0":
                                    {
                                        lp++;
                                        DataRow dRN = tab_1000.NewRow();

                                        dRN[1] = dR[1].ToString().Trim() + " " + dR[2].ToString().Trim();
                                        dRN[0] = dR[0].ToString().Trim();
                                        for (int i = 2; i < 10; i++)
                                        {
                                            dRN[i] = 0;
                                        }

                                        dRN[9] = id_tabeli;
                                        tab_1000.Rows.Add(dRN);
                                        // załadowanie danych do pierwszych kolumn
                                    }
                                    break;

                                default:
                                    {
                                        //++++++++++++++++
                                        try
                                        {
                                            DataRow[] result = tab_1000.Select("id_sedziego = " + dR[1].ToString().Trim());
                                            foreach (DataRow row in result)
                                            {
                                                DataRow newRow = tab_1000.NewRow();
                                                for (int i = 0; i < iloscKolumn - 1; i++)
                                                {
                                                    newRow[i] = row[i];
                                                }
                                                // newRow = row;
                                                tab_1000.Rows.Remove(row);// wywala rowa

                                                //   newRow.BeginEdit();
                                                newRow[int.Parse(id_kol) + 1] = dR[0].ToString().Trim();
                                                newRow["id_tabeli"] = id_tabeli;
                                                // newRow.AcceptChanges();
                                                tab_1000.Rows.Add(newRow);
                                            }
                                        }
                                        catch
                                        { }

                                        //++++++++++++++++++
                                    }
                                    break;
                            }
                        }
                    }
                }

                catch 

                { }//end of try
            }// end of if

            return tab_1000;
        }// end of generuj_dane_do_tabeli_3

        public string uzupelnij_statusy()
        {
            Common.runQuerry("update tbl_statystyki_tbl_02 set funkcja = (SELECT   rtrim([nazwa]) FROM[funkcje]  where [rodzaj] = 1 and ident = tbl_statystyki_tbl_02.funkcja)", con_str, null);
            Common.runQuerry("update tbl_statystyki_tbl_02 set stanowisko=(SELECT   rtrim([nazwa]) FROM [funkcje]  where [rodzaj]=2 and ident=tbl_statystyki_tbl_02.stanowisko)", con_str, null);

            return "1";
        }

        public string clear_maim_db()
        {
            string status = string.Empty;

            // skasowanie tabeli i wcyw
            status = status + "Kasowanie tabeli tbl_statystyki_tbl_02" + "<br>";
            Common.runQuerry("delete from  tbl_statystyki_tbl_02", con_str, null);
            Common.runQuerry("delete from  tbl_statystyki_tbl_01", con_str, null);
            status = status + "Tabela tbl_statystyki_tbl_02 skasowana" + "<br>";

            return status;
        }// end of clear_maim_db

        public bool debug(int wydzial)
        {
            DataTable parametry = Common.makeParameterTable();
            parametry.Rows.Add("@ident", wydzial);
            return Common.getQuerryValue("SELECT debug FROM  wydzialy where ident=@ident", con_str, parametry) == "1" ? true : false;
        }// end of debug

        public string nazwaSadu(string id_sadu)
        {
            DataTable parameters = Common.makeParameterTable();
            parameters.Rows.Add("@id_sadu", id_sadu);

            return Common.getQuerryValue("SELECT  sad FROM     wydzialy where ident=@id_sadu", con_str, parameters);
        }// end of nazwaSadu

        public string KonwertujDate(DateTime data) => data.Year.ToString().Trim() + "-" + data.Month.ToString("D2") + "-" + data.Day.ToString("D2");

        public string generuj_dane_do_tabeli_(int id_dzialu, int id_tabeli, DateTime poczatek, DateTime koniec)
        {
            string status = string.Empty;
            status = status + "pompowanie danch do tabeli: " + id_tabeli.ToString() + "<br>";
            Common.log.Info("generuj_dane_do_tabeli_ :pompowanie danch do tabeli: " + id_tabeli.ToString());
            var conn = new SqlConnection(con_str);
            string kwerenda = string.Empty;
            DataTable parameters = Common.makeParameterTable();
            parameters.Rows.Add("@id_dzialu", id_dzialu);
            parameters.Rows.Add("@id_tabeli", id_tabeli);

            DataTable ddT = Common.getDataTable("SELECT id_kolumny,[kwerenda] FROM [kwerendy] where id_tabeli=@id_tabeli and id_wydzial=@id_dzialu order by id_kolumny", con_str, parameters, "");
            if (ddT == null)
            {
                Common.log.Error("generuj_dane_do_tabeli_ : brak kwerend dla tabeli: " + id_tabeli.ToString());

                return status;
            }

            Common.log.Info("generuj_dane_do_tabeli_ ilosc kwerend " + ddT.Rows.Count.ToString());
            try
            {
                foreach (DataRow dRow in ddT.Rows)
                {
                    string id_kol = dRow[0].ToString().Trim();
                    string kwe = dRow[1].ToString().Trim();
                    string cs = podajConnectionString(id_dzialu);
                    Common.log.Info("kwerenda " + kwe);
                    ////############################################  ladowanie danych tabela 2 ##############################
                    // odczyt sedziów
                    parameters = Common.makeParameterTable();

                    parameters.Rows.Add("@id_dzialu", id_dzialu);
                    parameters.Rows.Add("@id_tabeli", id_tabeli);
                    parameters.Rows.Add("@data_1", KonwertujDate(poczatek));
                    parameters.Rows.Add("@data_2", KonwertujDate(koniec));

                    ddT = Common.getDataTable(kwe, cs, parameters, "");
                    //pętla ładująca dane dane sedzw

                    foreach (DataRow dR in ddT.Rows)
                    {
                        switch (id_kol)
                        {
                            case "0":
                                {
                                    parameters = Common.makeParameterTable();
                                    parameters.Rows.Add("@imie", dR[1].ToString().Trim());
                                    parameters.Rows.Add("@nazwisko", dR[2].ToString().Trim());
                                    parameters.Rows.Add("@funkcja", dR[3].ToString().Trim());
                                    parameters.Rows.Add("@stanowisko", dR[4].ToString().Trim());
                                    parameters.Rows.Add("@id_sedziego", dR[0].ToString().Trim());
                                    parameters.Rows.Add("@id_tabeli", id_tabeli);
                                    parameters.Rows.Add("@id_dzialu", id_dzialu);
                                    parameters.Rows.Add("@sesja", "s3030");
                                    Common.log.Info("generuj_dane_do_tabeli_ :Zapis do tabeli tbl_statystyki_tbl_02  danych sędziów: " + id_tabeli.ToString());

                                    Common.runQuerry("insert into tbl_statystyki_tbl_02 (imie,nazwisko,funkcja,stanowisko,id_sedziego,sesja,id_tabeli,id_dzialu) values (@imie,@nazwisko,@funkcja,@stanowisko,@id_sedziego,@sesja,@id_tabeli,@id_dzialu)", con_str, parameters);
                                    // załadowanie danych do pierwszych kolumn
                                }
                                break;

                            default:
                                {
                                    string txt = "d_" + int.Parse(id_kol.Trim()).ToString("D2");
                                    parameters = Common.makeParameterTable();

                                    string ttxx = dR[0].ToString().Trim();
                                    parameters.Rows.Add("@value", dR[0].ToString().Trim());
                                    if (dR.ItemArray.Length > 0)
                                    {
                                        try
                                        {
                                            parameters.Rows.Add("@id_", dR[1].ToString().Trim());
                                        }
                                        catch
                                        {
                                            parameters.Rows.Add("@id_", "0");
                                        }
                                    }
                                    else
                                    {
                                        parameters.Rows.Add("@id_", "0");
                                    }

                                    parameters.Rows.Add("@id_tabeli", id_tabeli);
                                    parameters.Rows.Add("@id_dzialu", id_dzialu);
                                    parameters.Rows.Add("@sesja", "s3030");

                                    //                                        string tvxt = dR[0].ToString().Trim();
                                    //                                      string tvxts = dR[1].ToString().Trim();
                                    Common.log.Info("update tbl_statystyki_tbl_02 " + txt);
                                    Common.runQuerry("update tbl_statystyki_tbl_02 set " + txt + "=@value, sesja=@sesja where id_sedziego=@id_ and id_tabeli=@id_tabeli and id_dzialu=@id_dzialu", con_str, parameters);
                                }
                                break;
                        }
                    }
                }
            }
            catch
            { }//end of try

            return status;
        }// end of generuj_dane_do_tabeli_3

        
        //================================================================================================

    } // end of class
}