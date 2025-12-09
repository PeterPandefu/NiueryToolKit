using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NiueryToolKit.Base.Interface.ImageConverter
{
    public interface IImageConverter
    {
        public void ConvertImage(string inputPath, string outputPath, string outputFormat);
    }
}
