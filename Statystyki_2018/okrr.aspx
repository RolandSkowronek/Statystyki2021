<%@ Page Title="" Language="C#" UICulture="pl" Culture="pl-PL" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="okrr.aspx.cs" Inherits="Statystyki_2018.okrr" AspCompat="true" MaintainScrollPositionOnPostback="true" %>

<%@ Register Assembly="DevExpress.Web.v17.1, Version=17.1.17.0,  Culture=neutral,  PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style>
        #menu {
            position: relative;
        }

            #menu.scrolling {
                position: fixed;
                top: 0;
                left: 0;
                right: 0;
            }

        @media print {
            @page {
                 size: 29cm 21.7cm;
                 margin: 0mm 0mm 0mm 0mm;

        }

        div {
           
           
        }
            .horizont {
                 transform: translate(-25mm, 0) scale(0.8);
            -webkit-transform: translate(-25mm, 0) scale(0.8);
            -moz-transform: translate(-25mm, 0) scale(0.8);

            }
        }
    </style>

    <script src="Scripts/rls.js"></script>

    <script language="javascript" type="text/javascript">
        var xvalue
        var yvalue
        function printMousePos(event) {

            xvalue = event.pageX;
            yvalue = event.pageY;

        }

        document.addEventListener("click", printMousePos);
    </script>

    <div class="noprint">
        <div id="menu" style="background-color: #f7f7f7; z-index: 9999">
            <div class="manu_back" style="height: 40px; margin: 0 auto 0 auto; position: relative; width: 1050px; left: 0px;">
                <table>
                    <tr>
                        <td style="width: auto; padding-left: 5px;">
                            <asp:Label ID="Label4" runat="server" Text="Proszę wybrać zakres:"></asp:Label>
                        </td>
                        <td style="width: auto; padding-left: 5px;">

                            <dx:ASPxDateEdit ID="Date1" runat="server" Theme="Moderno">
                            </dx:ASPxDateEdit>
                        </td>
                        <td style="width: auto; padding-left: 5px;">

                            <dx:ASPxDateEdit ID="Date2" runat="server" Theme="Moderno">
                            </dx:ASPxDateEdit>
                        </td>
                        <td style="width: auto; padding-left: 5px;">
                            <asp:LinkButton ID="LinkButton54" runat="server" class="ax_box" OnClick="LinkButton54_Click">  Odśwież</asp:LinkButton>
                        </td>
                        <td style="width: auto; padding-left: 5px;">
                            
                            <input id="Button1" class="ax_box" style="border-style: none; padding: 0px" type="button" onclick="JavaScript: window.print();" value="Drukuj" />

                        </td>
                        <td style="width: auto; padding-left: 5px;">

                            &nbsp;</td>
                        <td style="width: auto; padding-left: 5px;">
                            <asp:LinkButton ID="LinkButton57" runat="server" CssClass="ax_box" OnClick="Button3_Click">Zapisz do Excel</asp:LinkButton>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>

    <div style="position: relative; top: 30px; width: 1150px;">

        <div id="Div2" style="z-index: 10;" class="Landscape horizont">
            <div style="margin-left: auto; margin-right: auto; text-align: center; width: auto;">
                <asp:Label ID="Label3" runat="server" Text=" " Style="font-weight: 700"></asp:Label>
                <br />
            </div>
            <div style="margin-left: auto; margin-right: auto; text-align: center; width: auto;">
                <asp:Label runat="server" ID="id_dzialu" Visible="False"></asp:Label>
            </div>

            <asp:Label ID="lbtabela1" runat="server"></asp:Label>

            <asp:Label ID="infoLabel01" runat="server" Visible="False">\/\/\/  tabela 1</asp:Label>

            <br />
            <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" OnRowCreated="GridView6_RowCreated"
                Width="100%" ShowHeader="False" OnRowDataBound="GridView1_RowDataBound">
                <Columns>
                    <asp:BoundField DataField="id" HeaderText="L.p." SortExpression="id">
                        <ItemStyle Width="15px" />
                    </asp:BoundField>
                    <asp:TemplateField HeaderText="funkcja" SortExpression="imie">

                        <ItemTemplate>
                            <asp:Label ID="funkcja" runat="server" Text='<%# Eval("funkcja") %>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle CssClass="col_100" />
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="imie" SortExpression="imie">

                        <ItemTemplate>
                            <asp:Label ID="imie" runat="server" Text='<%# Eval("imie") %>'></asp:Label>
                            <asp:Label ID="nazwisko" runat="server" Text='<%# Eval("nazwisko") %>'></asp:Label>
                            <br />

                            <asp:Label ID="stanowisko" runat="server" Text='<%# Eval("stanowisko") %>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle CssClass="col_185_min center" />
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="d_01" SortExpression="d_01">

                        <ItemTemplate>
                            <a href="javascript:openPopup('popup.aspx?sesja=<%# Eval("id_sedziego")+ "!"+Eval("id_tabeli") +"!1!6"%>')">
                                <asp:Label ID="Label716" runat="server" Text='<%# Eval("d_01")%>' CssClass="normal"></asp:Label>
                            </a>
                        </ItemTemplate>
                        <ItemStyle CssClass="col_50H" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="d_02" SortExpression="d_02">

                        <ItemTemplate>
                            <a href="javascript:openPopup('popup.aspx?sesja=<%# Eval("id_sedziego")+ "!"+Eval("id_tabeli") +"!2!6"%>')">
                                <asp:Label ID="Label717" runat="server" Text='<%# Eval("d_02")%>' CssClass="normal"></asp:Label>
                            </a>
                        </ItemTemplate>
                        <ItemStyle CssClass="col_50H" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="d_03" SortExpression="d_03">

                        <ItemTemplate>
                            <a href="javascript:openPopup('popup.aspx?sesja=<%# Eval("id_sedziego")+ "!"+Eval("id_tabeli") +"!3!6"%>')">
                                <asp:Label ID="Label718" runat="server" Text='<%# Eval("d_03")%>' CssClass="normal"></asp:Label>
                            </a>
                        </ItemTemplate>
                        <ItemStyle CssClass="col_50H" />
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="d_04" SortExpression="d_03">

                        <ItemTemplate>

                            <asp:TextBox ID="TextBox29" runat="server" CssClass="col_29"></asp:TextBox>
                        </ItemTemplate>
                        <ItemStyle CssClass="col_50H" />
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="d_04" SortExpression="d_04">

                        <ItemTemplate>
                            <asp:TextBox ID="TextBox30X" runat="server" CssClass="col_29"></asp:TextBox>
                        </ItemTemplate>
                        <ItemStyle CssClass="col_50H" />
                    </asp:TemplateField>
                   
                    <asp:TemplateField HeaderText="d_06" SortExpression="d_06">

                        <ItemTemplate>
                            <a href="javascript:openPopup('popup.aspx?sesja=<%# Eval("id_sedziego")+ "!"+Eval("id_tabeli") +"!6!6"%>')">
                                <asp:Label ID="Label721" runat="server" Text='<%# Eval("d_06")%>' CssClass="normal"></asp:Label>
                            </a>
                        </ItemTemplate>
                        <ItemStyle CssClass="col_50H" />
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="d_07" SortExpression="d_07">
                        <ItemTemplate>
                            <a href="javascript:openPopup('popup.aspx?sesja=<%# Eval("id_sedziego")+ "!"+Eval("id_tabeli") +"!7!6"%>')">
                                <asp:Label ID="Label722" runat="server" Text='<%# Eval("d_07")%>' CssClass="normal"></asp:Label>
                            </a>
                        </ItemTemplate>
                        <ItemStyle CssClass="col_50H" />
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="d_07" SortExpression="d_07">
                        <ItemTemplate>
                            <a href="javascript:openPopup('popup.aspx?sesja=<%# Eval("id_sedziego")+ "!"+Eval("id_tabeli") +"!8!6"%>')">
                                <asp:Label ID="Label723" runat="server" Text='<%# Eval("d_08")%>' CssClass="normal"></asp:Label>
                            </a>
                        </ItemTemplate>
                        <ItemStyle CssClass="col_50H" />
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="d_07" SortExpression="d_07">
                        <ItemTemplate>
                            <a href="javascript:openPopup('popup.aspx?sesja=<%# Eval("id_sedziego")+ "!"+Eval("id_tabeli") +"!9!6"%>')">
                                <asp:Label ID="Label724" runat="server" Text='<%# Eval("d_09")%>' CssClass="normal"></asp:Label>
                            </a>
                        </ItemTemplate>
                        <ItemStyle CssClass="col_50H" />
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="d_07" SortExpression="d_07">
                        <ItemTemplate>
                            <a href="javascript:openPopup('popup.aspx?sesja=<%# Eval("id_sedziego")+ "!"+Eval("id_tabeli") +"!10!6"%>')">
                                <asp:Label ID="Label725" runat="server" Text='<%# Eval("d_10")%>' CssClass="normal"></asp:Label>
                            </a>
                        </ItemTemplate>
                        <ItemStyle CssClass="col_50H" />
                    </asp:TemplateField>
                        <asp:TemplateField HeaderText="d_07" SortExpression="d_07">
                        <ItemTemplate>

                            <a href="javascript:openPopup('popup.aspx?sesja=<%# Eval("id_sedziego")+ "!"+Eval("id_tabeli") +"!13!6"%>')">
                                <asp:Label ID="Label728" runat="server" Text='<%# Eval("d_13")%>' CssClass="normal"></asp:Label>
                            </a>
                        </ItemTemplate>
                        <ItemStyle CssClass="col_50H" />
                    </asp:TemplateField>
                      <asp:TemplateField HeaderText="d_07" SortExpression="d_07">
                        <ItemTemplate>

                            <a href="javascript:openPopup('popup.aspx?sesja=<%# Eval("id_sedziego")+ "!"+Eval("id_tabeli") +"!14!6"%>')">
                                <asp:Label ID="Label728x" runat="server" Text='<%# Eval("d_14")%>' CssClass="normal"></asp:Label>
                            </a>
                        </ItemTemplate>
                        <ItemStyle CssClass="col_50H" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="d_07" SortExpression="d_07">
                        <ItemTemplate>
                            <asp:TextBox ID="TextBoxx30" runat="server" CssClass="col_29"></asp:TextBox>
                        </ItemTemplate>
                        <ItemStyle CssClass="col_50H" />
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="d_07" SortExpression="d_07">
                        <ItemTemplate>
                            <asp:TextBox ID="TextBoxx29" runat="server" CssClass="col_29"></asp:TextBox>
                        </ItemTemplate>
                        <ItemStyle CssClass="col_50H" />
                    </asp:TemplateField>

                

                    <asp:TemplateField HeaderText="d_07" SortExpression="d_07">
                        <ItemTemplate>
                            <a href="javascript:openPopup('popup.aspx?sesja=<%# Eval("id_sedziego")+ "!"+Eval("id_tabeli") +"!15!6"%>')">
                                <asp:Label ID="Label729" runat="server" Text='<%# Eval("d_15")%>' CssClass="normal"></asp:Label>
                            </a>
                        </ItemTemplate>
                        <ItemStyle CssClass="col_50H" />
                    </asp:TemplateField>
                      <asp:TemplateField HeaderText="d_07" SortExpression="d_07">
                        <ItemTemplate>
                            <a href="javascript:openPopup('popup.aspx?sesja=<%# Eval("id_sedziego")+ "!"+Eval("id_tabeli") +"!16!6"%>')">
                                <asp:Label ID="Label730" runat="server" Text='<%# Eval("d_16")%>' CssClass="normal"></asp:Label>
                            </a>
                        </ItemTemplate>
                        <ItemStyle CssClass="col_50H" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="d_07" SortExpression="d_07">
                        <ItemTemplate>
                        </ItemTemplate>
                        <ItemStyle CssClass="col_50H doubleXcross gray" />
                    </asp:TemplateField>

               

                    <asp:TemplateField HeaderText="d_07" SortExpression="d_07">
                        <ItemTemplate>
                            <a href="javascript:openPopup('popup.aspx?sesja=<%# Eval("id_sedziego")+ "!"+Eval("id_tabeli") +"!18!6"%>')">
                                <asp:Label ID="Label732" runat="server" Text='<%# Eval("d_18")%>' CssClass="normal"></asp:Label>
                            </a>
                        </ItemTemplate>
                        <ItemStyle CssClass="col_50H  " />
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="d_07" SortExpression="d_07">
                        <ItemTemplate>
                            <a href="javascript:openPopup('popup.aspx?sesja=<%# Eval("id_sedziego")+ "!"+Eval("id_tabeli") +"!19!6"%>')">
                                <asp:Label ID="Label733" runat="server" Text='<%# Eval("d_19")%>' CssClass="normal"></asp:Label>
                            </a>
                        </ItemTemplate>
                        <ItemStyle CssClass="col_50H" />
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="d_07" SortExpression="d_07">
                        <ItemTemplate>
                            <a href="javascript:openPopup('popup.aspx?sesja=<%# Eval("id_sedziego")+ "!"+Eval("id_tabeli") +"!20!6"%>')">
                                <asp:Label ID="Label734" runat="server" Text='<%# Eval("d_20")%>' CssClass="normal"></asp:Label>
                            </a>
                        </ItemTemplate>
                        <ItemStyle CssClass="col_50H " />
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="d_07" SortExpression="d_07">
                        <ItemTemplate>
                            <a href="javascript:openPopup('popup.aspx?sesja=<%# Eval("id_sedziego")+ "!"+Eval("id_tabeli") +"!21!6"%>')">
                                <asp:Label ID="Label735" runat="server" Text='<%# Eval("d_21")%>' CssClass="normal"></asp:Label>
                            </a>
                        </ItemTemplate>
                        <ItemStyle CssClass="col_50H" />
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="d_07" SortExpression="d_07">
                        <ItemTemplate>
                            <a href="javascript:openPopup('popup.aspx?sesja=<%# Eval("id_sedziego")+ "!"+Eval("id_tabeli") +"!22!6"%>')">
                                <asp:Label ID="Label736" runat="server" Text='<%# Eval("d_22")%>' CssClass="normal"></asp:Label>
                            </a>
                        </ItemTemplate>
                        <ItemStyle CssClass="col_50H" />
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="d_07" SortExpression="d_07">
                        <ItemTemplate>
                            <a href="javascript:openPopup('popup.aspx?sesja=<%# Eval("id_sedziego")+ "!"+Eval("id_tabeli") +"!23!6"%>')">
                                <asp:Label ID="Label737" runat="server" Text='<%# Eval("d_23")%>' CssClass="normal"></asp:Label>
                            </a>
                        </ItemTemplate>
                        <ItemStyle CssClass="col_50H" />
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="d_07" SortExpression="d_07">
                        <ItemTemplate>
                            <a href="javascript:openPopup('popup.aspx?sesja=<%# Eval("id_sedziego")+ "!"+Eval("id_tabeli") +"!24!6"%>')">
                                <asp:Label ID="Label738" runat="server" Text='<%# Eval("d_24")%>' CssClass="normal"></asp:Label>
                            </a>
                        </ItemTemplate>
                        <ItemStyle CssClass="col_50H" />
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="d_07" SortExpression="d_07">
                        <ItemTemplate>
                            <a href="javascript:openPopup('popup.aspx?sesja=<%# Eval("id_sedziego")+ "!"+Eval("id_tabeli") +"!25!6"%>')">
                                <asp:Label ID="Label739" runat="server" Text='<%# Eval("d_25")%>' CssClass="normal"></asp:Label>
                            </a>
                        </ItemTemplate>
                        <ItemStyle CssClass="col_50H" />
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="d_07" SortExpression="d_07">
                        <ItemTemplate>
                            <a href="javascript:openPopup('popup.aspx?sesja=<%# Eval("id_sedziego")+ "!"+Eval("id_tabeli") +"!26!6"%>')">
                                <asp:Label ID="Label740" runat="server" Text='<%# Eval("d_26")%>' CssClass="normal"></asp:Label>
                            </a>
                        </ItemTemplate>
                        <ItemStyle CssClass="col_50H  " />
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="d_07" SortExpression="d_07">
                        <ItemTemplate>
                            <a href="javascript:openPopup('popup.aspx?sesja=<%# Eval("id_sedziego")+ "!"+Eval("id_tabeli") +"!27!6"%>')">
                                <asp:Label ID="Label741" runat="server" Text='<%# Eval("d_27")%>' CssClass="normal"></asp:Label>
                            </a>
                        </ItemTemplate>
                        <ItemStyle CssClass="col_50H" />
                    </asp:TemplateField>
                     <asp:TemplateField HeaderText="d_07" SortExpression="d_07">
                        <ItemTemplate>
                            <a href="javascript:openPopup('popup.aspx?sesja=<%# Eval("id_sedziego")+ "!"+Eval("id_tabeli") +"!28!6"%>')">
                                <asp:Label ID="Label742" runat="server" Text='<%# Eval("d_28")%>' CssClass="normal"></asp:Label>
                            </a>
                        </ItemTemplate>
                        <ItemStyle CssClass="col_50H" />
                    </asp:TemplateField>
                       <asp:TemplateField HeaderText="d_07" SortExpression="d_07">
                        <ItemTemplate>
                            <a href="javascript:openPopup('popup.aspx?sesja=<%# Eval("id_sedziego")+ "!"+Eval("id_tabeli") +"!29!6"%>')">
                                <asp:Label ID="Label74555" runat="server" Text='<%# Eval("d_29")%>' CssClass="normal"></asp:Label>
                            </a>
                        </ItemTemplate>
                        <ItemStyle CssClass="col_50H" />
                    </asp:TemplateField>
                           
                       <asp:TemplateField HeaderText="d_07" SortExpression="d_07">
                        <ItemTemplate>
                            <a href="javascript:openPopup('popup.aspx?sesja=<%# Eval("id_sedziego")+ "!"+Eval("id_tabeli") +"!30!6"%>')">
                                <asp:Label ID="Label74556" runat="server" Text='<%# Eval("d_30")%>' CssClass="normal"></asp:Label>
                            </a>
                        </ItemTemplate>
                        <ItemStyle CssClass="col_50H" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="d_07" SortExpression="d_07">
                        <ItemTemplate>
                            <asp:TextBox ID="TextBoxUwagi" runat="server" CssClass="col_29"></asp:TextBox>
                        </ItemTemplate>
                        <ItemStyle CssClass="col_50H" />
                    </asp:TemplateField>
                </Columns>
                <FooterStyle Font-Bold="True" HorizontalAlign="Center" />
            </asp:GridView>

            <br />
        </div>
    </div>
    <div style="width: 1150px; margin: 0 auto 0 auto; position: relative; z-index: 11;">

        <div id="zalatwienia" class="page-break">
            <br />

            <asp:Label ID="lbTabela2" runat="server"></asp:Label>
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
       <asp:Label ID="infoLabel2" runat="server" Text="Tabela 3 \/" Visible="False"></asp:Label>
            <asp:GridView ID="GridView3" runat="server" AutoGenerateColumns="False" OnRowCreated="GridView3_RowCreated1"
                Width="100%" ShowHeader="False" ShowFooter="True" OnRowDataBound="StopkaTabeli03">
                <Columns>
                    <asp:BoundField DataField="id" HeaderText="L.p." SortExpression="id">
                        <ItemStyle Width="15px" />
                    </asp:BoundField>

                    <asp:TemplateField HeaderText="imie" SortExpression="imie">

                        <ItemTemplate>
                            <asp:Label ID="lbFunkcja" runat="server" Text='<%# Eval("funkcja") %>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle CssClass="t2_nazwisko" />
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="stanowosko" SortExpression="imie">

                        <ItemTemplate>
                            <asp:Label ID="Label10" runat="server" Text='<%# Eval("stanowisko") %>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle CssClass="t2_nazwisko" />
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="imie" SortExpression="imie">

                        <ItemTemplate>

                            <asp:Label ID="lbImie" runat="server" Text='<%# Bind("imie") %>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle CssClass="t2_nazwisko" />
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="nazwosko" SortExpression="nazwisko">

                        <ItemTemplate>

                            <asp:Label ID="lbNazwisko" runat="server" Text='<%# Bind("nazwisko") %>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle CssClass="t2_nazwisko" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="d_01" SortExpression="d_01">

                        <ItemTemplate>
                            <a href="javascript:openPopup('popup.aspx?sesja=<%# Eval("id_sedziego")+ "!"+Eval("id_tabeli") +"!1!6"%>')">
                                <asp:Label ID="Label201" runat="server" Text='<%# Eval("d_01")%>' CssClass="normal"></asp:Label>
                            </a>
                        </ItemTemplate>
                        <ItemStyle CssClass="col_37" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="d_02" SortExpression="d_02">

                        <ItemTemplate>
                            <a href="javascript:openPopup('popup.aspx?sesja=<%# Eval("id_sedziego")+ "!"+Eval("id_tabeli") +"!2!6"%>')">
                                <asp:Label ID="Label202" runat="server" Text='<%# Eval("d_02")%>' CssClass="normal"></asp:Label>
                            </a>
                        </ItemTemplate>
                        <ItemStyle CssClass="col_37" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="d_03" SortExpression="d_03">

                        <ItemTemplate>
                            <a href="javascript:openPopup('popup.aspx?sesja=<%# Eval("id_sedziego")+ "!"+Eval("id_tabeli") +"!3!6"%>')">
                                <asp:Label ID="Label203" runat="server" Text='<%# Eval("d_03")%>' CssClass="normal"></asp:Label>
                            </a>
                        </ItemTemplate>
                        <ItemStyle CssClass="col_37" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="d_04" SortExpression="d_04">

                        <ItemTemplate>
                            <a href="javascript:openPopup('popup.aspx?sesja=<%# Eval("id_sedziego")+ "!"+Eval("id_tabeli") +"!4!6"%>')">
                                <asp:Label ID="Label204" runat="server" Text='<%# Eval("d_04")%>' CssClass="normal"></asp:Label>
                            </a>
                        </ItemTemplate>
                        <ItemStyle CssClass="col_37" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="d_05" SortExpression="d_05">

                        <ItemTemplate>
                            <a href="javascript:openPopup('popup.aspx?sesja=<%# Eval("id_sedziego")+ "!"+Eval("id_tabeli") +"!5!6"%>')">
                                <asp:Label ID="Label205" runat="server" Text='<%# Eval("d_05")%>' CssClass="normal"></asp:Label>
                            </a>
                        </ItemTemplate>
                        <ItemStyle CssClass="col_37" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="d_06" SortExpression="d_06">

                        <ItemTemplate>
                            <a href="javascript:openPopup('popup.aspx?sesja=<%# Eval("id_sedziego")+ "!"+Eval("id_tabeli") +"!6!6"%>')">
                                <asp:Label ID="Label206" runat="server" Text='<%# Eval("d_06")%>' CssClass="normal"></asp:Label>
                            </a>
                        </ItemTemplate>
                        <ItemStyle CssClass="col_37" />
                    </asp:TemplateField>
                </Columns>
                <FooterStyle Font-Bold="True" HorizontalAlign="Center" CssClass="gray" />
            </asp:GridView>

            <br />
        </div>
        <div id="wyznaczenia" class="page-break Landscape horizont" style="margin-top:30px;">
            <asp:Label ID="lbTabela3" runat="server"></asp:Label>
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:Label ID="infoLabel3" runat="server" Text="Tabela 4 \/" Visible="False"></asp:Label>
            <br />
            <asp:GridView ID="GridView4" runat="server" AutoGenerateColumns="False" OnRowCreated="GridView3_RowCreated" Width="100%" ShowHeader="False" OnRowDataBound="StopkaTabeli04" ShowFooter="True">
                <Columns>
                    <asp:BoundField DataField="id" HeaderText="L.p." SortExpression="id">
                        <ItemStyle Width="15px" />
                    </asp:BoundField>
                    <asp:TemplateField HeaderText="nazwisko" SortExpression="nazwisko">
                        <EditItemTemplate>
                            <asp:TextBox ID="TextBox9" runat="server" Text='<%# Bind("nazwisko") %>'></asp:TextBox>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="Label12" runat="server" Text='<%# Eval("nazwisko") %>'></asp:Label>
                            &nbsp;<asp:Label ID="Label1" runat="server" Text='<%# Bind("imie") %>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle CssClass="t3_nazwisko" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="d_01" SortExpression="d_01">

                        <ItemTemplate>
                            <a href="javascript:openPopup('popup.aspx?sesja=<%# Eval("id_sedziego")+ "!"+Eval("id_tabeli") +"!1!6"%>')">
                                <asp:Label ID="Label301" runat="server" Text='<%# Eval("d_01")%>' CssClass="normal"></asp:Label>
                            </a>
                        </ItemTemplate>
                        <ItemStyle CssClass="col_46" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="d_02" SortExpression="d_02">

                        <ItemTemplate>
                            <a href="javascript:openPopup('popup.aspx?sesja=<%# Eval("id_sedziego")+ "!"+Eval("id_tabeli") +"!2!6"%>')">
                                <asp:Label ID="Label302" runat="server" Text='<%# Eval("d_02")%>' CssClass="normal"></asp:Label>
                            </a>
                        </ItemTemplate>
                        <ItemStyle CssClass="col_46" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="d_03" SortExpression="d_03">

                        <ItemTemplate>
                            <a href="javascript:openPopup('popup.aspx?sesja=<%# Eval("id_sedziego")+ "!"+Eval("id_tabeli") +"!3!6"%>')">
                                <asp:Label ID="Label303" runat="server" Text='<%# Eval("d_03")%>' CssClass="normal"></asp:Label>
                            </a>
                        </ItemTemplate>
                        <ItemStyle CssClass="col_46" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="d_04" SortExpression="d_04">

                        <ItemTemplate>
                            <a href="javascript:openPopup('popup.aspx?sesja=<%# Eval("id_sedziego")+ "!"+Eval("id_tabeli") +"!4!6"%>')">
                                <asp:Label ID="Label304" runat="server" Text='<%# Eval("d_04")%>' CssClass="normal"></asp:Label>
                            </a>
                        </ItemTemplate>
                        <ItemStyle CssClass="col_46" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="d_05" SortExpression="d_05">

                        <ItemTemplate>
                            <a href="javascript:openPopup('popup.aspx?sesja=<%# Eval("id_sedziego")+ "!"+Eval("id_tabeli") +"!5!6"%>')">
                                <asp:Label ID="Label305" runat="server" Text='<%# Eval("d_05")%>' CssClass="normal"></asp:Label>
                            </a>
                        </ItemTemplate>
                        <ItemStyle CssClass="col_46" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="d_06" SortExpression="d_06">

                        <ItemTemplate>
                            <a href="javascript:openPopup('popup.aspx?sesja=<%# Eval("id_sedziego")+ "!"+Eval("id_tabeli") +"!6!6"%>')">
                                <asp:Label ID="Label306" runat="server" Text='<%# Eval("d_06")%>' CssClass="normal"></asp:Label>
                            </a>
                        </ItemTemplate>
                        <ItemStyle CssClass="col_46" />
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="d_07" SortExpression="d_07">
                        <ItemTemplate>
                            <a href="javascript:openPopup('popup.aspx?sesja=<%# Eval("id_sedziego")+ "!"+Eval("id_tabeli") +"!7!6"%>')">
                                <asp:Label ID="Label307" runat="server" Text='<%# Eval("d_07")%>' CssClass="normal"></asp:Label>
                            </a>
                        </ItemTemplate>
                        <ItemStyle CssClass="col_46" />
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="d_07" SortExpression="d_07">
                        <ItemTemplate>
                            <a href="javascript:openPopup('popup.aspx?sesja=<%# Eval("id_sedziego")+ "!"+Eval("id_tabeli") +"!8!6"%>')">
                                <asp:Label ID="Label308" runat="server" Text='<%# Eval("d_08")%>' CssClass="normal"></asp:Label>
                            </a>
                        </ItemTemplate>
                        <ItemStyle CssClass="col_46" />
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="d_07" SortExpression="d_07">
                        <ItemTemplate>
                            <a href="javascript:openPopup('popup.aspx?sesja=<%# Eval("id_sedziego")+ "!"+Eval("id_tabeli") +"!9!6"%>')">
                                <asp:Label ID="Label309" runat="server" Text='<%# Eval("d_09")%>' CssClass="normal"></asp:Label>
                            </a>
                        </ItemTemplate>
                        <ItemStyle CssClass="col_46" />
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="d_07" SortExpression="d_07">
                        <ItemTemplate>
                            <a href="javascript:openPopup('popup.aspx?sesja=<%# Eval("id_sedziego")+ "!"+Eval("id_tabeli") +"!10!6"%>')">
                                <asp:Label ID="Label310" runat="server" Text='<%# Eval("d_10")%>' CssClass="normal"></asp:Label>
                            </a>
                        </ItemTemplate>
                        <ItemStyle CssClass="col_46" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="d_07" SortExpression="d_07">
                        <ItemTemplate>
                            <a href="javascript:openPopup('popup.aspx?sesja=<%# Eval("id_sedziego")+ "!"+Eval("id_tabeli") +"!11!6"%>')">
                                <asp:Label ID="Label311" runat="server" Text='<%# Eval("d_11")%>' CssClass="normal"></asp:Label>
                            </a>
                        </ItemTemplate>
                        <ItemStyle CssClass="col_46" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="d_07" SortExpression="d_07">
                        <ItemTemplate>
                            <a href="javascript:openPopup('popup.aspx?sesja=<%# Eval("id_sedziego")+ "!"+Eval("id_tabeli") +"!12!6"%>')">
                                <asp:Label ID="Label312" runat="server" Text='<%# Eval("d_12")%>' CssClass="normal"></asp:Label>
                            </a>
                        </ItemTemplate>
                        <ItemStyle CssClass="col_46" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="d_07" SortExpression="d_07">
                        <ItemTemplate>
                            <a href="javascript:openPopup('popup.aspx?sesja=<%# Eval("id_sedziego")+ "!"+Eval("id_tabeli") +"!13!6"%>')">
                                <asp:Label ID="Label313" runat="server" Text='<%# Eval("d_13")%>' CssClass="normal"></asp:Label>
                            </a>
                        </ItemTemplate>
                        <ItemStyle CssClass="col_46" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="d_07" SortExpression="d_07">
                        <ItemTemplate>
                            <a href="javascript:openPopup('popup.aspx?sesja=<%# Eval("id_sedziego")+ "!"+Eval("id_tabeli") +"!14!6"%>')">
                                <asp:Label ID="Label314" runat="server" Text='<%# Eval("d_14")%>' CssClass="normal"></asp:Label>
                            </a>
                        </ItemTemplate>
                        <ItemStyle CssClass="col_46" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="d_07" SortExpression="d_07">
                        <ItemTemplate>
                            <a href="javascript:openPopup('popup.aspx?sesja=<%# Eval("id_sedziego")+ "!"+Eval("id_tabeli") +"!15!6"%>')">
                                <asp:Label ID="Label315" runat="server" Text='<%# Eval("d_15")%>' CssClass="normal"></asp:Label>
                            </a>
                        </ItemTemplate>
                        <ItemStyle CssClass="col_46" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="d_07" SortExpression="d_07">
                        <ItemTemplate>
                            <a href="javascript:openPopup('popup.aspx?sesja=<%# Eval("id_sedziego")+ "!"+Eval("id_tabeli") +"!16!6"%>')">
                                <asp:Label ID="Label316" runat="server" Text='<%# Eval("d_16")%>' CssClass="normal"></asp:Label>
                            </a>
                        </ItemTemplate>
                        <ItemStyle CssClass="col_46" />
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="d_07" SortExpression="d_07">
                        <ItemTemplate>
                            <a href="javascript:openPopup('popup.aspx?sesja=<%# Eval("id_sedziego")+ "!"+Eval("id_tabeli") +"!17!6"%>')">
                                <asp:Label ID="Label317" runat="server" Text='<%# Eval("d_17")%>' CssClass="normal"></asp:Label>
                            </a>
                        </ItemTemplate>
                        <ItemStyle CssClass="col_46" />
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="d_07" SortExpression="d_07">
                        <ItemTemplate>
                            <a href="javascript:openPopup('popup.aspx?sesja=<%# Eval("id_sedziego")+ "!"+Eval("id_tabeli") +"!18!6"%>')">
                                <asp:Label ID="Label318" runat="server" Text='<%# Eval("d_18")%>' CssClass="normal"></asp:Label>
                            </a>
                        </ItemTemplate>
                        <ItemStyle CssClass="col_46" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="d_07" SortExpression="d_07">
                        <ItemTemplate>
                            <a href="javascript:openPopup('popup.aspx?sesja=<%# Eval("id_sedziego")+ "!"+Eval("id_tabeli") +"!19!6"%>')">
                                <asp:Label ID="Label319" runat="server" Text='<%# Eval("d_19")%>' CssClass="normal"></asp:Label>
                            </a>
                        </ItemTemplate>
                        <ItemStyle CssClass="col_46" />
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="d_07" SortExpression="d_07">
                        <ItemTemplate>
                            <a href="javascript:openPopup('popup.aspx?sesja=<%# Eval("id_sedziego")+ "!"+Eval("id_tabeli") +"!20!6"%>')">
                                <asp:Label ID="Label320" runat="server" Text='<%# Eval("d_20")%>' CssClass="normal"></asp:Label>
                            </a>
                        </ItemTemplate>
                        <ItemStyle CssClass="col_46" />
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="d_07" SortExpression="d_07">
                        <ItemTemplate>
                            <a href="javascript:openPopup('popup.aspx?sesja=<%# Eval("id_sedziego")+ "!"+Eval("id_tabeli") +"!21!6"%>')">
                                <asp:Label ID="Label321" runat="server" Text='<%# Eval("d_21")%>' CssClass="normal"></asp:Label>
                            </a>
                        </ItemTemplate>
                        <ItemStyle CssClass="col_46" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="d_07" SortExpression="d_07">
                        <ItemTemplate>
                            <a href="javascript:openPopup('popup.aspx?sesja=<%# Eval("id_sedziego")+ "!"+Eval("id_tabeli") +"!22!6"%>')">
                                <asp:Label ID="Label322" runat="server" Text='<%# Eval("d_22")%>' CssClass="normal"></asp:Label>
                            </a>
                        </ItemTemplate>
                        <ItemStyle CssClass="col_46" />
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="d_07" SortExpression="d_07">
                        <ItemTemplate>
                            <a href="javascript:openPopup('popup.aspx?sesja=<%# Eval("id_sedziego")+ "!"+Eval("id_tabeli") +"!23!6"%>')">
                                <asp:Label ID="Label323" runat="server" Text='<%# Eval("d_23")%>' CssClass="normal"></asp:Label>
                            </a>
                        </ItemTemplate>
                        <ItemStyle CssClass="col_46" />
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="d_07" SortExpression="d_07">
                        <ItemTemplate>
                            <a href="javascript:openPopup('popup.aspx?sesja=<%# Eval("id_sedziego")+ "!"+Eval("id_tabeli") +"!24!6"%>')">
                                <asp:Label ID="Label324" runat="server" Text='<%# Eval("d_24")%>' CssClass="normal"></asp:Label>
                            </a>
                        </ItemTemplate>
                        <ItemStyle CssClass="col_46" />
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="d_07" SortExpression="d_07">
                        <ItemTemplate>
                            <a href="javascript:openPopup('popup.aspx?sesja=<%# Eval("id_sedziego")+ "!"+Eval("id_tabeli") +"!25!6"%>')">
                                <asp:Label ID="Label325" runat="server" Text='<%# Eval("d_25")%>' CssClass="normal"></asp:Label>
                            </a>
                        </ItemTemplate>
                        <ItemStyle CssClass="col_46" />
                    </asp:TemplateField>
                </Columns>
                <FooterStyle Font-Bold="True" HorizontalAlign="Center" CssClass="gray" />
                <RowStyle Height="25px" />
            </asp:GridView>
            <br />
        </div>
        <div id="Div11" class="page-break">
            &nbsp;<asp:Label ID="lbTabela4" runat="server">IV Terminowość sporządzania uzasadnień, absencja</asp:Label>
            &nbsp;
    &nbsp;<asp:Label ID="infoLabel4" runat="server" Text="Tabela 5 \/" Visible="False"></asp:Label>
            <asp:GridView ID="GridView5" runat="server" AutoGenerateColumns="False" OnRowCreated="GridView4_RowCreated" Width="100%" ShowHeader="False" OnRowDataBound="StopkaTabeli05" ShowFooter="True">
                <Columns>
                    <asp:BoundField DataField="id" HeaderText="L.p." SortExpression="id">
                        <ItemStyle Width="15px" />
                    </asp:BoundField>
                    <asp:TemplateField HeaderText="nazwisko" SortExpression="nazwisko">
                        <EditItemTemplate>
                            <asp:TextBox ID="TextBox28" runat="server" Text='<%# Bind("nazwisko") %>'></asp:TextBox>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="Label14" runat="server" Text='<%# Eval("nazwisko") %>'></asp:Label>
                            &nbsp;<asp:Label ID="Label13" runat="server" Text='<%# Bind("imie") %>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle CssClass="t4_nazwisko" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="d_01" SortExpression="d_01">

                        <ItemTemplate>
                            <a href="javascript:openPopup('popup.aspx?sesja=<%# Eval("id_sedziego")+ "!"+Eval("id_tabeli") +"!1!6"%>')">
                                <asp:Label ID="Label401" runat="server" Text='<%# Eval("d_01")%>' CssClass="normal"></asp:Label>
                            </a>
                        </ItemTemplate>
                        <ItemStyle CssClass="col_50H" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="d_02" SortExpression="d_02">

                        <ItemTemplate>
                            <a href="javascript:openPopup('popup.aspx?sesja=<%# Eval("id_sedziego")+ "!"+Eval("id_tabeli") +"!2!6"%>')">
                                <asp:Label ID="Label402" runat="server" Text='<%# Eval("d_02")%>' CssClass="normal"></asp:Label>
                            </a>
                        </ItemTemplate>
                        <ItemStyle CssClass="col_50H" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="d_03" SortExpression="d_03">

                        <ItemTemplate>
                            <a href="javascript:openPopup('popup.aspx?sesja=<%# Eval("id_sedziego")+ "!"+Eval("id_tabeli") +"!3!6"%>')">
                                <asp:Label ID="Label403" runat="server" Text='<%# Eval("d_03")%>' CssClass="normal"></asp:Label>
                            </a>
                        </ItemTemplate>
                        <ItemStyle CssClass="col_50H" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="d_04" SortExpression="d_04">

                        <ItemTemplate>
                            <a href="javascript:openPopup('popup.aspx?sesja=<%# Eval("id_sedziego")+ "!"+Eval("id_tabeli") +"!4!6"%>')">
                                <asp:Label ID="Label404" runat="server" Text='<%# Eval("d_04")%>' CssClass="normal"></asp:Label>
                            </a>
                        </ItemTemplate>
                        <ItemStyle CssClass="col_50H" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="d_05" SortExpression="d_05">

                        <ItemTemplate>
                            <a href="javascript:openPopup('popup.aspx?sesja=<%# Eval("id_sedziego")+ "!"+Eval("id_tabeli") +"!5!6"%>')">
                                <asp:Label ID="Label405" runat="server" Text='<%# Eval("d_05")%>' CssClass="normal"></asp:Label>
                            </a>
                        </ItemTemplate>
                        <ItemStyle CssClass="col_50H" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="d_06" SortExpression="d_06">

                        <ItemTemplate>
                            <a href="javascript:openPopup('popup.aspx?sesja=<%# Eval("id_sedziego")+ "!"+Eval("id_tabeli") +"!6!6"%>')">
                                <asp:Label ID="Label406" runat="server" Text='<%# Eval("d_06")%>' CssClass="normal"></asp:Label>
                            </a>
                        </ItemTemplate>
                        <ItemStyle CssClass="col_50H" />
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="d_06" SortExpression="d_06">

                        <ItemTemplate>
                            <a href="javascript:openPopup('popup.aspx?sesja=<%# Eval("id_sedziego")+ "!"+Eval("id_tabeli") +"!7!6"%>')">
                                <asp:Label ID="Label407" runat="server" Text='<%# Eval("d_07")%>' CssClass="normal"></asp:Label>
                            </a>
                        </ItemTemplate>
                        <ItemStyle CssClass="col_50H" />
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="d_06" SortExpression="d_06">

                        <ItemTemplate>
                            <a href="javascript:openPopup('popup.aspx?sesja=<%# Eval("id_sedziego")+ "!"+Eval("id_tabeli") +"!8!6"%>')">
                                <asp:Label ID="Label408" runat="server" Text='<%# Eval("d_08")%>' CssClass="normal"></asp:Label>
                            </a>
                        </ItemTemplate>
                        <ItemStyle CssClass="col_50H" />
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="d_06" SortExpression="d_06">

                        <ItemTemplate>
                            <a href="javascript:openPopup('popup.aspx?sesja=<%# Eval("id_sedziego")+ "!"+Eval("id_tabeli") +"!9!6"%>')">
                                <asp:Label ID="Label409" runat="server" Text='<%# Eval("d_09")%>' CssClass="normal"></asp:Label>
                            </a>
                        </ItemTemplate>
                        <ItemStyle CssClass="col_50H" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="d_06" SortExpression="d_06">

                        <ItemTemplate>
                            <a href="javascript:openPopup('popup.aspx?sesja=<%# Eval("id_sedziego")+ "!"+Eval("id_tabeli") +"!10!6"%>')">
                                <asp:Label ID="Label410" runat="server" Text='<%# Eval("d_10")%>' CssClass="normal"></asp:Label>
                            </a>
                        </ItemTemplate>
                        <ItemStyle CssClass="col_50H" />
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="d_06" SortExpression="d_06">

                        <ItemTemplate>
                            <a href="javascript:openPopup('popup.aspx?sesja=<%# Eval("id_sedziego")+ "!"+Eval("id_tabeli") +"!11!6"%>')">
                                <asp:Label ID="Label411" runat="server" Text='<%# Eval("d_11")%>' CssClass="normal"></asp:Label>
                            </a>
                        </ItemTemplate>
                        <ItemStyle CssClass="col_50H" />
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="d_06" SortExpression="d_06">

                        <ItemTemplate>
                            <a href="javascript:openPopup('popup.aspx?sesja=<%# Eval("id_sedziego")+ "!"+Eval("id_tabeli") +"!12!6"%>')">
                                <asp:Label ID="Label412" runat="server" Text='<%# Eval("d_12")%>' CssClass="normal"></asp:Label>
                            </a>
                        </ItemTemplate>
                        <ItemStyle CssClass="col_50H" />
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="d_06" SortExpression="d_06">

                        <ItemTemplate>
                            <a href="javascript:openPopup('popup.aspx?sesja=<%# Eval("id_sedziego")+ "!"+Eval("id_tabeli") +"!13!6"%>')">
                                <asp:Label ID="Label413" runat="server" Text='<%# Eval("d_13")%>' CssClass="normal"></asp:Label>
                            </a>
                        </ItemTemplate>
                        <ItemStyle CssClass="col_50H" />
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="d_06" SortExpression="d_06">

                        <ItemTemplate>
                            <a href="javascript:openPopup('popup.aspx?sesja=<%# Eval("id_sedziego")+ "!"+Eval("id_tabeli") +"!14!6"%>')">
                                <asp:Label ID="Label414" runat="server" Text='<%# Eval("d_14")%>' CssClass="normal"></asp:Label>
                            </a>
                        </ItemTemplate>
                        <ItemStyle CssClass="col_50H" />
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="d_06" SortExpression="d_06">

                        <ItemTemplate>
                            <a href="javascript:openPopup('popup.aspx?sesja=<%# Eval("id_sedziego")+ "!"+Eval("id_tabeli") +"!15!6"%>')">
                                <asp:Label ID="Label415" runat="server" Text='<%# Eval("d_15")%>' CssClass="normal"></asp:Label>
                            </a>
                        </ItemTemplate>
                        <ItemStyle CssClass="col_50H" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="d_06" SortExpression="d_06">

                        <ItemTemplate>
                            <a href="javascript:openPopup('popup.aspx?sesja=<%# Eval("id_sedziego")+ "!"+Eval("id_tabeli") +"!16!6"%>')">
                                <asp:Label ID="Label416" runat="server" Text='<%# Eval("d_16")%>' CssClass="normal"></asp:Label>
                            </a>
                        </ItemTemplate>
                        <ItemStyle CssClass="col_50H" />
                    </asp:TemplateField>
                </Columns>
                <FooterStyle Font-Bold="True" HorizontalAlign="Center" CssClass="gray" />
            </asp:GridView>
            <br />
        </div>

        <div id="debag">
            <br />
            Raport statystyczny
                    <asp:Label ID="Label27" runat="server"></asp:Label>
            &nbsp;Sporzadzone dn.
            <asp:Label ID="Label29" runat="server"></asp:Label>&nbsp;przez&nbsp;
&nbsp;&nbsp;
            <asp:Label ID="Label28" runat="server"></asp:Label>
            &nbsp;<asp:Label ID="Label30" runat="server"></asp:Label>
            <br />

            <asp:Label ID="Label11" runat="server"></asp:Label>
        </div>

        <br />
        <br />

        <br />
    </div>
</asp:Content>