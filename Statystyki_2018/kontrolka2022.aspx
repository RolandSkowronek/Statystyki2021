<%@ Page Title="" Language="C#" UICulture="pl" Culture="pl-PL" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="kontrolka2022.aspx.cs" Inherits="Statystyki_2018.kontrolka2022"  meta:resourcekey="PageResource1" MaintainScrollPositionOnPostback="true" %>

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
           
    </style>
   
    <script src="Scripts/rls.js"></script>

    <div class="noprint" style="margin-left: auto; margin-right: auto;">
        <div id="menu" style="background-color: #f7f7f7;">
            <div class="manu_back" style="height: 40px; margin: 0 auto 0 auto; position: relative; width: 1050px; left: 0px;">

                <table>
                    <tr>

                        <td style="padding: 0 5px 0 5px">Data od :</td>

                        <td style="padding: 0 5px 0 5px">
                            <dx:ASPxDateEdit ID="data1" runat="server" Width="100%" meta:resourcekey="data1Resource1" Theme="Moderno"></dx:ASPxDateEdit>
                        </td>

                        <td style="padding: 0 5px 0 5px">Data do :</td>

                        <td style="padding: 0 5px 0 5px">
                            <dx:ASPxDateEdit ID="data2" runat="server" Width="100%" meta:resourcekey="data2Resource1" Theme="Moderno"></dx:ASPxDateEdit>
                        </td>

                        <td style="padding: 0 5px 0 5px">
                            <asp:LinkButton ID="LinkButton1" runat="server" CssClass="ax_box" OnClick="szukaj" meta:resourcekey="LinkButton1Resource1">Szukaj</asp:LinkButton>
                        </td>

                        <td style="padding: 0 5px 0 5px">
                            <asp:LinkButton ID="LinkButton2" runat="server" CssClass="ax_box" OnClick="Druk" meta:resourcekey="LinkButton2Resource1">Drukuj</asp:LinkButton>
                        </td>
                        <td>
                            <asp:LinkButton ID="LinkButton3" runat="server" CssClass="ax_box" OnClick="Excell" meta:resourcekey="LinkButton2Resource1">Zapisz do Excell</asp:LinkButton>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>

    
    <div  id="gridContainer" >
           <dx:ASPxGridView ID="grid" ClientInstanceName="grid" runat="server" KeyFieldName="ID"
            Width="100%" AutoGenerateColumns="true" OnCustomColumnDisplayText="grid_CustomColumnDisplayText"
            OnSummaryDisplayText="grid_SummaryDisplayText" Theme="Moderno">
            <Columns>
        
             
            </Columns>
            <Settings 
              VerticalScrollBarMode="Visible"
VerticalScrollableHeight="640"
ShowFilterRow="True" 
EnableFilterControlPopupMenuScrolling="True" 
ShowFilterBar="Auto" 
ShowFilterRowMenu="True" 
ShowGroupFooter="VisibleAlways" 
ShowFooter="True" 
ShowHeaderFilterButton="True"
                />
            <SettingsPager>
                <PageSizeItemSettings Visible="true" Items="10, 20, 50" />
            </SettingsPager>
            <SettingsDataSecurity AllowInsert="false" AllowEdit="false" AllowDelete="false" />
            <ClientSideEvents Init="grid_Init" BeginCallback="grid_BeginCallback" EndCallback="grid_EndCallback" />
          
            <GroupSummary>
                <dx:ASPxSummaryItem SummaryType="Count" />
            </GroupSummary>
        </dx:ASPxGridView>
        <br />
      
        <dx:ASPxGridViewExporter ID="ASPxGridViewExporter1" 
            runat="server" 
            GridViewID="grid" 
            PaperKind="A4" 
            ExportedRowType="All" 
            PrintSelectCheckBox="True" 
            OnRenderBrick="ASPxGridViewExporter1_RenderBrick">
            <Styles>
                <Header Wrap="True">
                </Header>
                 
            </Styles>
        </dx:ASPxGridViewExporter>
        <br />
        <br />
        <br />
       
    </div>
    <asp:Label ID="Label1" runat="server" meta:resourcekey="Label1Resource1"></asp:Label>
</asp:Content>