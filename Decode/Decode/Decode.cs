using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using Bytescout.BarCodeReader;
using System.Threading.Tasks;
using System.Drawing;

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
                    Image myImage = Image.FromFile(path);
                    Bitmap source = new Bitmap(myImage);

                    int imgw = 0;
                    int imgh = 0;

                    reader.BarcodeTypesToFind.DataMatrix = true;

                    string cash = null;

                    for (int i = 250; i < source.Width; i = i + 125)
                    {
                        for (int j = 250; j < source.Height; j = j + 125)
                        {
                            imgw = i; imgh = j;

                            Bitmap bmp = source.Clone(new System.Drawing.Rectangle(source.Width - imgw, source.Height - imgh, 250, 250), source.PixelFormat);

                            FoundBarcode[] barcodes = reader.ReadFrom(bmp);

                            foreach (FoundBarcode code in barcodes)
                            {
                                cash = code.Value;
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

                                return cash;
                            }
                        }
                    }
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
