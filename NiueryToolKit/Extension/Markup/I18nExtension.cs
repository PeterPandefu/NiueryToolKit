using NiueryToolKit.Resource.I18n;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace NiueryToolKit.Extension.Markup
{
    public class I18nExtension : MarkupExtension
    {
        public string Key { get; set; }

        public I18nExtension() { }

        public I18nExtension(string key) { Key = key; }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (Key == null)
            {
                throw new NullReferenceException("Key cannot be null at the same time.");
            }

            return new Binding("Value")
            {
                Source = I18nManager.Instance.GetItem(I18n_UI.ResourceManager.BaseName, Key),
                Mode = BindingMode.OneWay,
            }.ProvideValue(serviceProvider);
        }
    }
}
