using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NiueryToolKit.Base.Interface
{
    public interface IMVVMController
    {
        object? View { get; }
        object? ViewModel { get; }
        public Type ViewType { get; set; }
        public Type ViewModelType { get; set; }
    }
}
