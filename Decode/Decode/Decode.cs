using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Threading;
using Bytescout.BarCodeReader;

namespace Decode
{
    public class Decode
    {
        public String TIFF(string path)
        {
            try
            {
                using (Reader reader = new Reader())
                {
                    reader.BarcodeTypesToFind.DataMatrix = true;
                    Image myImage = Image.FromFile(path);
                    Bitmap source = new Bitmap(myImage);
                    myImage.Dispose();

                    int imgw = 0;
                    int imgh = 0;

                    string cash = null;

                    for (int i = 250; i < source.Width; i = i + 20)
                    {
                        for (int j = 300; j < source.Height; j = j + 20)
                        {
                            imgw = i; imgh = j;

                            Bitmap bmp = source.Clone(new System.Drawing.Rectangle(source.Width - imgw, source.Height - imgh, 250, 300), source.PixelFormat);

                            FoundBarcode[] barcodes = reader.ReadFrom(bmp);

                            foreach (FoundBarcode code in barcodes)
                            {
                                if (code.Value.Split('_').Length == 6)
                                {
                                    cash = code.Value;
                                }
                            }

                            if (cash != null)
                            {
                                var fromEncodind = Encoding.GetEncoding("ISO-8859-1");//из какой кодировки
                                var bytes = fromEncodind.GetBytes(cash);
                                var toEncoding = Encoding.GetEncoding(1251);//в какую кодировку

                                cash = toEncoding.GetString(bytes);

                                while (cash.IndexOf("<FNC1>") != -1)
                                {
                                    cash = cash.Replace("<FNC1>", "и");
                                }

                                cash = cash.Remove(cash.IndexOf('>'), cash.IndexOf('<') - cash.IndexOf('>') + 1);

                                String[] ExistingCharaterEnglish = new String[] { "A", "a", "B", "C", "c", "E", "e", "H", "K", "M", "O", "o", "P", "p", "T" };

                                String[] ExistingCharaterRussia = new String[] { "А", "а", "В", "С", "с", "Е", "е", "Н", "К", "М", "О", "о", "Р", "р", "Т" };

                                String[] Temp = cash.Split('_');

                                String ReplaceMark = "";

                                for (int k = 0; k < ExistingCharaterRussia.Length; k++)
                                {
                                    ReplaceMark = Temp[2].Replace(ExistingCharaterRussia[k], ExistingCharaterEnglish[k]);
                                }

                                String[] Splitter = Temp[1].Split('и');

                                while (Splitter[0][0] == '0')
                                {
                                    Splitter[0] = Splitter[0].Remove(0, 1);
                                }

                                if (Splitter.Length != 1)
                                {
                                    Temp[1] = Splitter[0] + "и" + Splitter[1];
                                }
                                else
                                {
                                    Temp[1] = Splitter[0];
                                }

                                cash = Temp[0] + "_" + Temp[1] + "_" + ReplaceMark + "_" + Temp[3] + "_" + Temp[4] + "_" + Temp[5];

                                source.Dispose();
                                bmp.Dispose();

                                return cash;
                            }
                            bmp.Dispose();
                        }
                    }
                    source.Dispose();
                    cash = "Не правильно";
                    return cash;
                }

            }
            catch (Exception e)
            {
                SystemArgs.PrintLog(e.ToString());
                return e.Message;
            }
        }
    }
}
