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
            OleDbConnection xlsxConnection = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=D:\ozdilek.xlsx;Extended Properties='Excel 12.0 Xml;HDR=YES'");
            int recordCount = 0;
            try
            {
                if (xlsxConnection.State == System.Data.ConnectionState.Closed)
                {
                    xlsxConnection.Open();
                    OleDbCommand sqlCmd = new OleDbCommand("SELECT * FROM [6$]", xlsxConnection);
                    OleDbDataReader xlsxRead = sqlCmd.ExecuteReader();
                    while (xlsxRead.Read())
                    {
                        string barkod = xlsxRead["BARCODE"].ToString();
                        //string name = xlsxRead["NAME"].ToString();
                        // string smalImage = xlsxRead["Small Image"].ToString();
                        // string largeImage = xlsxRead["images3cdn"].ToString();
                        string MALINCINSI = xlsxRead["MALINCINSI"].ToString();

                        try
                        {
                            //SaveImage(largeImage, barkod, ImageFormat.Png);
                            SaveImageOzdilek(barkod, ImageFormat.Jpeg);
                            recordCount++;
                            Console.Title = "indirilen image Sayısı:" + recordCount;
                            //  Console.Clear();
                        }
                        catch (Exception hata)
                        {
                            Logger.LogMessage(hata.Message.ToString(), barkod);
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

                Logger.LogMessage(hata.Message.ToString(), null);
            }

        }

        private static void SaveImageOzdilek(string barkod, ImageFormat format)
        {
            using (WebClient client = new WebClient())
            {
                Stream stream = client.OpenRead(@"https://img-ozdilek.mncdn.com/images/catalog/646x1000/" + barkod + ".jpg");
                Bitmap bitmap = new Bitmap(stream);
                if (bitmap != null)
                {
                    bitmap.Save(@"d:\ozdilekImages1\" + barkod + ".jpg", format);
                    Console.WriteLine("Kaydedilen resmin barkodu :" + barkod);
                }
                stream.Flush();
                stream.Close();

            }
            #region  save images Migros
            //    private static void SaveImage(string imageUrl, string barkod, ImageFormat format)
            //{
            //       using (WebClient client = new WebClient())
            //        {
            //            Stream stream = client.OpenRead(imageUrl);
            //            Bitmap bitmap = new Bitmap(stream);
            //            if (bitmap != null)
            //            {
            //                bitmap.Save(@"d:\images3cdn\" + barkod+".jpg",format);
            //           Console.WriteLine("Kaydedilen resmin barkodu :" + barkod);
            //            }
            //            stream.Flush();
            //            stream.Close();
            //            client.Dispose();
            //        }
            //}
            #endregion

        }
    }
}
