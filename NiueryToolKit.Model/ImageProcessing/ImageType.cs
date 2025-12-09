using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NiueryToolKit.Model.ImageProcessing
{
    public partial class ImageType : ObservableObject
    {
        [ObservableProperty]
        public bool isChecked;

        [ObservableProperty]
        public string typeName;
    }
}
