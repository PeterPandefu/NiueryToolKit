using NiueryToolKit.Base.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NiueryToolKit.Base.Implementation
{
    public class FileHashController : IMVVMController
    {

        public Type ViewType { get; set; }
        public Type ViewModelType { get; set; }

        #region ctor

        private readonly IServiceProvider _serviceProvider;

        public FileHashController(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        #endregion

        public object? View
        {
            get
            {
                return _serviceProvider.GetService(ViewType);
            }
        }

        public object? ViewModel
        {
            get
            {
                return _serviceProvider.GetService(ViewModelType);
            }
        }
    }
}
