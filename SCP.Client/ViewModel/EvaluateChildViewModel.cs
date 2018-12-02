using System;
using System.Threading.Tasks;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using SCP.Client.APIService;
using SCP.Core.DTO;

namespace SCP.Client.ViewModel
{
    public class EvaluateChildViewModel : ViewModelBase
    {
        public EvaluateChildViewModel(IApiClient apiClient)
        {
            this.apiClient = apiClient;
        }

        private readonly IApiClient apiClient;

        public string ApiUrl { get => apiClient.ApiUrl; set => apiClient.ApiUrl = value; }
        public string ApiKey { get => apiClient.ApiKey; set => apiClient.ApiKey = value; }

        private RelayCommand _SearchChildCommand;

        public RelayCommand SearchChildCommand
        {
            get
            {
                if (_SearchChildCommand == null)
                {
                    _SearchChildCommand = new RelayCommand(SearchChildExecuteAsync, CanSearchChildExecute);
                }

                return _SearchChildCommand;
            }
        }

        private string _ChildId;
        public string ChildId
        {
            get => _ChildId;
            set
            {
                Set(ref _ChildId, value);
                this.SearchChildCommand.RaiseCanExecuteChanged();
            }
        }

        private ChildDto _Child;
        public ChildDto Child
        {
            get => _Child;
            set => Set(ref _Child, value);
        }

        private bool CanSearchChildExecute()
        {
            return !string.IsNullOrWhiteSpace(this.ChildId);
        }

        private async void SearchChildExecuteAsync()
        {
            this.ErrorMessage = null;
            this.IsBusy = true;
            try
            {
                this.Child = await this.apiClient.GetChildByIdAsync(this.ChildId);
            }
            catch (Exception ex)
            {
                this.Child = null;
                this.ErrorMessage = ex.Message;
            }
            this.IsBusy = false;
        }
    }
}