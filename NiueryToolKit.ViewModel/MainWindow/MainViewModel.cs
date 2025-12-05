using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using NiueryToolKit.Base.Implementation;
using NiueryToolKit.Base.Interface;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace NiueryToolKit.ViewModel.MainWindow
{
    public partial class MainViewModel : ObservableObject
    {
        #region ctor

        private readonly IServiceProvider _serviceProvider;
        public MainViewModel(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        #endregion

        #region command

        public RelayCommand<string> ShowWindowCommand => new RelayCommand<string>(ShowWindow);
        public RelayCommand SwitchLanguageCommand => new RelayCommand(SwitchLanguage);

        #endregion

        #region command method

        private void ShowWindow(string name)
        {
            var view = _serviceProvider.GetService<IViewOperator>();
            view.ShowWindow((IMVVMController)_serviceProvider.GetService(typeof(FileHashController)));
        }

        private void SwitchLanguage()
        {
            var view = _serviceProvider.GetService<IViewOperator>();
            view.SwitchLanguage();
        }

        #endregion

        #region private method

        public Type GetTypeByName(string name)
        {
            return name switch
            {
                "FileHASH" => typeof(FileHashController)
            };
        }
        #endregion
    }
}
