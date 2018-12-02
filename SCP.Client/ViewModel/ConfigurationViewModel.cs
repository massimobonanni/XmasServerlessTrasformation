using GalaSoft.MvvmLight;
using SCP.Client.APIService;
using System.Configuration;

namespace SCP.Client.ViewModel
{
    public class ConfigurationViewModel : ViewModelBase
    {
        public ConfigurationViewModel()
        {
            ReadConfiguration();
        }

        private void ReadConfiguration()
        {
            this.ApiUrl = ConfigurationManager.AppSettings["ApiUrl"];
        }

        private string _ApiUrl;
        public string ApiUrl { get => _ApiUrl; set => Set(ref _ApiUrl, value); }

        private string _ApiKey;
        public string ApiKey { get => _ApiKey; set => Set(ref _ApiKey, value); }
    }
}