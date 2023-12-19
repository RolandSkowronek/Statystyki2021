
using Org.BouncyCastle.Crypto.Generators;
using System;

using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace Statystyki_2018
{
    public partial class test : System.Web.UI.Page
    {

      

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void Button1_Click(object sender, EventArgs e)
        {

            Guid g = Guid.NewGuid();
            string myTempFile = Path.Combine(Path.GetTempPath(), g.ToString() +".ps1");
            using (StreamWriter sw = new StreamWriter(myTempFile))
            {
                
                sw.WriteLine(" $wshell = New-Object -ComObject Wscript.Shell");
                sw.WriteLine(" $wshell.Run(\"notepad\")" );
                sw.WriteLine(" Start-Sleep -m 1000" ) ;
                sw.WriteLine(" $wshell.SendKeys(\"^n\")");
                sw.WriteLine(" Start-Sleep -m 1000");
                sw.WriteLine(" $wshell.SendKeys(\"^v\")");
            }

            var startInfo = new ProcessStartInfo()
            {
                FileName = "powershell.exe",
                Arguments = $"-NoProfile -ExecutionPolicy ByPass -File \"{myTempFile}\""

        };
            Process.Start(startInfo);
            

        }



    }
}