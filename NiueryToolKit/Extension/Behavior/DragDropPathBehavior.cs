using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using TextBox = System.Windows.Controls.TextBox;


namespace NiueryToolKit.Extension.Behavior
{
    public static class DragDropPathBehavior
    {
        public static readonly DependencyProperty IsEnabledProperty =
            DependencyProperty.RegisterAttached("IsEnabled", typeof(bool), typeof(DragDropPathBehavior),
                new PropertyMetadata(false, OnIsEnabledChanged));
        public static bool GetIsEnabled(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsEnabledProperty);
        }
        public static void SetIsEnabled(DependencyObject obj, bool value)
        {
            obj.SetValue(IsEnabledProperty, value);
        }
        private static void OnIsEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is TextBox textBox)
            {
                if ((bool)e.NewValue)
                {
                    textBox.PreviewDragOver += TextBox_PreviewDragOver;
                    textBox.Drop += TextBox_Drop;
                }
                else
                {
                    textBox.PreviewDragOver -= TextBox_PreviewDragOver;
                    textBox.Drop -= TextBox_Drop;
                }
            }
        }

        private static void TextBox_PreviewDragOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effects = DragDropEffects.Copy;
            }
            else
            {
                e.Effects = DragDropEffects.None;
            }
            e.Handled = true;
        }

        private static void TextBox_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                if (files != null && files.Length > 0)
                {
                    TextBox textBox = sender as TextBox;
                    string fullPath = files[0];
                    bool isDirectory = Directory.Exists(fullPath);
                    string displayText = isDirectory
                        ? $"{fullPath}"
                        : $"{fullPath}";

                    textBox.Clear();
                    textBox.Text = displayText;

                    textBox.Select(textBox.Text.Length, 0);
                    textBox.ScrollToEnd();
                }
            }
            e.Handled = true;
        }
    }
}
