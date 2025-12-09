using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using NiueryToolKit.Base.Interface;
using NiueryToolKit.Model.FileHash;
using System.Collections.ObjectModel;
using System.IO;
using System.Security.Cryptography;


namespace NiueryToolKit.ViewModel.FileHash
{
    public partial class FileHashViewModel : ObservableObject
    {
        public string path = string.Empty;

        public string Path
        {
            get => path;
            set
            {
                if (path != value)
                {
                    path = value;
                    OnPropertyChanged(nameof(Path));
                    RefreshHashDates(Path);
                }
            }
        }

        [ObservableProperty]
        public ObservableCollection<FileHashDate> hashDates = new ObservableCollection<FileHashDate>();


        #region ctor

        private IViewOperator MainView => _serviceProvider.GetService<IViewOperator>();
        private readonly IServiceProvider _serviceProvider;
        public FileHashViewModel(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        #endregion

        [RelayCommand]
        public void OpenFile()
        {
            HashDates.Clear();
            var files = MainView?.OpenFile();

            Path = files.Count() > 0 ? files.First() : string.Empty;
        }

        [RelayCommand]
        public void OpenFolder()
        {
            HashDates.Clear();
            var folders = MainView?.OpenFolder();
            Path = folders.Count() > 0 ? folders.First() : string.Empty;
        }


        private void RefreshHashDates(string path)
        {
            HashDates.Clear();
            if (File.Exists(path))
            {
                CreateFileHashDate(path);
            }
            else if (Directory.Exists(path))
            {
                var files = Directory.GetFiles(path, "*", SearchOption.AllDirectories);
                foreach (string file in files)
                {
                    if (!File.Exists(file)) continue;

                    CreateFileHashDate(file);
                }
            }

        }

        private void CreateFileHashDate(string filePath)
        {
            if (!File.Exists(filePath)) return;

            string md5 = string.Empty;
            string sha1 = string.Empty;
            string sha256 = string.Empty;

            using (var stream = File.OpenRead(filePath))
            {
                using (var md5Alg = MD5.Create())
                {
                    md5 = BitConverter.ToString(md5Alg.ComputeHash(stream)).Replace("-", "");
                    stream.Position = 0;
                }
                using (var sha1Alg = SHA1.Create())
                {
                    sha1 = BitConverter.ToString(sha1Alg.ComputeHash(stream)).Replace("-", "");
                    stream.Position = 0;
                }
                using (var sha256Alg = SHA256.Create())
                {
                    sha256 = BitConverter.ToString(sha256Alg.ComputeHash(stream)).Replace("-", "");
                }
            }

            HashDates.Add(new FileHashDate
            {
                FilePath = filePath,
                MD5 = md5,
                SHA1 = sha1,
                SHA256 = sha256
            });
        }
    }
}
