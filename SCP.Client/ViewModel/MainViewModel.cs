using System;
using GalaSoft.MvvmLight;

namespace SCP.Client.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel(ConfigurationViewModel confViewModel, EvaluateChildViewModel evaluateChildViewModel)
        {
            ////if (IsInDesignMode)
            ////{
            ////    // Code runs in Blend --> create design time data.
            ////}
            ////else
            ////{
            ////    // Code runs "for real"
            ////}

            this.ConfigurationViewModel = confViewModel;
            this.ConfigurationViewModel.PropertyChanged += ConfigurationViewModel_PropertyChanged;

            this.EvaluateChildViewModel = evaluateChildViewModel;

            SetApiReferenceToViewModels();
        }

        private void ConfigurationViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            SetApiReferenceToViewModels();
            RaisePropertyChanged(nameof(ConfigurationViewModel));
        }

        private void SetApiReferenceToViewModels()
        {
            this.EvaluateChildViewModel.ApiUrl = this.ConfigurationViewModel.ApiUrl;
            this.EvaluateChildViewModel.ApiKey = this.ConfigurationViewModel.ApiKey;
        }

        private ConfigurationViewModel _ConfigurationViewModel;
        public ConfigurationViewModel ConfigurationViewModel
        {
            get => _ConfigurationViewModel;
            set => Set(ref _ConfigurationViewModel, value);
        }

        private EvaluateChildViewModel _EvaluateChildViewModel;
        public EvaluateChildViewModel EvaluateChildViewModel
        {
            get => _EvaluateChildViewModel;
            set => Set(ref _EvaluateChildViewModel, value);
        }
    }
}