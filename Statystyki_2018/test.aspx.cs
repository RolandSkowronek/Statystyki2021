
using DevExpress.XtraRichEdit.SpellChecker;
using OfficeOpenXml;
using Org.BouncyCastle.Crypto.Generators;
using System;

using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Web;
using System.Windows.Forms;
using static NPOI.HSSF.Util.HSSFColor;
using System.Threading;


namespace Statystyki_2018
{
    public partial class test : System.Web.UI.Page
    {
        public common cm = new common();
        private Thread trd;

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            string fileNewInfo =string.Empty;
            try
            {




              //  string download = Environment.GetEnvironmentVariable("USERPROFILE") + @"\" + "Downloads";
                Guid g = Guid.NewGuid();
               
                //   string Usertemp = System.IO.Path.GetTempPath();
               
            //    var test001 = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

              





                //   string download = Environment.GetEnvironmentVariable("USERPROFILE") + @"\" + "Downloads";

                /*  Response.Clear();
                  Response.AddHeader("content-disposition", "attachment; filename=" + myTempFile);
                  Response.AddHeader("content-type", "text/plain");

               /*   using (StreamWriter writer = new StreamWriter(Response.OutputStream))
                  {
                      writer.WriteLine(s);
                  }
                 string myFileName = g.ToString() + ".ps1";

  */

                string myFileName = g.ToString() + ".ps1";
                string s = string.Empty;
                string download = Server.MapPath("Template") + @"\" + myFileName;

                using (StreamWriter sw = new StreamWriter(download))
                {
                    s = TextBox1.Text;
                    sw.WriteLine(s);
                }


               
                FileInfo fNewFile = new FileInfo(download );
                
               
             
                

                    try
                    {
             
                        this.Response.Clear();
                        this.Response.ContentType = "application/vnd.ms-excel";
                        this.Response.AddHeader("Content-Disposition", "attachment;filename=" + fNewFile.Name);
                        this.Response.WriteFile(fNewFile.FullName);
                        this.Response.End();
                    }
                    catch (Exception ex)
                    {
                        //cm.log.Error(tenPlik + " " + ex.Message);
                    }
                 fileNewInfo = fNewFile.FullName; 
               
/*
                var startInfo = new ProcessStartInfo()
                {
                    FileName = "powershell.exe",
                    Arguments = $"-NoProfile -ExecutionPolicy ByPass -File \"{fNewFile}\""

                };
                Process.Start(startInfo);
                cm.log.Info(" Test PowerShell File Started " + fNewFile);
                Response.End();
              */
            }
            catch (Exception ex)
            {

                cm.log.Error(" Test PowerShell " + ex.Message);
            }

            try
            {

                var startInfo = new ProcessStartInfo()
                {
                    FileName = "powershell.exe",
                //    Arguments = $"-NoProfile -ExecutionPolicy ByPass -File \"{fileNewInfo}\""

                };
                Process.Start(startInfo);

                //ProcessStartInfo startInfo = new ProcessStartInfo(fileNewInfo);
                //Process.Start(startInfo);
            }
            catch (Exception)
            {

                throw;
            }

        }

    

    }
}