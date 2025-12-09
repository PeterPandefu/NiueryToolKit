using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using NiueryToolKit.Base.Interface;
using NiueryToolKit.Model.ImageProcessing;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageConverter = NiueryToolKit.Base.Implementation.ImageConverter.ImageConverter;

namespace NiueryToolKit.ViewModel.ImageProcessing
{
    public partial class ImageProcessingViewModel : ObservableObject
    {
        [ObservableProperty]
        private ObservableCollection<ImageType> imageTypes = new ObservableCollection<ImageType>();

        [ObservableProperty]
        private ObservableCollection<FileData> imagePaths = new ObservableCollection<FileData>();

        [ObservableProperty]
        private FileData selectedFile;

        [ObservableProperty]
        private string outputPath = string.Empty;

        private string path = string.Empty;

        public string Path
        {
            get => path;
            set
            {
                if (path != value)
                {
                    path = value;
                    OnPropertyChanged(nameof(Path));
                    RefreshImageDates(Path);
                }
            }
        }
        #region ctor

        private IViewOperator MainView => _serviceProvider.GetService<IViewOperator>();

        private readonly IServiceProvider _serviceProvider;
        public ImageProcessingViewModel(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;

            InitImageTypes();
        }


        #endregion

        [RelayCommand]
        public void OpenFile()
        {
            ImagePaths.Clear();

            var files = MainView.OpenFile();

            Path = files.Count() > 0 ? files.First() : string.Empty;
        }

        [RelayCommand]
        public void OpenFolder()
        {
            ImagePaths.Clear();

            var folders = MainView.OpenFolder();
            Path = folders.Count() > 0 ? folders.First() : string.Empty;
        }

        [RelayCommand]
        public void Open()
        {
            try
            {
                if (File.Exists(OutputPath))
                {
                    ProcessStartInfo startInfo = new ProcessStartInfo
                    {
                        FileName = OutputPath,
                        UseShellExecute = true
                    };
                    Process.Start(startInfo);
                }
                else
                {
                    MainView.Message($"The file does not exist.", "An exception occurred during image processing.");
                }
            }
            catch (Exception ex)
            {
                MainView.Message($"The file failed to open.{ex.StackTrace}", "An exception occurred during image processing.");
            }
        }


        public RelayCommand<string> ProcessingCommand => new RelayCommand<string>(Processing);

        public void Processing(string command)
        {
            var view = _serviceProvider.GetService<IViewOperator>();

            if (SelectedFile == null)
            {
                MainView.Message($"No file selected.", "An exception occurred during image processing.");
                return;
            }
            if (!IsImage(SelectedFile.Path))
            {
                MainView.Message($"Selected file is not in image format.", "An exception occurred during image processing.");
                return;
            }

            switch (command)
            {
                case "ImageConverter":
                    Converter();
                    break;
                case "ImageMerging":
                    Merger();
                    break;
                case "ImageCompression":
                    Compress();
                    break;
                case "ImageResizing":
                    Resize();
                    break;
                case "Image rotation":
                    Rotate();
                    break;
                case "ImageFlipping":
                    Flip();
                    break;

                default:
                    break;
            }
        }

        private void Flip()
        {
            throw new NotImplementedException();
        }

        private void Rotate()
        {
            throw new NotImplementedException();
        }

        private void Resize()
        {
            throw new NotImplementedException();
        }

        private void Compress()
        {
            throw new NotImplementedException();
        }

        private void Merger()
        {
            throw new NotImplementedException();
        }

        public void Converter()
        {
            bool isImage = false;
            using (Image img = Image.FromFile(SelectedFile.Path))
            {
                isImage = img.Width > 0 && img.Height > 0;
            }

            if (isImage)
            {
                if (ImageTypes.Any(t => t.IsChecked))
                {
                    var typeName = ImageTypes.First(t => t.IsChecked).TypeName;


                    ImageConverter converter = new ImageConverter();
                    var outputPath = GenerateNewFilePath(SelectedFile.Path, $"_To{typeName}", $".{typeName.ToLower()}");
                    converter.ConvertImage(SelectedFile.Path, outputPath, typeName);
                    OutputPath = outputPath;
                }
                else
                {

                    MainView.Message($"Please select the target format first.", "An exception occurred during image processing.");
                }
            }
            else
            {
                MainView.Message($"The file is not an image.Path:{SelectedFile.Path}", "An exception occurred during image processing.");
            }

        }

        private void InitImageTypes()
        {
            foreach (var item in ImageConverter.SupportedFormats)
            {
                ImageTypes.Add(new ImageType() { IsChecked = false, TypeName = item });
            }
        }

        private void RefreshImageDates(string path)
        {
            if (File.Exists(path))
            {
                ImagePaths.Add(new FileData() { Number = ImagePaths.Count + 1, Path = path });
                SelectedFile = ImagePaths.First();
            }
            else if (Directory.Exists(path))
            {
                var files = Directory.GetFiles(path, "*", SearchOption.AllDirectories);
                foreach (string file in files)
                {
                    if (!File.Exists(file)) continue;

                    ImagePaths.Add(new FileData() { Number = ImagePaths.Count + 1, Path = file });
                }
                SelectedFile = ImagePaths.First();
            }
            else
            {
                ImagePaths.Clear();
            }
        }

        private string GenerateNewFilePath(string originalPath, string suffix, string newExtension)
        {
            string directory = System.IO.Path.GetDirectoryName(originalPath);

            string fileNameWithoutExt = System.IO.Path.GetFileNameWithoutExtension(originalPath);

            string extension = string.IsNullOrEmpty(newExtension) ? System.IO.Path.GetExtension(originalPath) : newExtension;

            string newFileName = fileNameWithoutExt + suffix + extension;

            string newPath = System.IO.Path.Combine(directory, newFileName);

            return newPath;
        }

        private bool IsImage(string filePath)
        {
            if (!File.Exists(filePath))
                return false;
            try
            {
                using (Image image = Image.FromFile(filePath))
                {
                    // 若能成功加载，说明是有效图片
                    return image != null;
                }
            }
            catch (OutOfMemoryException)
            {
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
