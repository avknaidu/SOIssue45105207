using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace App1
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            this.Loaded += MainPage_Loaded;
        }

        private async void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            string jsonData = await RetrieveList("GlobalList.txt");
            if (!string.IsNullOrEmpty(jsonData))
            {
                App.GlobalList = JsonConvert.DeserializeObject<ObservableCollection<string>>(jsonData);
            }
            if (App.GlobalList.Count == 0)
            {
                for (int i = 1; i < 30; i++)
                {
                    App.GlobalList.Add("Item " + i.ToString());
                }
            }
            MyList.ItemsSource = App.GlobalList;
        }

        public async void SaveList(string FileName, string StringValue)
        {
            StorageFolder storageFolder = ApplicationData.Current.LocalFolder;

            StorageFile sampleFile = (File.Exists(Path.Combine(storageFolder.Path,FileName))) ?
                await storageFolder.GetFileAsync(FileName)
               :await storageFolder.CreateFileAsync(FileName, CreationCollisionOption.ReplaceExisting);

            await FileIO.WriteTextAsync(sampleFile, StringValue);
        }

        public async Task<string> RetrieveList(string FileName)
        {
            StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
            StorageFile sampleFile = (File.Exists(Path.Combine(storageFolder.Path,FileName))) ?
                await storageFolder.GetFileAsync(FileName)
               : await storageFolder.CreateFileAsync(FileName, CreationCollisionOption.ReplaceExisting);
            return await FileIO.ReadTextAsync(sampleFile);
        }

        private void Button_Tapped(object sender, TappedRoutedEventArgs e)
        {
            string jsonData = JsonConvert.SerializeObject(App.GlobalList);
            SaveList("GlobalList.txt", jsonData);
        }
    }
}
