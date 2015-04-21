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
    public class ExifRenamerController
    {
        public ExifRenamerController() { }

        internal string SearchDirectory()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.CheckPathExists = true;

            bool? res = ofd.ShowDialog();
            if (!res.HasValue || !res.Value)
                return "";

            return Path.GetDirectoryName(ofd.FileName);
        }

        internal void RenameFiles(string dir, string fileExtension, string fileNameTemplate, string indexLength)
        {
            // TODO: threading and progress dialog
            Dictionary<string, DateTime> fileDT = ReadExifData(dir, fileExtension);
            RenameFiles(fileDT, fileNameTemplate, indexLength);
        }

        private Dictionary<string, DateTime> ReadExifData(string dir, string fileExtension)
        {
            string[] files = Directory.GetFiles(dir, "*." + fileExtension);
            Bitmap bm;
            Dictionary<string, DateTime> fileDT = new Dictionary<string, DateTime>();
            string sdt;

            // read exif data
            foreach (string file in files)
            {
                using (bm = new Bitmap(file))
                {
                    foreach (PropertyItem item in bm.PropertyItems)
                    {
                        if (item.Id == 0x9003)
                        {
                            sdt = Encoding.ASCII.GetString(item.Value);
                            if (string.IsNullOrEmpty(sdt))
                                break;
                            fileDT.Add(file, ConvertFromExif(sdt));
                        }
                    }
                }
            }

            return fileDT;
        }

        private void RenameFiles(Dictionary<string, DateTime> fileDT, string fileNameTemplate, string sindexLength)
        {
            if (fileDT.Count == 0)
                return;

            int index, indexLenght;
            string curFileName, origPath, tmpPath, newFileName, newFilePath, ext, indexFormat = "";

            int.TryParse(sindexLength, out indexLenght);
            for (int i = 0; i < indexLenght; i++)
                indexFormat += "0";

            origPath = Path.GetDirectoryName(fileDT.First().Key);
            tmpPath = origPath + Path.DirectorySeparatorChar + DateTime.Now.Ticks.ToString();
            Directory.CreateDirectory(tmpPath);

            //move files to a temporary directory
            foreach (var dt in fileDT)
            {
                curFileName = Path.GetFileName(dt.Key);
                File.Move(dt.Key, tmpPath + Path.DirectorySeparatorChar + curFileName);
            }

            IEnumerable<IGrouping<DateTime, KeyValuePair<string, DateTime>>> groupByDate = fileDT.GroupBy(x => x.Value.Date);
            foreach (IGrouping<DateTime, KeyValuePair<string, DateTime>> grp in groupByDate.OrderBy(x => x.Key))
            {
                index = 1;
                foreach (KeyValuePair<string, DateTime> dt in grp.OrderBy(x => x.Value))
                {
                    curFileName = Path.GetFileName(dt.Key);
                    origPath = Path.GetDirectoryName(dt.Key);
                    ext = Path.GetExtension(dt.Key);
                    newFileName = dt.Value.ToString(fileNameTemplate);

                    newFileName += "_{0}" + ext;
                    newFilePath = origPath + Path.DirectorySeparatorChar + string.Format(newFileName, index.ToString(indexFormat));
                    while (File.Exists(newFilePath))
                    {
                        index++;
                        newFilePath = origPath + Path.DirectorySeparatorChar + string.Format(newFileName, index.ToString(indexFormat));
                    }

                    File.Move(tmpPath + Path.DirectorySeparatorChar + curFileName, newFilePath);

                    index++;
                }
            }

            Directory.Delete(tmpPath);
        }

        private DateTime ConvertFromExif(string sdt)
        {
            //2007:11:25 14:54:49
            string date = sdt.Split(' ')[0];
            string time = sdt.Split(' ')[1];

            int year, month, day, hour, minute, second;
            int.TryParse(date.Split(':')[0], out year);
            int.TryParse(date.Split(':')[1], out month);
            int.TryParse(date.Split(':')[2], out day);

            int.TryParse(time.Split(':')[0], out hour);
            int.TryParse(time.Split(':')[1], out minute);
            int.TryParse(time.Split(':')[2], out second);

            return new DateTime(year, month, day, hour, minute, second);
        }
    }
}
