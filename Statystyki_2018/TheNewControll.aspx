<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="TheNewControll.aspx.cs" Inherits="stat2018.TheNewControll" %>
<%@ Register assembly="DevExpress.Web.v17.1, Version=17.1.17.0,  Culture=neutral,  PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    
      <div class="noprint">
      <div id="menu" style="background-color: #f7f7f7; z-index: 9999">
          <div class="manu_back" style="height: 43px; margin: 0 auto 0 auto; position: relative; width: 1150px; left: 0px;">

              <table class="tbl_manu">

                  <tr>
                      <td style="width: 90px;">
                          <asp:ScriptManager ID="ScriptManager1" runat="server">
                          </asp:ScriptManager>
                          <asp:Label ID="Label4" runat="server" Text="Zakres:"></asp:Label>
                      </td>
                      <td style="width: 80px;">

                <dx:ASPxDateEdit ID="Date1" runat="server" Theme="Moderno">
                </dx:ASPxDateEdit>
              


                      </td>
                      <td style="width: 80px;">
               
                       <dx:ASPxDateEdit ID="Date2" runat="server" Theme="Moderno">
                </dx:ASPxDateEdit>
                      </td>
                      <td style="width: 100px">
                          <asp:LinkButton ID="LinkButton54" runat="server" class="ax_box" OnClick="Refresh">  Odśwież</asp:LinkButton>
                      </td>

                      <td>Id. raportu</td>
                      <td>

                          <asp:TextBox ID="idRaportu" runat="server"></asp:TextBox>
                      </td>
                      <td>Id. Sądu</td>
                      <td>

                          <asp:TextBox ID="idSad" runat="server"></asp:TextBox>
                      </td>
                      <td>
                          <asp:Button ID="Button1" runat="server" CssClass="ax_box" Text="Twórz plik csv" OnClick="MakeExcel" />
                      </td>
                  </tr>
              </table>
          </div>
      </div>
  </div>

 <div style="width: 1150px; margin: 0 auto 0 auto; position: relative;" >


     <div id="table01">

         aaa

        <dx:ASPxGridView ID="grid" 
            
            runat="server" 
            EnableTheming="True" 
            OnDataBinding="dataBinding" 
            Theme="Moderno"
            EnableCallbackAnimation="True" 
             ViewStateMode="Enabled" OnCustomColumnDisplayText="grid_CustomColumnDisplayText"
            Settings-UseFixedtableLayout ="true" Width="100%"
            
            >
            <Settings HorizontalScrollBarMode="Visible" UseFixedTableLayout="True" ShowGroupedColumns="True"  />

 <SettingsBehavior AllowSort="False" />

 <SettingsResizing ColumnResizeMode="NextColumn" />

              <SettingsDetail ExportMode="All" />
           
              <SettingsAdaptivity>
                  <AdaptiveDetailLayoutProperties>
                      <SettingsAdaptivity AdaptivityMode="SingleColumnWindowLimit" />
                      <SettingsItems Width="100%" VerticalAlign="Middle" />
                      <Styles>
                          <LayoutItem CssClass="NowaDopasowanie">
                          </LayoutItem>
                      </Styles>
                      <Paddings PaddingLeft="3px" PaddingRight="2px" />
                  </AdaptiveDetailLayoutProperties>
              </SettingsAdaptivity>
             <SettingsBehavior AllowEllipsisInText="true" />
      
           <SettingsPager AlwaysShowPager="True" EnableAdaptivity="True">
            </SettingsPager>
           
        
        
            <Settings 
               
                VerticalScrollBarMode="Visible"
                VerticalScrollableHeight="640"
                ShowFilterRow="True" 
                EnableFilterControlPopupMenuScrolling="True" 
                ShowFilterBar="Auto" 
                ShowFilterRowMenu="True" 
                ShowGroupFooter="VisibleAlways" ShowFooter="True" ShowHeaderFilterButton="True" />
              <SettingsBehavior AllowFixedGroups="true" AllowFocusedRow="True" />
            <SettingsDataSecurity 
                AllowDelete="False" 
                AllowEdit="False" 
                AllowInsert="False" />
              <SettingsSearchPanel ShowClearButton="True" Visible="True" />
            <Styles>
                <Header Wrap="True">
                </Header>
                <DetailRow Wrap="True">
                </DetailRow>
                <DetailCell Wrap="True">
                </DetailCell>
                <Cell Wrap="False">
                </Cell>
                <FocusedCell BackColor="#FFCC00">
                </FocusedCell>
            </Styles>
          
            
        </dx:ASPxGridView>

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

            </div>
     </div>
</asp:Content>
