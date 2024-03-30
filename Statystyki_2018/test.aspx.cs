
using DevExpress.XtraRichEdit.SpellChecker;
using Org.BouncyCastle.Crypto.Generators;
using System;

using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Web;
using System.Windows.Forms;

namespace Statystyki_2018
{
    public partial class test : System.Web.UI.Page
    {
        public common cm = new common();


        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
          
            try
            {


             
              
     
                Guid g = Guid.NewGuid();
                string myFileName = g.ToString() + ".ps1";
                string Usertemp = System.IO.Path.GetTempPath();
                  string myTempFile = Path.Combine(Usertemp, myFileName);
                string s = string.Empty;
               /*   using (StreamWriter sw = new StreamWriter(myFileName))
                  {

                      sw.WriteLine(" $wshell = New-Object -ComObject Wscript.Shell");
                      sw.WriteLine(" $wshell.Run(\"notepad\")");
                      sw.WriteLine(" Start-Sleep -m 1000");
                      sw.WriteLine(" $wshell.SendKeys(\"^n\")");
                      sw.WriteLine(" Start-Sleep -m 1000");
                      sw.WriteLine(" $wshell.SendKeys(\"^v\")");
                      s=sw.ToString();
                  }
                
                */
             
                s = " $wshell = New-Object -ComObject Wscript.Shell";
                s = s + " $wshell.Run(\"notepad\")" +Environment.NewLine;
                s = s + " Start-Sleep -m 1000" +Environment.NewLine;
                s = s + " $wshell.SendKeys(\"^n\")" + Environment.NewLine; ;
                s = s + " Start-Sleep -m 1000" + Environment.NewLine; ;
                s = s + " $wshell.SendKeys(\"^v\")" + Environment.NewLine;
                

            //    string download = Environment.GetEnvironmentVariable("USERPROFILE") + @"\" + "Downloads";

                Response.Clear();
                Response.AddHeader("content-disposition", "attachment; filename=" + myTempFile);
                Response.AddHeader("content-type", "text/plain");

                using (StreamWriter writer = new StreamWriter(Response.OutputStream))
                {
                    writer.WriteLine(s);
                }
               



                string FireFile = Usertemp + "\\"+ myFileName;

                var startInfo = new ProcessStartInfo()
                {
                    FileName = "powershell.exe",
                    Arguments = $"-NoProfile -ExecutionPolicy ByPass -File \"{FireFile}\""

                };
                Process.Start(startInfo);
                cm.log.Error(" Test PowerShell File Started " + FireFile);
                Response.End();
              
            }
            catch (Exception ex)
            {

                cm.log.Error(" Test PowerShell " + ex.Message);
            }
          
            

        }



    }
}