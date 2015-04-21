using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ExifRenamer
{
    public class Controller
    {

        public Controller()        { }

        internal string SearchDirectory()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.CheckPathExists = true;

            bool? res = ofd.ShowDialog();
            if (!res.HasValue || !res.Value)
                return "";

            return Path.GetDirectoryName(ofd.FileName);
        }

        internal void RenameFiles(string dir, string fileNameTemplate, string indexLength)
        {
            string[] files = Directory.GetFiles(dir, "*.jpg");
            Bitmap bm;
            Dictionary<string, DateTime> fileDT = new Dictionary<string, DateTime>();
            string sdt;
            foreach (string file in files)
            {
                using (bm = new Bitmap(file))
                {
                    foreach (PropertyItem item in bm.PropertyItems)
                    {
                        if (item.Id == 0x9003)
                        {
                            sdt = Encoding.ASCII.GetString(item.Value);
                            fileDT.Add(file, ConvertFromExif(sdt));
                        }
                    }
                }
            }
        }

        private DateTime ConvertFromExif(string sdt)
        {

            return DateTime.Now;
        }
    }
}
