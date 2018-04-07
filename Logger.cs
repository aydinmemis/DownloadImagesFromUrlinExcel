using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace imagesDownload
{
   public class Logger
    {
        private static string logDirectory = String.Empty;

        ///<summary>
        ///kurulu dizine log klasörü açıp günlük loglama yapar
        ///Ayruca mesaj paramteresini göndererk istenilen noktaya loglama yapmamızı sağlar
        /// </summary>
        public static void LogMessage(string message, string url)
        {
            //günün tarihini aldık
            DateTime dt = DateTime.Now;
            //logDirectory kalsörünü oluşturuyoruz
            if (logDirectory==String.Empty)
            {
                SetLogDirectory();
                if (!Directory.Exists(logDirectory + "Log\\"))
                {
                    Directory.CreateDirectory(logDirectory + "Log\\");
                }

            }
            //log doyasının adını filePAth değişkenine  yazdırıyoruz 
            String filePath = logDirectory + "Log\\" + dt.ToString("yyyy-MM-dd") + ".log";
            // log  dosyası yoksa oluşturuyoruz
            if (!File.Exists(filePath))
            {
                FileStream fs = File.Create(filePath);
                fs.Close();
            }
            try
            {
                StreamWriter sw = File.AppendText(filePath);
                sw.WriteLine(dt.ToString("hh:mm:ss") + "   |   " + url + "   |   "+ message );
                sw.Flush();
                sw.Close();
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message.ToString());
            }

        }
        /// <summary>
        /// logDirectory değişkenine dosya adını işlenecek dosya adını yazar
        ///
        /// </summary>
        private static void SetLogDirectory()
        {
            FileInfo fileInfo = new FileInfo(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);
            logDirectory = fileInfo.Directory.FullName + "\\";
        }
    }
}
