using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NiueryToolKit.Model.ImageProcessing
{
    public partial class FileData : ObservableObject
    {
        [ObservableProperty]
        public string path = string.Empty;

        [ObservableProperty]
        public int number = 0;
    }
}
