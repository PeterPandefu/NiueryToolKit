using Microsoft.Extensions.DependencyInjection;
using Microsoft.Win32;
using NiueryToolKit.Base.Interface;
using NiueryToolKit.Resource.I18n;
using NiueryToolKit.ViewModel.MainWindow;
using System.Globalization;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.WindowsAPICodePack.Dialogs;
using Microsoft.Extensions.Hosting;

namespace NiueryToolKit
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IViewOperator
    {
        private readonly IServiceProvider _serviceProvider;
        public MainWindow(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            DataContext = serviceProvider.GetService(typeof(MainViewModel));

            InitializeComponent();
        }

        public void Message(string message, string tiele)
        {
            MessageBox.Show(message, tiele);
        }

        public string[] OpenFile()
        {
            string filter = "所有文件|*.*";
            string defaultExt = "";
            string title = "选择文件";
            bool multiselect = false;

            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = filter,
                DefaultExt = defaultExt,
                Title = title,
                Multiselect = multiselect
            };
            bool? result = openFileDialog.ShowDialog();
            return result == true ? openFileDialog.FileNames : Array.Empty<string>();
        }

        public string[] OpenFolder()
        {
            var dlg = new CommonOpenFileDialog();
            dlg.IsFolderPicker = true;
            dlg.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            if (dlg.ShowDialog() == CommonFileDialogResult.Ok)
            {
                return new string[] { dlg.FileName };
            }
            return Array.Empty<string>();
        }

        public void ShowWindow(IMVVMController controller)
        {
            Window window = controller.View as Window;
            window.DataContext = controller.ViewModel;
            window.ShowDialog();
        }

        public void SwitchLanguage()
        {
            if (Thread.CurrentThread.CurrentUICulture.Name.ToLower() == "en-us")
            {
                I18nManager.Instance.CurrentUICulture = Thread.CurrentThread.CurrentCulture = Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("zh-cn");
            }
            else if (Thread.CurrentThread.CurrentUICulture.Name.ToLower() == "zh-cn")
            {
                I18nManager.Instance.CurrentUICulture = Thread.CurrentThread.CurrentCulture = Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("en-us");
            }
        }
    }
}