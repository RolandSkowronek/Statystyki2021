
using DevExpress.XtraRichEdit.SpellChecker;
using Org.BouncyCastle.Crypto.Generators;
using System;

using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Web;
using System.Windows.Forms;
using static NPOI.HSSF.Util.HSSFColor;

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




                string download = Environment.GetEnvironmentVariable("USERPROFILE") + @"\" + "Downloads";
                Guid g = Guid.NewGuid();
                string myFileName = g.ToString() + ".ps1";
                //   string Usertemp = System.IO.Path.GetTempPath();
                string myTempFile =  Path.Combine(download, myFileName);
            //    var test001 = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

                string s = string.Empty;


                  using (StreamWriter sw = new StreamWriter(myTempFile))
                  {
                    s = TextBox1.Text;
                    sw.WriteLine(s);
                }
                
               
           
 

                //   string download = Environment.GetEnvironmentVariable("USERPROFILE") + @"\" + "Downloads";

                Response.Clear();
                Response.AddHeader("content-disposition", "attachment; filename=" + myTempFile);
                Response.AddHeader("content-type", "text/plain");

             /*   using (StreamWriter writer = new StreamWriter(Response.OutputStream))
                {
                    writer.WriteLine(s);
                }
               

*/

              

                var startInfo = new ProcessStartInfo()
                {
                    FileName = "powershell.exe",
                    Arguments = $"-NoProfile -ExecutionPolicy ByPass -File \"{myTempFile}\""

                };
                Process.Start(startInfo);
                cm.log.Info(" Test PowerShell File Started " + myTempFile);
                Response.End();
              
            }
            catch (Exception ex)
            {

                cm.log.Error(" Test PowerShell " + ex.Message);
            }
          
            

        }



    }
}