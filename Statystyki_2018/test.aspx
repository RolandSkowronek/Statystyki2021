<%@ Page Title="" Language="C#" UICulture="pl" Culture="pl-PL" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="test.aspx.cs" Inherits="Statystyki_2018.test" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <br />
    <br />
    <br />

    <script src="Scripts/jquery-1.8.3.js"></script>
    <script src="Scripts/rls.js"></script>

    <script language="javascript" type="text/javascript">

        function RunEXE() {
            alert('AASSS');
            

            WshShell = new ActiveXObject("WScript.Shell");
            WshShell.Run("test.bat");
        }

        function RunME() {
            var shell = new ActiveXObject("WScript.Shell");
            var path = '%userprofile%\\Pulpit\\run.bat';
            shell.run(path, 1, false);
        }


        window.onload = function () {
              var request = new XMLHttpRequest();
        request.open("GET", "time.txt", false);
        request.send(null);
        var returnValue = request.responseText;

      
         //   alert(returnValue);
        }
        function runPowerShellScript() {
          
            var path = '%userprofile%\\Pulpit\\run.bat';
            alert(path);
                var process = new Process();
            process.Start(path);
            process.WaitForExit();
            alert('bbbb');
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
  
    <asp:Button ID="Button2" runat="server" OnClick="Button2_Click" Text="Zapisz plik" />
  
    <br />
    <br />
    <br />

<button onclick="RunME()">Run</button>






    <hr />

   

		<script>
            var txt = document.getElementById('txtToCopy');
            var btn = document.getElementById('btnCopy');

            var clipboard =
            {
                data: '',
                intercept: false,
                hook: function (evt) {
                    if (clipboard.intercept) {
                        evt.preventDefault();
                        evt.clipboardData.setData('text/plain', 'jojo ejej');
                        clipboard.intercept = false;
                        clipboard.data = '';
                    }
                }
            };

            window.addEventListener('copy', clipboard.hook);
            btn.addEventListener('click', onButtonClick);

            function onButtonClick() {
                clipboard.data = txt.value;

                if (window.clipboardData) {
                    window.clipboardData.setData('Text', clipboard.data);
                }
                else {
                    clipboard.intercept = true;
                    document.execCommand('copy');
                }
            }


            function Ojejku() {
                alert('aaaa');
                clipboard.data = 'AAAAAA';
                alert('bbbb');
                
                
                clipboard.intercept = true;
                document.execCommand('copy');
                alert('ccccc');

            }
        </script>
    </asp:Content>