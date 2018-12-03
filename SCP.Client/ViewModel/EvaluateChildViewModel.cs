using System;
using System.Threading.Tasks;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using SCP.Client.APIService;
using SCP.Client.Messages;
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


        private RelayCommand _SubmitEvaluationCommand;

        public RelayCommand SubmitEvaluationCommand
        {
            get
            {
                if (_SubmitEvaluationCommand == null)
                {
                    _SubmitEvaluationCommand = new RelayCommand(SubmitEvaluationExecuteAsync, CanSubmitEvaluationExecute);
                }

                return _SubmitEvaluationCommand;
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
                this.SubmitEvaluationCommand.RaiseCanExecuteChanged();
            }
        }

        private ChildDto _Child;
        public ChildDto Child
        {
            get => _Child;
            set => Set(ref _Child, value);
        }

        private int? _Goodness;
        public int? Goodness
        {
            get => _Goodness;
            set
            {
                Set(ref _Goodness, value);
                this.SubmitEvaluationCommand.RaiseCanExecuteChanged();
            }
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
                var apiResult = await this.apiClient.GetChildByIdAsync(this.ChildId);

                if (apiResult.Item1 == ApiClientResult.OK)
                {
                    this.Child = apiResult.Item2;
                    this.Goodness = this.Child.Goodness;
                }
                else
                {
                    var userMessage = new UserMessage()
                    {
                        Title = "Search Children",
                        MessageType = UserMessage.Type.Error
                    };

                    switch (apiResult.Item1)
                    {
                        case ApiClientResult.NotFound:
                            userMessage.Message = "Child not found!";
                            break;
                        case ApiClientResult.GenericError:
                            userMessage.Message = "An error occurs during search operation!";
                            break;
                    }

                    this.MessengerInstance.Send(userMessage);
                }
            }
            catch (Exception ex)
            {
                this.Child = null;
                this.ErrorMessage = ex.Message;
            }
            this.IsBusy = false;
        }

        private bool CanSubmitEvaluationExecute()
        {
            var result = !string.IsNullOrWhiteSpace(this.ChildId);
            result &= this.Goodness.HasValue &&
                      (this.Goodness.Value >= 0 && this.Goodness.Value <= 10);

            return result;
        }

        private async void SubmitEvaluationExecuteAsync()
        {
            this.ErrorMessage = null;
            this.IsBusy = true;
            try
            {
                var result = await this.apiClient.SubmitEvaluationAsync(this.ChildId, this.Goodness.Value);
                if (result==ApiClientResult.OK)
                {
                    this.ChildId = null;
                    this.Child = null;
                    this.Goodness = null;
                }
                else
                {
                    var userMessage = new UserMessage()
                    {
                        Title = "Children Evaluation",
                        MessageType = UserMessage.Type.Error
                    };

                    switch (result)
                    {
                        case ApiClientResult.NotFound:
                            userMessage.Message = "Child not found!";
                            break;
                        case ApiClientResult.GenericError:
                            userMessage.Message = "An error occurs during child evaluation!";
                            break;
                    }

                    this.MessengerInstance.Send(userMessage);
                }
            }
            catch (Exception ex)
            {
                this.ErrorMessage = ex.Message;
            }
            this.IsBusy = false;
        }
    }
}