using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Win32;
using NiueryToolKit.Base.Implementation;
using NiueryToolKit.Base.Interface;
using NiueryToolKit.Resource.I18n;
using NiueryToolKit.ViewModel.FileHash;
using NiueryToolKit.ViewModel.MainWindow;
using System;
using System.Configuration;
using System.Data;
using System.Reflection;
using System.Windows;

namespace NiueryToolKit
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Global exception capture
            AppDomain.CurrentDomain.UnhandledException += (sender, args) =>
            {
                Exception ex = args.ExceptionObject as Exception;
                MessageBox.Show($"Unhandled exception occurred: {ex?.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            };

            DispatcherUnhandledException += (sender, args) =>
            {
                MessageBox.Show($"Unhandled UI exception occurred: {args.Exception.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                args.Handled = true;
            };

            // Task exception capture
            System.Threading.Tasks.TaskScheduler.UnobservedTaskException += (sender, args) =>
            {
                MessageBox.Show($"Unhandled task exception occurred: {args.Exception.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                args.SetObserved();
            };

            I18nManager.Instance.CurrentUICulture = Thread.CurrentThread.CurrentUICulture;
            I18nManager.Instance.Add(I18n_UI.ResourceManager);

            // Register

            HostApplicationBuilder builder = Host.CreateApplicationBuilder();
            Registcontroller(builder);
            RegistView(builder);
            RegistViewModel(builder);
            IHost host = builder.Build();
            FillController(host.Services);
            var mainWindow = new MainWindow(host.Services);
            mainWindow.Closed += MainWindow_Closed;
            mainWindow.ShowDialog();
        }

        private void MainWindow_Closed(object? sender, EventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Registcontroller(HostApplicationBuilder builder)
        {
            builder.Services.AddSingleton<IMVVMController, FileHashController>();
            builder.Services.AddSingleton<FileHashController>();

            builder.Services.AddSingleton<IViewOperator, MainWindow>();
        }

        private void RegistViewModel(HostApplicationBuilder builder)
        {
            Assembly assembly = Assembly.Load("NiueryToolKit.ViewModel");

            var types = assembly.GetTypes()
                                .Where(t => t.Name.EndsWith("ViewModel", StringComparison.OrdinalIgnoreCase) &&
                                        !t.IsAbstract &&
                                        t.IsClass);
            foreach (var type in types)
            {
                builder.Services.AddSingleton(type);
            }
        }

        private void RegistView(HostApplicationBuilder builder)
        {
            Assembly assembly = Assembly.Load("NiueryToolKit");

            var types = assembly.GetTypes().Where(t => t.IsSubclassOf(typeof(Window)) && !t.IsAbstract);

            foreach (var type in types)
            {
                builder.Services.AddTransient(type);
            }
        }

        private void FillController(IServiceProvider service)
        {
            // Initialize mapping
            Dictionary<Type, Tuple<Type, Type>> mapper = new Dictionary<Type, Tuple<Type, Type>>();

            mapper.Add(typeof(FileHashController), Tuple.Create(typeof(View.FileHash.FileHash), typeof(FileHashViewModel)));

            foreach (var item in mapper)
            {
                var controller = service.GetService(item.Key) as IMVVMController;
                controller.ViewType = item.Value.Item1;
                controller.ViewModelType = item.Value.Item2;
            }
        }
    }
}
