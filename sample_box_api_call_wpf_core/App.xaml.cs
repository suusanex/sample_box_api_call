using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace sample_box_api_call_wpf_core
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static string AppRunFolderPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        private void App_OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBox.Show(e.Exception?.ToString());

            //動作確認用なので、例外が発生した場合でも終了せずにとりあえず処理続行する
            e.Handled = true;
        }
    }
}
