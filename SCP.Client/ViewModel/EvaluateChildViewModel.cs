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

        private string _ChildFirstName;
        public string ChildFirstName
        {
            get => _ChildFirstName;
            set => Set(ref _ChildFirstName, value);
        }

        private string _ChildLastName;
        public string ChildLastName
        {
            get => _ChildLastName;
            set => Set(ref _ChildLastName, value);
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
            this.IsBusy = true;

            var userMessage = new UserMessage()
            {
                Title = "Search Children",
            };

            try
            {
                var apiResult = await this.apiClient.GetChildByIdAsync(this.ChildId);

                if (apiResult.Item1 == ApiClientResult.OK)
                {
                    this.ChildFirstName = apiResult.Item2?.ChildFirstName;
                    this.ChildLastName = apiResult.Item2?.ChildLastName;
                    this.Goodness = apiResult.Item2?.Goodness;
                    userMessage = null;
                }
                else
                {
                    userMessage.MessageType = UserMessage.Type.Error;

                    switch (apiResult.Item1)
                    {
                        case ApiClientResult.NotFound:
                            userMessage.Message = "Child not found!";
                            break;
                        case ApiClientResult.GenericError:
                            userMessage.Message = "An error occurs during search operation!";
                            break;
                    }
                }

            }
            catch (Exception ex)
            {
                this.ChildLastName = null;
                this.ChildFirstName = null;

                userMessage.MessageType = UserMessage.Type.Error;
                userMessage.Message = ex.Message;
            }

            if (userMessage != null)
                this.MessengerInstance.Send(userMessage);
            this.IsBusy = false;
        }

        private bool CanSubmitEvaluationExecute()
        {
            var result = !string.IsNullOrWhiteSpace(this.ChildId);
            result &= !string.IsNullOrWhiteSpace(this.ChildLastName);
            result &= !string.IsNullOrWhiteSpace(this.ChildFirstName);
            result &= this.Goodness.HasValue &&
                      (this.Goodness.Value >= 0 && this.Goodness.Value <= 10);

            return result;
        }

        private async void SubmitEvaluationExecuteAsync()
        {
            this.IsBusy = true;

            var userMessage = new UserMessage()
            {
                Title = "Children Evaluation",
            };


            try
            {
                var result = await this.apiClient.SubmitEvaluationAsync(this.ChildId, this.ChildFirstName, this.ChildLastName, this.Goodness.Value);
                if (result == ApiClientResult.OK)
                {
                    this.ChildId = null;
                    this.ChildFirstName = null;
                    this.ChildLastName = null;
                    this.Goodness = null;
                    userMessage.MessageType = UserMessage.Type.Info;
                    userMessage.Message = "Child evaluation submitted!";
                }
                else
                {
                    userMessage.MessageType = UserMessage.Type.Error;
                    switch (result)
                    {
                        case ApiClientResult.NotFound:
                            userMessage.Message = "Child not found!";
                            break;
                        case ApiClientResult.GenericError:
                            userMessage.Message = "An error occurs during child evaluation!";
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                userMessage.Message = ex.Message;
                userMessage.MessageType = UserMessage.Type.Error;
            }

            this.MessengerInstance.Send(userMessage);
            this.IsBusy = false;
        }
    }
}