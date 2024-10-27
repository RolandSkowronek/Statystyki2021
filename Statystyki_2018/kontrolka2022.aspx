<%@ Page Title="" Language="C#" UICulture="pl" Culture="pl-PL" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="kontrolka2022.aspx.cs" Inherits="Statystyki_2018.kontrolka2022"  meta:resourcekey="PageResource1" MaintainScrollPositionOnPostback="true"   %>

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

      <style type="text/css">
        body, html {
            padding: 0;
            margin: 0;
        }

        .first  
{  background-color:indianred;
    text-align:center;  
    padding-left:0px !important;  
    padding-right:0px!important;  
    width: 14px!important; /*1px if for the border*/  
 
}
    </style>
    <script type="text/javascript">
        function OnInit(s, e) {
            AdjustSize();
            document.getElementById("gridContainer").style.visibility = "";
        }
        function OnEndCallback(s, e) {
            AdjustSize();
        }
        function OnControlsInitialized(s, e) {
            ASPxClientUtils.AttachEventToElement(window, "resize", function (evt) {
                AdjustSize();
            });
        }
        function AdjustSize() {
            var height = Math.max(0, document.documentElement.clientHeight);
            grid.SetHeight(height);
        }

        var doProcessClick;
        var index;
        function ProcessClick() {
            if (doProcessClick) {
                //  alert(`Here is the RowClick action in the ${index.toString()} row.`);
            }
        }
        function OnRowClick(s, e) {
            doProcessClick = true;
            index = e.visibleIndex + 1;
            window.setTimeout(ProcessClick, 500);
        }

        function OnRowDblClick(s, e) {
            alert(`Here is the RowClick action in the ${index.toString()} row.`);
            var indexX = index;
          
            doProcessClick = false;

            //  openPopup('popup.aspx?sesja=1!1!11!3');

            //var grid = gridLookUp.GetGridView();
            alert(indexX);
            var keys = grid.GetSelectedKeysOnPage();
            alert(keys);
            var row = grid.GetRowValues(indexX, 'Lp;numer', OnGetRowValues);
        }
        function OnGetRowValues(values) {
            alert('aaa00');
            if (values[0] == null) return;
            alert('Product: ' + values[0]);
        }
        
        function RunME() {

            alert('aaa00');
            const { exec } = require('child_process');
          
            exec('c:\\RunScript\\run.bat', (error, stdout, stderr) => {
                if (error) {
                    console.error(`error: ${error.message}`);
                    return;
                }
                if (stdout) {
                    console.log(`stdout: ${stdout}`);
                }
                if (stderr) {
                    console.error(`stderr: ${stderr}`);
                }
            });
            alert('aaa00');
        }
  
    </script>
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
                            <asp:LinkButton ID="LinkButton3" runat="server" CssClass="ax_box" OnClick="Excell" meta:resourcekey="LinkButton2Resource1" Width="100%">Zapisz do Excell</asp:LinkButton>
                        </td>
                          <td>
      <asp:LinkButton ID="LinkButton4" runat="server" CssClass="ax_box" OnClick="Automat" meta:resourcekey="LinkButton2Resource1" Width="100%">Zapisz do automatyzacji</asp:LinkButton>
  </td>
                                                <td>
                                                    &nbsp;</td>
                 <td> 
                     &nbsp;</td>
                    </tr>

                </table>
            </div>
            <asp:HiddenField ID="HiddenField1" runat="server" />
        </div>
    </div>


    <div data-bind="dxDataGrid: gridOptions" id="gridContainer">


        <dx:ASPxGridView ID="grid"
            runat="server"
            EnableTheming="True"
            OnDataBinding="dataBinding"
            Theme="Moderno"
            EnableCallbackAnimation="True"
            ViewStateMode="Enabled"
            OnCustomColumnDisplayText="grid_CustomColumnDisplayText"
            Settings-UseFixedTableLayout="True" Width="100%" OnSelectionChanged="grid_SelectionChanged"
            >  

  <ClientSideEvents RowClick="OnRowClick" RowDblClick="OnRowDblClick"/>
            <Settings
                HorizontalScrollBarMode="Visible"
                UseFixedTableLayout="True"
                ShowGroupedColumns="True"
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

            <SettingsBehavior
                AllowEllipsisInText="False"
                ColumnResizeMode="Control"
                AllowFixedGroups="true" 
                AllowFocusedRow="True"
            
                />



            <SettingsDetail ExportMode="All" />

            <SettingsAdaptivity AdaptivityMode="HideDataCellsWindowLimit">
                <AdaptiveDetailLayoutProperties>
                    <SettingsAdaptivity AdaptivityMode="SingleColumnWindowLimit" />
                    <SettingsItems Width="100%" VerticalAlign="Middle" />
                    <Styles>
                        <LayoutItem CssClass="first">
                        </LayoutItem>
                    </Styles>
                    <Paddings PaddingLeft="3px" PaddingRight="2px" />
                </AdaptiveDetailLayoutProperties>
            </SettingsAdaptivity>



            <SettingsPager 
                AlwaysShowPager="True" 
                EnableAdaptivity="True">
            </SettingsPager>

          
            <SettingsResizing ColumnResizeMode="Control" />
            <SettingsDataSecurity
                AllowDelete="False"
                AllowEdit="False"
                AllowInsert="False" />
            <SettingsSearchPanel ShowClearButton="True" Visible="False" />
            <Styles>
                <Header Wrap="True">
                </Header>
                <DetailRow Wrap="False">
                </DetailRow>
                <DetailCell Wrap="False">
                </DetailCell>
                <Cell Wrap="False">
                </Cell>
                <FocusedCell BackColor="#FFCC00">
                </FocusedCell>
            </Styles>


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

    </div>
             
         
         
       
    <asp:Label ID="Label1" runat="server" meta:resourcekey="Label1Resource1"></asp:Label>
</asp:Content>