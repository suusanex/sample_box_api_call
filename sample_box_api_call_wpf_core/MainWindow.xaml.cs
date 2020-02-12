using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Box.V2;
using Box.V2.Auth;
using Box.V2.Config;
using Microsoft.Extensions.Configuration;

namespace sample_box_api_call_wpf_core
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        internal class BindingSource : INotifyPropertyChanged
        {
            #region INotifyPropertyChanged実装 
            public event PropertyChangedEventHandler PropertyChanged;

            protected virtual void OnPropertyChanged(string propertyName)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
            #endregion

            internal BindingSource()
            {
            }

            string _ClientId;
            public string ClientId
            {
                get => _ClientId;
                set { _ClientId = value; OnPropertyChanged(nameof(ClientId)); }
            }

            //string _DUMMY;
            //public string DUMMY
            //{
            //    get => _DUMMY;
            //    set { _DUMMY = value; OnPropertyChanged(nameof(DUMMY)); }
            //}

        }

        internal BindingSource m_Bind;
        private Uri m_RedirectUri = new Uri("https://localhost");


        public MainWindow()
        {
            InitializeComponent();

            m_Bind = new BindingSource();

            DataContext = m_Bind;

        }

        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            var config = new AppConfig();
            m_Bind.ClientId = config.Config.GetSection("ClientAuthSettings")["ClientId"];
            ClientSecretBox.Password = config.Config.GetSection("ClientAuthSettings")["ClientSecret"];

            Browser.Navigating += BrowserOnNavigating;
        }


        private void OnBtnGetToken(object sender, RoutedEventArgs e)
        {
            var clientId = m_Bind.ClientId;
            var clientSecret = ClientSecretBox.Password;

            var config = new BoxConfig(clientId, clientSecret, m_RedirectUri);
            var client = new BoxClient(config);

            //var authUrl = "https://account.box.com/api/oauth2/authorize";


            Browser.Navigate(config.AuthCodeUri);

        }

        private void BrowserOnNavigating(object sender, NavigatingCancelEventArgs e)
        {
            if (e.Uri.Host.Equals(m_RedirectUri.Host))
            {

                //注：コピペ
                // grab access_token and oauth_verifier
                IDictionary<string, string> keyDictionary = new Dictionary<string, string>();
                var qSplit = e.Uri.Query.Split('?');
                foreach (var kvp in qSplit[qSplit.Length - 1].Split('&'))
                {
                    var kvpSplit = kvp.Split('=');
                    if (kvpSplit.Length == 2)
                    {
                        keyDictionary.Add(kvpSplit[0], kvpSplit[1]);
                    }
                }

                AuthCode = keyDictionary["code"];

                e.Cancel = true;
                Dispatcher.Invoke(() => Browser.Visibility = Visibility.Collapsed);

                MessageBox.Show(AuthCode);


                //if (AuthCodeReceived != null)
                //{
                //    AuthCodeReceived(this, new EventArgs() { });
                //    oauthBrowser.Visibility = Visibility.Collapsed;
                //}
            }
        }

        public string AuthCode { get; private set; }

    }
}
