<%@ Page Title="" Language="C#" UICulture="pl" Culture="pl-PL" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="test.aspx.cs" Inherits="Statystyki_2018.test" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <br />
    <br />
    <br />

    <script src="Scripts/jquery-1.8.3.js"></script>
    <script src="Scripts/rls.js"></script>

    <script type="text/javascript">
        window.onload = function () {
              var request = new XMLHttpRequest();
        request.open("GET", "time.txt", false);
        request.send(null);
        var returnValue = request.responseText;

      
         //   alert(returnValue);
        }
            function runPowerShellScript() {
                var script = document.getElementById("script").value;
                var process = new Process();
                process.Start("powershell.exe", ["-c", script]);
                process.WaitForExit();
            }
    
        function Kabooom() {
          
            var script = document.getElementById("script").value;
            alert(script);
            var powershell = new ActiveXObject("WScript.Shell");
            powershell.Run(script);
            alert("Kaboom End");
        }

    </script>

    <asp:GridView ID="GridView1" runat="server" DataSourceID="SqlDataSource1">
    </asp:GridView>
    <asp:TextBox ID="TextBox1" runat="server" Height="226px" TextMode="MultiLine" Width="588px"></asp:TextBox>
    <asp:SqlDataSource ID="SqlDataSource1" runat="server"></asp:SqlDataSource>
    <br />
    <br />
    <br />

    <asp:Button ID="Notatnik" runat="server" OnClick="Button1_Click" Text="Button" />
  
    <br />
    <br />
    <br />
    <br />


    </asp:Content>