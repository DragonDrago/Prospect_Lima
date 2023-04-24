using Chat.Client.Presenter;
using Flurl.Http;
using Flurl.Http.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Chat.Client
{
    internal static class Program
    {
        public const string BASE_URL = "http://localhost:5012";
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            FlurlConfigure();

            LoginPresenter loginPresenter = new LoginPresenter(new LoginForm());
            loginPresenter.Run();
        }

        private static void FlurlConfigure()
        {
            FlurlHttp.Configure(settings => {
                var jsonSettings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    ObjectCreationHandling = ObjectCreationHandling.Replace,
                    
                };
                
                settings.JsonSerializer = new NewtonsoftJsonSerializer(jsonSettings);
            });
        }
    }
}
