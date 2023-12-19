<%@ Page Title="" Language="C#" UICulture="pl" Culture="pl-PL" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="redirector.aspx.cs" Inherits="Statystyki_2018.redirector" %>

<%@ Register Assembly="DevExpress.Web.v17.1, Version=17.1.17.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

     <div style="width: 1150px; margin: 0 auto 0 auto; position: relative; top: 60px; z-index: 11;" >
    <asp:Panel ID="PanelMenuGlowne" runat="server" Visible="False">
        <table style="width: 100%;    ">
        <tr>
           <td style="padding-right: 10px; padding-left: 10px; padding-top: 10px; width:33%;">
                <asp:Button ID="Button2" runat="server" CssClass="myButton" Text="Statystyczne" Width="100%" OnClick="Button2_Click" OnClientClick="return SetText(this)" Height="146px"/>
            </td>
           <td style="padding-right: 10px; padding-left: 10px; padding-top: 10px; width:33%;">
                <asp:Button ID="Button3" runat="server" CssClass="myButton" Text="Kontrolki" Width="100%" OnClick="Button3_Click" Height="146px"/>
            </td>
           <td style="padding-right: 10px; padding-left: 10px; padding-top: 10px; width:33%;">
                <asp:Button ID="Button4" runat="server" CssClass="myButton" Text="MSS" Width="100%" OnClick="Button4_Click" Height="146px"/>
            </td>
        </tr>
        <tr>
           <td style="padding-right: 10px; padding-left: 10px; padding-top: 10px; width:33%;">
                <asp:Button ID="Button1" runat="server" CssClass="myButton" Text="Inne" Width="100%" OnClick="Button1_Click" Height="146px"/>
            </td>
           <td style="padding-right: 10px; padding-left: 10px; padding-top: 10px; width:33%;">
                <asp:Button ID="Button5" runat="server" CssClass="myButton" Text="Wymiana" Width="100%" OnClick="Button5_Click" Height="146px"/>
            </td>
            <td style="padding-right: 10px; padding-left: 10px; padding-top: 10px; width:33%;">
                <asp:Button ID="Button6" runat="server" CssClass="myButton" Text="Administracja" Width="100%" OnClick="Administracja_Click" Height="146px"/>
            </td>
        </tr>
        <tr>
            <td style="padding-right: 10px; padding-left: 10px; padding-top: 10px; width:33%;">
                <asp:Button ID="Button7" runat="server" CssClass="myButton" Text="Wylogowanie" Width="100%" OnClick="Button7_Click" Height="146px"/>
            </td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
        </tr>
    </table>

    </asp:Panel>
         
         <br />
         <asp:Panel ID="menuKategorii" runat="server">
           
                    <dx:ASPxCardView ID="cardView" runat="server" AutoGenerateColumns="False" KeyFieldName="nazwa" EnableTheming="True"  Cursor="pointer" Width="100%" OnPageSizeChanged="select" ClientIDMode="AutoID" EnableCallbackAnimation="True" ValidateRequestMode="Enabled" ViewStateMode="Enabled">
                        <SettingsPager Visible="False">
                            <SettingsTableLayout ColumnCount="3" RowsPerPage="200" />
                        </SettingsPager>
                        <SettingsBehavior AllowSelectByCardClick="true" />
                        <ClientSideEvents SelectionChanged="OnCardSelectionChanged" />
                       
                        <SettingsBehavior AllowFocusedCard="true" />
                        <SettingsDataSecurity AllowDelete="False" AllowEdit="False" AllowInsert="False" />
                        <SettingsSearchPanel Visible="True" />
                        <Columns>
                            <dx:CardViewTextColumn FieldName="nazwa" VisibleIndex="0">
                            </dx:CardViewTextColumn>
                            <dx:CardViewTextColumn FieldName="plik" Visible="False" VisibleIndex="1">
                            </dx:CardViewTextColumn>
                            <dx:CardViewTextColumn FieldName="wydzial" ReadOnly="True" Visible="False" VisibleIndex="2">
                            </dx:CardViewTextColumn>
                            <dx:CardViewTextColumn FieldName="grupa" ReadOnly="True" Visible="False" VisibleIndex="3">
                            </dx:CardViewTextColumn>
                        </Columns>
                        <CardLayoutProperties ColCount="3">
                            <Items>
                                <dx:CardViewLayoutGroup Caption="" CssClass="fontInCell " GroupBoxDecoration="None" HorizontalAlign="Center" SettingsItems-HorizontalAlign="Center" SettingsItems-ShowCaption="False" SettingsItems-VerticalAlign="Middle" ShowCaption="False" VerticalAlign="Middle">
                                    <Items>
                                        <dx:CardViewColumnLayoutItem ColumnName="nazwa" />
                                    </Items>
                                    <SettingsItems HorizontalAlign="Center" ShowCaption="False" VerticalAlign="Middle" />
                                </dx:CardViewLayoutGroup>
                            </Items>
                        </CardLayoutProperties>
                        <SettingsSearchPanel Visible="true" ShowClearButton="true" />
                        <Styles Header-HorizontalAlign="Center" Card-HorizontalAlign="Center">
                            <Card CssClass="myButton" Border-BorderColor="Black" Border-BorderStyle="Solid" Border-BorderWidth="2px" Height="80px" />
                            <Header HorizontalAlign="Center">
                            </Header>
                         </Styles>
                        <Border BorderStyle="None" />
                    </dx:ASPxCardView>
              
         </asp:Panel>
     </div>
    


    </asp:Content>