using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace imagesDownload
{
    class Program
    {
        static void Main(string[] args)
        {
            OleDbConnection xlsxConnection = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=D:\migros.xlsx;Extended Properties='Excel 12.0 Xml;HDR=YES'");
            int recordCount = 0;
            try
            {
                if (xlsxConnection.State == System.Data.ConnectionState.Closed)
                {
                    xlsxConnection.Open();
                    OleDbCommand sqlCmd = new OleDbCommand("SELECT * FROM [Sayfa1$]", xlsxConnection);
                    OleDbDataReader xlsxRead = sqlCmd.ExecuteReader();
                    while (xlsxRead.Read())
                    {
                        string barkod = xlsxRead["BARCODE"].ToString();
                        //string name = xlsxRead["NAME"].ToString();
                       // string smalImage = xlsxRead["Small Image"].ToString();
                        string largeImage = xlsxRead["images3cdn"].ToString();

                        try
                        {
                            SaveImage(largeImage, barkod, ImageFormat.Png);
                            recordCount++;
                            Console.Title ="indirilen image Sayısı:"+recordCount;
                          //  Console.Clear();
                        }
                        catch (Exception hata)
                        {
                            Logger.LogMessage(hata.Message.ToString(),largeImage);                          
                        }
                       
                    }
                    xlsxConnection.Close();
                    Console.Clear();
                    Console.WriteLine("-----------------------------------------");
                    Console.WriteLine("İşlem Tamamlandı....");
                    Console.WriteLine("Kaydedilen resim sayısı : {0}", recordCount);
                    Console.WriteLine("-----------------------------------------");
                    Console.ReadLine();
                }
            }
            catch (Exception hata)
            {

                Logger.LogMessage(hata.Message.ToString(),null);
            }
           
        }

        private static void SaveImage(string imageUrl, string barkod, ImageFormat format)
        {
               using (WebClient client = new WebClient())
                {
                    Stream stream = client.OpenRead(imageUrl);
                    Bitmap bitmap = new Bitmap(stream);
                    if (bitmap != null)
                    {
                        bitmap.Save(@"d:\images3cdn\" + barkod+".jpg",format);
                   Console.WriteLine("Kaydedilen resmin barkodu :" + barkod);
                    }
                    stream.Flush();
                    stream.Close();
                    client.Dispose();
                }
            }
        
    }
}
