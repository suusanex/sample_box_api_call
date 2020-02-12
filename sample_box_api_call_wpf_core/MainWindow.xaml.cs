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


        public MainWindow()
        {
            InitializeComponent();

            m_Bind = new BindingSource();

            DataContext = m_Bind;

        }

        private void OnBtnGetToken(object sender, RoutedEventArgs e)
        {
            var clientId = m_Bind.ClientId;
            var clientSecret = ClientSecretBox.Password;

            var config = new BoxConfig(clientId, clientSecret, new Uri("http://localhost"));
            var client = new BoxClient(config);
            

        }

        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            var config = new AppConfig();
            m_Bind.ClientId = config.Config.GetSection("ClientAuthSettings")["ClientId"];
            ClientSecretBox.Password = config.Config.GetSection("ClientAuthSettings")["ClientSecret"];

        }
    }
}
