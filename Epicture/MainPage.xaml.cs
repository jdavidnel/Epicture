
using Epicture.Pages;
using Epicture.Src;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace Epicture
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        Profile Current_User;

        public MainPage()
        {
            this.InitializeComponent();
            this.Current_User = new Profile(Login.Text,string.Empty);
        }

        public async Task<bool> isFilePresent(string fileName)
        {
            try
            {
                var item = await ApplicationData.Current.LocalFolder.TryGetItemAsync(fileName);
                return item != null;
            } catch(Exception e)
            {
                return false;
            }
        }

        private async Task<Profile> creatConfig(Epicture.Src.Profile user)
        {
            Windows.Storage.StorageFolder storageFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
            Windows.Storage.StorageFolder folder = ApplicationData.Current.LocalFolder; ;
            Windows.Storage.StorageFile sampleFile;
            string path_file = @"Config_" + user.username + ".Json";

            sampleFile = await folder.CreateFileAsync(path_file, Windows.Storage.CreationCollisionOption.ReplaceExisting);
            await Windows.Storage.FileIO.WriteTextAsync(sampleFile, JsonConvert.SerializeObject(user));

            return user;           
        }

        private async Task<Profile> GetUser(string username)
        {
            Windows.Storage.StorageFolder storageFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
            Windows.Storage.StorageFolder folder = ApplicationData.Current.LocalFolder; ;
            Windows.Storage.StorageFile sample;
            string path_file = @"config_" + username + ".json";

            var presence = await isFilePresent(path_file);
            if (presence)
            {
                sample = await storageFolder.GetFileAsync(path_file);
                string text = await Windows.Storage.FileIO.ReadTextAsync(sample);
                this.Login.Text = text;
                return JsonConvert.DeserializeObject<Profile>(text);
            }
            this.Login.Text = "NULL";
            return null;
        }

        private async void PrononceSentence(string text_to_prononce)
        {
            MediaElement mediaElement = new MediaElement();
            var synth = new Windows.Media.SpeechSynthesis.SpeechSynthesizer();

            if (text_to_prononce == string.Empty)
                return;
            Windows.Media.SpeechSynthesis.SpeechSynthesisStream stream = await synth.SynthesizeTextToStreamAsync(text_to_prononce);
            mediaElement.SetSource(stream, stream.ContentType);
            mediaElement.Play();
        }

        private async void ConnectionAsync(object sender, RoutedEventArgs e)
        {
            //Profile current_user = new Profile(this.Login.Text, string.Empty);

            var config = await GetUser(this.Login.Text);
            if (config == null)
            { 
                this.Link_profil.Visibility = Visibility.Visible;
            } else
            {
               PrononceSentence("Welcome, , to your new favorit app !");
               this.Frame.Navigate(typeof(Search));
            }
        }

        private async void HyperlinkButton_Click(object sender, RoutedEventArgs e)
        {
            Profile current_user = new Profile(this.Login.Text, string.Empty);

            var config = await creatConfig(current_user);
            this.Frame.Navigate(typeof(Create_Profil));
            //nav to inscription page
        }
    }
}
