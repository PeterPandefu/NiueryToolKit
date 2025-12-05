using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace NiueryToolKit.Resource.I18n
{
    public partial class I18nManager : ObservableObject
    {
        private readonly ConcurrentDictionary<string, ResourceManager> resourceManagerStorage = new ConcurrentDictionary<string, ResourceManager>();
        private readonly ConcurrentDictionary<string, ConcurrentBag<I18nResourceItem>> resourceItems = new ConcurrentDictionary<string, ConcurrentBag<I18nResourceItem>>();
        public static I18nManager Instance { get; } = new I18nManager();

        public event Action? CultureChanged;

        private CultureInfo currentUICulture = Thread.CurrentThread.CurrentUICulture;
        public CultureInfo CurrentUICulture
        {
            get => currentUICulture;
            set
            {
                currentUICulture = value;
                OnPropertyChanged(nameof(CurrentUICulture));
                CultureChanged?.Invoke();
            }

        }

        public void Add(ResourceManager resourceManager)
        {
            if (!resourceManagerStorage.ContainsKey(resourceManager.BaseName))
            {
                resourceManagerStorage[resourceManager.BaseName] = resourceManager;
                InitializeResourceItems(resourceManager);
            }
        }

        private void InitializeResourceItems(ResourceManager resourceManager)
        {
            resourceManager.GetResourceSet(CurrentUICulture, true, true)
            .OfType<DictionaryEntry>()
            .ToList()
            .ForEach(entry =>
            {
                var item = new I18nResourceItem(resourceManager, entry.Key.ToString() ?? string.Empty);

                resourceItems.AddOrUpdate(resourceManager.BaseName,
                    new ConcurrentBag<I18nResourceItem>() { item },
                    (key, existingBag) =>
                    {
                        existingBag.Add(item);
                        return existingBag;
                    });
            });
        }

        public I18nResourceItem GetItem(string baseName, string key)
        {
            if (resourceItems.TryGetValue(baseName, out ConcurrentBag<I18nResourceItem> itmes))
            {
                return itmes.Where(item => item.Key == key).FirstOrDefault();
            }
            else
            {
                return default;
            }
        }
    }

    public partial class I18nResourceItem : ObservableObject
    {
        private ResourceManager resourceManager;
        public string Key { get; private set; }
        [ObservableProperty]
        public string value = string.Empty;
        public I18nResourceItem(ResourceManager manger, string key)
        {
            resourceManager = manger;
            Key = key;
            Value = resourceManager.GetString(Key, I18nManager.Instance.CurrentUICulture) ?? Key ?? string.Empty;
            I18nManager.Instance.CultureChanged += () =>
            {
                Value = resourceManager.GetString(Key, I18nManager.Instance.CurrentUICulture) ?? Key ?? string.Empty;
            };
        }
    }
}
