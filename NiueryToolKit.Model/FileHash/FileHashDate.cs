using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NiueryToolKit.Model.FileHash
{
    public partial class FileHashDate : ObservableObject
    {
        [ObservableProperty]
        public string filePath = string.Empty;

        [ObservableProperty]
        public string _MD5 = string.Empty;

        [ObservableProperty]
        public string _SHA1 = string.Empty;

        [ObservableProperty]
        public string _SHA256 = string.Empty;
    }
}
